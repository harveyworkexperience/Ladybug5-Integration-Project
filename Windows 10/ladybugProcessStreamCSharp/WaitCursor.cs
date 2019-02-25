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
using System;
using System.Windows.Forms;

public class WaitCursor : IDisposable
{
    public WaitCursor()
    {
        Enabled = true;
    }

    public void Dispose()
    {
        Enabled = false;
    }

    public static bool Enabled
    {
        get { return Application.UseWaitCursor; }

        set
        {
            if (value == Application.UseWaitCursor)
            {
                return;
            }

            Application.UseWaitCursor = value;
            Form f = Form.ActiveForm;
            if (f != null && !f.InvokeRequired && f.Handle != IntPtr.Zero)   // Send WM_SETCURSOR
            {
                SendMessage(f.Handle, 0x20, f.Handle, (IntPtr)1);
            }
        }
    }
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
}
