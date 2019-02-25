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
// ladybugSimpleRecording.cpp
//
//=============================================================================
// This example shows users how to record Ladybug images to PGR stream files. 
// This example starts the first Labybug camera on the bus with 
// the parameters in the .ini file defined by INI_FILENAME.
//
// This example displays the grabbed images only when the grabbing function 
// returns LADYBUG_TIMEOUT. This means that saving images is the highest 
// priority. 
//
// Right click the mouse in the client area to popup a menu and select 
// various options, or use the following hot keys:
//    'r' or 'R' - start recording, press again to stop recording.
//    'p' or 'P' - display panoramic image.
//    'a' or 'A' - display all-camera-image.
//    'd' or 'D' - display dome view image.
//    'Esc','q' or 'Q' - exit the program.
//
// This example also shows how to record images when the GPS location changes 
// after a specified distance(in meters). The distance parameter is specified 
// in the .ini file. The accuracy of the result depends on the GPS device and 
// the GPS data update rate.
//
// Note: This example has to be run with freeglut.dll and Ladybug SDK 1.3Alpha02
//     or later.
// 
//=============================================================================

#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <assert.h>
#include <GL/freeglut.h>

#ifdef _WIN32

#include <windows.h>
#include <shlobj.h>

#else

#include <time.h>
#include <unistd.h>
#include <pwd.h>

#endif

#include <ladybug.h>
#include <ladybugGPS.h>
#include <ladybuggeom.h>
#include <ladybugrenderer.h>
#include <ladybugstream.h>

// Macros to check, report on, and handle Ladybug API error codes.
#define _HANDLE_ERROR \
    if( error != LADYBUG_OK ) \
   { \
   printf( "Error! Ladybug library reported %s\n", \
   ::ladybugErrorToString( error ) ); \
   assert( false ); \
   exit( 1 ); \
   } \

#define _DISPLAY_ERROR_MSG_AND_RETURN \
    if( error != LADYBUG_OK ) \
   { \
   printf( "Ladybug library reported %s\n", \
   ::ladybugErrorToString( error ) ); \
   return; \
   } \

struct GPSDATA
{
    // Flag indicating whether all the data in this structure is valid.
    // True - Valid, False - Invalid.
    bool bValidData;

    // Latitude. 
    // < 0 = South of Equator, > 0 = North of Equator. 
    double dLatitude;

    // Longitude. 
    // < 0 = West of Prime Meridian, > 0 = East of Prime Meridian. 
    double dLongitude;
};

#ifndef _WIN32
#define _MAX_PATH 4096
#endif

// INI file name
#define  INI_FILENAME "ladybugSimpleRecording.ini"

// INI file keys
#define INI_RECORDING_FILE_BASE_NAME   "BaseStreamName"
#define INI_WINDOW_POSITION_X          "WindowPositionX"
#define INI_WINDOW_POSITION_Y          "WindowPositionY"
#define INI_WINDOW_SIZE_WIDTH          "WindowSizeWidth"
#define INI_WINDOW_SIZE_HEIGHT         "WindowSizeHeight"
#define INI_NUMBER_OF_BUFFERS          "NumberOfBuffers"
#define INI_RECORDING_AUTO_START       "RecordingAutoStart"
#define INI_LADYBUG_RESOLUTION         "LadybugResolution"
#define INI_DATA_FORMAT                "DataFormat"
#define INI_FRAME_RATE                 "FrameRate"
#define INI_COLOR_PROCESSING_METHOD    "ColorProcessingMethod"
#define INI_INITIAL_DISPLAY_TYPE       "InitialDisplayType"
#define INI_JPEG_COMPRESSION_QUALITY   "JPEGCompressionQuality"
#define INI_RECORDING_GPS_DATA         "RecordingGPSData"
#define INI_PORT_NUMBER                "PortNumber"
#define INI_BAUD_RATE                  "BaudRate"
#define INI_UPDATE_RATE                "UpdateRate"
#define INI_DISTANCE_X                 "Distance_x"

// Values in INI file
char pszStreamBaseName[_MAX_PATH];
int iWindowPositionX;
int iWindowPositionY;
int iWindowSizeWidth;
int iWindowSizeHeight;
int iNumberOfBuffers;
bool bRecordingAutoStart;
LadybugDataFormat ladybugDataFormat;
LadybugColorProcessingMethod  colorProcessingMethod;
bool bRecordingGPSData;
int iPortNumber = 1;
int iBaudRate = 4800;
int iUpdateRate = 1;
int iDistance_x = 10;

enum DisplayModes
{
    MENU_PANORAMIC = 1, // Display Panoramic 
    MENU_ALL_CAMERAS, // Display all camera images in one view
    MENU_DOME, // Display dome view
};

