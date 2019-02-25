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
 * @defgroup LadybugFunctors_cs LadybugFunctors.cs
 * 
 *  LadybugFunctors.cs
 *
 *    This file defines useful functions to be used with Ladybug cameras.
 *
 *  We welcome your bug reports, suggestions, and comments: 
 *  www.ptgrey.com/support/contact
 */

/*@{*/

//=============================================================================
// System Includes
//=============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

//=============================================================================
// PGR Includes
//=============================================================================
using LadybugAPI;

namespace LadybugAPI
{
    public class LadybugFunctors
    {
        //=============================================================================
        /// <summary>
        /// Given a <c>LadybugPixelFormat</c>, returns the number of pixels per channel.
        /// </summary>
        /// <param name="format"><c>LadybugPixelFormat</c>.</param>
        /// <returns>Number of pixels per channel for specified pixel format.</returns>
        //=============================================================================
        public static int ToBytePerChannel(LadybugPixelFormat format)
        {
            if (Is8bitInt(format))
            {
                return 1;
            }
            else if (Is16bitInt(format) || Is16bitFloat(format))
            {
                return 2;
            }
            else if (Is32bitFloat(format))
            {
                return 4;
            }
            else
            {
                return -1;
            } 
        }

        //=============================================================================
        /// <summary>
        /// Checks if a given <c>LadybugPixelFormat</c> is 8-bit int.
        /// </summary>
        /// <param name="pixelFormat"><c>LadybugPixelFormat</c>.</param>
        /// <returns>Returns true if format is 8-bit int, false otherwise.</returns>
        //=============================================================================
        public static bool Is8bitInt(LadybugPixelFormat pixelFormat)
        {
            return (pixelFormat == LadybugPixelFormat.LADYBUG_MONO8 ||
                    pixelFormat == LadybugPixelFormat.LADYBUG_RAW8 ||
                    pixelFormat == LadybugPixelFormat.LADYBUG_BGR ||
                    pixelFormat == LadybugPixelFormat.LADYBUG_BGRU);
        }

        //=============================================================================
        /// <summary>
        /// Checks if a given <c>LadybugPixelFormat</c> is a 16-bit int.
        /// </summary>
        /// <param name="pixelFormat"><c>LadybugPixelFormat</c>.</param>
        /// <returns>Returns true if format is a 16-bit int, false otherwise.</returns>
        //=============================================================================
        public static bool Is16bitInt(LadybugPixelFormat pixelFormat)
        {
            return (pixelFormat == LadybugPixelFormat.LADYBUG_MONO16 ||
                    pixelFormat == LadybugPixelFormat.LADYBUG_RAW16 ||
                    pixelFormat == LadybugPixelFormat.LADYBUG_BGR16 ||
                    pixelFormat == LadybugPixelFormat.LADYBUG_BGRU16);
        }

        //=============================================================================
        /// <summary>
        /// Checks if a given <c>LadybugPixelFormat</c> is a 16-bit float.
        /// </summary>
        /// <param name="pixelFormat"><c>LadybugPixelFormat</c>.</param>
        /// <returns>Returns true if format is 16-bit float, false otherwise.</returns>
        //=============================================================================
        public static bool Is16bitFloat(LadybugPixelFormat pixelFormat)
        {
            return (pixelFormat == LadybugPixelFormat.LADYBUG_BGR16F);
        }

        //=============================================================================
        /// <summary>
        /// Checks if a given <c>LadybugPixelFormat</c> is a 32-bit float.
        /// </summary>
        /// <param name="pixelFormat"><c>LadybugPixelFormat</c>.</param>
        /// <returns>Returns true if format is 32-bit float, false otherwise.</returns>
        //=============================================================================
        public static bool Is32bitFloat(LadybugPixelFormat pixelFormat)
        {
            return (pixelFormat == LadybugPixelFormat.LADYBUG_BGR32F);
        }

