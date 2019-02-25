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

//=============================================================================
//
// ladybugProcessStream.cpp
// 
// This example shows users how to process entire or part of a stream file.
// The program processes each frame and outputs an image file sequentially.
// If the stream file contains GPS information, the program outputs the 
// information to a separate text file.
//
// This example reads processing parametere options from command line.
// Use -? or -h option to display the usage help.
//
//===============================================================


//=============================================================================
// System Includes
//=============================================================================
#include <stdio.h>
#include <string.h>
#include <stdlib.h>

//=============================================================================
// PGR Includes
//=============================================================================
#include <ladybug.h>
#include <ladybugrenderer.h>
#include <ladybuggeom.h>
#include <ladybugstream.h>
#include <ladybugGPS.h>
#include <ladybugvideo.h>
#include "getopt.h"

//=============================================================================
// Platform specific indludes and definitions
//=============================================================================
#ifdef _WIN32

#include <conio.h>

#else

#include <strings.h>
#define _MAX_PATH 4096

#endif

//=============================================================================
// Global variables
//=============================================================================
unsigned int iFrameFrom = 0;
unsigned int iFrameTo = 0;
char pszInputStream[ _MAX_PATH ] = "ladybug-000000.pgr";
char pszOutputFilePrefix[ _MAX_PATH ] = "ladybugImageOutput";
char pszOutputGPSPrefix[ _MAX_PATH ] = "ladybugGPSOutput";
char pszConfigFile[ _MAX_PATH] = "";
int iOutputImageWidth = 2048;
int iOutputImageHeight = 1024;
LadybugOutputImage outputImageType = LADYBUG_PANORAMIC;
LadybugSaveFileFormat outputImageFormat = LADYBUG_FILEFORMAT_JPG;
LadybugColorProcessingMethod colorProcessingMethod = LADYBUG_HQLINEAR;
int iBlendingWidth = 100;
float fFalloffCorrectionValue = 1.0f;
bool bFalloffCorrectionFlagOn = false;
bool bEnableAntiAliasing = false;
bool bEnableSoftwareRendering = false;
bool bEnableStabilization = false;
LadybugStabilizationParams stabilizationParams = { 6, 100, 0.95 };
LadybugContext context;
LadybugStreamContext readContext;
LadybugStreamHeadInfo streamHeaderInfo;
unsigned int iTextureWidth, iTextureHeight;
unsigned char* arpTextureBuffers[ LADYBUG_NUM_CAMERAS]= { NULL, NULL, NULL, NULL, NULL, NULL };
float fFOV = 60.0f;
float fRotX = 0.0f;
float fRotY = 0.0f;
float fRotZ = 0.0f;
int iBitRate = 4000; // in kbps
bool processH264 = false;

//=============================================================================
// Macro Definitions
//=============================================================================
#define _CHECK_ERROR \
    if( error != LADYBUG_OK ) \
{ \
    return error; \
} \

#define _ON_ERROR_BREAK \
    if( error != LADYBUG_OK ) \
{ \
    printf( "Error! Ladybug library reported %s\n", \
    ::ladybugErrorToString( error ) ); \
    break; \
} \

#define _ON_ERROR_CONTINUE \
    if( error != LADYBUG_OK ) \
{ \
    printf( "Error! Ladybug library reported %s\n", \
    ::ladybugErrorToString( error ) ); \
    continue; \
} \

#define _ON_ERROR_EXIT \
    if( error != LADYBUG_OK ) \
{ \
    printf( "Error! Ladybug library reported %s\n", \
    ::ladybugErrorToString( error ) ); \
    cleanupLadybug(); \
    return 0; \
} \

//=============================================================================
// Function Definitions
//=============================================================================


