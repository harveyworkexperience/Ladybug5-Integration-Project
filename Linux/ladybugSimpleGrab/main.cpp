/*

Multithreaded application to test multithreaded Simple Grab Performance
with the Ladybug 5

Must be compiled with:  g++ -pthread -o <name> grabMultiThread.cpp

Kieran Hunt 21/01/2019

*/

#include <iostream>
#include <pthread.h>
#include <cstdlib>
#include <stdlib.h>
#include <stdio.h>
#include <string>
#include "ladybug.h"
#include "ladybugstream.h"

// networking headers
#include <netdb.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>



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

using namespace std;

#define NUM_THREADS 6

struct threadData
{
    int uiCamera;
        LadybugImage image;
        LadybugContext context;
        LadybugCameraInfo caminfo;
        int frames;
        int threadID;
}; 

void error(const char* msg) {
	perror(msg);
	exit(1);
}

void sendUdp (unsigned char *bytes, int length)
{
   	int sockfd;						// socket
	int portno = 10001;						// port to listen to
	socklen_t clientlen;			// byte size of client address
	struct sockaddr_in serveraddr;	// server's addr
	struct sockaddr_in clientaddr;	// client addr
	struct hostent* hostp;			// client host info
	// char buf[BUFSIZE];				// message receive buffer
	char *hostaddrp;				// dotted decimal host addr string
	int optval =1;						// flag value for setsockopt
	int n;							// message byte size

	if ((sockfd = socket(AF_INET, SOCK_DGRAM, 0)) < 0)
		error((const char*)"ERROR opening socket");

	setsockopt(sockfd, SOL_SOCKET, SO_REUSEADDR, 
		(const void *)&optval , sizeof(int));

	/*
	 * build the server's Internet address
	 */
	bzero((char *) &serveraddr, sizeof(serveraddr));
	serveraddr.sin_family = AF_INET;
	serveraddr.sin_addr.s_addr = htonl(INADDR_ANY);
	serveraddr.sin_port = htons((unsigned short)portno);

	/* 
	 * bind: associate the parent socket with a port 
	 */
	if (bind(sockfd, (struct sockaddr *) &serveraddr, 
		sizeof(serveraddr)) < 0) 
		error((const char*)"ERROR on binding");

		/*
		 * sendto: echo the input back to the client
		 */
    cout << "Message size " << length << " bytes\n";
	n = sendto(sockfd, bytes, length, 0,
		(struct sockaddr*)&clientaddr, clientlen);
	if (n < 0)
		error((const char*)"ERROR in sendto");

    cout << "UDP Packet sent \n";

}

void *saveSingleImage(void *threadID)
{
    struct threadData *myData;
    myData = (struct threadData *) threadID;
    
    // Allocate memory for the 6 processed images
    unsigned char *arpBuffers[LADYBUG_NUM_CAMERAS] = {0};
    for (unsigned int uiCamera = 0; uiCamera < LADYBUG_NUM_CAMERAS; uiCamera++)
    {
        arpBuffers[uiCamera] = new unsigned char[myData->image.uiRows * myData->image.uiCols * 4];

        // Initialize the entire buffer so that the alpha channel has a valid (maximum) value.
        memset(arpBuffers[uiCamera], 0xff, myData->image.uiRows * myData->image.uiCols * 4);
    }

    // Color-process the image
    printf("Converting image...\n");
    LadybugError error = ::ladybugConvertImage(myData->context, &myData->image, arpBuffers, LADYBUG_BGRU);


    // TODO SEND UDP HERE
    // arpBuffers[uiCamera]
    for (int frame = 0; frame < LADYBUG_NUM_CAMERAS; frame ++)
    {
        sendUdp (arpBuffers[frame], myData->image.uiRows * myData->image.uiCols * 4);
    }

    // Save the image as 6 individual raw (unstitched, distorted) images
    printf("Saving images...\n");

    LadybugProcessedImage processedImage;
    processedImage.pData = arpBuffers[myData->uiCamera];
    processedImage.pixelFormat = LADYBUG_BGRU;
    processedImage.uiCols = myData->image.uiCols;
    processedImage.uiRows = myData->image.uiRows;

    char pszOutputFilePath[256] = {0};
    sprintf(pszOutputFilePath, "ladybug_frame%03u_%u_camera_%02u.jpg", myData->frames, myData->caminfo.serialHead, myData->uiCamera);
    const std::string outputPath = getWriteableDirectory() + std::string(pszOutputFilePath);

    error = ::ladybugSaveImage(myData->context, &processedImage, outputPath.c_str(), LADYBUG_FILEFORMAT_JPG);
    //_HANDLE_ERROR;

    printf("Saved camera %u image to %s.\n", myData->uiCamera, outputPath.c_str());

    // Clean up the buffers
    for (unsigned int uiCamera = 0; uiCamera < LADYBUG_NUM_CAMERAS; uiCamera++)
    {
        delete arpBuffers[uiCamera];
    }

    pthread_exit(NULL);
}

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


        //call threads here

        // pthread_t threads[NUM_THREADS];
        struct threadData td[NUM_THREADS];
        int rc;

        for (int i = 0; i < NUM_THREADS; i++)
        {
            cout << "creating thread " << i << endl;
            
            td[i].uiCamera = i;
            td[i].context = context;
            td[i].image = image;
            td[i].frames = frames;
            td[i].caminfo = caminfo;
            td[i].threadID = i;

            saveSingleImage(td);
/*            rc = pthread_create(&threads[i], NULL, saveSingleImage, (void *)i);

            if (rc)
            {
                cout << "Error creating thread " << rc << endl;
                exit(-1);
            }*/
        }
        // pthread_exit(NULL);
    }

    // Destroy the context
    printf("Destroying context...\n");
    error = ::ladybugDestroyContext(&context);
    _HANDLE_ERROR;

    printf("Done.\n");

    return 0;
}



int createThreads()
{
    pthread_t threads[NUM_THREADS];
    int rc;

    for (int i = 0; i < NUM_THREADS; i++)
    {
        cout << "creating thread " << i << endl;
        rc = pthread_create(&threads[i], NULL, saveSingleImage, (void *)i);

        if (rc)
        {
            cout << "Error creating thread " << rc << endl;
            exit(-1);
        }
    }
    pthread_exit(NULL);
}