        //=============================================================================
        /// <summary>
        /// Given a <c>LadybugColorProcessingMethod</c>, returns the associated scale factor.
        /// </summary>
        /// <param name="method"><c>LadybugColorProcessingMethod</c>.</param>
        /// <returns>Scale factor uint.</returns>
        //=============================================================================
        public static uint GetScaleFactor(LadybugColorProcessingMethod method)
        {
            switch (method)
            {
                case LadybugColorProcessingMethod.LADYBUG_DISABLE: return 1;
                case LadybugColorProcessingMethod.LADYBUG_EDGE_SENSING: return 1;
                case LadybugColorProcessingMethod.LADYBUG_NEAREST_NEIGHBOR_FAST: return 1;
                case LadybugColorProcessingMethod.LADYBUG_RIGOROUS: return 1;
                case LadybugColorProcessingMethod.LADYBUG_HQLINEAR: return 1;
                case LadybugColorProcessingMethod.LADYBUG_HQLINEAR_GPU: return 1;
                case LadybugColorProcessingMethod.LADYBUG_DIRECTIONAL_FILTER: return 1;
                case LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE4: return 2;
                case LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE16: return 4;
                case LadybugColorProcessingMethod.LADYBUG_MONO: return 2;
                default:
                    var msge = string.Format("GetScaleFactor error, proper case not found. Input format was: {0}", method.ToString());
                    Debug.Assert(false, msge);
                    return 1;
            }
        }

        //=============================================================================
        /// <summary>
        /// Takes a <c>LadybugPixelFormat</c> and returns the corresponding number of
        /// channels.
        /// </summary>
        /// <param name="format"><c>LadybugPixelFormat</c>.</param>
        /// <returns>Number of channels used for the <c>LadybugPixelFormat.</c></returns>
        //=============================================================================
        public static int ToNumChannels(LadybugPixelFormat format)
        {
            switch (format)
            {
                case LadybugPixelFormat.LADYBUG_MONO8:
                case LadybugPixelFormat.LADYBUG_RAW8:
                case LadybugPixelFormat.LADYBUG_MONO16:
                case LadybugPixelFormat.LADYBUG_RAW16:
                    return 1;
                case LadybugPixelFormat.LADYBUG_BGR:
                case LadybugPixelFormat.LADYBUG_BGR16:
                case LadybugPixelFormat.LADYBUG_BGR16F:
                case LadybugPixelFormat.LADYBUG_BGR32F:
                    return 3;
                case LadybugPixelFormat.LADYBUG_BGRU:
                case LadybugPixelFormat.LADYBUG_BGRU16:
                    return 4;
                default:
                    var msge = string.Format("ToNumChannels error, proper case not found. Input format was: {0}", format.ToString());
                    Debug.Assert(false, msge);
                    return -1;
            }
        }

        //=============================================================================
        /// <summary>
        /// Converts the given <c>LadybugDataFormat</c> into its string representation.
        /// </summary>
        /// <param name="format"><c>LadybugDataFormat</c> to convert.</param>
        /// <returns>String value of the <c>LadybugDataFormat</c>.</returns>
        //=============================================================================
        static public string ToAbbrevString(LadybugDataFormat format)
        {
            switch (format)
            {
                case LadybugDataFormat.LADYBUG_DATAFORMAT_RAW8:
                    return "RAW8";
                case LadybugDataFormat.LADYBUG_DATAFORMAT_JPEG8:
                    return "JPEG8";
                case LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_RAW8:
                    return "COLOR_SEP_RAW8";
                case LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_JPEG8:
                    return "COLOR_SEP_JPEG8";
                case LadybugDataFormat.LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW8:
                    return "HALF_HEIGHT_RAW8";
                case LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG8:
                    return "COLOR_SEP_HALF_HEIGHT_JPEG8";
                case LadybugDataFormat.LADYBUG_DATAFORMAT_RAW16:
                    return "RAW16";
                case LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_JPEG12:
                    return "COLOR_SEP_JPEG12";
                case LadybugDataFormat.LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW16:
                    return "HALF_HEIGHT_RAW16";
                case LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG12:
                    return "COLOR_SEP_HALF_HEIGHT_JPEG12";
                case LadybugDataFormat.LADYBUG_DATAFORMAT_RAW12:
                    return "RAW12";
                case LadybugDataFormat.LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW12:
                    return "HALF_HEIGHT_RAW12";
                case LadybugDataFormat.LADYBUG_DATAFORMAT_ANY:
                    return "ANY";
                default:
                    var msge = string.Format("ToAbbrevString error, proper case not found. Input format was: {0}", format.ToString());
                    Debug.Assert(false, msge);
                    return "Unknown";
            }
        }
    }