void
display_Usage( const char* pszProgramName )
{
    printf( "Usage: \n\n" );

    printf( "%s [OPTIONS]\n\n", pszProgramName );

    printf( 
        "OPTIONS\n\n"
        "  -i STREAM_PATH     The PGR stream file to process with an extension of .pgr\n"
        "  -r NNN-NNN         The frame range to process. The first frame is 0.\n"
        "                     Default setting is to process all the images.\n"
        "  -o OUTPUT_PATH     Output file prefix. \n"
        "                     Default is %s\n"
        "  -g GPS_OUTPUT_PATH Output GPS file prefix. \n"
        "                     Default is %s\n"
        "  -w NNNNxNNNN       Output image size (widthxheight) in pixel. \n"
        "                     Default is %dx%d.\n"
        "  -t RENDER_TYPE     Output image rendering type:\n"
        "              pano      - panoramic view (default)\n"   
        "              dome      - dome view\n"
        "              spherical - spherical view\n"
        "              rectify-0 - rectified image (camera 0)\n"
        "              rectify-1 - rectified image (camera 1)\n"
        "              rectify-2 - rectified image (camera 2)\n"
        "              rectify-3 - rectified image (camera 3)\n"
        "              rectify-4 - rectified image (camera 4)\n"
        "              rectify-5 - rectified image (camera 5)\n"
        "  -f FORMAT Output image format:\n"
        "              bmp      - Windows BMP image\n"
        "              jpg      - JPEG image (default)\n"
        "              tiff     - TIFF image\n"
        "              png      - PNG image\n"
        "              h264     - H.264 video\n"
        "  -c COLOR_PROCESS Debayering method:\n"
        "              hq       - High quality linear method (default)\n" 
        "              hq-gpu   - High quality linear method\n" 
        "              edge     - Edge sensing method\n"
        "              near     - Nearest neighbor method\n"
        "              near-f   - Nearest neighbor(fast) method\n"
        "              rigorous - Rigorous method\n"
        "              down4    - Downsample method\n"
        "              mono     - Monochrome method\n"
        "              df       - Directional filter method\n"
        "              wdf       - Weighted Directional filter method\n"
        "  -b NNN   Blending width in pixel. Default is %d.\n"
        "  -v X.XX  Falloff correction value. Default is %f.\n"
        "  -a true/false   Enable falloff correction. \n"
        "              true - Enable.\n"
        "              false - Disable.\n"
        "              Default is %s.\n"
        "  -s true/false   Enable software rendering.\n"
        "              true - Enable.\n"
        "              false - Disable.\n"
        "              Default is %s.\n"
        "  -k true/false   Enable anti-aliasing.\n"
        "              true - Enable.\n"
        "              false - Disable.\n"
        "              Default is %s.\n"
        "  -z true/false   Enable stabilization.\n"
        "              true - Enable.\n"
        "              false - Disable.\n"
        "              Default is %s.\n"
        "  -n N        Stabilization paramnter - Number of templates. Default is %d.\n"
        "  -m NNN      Stabilization paramnter - Maximum search region. Default is %d.\n"
        "  -d X.XX     Stabilization paramnter - Decay rate. Default is %f.\n"
        "  -q XXX      Field of view in degrees when RENDER_TYPE is \"spherical\". Default is %f.\n"
        "  -x XXX-YYY-ZZZ   Euler rotation angle in degrees when RENDER_TYPE is \"spherical\". Default is %f-%f-%f.\n"
        "  -l CAL_FILE_PATH Path to calibration file to replace.\n"
        "  -e BITRATE  Bitrate in kbps for H.264 video output. Default is %d.\n"
        "\n", 
        pszOutputFilePrefix, pszOutputGPSPrefix,
        iOutputImageWidth, iOutputImageHeight,
        iBlendingWidth, fFalloffCorrectionValue,
        bFalloffCorrectionFlagOn?"true":"false",
        bEnableSoftwareRendering?"true":"false",
        bEnableAntiAliasing?"true":"false",
        bEnableStabilization?"true":"false",
        stabilizationParams.iNumTemplates, 
        stabilizationParams.iMaximumSearchRegion, 
        stabilizationParams.dDecayRate,
        fFOV,
        fRotX,
        fRotY,
        fRotZ,
        iBitRate
        );

    printf( 
        "EXAMPLES\n\n"

        "  %s -i lb-000000.pgr\n\n"
        "        Process all the images in the stream files named lb-000000.pgr,\n"
        "        lb-000001.pgr, lb-000002.pgr,... \n"
        "        Use all the default settings.\n\n\n" 

        "  %s -i c:\\tmp\\lb-000000.pgr -r 11-20 -o c:\\tmp\\Processed -t pano -w 5400x2700 -f jpg -c near-f \n\n"
        "        Read stream file c:\\lb-000000.pgr and render 10 frames of \n"
        "        panoramic image starting from frame 11.\n"
        "        The raw image is processed with nearest neighbor(fast) method.\n"
        "        The rendered panoramic images are writted as JPEG files: \n"
        "           c:\\tmp\\Processed_000001.jpg, \n"
        "           c:\\tmp\\Processed_000002.jpg, ...\n"
        "           ...\n"
        "           c:\\tmp\\Processed_000010.jpg, ...\n"
        "        The rendered panoramic image size is 5400x2700.\n\n\n"

        "  %s -i lb-000000.pgr -t rectify-0 -o Rectified -w 1616x1232 -f bmp -c hq \n\n"
        "        Render the rectified image of camera 0 in stream file lb-000000.pgr.\n"
        "        The raw image is processed with high quality linear method.\n"
        "        Output the rectified images to Rectified_000000.bmp, \n"
        "        Rectified_000001.bmp... \n\n\n"

        "  %s -i lb-000000.pgr -t pano -b 80 -s true \n\n"
        "        Render panoramic images with blending width 80.\n"
        "        Use software rendering, where the image rendering process is not hardware\n" 
        "        accelerated regardless of the existence of the graphics card.\n\n\n"
        ,
        pszProgramName,
        pszProgramName,
        pszProgramName,
        pszProgramName
        );

    printf( "\n" );
}


