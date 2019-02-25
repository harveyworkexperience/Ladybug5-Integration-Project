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

#ifndef __LADYBUGGEOM_H__
#define __LADYBUGGEOM_H__

/** 
 * @defgroup Ladybuggeom_h ladybuggeom.h
 * 
 *  ladybuggeom.h
 *
 *    This file defines the geometry-related Ladybug functions: rectification,
 *    3d-mapping, etc.
 *
 *  We welcome your bug reports, suggestions, and comments: 
 *  www.ptgrey.com/support/contact
 *
 */

/*@{*/

#ifdef __cplusplus
extern "C"
{
#endif

#include "ladybug.h"

/** 
 * @defgroup Structures Structures
 */

/*@{*/ 

/**
 * A 3D point, in spherically-oriented coordinates and radial coordinates,
 * as well as cylindrical coordinates
 */
typedef struct LadybugPoint3d
{   
   float fX; /**< Spherical Coordinate X. */   
   float fY; /**< Spherical Coordinate Y. */   
   float fZ; /**< Spherical Coordinate Z. */   
   float fTheta; /**< Radial Coordinate Theta. Ranges from -PI (right) to +PI (left). */   
   float fPhi; /**< Radial Coordinate Phi. Ranges from zero (up) to PI (down). */   
   float fCylAngle; /**< Cylindrical Coordinate Angle, Ranges from -PI (right) to +PI (left). */   
   float fCylHeight; /**< Cylindrical Coordinate Height, actual projected height on the cylinder. */

} LadybugPoint3d;


/** An "image" of 3D points corresponding to values of the source image. */
typedef struct LadybugImage3d
{   
   unsigned int uiRows; /**< The number of rows in the image. */    
   unsigned int uiCols; /**< The number of columns in the image. */

   /** 
    * Transformation is applied to the 3D points before computing the spherical angles 
    * (in Euler angle convention; same as in ladybugGetCameraUnitExtrinsics(). ). 
    */
   double dRx;
   double dRy;
   double dRz;
   double dTx;
   double dTy;
   double dTz;
   
   float fCylHeightMin; /**< Cylindrical height minimum. */   
   float fCylHeightMax; /**< Cylindrical height maximum. */   
   LadybugPoint3d* ppoints;  /**< The area for the pixel data. */
   
} LadybugImage3d;

/**
 * Available viewing angle options for panoramic and spherical image rendering.
 *
 * The viewing angle is specified by choosing which camera views are the  
 * front and pole/down cameras.
 *
 * The front camera occupies the center region of the rendered image.
 *
 * The pole or down camera occupies the top or bottom region of the 
 * rendered image.
 *
 * Together they dictate the exact orientation in which all the cameras
 * must be rendered in order to generate an image that satisfies the 
 * placement of these two camera units.
 * 
 * This enum is used in calls to ladybugSetPanoramicViewingAngle() and 
 * ladybugGetPanoramicViewingAngle(). 
 */
typedef enum LadybugPanoAngle
{
   LADYBUG_DEFAULT_PANO,   /**< Default is LADYBUG_FRONT_0_POLE_5 */
   LADYBUG_FRONT_0_POLE_5, /**< front camera = 0, pole/top camera = 5 */
   LADYBUG_FRONT_1_POLE_5, /**< front camera = 1, pole/top camera = 5 */
   LADYBUG_FRONT_2_POLE_5, /**< front camera = 2, pole/top camera = 5 */
   LADYBUG_FRONT_3_POLE_5, /**< front camera = 3, pole/top camera = 5 */
   LADYBUG_FRONT_4_POLE_5, /**< front camera = 4, pole/top camera = 5 */
   LADYBUG_FRONT_5_POLE_0, /**< front camera = 5, pole/top camera = 0 */
   LADYBUG_FRONT_5_POLE_1, /**< front camera = 5, pole/top camera = 1 */
   LADYBUG_FRONT_5_POLE_2, /**< front camera = 5, pole/top camera = 2 */
   LADYBUG_FRONT_5_POLE_3, /**< front camera = 5, pole/top camera = 3 */
   LADYBUG_FRONT_5_POLE_4, /**< front camera = 5, pole/top camera = 4 */
   LADYBUG_FRONT_0_DOWN_5, /**< front camera = 0, down camera = 5 */
   LADYBUG_FRONT_1_DOWN_5, /**< front camera = 1, down camera = 5 */
   LADYBUG_FRONT_2_DOWN_5, /**< front camera = 2, down camera = 5 */
   LADYBUG_FRONT_3_DOWN_5, /**< front camera = 3, down camera = 5 */
   LADYBUG_FRONT_4_DOWN_5, /**< front camera = 4, down camera = 5 */
   LADYBUG_FRONT_5_DOWN_0, /**< front camera = 5, down camera = 0 */
   LADYBUG_FRONT_5_DOWN_1, /**< front camera = 5, down camera = 1 */
   LADYBUG_FRONT_5_DOWN_2, /**< front camera = 5, down camera = 2 */
   LADYBUG_FRONT_5_DOWN_3, /**< front camera = 5, down camera = 3 */
   LADYBUG_FRONT_5_DOWN_4, /**< front camera = 5, down camera = 4 */
   NUM_PANO_ANGLES,

   // Unused member to force this enum to compile to 32 bits.
   LADYBUG_PANOANGLE_FORCE_QUADLET = 0x7FFFFFFF,

} LadybugPanoAngle;

/**
 * The available panoramic image mapping types.
 *
 * This enumeration indicates the kind of coordinate mapping used to transform a 3D point
 * into a 2D representation.
 */
typedef enum LadybugMapType
{
   /** 
    * Radial Angle Mapping - all 6 cameras are mapped to a panoramic image 
    * where 3D is mapped to 2 angles 
    */
   LADYBUG_MAP_RADIAL,

   /** 
    * Cylindrical Mapping - only the 5 cameras in the horizontal ring are 
    * mapped to the panoramic image - 3D is mapped to an angle and height 
    */
   LADYBUG_MAP_CYLINDRICAL,  

} LadybugMapType;

/*@}*/ 

/** 
 * @defgroup GeneralMethods General Methods
 *
 * This group of functions provides the user with access to the geometry
 * related aspects of the library.
 */

/*@{*/ 

/**
 * Loads a configuration file containing intrinsic and extrinsic camera 
 * properties. 
 * 
 * This function must be called once before any calls involving 3D mapping
 * and rectification are performed. 
 * 
 * This function should also be called before beginning to transfer images with
 * ladybugStart(), ladybugStartLockNext(), etc.
 *
 * @param context - The LadybugContext to access.
 * @param pszPath - Path to the config file. Use NULL to attempt to load the file
 *                  from the camera head itself.
 *
 * @return A LadybugError indicating the success of the function. 
 *
 * @see ladybugWriteConfigurationFile()
 */
LADYBUGDLL_API LadybugError
ladybugLoadConfig( 
    LadybugContext context,
    const char* pszPath);

/**
 * Maps all images onto a sphere of fixed radius and returns the 3D mapping
 * coordinates to the sphere for the given camera.
 *
 * If the user calls this function with uiCamera = 1, it returns a
 * LadybugImage3d indicating the 3D position of every pixel from camera 1 on
 * the surface of the sphere.
 *
 * @param  context         - The LadybugContext to access.
 * @param  uiCamera        - The camera index to retrieve.
 * @param  uiGridCols      - Columns in the 3D grid to return.
 * @param  uiGridRows      - Rows in the 3D grid to return.
 * @param  uiSrcCols       - Columns in the source raw image to sample.
 * @param  uiSrcRows       - Rows in the source image to sample.
 * @param  bCenterSampling - Flag for whether samples are taken at the center of a
 *                           pixel, or from the edge dividing two pixels.                  
 * @param  ppimage         - Returned LadybugImage3d structure.  This should be
 *                           considered read-only.
 *
 * @return A LadybugError indicating the success of the function.       
 */
LADYBUGDLL_API LadybugError
ladybugGet3dMap( 
    LadybugContext context,
    unsigned int uiCamera,
    unsigned int uiGridCols,
    unsigned int uiGridRows,
    unsigned int uiSrcCols,
    unsigned int uiSrcRows,
    bool bCenterSampling,
    const LadybugImage3d** ppimage);

/**
 * Projects a 3D point (with respect to the Ladybug coordinate frames) onto  
 * the indicated camera unit and returns where it will falls on its rectified 
 * image.
 *
 * @param  context        - The LadybugContext to access.
 * @param  dLadybugX      - X coordinate of the point to project.
 * @param  dLadybugY      - Y coordinate of the point to project.
 * @param  dLadybugZ      - Z coordinate of the point to project.
 * @param  uiCamera       - Camera index this image corresponds to.
 * @param  pdRectifiedRow - The returned rectified row location where the 3D point 
 *                          falls (Will be less than 0 if the point does not 
 *                          project to the rectified image).
 * @param  pdRectifiedCol - The returned rectified column location where the 3D point 
 *                          falls (Will be less than 0 if the point does not 
 *                          project to the rectified image).
 * @param  pdNormalized   - The distance from the rectified pixel to the focal center
 *                          normalized by the focal length.  Can be set to NULL if
 *                          the caller is not interested in this value.
 *
 * @return A LadybugError indicating the success of the function.     
 */
LADYBUGDLL_API LadybugError
ladybugXYZtoRC(
    LadybugContext context,
    double dLadybugX,
    double dLadybugY,
    double dLadybugZ,
    unsigned int uiCamera,
    double* pdRectifiedRow,
    double* pdRectifiedCol,
    double* pdNormalized);

/**
 * Projects a 2D point on a specific camera unit into a 3D ray in the Ladybug
 * coordinate frame.  The ray is defined as its starting point and direction.
 * The starting point takes into account the camera unit's offset from the
 * center of the ladybug camera which allows accurate projection of the
 * ray at distances different from the stitching radius.
 *
 * @param  context       - The LadybugContext to access.
 * @param  dRectifiedRow - The rectified row of the 2D input point.
 * @param  dRectifiedCol - The rectified column of the 2D input point.
 * @param  uiCamera      - The camera unit.
 * @param  pdLocationX   - The output ray location x component.
 * @param  pdLocationY   - The output ray location y component.
 * @param  pdLocationZ   - The output ray location z component.
 * @param  pdDirectionX  - The output ray direction x component.
 * @param  pdDirectionY  - The output ray direction y component.
 * @param  pdDirectionZ  - The output ray direction z component.
 * 
 * @return A LadybugError indicating the success of the function.   
 */
LADYBUGDLL_API LadybugError
ladybugRCtoXYZ(
	LadybugContext context,
	double	 	   dRectifiedRow,
	double		   dRectifiedCol,
	unsigned int   uiCamera,
	double*	       pdLocationX,
	double*	       pdLocationY,
	double*	       pdLocationZ,
	double*	       pdDirectionX,
	double*	       pdDirectionY,
	double*	       pdDirectionZ);

/**
 * Sets the rotation of 3D mesh for a given set of front and pole cameras.
 * 
 * This call is equivalent to ladybugSet3dMapRotation where the rotation
 * is specified by which camera is in front and which is on top.
 *
 * @param context       - The LadybugContext to access.
 * @param panoViewAngle - The viewing angle to use. See LadybugPanoAngle.
 *
 * @return A LadybugError indicating the success of the function.   
 *
 * @see ladybugGetPanoramicViewingAngle()
 *   ladybugGet3dMapRotation()
 *   ladybugSet3dMapRotation()
 */
LADYBUGDLL_API LadybugError 
ladybugSetPanoramicViewingAngle(
    LadybugContext context,
    LadybugPanoAngle panoViewAngle );         

/**
 * Gets the current panoramic viewing angle. 
 *
 * @param context        - The LadybugContext to access.
 * @param pPanoViewAngle - Location to return the viewing angle.
 *
 * @return A LadybugError indicating the success of the function.   
 *
 * @see ladybugSetPanoramicViewingAngle()
 *   ladybugSet3dMapRotation()
 *   ladybugGet3dMapRotation()
 */
LADYBUGDLL_API LadybugError 
ladybugGetPanoramicViewingAngle(
    LadybugContext context,
    LadybugPanoAngle* pPanoViewAngle);

/**
 * Sets the rotation of 3D mesh. 
 * The representation of the rotation follows EulerZYX convention.
 *
 * @param  context - The LadybugContext to access.
 * @param  dRx     - Rotation angle about X-axis in radians.
 * @param  dRy     - Rotation angle about Y-axis in radians.
 * @param  dRz     - Rotation angle about Z-axis in radians.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugGet3dMapRotation(), ladybugGetCameraUnitExtrinsics()
 */
LADYBUGDLL_API LadybugError 
ladybugSet3dMapRotation( 
    LadybugContext context, 
    double dRx, 
    double dRy, 
    double dRz);

/**
 * Gets the rotation of the 3D mesh. 
 * 
 * The representation of the rotation follows EulerZYX convention.
 *
 * @param context - The LadybugContext to access.
 * @param pdRx    - Rotation angle about X-axis in radians.
 * @param pdRy    - Rotation angle about Y-axis in radians.
 * @param pdRz    - Rotation angle about Z-axis in radians.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugSet3dMapRotation(), ladybugGetCameraUnitExtrinsics()
 */
LADYBUGDLL_API LadybugError 
ladybugGet3dMapRotation( 
    LadybugContext context, 
    double* pdRx, 
    double* pdRy, 
    double* pdRz);

/**
 * Sets the radius of the sphere used for stitching. 
 * 
 * When stitching multiple images from a Ladybug camera, the Ladybug SDK 
 * assumes that all the scenes in the images are located at the same distance
 * from the camera. However, because of the parallax of the cameras, scenes
 * that are not at this distance will have a stitching error at the seam of 
 * the cameras.
 * 
 * With this function, users can adjust the distance of the scene to be 
 * stitched. The default value is 20m.
 * 
 * This function reinitializes the alpha masks if they have already been initialized.
 *
 * See http://www.ptgrey.com/support/kb/index.asp?a=4&q=250 for further
 * information.
 *
 * @param context - The LadybugContext to access.
 * @param dRadius - The radius of the sphere.
 *
 * @return A LadybugError indicating the success of the function.
 *   
 * @see ladybugGet3dMapSphereSize()
 */
LADYBUGDLL_API LadybugError 
ladybugSet3dMapSphereSize( 
    LadybugContext context, 
    double dRadius);

/**
 * Sets the radius of the sphere used for stitching. 
 *
 * @param context  - The LadybugContext to access.
 * @param pdRadius - The pointer to the radius of the sphere to receive.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugSet3dMapSphereSize()
 */
LADYBUGDLL_API LadybugError 
ladybugGet3dMapSphereSize( 
    LadybugContext context, 
    double *pdRadius); 

/**
 * Sets the dynamic stitching status.
 * 
 * When applied, the stitching distance is determined by inspecting
 * areas of the image where adjacent cameras overlap. This inspection is 
 * performed within small, localized areas, so that different stitching 
 * distances can be applied to different areas. Localized stitching is 
 * achieved by searching for the distance that best overlaps the images in 
 * each area.
 * 
 * If dynamic stitching is not applied, stitching is performed according to 
 * a fixed distance for the entire sphere.
 * 
 * Dynamic stitching is recommended when subjects in the scene are located 
 * within various distances and prominent stitching errors cannot be 
 * rectified using a fixed stitching distance.
 * 
 * Note that dynamic stitching is computationally intensive and may result in 
 * unexpected effects when a search for the best distance fails. 
 *
 * See http://www.ptgrey.com/support/kb/index.asp?a=4&q=250 for more
 * details on the stitching process.
 *
 * @param  context  - The LadybugContext to access.
 * @param  bOnePush - If true, dynamic stitching is applied to the current
 *                    3D map one time. Note that this option may result in
 *                    unexpected behaviour when image stabilization is enabled.
 *                    @see ladybugEnableImageStabilization()
 * @param  bAuto    - If true, dynamic stitching is executed every time
 *                    ladybugUpdateTextures() is called.
 *                    This auto status is turned off when the user calls 
 *                    ladybugSet3dMapSphereSize(). The default is false.                       
 * @param  pPoint   - The 3D coordinates where dynamic stitching should be 
 *                    applied. This parameter is useful when dynamic 
 *                    stitching should only be applied to a certain location 
 *                    in the image.
 *                    The fX, fY and fZ members of LadybugPoint3d struct must
 *                    be set. All other members are ignored.
 *                    When NULL, dynamic stitching is applied to the 
 *                    entire image.
 *                    This argument is valid only when bOnePush is true.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugSet3dMapSphereSize()
 *   ladybugGetDynamicStitching()
 *   ladybugSetDynamicStitchingParams()
 *   ladybugGetDynamicStitchingParams()
 *   LadybugDynamicStitchingParams struct  
 */
LADYBUGDLL_API LadybugError 
ladybugSetDynamicStitching( 
    LadybugContext context,
    bool bOnePush,
    bool bAuto, 
    const LadybugPoint3d* pPoint = NULL); 

/**
 * Gets the dynamic stitching status.
 *
 * @param  context - The LadybugContext to access.
 * @param  pbAuto  - Pointer to the auto status.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugSetDynamicStitching()
 *   ladybugSetDynamicStitchingParams()
 *   ladybugGetDynamicStitchingParams()
 */
LADYBUGDLL_API LadybugError 
ladybugGetDynamicStitching( 
    LadybugContext context,
    bool* pbAuto); 

/**
 * Sets the parameters for dynamic stitching.
 *
 * @param  context - The LadybugContext to access.
 * @param  pDynamicStitchingParams - Pointer to the parameter struct.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugSetDynamicStitching()
 *   ladybugGetDynamicStitching()
 *   ladybugGetDynamicStitchingParams()
 *   LadybugDynamicStitchingParams struct
 */
LADYBUGDLL_API LadybugError 
ladybugSetDynamicStitchingParams( 
    LadybugContext context, 
    const LadybugDynamicStitchingParams* pDynamicStitchingParams); 

/**
 * Gets the parameters for dynamic stitching.
 *
 * @param context - The LadybugContext to access.
 * @param pDynamicStitchingParams - The pointer to the parameter struct to receive.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugSetDynamicStitching()
 *   ladybugGetDynamicStitching()
 *   ladybugSetDynamicStitchingParams()
 */
LADYBUGDLL_API LadybugError 
ladybugGetDynamicStitchingParams( 
    LadybugContext context, 
    LadybugDynamicStitchingParams* pDynamicStitchingParams); 

/**
 * Sets the panoramic mapping type. 
 *
 * @param context - The LadybugContext to access.
 * @param mappingType - The panoramic mapping type to use. See LadybugMapType.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see ladybugGetPanoramicMappingType()
 */
LADYBUGDLL_API LadybugError 
ladybugSetPanoramicMappingType(
    LadybugContext context,
    LadybugMapType mappingType );             

/**
 * Gets the panoramic mapping type. 
 *
 * @param context      - The LadybugContext to access.
 * @param pMappingType - Location to return the mapping type. See LadybugMapType.
 *
 * @return A LadybugError indicating the success of the function.   
 *
 * @see ladybugSetPanoramicMappingType()
 */
LADYBUGDLL_API LadybugError 
ladybugGetPanoramicMappingType(
    LadybugContext context,
    LadybugMapType* pMappingType );

/**
 * Gets the focal length (in pixels) for the specified camera unit.  
 *
 * Because the focal length is associated with the rectified camera, 
 * this function must be called after ladybugSetOffScreenImageSize(), which 
 * sets the resolution of rectified images. 
 * 
 * It is assumed that the aspect ratio of the rectified image is
 * the same as that of the raw image. Otherwise, the vertical and horizontal
 * focal lengths are different. In this case, the returned focal length
 * indicates the horizontal one.
 *
 * @param context       - The LadybugContext to access.
 * @param uiCamera      - Camera index that this image corresponds to.
 * @param pdFocalLength - The returned rectified focal length.
 *
 * @return A LadybugError indicating the success of the function.    
 *
 * @see ladybugSetOffScreenImageSize()
 */
LADYBUGDLL_API LadybugError
ladybugGetCameraUnitFocalLength(
    LadybugContext context,
    unsigned int uiCamera,
    double* pdFocalLength  );

/**
 * Gets the rectified image center for the specified camera unit.  
 *
 * This function must be called after ladybugSetOffScreenImageSize(), which 
 * sets the resolution of rectified images. 
 *
 * @param  context   - The LadybugContext to access.
 * @param  uiCamera  - Camera index that this image corresponds to.
 * @param  pdCenterX - The returned X coordinate of the rectified image center.
 * @param  pdCenterY - The returned Y coordinate of the rectified image center.
 *
 * @return A LadybugError indicating the success of the function. 
 *
 * @see ladybugSetOffScreenImageSize()
 */
LADYBUGDLL_API LadybugError 
ladybugGetCameraUnitImageCenter(
    LadybugContext context,
    unsigned int uiCamera,
    double* pdCenterX,
    double* pdCenterY   );

/**
 * Gets the 6-D extrinsics vector for the specified camera unit.  
 *
 * The 6-D extrinsics vector is in EulerZYX convention (see Craig's 
 * Introduction to Robotics pg. 45-49).  The ordering of the extrinsics
 * components are :
 *  
 * - element 0 - Rx - Rotation about X (radians)
 * - element 1 - Ry - Rotation about Y (radians)
 * - element 2 - Rz - Rotation about Z (radians)
 * - element 3 - Tx - Translation along X (meters)
 * - element 4 - Ty - Translation along Y (meters)
 * - element 5 - Tz - Translation along Z (meters)
 *  
 * By extrinsics, we mean that the corresponding 4x4 transformation matrix 
 * allows one to map a point in the local camera unit coordinates to that 
 * of the Ladybug coordinate frame.  Where s=sin and c=cos, the format of the  
 * matrix is given below.   
 *
 * Example: 
 * To map this to Craig's matrix: 
 * - Rz = alpha
 * - Ry = beta
 * - Rx = gamma
 *
 * - |X'| |((cRz)(cRy)) ((cRz)(sRy)(sRx)-(sRz)(cRx)) ((cRz)(sRy)(cRx)+(sRz)(sRx)) Tx||X|
 * - |Y'|=|((sRz)(cRy)) ((sRz)(sRy)(sRx)+(cRz)(cRx)) ((sRz)(sRy)(cRx)-(cRz)(sRx)) Ty||Y|
 * - |Z'| |((-sRy))     ((cRy)(sRx))                 ((cRy)(cRx)))                Tz||Z|
 * - |1 | |0            0                            0                            1 ||1| 
 *
 * @param  context     - The LadybugContext to access.
 * @param  uiCamera    - Camera index of interest.
 * @param  ardEulerZYX - The returned 6-D EulerZYX extrinsics vector.
 *
 * @return A LadybugError indicating the success of the function.  
 */
LADYBUGDLL_API LadybugError
ladybugGetCameraUnitExtrinsics(
    LadybugContext context,
    unsigned int uiCamera,
    double ardEulerZYX[6] );

/**
 * Sets the Z axis by rotating the 3D mesh so that a pair of supplied lines 
 * become parallel to the Z axis.
 * 
 * In order to determine the new Z axis, the library needs two vertical lines
 * in the scene supplied by the user.
 * 
 * The lines can be specified by four points - 2 on each line.
 * 
 * To increase the precision of the estimation, the two points on each line
 * should be as distant as possible.
 * 
 * Also, the lines should not be too close to each other or too close to an 
 * exact opposite position (180 degrees apart).
 *
 * @param  context    - The LadybugContext to access.
 * @param  pPoints    - The 4 points on the two lines to which the Z axis should
 *                      be aligned.
 *                      The first two elements in the array must belong to one
 *                      line and the last two must belong to the other line.
 *                      The points are specified by the radial coordinates
 *                      (theta, phi) of the panoramic view. 
 *                      In the LadybugPoint3d struct, only the fTheta and fPhi members
 *                      must be set. All the other members are ignored.
 * @param  iNumPoints - This must be 4.
 *
 * @return A LadybugError indicating the success of the function. 
 *
 * @see   ladybugSet3dMapRotation()
 */
LADYBUGDLL_API LadybugError 
ladybugSetZAxis(
    LadybugContext context,
    const LadybugPoint3d* pPoints,
    int iNumPoints);

/*@}*/ 

/** 
 * @defgroup AlphaMaskMethods Alpha Mask Methods
 *
 * This group of functions provides control over the libraries alpha mask 
 * functionality.
 */

/*@{*/ 

/**
 * Initializes alpha mask buffers.
 * 
 * In order to properly blend adjacent images at the borders, the system
 * uses the concept of an alpha mask. The alpha mask indicates how much one
 * pixel contributes to the final display relative to its neighbors.
 * 
 * The alpha mask changes as the blending width changes or the stitching sphere size 
 * changes. Also, it changes when the size of the color processed image changes.
 * The bWriteToFile flag can be set to true if the generated alpha masks have to
 * be saved to a file and later be loaded with ladybugLoadAlphaMasks().
 *
 * @param  context      - The LadybugContext to access.
 * @param  uiCols       - Columns in the alpha mask. This should map to the number of 
 *                        columns in the image captured from each camera unit.
 * @param  uiRows       - Rows in the alpha mask. This should map to the number of 
 *                        rows in the image captured from each camera unit.
 * @param  bWriteToFile - A flag to control to write the alpha masks to a file.
 *
 * @return A LadybugError indicating the success of the function.
 *
 * @see   ladybugSetAlphaMaskPathname(), ladybugLoadAlphaMasks()
 */
LADYBUGDLL_API LadybugError
ladybugInitializeAlphaMasks(
    LadybugContext context,
    unsigned int uiCols,
    unsigned int uiRows,
    bool bWriteToFile = false);

/**
 * Sets the pathname of alpha mask files.
 *
 * The default value of the alpha mask pathname is NULL. 
 * If the pathname is set, Ladybug uses it to save alpha mask files.
 * Otherwise, the current directory is used.        
 *
 * @param  context      - The LadybugContext to access.
 * @param  pszFilename  - The pathname of the alpha mask files.
 *
 * @return A LadybugError indicating the success of the function. 
 *
 * @see   ladybugInitializeAlphaMasks(), ladybugLoadAlphaMasks()
 */
LADYBUGDLL_API LadybugError
ladybugSetAlphaMaskPathname( 
    LadybugContext context,
    const char* pszFilename);

/**
 * Loads alpha masks from the current path.
 *
 * This function is useful when using the alpha masks which were previously
 * generated and saved to a file instead of using the dynamically created 
 * alpha masks generated by ladybugInitializeAlphaMasks().
 *
 * This only attempts to load them from the current path, or the path specified
 * by ladybugSetAlphaMaskPathname().
 *
 * If it fails to load, it does not attempt to recalculate the alpha masks.
 *
 * @param  context - The LadybugContext to access.
 * @param  uiCols  - Columns in the alpha mask. This should map to the number of 
 *             columns in the image captured from each camera unit.
 * @param  uiRows  - Rows in the alpha mask. This should map to the number of 
 *             rows in the image captured from each camera unit.
 *
 * @return LADYBUG_OK - Alpha map was loaded successfully.
 * @return LADYBUG_COULD_NOT_OPEN_FILE - Alpha mask could not be located.        
 *
 * @see   ladybugInitializeAlphaMasks(), ladybugSetAlphaMaskPathname()
 */
LADYBUGDLL_API LadybugError
ladybugLoadAlphaMasks(
    LadybugContext context,
    unsigned int uiCols,
    unsigned int uiRows);

/**
 * If set to true, invokes the alpha masks to be copied to color images
 * on the next call to ladybugConvertImage().
 *
 * As long as you reuse the same color image buffers, you do not have to set
 * this to true every time.
 * 
 * Set bMasking to "false" if you are not doing any texture map blending.  
 * The default value is "true".
 *
 * @param  context  - The LadybugContext to access.
 * @param  bMasking - The state of alpha masking to set.
 *
 * @return A LadybugError indicating the success of the function. 
 * 
 * @see   ladybugConvertImage(), ladybugInitializeAlphaMasks()
 */
LADYBUGDLL_API LadybugError
ladybugSetAlphaMasking(
    LadybugContext context,
    bool bMasking );

/**
 * Sets the maximum blending (overlap between cameras) width in pixels. 
 *
 * The alpha masks will be reinitialized if they have already been initialized.
 *
 * The default blending width is 100.  
 *
 * @param  context           - The LadybugContext to access.
 * @param  dMaxBlendingWidth - The maximum blending width in pixels to set.
 *
 * @return A LadybugError indicating the success of the function.     
 */
LADYBUGDLL_API LadybugError
ladybugSetBlendingParams(
    LadybugContext context,
    double dMaxBlendingWidth );

/**
 * Gets the maximum blending (overlap between cameras) width in pixels.
 *
 * @param context            - The LadybugContext to access.
 * @param pdMaxBlendingWidth - Pointer to the location where the maximum blending
 *                             width in pixels is to be returned.
 *
 * @return A LadybugError indicating the success of the function.     
 */
LADYBUGDLL_API LadybugError
ladybugGetBlendingParams(
    LadybugContext context,
    double* pdMaxBlendingWidth );

/**
 * Retrieves the alpha mask corresponding to the indicated camera.
 *
 * Must be called after ladybugInitializeAlphaMasks() with the appropriate  
 * resolution. 
 *
 * @param  context      - The LadybugContext to access.
 * @param  uiCamera     - The camera index corresponding to the requested mask.
 * @param  uiCols       - The number of columns of the requested mask. This should map
 *                        to the number of columns in the image captured from each
 *                        camera unit.
 * @param  uiRows       - The number of rows of the requested mask. This should map
 *                        to the number of rows in the image captured from each
 *                        camera unit.
 * @param  ppAlphaImage - The returned alpha mask. This is an 8 bit image.
 *
 * @return A LadybugError indicating the success of the function.     
 */
LADYBUGDLL_API LadybugError 
ladybugGetAlphaMask( 
    LadybugContext context,
    unsigned int uiCamera, 
    unsigned int uiCols,
    unsigned int uiRows,
    const unsigned char** ppAlphaImage);

/*@}*/ 

/** 
 * @defgroup ImageRectificationMethods Image Rectification Methods
 *
 * This group of functions provides access to the libraries image distortion
 * correction capabilities
 */

/*@{*/ 

/**
 * Maps a rectified pixel location to its corresponding point in the
 * distorted (raw) image.
 *
 * This function must be called after ladybugSetOffScreenImageSize(), which 
 * sets the resolution of rectified images. 
 *
 * @param  context        - The LadybugContext to access.
 * @param  uiCamera       - Camera index that this image corresponds to.
 * @param  dRectifiedRow  - Row coordinate of the rectified pixel to map.
 * @param  dRectifiedCol  - Column coordinate of the rectified pixel to map.
 * @param  pdDistortedRow - Location to return the row coordinate of the same point
 *                          in the distorted (raw) image.
 * @param  pdDistortedCol - Location to return the column coordinate of the same 
 *                          point in the distorted (raw) image.
 *
 * @return A LadybugError indicating the success of the function.     
 *
 * @see ladybugSetOffScreenImageSize()
 */
LADYBUGDLL_API LadybugError
ladybugUnrectifyPixel( 
    LadybugContext context,
    unsigned int uiCamera,
    double dRectifiedRow,
    double dRectifiedCol,
    double* pdDistortedRow,
    double* pdDistortedCol );

/**
 * Maps a distorted (raw) pixel location to its corresponding point in the
 * rectified image.
 *
 * This function must be called after ladybugSetOffScreenImageSize(), which 
 * sets the resolution of rectified images. 
 *
 * @param  context        - The LadybugContext to access.
 * @param  uiCamera       - Camera index that this image corresponds to.
 * @param  dDistortedRow  - Row coordinate of the distorted (raw) pixel to map.
 * @param  dDistortedCol  - Column coordinate of the distorted (raw) pixel to map.
 * @param  pdRectifiedRow - Location to return the row coordinate of the same point
 *                          in the rectified image.
 * @param  pdRectifiedCol - Location to return the column coordinate of the same 
 *                          point in the rectified image.
 *
 * @return A LadybugError indicating the success of the function.
 *         LADYBUG_OVEREXPOSED is returned if the selected region's average
 *         intensity exceeds 254/255 for any channel.
 *
 * @see   ladybugSetOffScreenImageSize()
 */
LADYBUGDLL_API LadybugError
ladybugRectifyPixel( 
    LadybugContext context,
    unsigned int uiCamera,
    double dDistortedRow,
    double dDistortedCol,
    double* pdRectifiedRow,
    double* pdRectifiedCol );
                    
/*@}*/

/*@}*/

#ifdef __cplusplus
};
#endif

#endif // #ifndef __LADYBUGGEOM_H__