    public static class dataFormat
    {
        //=============================================================================
        /**
         * Method:    isImplemented
         * Description : Return true if the format is implemented and
         *               is concrete (return false for LADYBUG_DATAFORMAT_ANY
         *
         * @param format
         *
         * @return
         */
        //=============================================================================
        public static bool isImplemented(LadybugDataFormat format)
        {
            switch (format)
            {
                case LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_JPEG8:
                case LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG8:
                case LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_JPEG12:
                case LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG12:
                case LadybugDataFormat.LADYBUG_DATAFORMAT_RAW16:
                case LadybugDataFormat.LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW16:
                case LadybugDataFormat.LADYBUG_DATAFORMAT_RAW12:
                case LadybugDataFormat.LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW12:
                case LadybugDataFormat.LADYBUG_DATAFORMAT_RAW8:
                case LadybugDataFormat.LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW8:
                    return true;
                default:
                    return false;
            }
        }

        //=============================================================================
        /**
         * Method:    isHalfHeight
         * Description :
         *
         * @param format
         *
         * @return True if format is half, false if it is full
         */
        //=============================================================================
        public static bool isHalfHeight(LadybugDataFormat format)
        {
            switch (format)
            {
                case LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG8:
                case LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG12:
                case LadybugDataFormat.LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW16:
                case LadybugDataFormat.LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW12:
                case LadybugDataFormat.LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW8:
                    return true;
                default:
                    return false;
            }
        }

        //=============================================================================
        /**
         * Method:    isUncompressed
         * Description :
         *
         * @param format
         *
         * @return  True if format is RAW, false if it is JPEG
         */
        //=============================================================================
        public static bool isUncompressed(LadybugDataFormat format)
        {
            System.Diagnostics.Debug.Assert(isImplemented(format));

            return (format == LadybugDataFormat.LADYBUG_DATAFORMAT_RAW8 ||
                format == LadybugDataFormat.LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW8 ||
                format == LadybugDataFormat.LADYBUG_DATAFORMAT_RAW12 ||
                format == LadybugDataFormat.LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW12 ||
                format == LadybugDataFormat.LADYBUG_DATAFORMAT_RAW16 ||
                format == LadybugDataFormat.LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW16);
        }

        //=============================================================================
        /**
         * Method:    isJPEG
         * Description :
         *
         * @param format
         *
         * @return True if format is JPEG, false if it is RAW
         */
        //=============================================================================
        public static bool isJPEG(LadybugDataFormat format)
        {
            System.Diagnostics.Debug.Assert(isImplemented(format));

            return (format == LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_JPEG8 ||
                format == LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG8 ||
                format == LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_JPEG12 ||
                format == LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG12);
        }

        //=============================================================================
        /**
         * Method: is12bit
         * Description : Checks if the specified data format is 12 bit.
         *
         * @param format
         *
         * @return True if format is 1.5 bytes/pixel, false if it is 1.
         */
        //=============================================================================
        public static bool is12bit(LadybugDataFormat format)
        {
            System.Diagnostics.Debug.Assert(isImplemented(format));

            return (format == LadybugDataFormat.LADYBUG_DATAFORMAT_RAW12 ||
                format == LadybugDataFormat.LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW12 ||
                format == LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_JPEG12 ||
                format == LadybugDataFormat.LADYBUG_DATAFORMAT_COLOR_SEP_HALF_HEIGHT_JPEG12);
        }

        //=============================================================================
        /**
         * Method:    isHighBitDepth
         * Description : Checks if the specified data format is high bit depth (either 12 or 16 bit)
         *
         * @param format
         *
         * @return True if format is 12 or 16 bits wide, false otherwise.
         */
        //=============================================================================
        public static bool isHighBitDepth(LadybugDataFormat format)
        {
            System.Diagnostics.Debug.Assert(isImplemented(format));

            bool is16Bit = (format == LadybugDataFormat.LADYBUG_DATAFORMAT_RAW16 ||
                format == LadybugDataFormat.LADYBUG_DATAFORMAT_HALF_HEIGHT_RAW16);

            return (is12bit(format) || is16Bit);
        }
    }
}

/*@}*/