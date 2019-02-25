//=============================================================================
// Copyright � 2017 FLIR Integrated Imaging Solutions, Inc. All Rights Reserved.
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

/** 
 * @defgroup Ladybug_cs Ladybug.cs
 *
 * Ladybug.cs
 *
 * This file defines the interface that C# programs need to use Ladybug SDK.
 * This file must be added to your C# project.
 */

/*@{*/

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace LadybugAPI
{
    /**
     * @defgroup ManagedEnumerations Enumerations
     * 
     * @ingroup Ladybug_cs
     */

    /*@{*/ 

    /** An enumeration of all possible errors returned by the Ladybug API. */
    public enum LadybugError
    {
        /** Function completed successfully. */
        LADYBUG_OK,

        /** General failure. */
        LADYBUG_FAILED,

        /** Invalid argument passed to the function. */
        LADYBUG_INVALID_ARGUMENT,

        /** Invalid context passed to the function. */
        LADYBUG_INVALID_CONTEXT,

        /** The functionality is not implemented with this version of the library. */
        LADYBUG_NOT_IMPLEMENTED,

        /**
         * The functionality is not supported with the current software or hardware
         * configurations.
         */
        LADYBUG_NOT_SUPPORTED,

        /** The device or context has already been initialized. */
        LADYBUG_ALREADY_INITIALIZED,

        /** Grabbing has already been started. */
        LADYBUG_ALREADY_STARTED,

        /** Failed to open file. */
        LADYBUG_COULD_NOT_OPEN_FILE,

        /** Memory allocation error. */
        LADYBUG_MEMORY_ALLOC_ERROR,

        /** There is not enough space on the disk. */
        LADYBUG_ERROR_DISK_NOT_ENOUGH_SPACE,

        /** Stream file has not opened. */
        LADYBUG_STREAM_FILE_NOT_OPENED,

        /** Invalid stream file name. */
        LADYBUG_INVALID_STREAM_FILE_NAME,

        /** Device or object not initialized. */
        LADYBUG_NOT_INITIALIZED,

        /** Camera has not been started. */
        LADYBUG_NOT_STARTED,

        /** Request would exceed maximum bandwidth of the bus. */
        LADYBUG_MAX_BANDWIDTH_EXCEEDED,

        /** Invalid video mode or frame rate passed or retrieved. */
        LADYBUG_INVALID_VIDEO_SETTING,

        /** The rectify resolution has not been properly set. */
        LADYBUG_NEED_RECTIFY_RESOLUTION,

        /** Function is deprecated - please see documentation. */
        LADYBUG_DEPRECATED,

        /**
         * The image buffer returned by the camera was too small to contain all of
         * the JPEG image data.
         */
        LADYBUG_IMAGE_TOO_SMALL,

        /** Operation timed out. */
        LADYBUG_TIMEOUT,

        /** Too many image buffers are locked by the user. */
        LADYBUG_TOO_MANY_LOCKED_BUFFERS,

        /** No calibration file was found on the Ladybug head unit. */
        LADYBUG_CALIBRATION_FILE_NOT_FOUND,

        /** An error occurred during JPEG decompression. */
        LADYBUG_JPEG_ERROR,

        /** An error occurred in JPEG header verification. */
        LADYBUG_JPEG_HEADER_ERROR,

        /** The compressor did not have enough time to finish compressing the data. */
        LADYBUG_JPEG_INCOMPLETE_COMPRESSION,

        /** There is no image in this frame. */
        LADYBUG_JPEG_NO_IMAGE,

        /** The compressor detected a corrupted image. */
        LADYBUG_CORRUPTED_IMAGE_DATA,

        /** An error occurred in off-screen buffer initialization. */
        LADYBUG_OFFSCREEN_BUFFER_INIT_ERROR,

        /** Unsupported framebuffer format. */
        LADYBUG_FRAMEBUFFER_UNSUPPORTED_FORMAT,

        /** Framebuffer incomplete. */
        LADYBUG_FRAMEBUFFER_INCOMPLETE,

        /** GPS device could not be started. */
        LADYBUG_GPS_COULD_NOT_BE_STARTED,

        /** GPS has not been started. */
        LADYBUG_GPS_NOT_STARTED,

        /** No GPS data. */
        LADYBUG_GPS_NO_DATA,

        /** No GPS data for this sentence. */
        LADYBUG_GPS_NO_DATA_FOR_THIS_SENTENCE,

        /** GPS communication port may be in use. */
        LADYBUG_GPS_COMM_PORT_IN_USE,

        /** GPS communication port does not exist. */
        LADYBUG_GPS_COMM_PORT_DOES_NOT_EXIST,

        /** OpenGL display list has not initialized. */
        LADYBUG_OPENGL_DISPLAYLIST_NOT_INITIALIZED,

        /** OpenGL image texture has not updated. */
        LADYBUG_OPENGL_TEXTUREIMAGE_NOT_UPDATED,

        /** OpenGL device context is invalid. */
        LADYBUG_INVALID_OPENGL_DEVICE_CONTEXT,

        /** OpenGL rendering context is invalid. */
        LADYBUG_INVALID_OPENGL_RENDERING_CONTEXT,

        /** OpenGL texture is invalid. */
        LADYBUG_INVALID_OPENGL_TEXTURE,

        /** The requested OpenGL operation is not valid. */
        LADYBUG_INVALID_OPENGL_OPERATION,

        /** There are not enough resources available for image texture. */
        LADYBUG_NOT_ENOUGH_RESOURCE_FOR_OPENGL_TEXTURE,

        /**
         * The current rendering context failed to share the display-list space
         * of another rendering context
         */
        LADYBUG_SHARING_DISPLAYLIST_FAILED,

        /** The specified off-screen image is invalid. */
        LADYBUG_INVALID_OFFSCREEN_BUFFER_SIZE,

        /** The requested job is still on-going. */
        LADYBUG_STILL_WORKING,

        /** The PGR stream is corrupted and cannot be corrected. */
        LADYBUG_CORRUPTED_PGR_STREAM,

        /** The driver and runtime version may be mismatched. */
        LADYBUG_GPU_CUDA_DRIVER_ERROR,

        /** There is no device supporting CUDA. */
        LADYBUG_NO_CUDA_DEVICE,

        /** An error occurred in GPU functions. */
        LADYBUG_GPU_ERROR,

        /** Low level failure */
        LADYBUG_LOW_LEVEL_FAILURE,

        /** Register failure */
        LADYBUG_REGISTER_FAILED,

        /** Isoch-related failure */
        LADYBUG_ISOCH_FAILED,

        /** Buffer retrieval failure */
        LADYBUG_RETRIEVE_BUFFER_FAILED,

        /** Image library failure */
        LADYBUG_IMAGE_LIBRARY_FAILED,

        /** Busmaster-related failure */
        LADYBUG_BUS_MASTER_FAILED,

        /** Unknown error. */
        LADYBUG_ERROR_UNKNOWN,

        /** Voltage error (eg. power cable is not connected on LD5) */
        LADYBUG_BAD_VOLTAGE,

        /** Interface error (eg. USB2 instead of USB3 on LD5) */
        LADYBUG_BAD_INTERFACE,

        /** Overexposure was detected */
        LADYBUG_OVEREXPOSED,

        /** Reported if an internal inconsistency is detected */
        LADYBUG_INTERNAL_ERROR,

        /** There is no overlap at the point of interest */
        LADYBUG_NO_OVERLAP,

        /** The configuration file is invalid (eg. File is missing data, corrupted, or outdated) */
        LADYBUG_INVALID_CONFIG_FILE,

        /** Number of errors */
        LADYBUG_NUM_LADYBUG_ERRORS,

        /** Unused member. */
        LADYBUG_ERROR_FORCE_QUADLET = 0x7FFFFFFF,
    };

    /** An enumeration used to describe the maximum bus speed. */
    public enum LadybugBusSpeed
    {
        LADYBUG_S100,
        LADYBUG_S200,
        LADYBUG_S400,
        LADYBUG_S800,
        LADYBUG_S1600,
        LADYBUG_S3200,
        LADYBUG_S_FASTEST,
        LADYBUG_SPEED_UNKNOWN = -1,
        LADYBUG_SPEED_FORCE_QUADLET = 0x7FFFFFFF,
    };

    /** An enumeration used to describe the interface type. */
    public enum LadybugInterfaceType
    {
        LADYBUG_INTERFACE_IEEE1394,
        LADYBUG_INTERFACE_USB2,
        LADYBUG_INTERFACE_USB3,
        LADYBUG_INTERFACE_UNKNOWN,
        LADYBUG_INTERFACE_FORCE_QUADLET = 0x7FFFFFFF,
    };

    /**
     * An enumeration of the different camera properties for the Ladybug.
     *
     * Many of these properties are included only for completeness and future
     * expandability, and will have no effect on a Ladybug camera.
     */
    public enum LadybugProperty
    {
        LADYBUG_BRIGHTNESS, /**< The brightness property. */
        LADYBUG_AUTO_EXPOSURE, /**< The auto exposure property. */
        LADYBUG_SHARPNESS, /**< The sharpness property. Not supported */
        LADYBUG_WHITE_BALANCE, /**< The white balance property. */
        LADYBUG_HUE, /**< The hue property. Not supported */
        LADYBUG_SATURATION, /**< The saturation property. Not supported */
        LADYBUG_GAMMA, /**< The gamma property. */
        LADYBUG_IRIS, /**< The iris property. Not supported */
        LADYBUG_FOCUS, /**< The focus property. Not supported */
        LADYBUG_ZOOM, /**< The zoom property. Not supported */
        LADYBUG_PAN, /**< The pan property. Not supported */
        LADYBUG_TILT, /**< The tilt property. Not supported */
        LADYBUG_SHUTTER, /**< The shutter property. */
        LADYBUG_GAIN, /**< The gain property. */
        LADYBUG_FRAME_RATE, /**< The camera heads frame rate */
        LADYBUG_PROPERTY_FORCE_QUADLET = 0x7FFFFFFF, /**< Unused member. */
    };

    /**
     * The independent properties provide control over each of the
     * individual camera units.
     */
    public enum LadybugIndependentProperty
    {
        LADYBUG_SUB_GAIN, /**< Per-camera gain settings. */
        LADYBUG_SUB_SHUTTER, /**< Per-camera shutter settings. */
        LADYBUG_SUB_AUTO_EXPOSURE, /**< Per-camera auto exposure settings as well as "cameras of interest". */
        LADYBUG_SUB_FORCE_QUADLET = 0x7FFFFFFF, /**< Unused member. */
    };

    /**
     * Bit positions for API functions requiring a camera selection bit field, 
     * such as ladybugSetIndProperty().
     */
    public enum LadybugCameraBits
    {  
        LADYBUG_UNIT_0 = (0x1 << 0), /**< Camera Unit 0 */
        LADYBUG_UNIT_1 = (0x1 << 1), /**< Camera Unit 0 */
        LADYBUG_UNIT_2 = (0x1 << 2), /**< Camera Unit 0 */
        LADYBUG_UNIT_3 = (0x1 << 3), /**< Camera Unit 0 */
        LADYBUG_UNIT_4 = (0x1 << 4), /**< Camera Unit 0 */
        LADYBUG_UNIT_5 = (0x1 << 5), /**< Camera Unit 0 */
        LADYBUG_ALL_UNITS = 0xFF /**< All cameras. */
    };

    /**
     * Possible data formats returned by the Ladybug library. Please consult
     * the Ladybug technical reference to determine which data formats are
     * supported by each camera model.
     */
    public enum LadybugDataFormat
    {
        /**
         * This format produces a single image buffer that has each sensor's image
         * one after the other. Again, each pixel is in its raw 8bpp format.
         */
        LADYBUG_DATAFORMAT_RAW8 = 1,

        /**
         * This format is similar to LADYBUG_DATAFORMAT_RAW8 except that the entire
         * buffer is JPEG compressed.  This format is intended for use with cameras
         * that have black and white sensors.
         */
        LADYBUG_DATAFORMAT_JPEG8,

        /**
         * This format separates each individual image into its 4 individual Bayer
         * channels (Green, Red, Blue and Green - not necessarily in that order).
         */
        LADYBUG_DATAFORMAT_COLOR_SEP_RAW8,

        /**
         * Similar to LADYBUG_DATAFORMAT_COLOR_SEP_RAW8 except that the
         * transmitted buffer is JPEG compressed.
         */
        LADYBUG_DATAFORMAT_COLOR_SEP_JPEG8,

        /**
         * Similar to LADYBUG_DATAFORMAT_RAW8.
         * The height of the image is only half of that in LADYBUG_DATAFORMAT_RAW8 format.
         */
        LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW8,

        /**
         * Similar to LADYBUG_DATAFORMAT_COLOR_SEP_JPEG8.
         * The height of the image is only half of the original image.
         * This format is only supported by Ladybug3.
         */
        LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG8,

        /**
         * This format produces a single image buffer that has each sensor's image
         * one after the other. Each pixel is in raw 16bpp format.
         */
        LADYBUG_DATAFORMAT_RAW16,

        /**
         * Similar to LADYBUG_DATAFORMAT_COLOR_SEP_JPEG8 except that the image
         * data is 12bit JPEG compressed.
         */
        LADYBUG_DATAFORMAT_COLOR_SEP_JPEG12,

        /**
         * Similar to LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW8.
         * Each pixel is in raw 16bpp format.
         */
        LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW16,

        /**
         * Similar to LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG8 except that
         * the image data is 12bit JPEG compressed.
         */
        LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG12,

        /**
         * This format produces a single image buffer that has each sensor's image
         * one after the other. Each pixel is in raw 12bpp format.
         *
         * The image data is laid out as follows (24 bytes / 2 pixels):
         * Px1 (top 8 bytes) | Px2 (top 8 bytes) | Px1 (bottom 4 bytes) | Px2 (bottom 4 bytes)
         */
        LADYBUG_DATAFORMAT_RAW12,

        /**
         * Similar to LADYBUG_DATAFORMAT_RAW12.
         * The height of the image is only half of that in LADYBUG_DATAFORMAT_RAW12 format.
         */
        LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW12,

        /** The number of possible data formats. */
        LADYBUG_NUM_DATAFORMATS,

        /** Hook for "any usable video mode". */
        LADYBUG_DATAFORMAT_ANY,

        /** Unused member. */
        LADYBUG_DATAFORMAT_FORCE_QUADLET = 0x7FFFFFFF,
    };

    /**
     * This enumeration describes the raw per-sensor resolutions returned by
     * the camera.
     *
     * This enumeration is not used to represent the resolution of the actual image.
     *
     * LADYBUG_RESOLUTION_ANY can be used to work with any camera.
     */
    public enum LadybugResolution
    {
        LADYBUG_RESOLUTION_1024x768 = 4, /**< 1024x768 pixels. Ladybug2 camera. */
        LADYBUG_RESOLUTION_1616x1232 = 8, /**< 1616x1232 pixels. Ladybug3 camera.  */
        LADYBUG_RESOLUTION_2448x2048 = 9, /**< 2448x2048 pixels. Ladybug5 camera.  */
        LADYBUG_RESOLUTION_2464x2048 = 12, /**< 2464x2048 pixels. Ladybug5P camera.  */
        LADYBUG_NUM_RESOLUTIONS = 10, /**< Number of possible resolutions. */
        LADYBUG_RESOLUTION_ANY = 11, /**< Hook for any usable resolution. */
        LADYBUG_RESOLUTION_FORCE_QUADLET = 0x7FFFFFFF, /**< Unused member. */
    };

/** The available color processing/destippling/demosaicing methods. */
    public enum LadybugColorProcessingMethod
    {
        /**
        * Disable color processing - This is useful for retrieving the
        * original bayer patten image. When the image is the color-separated
        * JPEG stream, the JPEG data is decompressed and the 4 color-separated
        * channels are combined into one bayer image.
        */
        LADYBUG_DISABLE,

        /**
         * Edge sensing de-mosaicing - This is the most accurate method
         * that can keep up with the camera's frame rate.
         */
        LADYBUG_EDGE_SENSING,

        /**
         * Nearest neighbour de-mosaicing (fast) - Faster, less accurate than
         * nearest neighbor de-mosaicing.
         */
        LADYBUG_NEAREST_NEIGHBOR_FAST,

        /**
         *  Rigorous de-mosaicing - This provides the second best quality colour
         *  reproduction.  This method is very processor intensive and may
         *  not keep up with the camera's frame rate.  Best used for
         *  offline processing where accurate colour reproduction is required.
         */
        LADYBUG_RIGOROUS,

        /**
         * Downsample4 mode - Color process to output a half width and half height
         * image. This allows for faster previews and processing. This results in
         * an output image that is 1/4 the size of the source image.
         */
        LADYBUG_DOWNSAMPLE4,

        /**
         * Downsample16 mode - Color process to output a quarter width and quarter
         * height image. This allows for faster previews and processing. This
         * results in an image that is 1/16th the size of the source image.
         */
        LADYBUG_DOWNSAMPLE16,

        /**
         * Mono - This processing method only uses the green color channel to
         * generate grey scale Ladybug images. It is designed for fast previews of
         * compressed JPEG image streams. This method also downsamples the image
         * as in LADYBUG_DOWNSAMPLE4 so the result image is quarter size.
         */
        LADYBUG_MONO,

        /**
         * High quality linear interpolation - This algorithm provides similar
         * results to Rigorous, but is up to 30 times faster.
         */
        LADYBUG_HQLINEAR,

        /**
         * High quality linear interpolation - This algorithm is the same with
         * LADYBUG_HQLINEAR, but the color processing is performed on GPU.
         */
        LADYBUG_HQLINEAR_GPU,

        /**
         * A de-mosaicking algorithm based on the directional filter - This should
         * give the best image quality.
         */
        LADYBUG_DIRECTIONAL_FILTER,

        /**
         * A demosaicking algorithm that weights different directions properly during 
         * green interpolation - This yields the best image quality with least false 
         * colors among all implemented algorithms.
         */
        LADYBUG_WEIGHTED_DIRECTIONAL_FILTER,

        /** Unused member. */
        LADYBUG_COLOR_FORCE_QUADLET = 0x7FFFFFFF,
    };

    /**
     * The format of stippled pixels (Bayer pattern.)
     *
     * The four characters correspond to the top left 2x2 grid of pixels.
     * For example, a "BGGR" image has row 0 = BGBGBGBGBG... and
     * row 1 = GRGRGRGR...
     */
    public enum LadybugStippledFormat
    {
        LADYBUG_BGGR, /**< BGGR image. */
        LADYBUG_GBRG, /**< GBRG image. */
        LADYBUG_GRBG, /**< GRBG image. */
        LADYBUG_RGGB, /**< RGGB image. */
        LADYBUG_DEFAULT, /**< Default stippled format for the camera. */
        LADYBUG_STIPPLED_FORCE_QUADLET = 0x7FFFFFFF, /**< Unused member. */
    };

    /**
     * An enumeration used to indicate the pixel format of an image.
     * This is used for ladybugRenderOffscreenImage() and LadybugProcessedImage.
     */
    public enum LadybugPixelFormat
    {
        LADYBUG_MONO8 = 0x00000001, /**< 8 bit mono */
        LADYBUG_MONO16 = 0x00000020, /**< 16 bit mono */
        LADYBUG_RAW8 = 0x00000200, /**< 8 bit raw data */
        LADYBUG_RAW16 = 0x00000400, /**< 16 bit raw data */
        LADYBUG_BGR = 0x10000001, /**< 24 bit BGR */
        LADYBUG_BGRU = 0x10000002, /**< 32 bit BGRU */
        LADYBUG_BGR16 = 0x10000004, /**< 48 bit BGR (16 bit int per channel) */
        LADYBUG_BGRU16 = 0x10000008, /**< 64 bit BGRU (16 bit int per channel) */
        LADYBUG_BGR16F = 0x10000010, /**< 48 bit BGR (16 bit float per channel) */
        LADYBUG_BGR32F = 0x10000020, /**< 96 bit BGR (32 bit float per channel) */
        LADYBUG_PIXELFORMAT_FORCE_QUADLET = 0x7FFFFFFF, /**< Unused member. */
        LADYBUG_UNSPECIFIED_PIXEL_FORMAT = 0, /**< Unspecified pixel format. */
    };

    /**
     *   File format for saved images.
     *
     * Remarks:
     *   Not all of these file formats are compatible with all functions that take
     *   a LadybugSaveFileFormat argument.
     */
    public enum LadybugSaveFileFormat
    {
        LADYBUG_FILEFORMAT_PGM, /**< 8-bit greyscale .PGM */
        LADYBUG_FILEFORMAT_PPM, /**< 24 bit .PPM */
        LADYBUG_FILEFORMAT_BMP, /**< 24 bit .BMP */
        LADYBUG_FILEFORMAT_JPG, /**< JPEG image */
        LADYBUG_FILEFORMAT_PNG, /**< PNG image */
        LADYBUG_FILEFORMAT_TIFF, /**< TIFF image */
        LADYBUG_FILEFORMAT_EXIF, /**< EXIF image, GPS information is stored in EXIF tags present in LadybugProcessedImage. */
        LADYBUG_FILEFORMAT_HDR, /**< HDR (Radiance) format */
        LADYBUG_FILEFORMAT_FORCE_QUADLET = 0x7FFFFFFF, /**< Unused member. */
    };

    /** The type of device the driver is talking to. */
    public enum LadybugDeviceType
    {
        LADYBUG_DEVICE_LADYBUG,
        LADYBUG_DEVICE_COMPRESSOR,
        LADYBUG_DEVICE_LADYBUG3,
        LADYBUG_DEVICE_LADYBUG5,
        LADYBUG_DEVICE_LADYBUG5P,
        LADYBUG_DEVICE_UNKNOWN,
        LADYBUG_DEVICE_FORCE_QUADLET = 0x7FFFFFFF,
    };

    /** Auto shutter modes supported by the camera. */
    public enum LadybugAutoShutterRange
    {
        LADYBUG_AUTO_SHUTTER_DRIVE_HIGHWAY,
        LADYBUG_AUTO_SHUTTER_DRIVE_CITY,
        LADYBUG_AUTO_SHUTTER_INDOOR,
        LADYBUG_AUTO_SHUTTER_LOW_NOISE,
        LADYBUG_AUTO_SHUTTER_CUSTOM,
        LADYBUG_AUTO_SHUTTER_FORCE_QUADLET = 0x7FFFFFFF
    };

    /**
     * Regions of interest used for auto exposure calculation.
     * This can be applied to both the camera and the post processing pipeline.
     */
    public enum LadybugAutoExposureRoi
    {
        /** Use the full image (default). */
        LADYBUG_AUTO_EXPOSURE_ROI_FULL_IMAGE,

        /** 
         * Use the bottom 50% of each image, excluding the top camera. 
         * Prioritizes detail in the lower half of the image (i.e. ignoring the sky).
         */
        LADYBUG_AUTO_EXPOSURE_ROI_BOTTOM_50,

        /** 
         * Use the top 50% of each image, including the top camera.
         * Prioritizes detail in the top half of the image.
         * Ideal for situations where cameras are mounted upside down.
         */
        LADYBUG_AUTO_EXPOSURE_ROI_TOP_50,

        /** The number of values defined in LadybugAutoExposureRoi. */
        LADYBUG_AUTO_EXPOSURE_ROI_SIZE,

        /** Unused member. */
        LADYBUG_AUTO_EXPOSURE_ROI_FORCE_QUADLET = 0x7FFFFFFF
    };

    /**
     * Tone mapping models available. The default is to have no tone mapping enabled.
     *
     * The Ladybug library provides 2 implementations of tone mapping, an OpenGL
     * implementation as well as a CPU implementation.
     *
     * The OpenGL implementation is available to all data formats.
     *
     * The CPU implementation is only available for images that are not adjusted
     * on the camera. See LadybugImageAdjustment.h for more details on
     * the data formats that are supported.
     *
     * The OpenGL implementation is very fast, but produces poorer results as
     * compared to the CPU implementation.
     *
     * The conversion used for the OpenGL tone mapping is based on Reinhard's
     * tone mapping operator. It requires OpenGL version 2.0 or later which is not
     * provided by the software renderer (see ladybugEnableSoftwareRendering()).
     *
     * Testing indicates that OpenGL tone mapping may produce unexpected results
     * on ATI graphics cards that support OpenGL version 3.0 or earlier.
     */
    public enum LadybugToneMappingMode
    {
        LADYBUG_TONE_MAPPING_NONE,
        LADYBUG_TONE_MAPPING_OPENGL,
        LADYBUG_TONE_MAPPING_CPU,

        /** The number of values defined in LadybugToneMappingMode */
        LADYBUG_TONE_MAPPING_MODE_SIZE,

        /** Unused member. */
        LADYBUG_TONE_MAPPING_FORCE_QUADLET = 0x7FFFFFFF
    };

    /**
     * Optional ladybug context features
     */
    public enum LadybugContextFeature
    {
        /**
         * Corresponds to GPU acceleration.  Calling ladybugGetContextFeatureSupport() may return:
         *     LADYBUG_FEATURE_SUPPORT_OK - GPU acceleration is fully supported
         *     LADYBUG_FEATURE_SUPPORT_OLD_GPU_DRIVER - GPU acceleration is not supported due to old driver
         *     LADYBUG_FEATURE_SUPPORT_NOT_SUPPORTED - GPU acceleration is not supported
         */
        LADYBUG_FEATURE_GPU_ACCELERATION,

        LADYBUG_NUM_FEATURES /**< The number of supported ladybug features */
    };

    /**
     * Defines that status of a LadybugContextFeature
     */
    public enum LadybugFeatureStatus
    {
        LADYBUG_FEATURE_STATUS_OK,
        LADYBUG_FEATURE_STATUS_NOT_SUPPORTED,
        LADYBUG_FEATURE_STATUS_OLD_GPU_DRIVER,
        LADYBUG_NUM_FEATURE_STATUSES
    };


    /*@}*/

    /** 
     * @defgroup ManagedStructures Structures
     * 
     * @ingroup Ladybug_cs
     */

    /*@{*/ 

    /**
     * This structure defines the format by which time is represented in the
     * Ladybug SDK.
     *
     * The ulSeconds and ulMicroSeconds values represent the
     * absolute system time when the image was captured.
     *
     * The ulCycleSeconds and ulCycleCount are higher-precision values that
     * have either been propagated up from the 1394 / USB3 bus or extracted from
     * the image itself. The data will be extracted from the image if image
     * timestamping is enabled and directly (and less accurately) from the
     * 1394 / USB3 bus otherwise.
     *
     * The ulCycleSeconds value will wrap around after 128 seconds. The
     * ulCycleCount represents the 1/8000 second component.
     */
    public struct LadybugTimestamp
    {
        // The number of seconds since the epoch.
        public long ulSeconds;
        // The microseconds component.
        public uint ulMicroSeconds;
        // The cycle time seconds.  0-127.
        public uint ulCycleSeconds;
        // The cycle time count.  0-7999 (1/8000ths of a second.)
        public uint ulCycleCount;
        // The cycle offset.  0-3071 (1/3072ths of a cycle count.)
        public uint ulCycleOffset;
    };

    /**
     * Structure containing the image information of the captured images.
     *
     * There are 3 main uses for this structure:
     *
     * - It is stored to the hard drive as part of each image set at capture time.
     * - It is used in LadybugImage to store image information when each
     *   image is captured.
     * - It is used in LadybugImage to store image information when each
     *   image is read from a .pgr stream file.
     *
     * This image info structure will only contain valid information if
     * Ladybug2 and 3 : Camera is set in any JPEG mode
     * Ladybug5: Always valid.
     *
     * Camera properties stored here follow the IIDC register specifications. This is a serialized struct.
     */

    unsafe public struct LadybugImageInfo
    {
        // Constant fingerprint, should be LADYBUGIMAGEINFO_STRUCT_FINGERPRINT.
        public uint ulFingerprint;
        // Structure version number, should be 0x00000002.
        public uint ulVersion;
        // Timestamp, in seconds, since the UNIX time epoch.
        // If it is 0, all the data in LadybugImageInfo are invalid.
        public uint ulTimeSeconds;
        // Microsecond fraction of above second.
        public uint ulTimeMicroSeconds;
        // Sequence number of the image.  Reset to zero when the head powers up
        //  and incremented for every image.
        public uint ulSequenceId;
        // Horizontal refresh rate. (reserved)
        public uint ulHRate;
        // Actual adjusted gains used by each of the 6 cameras.  Similar to the
        //  DCAM gain register.
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public uint[] arulGainAdjust;
        // A copy of the DCAM whitebalance register.
        public uint ulWhiteBalance;
        // This is the same as register 0x1044, described in the PGR IEEE 1394
        //  Register Reference.
        public uint ulBayerGain;
        // This is the same as register 0x1040, described in the PGR IEEE 1394
        //  Register Reference.
        public uint ulBayerMap;
        // A copy of the Brightness DCAM register.
        public uint ulBrightness;
        // A copy of the Gamma DCAM register.
        public uint ulGamma;
        // The serial number of the Ladybug head.
        public uint ulSerialNum;
        // Shutter values for each sensor.  Similar to the DCAM shutter register.
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public uint[] ulShutter;
        // GPS fix quality, taken from GGA NMEA sentence Only supported on LD5+ 
        public uint ulGpsFixQuality;
        // Represents whether the PPS is enabled or not. Only supported on LD5+ 
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bPpsStatus;
        // Represents whether the GPS is enabled or not. Only supported on LD5+ 
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bGpsStatus; 
        // GPS Latitude, < 0 = South of Equator, > 0 = North of Equator.
        // If dGPSLatitude = LADYBUG_INVALID_GPS_DATA(defined in ladybugstream.h),
        // the data is invalid
        public double dGPSLatitude;
        // GPS Longitude, < 0 = West of Prime Meridian, > 0 = East of Prime Meridian.
        // If dGPSLongitude = LADYBUG_INVALID_GPS_DATA(defined in ladybugstream.h),
        // the data is invalid
        public double dGPSLongitude;
        // GPS Antenna Altitude above/below mean-sea-level (geoid) (in meters).
        // If dGPSAltitude = LADYBUG_INVALID_GPS_DATA(defined in ladybugstream.h),
        // the data is invalid
        public double dGPSAltitude;
		

    };

    /** Simple structure to hold information from a 3-axis sensor. */
    unsafe public struct LadybugTriplet
    {
        //float x;
        //float y;
        //float z;
    };

    /** Image information for this image */
    unsafe public struct LadybugImageHeader
    {
        public uint uiTemperature;
        public uint uiHumidity;
        public uint uiAirPressure;
        LadybugTriplet compass;
        LadybugTriplet accelerometer;
        LadybugTriplet gyroscope;

        [MarshalAsAttribute(UnmanagedType.I1)]
        bool needSoftwareAdjustment;
    };

    /**
     * In certain data formats, the camera may transmit the fully-masked pixels
     * at the edges of the sensor in addition to the useful image data.
     *
     * When available, this information is used to adjust the black level of
     * the image data during ladybugConvertImage().
     */
    unsafe public struct LadybugImageBorder
    {
        public uint uiTopRows; /**< Number of pixels of border on top of image */
        public uint uiBottomRows; /**< Number of pixels of border on bottom of image */
        public uint uiLeftCols; /**< Number of pixels of border on left of image */
        public uint uiRightCols; /**< Number of pixels of border on right of image */
    };

    /**
     * Specifies a contiguous region within an image.
     */
    unsafe public struct LadybugImageRegion
    {
        public uint uiLowRow;  /**< The lowest row included in the region */
        public uint uiHighRow; /**< The highest row included in the region */
        public uint uiLowCol;  /**< The lowest column included in the region */
        public uint uiHighCol; /**< The highest column included in the region */
    };

    /** The LadybugImage structure is used to describe the image captured from the camera. */
    unsafe public struct LadybugImage
    {
        // Columns, in pixels, of a single sensor image.
        public uint uiCols;

        // Rows, in pixels, of a single sensor image.
        public uint uiRows;

        /** Dimensions of the image border. */
        public LadybugImageBorder imageBorder;

        /**
        * Columns, in pixels, of the full sensor image. This is equal to
        * uiCols + imageBorder.uiLeft + imageBorder.uiRight.
        */
        public uint uiFullCols;

        /**
        * Rows, in pixels, of the full sensor image. This is equal to
        * uiRows + imageBorder.uiTop + imageBorder.uiBottom.
        */
        public uint uiFullRows;

        // The data format of associated image buffer contained in pData.
        public LadybugDataFormat dataFormat;

        // The per-sensor resolution of the returned image.
        public LadybugResolution resolution;

        // Timestamp of this image.
        public LadybugTimestamp timeStamp;

        // Image information for this image.
        public LadybugImageInfo imageInfo;

        /** Header **/
        //LadybugImageHeader imageHeader;

        // Pointer to the image data.  The format is defined by dataFormat.
        public byte* pData;

        // Indicates whether the raw image data is stippled or not.
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bStippled;

        /** Bayer pattern of image data. */
        //LadybugStippledFormat stippledFormat;

        // Real data size, in bytes, of the data pointed to by pData.  Useful for
        // non-constant sizes (JPEG images).
        public uint uiDataSizeBytes;

        // The internal buffer index that the image buffer corresponds to.
        // For functions that lock the image, this number must be passed back to
        // the "unlock" function.  If ladybugInitializePlus() was called, this
        // number corresponds to the position of the buffer in the buffer array
        // passed in.
        // If the image is from a .pgr stream file, uiBufferIndex is not used.
        public uint uiBufferIndex;

        // Reserved for future image information
        public fixed uint ulReserved[3];
    };

    /** Structure containing metadata for a Ladybug image. */
    unsafe public struct LadybugImageMetaData
    {
        // GPS data

        // Hour, in Coordinated Universal Time
        public byte ucGPSHour;
        // Minute, in Coordinated Universal Time
        public byte ucGPSMinute;
        // Second, in Coordinated Universal Time
        public byte ucGPSSecond;
        // Millisecond portion of the Second
        public ushort wGPSSubSecond;
        // Day
        public byte ucGPSDay;
        // Month
        public byte ucGPSMonth;
        // Year
        public ushort wGPSYear;
        // Latitude, <0 south of Equator, >0 north of Equator
        public double dGPSLatitude;
        // Longitude, <0 west of Prime Meridian, >0 east of Prime Meridian
        public double dGPSLongitude;
        // Antenna altitude.
        public double dGPSAltitude;
        // Ground speed, in kilometers per hour
        public double dGPSGroundSpeed;

        // Reserved.
        public fixed uint ulReserved[50];

    };

    /** Structure containing GpsTimeSync parameters */
    unsafe public struct GpsTimeSyncSettings
    {
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool enablePps;
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool enableGpsTimeSync;
        public uint baudRate;
    };
    
    /** The Ladybug processed image structure. */
    unsafe public struct LadybugProcessedImage
    {
        // Columns, in pixels, of the stitched image
        public uint uiCols;

        // Rows, in pixels, of the stitched image
        public uint uiRows;

        // Pointer to the image data
        public byte* pData;

        // The pixel format of this image
        public LadybugPixelFormat pixelFormat;

        // Metadata of the image.
        public LadybugImageMetaData metaData;

        // Reserved
        public uint ulReserved;
    };

    /** A record used in querying the camera properties. */
    [StructLayout(LayoutKind.Sequential)]
    unsafe public struct LadybugCameraInfo
    {
        public uint serialBase; /**< Base unit serial number. */
        public uint serialHead; /**< Camera serial number. */   

        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bIsColourCamera; /**< Indicates whether or not the camera is a colour camera. */

        public LadybugDeviceType deviceType; /**< Camera type. */

        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string pszModelName; /**< Model name string. */ 

        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string pszSensorInfo; /**< Sensor info string. */   

        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string pszVendorName; /**< Vendor name string. */

        public int iDCAMVer; /**< IEEE-1394 specification value. Value is version * 100. */
        public int iBusNum; /**< Low-level bus number. */
        public int iNodeNum; /**< Low-level node number. */

        public LadybugBusSpeed maxBusSpeed; /**<  Maximum bus speed. */

        public LadybugInterfaceType interfaceType; /**<  Interface type of camera. */
    };

        
    /** A camera trigger property. */
    unsafe public struct LadybugTriggerModeInfo
    {
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bPresent; /**< Presence of trigger mode */
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bReadOutSupported; /**< Whether or not the user can read values in the trigger functionality. */
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bOnOffSupported; /**< Whether or not the functionality can be turned on or off. */
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bPolaritySupported; /**< Whether or not the polarity can be changed. */
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bValueReadable; /**< Whether or not the raw trigger input can be read. */

        public uint uiSourceMask; /**< A bit field indicating which trigger sources are available. */

        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bSoftwareTriggerSupported; /**< Whether or not software triggering is available. */

        public uint uiModeMask; /**< A bit field indicating which trigger modes are available. */

    };

    /** A camera trigger. */
    unsafe public struct LadybugTriggerMode
    {
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bOnOff; /**< On off enabled/disabled */

        public uint uiPolarity; /**< The polarity of the trigger. 1 or 0. */
        public uint uiSource; /**< The trigger source. Corresponds to the source mask. Use 7 for software triggering. */
        public uint uiMode; /**< The trigger mode. Corresponds to the mode mask. */
        public uint uiParameter; /**< The (optional) parameter to the trigger function, if required. */

    };

    /** A camera strobe property. */
    unsafe public struct LadybugStrobeInfo
    {
        public uint uiSource; /**< Strobe source to be queried. */

        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bAvailable; /**< Indicates if strobe is supported. */
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bReadOutSupported; /**< Describes whether the source allows reading of the current value. */
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bOnOffSupported; /**< Describes whether the source can be turned on or off. */
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bPolaritySupported; /**< Describes whether the source's polarity can be changed. */

        public float fMinValue; /**< This parameter holds the minimum value of the delay and duration. */
        public float fMaxValue; /**< This parameter holds the maximum value of the delay and duration. */

    };

    /** A camera strobe. */
    unsafe public struct LadybugStrobeControl
    {
        public uint uiSource; /**< Strobe source to be set */

        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool bOnOff; /**< Describes whether to turn the strobe on or off. */

        public uint uiPolarity; /**< The polarity of the strobe. 1 or 0. */
        public float fDelay; /**< The delay of the strobe. */
        public float fDuration; /**< The duration of the strobe. */

    };

    /** A structure to control stabilization parameters. */
    unsafe public struct LadybugStabilizationParams
    {
        /**
         * The square of this number is used for the number of templates in the
         * image of one camera.
         *
         * A higher value produces more reliable results, at the expense of
         * processing speed.
         *
         * A range between 5 and 8 is preferable.
         */
        public int iNumTemplates;

        /**
         * Templates are searched in each image within the range specified by this
         * value, in pixels.
         *
         * If camera movement is significant and/or the frame rate is low, a higher
         * value should be specified. However, a higher value results in lower
         * processing speed.
         *
         * A range between 50 and 200 is preferable.
         */
        public int iMaximumSearchRegion;

        /**
         * Rotational differences accumulate over time in successive frames.
         *
         * To prevent these differences from accumulating, the rotation result can
         * fall back to an initial position. This value determines how gradually the
         * fall back, or 'decay,' should take place.
         *
         * A value between 0 and 1 must be specified. A value of 1 specifies no decay.
         *
         * A value between 0.9 and 1.0 is usually used.
         */
        public double dDecayRate;

        public fixed int reserved[28]; /**< Reserved field. Should not be used. */
    };

    /**
     * A structure to control the ranges of distances being searched
     * for dynamic stitching. If the distances to the subjects in the scene falls
     * within a known range, these distances can be set to avoid false matching.
     *
     * Limiting the range also improves the precision of search result.
     *
     * All values are specified in meters, with a minimum of 0.5m.
     */
    unsafe public struct LadybugDynamicStitchingParams
    {
        public double dMinimumDistance; /**< Minimum distance for search. Default is 2. */
        public double dMaximumDistance; /**< Maximum distance for search. Default is 100. */
        public double dDefaultDistance; /**< Default distance for search. Default is 20. */

        public fixed int reserved[26]; /**< Reserved field. Should not be used. */
    };

	/** Structure containing Ladybug image statistics data. */
	unsafe public struct LadybugImageStatistics
	{
		public enum Channel
		{
			GREY,
			RED,
			GREEN,
			BLUE,
			NUM_STATISTICS_CHANNELS
		};

		public struct ChannelData
		{
            [MarshalAsAttribute(UnmanagedType.I1)]
            public bool bValid; /**< Whether this channel has valid data.  */

			public uint uiRangeMin; /**< Minimum possible pixel value for this channel. */
			public uint uiRangeMax; /**< Maximum possible pixel value for this channel. */
			public uint uiPixelValueMin; /**< Minimum pixel value of the channel. */
			public uint uiPixelValueMax; /**< Maximum pixel value of the channel. */
			public float fPixelValueMean; /**< Mean value of the channel. */

			/**
			* Histogram array. Each value in the array contains the pixel
			* count for the corresponding pixel value. For example, if
			* histogram[1] is 100, that means there are 100 pixels in the image
			* with a pixel value of 1 for that channel.
			*/
			public fixed int uiHistogram[Ladybug.LADYBUG_HISTOGRAM_SIZE];

		};

        public struct TotalStatisticData
        {
            public ChannelData channelData1;
            public ChannelData channelData2;
            public ChannelData channelData3;
            public ChannelData channelData4;
        }

        public TotalStatisticData StatisticsData;
	};

    /** Structure containing color correction parameters. */
    unsafe public struct LadybugColorCorrectionParams
    {
        public int iHue; /**< The hue value. */
        public int iSaturation; /**< The saturation value. */
        public int iIntensity; /**< The intensity value. */
        public int iRed; /**< The red value */
        public int iGreen; /**< The green value */
        public int iBlue; /*<* The blue value */
        public fixed int reserved[25]; /**< Reserved field. Should not be used. */

    };

    /** Structure containing tone mapping parameters. */
    unsafe public struct LadybugToneMappingParams
    {
       /**
        * This value is only applicable for OpenGL tone mapping. It is ignored
        * in all other cases.
        *
        * This value determines how much compression is applied to the image.
        * This value must be between 0.1 and 40.0.
        */
        public double dCompressionScale;

       /**
        * This value is only applicable for OpenGL tone mapping. It is ignored
        * in all other cases.
        *
        * This value determines the size of the local area when calculating the
        * average intensity of a given pixel.
        * This value must be between 0 and 10. If the value is 0, the local average
        * is determined by the pixel itself, so it behaves as a global compression
        * operator.
        */
        public double dLocalAreaSize;

        /** Tone mapping mode to be used. */
        public LadybugToneMappingMode toneMappingMode;

        public fixed int reserved[27];
    };

    /*Informations returned by the LadybugConvertImage function*/
    unsafe public struct ConvertImageOutput
    {
        /** White balance values that were applied when converting. */
        public float gainRed_Value;
        public float gainBlue_Value;

        /** Gain in dB that was applied as part of the automatic exposure algorithm. */
        public float gainApplied;

        /** Whether the target exposure was obtained. */
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool targetMeanReach;

    };

    /*@}*/

    // This class defines static functions to access most of the
    // Ladybug APIs defined in ladybug.h
    unsafe public partial class Ladybug
    {        
        /** 
         * @defgroup ManagedGeneralFunctions General Functions 
         * 
         * @ingroup Ladybug_cs
         */

        /*@{*/

        public const string LADYBUG_DLL = "ladybug.dll";
        public const int LADYBUG_NUM_CAMERAS = 6;
        public const int LADYBUG_HISTOGRAM_SIZE = 256;

        /**
         * Returns a string describing the passed LadybugError. Need to use IntPtr not string
         * since the string should not be freed.
         * See: MSDN "Memory Management with the Interop Marshaler"
         * Note: You can use System.Runtime.InteropServices.Marshal.PtrToStringAnsi() to get a
         * managed string.
         *
         * @param errorCode - The LadybugError to convert.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugErrorToString", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ErrorToString(LadybugError errorCode);

        /**
         * Returns the version numbers of the ladybug library.
         *
         * @param context   - The LadybugContext to access.
         * @param major     - The major version number
         * @param minor     - The minor version number
         * @param type      - The version type (0-alpha, 1-beta, 2-release)
         * @param nuild     - The build number
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetLibraryVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetLibraryVersion(IntPtr context, out uint major, out uint minor, out uint type, out uint build);

        /**
         * Writes the specified image to disk. 
         *
         * If the file format is LADYBUG_FILEFORMAT_JPG, the JPEG compression quality
         * may be set by calling ladybugSetImageSavingJpegQuality().
         * 
         * If the file format is LADYBUG_FILEFORMAT_EXIF, the metadata in 
         * LadybugProcessedImage should be filled in properly.
         *
         * If async is not specified or specified as false, the image saving is 
         * performed synchronously. The function is blocked until the image saving
         * operation completes.
         *
         * If async is true, the image saving is performed asynchronously on a 
         * separate thread. The function returns immediately, before the operation 
         * completes. Note that when attempting to access the image file specified by 
         * path, the file does not appear on disk until the image saving operation 
         * completes.
         *
         * @param context           - The LadybugContext to access.
         * @param processedImage    - The image to save.
         * @param path              - The name of the file to write to.
         * @param format            - The file format to write to. See LadybugSaveFileFormat.
         * @param async             - A flag indicating if the image saving operation is 
         *                            synchronous or asynchronous. If true, the image saving 
         *                            operation is asynchronous and this function returns
         *                            immediately. The default value is false.
         *
         * @return A LadybugError indicating the success of the function.   
         *
         * @see ladybugSetImageSavingJpegQuality(), ladybugGetImageSavingJpegQuality()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSaveImage", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SaveImage(IntPtr context,
                           ref LadybugProcessedImage processedImage,
                           string path,
                           LadybugSaveFileFormat format,
                           bool async);

        /**
         * Sets the JPEG compression quality to use when saving JPEG images.
         *
         * @param context   - The LadybugContext to access.
         * @param quality   - The JPEG compression quality to use. 
         *                    The valid value range is between 1 and 100. Default is 85.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugSaveImage(), ladybugGetImageSavingJpegQuality()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetImageSavingJpegQuality", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetImageSavingJpegQuality(IntPtr context, int quality);

        /**
         *   Gets the JPEG compression quality being used when saving JPEG images.
         *
         * @param context   - The LadybugContext to access.
         * @param quality   - A pointer to the JPEG quality being used to save images.
         *
         * @return A LadybugError indicating the success of the function.   
         *
         * @see ladybugSaveImage(), ladybugSetImageSavingJpegQuality()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetImageSavingJpegQuality", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetImageSavingJpegQuality(IntPtr context, out int quality);

        /*@}*/

        /** 
         * @defgroup ManagedContextCreationMethods Context Creation and Initialization Methods 
         *
         * The functions contained within this group allow users
         * to create and initialize the camera context.  Generally speaking, these
         * functions need to be called prior to other functions in the library.
         * 
         * @ingroup Ladybug_cs
         */

        /*@{*/

        /**
         * Creates a new context for accessing the camera-specific functions of the
         * library. A context must be created for every camera that is going to be 
         * controlled. This must be done before any other function calls can be made.
         *
         * This function will set the context to NULL if it is unsuccessful.
         *
         * @param context - A pointer to a LadybugContext to fill with the created context.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugDestroyContext()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugCreateContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError CreateContext(out IntPtr context);

        /**
         * Frees memory associated with the LadybugContext. This function should be 
         * called when your application stops using the Ladybug API.
         *   
         * This function will set the context to NULL if successful.
         *
         * @param context - A pointer to the LadybugContext to destroy.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugCreateContext()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugDestroyContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError DestroyContext(ref IntPtr context);

        /**
         * Queries the context about optional features that it supports.
         *
         * @param pcontext - A pointer to the LadybugContext.
         * @param feature  - Specifies the feature of interest.
         * @param pStatus  - Will be populated with the status of the requested feature.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetContextFeatureSupport", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetContextFeatureSupport(IntPtr context, LadybugContextFeature feature, out LadybugFeatureStatus status);

        /**
         * Fills an array of LadybugCameraInfo structures with all of the
         * pertinent information from the attached cameras.
         *
         * If camera is not a Ladybug, the deviceType in the LadybugCameraInfo 
         * structure is set as LADYBUG_DEVICE_UNKNOWN.
         *
         * @param context   - The LadybugContext to access.
         * @param camInfo   - An array of LadybugCameraInfo structures, at least as
         *                    large as the number of Ladybug cameras on the bus.
         * @param size      - The size of the array passed in. The number of cameras
         *                    detected is passed back in this argument also.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugBusEnumerateCameras", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError BusEnumerateCameras(
                       IntPtr context,
                       [In, Out] LadybugCameraInfo[] camInfos,
                       ref uint size);

        /**
         * Initializes a Ladybug camera using a serial number.  
         *
         * This function, ladybugInitializePlus(), or ladybugInitializeFromIndex()
         * must be called before any of the ladybugStart commands.
         *
         * @param context      - The LadybugContext to access.
         * @param serialNumber - The serial number of the Ladybug to initialize.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugInitializeFromIndex(), ladybugInitializePlus()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugInitializeFromSerialNumber", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError InitializeFromSerialNumber(IntPtr context, int serialNumber);

        /**
         * Initialize a Ladybug camera using a device index.
         *
         * @param context  - The LadybugContext to access.
         * @param ulDevice - The device number of the Ladybug to initialize. Use 0 to 
         *                   initialize the first (or only) Ladybug on the bus.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugInitializePlus(), ladybugInitializeFromSerialNumber()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugInitializeFromIndex", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError InitializeFromIndex(IntPtr context, uint ulDevice);

        /**
         * Initialize a Ladybug camera using a device index. 
         * 
         * In addition to the same behvaior as ladybugInitializeFromIndex(), 
         * this function also allows the user to specify the number of buffers to use
         * as well as optionally allocate those buffers outside the library.
         *
         * @param context       - The LadybugContext to access.
         * @param nusIndex      - The device number of the Ladybug to initialize. Use 0 to 
         *                       initialize the first (or only) Ladybug on the bus.
         * @param numBuffers    - The number of buffers to expect or allocate. The 
         *                       minimum number of buffers is 2. The maximum  number 
         *                       of buffers is limited only by available memory.
         * @param buffer        - A pointer to the user-supplied buffer.  If this argument 
         *                       is NULL the library will allocate and free the buffers
         *                       internally, otherwise the caller is responsible for
         *                       allocation and deallocation.  
         *                       This buffer has to be one buffer that can hold the entire
         *                       images. Therefore, the size of the buffer has to be
         *                       size >= numBuffers * size_of_each_image.
         *                       No boundary checking is performed on these images,
         *                       so if you are supplying your own buffers, they must
         *                       be large enough to hold the largest image that is expected.                       
         * @param uiSize        - The size of the buffer. This is valid when pBuffer is not NULL.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugInitializeFromIndex(), ladybugInitializeFromSerialNumber()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugInitializePlus", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError InitializePlus(IntPtr context, uint busIndex, uint numBuffers, byte* buffer, uint size);

        /**
         * Writes the configuration information of the context to a file.
         *
         * @param context           - The LadybugContext to access.
         * @param configFileName    - The name of the configuration file. If this file
         *                            already exists, this function overwrites the existing
         *                            file. If pszConfigFileName is NULL, this function
         *                            returns an error.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugLoadConfig()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugWriteConfigurationFile", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError WriteConfigurationFile(IntPtr context, string configFilename);

        /**
         * Retrieves camera information for the given context.
         *
         * @param context - The LadybugContext to access.
         * @param info    - A pointer to a LadybugCameraInfo structure to be populated.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetCameraInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetCameraInfo(IntPtr context, ref LadybugCameraInfo info);

        /**
         * Sets the asynchronous and isochronous transmit and receive bus speeds.
         *
         * If only one of asyncBusSpeed or isochBusSpeed is required, set the other
         * parameter to LADYBUG_S_FASTEST.
         *
         * @param context       - The LadybugContext to access.
         * @param asyncBusSpeed - One of the available bus speeds, LADYBUG_S100, etc.
         * @param isochBusSpeed - One of the available bus speeds, LADYBUG_S100, etc.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetBusSpeed", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetBusSpeed(IntPtr context, LadybugBusSpeed asyncBusSpeed, LadybugBusSpeed isochBusSPeed);

        /**
         * Gets the current isochronous bus speed and the current asynchronous bus 
         * speed.
         *
         * @param context     - The LadybugContext to access.
         * @param pAsyncSpeed - The location to return current asynchronous bus speed
         * @param pIsochSpeed - The location to return current isochronous bus speed
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetBusSpeed", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetBusSpeed(IntPtr context, out LadybugBusSpeed asyncBusSpeed, out LadybugBusSpeed isochBusSPeed);

        /**
         * Gets the current data format that the camera is currently set to.
         * Setting the data format for the camera can be performed when starting the camera.
         *
         * @param context    - The LadybugContext to access.
         * @param dataFormat - The data format that the camera is currently set to.
         *
         * @see ladybugStart(), ladybugStartEx(), ladybugStartLockNext(), ladybugStartLockNextEx()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetDataFormat", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetDataFormat(IntPtr context, out LadybugDataFormat dataFormat);

        /*@}*/

        /** 
         * @defgroup ManagedCameraStartMethods Camera Start and Image Grab Methods 
         *
         * All of the functions in this group allow the user
         * to initiate data transmission from the camera and acquire images.
         * 
         * @ingroup Ladybug_cs
         */

        /*@{*/ 

        /**
         * Starts the camera with the specified data format.  
         *
         * This function must be called before ladybugGrabImage().
         *
         * @param context - The LadybugContext to access.
         * @param format  - The data format to start the camera in.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugStartEx(), ladybugGrabImage(), ladybugStop()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError Start(
            IntPtr context,
            LadybugDataFormat format);

        /**
         * Starts the camera with the specified data format, packet size and 
         * image buffer size.  
         * 
         * This function must be called before ladybugGrabImage(). 
         *
         * This function negotiates 1394 bandwidth between the camera and the PC. 
         * As there is a limited amount of bandwidth available on the bus, any 
         * bandwidth used limits what is available to other devices on the bus.
         * 
         * The bandwidth is negotiated based on two parameters:
         * - The number of bytes sent in a packet every 1394 cycle (8000 cycles/s)
         *   - 1394a = 4096
         *   - 1394b = 9792
         *   - USB3 = 32000 (equivalent)
         * - The total size of the buffer to be used for each image. The maximum
         *   size is the total number of pixels on all of the sensors.
         *   The larger the buffer size, the lower the maximum frame rate.
         *   - Ladybug2 = 1024 x 768 x 6 = 4718592
         *   - Ladybug3 = 1616 x 1232 x 6 = 11945472
         *   - Ladybug5 = 2448 x 2048 x 6 = 30081024
         *
         * In order to achieve the desired frame rate, the user may also have to
         * set two additional parameters:
         * - Set the frame rate on the head unit using the ladybugSetAbsProperty()
         *   function with the LADYBUG_FRAME_RATE argument in order to adjust the 
         *   number of images actually being sent by the camera.
         * - When using JPEG formats, set the auto JPEG compression quality setting
         *   functionality via ladybugSetAutoJPEGQualityControlFlag() to make sure that
         *   the data being produced does not exceed the buffer size.
         *
         * The following tables indicate a couple of sample combinations of the
         * packet size and buffer size parameters. Note the buffer size and maximum 
         * frame rate are inversely related.
         * This is useful for JPEG formats because you can adjust the image quality
         * (determined by the buffer size) and the frame rate based on your 
         * requirement.
         *
         * <PRE>
         *  Ladybug2
         *  |------------|----------------|-------------|-------------|---------------|
         *  | 1394 speed | Cameras on Bus | Packet Size | Buffer Size | Max Frame Rate|
         *  |------------|----------------|-------------|-------------|---------------|
         *  |    S800    |       1        |    9792     |  2400000    |   29.38 fps   |
         *  |    S800    |       1        |    9792     |  4718592    |   14.69 fps   |
         *  |    S800    |       2        |    4096     |  1200000    |   29.38 fps   |
         *  |    S400    |       1        |    4096     |  1000000    |   29.38 fps   |
         *  |    S400    |       2        |    2048     |  1000000    |   14.69 fps   |  
         *  |------------|----------------|-------------|-------------|---------------|
         *
         *  Ladybug3
         *  |------------|----------------|-------------|-------------|---------------|
         *  | 1394 speed | Cameras on Bus | Packet Size | Buffer Size | Max Frame Rate|
         *  |------------|----------------|-------------|-------------|---------------|
         *  |    S800    |       1        |    9792     |  4650848    |   16.0 fps    |
         *  |    S800    |       1        |    9792     | 11945472    |    6.5 fps    |
         *  |    S800    |       2        |    4096     |  4650848    |    7.0 fps    |
         *  |    S400    |       1        |    4096     |  4650848    |    7.0 fps    |
         *  |    S400    |       1        |    4096     |  2571056    |   12.1 fps    |
         *  |    S400    |       2        |    2048     |  4650848    |    3.5 fps    |  
         *  |------------|----------------|-------------|-------------|---------------|
         *  
         *  Ladybug5
         *  |------------|----------------|-------------|-------------|---------------|
         *  | 1394 speed | Cameras on Bus | Packet Size | Buffer Size | Max Frame Rate|
         *  |------------|----------------|-------------|-------------|---------------|
         *  |    N/A     |       1        |    32000    |     TBD     |    10.0 fps   |
         *  |------------|----------------|-------------|-------------|---------------|
         * </PRE>
         *
         * @param context      - The LadybugContext to access.
         * @param format       - The data format to start the camera in.
         * @param packetSize   - The packet size. 0 uses the current value on the camera.
         * @param bufferSize   - The image buffer size for receiving image data. 0 uses
         *                       the current buffer size for JPEG formats, and the 
         *                       maximum buffer size for uncompressed images.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugStart(), ladybugGrabImage(), ladybugStop(), ladybugSetAutoJPEGQualityControlFlag()
         */                 
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugStartEx", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError StartEx(
            IntPtr context,
            LadybugDataFormat format,
            uint packetSize,
            uint bufferSize);

        /**
         * Starts the camera and initializes the library for "lock next"
         * functionality. 
         * 
         * This function must be called before ladybugLockNext().
         *
         * @param context - The LadybugContext to access.
         * @param format  - The data format to start the camera in.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugStartLockNextEx(), ladybugLockNext(), ladybugStop()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugStartLockNext", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError StartLockNext(
            IntPtr context,
            LadybugDataFormat format);

        /**
         * Starts the camera with the specified packet size and image buffer size, 
         * and initializes the library for "lock next" functionality.
         * 
         * This function must be called before ladybugLockNext().
         *
         * If the value of packetSize is not specified or specified as 0, 
         * this call starts the camera with the current packet size value. 
         * 
         * If the value of bufferSize is not specified or specified as 0, 
         * this call starts the camera with the current buffer size setting 
         * for JPEG compressed images and use the maximum buffer size value
         * for uncompressed images.
         *
         * See comments in ladybugStartEx() for bandwidth negotiation between the
         * camera and the PC. 
         *
         * @param context      - The LadybugContext to access.
         * @param format       - The data format to start the camera in.
         * @param packetSize   - The packet size.
         * @param bufferSize   - The buffer size for receiving JPEG image data.
         *
         * @return A LadybugError indicating the success of the function. 
         *
         * @see ladybugStartLockNext(), ladybugLockNext(), ladybugStop()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugStartLockNextEx", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError StartLockNextEx(
            IntPtr context,
            LadybugDataFormat format,
            uint packetSize,
            uint bufferSize);

        /**
         * Stops the camera from grabbing images.
         *
         * This function should be called to stop the camera from sending images to the
         * driver on the host machine. This function also frees any previously
         * negotiated bandwidth.
         *
         * @param context - The LadybugContext to access.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugStart(),
         *   ladybugStartEx(),
         *   ladybugStartLockNext(),
         *   ladybugStartLockNextEx()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugStop", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError Stop(IntPtr context);

        /**
         * Sets the timeout value for grab functions.
         *
         * Use of this capability is recommended in cases where it is not desirable
         * to wait for the next image to arrive at the PC.  Setting timeout to 0
         * can also be useful in cases where 'Lock Next' functionality is being used 
         * and one is trying to determine how far behind they are.
         *
         * This function needs to be called prior to starting the camera.
         *
         * @param context   - The LadybugContext to access.
         * @param timeout   - The timeout value in milliseconds.  A value of 
         *                    LADYBUG_INFINITE indicates an infinite wait.  A value of
         *                    zero indicates a nonblocking grab call.
         *
         * @return A LadybugError indicating the success of the function.
         *   
         * @see ladybugStart(), ladybugStartEx(), ladybugStartLockNext(), ladybugStartLockNextEx()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetGrabTimeout", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetGrabTimeout(IntPtr context, uint timeout);

        /**
         * Gets the timeout value for grab functions.
         *
         * @param context    - The LadybugContext to access.
         * @param timeout    - The timeout value in milliseconds.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetGrabTimeout", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetGrabTimeout(IntPtr context, out uint timeout);

        /**
         * Retrieves the newest image that has not previously been seen.
         *
         * If an image is waiting, the function will return immediately. Otherwise,
         * the function will block until the next image arrives or the grab timeout
         * expires - whichever happens first.
         *
         * @param context - The LadybugContext to access.
         * @param pImage  - A pointer to an image structure. It will be filled with the
         *                  image information.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugSetGrabTimeout()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGrabImage", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GrabImage(
                       IntPtr context,
                       out LadybugImage pImage);

        /**
         * Locks the oldest image that has not been seen until it is manually released
         * using ladybugUnlock() or ladybugUnlockAll().
         *
         * If there is an image available that has not yet been locked, this
         * function returns immediately. Otherwise, it will block until an image
         * arrives or the grab timeout expires - whichever happens first.
         *
         * As long as the user locks and releases the image buffers fast enough to
         * prevent the allocated buffers from filling up, this function will ensure
         * that no images are lost.
         *
         * Users can verify image sequentiality by comparing the sequence numbers of 
         * the images.
         *
         * @param context - The LadybugContext to access.
         * @param pImage  - A pointer to an image structure. It will be filled with the
         *                  image information.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugSetGrabTimeout(), ladybugUnlock(), ladybugUnlockAll()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugLockNext", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError LockNext(
                       IntPtr context,
                       out LadybugImage pImage);

        /**
         * Unlocks and returns a previously locked buffer into the pool to be filled by
         * the library.
         *
         * This function must be called for each image locked using ladybugLockNext(). 
         * Otherwise, the library will run out of buffers.
         *
         * @param context       - The LadybugContext to access.
         * @param bufferIndex   - The index of the buffer to unlock.
         *
         * @return A LadybugError indicating the success of the function. 
         *
         * @see ladybugLockNext(), ladybugUnlockAll()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugUnlock", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError Unlock(
                       IntPtr context,
                       uint bufferIndex);

        /**
         * Unlocks all locked images. This function is equivalent to calling 
         * ladybugUnlock() for every locked buffer.
         *
         * @param context - The LadybugContext to access.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugLockNext(), ladybugUnlock()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugUnlockAll", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError UnlockAll(IntPtr context);

        /*@}*/

        /** 
         * @defgroup ManagedColorProcessingMethods Color Processing and Color Correction Methods 
         *
         * The functions contained within this group provide the functionality 
         * required to perform color processing and color correction on incoming
         * images.
         * 
         * @ingroup Ladybug_cs
         */

        /*@{*/ 


        /**
         * Get the number of CUDA GPUs available to perform GPU accelerated 
         * color processing.
         *
         * @param context - The LadybugContext to access.
         * @param numGPUs - The number of GPUs detected on the system.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetNumGPUs", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetNumGPUs(IntPtr context, out uint numGPUs);

        /**
         * Gets the current color processing method.
         *
         * @param context - The LadybugContext to access.
         * @param method  - The current color processing method.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugSetColorProcessingMethod()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetColorProcessingMethod", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetColorProcessingMethod(
            IntPtr context,
            out LadybugColorProcessingMethod method);

        /**
         * Sets the color processing method to use.  
         *
         * This method is useful when trying to change the quality of the images that
         * are produced by the library.
         * 
         * If the color processing method is set to LADYBUG_HQLINEAR_GPU, the color
         * processing will be performed on the GPU. This function checks the
         * availability of the required GPU resources, and may return 
         * LADYBUG_NO_CUDA_DEVICE indicating the required GPU functions 
         * are not supported or return LADYBUG_GPU_CUDA_DRIVER_ERROR indicating 
         * the GPU driver needs to be updated.
         *
         * @param context - The LadybugContext to access.
         * @param method  - The color processing method to set.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetColorProcessingMethod()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetColorProcessingMethod", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetColorProcessingMethod(
            IntPtr context,
            LadybugColorProcessingMethod method);

        /**
         * Gets the current color tile format.
         *
         * @param context - The LadybugContext to access.
         * @param format  - The current color tile format.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugSetColorTileFormat()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetColorTileFormat", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetColorTileFormat(
          IntPtr context,
          out LadybugStippledFormat format);

        /**
         * Sets the current color tile format.
         *
         * The color tile format is read from hardware and set automatically.  
         * You should not have to use this function.
         *
         * @param context - The LadybugContext to access.
         * @param format  - The color tile format to set.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetColorTileFormat()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetColorTileFormat", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetColorTileFormat(
          IntPtr context,
          LadybugStippledFormat format);

        /**
         * Fills in the alpha channel value in the specified BGRA buffers. 
         *
         * The alpha channel specifies how the overlapped regions between cameras 
         * should be rendered by OpenGL. Therefore, this function should be called 
         * before passing the BGRA buffers to any of the OpenGL views.
         *
         * @param context         - The LadybugContext to access.
         * @param numCols         - The number of columns in the images.
         * @param numRows         - The number of rows in the images.
         * @param arpBGRA32Images - The array of BGRA images
         * @param pixelFormat     - Pixel format of the BGRA images
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugAddAlphaChannel", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError AddAlphaChannel(IntPtr context, uint numCols, uint numRows, byte** arpBGRA32Images, LadybugPixelFormat pixelFormat = LadybugPixelFormat.LADYBUG_BGRU);

        /**
         * Converts the 6 images in a LadybugImage into 6 BGRA buffers. 
         *
         * The current color processing method is used.
         *
         * If arpDestBuffers is specified as NULL, this function will put the 
         * processed images in internal buffers. If there are no internal 
         * buffers, this function creates them.
         * 
         * This function also checks the size of the internal buffers and adjusts 
         * accordingly.
         *
         * If pixelFormat is specified as LADYBUG_UNSPECIFIED_PIXEL_FORMAT, the 
         * image will be converted to LADYBUG_BGRU for a raw8 or 8-bit JPEG raw
         * image, or LADYBUG_BGRU16 for a raw16 or 12-bit JPEG raw image. If the raw
         * image format is raw8 or 8-bit JPEG, specifying pixelFormat as 
         * LADYBUG_BGRU16 will return LADYBUG_INVALID_ARGUMENT. 
         *
         * The alpha mask (see ladybuggeom.h) is only written to the 
         * destination buffer the first time any Convert functions
         * are called.  If you wish to use a different destination buffer,
         * call ladybugSetAlphaMasking( true ) again.
         * 
         * Image border is cropped in order to remove LadybugImage::LadybugImageBorder
         *
         * If image needs adjustment ( see LadybugImage::LadybugImageHeader::needSoftwareAdjustment),
         * the requested adjustment is applied (see ladybugImageAdjustment.h) 
         *
         * Falloff correction is applied if the falloff correction flag is on.
         * 
         * Color correction is applied if the color correction flag is on.
         * 
         * Image statistics data is calculated if the image statistics flag is on.
         *
         * @param context        - The LadybugContext to access.
         * @param pImage         - A pointer to the raw image to be processed.
         * @param arpDestBuffers - An array of pointers to destination buffers that 
         *                         will hold the processed images. If specified as
         *                         NULL, this function will use internal buffers as 
         *                         destination buffers.
         * @param pixelFormat    - Pixel format of the processed images. The
         *                         supported formats depend on the data format
         *                         of the raw image pointed by pImage. The default 
         *                         value is LADYBUG_UNSPECIFIED_PIXEL_FORMAT.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugSetAlphaMasking(),
         *   ladybugSetFalloffCorrectionFlag(),
         *   ladybugSetFalloffCorrectionAttenuation(),
         *   ladybugSetColorCorrectionFlag(),
         *   ladybugGetColorCorrectionFlag(),
         *   ladybugSetColorCorrection(),
         *   ladybugGetColorCorrection(),
         *   ladybugSetImageStatisticsFlag(),
         *   ladybugGetImageStatisticsFlag(),
         *   ladybugGetImageStatistics()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugConvertImage", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError ConvertImage(
                    IntPtr context,
                    ref LadybugImage pImage,
                    byte** arpDestBuffers,
                    LadybugPixelFormat pixelFormat = LadybugPixelFormat.LADYBUG_UNSPECIFIED_PIXEL_FORMAT);
        
        /**
         * Converts the 6 images in a LadybugImage into 6 BGRA buffers. 
         *
         * Same as ladybugConvertImage() but with an output telling the client
         * what were the results of some automatic algorithm.
         *
         * @param context        - The LadybugContext to access.
         * @param pImage         - A pointer to the raw image to be processed.
         * @param arpDestBuffers - An array of pointers to destination buffers that 
         *                         will hold the processed images. If specified as
         *                         NULL, this function will use internal buffers as 
         *                         destination buffers.
         * @param pixelFormat    - Pixel format of the processed images. The
         *                         supported formats depend on the data format
         *                         of the raw image pointed by pImage.
         * @param outImage       - Modifiers applied on the image when converting it.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugConvertImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugConvertImageEx", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError ConvertImageEx(
                    IntPtr context,
                    ref LadybugImage pImage,
                    byte** arpDestBuffers,
                    LadybugPixelFormat pixelFormat,
                    out ConvertImageOutput outImage);

        /**
         * Converts the pixel format of buffers populated by ladybugConvertImage().
         * This has no affect on the internal library buffers that are used when the
         * arpDestBuffers argument to ladybugConvertImage() is NULL.
         *
         * Currently supported pairs of (pixelFormatIn, pixelFormatOut) are:
         *   (LADYBUG_BGR16, LADYBUG_BGR)
         *   (LADYBUG_BGRU16, LADYBUG_BGRU)
         *   (LADYBUG_BGRU16, LADYBUG_BGR)
         *
         * The input and output buffers can be disjoint:
         * assert (noOverlap(arpBuffersIn[i], arpBuffersOut[i])) for i = 0 ... numBuffers - 1
         * 
         * In-place operation is also supported:
         * assert (arpBuffersIn[i] == arpBuffersOut[i]) for i = 0 ... numBuffers - 1
         *
         * @param context            - The LadybugContext to access.
         * @param arpBuffersIn       - An array of input image buffers populated by ladybugConvertImage()
         * @param arpBuffersOut      - An array of output image buffers
         * @param numBuffers         - The number of buffers in arpImageBuffersIn and arpImageBuffersOut.
         * @param numCols            - The number of columns in each input and output buffer.
         *                             This depends on the LadybugColorProcessingMethod used to produce the LadybugImage.
         * @param numRows            - The number of rows in each input and output buffer.
         *                             This depends on the LadybugColorProcessingMethod used to produce the LadybugImage.
         * @param pixelFormatIn      - The input pixel format of arpImageBuffersIn.
         * @param pixelFormatOut     - The output pixel format requested for arpImageBuffersOut.
         *
         * @return A LadybugError indicating the success of the function.  
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugConvertImageBuffersPixelFormat", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError ConvertImageBuffersPixelFormat(
                    IntPtr context,
                    byte** arpBuffersIn,
                    byte** arpBuffersOut,
                    uint numBuffers,
                    uint numCols,
                    uint numRows,
                    LadybugPixelFormat pixelFormatIn,
                    LadybugPixelFormat pixelFormatOut);

        /**
         * Converts a LadybugImage to a set of color-processed images. 
         * The current color processing method is used.
         *
         * @param context        - The LadybugContext to access.
         * @param image          - A pointer to the image to be processed.
         * @param filenames      - An array of pointers to filenames for
         *                         saving the images.
         * @param imageInfo      - Image information extracted from the retrieved 
         *                         image. Use NULL to disable.
         * @param saveFormat     - The format to save the images in.
         *
         * @return A LadybugError indicating the success of the function.  
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugExtractLadybugImageToFilesBGRU32", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError ExtractLadybugImageToFilesBGRU32(
            IntPtr context, 
            ref LadybugImage image,
            IntPtr* fileNames,
            ref LadybugImageInfo imageInfo,
            LadybugSaveFileFormat saveFormat);

        /**
         * Converts an interleaved data buffer to a set of color-processed images.  
         *
         * The current color processing method is used.
         *
         * Filenames are generated automatically.
         *
         * The alpha mask (see ladybuggeom.h) are only written to the 
         * destination buffer the first time any of the Convert functions
         * are called.  If you wish to use a different destination buffer,
         * call ladybugSetAlphaMasking( true ) again.
         * 
         * Falloff correction is applied if the correction flag is on.
         *
         * @param context    - The LadybugContext to access.
         * @param image      - A pointer to the raw image to be processed.
         * @param imageNum     - The image number this image corresponds to.  Use -1
         *                     to disable image numbering in the output filenames.
         * @param path       - The base path to use when generating filenames.
         * @param imageInfo  - Image information extracted from the image. 
         *                     Use NULL to disable.
         * @param saveformat - The format to save the images in.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugSetAlphaMasking(),
         *   ladybugSetFalloffCorrectionFlag(),
         *   ladybugSetFalloffCorrectionAttenuation()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugExtractLadybugImageToAutoFilesBGRU32", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError ExtractLadybugImageToAutoFilesBGRU32(
            IntPtr context,
            ref LadybugImage image,
            int imageNum,
            string path,
            out LadybugImageInfo imageInfo,
            LadybugSaveFileFormat saveFormat);

        /**
         * Retrieves a flag indicating if color correction is applied in 
         * ladybugConvertImage(). It is off by default.
         *
         * @param context   - The LadybugContext to access.
         * @param flag      - Location to return the color correction flag.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugSetColorCorrectionFlag(),
         *   ladybugGetColorCorrection(),
         *   ladybugSetColorCorrection(),
         *   ladybugConvertImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetColorCorrectionFlag", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetColorCorrectionFlag(
                             IntPtr context,
                             out bool flag);

        /**
         * Sets an internal flag indicating if color correction is applied
         * in ladybugConvertImage. It is off by default.
         *
         * @param context   - The LadybugContext to access.
         * @param flag      - A flag indicating if color correction is applied.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetColorCorrectionFlag(),
         *   ladybugGetColorCorrection(),
         *   ladybugSetColorCorrection(),
         *   ladybugConvertImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetColorCorrectionFlag", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetColorCorrectionFlag(
                             IntPtr context,
                             bool flag);

        /**
         * Retrieves the current color correction values.
         *
         * @param context  - The LadybugContext to access.
         * @param ccparams - The pointer to store the correction parameter struct.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetColorCorrectionFlag(),
         *   ladybugSetColorCorrectionFlag(),
         *   ladybugSetColorCorrection(),
         *   ladybugConvertImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetColorCorrection", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetColorCorrection(IntPtr context,
                           out LadybugColorCorrectionParams ccparams);

        /**
         * Sets the color correction parameters to be applied when it is enabled.
         *
         * The valid range of values is between -255 and 255.
         * A value of 0 indicates no adjustment.
         *
         * @param context  - The LadybugContext to access.
         * @param ccparams - The pointer to the color correction parameter struct.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetColorCorrectionFlag(),
         *   ladybugSetColorCorrectionFlag(),
         *   ladybugGetColorCorrection(),
         *   ladybugConvertImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetColorCorrection", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetColorCorrection(IntPtr context,
                           ref LadybugColorCorrectionParams ccparams);

        /**
         * Retrieves a flag indicating if false color removal is applied in 
         * ladybugConvertImage. It is off by default.
         *
         * False color removal is only supported for BGRU16 and BGRU images.
         *
         * @param context           - The LadybugContext to access.
         * @param falseColorRemoval - Location to return the flag.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugSetFalseColorRemoval(), ladybugConvertImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetFalseColorRemoval", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetFalseColorRemoval(IntPtr context, out bool falseColorRemoval);

        /**
         * Sets an internal flag indicating if false color removal is applied
         * in ladybugConvertImage. It is off by default.
         *
         * False color removal is only supported for BGRU16 images.
         *
         * @param context           - The LadybugContext to access.
         * @param falseColorRemoval - A flag indicating if false color removal is applied.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetFalseColorRemoval(), ladybugConvertImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetFalseColorRemoval", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetFalseColorRemoval(IntPtr context, bool falseColorRemoval);

        /**
         * Retrieves a flag indicating if image sharpening is applied in 
         * ladybugConvertImage. It is off by default.
         *
         * @param context   - The LadybugContext to access.
         * @param flag      - Location to return the sharpening flag.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugSetSharpening(),
         *   ladybugConvertImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetSharpening", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetSharpening(
                             IntPtr context,
                             out bool flag);

        /**
         * Sets an internal flag indicating if image sharpening is applied
         * in ladybugConvertImage. It is off by default.
         *
         * @param context   - The LadybugContext to access.
         * @param flag      - A flag indicating if sharpening is applied.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetSharpening(),
         *   ladybugConvertImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetSharpening", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetSharpening(
                             IntPtr context,
                             bool flag);

        /**
         * Enables or disables image stabilization functionality.
         * When stabilization is enabled, the output of stitched images are
         * stabilized so that the effect of camera rotation is minimized.
         * This feature works by setting an initial rotation position, and in
         * successive frames, the rotation positions fall back to that initial
         * setting according to a specified decay rate.
         * This function can be applied either during image grabbing or
         * when images are read from stream files. We recommend enabling image
         * stabilization while reading stream files, as this feature can be 
         * computationally intensive and may result in lost frames during image capture.
         *
         * @param context - The LadybugContext to access.
         * @param bEnable - Flag to indicate whether stabilization is enabled or disabled.
         * @param pParams - The stabilization control parameters used when bEnable is
         *                  true. If this is null, the default parameters are used.
         *
         * @return A LadybugError indicating the success of the function.   
         *
         * @see ladybugConvertImage().
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugEnableImageStabilization", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError EnableImageStabilization(IntPtr context,
                           bool enable,
                           ref LadybugStabilizationParams stabilizationParams);

        /**
         * Set tone mapping parameters.
         *
         * OpenGL tonemapping will affect the OpenGL state on the currently bound
         * device context.  Thus, you should have the same device context bound as
         * when you call ladybugUpdateTextures().
         *
         * @param context           - The LadybugContext to access.
         * @param toneMappingParams - Parameters for tone mapping.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetToneMappingParams", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetToneMappingParams(IntPtr context, ref LadybugToneMappingParams toneMappingParams);

        /**
         * Get tone mapping parameters.
         *
         * @param context            - The LadybugContext to access.
         * @param toneMappingParams  - Parameters for tone mapping.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetToneMappingParams", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetToneMappingParams(IntPtr context, out LadybugToneMappingParams toneMappingParams);

        /*@}*/

        /** 
         * @defgroup ManagedLensFallOffMethods Lens Falloff Correction Methods
         *
         * Because of their short focal length, the 
         * lenses used in the Ladybug camera system suffer from lens falloff.  This
         * phenomenon can be observed in both raw and color-processed images as a 
         * darkening near the edges of the images.  
         * The functions contained in this group provide the the ability
         * to control the amount of correction applied to the images.
         * 
         * The correction can either be applied implicitly or explicitly as outlined
         * below.
         * Implicit - ladybugConvertImage will correct falloff with the 
         * specified attenuation factor if and only if the correction flag has been 
         * set via ladybugSetFalloffCorrectionFlag. The correction is done implicitly
         * when the function parses the raw image into 6 individual BGRU images.
         * 
         * @ingroup Ladybug_cs
         */

        /*@{*/ 

        /**
         * Retrieves a flag indicating if intensity falloff will be corrected in 
         * ladybugConvertImage.
         *
         * The exact amount to be corrected is controlled by the correction
         * attenuation factor set in ladybugSetFalloffCorrectionAttenuation.
         *
         * @param context          - The LadybugContext to access.
         * @param pcorrectFalloff   - Location to return the intensity falloff 
         *                           correction flag.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see   ladybugSetFalloffCorrectionFlag(),
         *   ladybugGetFalloffCorrectionAttenuation(),
         *   ladybugSetFalloffCorrectionAttenuation(),
         *   ladybugConvertImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetFalloffCorrectionFlag", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetFalloffCorrectionFlag(IntPtr context, out bool correctFalloff);

        /**
         * Sets an internal flag indicating if intensity falloff will be corrected
         * in ladybugConvertImage.
         *
         * The exact amount to be corrected is controlled by the correction
         * attenuation factor set in ladybugSetFalloffCorrectionAttenuation().
         *
         * @param context         - The LadybugContext to access.
         * @param correctFalloff - A flag indicating if intensity falloff will be 
         *                          corrected.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see   ladybugGetFalloffCorrectionFlag(),
         *   ladybugGetFalloffCorrectionAttenuation(),
         *   ladybugSetFalloffCorrectionAttenuation(),
         *   ladybugConvertImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetFalloffCorrectionFlag", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetFalloffCorrectionFlag(IntPtr context, bool correctFalloff);

        /**
         * Gets the current falloff correction value.
         *
         * @param context               - The LadybugContext to access.
         * @param attenuationFraction   - Location to return the fraction used for
         *                                falloff correction.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetFalloffCorrectionFlag(),
         *   ladybugSetFalloffCorrectionFlag(),
         *   ladybugSetFalloffCorrectionAttenuation()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetFalloffCorrectionAttenuation", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetFalloffCorrectionAttenuation(IntPtr context, out float attenuationFraction);
        
        /**
         * Sets the current falloff correction value.
         *
         * @param context              - The LadybugContext to access.
         * @param attenuationFraction  - Fraction used to adjust the intensity correction 
         *                               amount. (Must be a value between 0.0 and 1.0).
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetFalloffCorrectionFlag(),
         *   ladybugSetFalloffCorrectionFlag(),
         *   ladybugGetFalloffCorrectionAttenuation()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetFalloffCorrectionAttenuation", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetFalloffCorrectionAttenuation(IntPtr context, float attenuationFraction);

        /**
         * Applies intensity falloff correction on the specified BGRU images.
         *
         * The amount of correction applied will depend on the setting of the current
         * correction attenuation factor (see ladybugSetFalloffCorrectionAttenuation)
         * and the specified gamma.
         *
         * The specified gamma should match the gamma setting when these images were
         * originally captured. Gamma can be extracted either interactively from the
         * Camera Control Dialog of the LadybugCap or LadybugCapPro programs, or 
         * programmatically from the ulGamma field inside the LadybugImageInfo 
         * of the original LadybugImage.
         *
         * Note that the ulGamma field in LadybugImageInfo is in IEEE-1394
         * register encoded format. Be sure to decode it appropriately to extract 
         * the value (see description in the FLIR Machine Vision Camera Register Reference).
         *
         * @param context          - The LadybugContext to access.
         * @param numCols          - The number of columns in the images.
         * @param numRows          - The number of rows in the images.
         * @param arpBGRU32Images  - The array of BGRU images.
         * @param pixelFormat      - The pixel format of the BGRU images.
         * @param gamma            - The gamma value under which the images were captured.
         *                          (A -1 gamma value will invoke the default gamma value)
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetFalloffCorrectionAttenuation(),
         *   ladybugSetFalloffCorrectionAttenuation()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugCorrectBGRUFalloffEx", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError CorrectBGRUFallofEx(
            IntPtr context, 
            uint numCols, 
            uint numRows, 
            byte** arpBGRU3Images, 
            LadybugPixelFormat pixelFormat, long gamma);

        /**
         * Applies intensity falloff correction on the specified stippled images.
         *
         * The amount of correction applied will depend on the setting of the
         * current correction attenuation factor (see ladybugSetFalloffCorrectionAttenuation)
         * and the specified gamma.
         *
         * The specified gamma should match the gamma setting when these images were
         * originally captured. Gamma can be extracted either interactively from the
         * Camera Control Dialog of the LadybugCap or LadybugCapPro programs or 
         * programmatically from the ulGamma field inside the LaydbugImageInfo of 
         * the original LadybugImage.
         *
         * Note that the ulGamma field in LadybugImageInfo is in IEEE-1394
         * register encoded format. Be sure to decode it appropriately to extract 
         * the value (see description in the FLIR Machine Vision Camera Register Reference).
         *
         * @param context            - The LadybugContext to access.
         * @param numCols            - The number of columns in the images.
         * @param numRows            - The number of rows in the images.
         * @param arpStippledImages  - The array of stippled images.
         * @param gamma              - The gamma value under which the images were captured.
         *                            (A -1 gamma value will invoke the default gamma value)
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetFalloffCorrectionAttenuation(),
         *   ladybugSetFalloffCorrectionAttenuation()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugCorrectStippledFalloffEx", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError CorrectStippledFalloffEx(
            IntPtr context, 
            uint numCols, 
            uint numRows, 
            byte** arpStippledImages, 
            long gamma);

        /*@}*/

        /** 
         * @defgroup ManagedCameraPropMethods Camera Property Access and Control Methods
         *
         * These functions allow users
         * to access and control a variety of camera parameters.
         * 
         * @ingroup Ladybug_cs
         */

        /*@{*/ 

        /**
         * Get information about the range of a specific camera property.
         *
         * The values obtained from this function are useful when trying to interpret
         * and set camera properties with the ladybugGetProperty and 
         * ladybugSetProperty functions.
         *
         * @param context   - The LadybugContext to access.
         * @param property  - The camera property to query.
         * @param present - Whether or not the property is present.
         * @param min     - The minimum value of the property.
         * @param max     - The maximum value of the property.
         * @param default - The default value of the property.
         * @param auto    - The availability of auto mode for the property.
         * @param manual  - The ability to manually control the value of the property.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetProperty(),
         *    ladybugSetProperty(),
         *    ladybugGetAbsProperty(),
         *    ladybugSetAbsProperty(),
         *    ladybugGetPropertyEx(),
         *    ladybugSetPropertyEx(),
         *    ladybugGetAbsPropertyEx(),
         *    ladybugSetAbsPropertyEx(),
         *    ladybugGetPropertyRangeEx(),
         *    ladybugGetAbsPropertyRange()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetPropertyRange", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetPropertyRange(IntPtr context,
                             LadybugProperty property,
                             out bool present,
                             out int min,
                             out int max,
                             out int defaultValue,
                             out bool auto,
                             out bool manual);

        /**
         * Gets information about the features and range of values of a specific camera property. 
         *
         * This function provides better access to camera features as compared to
         * ladybugGetPropertyRange(). The values obtained from this function are 
         * useful when trying to interpret and set camera properties with the 
         * ladybugGetProperty and ladybugSetProperty functions.
         *
         * @param context   - The LadybugContext to access.
         * @param property  - The camera property to query.
         * @param present   - Whether or not the property is present.
         * @param onePush   - The availability of the one push feature.
         * @param readOut   - The ability to read out the value of this property.
         * @param onOff     - The ability to turn this property on and off.
         * @param auto      - The availability of auto mode for the property.
         * @param manual    - The ability to manually control the value of the property.
         * @param min       - The minimum possible value of the property.
         * @param max       - The maximum possible value of the property.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetProperty(),
         *    ladybugSetProperty(),
         *    ladybugGetAbsProperty(),
         *    ladybugSetAbsProperty(),
         *    ladybugGetPropertyEx(),
         *    ladybugSetPropertyEx(),
         *    ladybugGetAbsPropertyEx(),
         *    ladybugSetAbsPropertyEx(),
         *    ladybugGetPropertyRange(),
         *    ladybugGetAbsPropertyRange()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetPropertyRangeEx", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetPropertyRangeEx(IntPtr context,
                             LadybugProperty property,
                             out bool present,
                             out bool onePush,
                             out bool readOut,
                             out bool onOff,
                             out bool auto,
                             out bool manual,
                             out int min,
                             out int max);

        /**
         * Gets the range of possible absolute values of the specified camera property. 
         *
         * @param context   - The LadybugContext to access.
         * @param property  - The camera property to query.
         * @param present   - Presence of the property.
         * @param min       - The absolute minimum value of the property.
         * @param max       - The absolute maximum value of the property.
         * @param units     - A string containing the units of the register.
         * @param unitAbbr  - A string containing an abbreviation of the units.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetProperty(),
         *    ladybugSetProperty(),
         *    ladybugGetPropertyEx(),
         *    ladybugSetPropertyEx(),
         *    ladybugSetAbsProperty(),
         *    ladybugGetAbsProperty(),
         *    ladybugGetAbsPropertyEx(),
         *    ladybugSetAbsPropertyEx(),
         *    ladybugGetPropertyRange(),
         *    ladybugGetPropertyRangeEx(),
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetAbsPropertyRange", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetAbsPropertyRange(IntPtr context,
                             LadybugProperty property,
                             out bool present,
                             out float min,
                             out float max,
                             out IntPtr units,
                             out IntPtr unitAbbr);
        /**
         * Gets the current values of the specified property.
         * Most properties only have an "A" value. 
         *
         * @param context  - The LadybugContext to access.
         * @param property - The camera property to query.
         * @param valueA   - The "A", or first, value of the property.
         * @param valueB   - The "B", or second, value of the property.
         * @param auto     - The current auto value of the property.
         *
         * @return A LadybugError indicating the success of the function.  
         *
         * @see ladybugSetProperty(),
         *    ladybugGetPropertyEx(),
         *    ladybugSetPropertyEx(),
         *    ladybugSetAbsProperty(),
         *    ladybugGetAbsProperty(),
         *    ladybugGetAbsPropertyEx(),
         *    ladybugSetAbsPropertyEx(),
         *    ladybugGetPropertyRange(),
         *    ladybugGetPropertyRangeEx(),
         *    ladybugGetAbsPropertyRange()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetProperty", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetProperty(IntPtr context,
                             LadybugProperty property,
                             out int valueA,
                             out int valueB,
                             out bool auto);

        /**
         * Gets information about the features and current values of a specified camera property. 
         *
         * This API function provides better access to camera features as compared to
         * ladybugGetProperty().
         *
         * @param context   - The LadybugContext to access.
         * @param property  - The camera property to query.
         * @param onePush   - The availability of the one push feature.
         * @param onOff     - The ability to turn this property on and off.
         * @param auto      - The auto value of the property.
         * @param valueA    - The "A" value.
         * @param valueB    - The "B" value.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetProperty(),
         *    ladybugSetProperty(),
         *    ladybugSetPropertyEx(),
         *    ladybugGetAbsProperty(),
         *    ladybugSetAbsProperty(),
         *    ladybugGetAbsPropertyEx(),
         *    ladybugSetAbsPropertyEx(),
         *    ladybugGetPropertyRange(),
         *    ladybugGetPropertyRangeEx(),
         *    ladybugGetAbsPropertyRange()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetPropertyEx", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetPropertyEx(IntPtr context,
                             LadybugProperty property,
                             out bool onePush,
                             out bool onOff,
                             out bool auto,
                             out int valueA,
                             out int valueB);

        /**
         * Gets the current absolute value of the specified property.
         *
         * @param context  - The LadybugContext to access.
         * @param property - The camera property to query.
         * @param valueA   - A pointer to a float that will contain the result.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetProperty(),
         *    ladybugSetProperty(),
         *    ladybugGetPropertyEx(),
         *    ladybugSetPropertyEx(),
         *    ladybugSetAbsProperty(),
         *    ladybugGetAbsPropertyEx(),
         *    ladybugSetAbsPropertyEx(),
         *    ladybugGetPropertyRange(),
         *    ladybugGetPropertyRangeEx(),
         *    ladybugGetAbsPropertyRange()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetAbsProperty", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetAbsProperty(IntPtr context,
                             LadybugProperty property,
                             out float valueA);

        /**
         * Gets information about the range of the absolute value of a specific camera property.
         *
         * This API function provides better access to camera features as compared to
         * ladybugGetAbsProperty().
         *
         * @param context   - The LadybugContext to access.
         * @param property  - The camera property to query.
         * @param onePush   - The availability of the one push feature.
         * @param onOff     - The ability to turn this property on and off.
         * @param auto      - The auto value of the property.
         * @param value     - A pointer to a float that will contain the result.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see    ladybugGetProperty(),
         *    ladybugSetProperty(),
         *    ladybugGetPropertyEx(),
         *    ladybugSetPropertyEx(),
         *    ladybugGetAbsProperty(),
         *    ladybugSetAbsProperty(),
         *    ladybugGetAbsPropertyEx(),
         *    ladybugGetPropertyRange(),
         *    ladybugGetPropertyRangeEx(),
         *    ladybugGetAbsPropertyRange()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetAbsPropertyEx", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetAbsPropertyEx(IntPtr context,
                             LadybugProperty property,
                             out bool onePush,
                             out bool onOff,
                             out bool auto,
                             out float value);

        /**
         * Sets a camera property.  
         * Most properties only require an "A" value.
         *
         * @param context   - The LadybugContext to access.
         * @param property  - The camera property to query.
         * @param valueA    - The "A", or first, new value of the property.
         * @param valueB    - The "B", second, new value of the property.
         * @param auto      - The auto value.
         *
         * @return A LadybugError indicating the success of the function. 
         *
         * @see ladybugGetProperty(),
         *    ladybugGetPropertyEx(),
         *    ladybugSetPropertyEx(),
         *    ladybugGetAbsProperty(),
         *    ladybugSetAbsProperty(),
         *    ladybugGetAbsPropertyEx(),
         *    ladybugSetAbsPropertyEx(),
         *    ladybugGetPropertyRange(),
         *    ladybugGetPropertyRangeEx(),
         *    ladybugGetAbsPropertyRange()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetProperty", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetProperty(IntPtr context,
                             LadybugProperty property,
                             int valueA,
                             int valueB,
                             bool auto);

        /**
         * Sets the value and other features of a specified camera property.   
         *
         * This API function provides better access to camera features as compared
         * to ladybugSetProperty().
         *
         * @param context   - The LadybugContext to access.
         * @param property  - The camera property to query.
         * @param onePush   - Specifies if the one push feature is enabled.
         * @param onOff     - Specifies if the property should be on or off.
         * @param auto      - Specifies if the property can be automatically controlled by the camera..
         * @param valueA    - The "A", or first, value of the property.
         * @param valueB    - The "B", or second, value of the property.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see    ladybugGetProperty(),
         *    ladybugSetProperty(),
         *    ladybugGetPropertyEx(),
         *    ladybugGetAbsProperty(),
         *    ladybugSetAbsProperty(),
         *    ladybugGetAbsPropertyEx(),
         *    ladybugSetAbsPropertyEx(),
         *    ladybugGetPropertyRange(),
         *    ladybugGetPropertyRangeEx(),
         *    ladybugGetAbsPropertyRange()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetPropertyEx", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetPropertyEx(IntPtr context,
                             LadybugProperty property,
                             bool onePush,
                             bool onOff,
                             bool auto,
                             int valueA,
                             int valueB);

        /**
         * Sets the absolute value of a camera property.
         *
         * @param context  - The LadybugContext to access.
         * @param property - The camera property to query.
         * @param valueA   - A float containing the new value of the parameter.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetProperty(),
         *    ladybugSetProperty(),
         *    ladybugGetPropertyEx(),
         *    ladybugSetPropertyEx(),
         *    ladybugGetAbsProperty(),
         *    ladybugGetAbsPropertyEx(),
         *    ladybugSetAbsPropertyEx(),
         *    ladybugGetPropertyRange(),
         *    ladybugGetPropertyRangeEx(),
         *    ladybugGetAbsPropertyRange()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetAbsProperty", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetAbsProperty(IntPtr context,
                             LadybugProperty property,
                             float valueA);

        /**
         * Sets the absolute value of a camera property. 
         *
         * This function also allows the user to specify the one push, on/off, and auto 
         * settings of the same property. 
         *
         * This API function provides better access to camera features as compared
         * to ladybugSetProperty().
         *
         * @param context   - The LadybugContext to access.
         * @param property  - The camera property to query.
         * @param onePush   - Specifies if the one push feature is enabled.
         * @param onOff     - Specifies if the property should be on or off.
         * @param auto      - Specifies if the property can be automatically controlled by the camera.
         * @param value     - A float containing the new value of the parameter.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetProperty(),
         *    ladybugSetProperty(),
         *    ladybugGetPropertyEx(),
         *    ladybugSetPropertyEx(),
         *    ladybugGetAbsProperty(),
         *    ladybugSetAbsProperty(),
         *    ladybugGetPropertyRange(),
         *    ladybugGetPropertyRangeEx(),
         *    ladybugGetAbsPropertyRange()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetAbsPropertyEx", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetAbsPropertyEx(IntPtr context,
                             LadybugProperty property,
                             bool onePush,
                             bool onOff,
                             bool auto,
                             float value);

        /**
         * Gets the value of a register on the camera.
         *
         * pulValue is set to zero if this function fails.
         *
         * All of the Ladybug camera systems loosely follow the IIDC digital camera 
         * specification. As such, users wishing to explore low level camera
         * configuration options should reference the FLIR Machine Vision Camera Register
         * Reference.
         *
         * @param context   - The LadybugContext to access.
         * @param register  - The register to query.
         * @param value     - The returned register value.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugSetRegister()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetRegister", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetRegister(IntPtr context,
                             uint register,
                             out uint value);

        /**
         * Sets the value of a register on the camera.
         *
         * @param context   - The LadybugContext to access.
         * @param register  - The register to set.
         * @param value     - The value of the register to set.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetRegister()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetRegister", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetRegister(IntPtr context,
                             uint register,
                             uint value);

        /**
         * Provides block-read (asynchronous) access to the entire register space of 
         * the Ladybug camera.
         *
         * @param context   - The LadybugContext to access.
         * @param addrHigh  - The top 16 bits of the 48-bit absolute address to read.
         * @param addrLow   - The bottom 32 bits of the 48-bit absolute address to read.
         * @param buffer    - The buffer that will receive the data. Must be of size ulLength.
         * @param length    - The length, in quadlets, of the block to read.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugWriteRegisterBlock()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugReadRegisterBlock", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError ReadRegisterBlock(IntPtr context, ushort addrHigh, uint addrLow, out uint* buffer, uint length);

        /**
         * Provides block-write (asynchronous) access to the entire register space of 
         * the Ladybug camera.
         *
         * @param context   - The LadybugContext to access.
         * @param addrHigh  - The top 16 bits of the 48-bit absolute address to write.
         * @param addrLow   - The bottom 32 bits of the 48-bit absolute address to write.
         * @param buffer    - The buffer that contains the data to be written.
         * @param length    - The length, in quadlets, of the block to write.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugReadRegisterBlock()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugWriteRegisterBlock", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError WriteRegisterBlock (IntPtr context, ushort addrHigh, uint addLow, uint* buffer, uint length);

        /**
         * Gets the range of possible values of a property associated with a 
         * specified camera, independent of other Ccameras in the camera unit.
         *
         * @param context   - The LadybugContext to access.
         * @param property  - The independent property to query.
         * @param camera    - The camera to query.  
         * @param present   - The presence of the register.
         * @param min       - The minimum value.
         * @param max       - The maximum value.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetIndPropertyRange", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetIndPropertyRange(IntPtr context, LadybugIndependentProperty property, uint camera, out bool present, out uint min, out uint max);

        /**
         * Gets the current value and other features of a property associated 
         * with a specified camera, independent of other cameras in the camera unit.
         *
         * @param context        - The LadybugContext to access.
         * @param property       - The independent property to query.
         * @param uiCamera       - The CCD to query.  
         * @param pulValue       - The current value of the property.
         * @param pbOnOff        - Whether the property is on or off.
         * @param pbAuto         - Whether or not the auto flag is set for the property.
         * @param puiAutoExpCams - A bitfield indicating which of the other CCDs this
         *                         CCD should use for Auto Exposure calculations.
         *                         Only used with LADYBUG_SUB_AUTO_EXPOSURE. 
         *                         See LadybugCameraBits.
         *                         This parameter is only valid for Ladybug2.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugSetIndProperty()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetIndProperty", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetIndProperty(
                            IntPtr context,
                            LadybugIndependentProperty property,
                            uint uiCamera,
                            out ulong pulValue,
                            out bool pbOnOff,
                            out bool pbAuto,
                            out uint puiAutoExpCams );

        /**
         * Sets the value of a property of a specified camera, independent of the 
         * other cameras in the camera unit.
         * 
         * For Ladybug3 and Ladybug5, the bOnOff field affects all cameras
         * simultaneously.  For example, On is set to any one camera, then the camera 
         * is set to independent shutter (or gain) mode, where each shutter (or gain)
         * can take different values. If you set Off to any one camera, all the 
         * shutter (or gain) values will be the same across all cameras. 
         *
         * @param context       - The LadybugContext to access.
         * @param property      - The independent property to query.
         * @param uiCamera      - The camera to query.  
         * @param ulValue       - The value to set.
         * @param bOnOff        - Specifies if the property should be on or off. *                        
         * @param bAuto         - Specifies if the property should be automatically 
         *                        controlled by the camera. To manually control the value
         *                        with ulValue, this must be false.
         * @param uiAutoExpCams - A bitfield indicating which of the other cameras this 
         *                        camera should use for Auto Exposure calculations.  
         *                        Only used with LADYBUG_SUB_AUTO_EXPOSURE. 
         *                        See LadybugCameraBits.
         *                        This parameter is only valid for Ladybug2.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetIndProperty()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetIndProperty", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetIndProperty(
                            IntPtr context,
                            LadybugIndependentProperty property,
                            uint uiCamera,
                            ulong ulValue,
                            bool bOnOff,
                            bool bAuto,
                            uint uiAutoExpCams );

        /**
         * Get the current shutter range set on the camera. If a range not matching
         * the preset values is entered, then LADYBUG_AUTO_SHUTTER_CUSTOM is returned 
         * as the shutter range.
         *
         * @param context - The LadybugContext to access.
         * @param autoShutterRange - The shutter range set on the camera.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetAutoShutterRange", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetAutoShutterRange(
            IntPtr context,
            out LadybugAutoShutterRange autoShutterRange);

        /**
         * Set the shutter range on the camera. LADYBUG_AUTO_SHUTTER_CUSTOM is not
         * an acceptable value.
         *
         * @param context - The LadybugContext to access.
         * @param autoShutterRange - The shutter range to set on the camera.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetAutoShutterRange", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetAutoShutterRange(
            IntPtr context,
            LadybugAutoShutterRange autoShutterRange);

        /**
         * Get the ROI used for auto exposure calculations from the camera. 
         *
         * This is a separate setting from the auto exposure ROI setting 
         * for post processing.
         *
         * This is only supported for Ladybug5 or newer cameras.
         *
         * @param context - The LadybugContext to access.
         * @param roi - The auto exposure ROI set on the camera.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetAutoExposureROI", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetAutoExposureROI(
            IntPtr context,
            out LadybugAutoExposureRoi roi);

        /**
         * Set the ROI used for auto exposure calculations to the camera. 
         *
         * This is a separate setting from the auto exposure ROI setting 
         * for post processing.
         *
         * This is only supported for Ladybug5 or newer cameras.
         *
         * @param context - The LadybugContext to access.
         * @param roi - The auto exposure ROI to set on the camera.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetAutoExposureROI", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetAutoExposureROI(
            IntPtr context,
            LadybugAutoExposureRoi roi);

        /*@}*/

        /** 
         * @defgroup ManagedMemoryChannelMethods Memory Channel Methods
         *
         * These functions are related to the memory channel support on the camera.
         * 
         * @ingroup Ladybug_cs
         */

        /*@{*/ 

        /**
         * Get the current memory channel in use.
         *
         * @param context - The LadybugContext to access.
         * @param currMemoryChannel - Current memory channel in use.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetMemoryChannel", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetMemoryChannel(IntPtr context, out uint currMemoryChannel);

        /**
         * Save current settings to specified memory channel.
         *
         * @param context - The LadybugContext to access.
         * @param memoryChannel - Memory channel to save to.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSaveToMemoryChannel", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SaveToMemoryChannel(IntPtr context, uint memoryChannel);

        /**
         * Restore settings from specified memory channel.
         *
         * @param context - The LadybugContext to access.
         * @param memoryChannel - Memory channel to restore from.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugRestoreFromMemoryChannel", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError RestoreFromMemoryChannel(IntPtr context, uint memoryChannel);

        /**
         * Get the number of memory channels available on the camera.
         *
         * @param context - The LadybugContext to access.
         * @param numMemoryChannels - Number of memory channels.
         *
         * @return A LadybugError indicating the success of the function.
         */    
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetMemoryChannelInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetMemoryChannelInfo(IntPtr context, out uint numMemoryChannels);

        /*@}*/

        /** 
         * @defgroup ManagedJPEGMethods JPEG Methods
         *
         * These functions are related to the camera and library's JPEG compression
         * functionality. All of these functions are only applicable
         * to the Ladybug2 and newer.
         * 
         * @ingroup Ladybug_cs
         */

        /*@{*/ 

        /**
         * Gets the current JPEG compression quality setting on the camera (used with JPEG data formats).
         * Used if ladybugGetAutoJPEGQualityControlFlag() is false.
         *
         * @param context   - The LadybugContext to access.     
         * @param quality   - The retrieved quality metric.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugSetJPEGQuality()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetJPEGQuality", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetJPEGQuality(IntPtr context,
                             out int quality);

        /**
         * Sets the JPEG compression quality setting on the camera (used with JPEG data formats).
         * Used if ladybugGetAutoJPEGQualityControlFlag() is false.
         *
         * @param context  - The LadybugContext to access.     
         * @param quality  - An integer from 1 to 100 indicating the JPEG compression 
         *                   quality.  Higher settings result in a larger image
         *                   data size and slower decompression rate.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetJPEGQuality()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetJPEGQuality", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetJPEGQuality(IntPtr context,
                             int quality);

        /**
         * Gets the current value of the JPEG image buffer usage register.
         * Used if ladybugGetAutoJPEGQualityControlFlag() is true.
         *
         * @param context        - The LadybugContext to access.     
         * @param puiBufferUsage - The retrieved JPEG image buffer usage value.
         *
         * @return   A LadybugError indicating the success of the function.
         *
         * @see   ladybugSetAutoJPEGBufferUsage(),
         *   ladybugSetAutoJPEGQualityControlFlag(),
         *   ladybugGetAutoJPEGQualityControlFlag(),
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetAutoJPEGBufferUsage", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetAutoJPEGBufferUsage(IntPtr context, out uint puiBufferUsage);

        /**
         * Sets a value to JPEG buffer usage register indicating the percentage of 
         * of the image buffer used for JPEG image data.  Specifying a value less 
         * than the maximum allows for room in the image buffer to accommodate extra
         * image data, depending on scene variations from frame to frame.
         *
         * Used if ladybugGetAutoJPEGQualityControlFlag() is true.
         *
         * A uiBufferUsage value of 0 is treated as follows:
         * - 0x66 (80%) for Ladybug2 
         * - 0x72 (90%) for Ladybug3 firmware v1.2.2.1 or later
         *
         * @param context       - The LadybugContext to access.     
         * @param uiBufferUsage - An integer from 0x00 (0%) to 0x7F (100%) indicating the 
         *                        percentage of the image buffer used for JPEG image data.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see   ladybugGetAutoJPEGBufferUsage()
         *   ladybugSetAutoJPEGQualityControlFlag(),
         *   ladybugGetAutoJPEGQualityControlFlag(),
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetAutoJPEGBufferUsage", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetAutoJPEGBufferUsage(IntPtr context, uint uiBufferUsage);

        /**
         * Gets the auto JPEG compression quality control flag.
         *
         * @param context                  - The LadybugContext to access.
         * @param autoJPEGQualityControl   - Location to return the auto JPEG quality 
         *                                   control flag. If the value is true, then
         *                                   the camera is automatically controlling the
         *                                   compression quality.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see   ladybugSetAutoJPEGQualityControlFlag(),
         *   ladybugGetAutoJPEGBufferUsage(),
         *   ladybugSetAutoJPEGBufferUsage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetAutoJPEGQualityControlFlag", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetAutoJPEGQualityControlFlag(IntPtr context,
                             out bool autoJPEGQualityControl);

        /**
         * Sets the auto JPEG compression quality control flag.
         *
         * @param context                 - The LadybugContext to access.
         * @param autoJPEGQualityControl  - A flag indicating if the Ladybug camera should
         *                                  automatically control JPEG compression            
         *                                  quality. If the value is true, then
         *                                  the camera will automatically control the
         *                                  compression quality.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetAutoJPEGQualityControlFlag(),
         *   ladybugGetAutoJPEGBufferUsage(),
         *   ladybugSetAutoJPEGBufferUsage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetAutoJPEGQualityControlFlag", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetAutoJPEGQualityControlFlag(IntPtr context,
                             bool autoJPEGQualityControl);

        /*@}*/

        /** 
         * @defgroup ManagedTriggerMethods Trigger and Strobe methods
         *
         * The functions in this group allow the user to control triggering and strobe
         * functions.
         * 
         * @ingroup Ladybug_cs
         */

        /*@{*/ 

        /**
         * This function retrieves information from the camera about the 
         * trigger feature.
         *
         * @param context          - The LadybugContext to access.
         * @param triggerModeInfo  - Structure that receives the information from 
         *                           the camera about the trigger.
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetTriggerMode(),
         *   ladybugSetTriggerMode()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetTriggerModeInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetTriggerModeInfo(
            IntPtr context,
            out LadybugTriggerModeInfo triggerModeInfo);

        /**
         * This function gets the current settings for the trigger feature. 
         *
         * @param context      - The LadybugContext to access.
         * @param triggerMode  - Structure that receives the current settings 
         *                       from the camera about the trigger.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetTriggerModeInfo(), ladybugSetTriggerMode()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetTriggerMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetTriggerMode(
            IntPtr context,
            out LadybugTriggerMode triggerMode);

        /**
         * This function sets the trigger settings on the camera. This will also set 
         * the specified GPIO pin to an input for trigger input.
         * 
         * For Ladybug2, the image grabbed at the trigger is
         * the image of the previous trigger, resulting in a one-trigger
         * delay.
         *
         * @param context      - The LadybugContext to access.
         * @param triggerMode  - Structure that provides the settings to be 
         *                       written to the camera.
         * @param broadcast    - Flag indicating whether setting of the 
         *                       trigger should be broadcast.   
         *
         * @return A LadybugError indicating the success of the function.
         *
         * Remarks:
         *
         * @see ladybugGetTriggerMode(), ladybugGetTriggerModeInfo()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetTriggerMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetTriggerMode(
            IntPtr context,
            LadybugTriggerMode triggerMode,
            bool broadcast = false);

        /**
         * This function retrieves information about the strobe feature
         * from the camera.
         *
         * @param context     - The LadybugContext to access.
         * @param strobeInfo  - Structure that receives the information from the 
         *                      camera about the strobe. The uiSource parameter
         *                      must contain the source pin to be queried.
         *
         * @return   A LadybugError indicating the success of the function.
         *
         * @see ladybugGetStrobe(), ladybugSetStrobe()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetStrobeInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetStrobeModeInfo(
            IntPtr context,
            out LadybugStrobeInfo strobeInfo);

        /**
         * This function gets the current settings for the strobe feature. 
         * Note that the strobe pin must be specified in the structure 
         * before being passed in as an argument.
         *
         * @param context        - The LadybugContext to access.
         * @param strobeControl  - Structure that receives the current settings 
         *                         from the camera about the strobe. The uiSource 
         *                         parameter must contain the source pin to be queried.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetStrobeInfo(), ladybugSetStrobe()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetStrobe", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetStrobeMode(
            IntPtr context,
            out LadybugStrobeControl strobeControl);

        /**
         * This function sets up the strobe and turns it on for the
         * specified source. Note that this function will also set the 
         * GPIO pin to an output for strobe.
         *
         * @param context        - The LadybugContext to access.
         * @param strobeControl - Structure that provides the settings to be 
         *                         written to the camera.
         * @param broadcast      - Flag indicating whether setting of the strobe 
         *                         feature should be broadcast.    
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetStrobe(), ladybugGetStrobeInfo()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetStrobe", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetStrobe(
            IntPtr context,
            LadybugStrobeControl strobeControl,
            bool broadcast = false);

        /**
         * Sets an internal flag indicating if image statistics data will be 
         * calculated in ladybugConvertImage().
         *
         * @param context          - The LadybugContext to access.
         * @param imageStatistics  - Flag indicating whether image statistics data
         *                           will be calculated.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetImageStatisticsFlag(), ladybugGetImageStatistics(), ladybugConvertImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetImageStatisticsFlag", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetImageStatisticsFlag(IntPtr context, bool imageStatistics);

        /**
         * Retrieves a flag indicating if image statistics are calculated in 
         * ladybugConvertImage. Off by default.
         *
         * @param context           - The LadybugContext to access.
         * @param imageStatistics   - Location to return the image statistics flag.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugSetImageStatisticsFlag(), ladybugGetImageStatistics(), ladybugConvertImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetImageStatisticsFlag", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetImageStatisticsFlag(IntPtr context, out bool imageStatistics);

        /**
         * Gets image statistics data for the specified Ladybug camera unit.  
         *
         * If the image statistics flag has not been set by calling
         * ladybugSetImageStatisticsFlag(), this function returns
         * LADYBUG_FAILED.
         *
         * The bValid flag in LadybugImageStatistics indicates whether the
         * data returned for this channel is valid.
         *
         * For example, ladybugGetImageStatistics(context, 2, &imageStats )
         * will return the image statistics data for camera 2, where
         * imageStats is defined as LadybugImageStatistics.
         *
         * @param context     - The LadybugContext to access.
         * @param camera      - The camera index corresponding to the requested image 
         *                      statistics data.
         * @param statistics  - The pointer to LadybugImageStatistics to return 
         *                      the statistics data
         * 
         * @return A LadybugError indicating the success of the function. 
         *
         * @see ladybugSetImageStatisticsFlag(), ladybugGetImageStatisticsFlag(),
         *      ladybugConvertImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetImageStatistics", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetImageStatistics(IntPtr context, uint camera, out LadybugImageStatistics statistics);

        /**
         * Perform a one-shot auto white balance adjustment.
         *
         * When this function is called, subsequent calls to 
         * ladybugConvertImage() will perform a one-shot auto white balance 
         * adjustment by modifying the relative gain of red and blue channels. 
         * Because Ladybug cameras do not have full hardware auto white balance 
         * functionality, white balance adjustment is initially performed in the 
         * software by analyzing images that are already grabbed and processed on the 
         * PC. The actual modification of white balance is then performed inside the 
         * camera. This half-software, half-hardware solution helps ensure better 
         * image quality over a software-only solution.
         *
         * Note that calling this function by itself does not adjust white balance. 
         *
         * Additionally, the application may need to call 
         * ladybugConvertImage() many times until proper white balance 
         * adjustment is achieved. You can get the current status of white balance 
         * adjustment by calling ladybugGetOneShotAutoWhiteBalanceStatus().
         *
         * @param context - The LadybugContext to access.
         * 
         * @return A LadybugError indicating the success of the function. 
         *
         * @see ladybugGetOneShotAutoWhiteBalanceStatus()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugDoOneShotAutoWhiteBalance", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError DoOneShotAutoWhiteBalance(IntPtr context);

        /**
         * Returns the status of a previously-called instance of 
         * ladybugDoOneShotAutoWhiteBalance. 
         *
         * A return value of LADYBUG_STILL_WORKING indicates white balance
         * adjustment is ongoing.
         *
         * A return value of LADYBUG_FAILED indicates white balance adjustment
         * failed.
         *
         * @param context - The LadybugContext to access.
         * 
         * @return A LadybugError indicating the success of the function. 
         * 
         * @see ladybugDoOneShotAutoWhiteBalance()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetOneShotAutoWhiteBalanceStatus", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetOneShotAutoWhiteBalanceStatus(IntPtr context);

        /*@}*/
    }

    unsafe public class Win32
    {
        // this can be used to copy from an unmanaged buffer to another unmanaged buffer.
        [DllImport("kernel32.dll")]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint len);
        [DllImport("kernel32.dll")]
        public static extern void RtlMoveMemory(IntPtr dest, IntPtr src, uint len); // works but slow?
    }

}

/*@}*/