static double lastIdleTime;
unsigned int frameCounter = 0;
double frameRate = 0.0;
double totalMBWritten = 0.0;
unsigned long totalNumberOfImagesWritten = 0;
bool fullScreenMode = false;
bool isTextureUpdated = false;

LadybugOutputImage uiDisplayMode = LADYBUG_PANORAMIC;

LadybugContext context = NULL;            // Ladybug context
LadybugStreamContext streamContext = NULL;      // Ladybug stream context
LadybugError error;                       // Ladybug error message
LadybugImage image_Current, image_Prev; // Ladybug image
bool b[256];                    // keyboard state
bool bRecordingInProgress = false;
int menu;
LadybugGPSContext GPScontext = NULL;
GPSDATA GPS_Data_Prev, GPS_Data_Current;

//=============================================================================
// A class for reading INI file.  
// The "default" value passed to "get" arguments will be returned 
// if the key is not found in the .ini file.
//=============================================================================
class ReadINIFile  
{
public:
    //
    // Error codes returned by methods of this class.
    //
    enum Error
    {
        OK,
        FILE_NOT_FOUND,
        FILE_ERROR,
        KEY_NOT_FOUND,
    };

    ReadINIFile()
    {
        m_pfile = NULL;
    }

    virtual ~ReadINIFile()
    {
        close();
    };

    //
    // Open an .ini file.  This must be done before any other operation.  
    //
    Error open( const char* pszFilename )
    {
        m_pfile = ::fopen( pszFilename, "r" );
        if ( m_pfile == NULL )
            return FILE_ERROR;
        else
            return OK;
    }

    //
    // Close the current .ini file.
    //
    void close()
    {
        if( m_pfile != NULL )
        {
            ::fclose( m_pfile );
            m_pfile = NULL;
        }
    }

    //
    // Get a string value.
    //
    Error getString( 
        const char* pszKey, char* pszValue, int iSize, const char* pszDefault )
    {      
        if( m_pfile == NULL )
        {
            strncpy( pszValue, pszDefault, iSize );
            return FILE_NOT_FOUND;
        }

        char pszTempVal[ _MAX_PATH ];
        Error readError = readKey( pszKey, pszTempVal );

        if(readError == OK )
            strncpy( pszValue, pszTempVal, iSize );
        else
            strncpy( pszValue, pszDefault, iSize );      

        return readError;
    }

    //
    // Get an integer value.
    //
    Error getInt( const char* pszKey, int* piValue, const int iDefault )
    {
        if( m_pfile == NULL )
        {
            *piValue = iDefault;
            return FILE_NOT_FOUND;
        }

        char pszTempVal[ _MAX_PATH ];
        Error readError = readKey( pszKey, pszTempVal );

        if(readError == OK )
            *piValue = ::atoi( pszTempVal );
        else
            *piValue = iDefault;

        return readError;
    }

    //
    // Get a double value.
    //
    Error getDouble( const char* pszKey, double* pdValue, const double dDefault )
    {
        if( m_pfile == NULL )
        {
            *pdValue = dDefault;
            return FILE_NOT_FOUND;
        }

        char pszTempVal[ _MAX_PATH ];
        Error readError = readKey( pszKey, pszTempVal );

        if(readError == OK )
            *pdValue = ::atof( pszTempVal );
        else
            *pdValue = dDefault;

        return readError;
    }

    //
    // Get a boolean value.
    //
    Error getBool( const char* pszKey, bool* pbValue, const bool bDefault )
    {
        if( m_pfile == NULL )
        {
            *pbValue = bDefault;
            return FILE_NOT_FOUND;
        }

        char pszTempVal[ _MAX_PATH ];
        Error readError = readKey( pszKey, pszTempVal );

        if(readError == OK )
        {
            //
            // True is nonzero or non-"false".
            //
            *pbValue =
                !(   pszTempVal[ 0 ] == '0' || 
                pszTempVal[ 0 ] == 'F' || 
                pszTempVal[ 0 ] == 'f' );
        }
        else
        {
            *pbValue = bDefault;
        }

        return readError;
    }

private:

    // Handle to the .ini file to open
    FILE*    m_pfile;

    // Finds the specified key 
    Error readKey( const char* pszKeyName, char* pszValue )
    {
        char pszLineBuffer[ _MAX_PATH ];
        bool bKeyFound = false;

        assert( m_pfile != NULL );
        assert( pszKeyName != NULL );
        assert( pszValue != NULL );

        ::rewind( m_pfile );

        while( !bKeyFound && 
            ( fgets( pszLineBuffer, _MAX_PATH, m_pfile ) != NULL ) )
        {      
            if( strchr( "#;", pszLineBuffer[0] ) != NULL )
            {
                continue; // Skip comment lines
            }

            if( pszLineBuffer[ 0 ] == '[' )
                return KEY_NOT_FOUND;         

            char* pPos;
            if( ( pPos = strchr( pszLineBuffer, '=' ) ) != NULL )
            {
                if( strstr( pszLineBuffer, pszKeyName ) != NULL )
                {
                    strncpy( pszValue, pPos + 1, _MAX_PATH );

                    if( ( pPos = strrchr( pszValue, '\n' ) ) != NULL )
                        *pPos = '\0';

                    if( ( pPos = strrchr( pszValue, '\r' ) ) != NULL )
                        *pPos = '\0';

                    return OK;
                }
            }
        }

        if( !bKeyFound )
            return KEY_NOT_FOUND;

        return OK;
    }
};

