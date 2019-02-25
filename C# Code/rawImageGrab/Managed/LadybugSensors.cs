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
 * @defgroup LadybugSensors_cs LadybugSensors.cs
 * 
 *  LadybugSensors.cs
 *
 *    This file defines the interface to the environmental sensors on a Ladybug camera.
 *    These sensors are not available on Ladybug3 or earlier cameras.
 *
 *  We welcome your bug reports, suggestions, and comments: 
 *  www.ptgrey.com/support/contact
 */

/*@{*/

using System;
using System.Runtime.InteropServices;

namespace LadybugAPI
{
	/** The type of sensor. */
	public enum LadybugSensorType
	{
		TEMPERATURE, /**< Temperature sensor. */
		HUMIDITY, /**< Humidity sensor. */
		BAROMETER, /**< Barometer. */
		COMPASS, /**< 3-axis compass. */
		ACCELEROMETER, /**< 3-axis accelerometer. */
		GYROSCOPE /**< 3-axis gyroscope. */

	};

	/** Information about a given sensor. */
	[StructLayout(LayoutKind.Sequential)]
	unsafe public struct LadybugSensorInfo
	{
        /** Whether the sensor is supported by the current camera. */
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool isSupported;

		/** The minimum value of the sensor. */
		public float min;

		/** The maximum value of the sensor. */
		public float max;

		/** Textual representation of the sensor units. */
		[MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string units;

		/** Abbreviated textual representation of the sensor units. */
		[MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string unitsAbbr;
	};

    // This class defines static functions to access most of the
    // Ladybug APIs defined in ladybugsensors.h
    unsafe public partial class Ladybug
    {
        /**
         * Get the current value of the specified sensor.
         *
         * @param context The LadybugContext to access.
         * @param sensorType The sensor to be accessed.
         * @param value The value returned by the sensor.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetSensor", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetSensor(IntPtr context, LadybugSensorType sensorType, out float value);

        /**
         * Get the current value of the specified 3-axis sensor.
         *
         * @param context The LadybugContext to access.
         * @param sensorType The sensor to be accessed.
         * @param value The values returned by the sensor.
         *
         * @return A LadybugError indicating the success of the function.
         */
		[DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetSensorAxes", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetSensorAxes(IntPtr context, LadybugSensorType sensorType, out LadybugTriplet value);

        /**
         * Get information about the specified sensor.
         *
         * @param context The LadybugContext to access.
         * @param sensorType The sensor to be accessed.
         * @param value The information returned by the sensor.
         *
         * @return A LadybugError indicating the success of the function.
         */
		[DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetSensorInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetSensorInfo(IntPtr context, LadybugSensorType sensorType, out LadybugSensorInfo value);
	}	
}

/*@}*/