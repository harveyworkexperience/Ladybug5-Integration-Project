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

/** 
 * @defgroup LadybugImageAdjustment_cs LadybugImageAdjustment.cs
 * 
 *  LadybugImageAdjustment.cs
 *
 *
 *      This file defines the needed to adjust image that were not adjusted on camera.
 *
 *      This is the case for Ladybug5 when the data format is either of the 
 *      following formats:
 *      - Raw12
 *      - Raw16
 *      - JPEG12 
 *
 *      The image adjustment step happens as part of ladybugConvertImage().
 *      It happens after JPEG decompression (if required) and debayering.  
 *      If the user does not set an adjustment, a default adjustment will be applied. 
 *
 *      The software adjustment can be disabled or modified by calling 
 *      ladybugSetAdjustmentParameters() which will apply on future images.
 *
 *      This feature gives a lot of power for image post-processing.
 *
 *  If your C# project uses Ladybug SDK's post adjustment functions, this file
 *  must also be added to your project along with Ladybug_Managed.cs.
 *
 *  We welcome your bug reports, suggestions, and comments: 
 *  www.ptgrey.com/support/contact
 */

/*@{*/

using System;
using System.Runtime.InteropServices;

namespace LadybugAPI
{
	/** Possible adjustment states for an adjustment parameter. */
    public enum LadybugAdjustmentType
    {
        DISABLE, /**< Adjustment parameter is disabled. */
        AUTOMATIC, /**< Adjustment parameter value is automatically determined. */
        MANUAL, /**< Adjustment parameter is manually specified. */
        LADYBUG_ADJUSTMENT_TYPE_SIZE /**< The number of values defined in LadybugAdjustmentType. */
    };

	/** Possible gain modes for exposure algorithm. */
    public enum LadybugGainAdjustmentType
    {
        GAIN_DISABLE, /**< No gain is applied, equal to 0dB of gain. */
        GAIN_MANUAL, /**< User specified gain is applied. */
        GAIN_FIX_EXPOSURE, /**< Gain is automatically adjustment to meet a specified pixel average. */
        GAIN_AUTOMATIC_COMPENSATION, /**< Gain is automatically adjusted. EV compensation can be specified. */
        GAIN_AUTOMATIC_COMPENSATION_INDEPENDENT, /**< Gain is automatically adjusted independently for each camera. EV compensation can be specified. */
        LADYBUG_GAIN_ADJUSTMENT_TYPE_SIZE /**< The number of values defined in LadybugGainAdjustmentType. */
    };

	/** Types of smear correction algorithms. */
    public enum LadybugSmearCorrectionType
    {
        SMEAR_DISABLE, /**< No smear correction is performed. */
        SMEAR_REMOVE, /**< Correct unsaturated smear only. */

        /** 
         * Correct unsaturated and saturated smear. Saturated smear is corrected
         * by performing interpolation against adjacent pixels.
         */
        SMEAR_REMOVE_FILL,

        /** The number of values defined in LadybugSmearCorrectionType. */
        LADYBUG_SMEAR_CORRECTION_TYPE_SIZE
    };

    /**
     * Defines values for refering to specific fields in the LadybugAdjustmentParameters struct.
     *
     * For example, LADYBUG_ADJUSTMENT_PARAMETERS_BLACKLEVEL, refers to the blackLevel field.
     */
    public enum LadybugAdjustmentParametersField
    {
	    LADYBUG_ADJUSTMENT_PARAMETERS_BLACKLEVEL,
	    LADYBUG_ADJUSTMENT_PARAMETERS_GAIN_MANUAL_VALUE,
	    LADYBUG_ADJUSTMENT_PARAMETERS_EXPOSURE_TARGET,
	    LADYBUG_ADJUSTMENT_PARAMETERS_EXPOSURE_COMPENSATION,
	    LADYBUG_ADJUSTMENT_PARAMETERS_GAIN_RED_MANUAL_VALUE,
	    LADYBUG_ADJUSTMENT_PARAMETERS_GAIN_BLUE_MANUAL_VALUE,
	    LADYBUG_ADJUSTMENT_PARAMETERS_GAMMA_MANUAL_VALUE,
	    LADYBUG_ADJUSTMENT_PARAMETERS_SATURATION,