//=============================================================================
// Load parameters from the INI file
//=============================================================================
bool 
loadIniFile( const char* pszINIFilename )
{
    ReadINIFile          iniFile;
    ReadINIFile::Error   iniFileError;
    bool                 bErrorFound = false;
    int                  iItemIndex;

    // Open the ini file
    iniFile.open( pszINIFilename );

    // Read data from ini file
    iniFileError = iniFile.getInt( 
        INI_WINDOW_POSITION_X, &iWindowPositionX, 100 );
    if ( iniFileError != ReadINIFile::OK ) bErrorFound = true;
    iniFileError = iniFile.getInt( 
        INI_WINDOW_POSITION_Y, &iWindowPositionY, 100 );
    if ( iniFileError != ReadINIFile::OK ) bErrorFound = true;
    iniFileError = iniFile.getInt( 
        INI_WINDOW_SIZE_WIDTH, &iWindowSizeWidth, 1000 );
    if ( iniFileError != ReadINIFile::OK ) bErrorFound = true;
    iniFileError = iniFile.getInt( 
        INI_WINDOW_SIZE_HEIGHT, &iWindowSizeHeight, 500 );
    if ( iniFileError != ReadINIFile::OK ) bErrorFound = true;
    iniFileError = iniFile.getInt( 
        INI_NUMBER_OF_BUFFERS, &iNumberOfBuffers, 30 );
    if ( iniFileError != ReadINIFile::OK ) bErrorFound = true;
    iniFileError = iniFile.getBool( 
        INI_RECORDING_AUTO_START, &bRecordingAutoStart, false );
    if ( iniFileError != ReadINIFile::OK ) bErrorFound = true;

    iniFileError = iniFile.getBool( 
        INI_RECORDING_GPS_DATA, &bRecordingGPSData, false );
    if ( iniFileError != ReadINIFile::OK ) bErrorFound = true;
    iniFileError = iniFile.getInt( 
        INI_PORT_NUMBER, &iPortNumber, 1 );
    if ( iniFileError != ReadINIFile::OK ) bErrorFound = true;
    iniFileError = iniFile.getInt( 
        INI_BAUD_RATE, &iBaudRate, 4800 );
    if ( iniFileError != ReadINIFile::OK ) bErrorFound = true;
    iniFileError = iniFile.getInt( 
        INI_UPDATE_RATE, &iUpdateRate, 1 );
    if ( iniFileError != ReadINIFile::OK ) bErrorFound = true;
    iniFileError = iniFile.getInt( 
        INI_DISTANCE_X, &iDistance_x, 10 );
    if ( iniFileError != ReadINIFile::OK ) bErrorFound = true;

    iniFileError = iniFile.getInt( INI_DATA_FORMAT, &iItemIndex, 1 );
    if ( iniFileError != ReadINIFile::OK ) bErrorFound = true;
    switch ( iItemIndex )
    {
    case 0:
        ladybugDataFormat = LADYBUG_DATAFORMAT_RAW8;
        break;
    case 1:
        ladybugDataFormat = LADYBUG_DATAFORMAT_COLOR_SEP_JPEG8;
        break;
    case 2:
        ladybugDataFormat = LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW8;
        break;
    case 3:
        ladybugDataFormat = LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG8;
        break;
    case 4:
        ladybugDataFormat = LADYBUG_DATAFORMAT_RAW12;
        break;
    case 5:
        ladybugDataFormat = LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW12;
        break;
    case 6:
        ladybugDataFormat = LADYBUG_DATAFORMAT_COLOR_SEP_JPEG12;
        break;
    case 7:
        ladybugDataFormat = LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG12;
        break;
    default:
        ladybugDataFormat = LADYBUG_DATAFORMAT_COLOR_SEP_JPEG8;
    }

    iniFileError = iniFile.getInt( 
        INI_COLOR_PROCESSING_METHOD, &iItemIndex, 4 );
    if ( iniFileError != ReadINIFile::OK ) bErrorFound = true;
    switch ( iItemIndex )
    {
    case 0:
        colorProcessingMethod = LADYBUG_EDGE_SENSING;
        break;
    case 1:
    case 2:
        colorProcessingMethod = LADYBUG_NEAREST_NEIGHBOR_FAST;
        break;
    case 3:
        colorProcessingMethod = LADYBUG_RIGOROUS;
        break;
    case 4:
        colorProcessingMethod = LADYBUG_DOWNSAMPLE4;
        break;
    case 5:
        colorProcessingMethod = LADYBUG_DOWNSAMPLE16;
        break;
    case 6:
        colorProcessingMethod = LADYBUG_MONO;
        break;
    case 7:
        colorProcessingMethod = LADYBUG_HQLINEAR;
        break;
    case 8:
        colorProcessingMethod = LADYBUG_HQLINEAR_GPU;
        break;
    case 9:
        colorProcessingMethod = LADYBUG_DIRECTIONAL_FILTER;
        break;    
    case 10:
        colorProcessingMethod = LADYBUG_WEIGHTED_DIRECTIONAL_FILTER;
        break;
    default:
        colorProcessingMethod = LADYBUG_NEAREST_NEIGHBOR_FAST;
    }

    iniFileError = iniFile.getInt( INI_INITIAL_DISPLAY_TYPE, &iItemIndex, 0 );
    if ( iniFileError != ReadINIFile::OK ) bErrorFound = true;
    switch ( iItemIndex )
    {
    case 0:
        uiDisplayMode = LADYBUG_PANORAMIC;
        break;
    case 1:
        uiDisplayMode = LADYBUG_ALL_CAMERAS_VIEW;
        break;
    case 2:
        uiDisplayMode = LADYBUG_DOME;
        break;
    default:
        uiDisplayMode = LADYBUG_PANORAMIC;
    }

    // Close ini file
    iniFile.close();

    if ( bErrorFound )
    {
        return false;
    }
    else
    {
        return true;
    }
}

