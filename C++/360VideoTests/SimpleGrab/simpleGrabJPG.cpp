//=============================================================================
// Copyright (c) 2001-2018 FLIR Systems, Inc. All Rights Reserved.
//
// This software is the confidential and proprietary information of FLIR
// Integrated Imaging Solutions, Inc. ("Confidential Information"). You
// shall not disclose such Confidential Information and shall use it only in
// accordance with the terms of the license agreement you entered into
// with FLIR Integrated Imaging Solutions, Inc. (FLIR).
//
// FLIR MAKES NO REPRESENTATIONS OR WARRANTIES ABOUT THE SUITABILITY OF THE
// SOFTWARE, EITHER EXPRESSED OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE, OR NON-INFRINGEMENT. FLIR SHALL NOT BE LIABLE FOR ANY DAMAGES
// SUFFERED BY LICENSEE AS A RESULT OF USING, MODIFYING OR DISTRIBUTING
// THIS SOFTWARE OR ITS DERIVATIVES.
//=============================================================================
//
//=============================================================================
// This example illustrates the simplest procedure of acquiring an image from a
// Ladybug camera.
//
// This program does:
//  - create a context
//  - initialize a camera
//  - start transmission of images
//  - grab an image
//  - color process the grabbed image
//  - save the 6 raw images in BMP files
//  - destroy the context
//
//=============================================================================

#include <stdio.h>
#include <stdlib.h>
#include "ladybug.h"
#include "ladybugstream.h"
#include <string>

#ifdef _WIN32

#include <windows.h>
#include <shlobj.h>

#else

#include <unistd.h>
#include <sys/types.h>
#include <pwd.h>

#endif

#define _HANDLE_ERROR                                 \
    if (error != LADYBUG_OK)                          \
    {                                                 \
        printf(                                       \
            "Error: Ladybug library reported - %s\n", \
            ::ladybugErrorToString(error));           \
        return EXIT_FAILURE;                          \
    }

namespace
{
std::string getWriteableDirectory()
{
    std::string writeableDirectory;
#ifdef _WIN32
    char buf[_MAX_PATH];
    HRESULT res = SHGetFolderPath(NULL, CSIDL_PERSONAL, NULL, 0, buf);
    writeableDirectory.append(buf);
    if (res == S_OK)
    {
        writeableDirectory.append("\\");
    }
#else
    const char *homedir;

    if ((homedir = getenv("HOME")) == NULL)
    {
        uid_t uid = getuid();
        struct passwd *pw = getpwuid(uid);

        if (pw == NULL)
        {
            homedir = NULL;
        }
        else
        {
            homedir = pw->pw_dir;
        }
    }
    if (homedir != NULL)
    {
        writeableDirectory.append(homedir).append("/");
    }
#endif

    return writeableDirectory;
}

} // namespace

int main(int /* argc */, char ** /* argv[] */)
{
    // Initialize context.
    LadybugContext context;
    LadybugError error = ::ladybugCreateContext(&context);
    _HANDLE_ERROR;

    // Initialize the first ladybug on the bus.
    printf("Initializing...\n");
    error = ::ladybugInitializeFromIndex(context, 0);
    _HANDLE_ERROR;

    // Get camera info
    LadybugCameraInfo caminfo;
    error = ladybugGetCameraInfo(context, &caminfo);
    //_HANDLE_ERROR("ladybugGetCameraInfo()");

    // Start up the camera according to device type and data format
    printf("Starting %s (%u)...\n", caminfo.pszModelName, caminfo.serialHead);

    error = ::ladybugStart(context, LADYBUG_DATAFORMAT_RAW8);
    _HANDLE_ERROR;

    // Set color processing method
    printf("Setting debayering method...\n");
    error = ::ladybugSetColorProcessingMethod(context, LADYBUG_NEAREST_NEIGHBOR_FAST);
    _HANDLE_ERROR;

    for (int frames = 0; frames < 10; frames++)
    {

        // Grab a single image.
        printf("Grabbing image\n");
        error = LADYBUG_FAILED;
        LadybugImage image;

        for (int i = 0; i < 10 && error != LADYBUG_OK; i++)
        {
            printf(".");
            error = ::ladybugGrabImage(context, &image);
        }
        printf("\n");
        _HANDLE_ERROR;

        // Allocate memory for the 6 processed images
        unsigned char *arpBuffers[LADYBUG_NUM_CAMERAS] = {0};
        for (unsigned int uiCamera = 0; uiCamera < LADYBUG_NUM_CAMERAS; uiCamera++)
        {
            arpBuffers[uiCamera] = new unsigned char[image.uiRows * image.uiCols * 4];

            // Initialize the entire buffer so that the alpha channel has a valid (maximum) value.
            memset(arpBuffers[uiCamera], 0xff, image.uiRows * image.uiCols * 4);
        }

        // Color-process the image
        printf("Converting image...\n");
        error = ::ladybugConvertImage(context, &image, arpBuffers, LADYBUG_BGRU);

        // Save the image as 6 individual raw (unstitched, distorted) images
        printf("Saving images...\n");
        for (unsigned int uiCamera = 0; uiCamera < LADYBUG_NUM_CAMERAS; uiCamera++)
        {
            LadybugProcessedImage processedImage;
            processedImage.pData = arpBuffers[uiCamera];
            processedImage.pixelFormat = LADYBUG_BGRU;
            processedImage.uiCols = image.uiCols;
            processedImage.uiRows = image.uiRows;

            char pszOutputFilePath[256] = {0};
            sprintf(pszOutputFilePath, "ladybug_frame%03u_%u_camera_%02u.jpg", frames,caminfo.serialHead, uiCamera);
            const std::string outputPath = getWriteableDirectory() + std::string(pszOutputFilePath);

            error = ::ladybugSaveImage(context, &processedImage, outputPath.c_str(), LADYBUG_FILEFORMAT_JPG);
            _HANDLE_ERROR;

            printf("Saved camera %u image to %s.\n", uiCamera, outputPath.c_str());
        }

        // Clean up the buffers
        for (unsigned int uiCamera = 0; uiCamera < LADYBUG_NUM_CAMERAS; uiCamera++)
        {
            delete arpBuffers[uiCamera];
        }
    }

    // Destroy the context
    printf("Destroying context...\n");
    error = ::ladybugDestroyContext(&context);
    _HANDLE_ERROR;

    printf("Done.\n");

    return 0;
}