	     /** The number of elements in the LadybugAdjustmentParametersField enum */
	    LADYBUG_ADJUSTMENT_PARAMETERS_SIZE
    };

	/**
	 * Defines an image adjustment to be applied to the raw image.
	 *
	 * To apply these settings, set the parameters to the library via
	 * ladybugSetAdjustmentParameters(). These settings will then be applied the
	 * next time ladybugConvertImage() or ladybugConvertImageEx() is called.
	 *
	 * These are the types of adjustment that can be applied:
	 * - Black Level
	 * - Gain (including regions of interest for auto gain)
	 * - White balance
	 * - Gamma
	 * - Smear correction
	 */
	[StructLayout(LayoutKind.Sequential)]
    unsafe public struct LadybugAdjustmentParameters
    {
        /** 
         * Whether to perform any image adjustments.
         *
         * If true, adjustment will take effect in ladybugConvertImage() and 
         * ladybugConvertImageEx() on any image 
         * that was not previously adjusted by the camera.
         *
         * If false, it disables any adjustment and displays the image as received 
         * from the camera (i.e. the raw image). 
         *
         * Note that disabling individual parameters is not enough to disable 
         * everything since some default image adjustment will always be performed
         * regardless of image settings.
         */ 
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool doAdjustment;

        /** Whether to perform black level adjustment. */
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool blackLevelAdjustmentType;

        /** 
         * BlackLevel adjustment value. Applies a correction of 
         * 256 * 256 * blackLevel to every pixel of the RGB image.
         * A value between 0.00 and 0.07 is acceptable. The default value is 0.0. 
         */
        public float blackLevel;

        /** Gain adjustment type  */
        public LadybugGainAdjustmentType gainAdjustmentType;

        /** Region of interest for auto gain */
        public LadybugAutoExposureRoi gainRoi;

        /** 
         * Determines if gamma should be taken in account in the automatic gain adjustment. 
         * Has an impact only when gainAdjustmentType is automatic and gamma is on.
         */
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool considerGammaInGainAdjustment;

        /** 
         * When manual gain is specified, the amount of gain applied to the image (in dB). 
         * A value between 0.0 and 24.0 is acceptable.
         * The default value is 0.0.
         */
        public float gainManualValue;

        /** 
         * When gain is fix exposure, the library will attempt to adjust the
         * the gain in order to achieve the specified target mean.
         * A value between 0 and 65535 is acceptable.
         * The default value is 32768.
         */
        public float exposureTarget;

        /** 
         * When gain adjustment type is GAIN_AUTOMATIC_COMPENSATION, 
         * the library will determine the gain automatically. This compensation
         * indicates to the library if the user prefer a darker or brighter solution.
         * A value between -100 and 100 is acceptable.
         * The default value is 0.
         */
        public float exposureCompensation;

        /** White balance adjustment type. */
        public LadybugAdjustmentType whiteBalanceAdjustmentType;

        /** Gain applied to red channel (in dB relative to Green). A value between -12 and 12 is acceptable. */
        public float gainRed_ManualValue;

        /** Gain applied to blue channel. (in dB relative to Green). A value between -12 and 12 is acceptable. */
        public float gainBlue_ManualValue;

        /** Gamma adjustment type (Disable or Manual only). */
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool gammaAdjustmentType;

        /** Gamma value. A value between 0.5 and 4 is acceptable. */
        public float gammaManualValue;

        /** Smear correction to be performed. */
        public LadybugSmearCorrectionType smearAlgo;

        /** Whether to perform noise reduction on the image. */
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool noiseReduction;

        /** Saturation value. A value of 0.0 will fully desaturate the image. The default value is 1.0. */
        public float saturation;

        /** The black levelling type to be performed (ie. low intensity pixels). */
        public LadybugAdjustmentType blackLevellingType;

        /** The white levelling type to be performed (ie. high intensity pixels). */
        public LadybugAdjustmentType whiteLevellingType;

        /**
         * The manual black point value to use (maximum is 0.0, minimum is 1.0, default is 0.0).
         * The following must be satisifed as well: blackPointManual + 0.05 <= whitePointManual.
         */
        public double blackPointManual;