/** Determine if the data format is high bit depth. */
bool isHighBitDepth( LadybugDataFormat format)
{
    return format == LADYBUG_DATAFORMAT_RAW12 ||
        format == LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW12 ||
        format == LADYBUG_DATAFORMAT_COLOR_SEP_JPEG12 ||
        format == LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG12 ||
        format == LADYBUG_DATAFORMAT_RAW16 ||
        format == LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW16;
}

/*
Returns the current time in milliseconds based on a monotonically 
increasing clock with an unspecified starting time.
*/ 
double getCurrentMs()
{
#ifdef _WIN32
    return (double)GetTickCount();
#else
    timespec currentTime;
    if(clock_gettime(CLOCK_MONOTONIC, &currentTime))
    {
        return 0.0;
    }else
    {
        return (double)(currentTime.tv_sec * 1000 + (currentTime.tv_nsec /1000000.0));
    }
#endif
}

//Sleeps for a specified amount of milliseconds.
void sleepMs(unsigned int ms)
{
#ifdef _WIN32
    Sleep(ms);
#else
    usleep(ms * 1000);
#endif
}


//=============================================================================
// Clean up
//=============================================================================
void
cleanUp( void )
{
    if ( GPScontext != NULL )
    {
        //
        // Stop, Unregister GPS and destroy GPS context
        //
        error = ladybugStopGPS( GPScontext );
        _HANDLE_ERROR;

        error = ladybugUnregisterGPS( context, &GPScontext );
        _HANDLE_ERROR;

        error = ladybugDestroyGPSContext( &GPScontext );
        GPScontext = NULL;
        _HANDLE_ERROR;
    }

    printf( "Destroying context...\n" );
    if ( context != NULL ) 
    {
        error = ladybugDestroyContext( &context );
        _HANDLE_ERROR;
        context = NULL;
    }

    if ( streamContext != NULL )
    {
        error = ladybugDestroyStreamContext ( &streamContext );
        _HANDLE_ERROR;
        streamContext = NULL;
    }

    glutDestroyMenu( menu );

    return;
}


