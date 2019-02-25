//=============================================================================
// Copyright © 2017 FLIR Integrated Imaging Solutions, Inc. All Rights Reserved.
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

#ifndef __LADYBUGGPS_H__
#define __LADYBUGGPS_H__

/** 
 * @defgroup LadybugGPS_h ladybugGPS.h
 *
 * ladybugGPS.h
 *
 * This file defines GPS data structures, as well as API functions to retrieve
 * data from GPS devices and Ladybug images.
 *
 * We welcome your bug reports, suggestions, and comments: 
 * www.ptgrey.com/support/contact
 */

/*@{*/

#ifdef __cplusplus
extern "C"
{
#endif

#include "ladybug.h"

/** The maximum number of GPS channels. */
#define NP_MAX_CHAN             36  

/** The default GPS port number. */
#define DEFAULT_PORT_NUMBER      1

/** The default device name. */
#define DEFAULT_DEVICE_NAME "/dev/ttyACM0"

/** The default GPS baud rate. */
#define DEFAULT_BAUDRATE         4800

/** The default GPS device NMEA data update rate in milliseconds. */
#define DEFAULT_UPDATE_INTERVAL  1000

/**
 * The Ladybug GPS context. To access GPS-specific methods, a  
 * LadybugGPSContext must be created using ladybugCreateGPSContext().
 * Once created, the context is passed to the particular function.
 */
typedef void* LadybugGPSContext;

/**
 * GPS NMEA GPGGA data - Fix information.
 * Essential fix data which provides a 3D location and accuracy data.
 */
typedef struct LadybugNMEAGPGGA
{   
   /** Flag indicating whether all the data in this structure is valid. */
   bool bValidData;

   /** Hour, Coordinated Universal Time. */
   unsigned char ucGGAHour;

   /** Minute, Coordinated Universal Time. */
   unsigned char ucGGAMinute;

   /** Second, Coordinated Universal Time.  */
   unsigned char ucGGASecond;

   /** Millisecond portion of the Second*/
   unsigned short wGGASubSecond;

   /** Latitude. < 0 = South of Equator, > 0 = North of Equator. */
   double dGGALatitude;

   /** Longitude. < 0 = West of Prime Meridian, > 0 = East of Prime Meridian. */
   double dGGALongitude;

   /** 
    * GPS Quality Indicator. 
    * 0 = Fix not available. 
    * 1 = GPS SPS mode. 
    * 2 = Differential GPS, SPS mode, fix valid. 
    * 3 = GPS PPS mode, fix valid. 
    */
   unsigned char ucGGAGPSQuality;

   /** Number of satellites in view.  */
   unsigned char ucGGANumOfSatsInUse;

   /** Horizontal Dilution of precision (HDOP) (in meters). */
   double dGGAHDOP;

   /** Antenna Altitude above/below mean-sea-level (geoid) (in meters). */
   double dGGAAltitude;

   /** 
    * Geoidal separation.
    * This is the difference between the WGS-84 earth ellipsoid and the
    * and mean sea level (geoid). A negative value means the mean-sea-level
    * below ellipsoid.
    */
   double dGGAHeightOfGeoid ;

   /** 
    * A counter containing the number of GPGGA sentences parsed since
    * the GPS device was started (or restarted). 
    * If the GPS data is from a Ladybug image, it is always 1.
    */
   unsigned long ulCount;

   /** Reserved. */
   unsigned int ulReserved[14];
   
} LadybugNMEAGPGGA;
   
/**
 * GPS NMEA GPGSA data - Overall Satellite data.
 *
 * Details on the nature of the fix. It includes the number of
 * satellites being used in the current solution and the DOP 
 * (dilution of precision).
 */
typedef struct LadybugNMEAGPGSA
{
   /** Flag indicating whether all the data is valid. */
   bool bValidData;

   /** 
    * Selection mode. 
    * 'M' = manual, 'A' = automatic 2D/3D. 
    */
   unsigned char ucGSAMode;    

   /** 
    * Fix Mode. 
    *
    * Possible values:
    * - 1 = No fix. 
    * - 2 = 2D fix. 
    * - 3 = 3D fix.  
    */
   unsigned char ucGSAFixMode;

   /** IDs of satellites used for fix. */
   unsigned short wGSASatsInSolution[NP_MAX_CHAN];

   /** Dilution of precision (PDOP). */
   double dGSAPDOP;    

   /** Horizontal dilution of precision (HDOP). */
   double dGSAHDOP;

   /** Vertical dilution of precision (VDOP). */
   double dGSAVDOP;

   /** 
    * A counter containing the number of GPGSA sentences parsed since
    * the GPS device was started (or restarted).
    * If the GPS data is from a Ladybug image, it is always 1.
    */
   unsigned int ulCount;

   /** Reserved. */
   unsigned int ulReserved[16];
   
} LadybugNMEAGPGSA;

/**
 * GPS NMEA GPGSV data - Satellites in view.
 *
 * Data about the satellites that the unit might be able to find based on
 * its viewing mask and almanac data. It also shows current ability to 
 * track this data.
 */
typedef struct LadybugNMEAGPGSV
{
   /** Satellite information structure. Information about a single satellite. */
   typedef struct CNPSatInfo
   {
      /** Satellite ID. */
      unsigned short wPRN;

      /** Signal strength (Signal to Noise Ratio). */ 
      unsigned short wSignalQuality;    

      /** Boolean indicating if this satellite is being used in the solution. */ 
      bool bUsedInSolution;

      /** Azimuth (in degrees). */
      unsigned short wAzimuth;

      /** Elevation (in degrees). */
      unsigned short wElevation;    

   } CNPSatInfo;
   
   /** Whether the data in this structure is valid. */
   bool bValidData;

   /** Total number of messages. */
   unsigned char ucGSVTotalNumOfMsg;

   /** Total satellites in view. */
   unsigned short wGSVTotalNumSatsInView;

   /** Array of satellite information structures. */
   CNPSatInfo GSVSatInfo[NP_MAX_CHAN];

   /** 
    * A counter containing the number of GPGSV sentences parsed since
    * the GPS device was started (or restarted).
    * If the GPS data is from a Ladybug image, it is always 1.
    */
   unsigned int ulCount;

   /** Reserved Space */
   unsigned int ulReserved[16];
   
} LadybugNMEAGPGSV;

/** GPS NMEA GPRMC data - Recommended minimum data for GPS. */
typedef struct LadybugNMEAGPRMC
{
   /** Whether all the data is valid. */
   bool bValidData;

   /** Hour (Coordinated Universal Time). */
   unsigned char ucRMCHour;    

   /** Minute (Coordinated Universal Time). */
   unsigned char ucRMCMinute;    

   /** Second (Coordinated Universal Time). */
   unsigned char ucRMCSecond;

   /** Millisecond portion of the Second*/
   unsigned short wRMCSubSecond;

   /** 
    * Data valid character. 
    * 'A' - Data valid
    * 'V' - Navigation rx warning.
    */
   unsigned char ucRMCDataValid;

   /** Latitude. < 0 = South of Equator, > 0 = North of Equator. */
   double dRMCLatitude;

   /** Longitude. < 0 = West of Prime Meridian, > 0 = East of Prime Meridian. */
   double dRMCLongitude;

   /** Ground speed (in knots). */
   double dRMCGroundSpeed;

   /** Course over ground, degrees true. */
   double dRMCCourse;    

   /** Day. */
   unsigned char ucRMCDay;

   /** Month. */
   unsigned char ucRMCMonth;    

   /** Year. */
   unsigned short wRMCYear;

   /** 
    * Magnetic variation. 
    * Positive values indicate degrees east, while negative values indicate
    * west.
    */
   double dRMCMagVar;    

   /** 
    * A counter containing the number of GPRMC sentences parsed since
    * the GPS device was started (or restarted).
    * If the GPS data is from a Ladybug image, it is always 1.
    */
   unsigned int ulCount;

   /** Reserved. */
   unsigned int ulReserved[14];

} LadybugNMEAGPRMC;

/** GPS NMEA GPZDA data - Date and time. */
typedef struct LadybugNMEAGPZDA
{
   /** Whether all the data is valid. */
   bool bValidData;

   /** Hour (Coordinated Universal Time). */
   unsigned char ucZDAHour;    

   /** Minute (Coordinated Universal Time). */
   unsigned char ucZDAMinute;

   /** Second (Coordinated Universal Time). */
   unsigned char ucZDASecond;

   /** Millisecond portion of the Second*/
   unsigned short wZDASubSecond;

   /** Day. */
   unsigned char ucZDADay;

   /** Month. */
   unsigned char ucZDAMonth;    

   /** Year. */
   unsigned short wZDAYear;

   /** Local zone description, -13 to +13 hours. */
   unsigned char ucZDALocalZoneHour;

   /** Local zone minutes description, 00 to 59 minutes. */
   unsigned char ucZDALocalZoneMinute;    

   /** 
    * A counter containing the number of GPZDA sentences parsed since
    * the GPS device was started (or restarted).
    * If the GPS data is from a Ladybug image, it is always 1.
    */
   unsigned int ulCount;

   /** Reserved. */
   unsigned int ulReserved[14];
   
} LadybugNMEAGPZDA;

/**
 * GPS NMEA GPVTG data - Vector track and Speed over the Ground.
 *
 * Contains the current track and speed as recorded by the GPS device.
 */
typedef struct LadybugNMEAGPVTG
{
   /** Whether all the data is valid. */
   bool bValidData;

   /** Track Made Good. */
   double dVTGTrackMadeGood;

   /** Magnetic Track Made Good.  */
   double dVTGMagneticTrackMadeGood;

   /** Ground Speed (in knots). */
   double dVTGGroundSpeedKnots;

   /** Ground Speed (in kilometers per hour). */
   double dVTGGroundSpeedKilometersPerHour;

   /** 
    * A counter containing the number of GPVTG sentences parsed since
    * the GPS device was started (or restarted).
    * If the GPS data is from a Ladybug image, it is always 1. */
   unsigned int ulCount;

   /** Reserved. */
   unsigned int ulReserved[16];
   
} LadybugNMEAGPVTG;

/** GPS NMEA GPGLL data - Geographic Position Latitude/Longitude. */
typedef struct LadybugNMEAGPGLL
{
   /** Whether all the data is valid. */
   bool bValidData;

   /** Latitude, < 0 = South of Equator, > 0 = North of Equator. */
   double dGLLLatitude; 

   /** Longitude, < 0 = West of Prime Meridian, > 0 = East of Prime Meridian. */
   double dGLLLongitude;    

   /** Hour (Coordinated Universal Time). */
   unsigned char ucGLLHour;    

   /** Minute (Coordinated Universal Time). */
   unsigned char ucGLLMinute;  

   /** Second (Coordinated Universal Time). */
   unsigned char ucGLLSecond; 

   /** Millisecond portion of the Second*/
   unsigned short wGLLSubSecond;
   
   /** 
    * Data valid character. 
    * A - Data Valid
    * V - Data Invalid.
    */
   unsigned char ucGLLDataValid;

   /** 
    * A counter containing the number of GPGLL sentences parsed since
    * the GPS device was started (or restarted). 
    * If the GPS data is from a Ladybug image, it is always 1.
    */
   unsigned int ulCount;

   /** Reserved. */
   unsigned int ulReserved[14];
   
} LadybugNMEAGPGLL;

/** GPS NMEA GPHDT data - Heading - True. */
typedef struct LadybugNMEAGPHDT
{
    /** Whether all the data is valid. */
    bool bValidData;

    /** Heading degrees, true */
    double heading;

    /** 
     * A counter containing the number of GPVTG sentences parsed since
     * the GPS device was started (or restarted).
     * If the GPS data is from a Ladybug image, it is always 1. 
     */
   unsigned int ulCount;

    /** Reserved */
    unsigned int ulReserved[16];

} LadybugNMEAGPHDT;

/** GPS NMEA PRDID data. */
typedef struct LadybugNMEAPRDID
{
    /** Whether all the data is valid. */
    bool bValidData;

    /** Pitch degrees. */
    double pitch;

    /** Roll degrees. */
    double roll;

    /** Heading. */
    double heading;   

    /** 
     * A counter containing the number of GPVTG sentences parsed since
     * the GPS device was started (or restarted).
     * If the GPS data is from a Ladybug image, it is always 1. 
     */
   unsigned int ulCount;

    /** Reserved */
    unsigned int ulReserved[16];

} LadybugNMEAPRDID;

/**
 *   GPS NMEA data. A holder for all of the above structures.
 */
typedef struct LadybugNMEAGPSData
{   
   /** GPGGA data structure  */
   LadybugNMEAGPGGA dataGPGGA;
   
   /** GPGSA data structure */
   LadybugNMEAGPGSA dataGPGSA;
   
   /** GPGSV data structure */
   LadybugNMEAGPGSV dataGPGSV;
   
   /** GPRMC data structure */
   LadybugNMEAGPRMC dataGPRMC;
   
   /** GPZDA data structure */
   LadybugNMEAGPZDA dataGPZDA;
   
   /** GPVTG data structure */
   LadybugNMEAGPVTG dataGPVTG;
   
   /** GPGLL data structure */
   LadybugNMEAGPGLL dataGPGLL;

   /** GPHDT data structure */
   LadybugNMEAGPHDT dataGPHDT;

   /** PRDID data structure */
   LadybugNMEAPRDID dataPRDID;
   
   /** Reserved space  */
   unsigned int ulReserved[256];
   
} LadybugNMEAGPSData;

/** 
 * @defgroup GPSContextCreationMethods GPS Context Creation and Initialization Methods
 *
 * This group of functions provides control over GPS functionality.
 */

/*@{*/ 

/**
 * Creates a new GPS context.
 * 
 * A context must be created for a GPS device that is going to
 * be used. This must be done before any other GPS API function calls can 
 * be made.
 *
 * This function sets the context to NULL if it is unsuccessful.
 *
 * The default port number, baud rate and GPS update time interval values
 * are set to the values defined by DEFAULT_PORT_NUMBER, DEFAULT_BAUDRATE
 * and DEFAULT_UPDATE_INTERVAL respectively.
 *
 * @param pContext - A pointer to a LadybugGPSContext to fill with the created
 *                   context.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugDestroyGPSContext()
 */
LADYBUGDLL_API LadybugError 
ladybugCreateGPSContext( LadybugGPSContext* pContext );

/**
 * Destroys a GPS context.
 *
 * Frees memory associated with the LadybugGPSContext. This should be called 
 * when your application stops using the context.
 *
 * This function sets the context to NULL if successful.
 *
 * @param pContext - A pointer to the LadybugGPSContext to destroy.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugCreateGPSContext()
 */
LADYBUGDLL_API LadybugError 
ladybugDestroyGPSContext( LadybugGPSContext* pContext );

/**
 * Registers the GPS context with a Ladybug context. 
 *
 * The images returned by ladybugGrabImage() and ladybugLockNext() will have
 * GPS data included if this function and ladybugStartGPS() have been called. 
 *
 * @param context     - The LadybugContext to register the GPS context with.
 * @param pGPSContext - A pointer to a GPS context created with a call to 
 *                      ladybugCreateGPSContext().        
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see   ladybugUnregisterGPS(), ladybugStartGPS(), ladybugGrabImage(), ladybugLockNext()
 */
LADYBUGDLL_API LadybugError
ladybugRegisterGPS( 
             LadybugContext      context,
             LadybugGPSContext*  pGPSContext );

/**
 * Unregisters a GPS context that has been registered with a Ladybug context.
 *
 * This function should be called before the LadybugGPSContext is
 * destroyed by ladybugDestroyGPSContext().
 *
 * Images returned by ladybugGrabImage() and ladybugLockNext() will not have
 * GPS data included after this function is called. 
 *
 * @param context     - The LadybugContext to unregister the GPS context with.
 * @param pGPSContext - A pointer to a GPS context created with a call to 
 *                      ladybugCreateGPSContext().        
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugRegisterGPS()
 */
LADYBUGDLL_API LadybugError
ladybugUnregisterGPS( 
             LadybugContext      context,
             LadybugGPSContext*  pGPSContext );

/**
 * Sets the serial port number, baud rate and data update rate for the 
 * GPS device for Windows.
 *
 * This function only sets the GPS parameters without actually starting the
 * the GPS device. To start the GPS device, call ladybugStartGPS(). 
 *
 * The value of uiUpdateTimeInterval depends on the NMEA data update rate of
 * the GPS device. For example, if the GPS device data update rate is 4 Hz,
 * then uiUpdateTimeInterval should be set to no less  than 250 ms.
 *
 * If uiUpdateTimeInterval is less than 1 ms or more, LADYBUG_INVALID_ARGUMENT is
 * returned.
 *
 * @param context              - The LadybugGPSContext to access.
 * @param uiPort               - The number of the serial port of the GPS device. 
 * @param uiBaud               - Baud rate of the serial port.
 * @param uiUpdateTimeInterval - The GPS data update time interval in milliseconds.
 *                               The default value is 1000 milliseconds.                                
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugInitializeGPSEx(), ladybugQueryGPSStatus(), ladybugStartGPS()
 */
LADYBUGDLL_API LadybugError
ladybugInitializeGPS( 
    LadybugGPSContext context,
    unsigned int      uiPort,
    unsigned int      uiBaud,
    unsigned int      uiUpdateTimeInterval = 1000 );

/**
 * Sets the serial port number, baud rate and data update rate for the 
 * GPS device on Linux platform.
 *
 * This function only sets the GPS parameters without actually starting the
 * the GPS device. To start the GPS device, call ladybugStartGPS(). 
 *
 * The value of uiUpdateTimeInterval depends on the NMEA data update rate of
 * the GPS device. For example, if the GPS device data update rate is 4 Hz,
 * then uiUpdateTimeInterval should be set to no less  than 250 ms.
 *
 * If uiUpdateTimeInterval is less than 1 ms or more, LADYBUG_INVALID_ARGUMENT is
 * returned.
 *
 * @param context              - The LadybugGPSContext to access.
 * @param deviceName           - The device name to access (e.g. /dev/ttyACM0)
 * @param uiBaud               - Baud rate of the serial port.
 * @param uiUpdateTimeInterval - The GPS data update time interval in milliseconds.
 *                               The default value is 1000 milliseconds.                                
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugInitializeGPS(), ladybugQueryGPSStatus(), ladybugStartGPS()
 */
LADYBUGDLL_API LadybugError
ladybugInitializeGPSEx( 
    LadybugGPSContext context,
    const char* deviceName,
    unsigned int uiBaud,
    unsigned int uiUpdateTimeInterval = 1000 );

/**
 * Starts grabbing GPS data from the GPS device.
 *
 * This function starts grabbing GPS data using the parameters set by
 * ladybugInitializeGPS(). If ladybugInitializeGPS() is not called, the
 * default parameters set by ladybugCreateGPSContext() are used.
 *
 * @param context - The LadybugGPSContext to access.                              
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see   ladybugCreateGPSContext(), ladybugInitializeGPS(), ladybugStopGPS()
 */
LADYBUGDLL_API LadybugError
ladybugStartGPS( LadybugGPSContext context );

/**
 * Stops the GPS device.
 *
 * @param context - The LadybugGPSContext to access.                              
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugStartGPS()
 */
LADYBUGDLL_API LadybugError
ladybugStopGPS( LadybugGPSContext context );

/*@}*/ 

/** 
 * @defgroup GPSDataRetrievalMethods GPS Device Data Retrieval Methods
 *
 * This group of functions is used to access GPS status and data from 
 * the GPS Device when using the Ladybug camera.
 */

/*@{*/ 

/**
 * Queries the current GPS status.
 *
 * @param context              - The LadybugGPSContext to access.   
 * @param pbGPSDeviceStarted   - Whether or not the GPS device is started.
 * @param puiStartedPortNumber - The port number of the GPS device.
 * @param pbGPSDataIsAvailable - Whether or not GPS data is available.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugInitializeGPS()
 */
LADYBUGDLL_API LadybugError
ladybugQueryGPSStatus( LadybugGPSContext  context,
             bool*               pbGPSDeviceStarted,
             unsigned int*       puiStartedPortNumber,
             bool*               pbGPSDataIsAvailable );

/**
 * Gets all the GPS NMEA sentences from the GPS device.
 *
 * If the returned value pointed by puiLength is 0, this means that there are
 * no NMEA sentences available. 
 *
 * The recommended buffer size is between 600 bytes and 1024 bytes. 
 *
 * @param context      - The LadybugGPSContext to access.   
 * @param pBuffer      - The pointer to the buffer to receive GPS sentences.
 * @param uiBufferSize - The size of pBuffer.
 * @param puiLength    - Total size of the NMEA sentences received (in bytes).
 *
 * @return   A LadybugError indicating the success of the function.
 */
LADYBUGDLL_API LadybugError
ladybugGetGPSNMEASentences( 
            LadybugGPSContext    context,
            unsigned char*       pBuffer,
            unsigned int         uiBufferSize,
            unsigned int*        puiLength);

/**
 * Gets the specified NMEA sentence from the GPS device.
 *
 * To get GPGGA data from a GPS device registered with the LadybugGPSContext
 * pointed to by context, use the following function call, where 
 * pNMEAGGA_Data is a pointer to a LadybugNMEAGPGGA structure:   
 *
 * ladybugGetGPSNMEAData(m_context, "GPGGA", pNMEAGGA_Data);
 *
 * @param context           - The LadybugGPSContext to access.   
 * @param pszNMEASentenceId - A string specifying the NMEA sentence to get.
 * @param pNMEADataBuffer   - The pointer to the data structure to receive GPS data.
 *                            The structure should match the NMEA sentence 
 *                            specified above by pszNMEASentenceId.
 *
 * @return   A LadybugError indicating the success of the function.
 */
LADYBUGDLL_API LadybugError
ladybugGetGPSNMEAData( 
             LadybugGPSContext    context,
             char*                pszNMEASentenceId,
             void*                pNMEADataBuffer );

/**
 * Gets all GPS data from the GPS device that is currently running.
 *
 * @param context   - The LadybugGPSContext to access.   
 * @param pNMEAData - A pointer to a LadybugNMEAGPSData structure to receive all the
 *                    NMEA data.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugGetGPSNMEAData()
 */
LADYBUGDLL_API LadybugError
ladybugGetAllGPSNMEAData( 
             LadybugGPSContext    context,
             LadybugNMEAGPSData*  pNMEAData );

/*@}*/ 

/** 
 * @defgroup GPSImageDataAccessorMethods GPS Image Data Accessor Methods
 *
 * This group of functions is used to access GPS data from a Ladybug image.
 * These functions do not require a GPS context to be used.
 */

/*@{*/ 

/**
 *
 * Gets all the GPS NMEA sentences from a Ladybug image.
 *
 * If the returned value pointed by puiLength is 0, this means that there are
 * no NMEA sentences available. 
 *
 * The recommended buffer size is between 600 bytes and 1024 bytes. 
 *
 * @param pImage       - A pointer to the raw image in which GPS NMEA sentences are
 *                       included.
 * @param pBuffer      - A  pointer to a buffer to receive the NMEA sentences.
 * @param uiBufferSize - The size of the specified buffer.
 * @param puiLength    - Total size of the GPS NMEA sentences received (in bytes).
 *
 * @return   A LadybugError indicating the success of the function.
 */
LADYBUGDLL_API LadybugError
ladybugGetGPSNMEASentencesFromImage( 
            const LadybugImage*  pImage,
            unsigned char*       pBuffer,
            unsigned int         uiBufferSize,
            unsigned int*        puiLength);

/**
 * Gets the GPS data for the specified NMEA sentences from a Ladybug image.
 *
 * To get GPGGA data from a Ladybug image pointed to by pImage, use the
 * following function call, where pNMEAGGA_Data is a pointer to a 
 * LadybugNMEAGPGGA structure:
 *
 * ladybugGetGPSNMEAData( m_context, "GPGGA", pNMEAGGA_Data );
 *
 * @param pImage            - A pointer to the raw image in which GPS NMEA sentences are
 *                            included.
 * @param pszNMEASentenceId - A string specifying the NMEA sentence to get.
 * @param pNMEADataBuffer   - The pointer to the data structure to receive GPS data.
 *                            The structure should match the NMEA sentence 
 *                            specified above by pszNMEASentenceId.
 *
 * @return A LadybugError indicating the success of the function.
 */
LADYBUGDLL_API LadybugError
ladybugGetGPSNMEADataFromImage( 
            const LadybugImage*  pImage,
            char const*          pszNMEASentenceId,
            void*                pNMEADataBuffer );

/**
 * Gets all GPS data from a Ladybug image.
 *
 * @param pImage    - A pointer to the raw image in which GPS NMEA sentences are
 *                    included.
 * @param pNMEAData - A pointer to a LadybugNMEAGPSData structure to receive all the
 *                    NMEA data.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugGetGPSNMEAData()
 */
LADYBUGDLL_API LadybugError
ladybugGetAllGPSNMEADataFromImage( 
          const LadybugImage*  pImage,
          LadybugNMEAGPSData*  pNMEAData );

/*@}*/

/*@}*/

#ifdef __cplusplus
};
#endif

#endif // #ifndef __LADYBUGGPS_H__
