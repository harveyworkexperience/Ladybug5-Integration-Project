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
using LadybugAPI;

namespace ladybugProcessStream_CSharp
{
    public class ContextHolder : IDisposable
    {
        public ContextHolder()
        {
            LadybugError error;
            if (mainContext == IntPtr.Zero)
            {                
                error = Ladybug.CreateContext(out mainContext);
            }

            if (streamContext == IntPtr.Zero)
            {                
                error = Ladybug.CreateStreamContext(out streamContext);
            }
        }

        public void Dispose()
        {
            LadybugError error;
            if (streamContext != IntPtr.Zero)
            {
                error = Ladybug.DestroyStreamContext(ref streamContext);
            }

            if (mainContext != IntPtr.Zero)
            {
                error = Ladybug.DestroyContext(ref mainContext);
            }

            streamContext = IntPtr.Zero;
            mainContext = IntPtr.Zero;
        }

        public IntPtr GetContext() { return mainContext; }
        public IntPtr GetStreamContext() { return streamContext; }

        private IntPtr mainContext = IntPtr.Zero;
        private IntPtr streamContext = IntPtr.Zero;
    }
}