//=============================================================================
// Process keyboard command
//=============================================================================
void 
keyboard( unsigned char k, int /*x*/, int /*y*/ )
{
    b[k] = !b[k];

    switch ( k )
    {
    case 'q':
    case 'Q':
        if ( bRecordingInProgress )
        {
            error = ladybugStopStream( streamContext );
            _HANDLE_ERROR;      
        }

        cleanUp();
        exit (0);
        break; 

    case 'r':
    case 'R':
        //
        // Start recording
        //
        lastIdleTime = getCurrentMs();
        frameCounter = 0;   
        if ( !bRecordingInProgress )
        {
            // Get file name 
            ReadINIFile iniFile;
            ReadINIFile::Error iniFileError; 
            char pszStreamNameToOpen[ _MAX_PATH ];
            char pszStreamNameOpened[ _MAX_PATH ];

            // Open the ini file
            iniFile.open( INI_FILENAME );

            // Get stream file base name from ini file
            iniFileError = iniFile.getString( 
                INI_RECORDING_FILE_BASE_NAME, 
                pszStreamBaseName, _MAX_PATH,  "pgrLadybugStream" );
#ifdef _WIN32
            // Try to record stream to "My Documents".
            char buf[_MAX_PATH];
            HRESULT hres = SHGetFolderPath( NULL, CSIDL_PERSONAL, NULL, 0, buf);
            if ( hres == S_OK)
            {
                // Set stream name to "My Documents" folder
                sprintf( pszStreamNameToOpen,
                    "%s\\%s\0",buf, pszStreamBaseName );
            }
            else
            {
                // Set stream name to current folder
                strcpy( pszStreamNameToOpen, buf);
            }
#else
            const char *homedir = getenv("HOME");

            if (homedir == NULL)
            {
                uid_t uid = getuid();
                passwd *pw = getpwuid(uid);

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
                sprintf( pszStreamNameToOpen,
                    "%s/%s",homedir, pszStreamBaseName );
            }
#endif
            if ( bRecordingGPSData )
            {
                //Read GPS distance for recording
                iniFileError = iniFile.getInt( 
                    INI_DISTANCE_X, &iDistance_x, 10 );
                sprintf( pszStreamNameToOpen,
                    "%s_GPS_%dMeter\0",pszStreamBaseName, iDistance_x );
            }

            iniFile.close();

            if ( streamContext == NULL )
            {
                error = ladybugCreateStreamContext( &streamContext );
                _HANDLE_ERROR;      
            } 

            // Used to track image/frame stats
            totalMBWritten = 0;
            totalNumberOfImagesWritten = 0;


            error = ladybugInitializeStreamForWriting( 
                streamContext, 
                pszStreamNameToOpen, 
                context,
                pszStreamNameOpened,
                true );
            bRecordingInProgress = (error == LADYBUG_OK );
            if ( bRecordingInProgress )
            {
                printf( "Recording to %s\n", pszStreamNameOpened );
            }
            else
            {
                error = ladybugStopStream ( streamContext );
            }
        }
        else
        {
            //
            // Stop recording
            //
            error = ladybugStopStream( streamContext );
            bRecordingInProgress = false;
        }
        _DISPLAY_ERROR_MSG_AND_RETURN;  
        break;

    case 'p':
    case 'P':
        uiDisplayMode = LADYBUG_PANORAMIC;
        break;

    case 'd':
    case 'D':
        uiDisplayMode = LADYBUG_DOME;
        break;

    case 'a':
    case 'A':
        uiDisplayMode = LADYBUG_ALL_CAMERAS_VIEW;      
        break; 

    case 'f':
    case 'F':
        fullScreenMode = !fullScreenMode;
        if ( fullScreenMode )
        {
            glutFullScreen();
        }
        else
        {
            glutReshapeWindow( iWindowSizeWidth, iWindowSizeHeight ); 
            glutPositionWindow( iWindowPositionX, iWindowPositionY  );
        }
        break;  
    case 27: // ESC pressed
        if ( fullScreenMode )
        {
            fullScreenMode = !fullScreenMode;
            glutReshapeWindow( iWindowSizeWidth, iWindowSizeHeight ); 
            glutPositionWindow( iWindowPositionX, iWindowPositionY  );
        }
        break;  
    }

    // Redraw the window
    glutPostRedisplay();
}

//=============================================================================
// Display mode selection
//=============================================================================
void 
selectFromMenu( int iCommand )
{  
    keyboard((unsigned char)iCommand, 0, 0);
}

//=============================================================================
// Create a popup menu for selecting display modes
//=============================================================================
int 
buildPopupMenu (void)
{   
    menu = glutCreateMenu ( selectFromMenu );
    glutAddMenuEntry ( "Start/Stop Recording [r]", 'r' );
    glutAddMenuEntry ( "View Panoramic Image [p]", 'p' );
    glutAddMenuEntry ( "View All Camera Images [a]", 'a' );
    glutAddMenuEntry ( "View Dome Image [d]", 'd' );
    glutAddMenuEntry ( "Toggle Full Screen Mode [f]", 'f' );
    glutAddMenuEntry ( "Exit [q]", 'q');

    return 0;
}