bool isHighBitDepth( LadybugDataFormat format)
{
    return (format == LADYBUG_DATAFORMAT_RAW12 ||
        format == LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW12 ||
        format == LADYBUG_DATAFORMAT_COLOR_SEP_JPEG12 ||
        format == LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG12 ||
		format == LADYBUG_DATAFORMAT_RAW16 ||
        format == LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW16);
}

int strncmpCaseInsensitive(const char* str1, const char* str2, int num)
{
#ifdef _WIN32
    return _strnicmp(str1, str2, num);
#else
    return strncasecmp(str1, str2, num);
#endif
}

LadybugError
initializeLadybug( void )
{
    LadybugError error;
    LadybugImage image;
    char pszTempPath[_MAX_PATH] = { 0 };
    //
    // Create contexts and prepare stream for reading
    //
    error = ladybugCreateContext( &context);
    _CHECK_ERROR;

    error = ladybugCreateStreamContext( &readContext);
    _CHECK_ERROR;

    error = ladybugInitializeStreamForReading( readContext, pszInputStream, true );
    _CHECK_ERROR;

    // Is configuration file specified by the command line option?
    if ( strlen( pszConfigFile) == 0) {
#if _MSC_VER >= 1400 // Is this newer than Visual C++ 8.0?
        char* tempFile = ::_tempnam(NULL, NULL);
        if (tempFile == NULL)
        {
            printf( "Error creating temporary file name.\n");
            return LADYBUG_FAILED;
        }
        else
        {
            strncpy(pszTempPath, tempFile, _MAX_PATH);
            free(tempFile);
        }

#else
        strcpy( pszTempPath, "temp.cal");
#endif

        error = ladybugGetStreamConfigFile( readContext , pszTempPath );
        _CHECK_ERROR;

        strncpy(pszConfigFile, pszTempPath, strlen( pszTempPath) );
    }

    //
    // Load configuration file
    //
    error = ladybugLoadConfig( context, pszConfigFile );
    _CHECK_ERROR;

    if (pszTempPath != NULL)
    {
        // Remove the temporary configuration file 
        remove ( pszTempPath );
    }

    //
    // Get and display the the stream information
    //
    error = ladybugGetStreamHeader( readContext, &streamHeaderInfo );
    _CHECK_ERROR;

    const float frameRateToUse = streamHeaderInfo.ulLadybugStreamVersion < 7 ? (float)streamHeaderInfo.ulFrameRate : streamHeaderInfo.frameRate;

    printf( "--- Stream Information ---\n");
    printf( "Stream version : %d\n", streamHeaderInfo.ulLadybugStreamVersion);
    printf( "Base S/N: %d\n", streamHeaderInfo.serialBase);
    printf( "Head S/N: %d\n", streamHeaderInfo.serialHead);
    printf( "Frame rate : %3.2f\n", frameRateToUse);
    printf( "--------------------------\n");

    //
    // Set color processing method.
    //
    printf("Setting debayering method...\n" );
    error = ladybugSetColorProcessingMethod( context, colorProcessingMethod);     
    _CHECK_ERROR;

    // 
    // Set falloff correction value and flag
    //
    error = ladybugSetFalloffCorrectionAttenuation( context, fFalloffCorrectionValue );
    _CHECK_ERROR;
    error = ladybugSetFalloffCorrectionFlag( context, bFalloffCorrectionFlagOn );
    _CHECK_ERROR;

    //
    // read one image from the stream
    //
    error = ladybugReadImageFromStream( readContext, &image);
    _CHECK_ERROR;

    //
    // Allocate the texture buffers that hold the color-processed images for all cameras
    //
    if ( colorProcessingMethod == LADYBUG_DOWNSAMPLE4 || colorProcessingMethod == LADYBUG_MONO)
    {
        iTextureWidth = image.uiCols / 2;
        iTextureHeight = image.uiRows / 2;
    }
    else
    {
        iTextureWidth = image.uiCols;
        iTextureHeight = image.uiRows;
    }

	const unsigned int outputBytesPerPixel = isHighBitDepth(streamHeaderInfo.dataFormat) ? 2 : 1;
    for( int i = 0; i < LADYBUG_NUM_CAMERAS; i++)
    {
		arpTextureBuffers[ i ] = new unsigned char[ iTextureWidth * iTextureHeight * 4 * outputBytesPerPixel];
    }

    //
    // Set blending width
    //
    error = ladybugSetBlendingParams( context, iBlendingWidth );
    _CHECK_ERROR;

    //
    // Initialize alpha mask size - this can take a long time if the
    // masks are not present in the current directory.
    //
    printf( "Initializing alpha masks (this may take some time)...\n" );
    error = ladybugInitializeAlphaMasks( context, iTextureWidth, iTextureHeight );
    _CHECK_ERROR;

    // 
    // Make the rendering engine use the alpha mask
    //
    error = ladybugSetAlphaMasking( context, true );
    _CHECK_ERROR;

    //
    // Enable image sampling anti-aliasing
    //
    if ( bEnableAntiAliasing )
    {
        error = ladybugSetAntiAliasing( context, true );
        _CHECK_ERROR;
    }

    //
    // Use ladybugEnableSoftwareRendering() to enable 
    // Ladybug library to render the off-screen image using a bitmap buffer 
    // in system memory. The image rendering process will not be hardware 
    // accelerated.
    //
    if ( bEnableSoftwareRendering )
    {
        error = ladybugEnableSoftwareRendering( context, true );
        _CHECK_ERROR;
    }

    if ( bEnableStabilization )
    {
        error = ladybugEnableImageStabilization( 
            context, bEnableStabilization, &stabilizationParams);
        _CHECK_ERROR;
    }

    //
    // Configure output images in Ladybug liabrary
    //
    printf( "Configure output images in Ladybug library...\n" );
    error = ladybugConfigureOutputImages( 
        context, 
        outputImageType );
    _CHECK_ERROR;

    printf("Set off-screen panoramic image size:%dx%d image.\n", iOutputImageWidth, iOutputImageHeight );
    error = ladybugSetOffScreenImageSize(
        context,
        outputImageType,  
        iOutputImageWidth, 
        iOutputImageHeight );  
    _CHECK_ERROR;

    error = ladybugSetSphericalViewParams(
        context,
        fFOV,
        fRotX * 3.14159265f / 180.0f,
        fRotY * 3.14159265f / 180.0f,
        fRotZ * 3.14159265f / 180.0f,
        0.0f,
        0.0f,
        0.0f);
    _CHECK_ERROR;


    return LADYBUG_OK;
}

