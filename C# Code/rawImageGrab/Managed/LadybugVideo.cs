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
 * @defgroup LadybugVideo_cs LadybugVideo.cs
 *
 *  LadybugVideo.cs
 *
 *  This file defines the interface of Ladybug SDK's video-specific functions.
 *  If your C# project uses Ladybug SDK's video specific functions, this file 
 *  must also be added to your project along with Ladybug_Managed.cs.
 * 
 *  We welcome your bug reports, suggestions, and comments: 
 *  www.ptgrey.com/support/contact
 */

/*@{*/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LadybugAPI
{
	/** Options for saving H.264 files. */
    unsafe public struct LadybugH264Option
    {
        /** Frame rate of the stream */
        public float frameRate;

        /** Width of source image */
        public uint width;

        /** Height of source image */
        public uint height;

        /** Bitrate to encode at */
        public uint bitrate;

        /** Reserved for future use */
        public fixed uint reserved[256];
    };

    // This class defines static functions to access most of the
    // Ladybug APIs defined in ladybugvideo.h
	unsafe public partial class Ladybug
    {
        /** 
         * @defgroup ManagedVideoContextCreationMethods Video Context Creation and Initialization Methods
         *
         * This group of functions provides control over video functionality.
         * 
         * @ingroup LadybugVideo_cs
         */

        /*@{*/ 

        /**
         * Creates a new context for accessing video-specific functions of the
         * library.  
         *
         * A video context is useful for creating a video clip of the stitched image.
         * Currently, only H.264 codec is supported for creating a video.
         *
         * @param context - A pointer to a LadybugVideoContext to fill with the created context.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugDestroyVideoContext()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugCreateVideoContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError CreateVideoContext(out IntPtr context);

        /**
         * Frees memory associated with the LadybugVideoContext.
         *
         * @param context - A pointer to the LadybugVideoContext to destroy.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugCreateVideoContext()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugDestroyVideoContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError DestroyVideoContext(ref IntPtr context);

        /**
         * Opens a video file. 
         * Once opened, you can append as many frames as you want to the video. 
         * After finishing appending frames, you must close the video.
         *
         * @param context   - The video context. This must be created beforehand.
         * @param fileName  - The file path to save.
         * @param options   - The options for the codec.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugCreateVideoContext()
         * @see ladybugCloseVideo()
         * @see ladybugAppendVideoFrame()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugOpenVideo", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError OpenVideo(IntPtr context, ref string fileName, ref LadybugH264Option option);

        /**
         * Closes a video file.
         *
         * @param context - The video context.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugCreateVideoContext()
         * @see ladybugOpenVideo()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugCloseVideo", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError CloseVideo(IntPtr context);

        /**
         * Appends a frame to the video.
         * The video must be opened before appending frames.
         *
         * @param context - The video context.
         * @param image  - An image to be appended. This is created by ladybugRenderOffScreenImage.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugCreateVideoContext()
         * @see ladybugOpenVideo()
         * @see ladybugRenderOffScreenImage()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugAppendVideoFrame", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError AppendVideoFrame(IntPtr context, ref LadybugProcessedImage image);

        /*@}*/
	}
}

/*@}*/