//=============================================================================
// Start the first Ladybug2 camera the bus
//=============================================================================
int  
startCamera( void)
{    
    //
    // Initialize context.
    //
    error = ::ladybugCreateContext( &context );
    _HANDLE_ERROR;

    //
    // Initialize the first ladybug on the bus.
    //
    printf( "Initializing.\n" );
    error = ladybugInitializePlus( 
        context, 0, iNumberOfBuffers, NULL, 0 );
    _HANDLE_ERROR;

    if ( bRecordingGPSData )
    {
        printf( "Creating GPS context...\n" );
        error = ladybugCreateGPSContext( &GPScontext );
        _HANDLE_ERROR;


        printf( "Initializing GPS...\n" );
        error = ladybugInitializeGPS( 
            GPScontext, 
            iPortNumber, 
            iBaudRate, 
            1000/iUpdateRate );
        _HANDLE_ERROR;

        // Echo settings
        printf( "GPS initialized with the following settings:\n" );
        printf( "\tCOM Port: %d\n", iPortNumber );
        printf( "\tBaud Rate: %d\n", iBaudRate );
        printf( "\tUpdate Interval: %ums\n", 1000/iUpdateRate );

        printf( "Starting GPS...\n" );
        error = ladybugStartGPS( GPScontext );
        _HANDLE_ERROR;

        printf( "Registering GPS...\n" );
        error = ladybugRegisterGPS( context, &GPScontext );
        _HANDLE_ERROR;

        GPS_Data_Prev.bValidData = false;
        GPS_Data_Prev.dLatitude = 0;
        GPS_Data_Prev.dLongitude = 0;
    }

    //
    // Set the timeout value for grab function to zero.
    // Do not wait if there is no image waiting. 
    //
    ladybugSetGrabTimeout( context, 0 );

    //
    // Start Ladybug with the specified data format
    //
    printf( "Starting camera...\n" );
    error = ::ladybugStartLockNext(
        context,
        ladybugDataFormat);
    _HANDLE_ERROR;

    //
    // Set color processing method
    //
    error = ::ladybugSetColorProcessingMethod( context, colorProcessingMethod );
    _HANDLE_ERROR;

    int iTryTimes = 0;
    do
    {
        ladybugUnlockAll( context );
        
        sleepMs( 100 );
        error = ladybugLockNext( context, &image_Prev );
    } while ( ( error != LADYBUG_OK )  && ( iTryTimes++ < 10) );    
    _HANDLE_ERROR;

    //
    // Load config file from the head
    //
    printf( "Loading config info...\n" );
    error = ladybugLoadConfig( context, NULL );
    _HANDLE_ERROR;

    //
    // Set the size of the image to be processed
    //
    unsigned int uiRawCols, uiRawRows;
    if (colorProcessingMethod == LADYBUG_DOWNSAMPLE16)
    {
        uiRawCols = image_Prev.uiCols / 4;
        uiRawRows = image_Prev.uiRows / 4;
    }
    else if( colorProcessingMethod == LADYBUG_DOWNSAMPLE4 || 
        colorProcessingMethod == LADYBUG_MONO )
    {
        uiRawCols = image_Prev.uiCols / 2;
        uiRawRows = image_Prev.uiRows / 2;
    }
    else
    {
        uiRawCols = image_Prev.uiCols;
        uiRawRows = image_Prev.uiRows;
    }    

    //
    // Initialize alpha masks
    //
    printf( "Initializing alpha masks...\n" );
    error = ladybugInitializeAlphaMasks( context, uiRawCols, uiRawRows );
    _HANDLE_ERROR;
    ladybugSetAlphaMasking( context, true );


    // we will not use auto buffer usage feature, set it to a fixed percentage
    printf( "Disabling Auto JPEG Quality control...\n");
    error = ladybugSetAutoJPEGQualityControlFlag( context, false);
    _HANDLE_ERROR;

    printf( "Setting JPEG quality to 80...\n");
    error = ladybugSetJPEGQuality(context, 80);
    _HANDLE_ERROR;

    if ( bRecordingAutoStart )
    {
        keyboard('r', 0, 0);
    }

    return 0;
}


//=============================================================================
// Retrieve GPS data from image
// Get GPS data from GGA, RMC or GLL
//=============================================================================
void retrieveGPSData( const LadybugImage* pImage,  GPSDATA* pGPS_Data )
{
    LadybugError gpsError;

    LadybugNMEAGPGGA NMEAGGA_Data;
    pGPS_Data->bValidData = false;
    gpsError = ladybugGetGPSNMEADataFromImage( pImage, "GPGGA", &NMEAGGA_Data );
    if ( gpsError == LADYBUG_OK &&  NMEAGGA_Data.bValidData )
    {
        pGPS_Data->dLatitude = NMEAGGA_Data.dGGALatitude;
        pGPS_Data->dLongitude = NMEAGGA_Data.dGGALongitude;
        pGPS_Data->bValidData = true;
    }
    else
    {
        LadybugNMEAGPRMC NMEARMC_Data;
        gpsError = ladybugGetGPSNMEADataFromImage( pImage, "GPRMC", &NMEARMC_Data );
        if ( gpsError == LADYBUG_OK &&  NMEARMC_Data.bValidData )
        {
            pGPS_Data->dLatitude = NMEARMC_Data.dRMCLatitude;
            pGPS_Data->dLongitude = NMEARMC_Data.dRMCLongitude;
            pGPS_Data->bValidData = true;
        }
        else
        {
            LadybugNMEAGPGLL NMEAGLL_Data;
            gpsError = ladybugGetGPSNMEADataFromImage( pImage, "GPGLL", &NMEAGLL_Data );
            if (  gpsError == LADYBUG_OK &&  NMEAGLL_Data.bValidData )
            {
                pGPS_Data->dLatitude = NMEAGLL_Data.dGLLLatitude;
                pGPS_Data->dLongitude = NMEAGLL_Data.dGLLLongitude;
                pGPS_Data->bValidData = true;
            }
        }
    }
}