bool
cleanupLadybug( void )
{
    ladybugDestroyStreamContext( &readContext);
    ladybugDestroyContext( &context);
    for( int i = 0; i < LADYBUG_NUM_CAMERAS; i++)
    {
        if ( arpTextureBuffers[ i ] != NULL )
        {
            delete arpTextureBuffers[ i ];
            arpTextureBuffers[ i ] = NULL;
        }
    }
    return true;
}

void processArguments( int argc, char* argv[])
{
    const char* pszProgname = argv[ 0 ];
    int   iOpt;
    char* pszCurrParam;
    bool  bBadArgs = false;

    // no arguments?
    if( argc == 1 )
    {
        display_Usage( pszProgname );
        printf("<PRESS ENTER TO EXIT>");
        getchar();
        exit( 0);
    }

    while( ( iOpt = GetOption( argc, argv, "i:r:o:g:w:t:f:c:b:a:v:s:z:n:m:d:h:q:x:l:k:e:?", &pszCurrParam ) ) != 0 )
    {
        switch( iOpt )
        {
            //
            // Options
            //
        case 'i':
            if( sscanf( pszCurrParam, "%259s", pszInputStream ) != 1 )
            {
                bBadArgs = true;
            }
            break;
        case 'r':  // processing range:  MMM-NNN
            if( sscanf( pszCurrParam, "%u-%u", &iFrameFrom, &iFrameTo ) != 2 )
            {
                bBadArgs = true;
            }
            break;
        case 'o':
            if( sscanf( pszCurrParam, "%259s", pszOutputFilePrefix ) != 1 )
            {
                bBadArgs = true;
            }
            break;
        case 'g':
            if( sscanf( pszCurrParam, "%259s", pszOutputGPSPrefix ) != 1 )
            {
                bBadArgs = true;
            }
            break;
        case 'l':
            if( sscanf( pszCurrParam, "%259s", pszConfigFile ) != 1 )
            {
                bBadArgs = true;
            }
            break;
        case 'w': // rendering image size
            if( sscanf( pszCurrParam, "%dx%d", &iOutputImageWidth, &iOutputImageHeight ) != 2 )
            {
                bBadArgs = true;
            }
            break;
        case 't':
            if( strncmpCaseInsensitive( pszCurrParam, "pano", 4 ) == 0 )
            {
                outputImageType = LADYBUG_PANORAMIC;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "dome", 4 ) == 0 )
            {
                outputImageType = LADYBUG_DOME;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "spherical", 9 ) == 0 )
            {
                outputImageType = LADYBUG_SPHERICAL;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "rectify-0", 9 ) == 0 )
            {
                outputImageType = LADYBUG_RECTIFIED_CAM0;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "rectify-1", 9 ) == 0 )
            {
                outputImageType = LADYBUG_RECTIFIED_CAM1;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "rectify-2", 9 ) == 0 )
            {
                outputImageType = LADYBUG_RECTIFIED_CAM2;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "rectify-3", 9 ) == 0 )
            {
                outputImageType = LADYBUG_RECTIFIED_CAM3;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "rectify-4", 9 ) == 0 )
            {
                outputImageType = LADYBUG_RECTIFIED_CAM4;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "rectify-5", 9 ) == 0 )
            {
                outputImageType = LADYBUG_RECTIFIED_CAM5;
            }
            else
            {
                bBadArgs = true;
            }
            break;
        case 'f':
            if( strncmpCaseInsensitive( pszCurrParam, "bmp", 3 ) == 0 )
            {
                outputImageFormat = LADYBUG_FILEFORMAT_BMP;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "jpg", 3 ) == 0 )
            {
                outputImageFormat = LADYBUG_FILEFORMAT_JPG;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "tiff", 3 ) == 0 )
            {
                outputImageFormat = LADYBUG_FILEFORMAT_TIFF;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "png", 3 ) == 0 )
            {
                outputImageFormat = LADYBUG_FILEFORMAT_PNG;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "h264", 3 ) == 0 )
            {
                processH264 = true;
            }
            break;
        case 'c':
            if( strncmpCaseInsensitive( pszCurrParam, "edge", 4 ) == 0 )
            {
                colorProcessingMethod = LADYBUG_EDGE_SENSING;
            }
            if( strncmpCaseInsensitive( pszCurrParam, "near-f", 6 ) == 0 )
            {
                colorProcessingMethod = LADYBUG_NEAREST_NEIGHBOR_FAST;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "rigorous", 8 ) == 0 )
            {
                colorProcessingMethod = LADYBUG_RIGOROUS;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "df", 2 ) == 0 )
            {
                colorProcessingMethod = LADYBUG_DIRECTIONAL_FILTER;
            }
            else if (strncmpCaseInsensitive(pszCurrParam, "wdf", 3) == 0)
            {
                colorProcessingMethod = LADYBUG_WEIGHTED_DIRECTIONAL_FILTER;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "down4", 5 ) == 0 )
            {
                colorProcessingMethod = LADYBUG_DOWNSAMPLE4;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "mono", 4 ) == 0 )
            {
                colorProcessingMethod = LADYBUG_MONO;
            }
            else if (strncmpCaseInsensitive(pszCurrParam, "hq-gpu", 6) == 0)
            {
                colorProcessingMethod = LADYBUG_HQLINEAR_GPU;
            } 
            else if( strncmpCaseInsensitive( pszCurrParam, "hq", 2 ) == 0 )
            {
                colorProcessingMethod = LADYBUG_HQLINEAR;
            }
            else
            {
                bBadArgs = true;
            }
            break;
        case 'b':
            if( sscanf( pszCurrParam, "%d", &iBlendingWidth ) != 1 )
            {
                bBadArgs = true;
            }
            break;
        case 'a':
            if( strncmpCaseInsensitive( pszCurrParam, "true", 4 ) == 0 )
            {
                bFalloffCorrectionFlagOn = true;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "false", 5 ) == 0 )
            {
                bFalloffCorrectionFlagOn = false;
            }
            else
            {
                bBadArgs = true;
            }
            break;
        case 'v':
            if( sscanf( pszCurrParam, "%f", &fFalloffCorrectionValue ) != 1 )
            {
                bBadArgs = true;
            }
            break;
        case 's':
            if( strncmpCaseInsensitive( pszCurrParam, "true", 4 ) == 0 )
            {
                bEnableSoftwareRendering = true;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "false", 5 ) == 0 )
            {
                bEnableSoftwareRendering = false;
            }
            else
            {
                bBadArgs = true;
            }
            break;
        case 'z':
            if( strncmpCaseInsensitive( pszCurrParam, "true", 4 ) == 0 )
            {
                bEnableStabilization = true;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "false", 5 ) == 0 )
            {
                bEnableStabilization = false;
            }
            else
            {
                bBadArgs = true;
            }
            break;
        case 'n':
            if( sscanf( pszCurrParam, "%d", &(stabilizationParams.iNumTemplates) ) != 1 )
            {
                bBadArgs = true;
            }
            break;
        case 'm':
            if( sscanf( pszCurrParam, "%d", &(stabilizationParams.iMaximumSearchRegion) ) != 1 )
            {
                bBadArgs = true;
            }
            break;
        case 'd':
            {
                float fDecay = 0.0f;
                if( sscanf( pszCurrParam, "%f", &fDecay ) != 1 )
                    bBadArgs = true;
                else
                    stabilizationParams.dDecayRate = fDecay;
            }
            break;
        case 'q': // FOV for spherical view
            if( sscanf( pszCurrParam, "%f", &fFOV ) != 1 )
                bBadArgs = true;
            break;
        case 'x':  // rotation for spherical view (RotX-RotY-RotZ)
            if( sscanf( pszCurrParam, "%f-%f-%f", &fRotX, &fRotY, &fRotZ ) != 3 )
            {
                bBadArgs = true;
            }
            break;
        case 'e': // bitrate for H.264
            if( sscanf( pszCurrParam, "%d", &iBitRate ) != 1 )
                bBadArgs = true;
            break;
        case 'k':
            if( strncmpCaseInsensitive( pszCurrParam, "true", 4 ) == 0 )
            {
                bEnableAntiAliasing = true;
            }
            else if( strncmpCaseInsensitive( pszCurrParam, "false", 5 ) == 0 )
            {
                bEnableAntiAliasing = false;
            }
            else
            {
                bBadArgs = true;
            }
            break;
        case '?':
        case 'h':
        default:
            display_Usage( pszProgname );
            exit( 0);
        }
    }

    if( bBadArgs )
    {
        display_Usage( pszProgname );
        exit( 0);
    }
}

