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

#ifndef __LADYBUGIMAGEADJUSTMENT_H__
#define __LADYBUGIMAGEADJUSTMENT_H__

/** 
 * @defgroup ladybugImageAdjustment_h ladybugImageAdjustment.h
 * 
 *  ladybugImageAdjustment.h
 *
 *    Defines the interface needed to adjust image that were not adjusted on camera.
 *
 *    This is the case for Ladybug5 when the data format is either of the 
 *    following formats:
 *    - Raw12
 *    - Raw16
 *    - JPEG12 
 *
 *    The image adjustment step happens as part of ladybugConvertImage().
 *    It happens after JPEG decompression (if required) and debayering. 
 *    If the user does not set an adjustment, a default adjustment will be applied. 
 *
 *    The software adjustment can be disabled or modified by calling 
 *    ladybugSetAdjustmentParameters() which will apply on future images.
 *
 *    This feature gives a lot of power for image post-processing.
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

/** Possible adjustment states for an adjustment parameter. */
typedef enum LadybugAdjustmentType
{
        DISABLE, /**< Adjustment parameter is disabled. */
        AUTOMATIC, /**< Adjustment parameter value is automatically determined. */
        MANUAL, /**< Adjustment parameter is manually specified. */
        LADYBUG_ADJUSTMENT_TYPE_SIZE /**< The number of values defined in LadybugAdjustmentType. */
} LadybugAdjustmentType;

/** Possible gain modes for exposure algorithm. */
typedef enum LadybugGainAdjustmentType
{
    GAIN_DISABLE, /**< No gain is applied, equal to 0dB of gain. */
    GAIN_MANUAL, /**< User specified gain is applied. */
    GAIN_FIX_EXPOSURE, /**< Gain is automatically adjustment to meet a specified pixel average. */
    GAIN_AUTOMATIC_COMPENSATION, /**< Gain is automatically adjusted. EV compensation can be specified. */
	GAIN_AUTOMATIC_COMPENSATION_INDEPENDENT, /**< Gain is automatically adjusted independently for each camera. EV compensation can be specified. */
    LADYBUG_GAIN_ADJUSTMENT_TYPE_SIZE /**< The number of values defined in LadybugGainAdjustmentType. */
} LadybugGainAdjustmentType;


/** Types of smear correction algorithms. */
typedef enum LadybugSmearCorrectionType
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
} LadybugSmearCorrectionType;

/**
 * Defines values for refering to specific fields in the LadybugAdjustmentParameters struct.
 *
 * For example, LADYBUG_ADJUSTMENT_PARAMETERS_BLACKLEVEL, refers to the blackLevel field.
 */
typedef enum LadybugAdjustmentParametersField
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
} LadybugAdjustmentParametersField;

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
typedef struct LadybugAdjustmentParameters
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
    bool doAdjustment;

    /** Whether to perform black level adjustment. */
    bool blackLevelAdjustmentType;

    /** 
     * BlackLevel adjustment value. Applies a correction of 
     * 256 * 256 * blackLevel to every pixel of the RGB image.
     * A value between 0.00 and 0.07 is acceptable. The default value is 0.0. 
     */
    float blackLevel; 
                 
    /** Gain adjustment type  */
    LadybugGainAdjustmentType gainAdjustmentType;

    /** Region of interest for auto gain */
    LadybugAutoExposureRoi gainRoi;

    /** 
     * Determines if gamma should be taken in account in the automatic gain adjustment. 
     * Has an impact only when gainAdjustmentType is automatic and gamma is on.
     */
    bool considerGammaInGainAdjustment;

    /** 
     * When manual gain is specified, the amount of gain applied to the image (in dB). 
     * A value between 0.0 and 24.0 is acceptable.
     * The default value is 0.0.
     */
    float gainManualValue; 

    /** 
     * When gain is fix exposure, the library will attempt to adjust the
     * the gain in order to achieve the specified target mean.
     * A value between 0 and 65535 is acceptable.
     * The default value is 32768.
     */
    float exposureTarget; 
    
    /** 
     * When gain adjustment type is GAIN_AUTOMATIC_COMPENSATION, 
     * the library will determine the gain automatically. This compensation
     * indicates to the library if the user prefer a darker or brighter solution.
     * A value between -100 and 100 is acceptable.
     * The default value is 0.
     */
    float exposureCompensation;
    
    /** White balance adjustment type. */
    LadybugAdjustmentType whiteBalanceAdjustmentType;

    /** Gain applied to red channel (in dB relative to Green). A value between -12 and 12 is acceptable. */
    float gainRed_ManualValue;

    /** Gain applied to blue channel. (in dB relative to Green). A value between -12 and 12 is acceptable. */
    float gainBlue_ManualValue;
    
    /** Gamma adjustment type (Disable or Manual only). */
    bool gammaAdjustmentType;

    /** Gamma value. A value between 0.5 and 4 is acceptable. */
    float gammaManualValue;

    /** Smear correction to be performed. */
    LadybugSmearCorrectionType smearAlgo;

    /** Whether to perform noise reduction on the image. */
    bool noiseReduction;

    /** Saturation value. A value of 0.0 will fully desaturate the image. The default value is 1.0. */
    float saturation;

	/** The black levelling type to be performed (ie. low intensity pixels). */
	LadybugAdjustmentType blackLevellingType;

	/** The white levelling type to be performed (ie. high intensity pixels). */
	LadybugAdjustmentType whiteLevellingType;

	/**
     * The manual black point value to use (maximum is 0.0, minimum is 1.0, default is 0.0).
     * The following must be satisifed as well: blackPointManual + 0.05 <= whitePointManual.
     */
	double blackPointManual;

	/**
     * The manual white point value to use (maximum is 0.0, minimum is 1.0, default is 1.0)
     * The following must be satisifed as well: blackPointManual + 0.05 <= whitePointManual.
     */
	double whitePointManual;

    LadybugAdjustmentParameters()
    {
        doAdjustment = true;
        blackLevelAdjustmentType = false;
        blackLevel = 0.0f;
        gainAdjustmentType = GAIN_DISABLE;
        gainRoi = LADYBUG_AUTO_EXPOSURE_ROI_FULL_IMAGE;
        considerGammaInGainAdjustment = false;
        gainManualValue = 0.0f;
        exposureTarget = 32768.0f;
        exposureCompensation = 0.0f;
        whiteBalanceAdjustmentType = DISABLE;
        gainRed_ManualValue = 0.0f;
        gainBlue_ManualValue = 0.0f;
        gammaAdjustmentType = false;
        gammaManualValue = 1.0f;
        smearAlgo = SMEAR_DISABLE;
        noiseReduction = false;
        saturation = 1.0f;
		blackLevellingType = AUTOMATIC;
		whiteLevellingType = AUTOMATIC;
		blackPointManual = 0.0;
		whitePointManual = 1.0;
    }

} LadybugAdjustmentParameters;