//=============================================================================
// Display Ladybug images
//=============================================================================
void 
display(void)
{    
    if ( !isTextureUpdated)
    {
        return;
    }

    // Clear frame buffer and depth buffer
    glClearColor( 0.0f, 0.0f, 0.0f, 0.0f );
    glClearDepth(1.0);
    glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
    glLoadIdentity();

    // Display Ladybug images
    error = ladybugDisplayImage( context, uiDisplayMode );
    _DISPLAY_ERROR_MSG_AND_RETURN;

    char pszTimeString[128] = {0};
    if ( bRecordingGPSData )
    {
        if ( GPS_Data_Prev.bValidData )
        {
            sprintf( 
                pszTimeString, 
                "Grab and Display: %4.1ffps GPS Lat: %.5f Lon: %.5f", 
                frameRate, 
                GPS_Data_Prev.dLatitude, 
                GPS_Data_Prev.dLongitude );
        }
        else
        {
            sprintf( 
                pszTimeString, 
                "Grab and Display: %4.1ffps GPS: No valid data", 
                frameRate );
        }
    }
    else
    {
        sprintf( 
            pszTimeString, 
            "Grab and Display: %4.1ffps", 
            frameRate );
    }

    if ( !bRecordingInProgress )
    {
        glutSetWindowTitle( pszTimeString );
    }

    // Make sure changes appear onscreen
    glutSwapBuffers();
}

//=============================================================================
// Calculates distances between the two GPS points
// The two points is signed decimal degrees without compass direction. 
// Negative indicates west/south, e.g. 49.194169, -123.14540505
//=============================================================================
double dGPSDistance( double dLat1, double dLon1, double dLat2, double dLon2 )
{
    double R = 6371; // km
    double PI = 3.14159265358979323846;
    // Use Haversine formula to calculate distance (in km) between two points specified by 
    // latitude/longitude (in numeric degrees). 
    // It is the shortest distance over the earth�s surface.
    double dLat = (dLat2-dLat1) * (PI/180.0);
    double dLon = (dLon2-dLon1) * (PI/180.0);
    double a = sin(dLat/2) * sin(dLat/2) +
        cos(dLat1 * (PI/180.0)) * cos(dLat2 * (PI/180.0)) * 
        sin(dLon/2) * sin(dLon/2); 
    double c = 2 * atan2(sqrt(a), sqrt(1-a)); 
    // Distance in km
    double d = R * c;

    // Return distance in meters
    return d*1000.0;
}


