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
// $Id: LadybugFunctors.h 135702 2011-12-13 22:46:15Z soowei $
//=============================================================================
//
// LadybugFunctors.h : header file
// Group all functors related to Ladybug.h enums
//

#ifndef LadybugFunctors_h_23_11_2011_11_25
#define LadybugFunctors_h_23_11_2011_11_25

#include "ladybug.h"
#include "ladybugrenderer.h"
#include <string>

namespace dataFormat
{
    bool isImplemented              ( LadybugDataFormat format );
    bool isHalfHeight               ( LadybugDataFormat format );
    bool isUncompressed             ( LadybugDataFormat format );
    bool isJPEG                     ( LadybugDataFormat format );
    bool is16bit                    ( LadybugDataFormat format );
    int  toBytePerChannel           ( LadybugDataFormat format );
    int  toBytePerPixel             ( LadybugDataFormat format );
    LadybugPixelFormat toPixelFormatWithU( LadybugDataFormat format );
};

namespace pixelFormat
{
    int  toBytePerChannel  ( LadybugPixelFormat format);
    int  toChanPerPixel    ( LadybugPixelFormat format);
    int  toBytePerPixel    ( LadybugPixelFormat format );
}

namespace outputImage
{
    std::string toString(LadybugOutputImage imageType);
}




#endif // LadybugFunctors_h_23_11_2011_11_25