/** 
 * Wrapper struct to contain all the parameters that can be returned. 
 * This includes additional functionality that is not controlled directly 
 * via the LadybugAdjustmentParameters parameters.
 */
typedef struct LadybugFullAdjustmentParameters
{
    LadybugAdjustmentParameters postProcessingParams;
    LadybugToneMappingParams toneMappingParams;
    bool isSharpeningEnabled;
    bool isFalseColorRemovalEnabled;

    LadybugFullAdjustmentParameters()
    {
        isSharpeningEnabled = false;
        isFalseColorRemovalEnabled = false;
    }

} LadybugFullAdjustmentParameters;


/* Specifies types of inspections that can be performed with
 * ladybugInspectLadybugImage().
 */
typedef enum LadybugImageInspection
{
	/* Sets the white point (white balance and gain affected) so that the given
	 * region is full unsaturated white (RGB = 254,254,254).
	 *     
	 * Affects LadybugAdjustmentParameters:
	 *     gainManualValue, gainAdjustmentType, gainRed_ManualValue,
	 *     gainBlue_ManualValue, whiteBalanceAdjustmentType.
	 */
	WHITE_POINT_INSPECTION,

	/* Sets the grey point (white balance affected) so that the given region
	 * is a shade of grey (eg. RGB = 128,128,128 or RGB = 250,250,250).
	 *     
	 * Affects LadybugAdjustmentParameters: gainRed_ManualValue,
	 *     gainBlue_ManualValue, whiteBalanceAdjustmentType.
	 */
	GREY_POINT_INSPECTION,

	/* Number of LadybugImageInspection values */
	LADYBUG_IMAGE_INSPECTION_SIZE
} LadybugImageInspection;

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
LADYBUGDLL_API LadybugError
ladybugSetAdjustmentParameters(
    LadybugContext context,
    const LadybugAdjustmentParameters& param );

/**
 * Get the adjustment parameters from the specified context.
 *
 * @param context The LadybugContext to access.
 * @param param Adjustment parameters
 *
 * @return A LadybugError indicating the success of the function. 
 */
LADYBUGDLL_API LadybugError
ladybugGetAdjustmentParameters(
    LadybugContext context,
    LadybugAdjustmentParameters& param );

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
LADYBUGDLL_API LadybugError 
ladybugInspectLadybugImage( 
    LadybugContext context,
	LadybugImageInspection inspectionType,
	LadybugImage* pImage,
	unsigned int uiCamera,
	LadybugImageRegion region,
	LadybugAdjustmentParameters* pInOutAdjustment);

/**
 * Retrieves the minimum allowed value for the specified field.
 *
 * @param context - The LadybugContext to access.
 * @param field   - The field of interest.
 * @param pfValue - The value will be put into this argument.
 *
 * @return A LadybugError indicating the success of the function.
 */
LADYBUGDLL_API LadybugError
ladybugGetImageAdjustmentMin(
    LadybugContext context,
	LadybugAdjustmentParametersField field,
	float* pfValue);

/**
 * Retrieves the maximum allowed value for the specified field.
 *
 * @param context - The LadybugContext to access.
 * @param field   - The field of interest.
 * @param pfValue - The value will be put into this argument.
 *
 * @return A LadybugError indicating the success of the function.
 */
LADYBUGDLL_API LadybugError
ladybugGetImageAdjustmentMax(
    LadybugContext context,
	LadybugAdjustmentParametersField field,
	float* pfValue);

/*@}*/ 

#ifdef __cplusplus
};
#endif


#endif // ladybugImageAdjustment_h_15_3_2012_14_56