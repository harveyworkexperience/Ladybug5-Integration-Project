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

#ifndef __PGRCAMERAGUI_H__
#define __PGRCAMERAGUI_H__

/** 
 * @defgroup Pgrcameragui_h pgrcameragui.h
 *
 *  pgrcameragui.h
 *
 *  Provides an API to display dialogs for:
 *   - selecting a camera on the bus.
 *   - altering camera properties.
 *
 *  This header file defines the API for all three of the "GUI" .DLLs  The
 *  behaviour of the settings dialog is defined by the lib .DLL stub that is
 *  linked to.  PGRFlyCaptureGUI.LIB will load PGRFlyCaptureGUI.DLL, 
 *  DigiclopsGUI.LIB will load DigiclopsGUI.DLL and LadybugGUI.LIB will load
 *  LadybugGUI.DLL.  PGRCameraGUI.LIB and PGRCameraGUI.DLL are no longer
 *  distributed.  Please link to the appropriate SDK-specific .LIB file (and
 *  .DLL), and use this header file.
 *
 */

/*@{*/

#ifdef PGRCAMERAGUI_EXPORTS
#define PGRCAMERAGUI_API __declspec( dllexport )
#else
#define PGRCAMERAGUI_API __declspec( dllimport )
#endif

#ifdef __cplusplus
extern "C"
{
#endif
   
/**
 *   A user level handle for the PGRGui object.
 */
typedef void*  CameraGUIContext;


/**
 *   A generic camera context to cast SDK contexts to before passing them in.
 */
typedef void*  GenericCameraContext;


/**
 *   Error codes returned from all PGRCameraGUI functions.
 */
typedef enum CameraGUIError
{
   /** Function completed successfully. */
   PGRCAMGUI_OK,                  
   
   /** Function failed. */
   PGRCAMGUI_FAILED,
   
   /** Was unable to create dialog. */
   PGRCAMGUI_COULD_NOT_CREATE_DIALOG,
   
   /** An invalid argument was passed. */
   PGRCAMGUI_INVALID_ARGUMENT,
   
   /** An invalid context was passed. */
   PGRCAMGUI_INVALID_CONTEXT,
   
   /** Memory allocation error. */
   PGRCAMGUI_MEMORY_ALLOCATION_ERROR,

   /** There has been an internal camera error - call getLastError() */
   PGRCAMGUI_INTERNAL_CAMERA_ERROR,
      
} CameraGUIError;


/**
 *   Type of PGRCameraGUI settings dialog to display.  
 *
 * Remarks:
 *   This enum will be deprecated in the next version.  Please use the 
 *   SDK-specific .LIB and .DLL (see note above) and the new 
 *   pgrcamguiInitializeSettingsDialog() instead of pgrcamguiCreateSettingsDialog().
 */
typedef enum CameraGUIType
{
   /** PGRFlyCapture settings. */
   PGRCAMGUI_TYPE_PGRFLYCAPTURE,

   /** Digiclops Settings. */
   PGRCAMGUI_TYPE_DIGICLOPS,
   
   /** Ladybug settings. */
   PGRCAMGUI_TYPE_LADYBUG,

} CameraGUIType;


/**
 *    
 *    Allocates a PGRCameraGUI handle to be used in all successive calls.
 *    This function must be called before any other functions.
 *
 * @param    pcontext - a pointer to a PGRCameraGUI context to be created.
 *          
 * @return    PGRCAMGUI_OK - upon successful completion.
 */    
PGRCAMERAGUI_API CameraGUIError 
pgrcamguiCreateContext(
               CameraGUIContext* pcontext );


/**
 *    
 *    Frees memory associated with a given context.
 *
 * @param    context - context to destroy.
 *
 * @return    PGRCAMGUI_OK - upon successful completion.
 *    PGRCAMGUI_INVALID_CONTEXT - if context is null.
 */    
PGRCAMERAGUI_API CameraGUIError 
pgrcamguiDestroyContext(
            CameraGUIContext context );


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
PGRCAMERAGUI_API CameraGUIError 
pgrcamguiShowCameraSelectionModal(
                  CameraGUIContext       context,
                  GenericCameraContext   camcontext,
                  unsigned int*         pulSerialNumber,
                  INT_PTR*               pipDialogStatus );

/**
 *    
 *   This function is DEPRECATED.  Please use 
 *   pgrcamguiInitializeSettingsDialog()
 *
 * @return    PGRCAMGUI_OK - upon successful completion.
 *    PGRCAMGUI_FAILED - if the function failed.
 *    PGRCAMGUI_INVALID_CONTEXT - if context is NULL.
 */    
PGRCAMERAGUI_API CameraGUIError 
pgrcamguiCreateSettingsDialog(
                  CameraGUIContext       context,
                  CameraGUIType       type,
                  GenericCameraContext camcontext );


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
PGRCAMERAGUI_API CameraGUIError 
pgrcamguiInitializeSettingsDialog(
                                  CameraGUIContext       context,
                                  GenericCameraContext   camcontext );


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
PGRCAMERAGUI_API CameraGUIError 
pgrcamguiToggleSettingsWindowState(  
                   CameraGUIContext   context,
                   HWND              hwndParent );


/**
 *    
 *    Retrieves the state of the settings dialog.
 *
 * @param    context     - The PGRCameraGUI context to access.
 * @param   pbShowing   - A pointer to the returned state of the settings dialog.
 *
 * @return    PGRCAMGUI_OK - upon successful completion.
 *    PGRCAMGUI_FAILED - if the function failed.
 *    PGRCAMGUI_INVALID_CONTEXT - if context is null.
 */    
PGRCAMERAGUI_API CameraGUIError 
pgrcamguiGetSettingsWindowState(
                CameraGUIContext   context,
                BOOL*           pbShowing );


/**
 *    
 *    When used with Ladybug SDK version 1.6.0.1 and onwards, 
 *    this function is DEPRECATED.
 *
 *    Enables context sensitive help in the settings dialog by specifying the 
 *    help prefix to append topic specific pages to. eg:
 *    "..\\doc\\FlyCapture SDK help.chm::/FlyCap Demo Program/Camera Control Dialog"
 *    to which strings like "/Look Up Table.html" will be appended.
 *
 * @param    context       - The PGRCameraGUI context to access.
 * @param   pszHelpPrefix - a pointer to a string containing the desired help prefix
 *                    if NULL the context sensitive help will be unavailable.
 *
 * @return    PGRCAMGUI_OK - upon successful completion.
 *    PGRCAMGUI_FAILED - if the function failed.
 *    PGRCAMGUI_INVALID_CONTEXT - if context is null.
 */    
PGRCAMERAGUI_API CameraGUIError
pgrcamguiSetSettingsWindowHelpPrefix(
                                     CameraGUIContext context,
                                     const char* pszHelpPrefix );


/**
 *    
 *    When used with Ladybug SDK version 1.6.0.1 and onwards, 
 *    this function is DEPRECATED.
 *
 *    Displays a modal dialog that displays PGR version information.
 *
 * @param    context     - The PGRCameraGUI context to access.
 * @param   camcontext  - The SDK-level context to use.
 * @param   hwndParent  - Handle to the parent window.
 * @param   pszAppName  - Pointer to a string that will be prepended to the version
 *                  information and seperated by a newline.
 *
 * @return    PGRCAMGUI_OK - upon successful completion.
 */    
PGRCAMERAGUI_API CameraGUIError 
pgrcamguiShowInfoDlg(
                     CameraGUIContext      context,
                     GenericCameraContext  camcontext,
                     HWND                  hwndParent,
                     char*                 pszAppName = NULL );

/*@}*/

#ifdef __cplusplus
};
#endif

#endif  // !__PGRCAMERAGUI_H__
