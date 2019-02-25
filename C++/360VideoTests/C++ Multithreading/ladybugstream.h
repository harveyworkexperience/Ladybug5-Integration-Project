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

#ifndef __LADYBUGSTREAM_H__
#define __LADYBUGSTREAM_H__

/** 
 * @defgroup Ladybugstream ladybugstream.h
 * 
 *  ladybugstream.h
 *
 *  This file defines Ladybug stream writing and reading API functions.
 *
 *  Please view the Ladybug Stream File Specification document for 
 *  more information about Ladybug stream.
 *
 *  We welcome your bug reports, suggestions, and comments: 
 *  www.ptgrey.com/support/contact
 */

/*@{*/

#ifdef __cplusplus
extern "C"
{
#endif

#include "ladybug.h"

// Forward declaration    
struct LadybugFullAdjustmentParameters;

/** @defgroup Structures Structures */

/*@{*/ 

/**
 * Ladybug stream file header format.
 *
 * The stream file data sequence is detailed below:
 * - File signature ( "PGRLADYBUGSTREAM" )     
 * - Stream Header ( LadybugStreamHeadInfo )
 * - Camera Configuration Data               
 * - Image 1 Data                           
 * - Image 2 Data                           
 * - ...                                  
 * - Image N Data                          
 * - GPS Summary Data   
 *
 * For ulOffsetTable, the offset value is relative to the beginning of the
 * stream file.
 *
 * For example, if ulIncrement=50, 
 * ulOffsetTable[ 512 ] is the location of the first image.
 * ulOffsetTable[ 512 - 1] is the location of the 51st image,
 * ulOffsetTable[ 512 - 2] is the location of the 101st image and so on.                   
 *
 * For detailed information about Ladybug stream file format, 
 * see the topic 'Stream File Format' in the SDK Help.
 */
typedef struct LadybugStreamHeadInfo
{
   /** Ladybug stream file version number. */
   unsigned int ulLadybugStreamVersion;

   /** 
    * The compressor frame rate. Use the new frameRate entry below for any
    * stream files with ulLadybugStreamVersion >= 7.
    */
   unsigned int ulFrameRate;

   /** 
    * Base unit serial number for the Ladybug2 camera or ealier model.
    * For the Ladybug3 camera, it is assigned the same number as serialHead. 
    */
   LadybugSerialNumber serialBase;

   /** Camera serial number. */
   LadybugSerialNumber serialHead;

   /** Reserved. */
   unsigned int reserved[25];

   /** 
    * The size of the padding data block at the end of each recorded image.
    * The padding block is added for each image so that the total size of 
    * the recorded image data is in integer multiples of the hard disk's sector
    * size. 
    */
   unsigned int ulPaddingSize;

   /** Data format of the image. */
   LadybugDataFormat dataFormat;

   /** The resolution of the image. */
   LadybugResolution resolution; 

   /** The Bayer pattern of the stream. */
   LadybugStippledFormat stippledFormat;

   /** The size of the configuration data in bytes. */
   unsigned int ulConfigurationDataSize;
 
   /** The number of images stored in the stream file. (Not the number of the entire stream.) */
   unsigned int ulNumberOfImages;

   /** Number of index entries of ulOffsetTable[] used in the stream file. */
   unsigned int ulNumberOfKeyIndex;

   /** Incremental interval value for the indices. */
   unsigned int ulIncrement;
   
   /** Offset of the first image data. */
   unsigned int ulStreamDataOffset;

   /** Offset of the GPS summary data block. */
   unsigned int ulGPSDataOffset;

   /** Size in bytes of the GPS data block. */
   unsigned int ulGPSDataSize;

   /** 
    *  Size of internal frame header. 
    *  Frame headers are always a multiple of the sector size during recording.
    *  0 means a frame header is not present. 
    */
   unsigned int ulFrameHeaderSize;

   /** Whether humidity readings are available for this stream. */
   bool isHumidityAvailable;

   /** Minimum value of humidity sensor. */
   float humidityMin;

   /** Maximum value of humidity sensor. */
   float humidityMax;

   /** Whether air pressure readings are available for this stream. */
   bool isAirPressureAvailable;

   /** Minimum value of air pressure sensor. */
   float airPressureMin;

   /** Maximum value of air pressure sensor. */
   float airPressureMax;

   /** Whether compass readings are available for this stream. */
   bool isCompassAvailable;

   /** Minimum value of compass sensor. */
   float compassMin;

   /** Maximum value of compass sensor. */
   float compassMax;

   /** Whether accelerometer readings are available for this stream. */
   bool isAccelerometerAvailable;

   /** Minimum value of accelerometer sensor. */
   float accelerometerMin;

   /** Maximum value of accelerometer sensor. */
   float accelerometerMax;

   /** Whether gyroscope readings are available for this stream. */
   bool isGyroscopeAvailable;

   /** Minimum value of gyroscope sensor. */
   float gyroscopeMin;

   /** Maximum value of gyroscope sensor. */
   float gyroscopeMax;

   /** 
    * Actual frame rate, represented as a floating point value. 
    * This represents the actual frame rate as compared to ulFrameRate,
    * which does not handle non-integer frame rates like 3.75 fps.
    */
   float frameRate;

   /** Reserved. */
   unsigned char reservedSpace[780];

   /** Image offset index table. */
   unsigned int ulOffsetTable[512];

} LadybugStreamHeadInfo;

/**
 * Ladybug stream file GPS data output file type.
 *
 * This structure defines the types of output for 
 * ladybugWriteGPSSummaryDataToFile().
 */
typedef enum LadybugGPSFileType
{
   /** Text file. */
   LADYBUG_GPS_TXT,

   /** HTML file. */
   LADYBUG_GPS_HTML,

   /** KML File. */
   LADYBUG_GPS_KML,

   /** Unused member to force this enumeration to compile to 32 bits. */
   LADYBUG_GPS_FILE_TYPE_FORCE_QUADLET = 0x7FFFFFFF,

} LadybugGPSFileType;

/**
 *  The Ladybug stream context. To access stream-specific methods such as stream
 *  reading and writing, a LadybugStreamContext must be created using 
 *  ladybugCreateStreamContext(). Once created, the context is passed to be
 *  initialized either for reading or writing using
 *  ladybugInitializeStreamForReading() or ladybugInitializeStreamForWriting()
 *  respectively.
 */
typedef void* LadybugStreamContext;

/*@}*/ 

/** 
 * @defgroup ContextInitializationMethods Context Initialization Methods
 *
 * These functions are used 
 * to create and destroy stream contexts. 
 */

/*@{*/

/**
 * Creates a new Ladybug stream context.
 * This function must be called prior to other stream-related 
 * functions. A stream context can either be used to read from a stream OR
 * write to a stream, but not both simultaneously.
 *
 * Prior to writing images to a stream file, 
 * ladybugInitializeStreamForWriting() must be called to initialize the  
 * stream. To read images from a stream file, the application must call 
 * ladybugInitializeStreamForReading() to initialize the stream.
 *
 * This function will set the context to NULL if it is unsuccessful.
 *
 * @param pContext - A pointer to a LadybugStreamContext.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see   ladybugDestroyStreamContext()
 */
LADYBUGDLL_API LadybugError
ladybugCreateStreamContext( 
    LadybugStreamContext* pContext ); 

/**
 * Destroys a Ladybug stream context.
 *
 * Frees memory associated with the LadybugStreamContext. This function   
 * should be called when your application stops using the stream.
 *
 * @param pContext - A pointer to a LadybugStreamContext to destroy.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugCreateStreamContext()
 */
LADYBUGDLL_API LadybugError 
ladybugDestroyStreamContext( 
    LadybugStreamContext* pContext );

/**
 * Closes the current stream.
 *
 * This function should be called when your application stops writing
 * or reading.
 *
 * @param context - The LadybugStreamContext to access.
 *
 * @return A LadybugError indicating the success of the function.
 */
LADYBUGDLL_API LadybugError
ladybugStopStream( 
    LadybugStreamContext context );

/*@}*/ 

/** 
 * @defgroup StreamWritingMethods Stream Writing Methods
 *
 * These functions are used to write Ladybug stream files. 
 */

/*@{*/ 

/**
 * Initializes the stream context and opens the first stream file for writing.
 *
 * The initialized stream is used for recording images from a camera that
 * has been initialized with a Ladybug context. This function automatically
 * loads the configuration file from the camera.
 * 
 * The image stream is recorded to a set of files. The size of each file is
 * limited to 2GB. The Ladybug library names the recorded stream files 
 * automatically according to the name specified by pszBaseFileName. 
 * For example, if pszBaseFileName is "c:\\ladybug\\myStream" or 
 * "c:\\ladybug\\myStream.pgr", the recorded files will be named
 * "c:\\ladybug\\myStream-000000.pgr", "c:\\ladybug\myStream-000001.pgr", 
 * "c:\\ladybug\\myStream-000002.pgr", etc.. Here, "c:\\ladybug\myStream" 
 * is the base name of this stream.
 * 
 * If the first stream file name exists, the Ladybug library automatically
 * increases the first three digits of the numbers in the file name to 
 * find another available name. For example, if "myStream-000000.pgr" 
 * exists, "myStream-001000.pgr" will be used as the first
 * stream file name. The stream will be recorded to "myStream-001000.pgr",
 * "myStream-001001.pgr", "myStream-001002.pgr", etc.
 *
 * If pszFileNameOpened is not NULL, this call will return the name of the
 * first stream file at the location pointed by this pointer. The caller must
 * allocate the memory needed for the file name.
 *
 * For more information about Ladybug stream file format, see the topic 
 * 'Stream File Format' in the SDK Help.
 *
 * @param context           - The LadybugStreamContext to access.
 * @param pszBaseFileName   - Base name of the stream file.
 * @param cameraContext     - A LadybugContext created for a camera.
 * @param pszFileNameOpened - Location to return the name of the first stream file 
 *                            that is actually opened by this call.
 * @param bAsync            - A flag indicating if the image recording operation is 
 *                            synchronous or asynchronous. The default value is
 *                            false. If true, the image recording operation
 *                            is asynchronous and a call to ladybugWriteImageToStream()
 *                            returns immediately. If false, the image recording                             
 *                            operation is synchronous and a call to 
 *                            ladybugWriteImageToStream() is blocked until
 *                            the image recording operation completes. 
 *
 * @return A LadybugError indicating the success of the function. If this 
 *   function returns LADYBUG_ALREADY_INITIALIZED, this means that the
 *   context has already been initialized for writing. 
 *   To initialize this stream context for writing to a different stream,
 *   ladybugStopStream() must first be called to close the current stream.
 *   If the stream context is already initialized for reading, this 
 *   function will return LADYBUG_ALREADY_INITIALIZED_READING.
 *
 * @see ladybugWriteImageToStream()
 * @see ladybugInitializeStreamForWritingEx()
 * @see ladybugInitializeStreamForReading()
 */
LADYBUGDLL_API LadybugError
ladybugInitializeStreamForWriting( 
    LadybugStreamContext   context,
    const char*            pszBaseFileName, 
    LadybugContext         cameraContext,
    char*                  pszFileNameOpened = NULL,
    bool                   bAsync = false );

/**
 * Initializes the stream context and opens the first stream file for writing.
 *
 * This function is similar to ladybugInitializeStreamForWriting(). It 
 * initializes the stream context and opens the first stream file for
 * writing. This function is used to initialize a stream with a specified
 * LadybugStreamHeadInfo and configuration file name.
 *
 * The following members of the LadybugStreamHeadInfo structure need to be
 * initialized manually: ulFrameRate, serialBase, dataFormat, resolution
 * and stippledFormat. 
 *
 * @param context           - The LadybugStreamContext to access.
 * @param pszBaseFileName   - Name of the stream file.
 * @param streamInfo        - Pointer to a LadybugStreamHeadInfo structure.
 * @param pszConfigFilePath - Path to the config file of the Ladybug.
 * @param bAsync            - A flag indicating if the stream recording operation is 
 *                            synchronous or asynchronous. The default value is false. 
 *                            If true, the stream recording operation is asynchronous 
 *                            and a call to ladybugWriteImageToStream() returns 
 *                            immediately. If false, the stream recording operation 
 *                            is synchronous and a call to ladybugWriteImageToStream() 
 *                            is blocked until the image is actually written to 
 *                            the stream. The default value is false. 
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugWriteImageToStream(), ladybugInitializeStreamForWriting()
 */
LADYBUGDLL_API LadybugError
ladybugInitializeStreamForWritingEx( 
    LadybugStreamContext   context,
    const char*            pszBaseFileName, 
    LadybugStreamHeadInfo* streamInfo,
    const char*            pszConfigFilePath,
    bool                   bAsync );

/**
 * Writes a Ladybug image to the stream.
 *
 * If the stream recording is initialized as asynchronous:
 *   This function puts the image in image buffers and returns immediately.
 *   A copy of the buffer is made so it is safe to unlock the image immediately
 *   after this function returns. If an error is returned, the last one or two
 *   images written by this function may be lost. When ladybugStopStream() is
 *   called, all the buffered images are flushed.
 * 
 * If the stream recording is initialized as synchronous:
 *   This function is blocked until the image has been written to the stream.
 *
 * If there is less than 2GB free disk space, the function will return
 * LADYBUG_ERROR_DISK_NOT_ENOUGH_SPACE.
 *
 * @param context            - The LadybugStreamContext to access.
 * @param pimage             - A pointer to a LadybugImage to be written to disk.
 * @param pdBytesWritten     - A pointer to a double to contain the total number of
 *                             bytes written (in MB), including this frame.
 * @param pulNumOfImgWritten - A pointer to an unsigned long to return the number of 
 *                             images written so far, including this frame.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugInitializeStreamForWriting(),
 *   ladybugInitializeStreamForWritingEx(),
 *   ladybugWriteImageToStreamWithAdjustment(),
 *   ladybugStopStream()
 */
LADYBUGDLL_API LadybugError
ladybugWriteImageToStream( 
    LadybugStreamContext   context,
    const LadybugImage*    pimage,
    double*                pdBytesWritten = NULL,
    unsigned long*         pulNumOfImgWritten = NULL );

/**
 * Writes a Ladybug image to the stream, including adjustment parameters
 * for post-processing. This function is only valid for images that
 * require post-processing.
 *
 * This function performs identically to ladybugWriteImageToStream(),
 * with the exception of the additional parameter for adjustment.
 *
 * @param context               - The LadybugStreamContext to access.
 * @param pimage                - A pointer to a LadybugImage to be written to disk.
 * @param adjustmentParameters - Adjustment parameters for post-processing.
 * @param pdBytesWritten        - A pointer to a double to contain the total number of
 *                                bytes written (in MB), including this frame.
 * @param pulNumOfImgWritten    - A pointer to an unsigned long to return the number of 
 *                                images written so far, including this frame.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugWriteImageToStream()
 */
LADYBUGDLL_API LadybugError
ladybugWriteImageToStreamWithAdjustment( 
    LadybugStreamContext context,
    const LadybugImage* pimage,
    const LadybugFullAdjustmentParameters* adjustmentParameters,
    double* pdBytesWritten = NULL,
    unsigned long* pulNumOfImgWritten = NULL );

/**
* Writes a sequence of nmea sentences to the associated image. 
* Maximum length of the sentences is 1024 bytes.
*
* @param context                - The LadybugContext to access.
* @param image                  - A pointer to a LadybugImage.
* @param adjustmentParameters   - Adjustment parameters for post-processing.
* @param nmeaSentences          - The nmea sentences to be added to the image.
*
* @return A LadybugError indicating the success of the function.
*
*/
LADYBUGDLL_API LadybugError
ladybugWriteGPSDataToImage(
    LadybugContext context,
    LadybugImage* image,
    const char* nmeaSentences,
    const size_t sentenceSize);

/*@}*/ 

/** 
 * @defgroup StreamReadingMethods Stream Reading Methods
 *
 * These functions are used 
 * to read Ladybug stream files. 
 */

/*@{*/ 

/**
 * Initializes the context and opens the stream file(s) for reading.
 * Sets the current reading position to the first image in the stream.
 *
 * If an error is detected in the stream file, such as a corrupted header
 * due to power failure, this function returns LADYBUG_CORRUPTED_PGR_STREAM.
 *
 * The Ladybug library reads all the stream files that share the same base
 * name as specified by pszStreamFileName. The last three digits of the 
 * name of the first opened stream file are always "000".
 *
 * For streams that require post-processing (e.g. Ladybug5 12-bit streams),
 * this function will attempt to open the matching XML file that contains
 * the post-processing settings. For example, if the stream filename is
 * "myStream-000001.pgr", then the XML file to be loaded is named 
 * "myStream-000.xml".

 * If a XML file is not present, it will be automatically created for the 
 * entire stream, with each image containing default settings.
 *
 * Example:
 *   If pszStreamFileName is "c:\\ladybug\\myStream-000002.pgr" or
 *   "c:\\ladybug\\myStream-000010.pgr", the library will initialize and 
 *   read file "c:\\ladybug\\myStream-000000.pgr" and all the files that 
 *   share the same base name "c:\\ladybug\\myStream-000":
 *   - c:\\ladybug\\myStream-000000.pgr,
 *   - c:\\ladybug\\myStream-000001.pgr,
 *   - c:\\ladybug\\myStream-000002.pgr,
 *   - ...
 *   - c:\\ladybug\\myStream-000030.pgr,
 *   If applicable, the XML file will be c:\\ladybug\\myStream-000.xml.
 *
 * If an application calls ladybugInitializeStreamForReading(context, 
 * "d:\\ladybug\\lb2-003002.pgr"), the following stream files will be
 * initialized for reading:
 * - d:\\ladybug\\lb2-003000.pgr,
 * - d:\\ladybug\\lb2-003001.pgr,
 * - d:\\ladybug\\lb2-003002.pgr,
 * If applicable, the XML file will be d:\\ladybug\\lb2-003.xml.
 *
 * For more information about Ladybug stream file format, see the topic 'Stream File 
 * Format'.
 *
 * @param  context           - The LadybugStreamContext to access.
 * @param  pszStreamFileName - One of the names of the stream to be initialized.
 * @param  bAsync            - A flag indicating if the stream reading operation is
 *                             synchronous or asynchronous. If true, the stream 
 *                             reading operation is asynchronous and the Ladybug 
 *                             library uses internal buffers to speed up the image 
 *                             reading operation. ladybugReadImageFromStream()
 *                             may return the image immediately if the
 *                             requested image is already in the buffer. 
 *                             The default value is false.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugReadImageFromStream(), ladybugInitializeStreamForWriting()
 */
LADYBUGDLL_API LadybugError
ladybugInitializeStreamForReading( 
    LadybugStreamContext   context,
    const char*            pszStreamFileName,
    bool                   bAsync = false );

/**
 * Gets the information of the currently-opened stream file.
 *
 * The information is returned from the header of each stream file.
 * This function also resets the current reading position to the first image. 
 *
 * @param context               - The LadybugStreamContext to access.
 * @param pStreamHeaderInfo     - A pointer to a LadybugStreamHeadInfo structure.
 * @param pszAssociatedFileName - A pointer to a string to contain the name of the
 *                                stream file. Use NULL if this value is not
 *                                required.
 *
 * @return   A LadybugError indicating the success of the function.
 */
LADYBUGDLL_API LadybugError
ladybugGetStreamHeader(
    LadybugStreamContext   context,
    LadybugStreamHeadInfo* pStreamHeaderInfo,
    char*                  pszAssociatedFileName = NULL );

/**
 * Gets the total number of images in a stream. 
 *
 * The number of images returned is the total number of images in all of the
 * stream files that share the same base stream file name.
 * This function also resets the current reading position to the first image. 
 *
 * @param  context   - The LadybugStreamContext to access.
 * @param  puiImages - A pointer to an unsigned int to store the number of images.
 *
 * @return A LadybugError indicating the success of the function.
 */
LADYBUGDLL_API LadybugError
ladybugGetStreamNumOfImages(
    LadybugStreamContext   context,
    unsigned int*      puiImages );

/**
 * Sets the current reading position to the specified image in the stream.
 *
 * If the stream reading is initialized as asynchronous, the Ladybug library 
 * starts reading the current image from the image buffer automatically
 * so that a subsequent call to ladybugReadImageFromStream() can return
 * the image immediately.
 *
 * @param context    - The LadybugStreamContext to access.
 * @param uiImageNum - The index of the image to go to. The index for the first
 *                      image is zero.
 *
 * @return   A LadybugError indicating the success of the function.
 *   If uiImageNum is greater than the total number of images in the stream, 
 *   LADYBUG_INVALID_ARGUMENT will be returned.
 *
 * @see ladybugReadImageFromStream(), ladybugGetStreamNumOfImages()
 */
LADYBUGDLL_API LadybugError
ladybugGoToImage(
    LadybugStreamContext    context,
    unsigned int            uiImageNum );

/**
 * Reads the image that is located at the current reading position of the
 * stream and sets the reading position to the next image.
 *
 * If the stream reading is initialized as asynchronous, this function may
 * return the image immediately if the requested image is already in the 
 * buffer. If the stream reading is initialized as synchronous, this 
 * function starts reading and returns the image.
 *
 * @param context       - The LadybugStreamContext to access.
 * @param pLadybugImage - Pointer to a LadybugImage structure to store the image.
 *
 * @return   A LadybugError indicating the success of the function.
 *
 * @see ladybugInitializeStreamForReading(), ladybugGoToImage()
 */
LADYBUGDLL_API LadybugError
ladybugReadImageFromStream(
    LadybugStreamContext context,
    LadybugImage* pLadybugImage );

/**
 * Identical to ladybugReadImageFromStream(), but also returns the relevant
 * adjustment parameters associated with the image.
 *
 * Additional settings associated with the image are also returned:
 * - Tone mapping
 * - Sharpening
 * - False color removal
 *
 * @param context               - The LadybugStreamContext to access.
 * @param pLadybugImage         - Pointer to a LadybugImage structure to store the image.
 * @param adjustmentParameters - Pointer to a LadybugAdjustmentParameters structure to be filled.
 *
 * @return   A LadybugError indicating the success of the function.
 *
 * @see ladybugInitializeStreamForReading(), ladybugGoToImage()
 */
LADYBUGDLL_API LadybugError
ladybugReadImageFromStreamWithAdjustment(
    LadybugStreamContext context,
    LadybugImage* pLadybugImage,
    LadybugFullAdjustmentParameters* adjustmentParameters);

/**
 * Reads the configuration data from the stream and writes it to a 
 * configuration file.
 *
 * @param  context           - The LadybugStreamContext to access.
 * @param  pszConfigFileName - The file name of the configuration file. 
 *
 * @return   A LadybugError indicating the success of the function.
 *   This function also resets the current reading position to the first image. 
 */
LADYBUGDLL_API LadybugError
ladybugGetStreamConfigFile(
    LadybugStreamContext context,
    const char*          pszConfigFileName );

/**
 * Writes the GPS summary data from a stream to a specified file.
 *
 * This function outputs GPS data from all stream files with the same base
 * file name.
 * After calling this function successfully, the frame number is reset to 0.
 *
 * uiWidth and uiHeight are only used for file type LADYBUG_GPS_HTML.
 *
 * In order to publish the HTML file that contains GoogleMaps, a valid API 
 * key is necessary. See http: *code.google.com/apis/maps/signup.html for
 * more information.
 *
 * Since Ladybug SDK version 1.5.1.4, Google Maps JavaScript API V3
 * is used. The pszGoogleMapsAPIKey parameter is ignored.
 *
 * @param  context             - The LadybugStreamContext to access.
 * @param  pszTxtFileName      - The file name of the configuration file. 
 * @param  outputFileType      - The output file type. (See LadybugGPSFileType)
 * @param  uiWidth             - The Google Maps width in the HTML file.
 * @param  uiHeight            - The Google Maps height in the HTML file.
 * @param  pszGoogleMapsAPIKey - The Google Maps API key string.
 *
 * @return   A LadybugError indicating the success of the function.
 */
LADYBUGDLL_API LadybugError
ladybugWriteGPSSummaryDataToFile(
    LadybugStreamContext context,
    const char*          pszTxtFileName,
    LadybugGPSFileType   outputFileType,
    unsigned int         uiWidth = 600, 
    unsigned int         uiHeight = 400,
    const char*          pszGoogleMapsAPIKey = NULL);

/**
 * Replaces a adjustment parameter for the specified image. This may be 
 * useful if there is a need to change the adjustment parameters for an
 * image after recording.
 *
 * @param context - The LadybugStreamContext to access.
 * @param uiImageNum - Index of the image to set the new parameters to
 * @param ajustmentParameters - New parameters to be set
 *
 * @see ladybugInitializeStreamForReading()
 */
LADYBUGDLL_API LadybugError
ladybugReplaceAdjustmentParameterForImage(
    LadybugStreamContext context,
    unsigned int uiImageNum,
    const LadybugFullAdjustmentParameters& adjustmentParameters);

/**
 * Saves all outstanding adjustment parameters that have not been
 * written to disk.
 *
 * This is only applicable for stream reading contexts.
 * Adjustment parameters will be automatically written to disk
 * during stream writing.
 *
 * @param context - The LadybugStreamContext to access.
 */
LADYBUGDLL_API LadybugError
ladybugSaveModifiedAdjustmentParameters(LadybugStreamContext context);
                     
/*@}*/

/*@}*/

#ifdef __cplusplus
};
#endif

#endif  // #ifndef __LADYBUGSTREAM_H__
