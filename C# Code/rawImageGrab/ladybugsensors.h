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

#ifndef __LADYBUGSENSORS_H__
#define __LADYBUGSENSORS_H__

/** 
 * @defgroup LadybugSensors_h ladybugsensors.h
 * 
 *  ladybugsensors.h
 *
 *    This file defines the interface to the environmental sensors on a Ladybug camera.
 *    These sensors are not available on Ladybug3 or earlier cameras.
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

/** The type of sensor. */
typedef enum LadybugSensorType
{
    TEMPERATURE, /**< Temperature sensor. */
    HUMIDITY, /**< Humidity sensor. */
    BAROMETER, /**< Barometer. */
    COMPASS, /**< 3-axis compass. */
    ACCELEROMETER, /**< 3-axis accelerometer. */
    GYROSCOPE /**< 3-axis gyroscope. */

} LadybugSensorType;


/** Information about a given sensor. */
typedef struct _LadybugSensorInfo
{
    /** Whether the sensor is supported by the current camera. */
    bool isSupported;

    /** The minimum value of the sensor. */
    float min;

    /** The maximum value of the sensor. */
    float max;

    /** Textual representation of the sensor units. */
    char units[256];

    /** Abbreviated textual representation of the sensor units. */
    char unitsAbbr[256];

} LadybugSensorInfo;

/**
 * Get the current value of the specified sensor.
 *
 * @param context The LadybugContext to access.
 * @param sensorType The sensor to be accessed.
 * @param pValue The value returned by the sensor.
 *
 * @return A LadybugError indicating the success of the function.
 */
LADYBUGDLL_API LadybugError
ladybugGetSensor(LadybugContext context, LadybugSensorType sensorType, float* pValue);

/**
 * Get the current value of the specified 3-axis sensor.
 *
 * @param context The LadybugContext to access.
 * @param sensorType The sensor to be accessed.
 * @param pValue The values returned by the sensor.
 *
 * @return A LadybugError indicating the success of the function.
 */
LADYBUGDLL_API LadybugError
ladybugGetSensorAxes(LadybugContext context, LadybugSensorType sensorType, LadybugTriplet* pValue);

/**
 * Get information about the specified sensor.
 *
 * @param context The LadybugContext to access.
 * @param sensorType The sensor to be accessed.
 * @param pInfo The information returned by the sensor.
 *
 * @return A LadybugError indicating the success of the function.
 */
LADYBUGDLL_API LadybugError
ladybugGetSensorInfo(LadybugContext context, LadybugSensorType sensorType, LadybugSensorInfo* pInfo);

/*@}*/

#ifdef __cplusplus
};
#endif

#endif