//=============================================================================
// Grab and save images. Display an image only if no image waiting for writing
//=============================================================================
void 
recordingImage( void )
{
    char pszTimeString[128] = {0};
    double dTimeDiff = 0;    
    bool bRecordingCurrentImage = false;
    double dDistance = 0;

    error = ladybugLockNext( context, &image_Current );

    switch ( error )
    {
    case LADYBUG_OK:
        // Calculate frame rate
        dTimeDiff = (( image_Current.timeStamp.ulCycleSeconds * 8000.0 + 
            image_Current.timeStamp.ulCycleCount ) -  
            ( image_Prev.timeStamp.ulCycleSeconds * 8000.0 + 
            image_Prev.timeStamp.ulCycleCount ) ) / 8000.0;
        frameRate =  1.0 / dTimeDiff;

        if ( bRecordingGPSData )
        {         
            // Retrieve the GPS data from the current image
            if ( totalNumberOfImagesWritten == 0 )
                // It is the first image. 
                retrieveGPSData( &image_Current, &GPS_Data_Prev );
            else
                retrieveGPSData( &image_Current, &GPS_Data_Current );
        }

        bRecordingCurrentImage = true;
        if ( bRecordingGPSData && iDistance_x > 0 && totalNumberOfImagesWritten != 0) 
        {
            // Only calculate distance when iDistance_x > 0
            // Check the GPS distance
            if ( GPS_Data_Current.bValidData && GPS_Data_Prev.bValidData )
            {
                dDistance = dGPSDistance(
                    GPS_Data_Current.dLatitude, GPS_Data_Current.dLongitude,
                    GPS_Data_Prev.dLatitude, GPS_Data_Prev.dLongitude );
                bRecordingCurrentImage = dDistance >= iDistance_x; 
                if ( bRecordingCurrentImage )
                {
                    GPS_Data_Prev = GPS_Data_Current;
                    printf( "Distance:%f >= %d \n", dDistance, iDistance_x );
                }
            }
            else
            {
                bRecordingCurrentImage = false;
            }
        }

        //
        // Save the image to disk
        //
        if ( streamContext != NULL  && bRecordingInProgress )
        {
            if ( bRecordingCurrentImage )
            {
                error = ladybugWriteImageToStream( streamContext, 
                    &image_Current, 
                    &totalMBWritten, 
                    &totalNumberOfImagesWritten ); 
                if ( error != LADYBUG_OK )
                {
                    //
                    // Stop recording if a write error occurs
                    // If the return value is LADYBUG_ERROR_DISK_NOT_ENOUGH_SPACE, 
                    // the disk is full.
                    //
                    bRecordingInProgress = false;
                    ladybugStopStream ( streamContext );
                    _DISPLAY_ERROR_MSG_AND_RETURN;  
                }
            }

            char pszGPSStr[64];
            if ( GPS_Data_Current.bValidData )
            {
                sprintf( 
                    pszGPSStr,
                    "GPS Lat: %.5f Lon: %.5f\0", 
                    GPS_Data_Current.dLatitude, 
                    GPS_Data_Current.dLongitude );
            }
            else
            {
                sprintf( pszGPSStr,"GPS: No Data\0" );
            }
            sprintf( 
                pszTimeString,
                "Grab: %4.1ffps Recording Total Data: %.1fMB Frames: %lu %s",
                frameRate, 
                totalMBWritten, 
                totalNumberOfImagesWritten,
                pszGPSStr );

            //
            // Set to the window title to show the current info
            //
            glutSetWindowTitle( pszTimeString );
        }

        ladybugUnlock( context, image_Prev.uiBufferIndex );
        image_Prev = image_Current;
        _DISPLAY_ERROR_MSG_AND_RETURN; 
        break;      

    case LADYBUG_TIMEOUT:
        {
            // There is no image waiting, display the previous image.
            // Convert the image first
            error = ladybugConvertImage( context, &image_Prev, NULL );    
            _DISPLAY_ERROR_MSG_AND_RETURN;

            // Update images to the graphics card
            const LadybugPixelFormat pixelFormatToUse = isHighBitDepth(ladybugDataFormat) ? LADYBUG_BGRU16 : LADYBUG_BGRU;
            error = ladybugUpdateTextures( context, LADYBUG_NUM_CAMERAS, NULL, pixelFormatToUse);
            _DISPLAY_ERROR_MSG_AND_RETURN;
            isTextureUpdated = true;

            // Redisplay
            glutPostRedisplay();
        }
        break;

    default: 
        ladybugUnlockAll( context );
        _DISPLAY_ERROR_MSG_AND_RETURN; 
    }
}


int main(int argc, char** argv)
{

    //
    // Read initial configuration data from INI file
    //
    if ( !loadIniFile( INI_FILENAME ) )
    {
        printf( "Error found in reading INI file:%s. "
            "One or more default values are being used.\n", INI_FILENAME );
    };

    //
    // GLUT Window Initialization
    //
    glutInit( &argc, argv );
    glutInitWindowSize( iWindowSizeWidth, iWindowSizeHeight );
    glutInitDisplayMode ( GLUT_RGBA | GLUT_DOUBLE );
    glutInitWindowPosition( iWindowPositionX, iWindowPositionY );
    glutCreateWindow ( "Ladybug Simple Grab and Display" );

    //
    // Start Ladybug2 camera
    //
    startCamera();

    //
    // Configure output images in Ladybug library
    //
    error = ladybugConfigureOutputImages( 
        context, LADYBUG_PANORAMIC | LADYBUG_DOME );
    _HANDLE_ERROR;

    //
    // Set Ladybug library for displaying Ladybug image in the window
    //
    error = ladybugSetDisplayWindow( context );
    _HANDLE_ERROR;

    //
    // Register keyboard function:
    //
    glutKeyboardFunc(keyboard);

    //
    // Register callbacks:
    //
    glutDisplayFunc( display );

    //
    // Set recording loop function
    //
    glutIdleFunc( recordingImage );

    //
    // Create a popup menu
    //
    buildPopupMenu();
    glutAttachMenu(GLUT_RIGHT_BUTTON);

    //
    // Reset frame counter
    //
    lastIdleTime = getCurrentMs();
    frameCounter = 0;

    glutCloseFunc( cleanUp );

    //
    // Turn the flow of control over to GLUT
    //
    printf( "Grabbing...\n" );
    glutMainLoop();
    return 0;
}

