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
 * @defgroup PGRCameraGUI_cs PGRCameraGUI.cs
 *
 *  PGRCameraGUI.cs
 *
 *  This file defines the interface of Ladybug SDK's GUI-related functions.
 *  Provides an API to display dialogs for:
 *      - selecting a camera on the bus.
 *      - altering camera properties.
 *  If your C# project uses Ladybug SDK's GUI functions, this file must also be
 *  added to your project along with Ladybug_Managed.cs.
 *
 */

/*@{*/

using System;
using System.Runtime.InteropServices;

namespace LadybugAPI
{
	//
	// Description:
	//   Error codes returned from all PGRCameraGUI functions.
	//
	public enum CameraGUIError
	{
		// Function completed successfully.
		PGRCAMGUI_OK,
		// Function failed.
		PGRCAMGUI_FAILED,
		// Was unable to create dialog.
		PGRCAMGUI_COULD_NOT_CREATE_DIALOG,
		// An invalid argument was passed.
		PGRCAMGUI_INVALID_ARGUMENT,
		// An invalid context was passed.
		PGRCAMGUI_INVALID_CONTEXT,
		// Memory allocation error.
		PGRCAMGUI_MEMORY_ALLOCATION_ERROR,
		// There has been an internal camera error - call getLastError()
		PGRCAMGUI_INTERNAL_CAMERA_ERROR,
	};

	/**
	 *   Type of PGRCameraGUI settings dialog to display.
	 *
	 * Remarks:
	 *   This enum will be deprecated in the next version.  Please use the
	 *   SDK-specific .LIB and .DLL (see note above) and the new
	 *   pgrcamguiInitializeSettingsDialog() instead of pgrcamguiCreateSettingsDialog().
	 */
	public enum CameraGUIType
	{
	   /** PGRFlyCapture settings. */
	   PGRCAMGUI_TYPE_PGRFLYCAPTURE,

	   /** Digiclops Settings. */
	   PGRCAMGUI_TYPE_DIGICLOPS,

	   /** Ladybug settings. */
	   PGRCAMGUI_TYPE_LADYBUG,

	};

    // This class defines static functions to access most of the
    // Ladybug APIs defined in pgrcameragui.h
    unsafe public class CameraGUI
    {
        private const string LADYBUG_GUI_DLL = "LadybugGUI.dll";

        /**
         *    
         *    Allocates a PGRCameraGUI handle to be used in all successive calls.
         *    This function must be called before any other functions.
         *
         * @param    context - a pointer to a PGRCameraGUI context to be created.
         *          
         * @return    PGRCAMGUI_OK - upon successful completion.
         */  
        [DllImport(LADYBUG_GUI_DLL, EntryPoint = "pgrcamguiCreateContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraGUIError CreateContext(out IntPtr context);

        /**
         *    
         *    Frees memory associated with a given context.
         *
         * @param    context - context to destroy.
         *
         * @return    PGRCAMGUI_OK - upon successful completion.
         *    PGRCAMGUI_INVALID_CONTEXT - if context is null.
         */
        [DllImport(LADYBUG_GUI_DLL, EntryPoint = "pgrcamguiDestroyContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraGUIError DestroyContext(IntPtr context);

        /**
         *    
         *    Displays the camera selection dialog.
         *
         * @param    context           - The PGRCameraGUI context to access.
         * @param    camcontext        - The SDK-specific context to use, casted appropriately.
         *                        (ie, FlycaptureContext, DigiclopsContext, etc.)
         * @param    pulSerialNumber   - Pointer to the returned serial number of the selected
         *                        camera.
         * @param    pipDialogStatus   - Status returned from the DoModal() call from the dialog.
         *                        Use this to check for "Ok" or "Cancel."
         *
         * @return    PGRCAMGUI_OK - upon successful completion.
         *    PGRCAMGUI_FAILED - if the function failed.
         *    PGRCAMGUI_INVALID_CONTEXT - if context is NULL.
         */   
        [DllImport(LADYBUG_GUI_DLL, EntryPoint = "pgrcamguiShowCameraSelectionModal", CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraGUIError ShowCameraSelectionModal(
                      IntPtr context,
                      IntPtr camcontext,
                      out int pulSerialNumber,
                      out int pipDialogStatus);

        /**
         *    
         *    Creates the settings dialog.  Call this before calling the either of the
         *    other two functions dealing with the Settings dialog.
         *
         * @param    context     - The PGRCameraGUI context to access.
         * @param   camcontext  - The SDK-level context to use.
         *
         * @return    PGRCAMGUI_OK - upon successful completion.
         *    PGRCAMGUI_FAILED - if the function failed.
         *    PGRCAMGUI_INVALID_CONTEXT - if context is null.
         */    
        [DllImport(LADYBUG_GUI_DLL, EntryPoint = "pgrcamguiInitializeSettingsDialog", CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraGUIError InitializeSettingsDialog(
                      IntPtr context,
                      IntPtr camcontext);

        /**
         *    
         *    Displays or hides the modeless settings dialog.
         *
         * @param    context     - The PGRCameraGUI context to access.
         * @param   hwndParent  - Handle to the parent window.
         *
         * @return    PGRCAMGUI_OK - upon successful completion.
         *    PGRCAMGUI_FAILED - if the function failed.
         *    PGRCAMGUI_INVALID_CONTEXT - if context is null.
         */   
        [DllImport(LADYBUG_GUI_DLL, EntryPoint = "pgrcamguiToggleSettingsWindowState", CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraGUIError ToggleSettingsWindowState(
                   IntPtr context,
                   IntPtr hwndParent);

        /**
         *    
         *    Retrieves the state of the settings dialog.
         *
         * @param    context     - The PGRCameraGUI context to access.
         * @param   showing   - A pointer to the returned state of the settings dialog.
         *
         * @return    PGRCAMGUI_OK - upon successful completion.
         *    PGRCAMGUI_FAILED - if the function failed.
         *    PGRCAMGUI_INVALID_CONTEXT - if context is null.
         */    
        [DllImport(LADYBUG_GUI_DLL, EntryPoint = "pgrcamguiGetSettingsWindowState", CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraGUIError GetSettingsWindowStat(IntPtr context, out bool showing);
    }
}

/*@}*/