//=============================================================================
// Main Routine
//=============================================================================
int 
main( int argc, char* argv[] )
{
    LadybugError error;
    LadybugImage image;
    LadybugVideoContext videoContext;
    char videoPath[ 256];
    FILE *fp = NULL;

    processArguments( argc, argv);

    error = initializeLadybug();
    _ON_ERROR_EXIT;

    unsigned int totalFrames = 0;
    error = ladybugGetStreamNumOfImages( readContext, &totalFrames); 
    _ON_ERROR_EXIT;

    //
    // Check frame number range is valid
    //
    if ( iFrameTo == 0 )
    {
        //
        // Not specified in the command line argument.
        // Set it to the last image
        //
        iFrameTo = totalFrames - 1;
    }

    if ( ( iFrameFrom > totalFrames - 1) || 
        ( iFrameTo > totalFrames - 1) ||
        ( iFrameTo < iFrameFrom) )
    {
        printf( "Invalid frame number range.\n");
        cleanupLadybug();
        return 0;
    }

    if ( processH264)
    {
        LadybugH264Option h264Option;
        memset( &h264Option, 0, sizeof( h264Option));
        h264Option.bitrate = iBitRate * 1024;
        h264Option.frameRate = 15; // TODO - this should be configurable through options.
        h264Option.width = iOutputImageWidth;
        h264Option.height = iOutputImageHeight;

        sprintf( videoPath, "%s.mp4", pszOutputFilePrefix); 
        error = ladybugCreateVideoContext( &videoContext);
        _ON_ERROR_EXIT;
        error = ladybugOpenVideo( videoContext, videoPath, &h264Option);
        _ON_ERROR_EXIT;
    }

    //
    // fast-forward to the first frame to process in the stream
    //
    error = ladybugGoToImage( readContext, iFrameFrom); 
    _ON_ERROR_EXIT;

    //
    // process frames in the range
    //
    for ( unsigned int iFrame = iFrameFrom; iFrame <= iFrameTo; iFrame++)
    {
        printf( "Processing frame %u of %u\n", iFrame, iFrameTo);

        //
        // Read one frame from stream
        //
        error = ladybugReadImageFromStream( readContext, &image);
        _ON_ERROR_BREAK;

        //
        // Convert the image to BGRU format texture buffers
        //
		error = ladybugConvertImage( context, &image, arpTextureBuffers, isHighBitDepth(streamHeaderInfo.dataFormat) ? LADYBUG_BGRU16 : LADYBUG_BGRU);
        _ON_ERROR_CONTINUE;

        //
        // Update the textures on graphics card
        //
        error = ladybugUpdateTextures( 
            context, LADYBUG_NUM_CAMERAS, (const unsigned char**)arpTextureBuffers, isHighBitDepth(streamHeaderInfo.dataFormat) ? LADYBUG_BGRU16 : LADYBUG_BGRU);
        _ON_ERROR_BREAK;

        //
        // Output GPS information on text file if it exists in the image
        //
        LadybugNMEAGPGGA gpsData;
        error = ladybugGetGPSNMEADataFromImage( &image, "GPGGA", &gpsData);
        if ( error == LADYBUG_OK && gpsData.bValidData)
        {
            printf( "GPS INFO: LAT %lf, LONG %lf\n", gpsData.dGGALatitude, gpsData.dGGALongitude);
            if ( fp == NULL)
            {
                char pszGpsFilePath[ 256];
                sprintf( pszGpsFilePath, "%s%u_%u.txt", pszOutputGPSPrefix, iFrameFrom, iFrameTo);
                fp = fopen( pszGpsFilePath, "w");
            }
            if ( fp != NULL)
            {
                fprintf( fp, "%u, LAT %lf, LONG %lf\n", iFrame, gpsData.dGGALatitude, gpsData.dGGALongitude);
            }
        }

        //
        // Render and obtain the image in off-screen buffer
        //
        LadybugProcessedImage processedImage;
        error = ladybugRenderOffScreenImage(
            context, outputImageType, LADYBUG_BGR, &processedImage);
        _ON_ERROR_BREAK;

        //
        // Write the rendered image to a file
        //
        if ( processH264)
        {
            printf("Getting panoramic image (%u) and appending it to %s...\n", iFrame, videoPath);
            error = ladybugAppendVideoFrame( videoContext, &processedImage);
            _ON_ERROR_BREAK;
        }
        else
        {
            char pszOutputName[ 256];
            switch ( outputImageFormat ){
            case LADYBUG_FILEFORMAT_BMP: 
                sprintf( pszOutputName, "%s_%06u.bmp", pszOutputFilePrefix, iFrame); 
                break;
            case LADYBUG_FILEFORMAT_JPG: 
                sprintf( pszOutputName, "%s_%06u.jpg", pszOutputFilePrefix, iFrame); 
                break;
            case LADYBUG_FILEFORMAT_TIFF: 
                sprintf( pszOutputName, "%s_%06u.tiff", pszOutputFilePrefix, iFrame); 
                break;
            case LADYBUG_FILEFORMAT_PNG: 
                sprintf( pszOutputName, "%s_%06u.png", pszOutputFilePrefix, iFrame); 
                break;
            default: 
                sprintf( pszOutputName, "%s_%06u", pszOutputFilePrefix, iFrame);
            }
            printf("Getting panoramic image and writing it to %s...\n", pszOutputName);

            error = ladybugSaveImage( 
                context, &processedImage, pszOutputName, outputImageFormat, true);
            _ON_ERROR_BREAK;
        }
    }

    if ( fp != NULL )
    {
        fclose( fp);
    }

    if ( processH264)
    {
        error = ladybugCloseVideo( videoContext);
        error = ladybugDestroyVideoContext( &videoContext);
    }

    cleanupLadybug();

    return 0;
}