        /**
         * The manual white point value to use (maximum is 0.0, minimum is 1.0, default is 1.0)
         * The following must be satisifed as well: blackPointManual + 0.05 <= whitePointManual.
         */
        public double whitePointManual;
    };

    /** 
     * Wrapper struct to contain all the parameters that can be returned. 
     * This includes additional functionality that is not controlled directly 
     * via the LadybugAdjustmentParameters parameters.
     */
    unsafe public struct LadybugFullAdjustmentParameters
    {
        public LadybugAdjustmentParameters postProcessingParams;
        public LadybugToneMappingParams toneMappingParams;

        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool isSharpeningEnabled;
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool isFalseColorRemovalEnabled;
    };

    /* Specifies types of inspections that can be performed with
     * ladybugInspectLadybugImage().
     */
    public enum LadybugImageInspection
    {
	    /* Sets the grey point (white balance affected) so that the given region
	     * is a shade of grey (eg. RGB = 128,128,128 or RGB = 250,250,250).
	     *     
	     * Affects LadybugAdjustmentParameters: gainRed_ManualValue,
	     *     gainBlue_ManualValue, whiteBalanceAdjustmentType.
	     */
	    GREY_POINT_INSPECTION,

	    /* Number of LadybugImageInspection values */
	    LADYBUG_IMAGE_INSPECTION_SIZE
    };

    // This class defines static functions to access most of the
    // Ladybug APIs defined in ladybugImageAdjustment.h
	unsafe public partial class Ladybug
    {
        /**
         * Set the adjustment parameters to the specified context.
         *
         * These parameters will apply to any unprocessed images sent by the camera and
         * will have no effect on images that are processed on the camera.
         *
         * If this function is not called before image conversion, the system will 
         * use a default adjustment.
         * 
         * These parameters take effect when converting the image using 
         * ladybugConvertImage() or ladybugConvertEx().
         *
         * @param context The LadybugContext to access.
         * @param param Adjustment parameters
         *
         * @return A LadybugError indicating the success of the function. If the
         *         parameters specified as invalid, an error will be returned.
         *
         * @see ladybugConvertImage(), ladybugConvertImageEx()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetAdjustmentParameters", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetAdjustmentParameters(IntPtr context, ref LadybugAdjustmentParameters param);

        /**
         * Get the adjustment parameters from the specified context.
         *
         * @param context The LadybugContext to access.
         * @param param Adjustment parameters
         *
         * @return A LadybugError indicating the success of the function. 
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetAdjustmentParameters", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetAdjustmentParameters(IntPtr context, ref LadybugAdjustmentParameters param);

        /**
         * Inspect a location in a LadybugImage and set parameters based on the type
         * of inspection being performed.
         *
         * Inspection types:
         *   WhitePointInspection requires : uiCamera, region.
         *   GreyPointInspection requires  : uiCamera, region.
         *
         * @param context          - The LadybugContext to access.
         * @param inspectionType   - The type of inspection to perform.
         * @param pImage           - The image to perform the inspection on.
         * @param uiCamera         - The camera on which to perform the inspection.
         * @param region           - The area over which to perform the inspection.
         *                           May be unused depending on the inspection type.
         * @param pInOutAdjustment - The adjustment parameters that will be modified
         *                           based on the inspection type and the contents
         *                           of the ladybug image.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugInspectLadybugImage", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError InspectLadybugImage(
            IntPtr context,
            LadybugImageInspection inspectionType,
	        ref LadybugImage pImage,
	        uint uiCamera,
	        LadybugImageRegion region,
	        out LadybugAdjustmentParameters pInOutAdjustment);

        /**
         * Retrieves the minimum allowed value for the specified field.
         *
         * @param context - The LadybugContext to access.
         * @param field   - The field of interest.
         * @param fValue  - The value will be put into this argument.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetImageAdjustmentMin", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetImageAdjustmentMin(
            IntPtr context,
            LadybugAdjustmentParametersField field,
            out float fValue);

        /**
         * Retrieves the maximum allowed value for the specified field.
         *
         * @param context - The LadybugContext to access.
         * @param field   - The field of interest.
         * @param fValue - The value will be put into this argument.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetImageAdjustmentMax", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetImageAdjustmentMax(
            IntPtr context,
            LadybugAdjustmentParametersField field,
            out float fValue);
	}
}

/*@}*/