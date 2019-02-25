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
 * @defgroup LadybugGPS_cs LadybugGPS.cs
 *
 * LadybugGPS.cs
 *
 *      This file defines the interface of Ladybug SDK's GPS-related functions.
 *      If your C# project uses Ladybug SDK's GPS functions, this file must also be
 *      added to your project along with Ladybug_Managed.cs.
 *
 * We welcome your bug reports, suggestions, and comments: 
 * www.ptgrey.com/support/contact
 */

/*@{*/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LadybugAPI
{
    // Description:
    //   Ladybug2 GPS NMEA GPGGA data - Fix information.
    //
    // Remarks:
    //   Essential fix data which provides a 3D location and accuracy data.
    [StructLayout(LayoutKind.Explicit, Size = 120)]
    unsafe public struct LadybugNMEAGPGGA
    {
        // Flag indicating whether all the data in this structure is valid.
        // True - Valid, False - Invalid.
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.I1)]
        public bool bValidData;

        // Hour, Coordinated Universal Time.
        [FieldOffset(1)]
        public byte ucGGAHour;

        // Minute, Coordinated Universal Time.
        [FieldOffset(2)]
        public byte ucGGAMinute;

        // Second, Coordinated Universal Time.
        [FieldOffset(3)]
        public byte ucGGASecond;

        // Sub-second.
        [FieldOffset(4)]
        public ushort wGGASubSecond;

        // Latitude.
        // < 0 = South of Equator, > 0 = North of Equator.
        [FieldOffset(8)]
        public double dGGALatitude;

        // Longitude.
        // < 0 = West of Prime Meridian, > 0 = East of Prime Meridian.
        [FieldOffset(16)]
        public double dGGALongitude;

        // GPS Quality Indicator.
        // 0 = Fix not available.
        // 1 = GPS SPS mode.
        // 2 = Differential GPS, SPS mode, fix valid.
        // 3 = GPS PPS mode, fix valid.
        [FieldOffset(24)]
        public byte ucGGAGPSQuality;

        // Number of satellites in view.
        [FieldOffset(25)]
        public byte ucGGANumOfSatsInUse;

        // Horizontal Dilution of precision (HDOP) (in meters).
        [FieldOffset(32)]
        public double dGGAHDOP;

        // Antenna Altitude above/below mean-sea-level (geoid) (in meters).
        [FieldOffset(40)]
        public double dGGAAltitude;

        // Geoidal separation.
        // This is the difference between the WGS-84 earth ellipsoid and the
        // and mean sea level (geoid). A negative value means the mean-sea-level
        // below ellipsoid.
        [FieldOffset(48)]
        public double dGGAHeightOfGeoid;

        // A counter containing the number of GPGGA sentences parsed since
        // the GPS device was started (or restarted).
        // If the GPS data is from a Ladybug image, it is always 1.
        [FieldOffset(56)]
        public uint ulCount;

        // Reserved.
        [FieldOffset(60)]
        public fixed uint ulReserved[14];

		/** Override ToString for struct */
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.Append("GGA:\n");
            output.AppendFormat("\tValid data: {0}\n", bValidData ? "True" : "False");
            output.AppendFormat("\tHour / Minute / Second / Millisecond: {0} / {1} / {2} / {3}\n", (int)ucGGAHour, (int)ucGGAMinute, (int)ucGGASecond, (int)wGGASubSecond);
            output.AppendFormat("\tLatitude / Longitude: {0} / {1}\n", dGGALatitude, dGGALongitude);
            output.AppendFormat("\tQuality: {0}\n", (int)ucGGAGPSQuality);
            output.AppendFormat("\tNumber of sats: {0}\n", (int)ucGGANumOfSatsInUse);
            output.AppendFormat("\tHDOP: {0}\n", dGGAHDOP);
            output.AppendFormat("\tAltitude: {0}\n", dGGAAltitude);
            output.AppendFormat("\tHeight of geoid: {0}\n", dGGAHeightOfGeoid);
            output.AppendFormat("\tCount: {0}\n", ulCount);
            return output.ToString();
        }
    }

    // Description:
    //   Ladybug2 GPS NMEA GPGSA data - Overall Satellite data.
    //
    // Remarks:
    //   Details on the nature of the fix. It includes the numbers of the
    //   satellites being used in the current solution and the DOP
    //   (dilution of precision).
    [StructLayout(LayoutKind.Explicit, Size = 176)]
    unsafe public struct LadybugNMEAGPGSA
    {
        //
        // Description:
        //   Struct for storing all available Satellite in solution
        // Remarks:
        //   This struct is used in place of an array because the Marshaller has a
        //   limitation where arrays need to be aligned/offset at 8 byte boundaries
        //   to run correctly on 64bit systems.
        [StructLayout(LayoutKind.Explicit, Size = 72)]
        public struct GSASatsInSolution
        {
            [FieldOffset(0)]
            public ushort usSat1;

            [FieldOffset(2)]
            public ushort usSat2;

            [FieldOffset(4)]
            public ushort usSat3;

            [FieldOffset(6)]
            public ushort usSat4;

            [FieldOffset(8)]
            public ushort usSat5;

            [FieldOffset(10)]
            public ushort usSat6;

            [FieldOffset(12)]
            public ushort usSat7;

            [FieldOffset(14)]
            public ushort usSat8;

            [FieldOffset(16)]
            public ushort usSat9;

            [FieldOffset(18)]
            public ushort usSat10;

            [FieldOffset(20)]
            public ushort usSat11;

            [FieldOffset(22)]
            public ushort usSat12;

            [FieldOffset(24)]
            public ushort usSat13;

            [FieldOffset(26)]
            public ushort usSat14;

            [FieldOffset(28)]
            public ushort usSat15;

            [FieldOffset(30)]
            public ushort usSat16;

            [FieldOffset(32)]
            public ushort usSat17;

            [FieldOffset(34)]
            public ushort usSat18;

            [FieldOffset(36)]
            public ushort usSat19;

            [FieldOffset(38)]
            public ushort usSat20;

            [FieldOffset(40)]
            public ushort usSat21;

            [FieldOffset(42)]
            public ushort usSat22;

            [FieldOffset(44)]
            public ushort usSat23;

            [FieldOffset(46)]
            public ushort usSat24;

            [FieldOffset(48)]
            public ushort usSat25;

            [FieldOffset(50)]
            public ushort usSat26;

            [FieldOffset(52)]
            public ushort usSat27;

            [FieldOffset(54)]
            public ushort usSat28;

            [FieldOffset(56)]
            public ushort usSat29;

            [FieldOffset(58)]
            public ushort usSat30;

            [FieldOffset(60)]
            public ushort usSat31;

            [FieldOffset(62)]
            public ushort usSat32;

            [FieldOffset(64)]
            public ushort usSat33;

            [FieldOffset(66)]
            public ushort usSat34;

            [FieldOffset(68)]
            public ushort usSat35;

            [FieldOffset(70)]
            public ushort usSat36;
        }

        // Flag indicating whether all the data in this structure is valid.
        // True - Valid, False - Invalid.
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.I1)]
        public bool bValidData;

        // Selection mode.
        // M = manual, A = automatic 2D/3D.
        [FieldOffset(1)]
        public byte ucGSAMode;

        // Fix Mode.
        // 1 = No fix.
        // 2 = 2D fix.
        // 3 = 3D fix.
        [FieldOffset(2)]
        public byte ucGSAFixMode;

        // ID of 1st satellite used for fix.
        [FieldOffset(4)]
        // If an array was used here, it has to be aligned at 8 byte boundary to run on 64bit machine
        public GSASatsInSolution wGSASatsInSolution;

        // Dilution of precision (PDOP).
        [FieldOffset(80)]
        public double dGSAPDOP;

        // Horizontal dilution of precision (HDOP).
        [FieldOffset(88)]
        public double dGSAHDOP;

        // Vertical dilution of precision (VDOP).
        [FieldOffset(96)]
        public double dGSAVDOP;

        // A counter containing the number of GPGSA sentences parsed since
        // the GPS device was started (or restarted).
        // If the GPS data is from a Ladybug image, it is always 1.
        [FieldOffset(104)]
        public uint ulCount;

        // Reserved.
        [FieldOffset(108)]
        public fixed uint ulReserved[16];

		/** Override ToString for struct */
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("GSA:\n");
            output.AppendFormat("\tValid data: {0}\n", bValidData);
            output.AppendFormat("\tMode: {0}\n", (int)ucGSAMode);
            output.AppendFormat("\tFix mode: {0}\n", (int)ucGSAFixMode);

            output.AppendFormat("\tSats used:\n");
            Type structType = typeof(LadybugNMEAGPGSA.GSASatsInSolution);
            System.Reflection.FieldInfo[] fields = structType.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                output.AppendFormat("\t  Sat {0} : {1}\n", field.Name, field.GetValue(wGSASatsInSolution));
            }

            output.AppendFormat("\tPDOP: {0}\n", dGSAPDOP);
            output.AppendFormat("\tHDOP: {0}\n", dGSAHDOP);
            output.AppendFormat("\tVDOP: {0}\n", dGSAVDOP);
            output.AppendFormat("\tCount: {0}\n", ulCount);
            return output.ToString();
        }
    }

    // Description:
    //   Ladybug2 GPS NMEA GPGSV data - Satellites in view.
    //
    // Remarks:
    //   Data about the satellites that the unit might be able to find based on
    //   its viewing mask and almanac data. It also shows current ability to
    //   track this data.
    [StructLayout(LayoutKind.Explicit, Size = 432)]
    unsafe public struct LadybugNMEAGPGSV
    {
        //
        // Description:
        //   Satellite information structure
        //
        // Remarks:
        //   This structure contains information about a single satellite.
        [StructLayout(LayoutKind.Explicit, Size = 10)]
        public struct CNPSatInfo
        {
            // Satellite ID.
            [FieldOffset(0)]
            public ushort wPRN;

            // Signal strength (Signal to Noise Ratio).
            [FieldOffset(2)]
            public ushort wSignalQuality;

            // Boolean indicating if this satellite is being used in the solution.
            [FieldOffset(4)]
            [MarshalAs(UnmanagedType.I1)]
            public bool bUsedInSolution;

            // Azimuth (in degrees).
            [FieldOffset(6)]
            public ushort wAzimuth;

            // Elevation (in degrees).
            [FieldOffset(8)]
            public ushort wElevation;
        }

        //
        // Description:
		//   Struct for storing all available Satellite in solution
        // Remarks:
        //   This struct is used in place of an array because the Marshaller has a
        //   limitation where arrays need to be aligned/offset at 8 byte boundaries
        //   to run correctly on 64bit systems.
        [StructLayout(LayoutKind.Explicit, Size = 360)]
        public struct GSVTotalNumSatsInView
        {
            [FieldOffset(0)]
            public CNPSatInfo CNPSat1;

            [FieldOffset(10)]
            public CNPSatInfo CNPSat2;

            [FieldOffset(20)]
            public CNPSatInfo CNPSat3;

            [FieldOffset(30)]
            public CNPSatInfo CNPSat4;

            [FieldOffset(40)]
            public CNPSatInfo CNPSat5;

            [FieldOffset(50)]
            public CNPSatInfo CNPSat6;

            [FieldOffset(60)]
            public CNPSatInfo CNPSat7;

            [FieldOffset(70)]
            public CNPSatInfo CNPSat8;

            [FieldOffset(80)]
            public CNPSatInfo CNPSat9;

            [FieldOffset(90)]
            public CNPSatInfo CNPSat10;

            [FieldOffset(100)]
            public CNPSatInfo CNPSat11;

            [FieldOffset(110)]
            public CNPSatInfo CNPSat12;

            [FieldOffset(120)]
            public CNPSatInfo CNPSat13;

            [FieldOffset(130)]
            public CNPSatInfo CNPSat14;

            [FieldOffset(140)]
            public CNPSatInfo CNPSat15;

            [FieldOffset(150)]
            public CNPSatInfo CNPSat16;

            [FieldOffset(160)]
            public CNPSatInfo CNPSat17;

            [FieldOffset(170)]
            public CNPSatInfo CNPSat18;

            [FieldOffset(180)]
            public CNPSatInfo CNPSat19;

            [FieldOffset(190)]
            public CNPSatInfo CNPSat20;

            [FieldOffset(200)]
            public CNPSatInfo CNPSat21;

            [FieldOffset(210)]
            public CNPSatInfo CNPSat22;

            [FieldOffset(220)]
            public CNPSatInfo CNPSat23;

            [FieldOffset(230)]
            public CNPSatInfo CNPSat24;

            [FieldOffset(240)]
            public CNPSatInfo CNPSat25;

            [FieldOffset(250)]
            public CNPSatInfo CNPSat26;

            [FieldOffset(260)]
            public CNPSatInfo CNPSat27;

            [FieldOffset(270)]
            public CNPSatInfo CNPSat28;

            [FieldOffset(280)]
            public CNPSatInfo CNPSat29;

            [FieldOffset(290)]
            public CNPSatInfo CNPSat30;

            [FieldOffset(300)]
            public CNPSatInfo CNPSat31;

            [FieldOffset(310)]
            public CNPSatInfo CNPSat32;

            [FieldOffset(320)]
            public CNPSatInfo CNPSat33;

            [FieldOffset(330)]
            public CNPSatInfo CNPSat34;

            [FieldOffset(340)]
            public CNPSatInfo CNPSat35;

            [FieldOffset(350)]
            public CNPSatInfo CNPSat36;
        }

        // Flag indicating whether all the data in this structure is valid.
        // True - Valid, False - Invalid.
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.I1)]
        public bool bValidData;

        // Total number of messages.
        [FieldOffset(1)]
        public byte ucGSVTotalNumOfMsg;

        // Total satellites in view.
        [FieldOffset(2)]
        public ushort wGSVTotalNumSatsInView;

        // Array of satellite information structures.
        [FieldOffset(4)]
        // If an array was used here, it has to be aligned at 8 byte boundary to run on 64bit machine
        public GSVTotalNumSatsInView GSVSatInfo;

        // A counter containing the number of GPGSV sentences parsed since
        // the GPS device was started (or restarted).
        // If the GPS data is from a Ladybug image, it is always 1.
        [FieldOffset(364)]
        public uint ulCount;

        // Reserved Space
        [FieldOffset(368)]
        public fixed uint ulReserved[16];

		/** Override ToString for struct */
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("GSV:\n");
            output.AppendFormat("\tValid data: {0}\n", bValidData);
            output.AppendFormat("\tNumber of messages: {0}\n", (int)ucGSVTotalNumOfMsg);
            output.AppendFormat("\tNumber of sats in view: {0}\n", wGSVTotalNumSatsInView);

            Type structType = typeof(LadybugNMEAGPGSV.GSVTotalNumSatsInView);
            System.Reflection.FieldInfo[] fields = structType.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                LadybugNMEAGPGSV.CNPSatInfo satInfo = (LadybugNMEAGPGSV.CNPSatInfo)field.GetValue(GSVSatInfo);
                output.AppendFormat("\t  PRN: {0}\n", satInfo.wPRN);
                output.AppendFormat("\t  Signal quality: {0}\n", satInfo.wSignalQuality);
                output.AppendFormat("\t  Used: {0}\n", satInfo.bUsedInSolution);
                output.AppendFormat("\t  Azimuth: {0}\n", satInfo.wAzimuth);
                output.AppendFormat("\t  Elevation: {0}\n", satInfo.wElevation);
            }

            output.AppendFormat("\tCount: {0}\n", ulCount);

            return output.ToString();
        }
    }

    // Description:
    //   Ladybug2 GPS NMEA GPRMC data - Recommended minimum data for GPS.
    [StructLayout(LayoutKind.Explicit, Size = 120)]
    unsafe public struct LadybugNMEAGPRMC
    {
        // Flag indicating whether all the data in this structure is valid.
        // True - Valid, False - Invalid.
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.I1)]
        public bool bValidData;

        // Hour (Coordinated Universal Time).
        [FieldOffset(1)]
        public byte ucRMCHour;

        // Minute (Coordinated Universal Time).
        [FieldOffset(2)]
        public byte ucRMCMinute;

        // Second (Coordinated Universal Time).
        [FieldOffset(3)]
        public byte ucRMCSecond;

        // Sub-second.
        [FieldOffset(4)]
        public ushort wRMCSubSecond;

        // Data valid character.
        // A - Data valid, V - Navigation rx warning.
        [FieldOffset(6)]
        public byte ucRMCDataValid;

        // Latitude.
        // < 0 = South of Equator, > 0 = North of Equator.
        [FieldOffset(8)]
        public double dRMCLatitude;

        // Longitude.
        // < 0 = West of Prime Meridian, > 0 = East of Prime Meridian.
        [FieldOffset(16)]
        public double dRMCLongitude;

        // Ground speed (in knots).
        [FieldOffset(24)]
        public double dRMCGroundSpeed;

        // Course over ground, degrees true.
        [FieldOffset(32)]
        public double dRMCCourse;

        // Day.
        [FieldOffset(40)]
        public byte ucRMCDay;

        // Month.
        [FieldOffset(41)]
        public byte ucRMCMonth;

        // Year.
        [FieldOffset(42)]
        public ushort wRMCYear;

        // Magnetic variation.
        // Positive values indicate degrees east, while negative values indicate
        // west.
        [FieldOffset(48)]
        public double dRMCMagVar;

        // A counter containing the number of GPRMC sentences parsed since
        // the GPS device was started (or restarted).
        // If the GPS data is from a Ladybug image, it is always 1.
        [FieldOffset(56)]
        public uint ulCount;

        // Reserved.
        [FieldOffset(60)]
        public fixed uint ulReserved[14];

		/** Override ToString for struct */
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("RMC:\n");
            output.AppendFormat("\tValid data: {0}\n", bValidData);
            output.AppendFormat("\tHour / Minute / Second / Millisecond: {0} / {1} / {2} / {3}\n", (int)ucRMCHour, (int)ucRMCMinute, (int)ucRMCSecond, (int)wRMCSubSecond);
            output.AppendFormat("\tData valid: {0}\n", Convert.ToChar(ucRMCDataValid));
            output.AppendFormat("\tLatitude / Longitude: {0} / {1}\n", dRMCLatitude, dRMCLongitude);
            output.AppendFormat("\tGround speed: {0}\n", dRMCGroundSpeed);
            output.AppendFormat("\tCourse: {0}\n", dRMCCourse);
            output.AppendFormat("\tDay / Month / Year: {0} / {1} / {2}\n", (int)ucRMCDay, (int)ucRMCMonth, (int)wRMCYear);
            output.AppendFormat("\tMagnetic variation: {0}\n", dRMCMagVar);
            output.AppendFormat("\tCount: {0}\n", ulCount);
            return output.ToString();
        }
    }

    // Description:
    //   Ladybug2 GPS NMEA GPZDA data - Date and time.
    [StructLayout(LayoutKind.Explicit, Size = 72)]
    unsafe public struct LadybugNMEAGPZDA
    {
        // Flag indicating whether all the data in this structure is valid.
        // True - Valid, False - Invalid.
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.I1)]
        public bool bValidData;

        // Hour (Coordinated Universal Time).
        [FieldOffset(1)]
        public byte ucZDAHour;

        // Minute (Coordinated Universal Time).
        [FieldOffset(2)]
        public byte ucZDAMinute;

        // Second (Coordinated Universal Time).
        [FieldOffset(3)]
        public byte ucZDASecond;

        // Sub-second.
        [FieldOffset(4)]
        public ushort wZDASubSecond;

        // Day.
        [FieldOffset(6)]
        public byte ucZDADay;

        // Month.
        [FieldOffset(7)]
        public byte ucZDAMonth;

        // Year.
        [FieldOffset(8)]
        public ushort wZDAYear;

        // Local zone description, -13 to +13 hours.
        [FieldOffset(19)]
        public byte ucZDALocalZoneHour;

        // Local zone minutes description, 00 to 59 minutes.
        [FieldOffset(11)]
        public byte ucZDALocalZoneMinute;

        // A counter containing the number of GPZDA sentences parsed since
        // the GPS device was started (or restarted).
        // If the GPS data is from a Ladybug image, it is always 1.
        [FieldOffset(12)]
        public uint ulCount;

        // Reserved.
        [FieldOffset(16)]
        public fixed uint ulReserved[14];

		/** Override ToString for struct */
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("ZDA: ");
            output.AppendFormat("\tValid data: {0}\n", bValidData);
            output.AppendFormat("\tHour / Minute / Second / Millisecond: {0} / {1} / {2} / {3}\n", (int)ucZDAHour, (int)ucZDAMinute, (int)ucZDASecond, (int)wZDASubSecond);
            output.AppendFormat("\tDay / Month / Year: {0} / {1} / {2}\n", (int)ucZDADay, (int)ucZDAMonth, (int)wZDAYear);
            output.AppendFormat("\tLocal hour / minute: {0} / {1}\n", (int)ucZDALocalZoneHour, (int)ucZDALocalZoneMinute);
            output.AppendFormat("\tCount: {0}\n", ulCount);
            return output.ToString();
        }
    }

    // Description:
    //   Ladybug2 GPS NMEA GPVTG data - Vector track and Speed over the Ground.
    //
    // Remarks:
    //   This structure contains the current track and speed as recorded by the
    //   GPS device.
    [StructLayout(LayoutKind.Explicit, Size = 112)]
    unsafe public struct LadybugNMEAGPVTG
    {
        // Flag indicating whether all the data in this structure is valid.
        // True - Valid, False - Invalid.
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.I1)]
        public bool bValidData;

        // Track Made Good.
        [FieldOffset(8)]
        public double dVTGTrackMadeGood;

        // Magnetic Track Made Good.
        [FieldOffset(16)]
        public double dVTGMagneticTrackMadeGood;

        // Ground Speed (in knots).
        [FieldOffset(24)]
        public double dVTGGroundSpeedKnots;

        // Ground Speed (in kilometers per hour).
        [FieldOffset(32)]
        public double dVTGGroundSpeedKilometersPerHour;

        // A counter containing the number of GPVTG sentences parsed since
        // the GPS device was started (or restarted).
        // If the GPS data is from a Ladybug image, it is always 1.
        [FieldOffset(40)]
        public uint ulCount;

        // Reserved.
        [FieldOffset(44)]
        public fixed uint ulReserved[16];

        /** Override ToString for struct */
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("VTG:\n");
            output.AppendFormat("\tValid data: {0}\n", bValidData);
            output.AppendFormat("\tTrack: {0}\n", dVTGTrackMadeGood);
            output.AppendFormat("\tMagnetic track: {0}\n", dVTGMagneticTrackMadeGood);
            output.AppendFormat("\tGround speed (knots): {0}\n", dVTGGroundSpeedKnots);
            output.AppendFormat("\tGround speed (km/h): {0}\n", dVTGGroundSpeedKilometersPerHour);
            output.AppendFormat("\tCount: {0}\n", ulCount);
            return output.ToString();
        }
    }


    // Description:
    //   Ladybug2 GPS NMEA GPGLL data - Geographic Position Latitude/Longitude.
    [StructLayout(LayoutKind.Explicit, Size = 96)]
    unsafe public struct LadybugNMEAGPGLL
    {
        // Flag indicating whether all the data in this structure is valid.
        // True - Valid, False - Invalid.
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.I1)]
        public bool bValidData;

        // Latitude, < 0 = South of Equator, > 0 = North of Equator.
        [FieldOffset(8)]
        public double dGLLLatitude;

        // Longitude, < 0 = West of Prime Meridian, > 0 = East of Prime Meridian.
        [FieldOffset(16)]
        public double dGLLLongitude;

        // Hour (Coordinated Universal Time).
        [FieldOffset(24)]
        public byte ucGLLHour;

        // Minute (Coordinated Universal Time).
        [FieldOffset(25)]
        public byte ucGLLMinute;

        // Second (Coordinated Universal Time).
        [FieldOffset(26)]
        public byte ucGLLSecond;

        // Sub-second.
        [FieldOffset(28)]
        public ushort wGLLSubSecond;

        // Data valid character.
        // A - Data Valid, V - Data Invalid.
        [FieldOffset(30)]
        public byte ucGLLDataValid;

        // A counter containing the number of GPGLL sentences parsed since
        // the GPS device was started (or restarted).
        // If the GPS data is from a Ladybug image, it is always 1.
        [FieldOffset(32)]
        public uint ulCount;

        // Reserved.
        [FieldOffset(36)]
        public fixed uint ulReserved[14];

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("GLL:\n");
            output.AppendFormat("\tValid data: {0}\n", bValidData);
            output.AppendFormat("\tLatitude / Longitude: {0} / {1}\n", dGLLLatitude, dGLLLongitude);
            output.AppendFormat("\tHour / Minute / Second / Millisecond: {0} / {1} / {2} / {3}\n", (int)ucGLLHour, (int)ucGLLMinute, (int)ucGLLSecond, (int)wGLLSubSecond);
            output.AppendFormat("\tData valid: {0}\n", (int)ucGLLDataValid);
            output.AppendFormat("\tCount: {0}\n", ulCount);
            return output.ToString();
        }
    }

	/** GPS NMEA GPHDT data - Heading - True. */
	[StructLayout(LayoutKind.Explicit, Size = 88)]
	unsafe public struct LadybugNMEAGPHDT
	{
		/** Whether all the data is valid. */
		[FieldOffset(0)]
        [MarshalAs(UnmanagedType.I1)]
		public bool bValidData;

		/** Heading degrees, true */
		[FieldOffset(8)]
		public double heading;

		/**
		 * A counter containing the number of GPVTG sentences parsed since
		 * the GPS device was started (or restarted).
		 * If the GPS data is from a Ladybug image, it is always 1.
		 */
		 [FieldOffset(16)]
		public uint ulCount;

		/** Reserved */
		[FieldOffset(20)]
		public fixed uint ulReserved[16];

		/** Override ToString for struct */
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("HDT:\n");
            output.AppendFormat("\tValid data: {0}\n", bValidData);
            output.AppendFormat("\tHeading: {0}\n", heading);
            output.AppendFormat("\tCount: {0}\n", ulCount);
            return output.ToString();
        }
	};

    /** GPS NMEA PRDID data. */
    [StructLayout(LayoutKind.Explicit, Size = 104)]
    unsafe public struct LadybugNMEAPRDID
    {
        /** Whether all the data is valid. */
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.I1)]
        public bool bValidData;

        /** Pitch degrees. */
        [FieldOffset(8)]
        public double pitch;

        /** Roll degrees. */
        [FieldOffset(16)]
        public double roll;

        /** Heading. */
        [FieldOffset(24)]
        public double heading;

        /**
         * A counter containing the number of GPVTG sentences parsed since
         * the GPS device was started (or restarted).
         * If the GPS data is from a Ladybug image, it is always 1.
         */
        [FieldOffset(32)]
        public uint ulCount;

        /** Reserved */
        [FieldOffset(36)]
        public fixed uint ulReserved[16];

		/** Override ToString for struct */
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("PRDID:\n");
            output.AppendFormat("\tValid data: {0}\n", bValidData);
            output.AppendFormat("\tPitch: {0}\n", pitch);
            output.AppendFormat("\tRoll: {0}\n", roll);
            output.AppendFormat("\tHeading: {0}\n", heading);
            output.AppendFormat("\tCount: {0}\n", ulCount);
            return output.ToString();
        }
    };

    // Description:
    //   Ladybug2 GPS NMEA data.
    //
    // Remarks:
    //   This structure is a holder for all of the above structures.
    [StructLayout(LayoutKind.Explicit, Size = 2344)]
    unsafe public struct LadybugNMEAGPSData
    {
        // GPGGA data structure
        [FieldOffset(0)]
        public LadybugNMEAGPGGA dataGPGGA;

        // GPGSA data structure
        [FieldOffset(120)]
        public LadybugNMEAGPGSA dataGPGSA;

        // GPGSV data structure
        [FieldOffset(296)]
        public LadybugNMEAGPGSV dataGPGSV;

        // GPRMC data structure
        [FieldOffset(728)]
        public LadybugNMEAGPRMC dataGPRMC;

        // GPZDA data structure
        [FieldOffset(848)]
        public LadybugNMEAGPZDA dataGPZDA;

        // GPVTG data structure
        [FieldOffset(920)]
        public LadybugNMEAGPVTG dataGPVTG;

        // GPGLL data structure
        [FieldOffset(1032)]
        public LadybugNMEAGPGLL dataGPGLL;

        /** GPHDT data structure */
        [FieldOffset(1128)]
        public LadybugNMEAGPHDT dataGPHDT;

        /** PRDID data structure */
        [FieldOffset(1216)]
        public LadybugNMEAPRDID dataPRDID;

        // Reserved space
        [FieldOffset(1320)]
        public fixed uint ulReserved[256];

		/** Override ToString for struct */
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.Append(dataGPGGA);
            output.Append(dataGPGSA);
            output.Append(dataGPGSV);
            output.Append(dataGPRMC);
            output.Append(dataGPZDA);
            output.Append(dataGPVTG);
            output.Append(dataGPGLL);
            output.Append(dataGPHDT);
            output.Append(dataPRDID);
            return output.ToString();
        }
    }

    // This class defines static functions to access most of the
    // Ladybug APIs defined in ladybugGPS.h
    unsafe public partial class Ladybug
    {
        /** 
         * @defgroup ManagedGPSContextCreationMethods GPS Context Creation and Initialization Methods
         *
         * This group of functions provides control over GPS functionality.
         * 
         * @ingroup LadybugGPS_cs
         */

        /*@{*/ 

        /** The maximum number of GPS channels. */
        public const int NP_MAX_CHAN = 36;

        /** The default GPS port number. */
        public const int DEFAULT_PORT_NUMBER = 1;

        /** The default device name. */
        public const String DEFAULT_DEVICE_NAME = "/dev/ttyACM0";

        /** The default GPS baud rate. */
        public const int DEFAULT_BAUDRATE = 4800;

        /** The default GPS device NMEA data update rate in milliseconds. */
        public const int DEFAULT_UPDATE_INTERVAL = 1000;

        /**
         * Creates a new GPS context.
         * 
         * A context must be created for a GPS device that is going to
         * be used. This must be done before any other GPS API function calls can 
         * be made.
         *
         * This function sets the context to NULL if it is unsuccessful.
         *
         * The default port number, baud rate and GPS update time interval values
         * are set to the values defined by DEFAULT_PORT_NUMBER, DEFAULT_BAUDRATE
         * and DEFAULT_UPDATE_INTERVAL respectively.
         *
         * @param context  - A pointer to a LadybugGPSContext to fill with the created
         *                   context.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugDestroyGPSContext()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugCreateGPSContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError CreateGPSContext(out IntPtr context);

        /**
         * Destroys a GPS context.
         *
         * Frees memory associated with the LadybugGPSContext. This should be called 
         * when your application stops using the context.
         *
         * This function sets the context to NULL if successful.
         *
         * @param context - A pointer to the LadybugGPSContext to destroy.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugCreateGPSContext()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugDestroyGPSContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError DestroyGPSContext(ref IntPtr context);

        /**
         * Registers the GPS context with a Ladybug context. 
         *
         * The images returned by ladybugGrabImage() and ladybugLockNext() will have
         * GPS data included if this function and ladybugStartGPS() have been called. 
         *
         * @param context     - The LadybugContext to register the GPS context with.
         * @param GPSContext - A pointer to a GPS context created with a call to 
         *                      ladybugCreateGPSContext().        
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see   ladybugUnregisterGPS(), ladybugStartGPS(), ladybugGrabImage(), ladybugLockNext()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugRegisterGPS", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError RegisterGPS(IntPtr context, ref IntPtr GPSContext);

        /**
         * Unregisters a GPS context that has been registered with a Ladybug context.
         *
         * This function should be called before the LadybugGPSContext is
         * destroyed by ladybugDestroyGPSContext().
         *
         * Images returned by ladybugGrabImage() and ladybugLockNext() will not have
         * GPS data included after this function is called. 
         *
         * @param context     - The LadybugContext to unregister the GPS context with.
         * @param GPSContext - A pointer to a GPS context created with a call to 
         *                      ladybugCreateGPSContext().        
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugRegisterGPS()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugUnregisterGPS", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError UnregisterGPS(IntPtr context, ref IntPtr GPSContext);

        /**
         * Sets the serial port number, baud rate and data update rate for the 
         * GPS device for Windows.
         *
         * This function only sets the GPS parameters without actually starting the
         * the GPS device. To start the GPS device, call ladybugStartGPS(). 
         *
         * The value of uiUpdateTimeInterval depends on the NMEA data update rate of
         * the GPS device. For example, if the GPS device data update rate is 4 Hz,
         * then uiUpdateTimeInterval should be set to no less  than 250 ms.
         *
         * If uiUpdateTimeInterval is less than 1 ms or more, LADYBUG_INVALID_ARGUMENT is
         * returned.
         *
         * @param context              - The LadybugGPSContext to access.
         * @param uiPort               - The number of the serial port of the GPS device. 
         * @param uiBaud               - Baud rate of the serial port.
         * @param uiUpdateTimeInterval - The GPS data update time interval in milliseconds.
         *                               The default value is 1000 milliseconds.                                
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugInitializeGPSEx(), ladybugQueryGPSStatus(), ladybugStartGPS()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugInitializeGPS", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError InitializeGPS(
            IntPtr context,
            uint uiPort,
            uint uiBaud,
            uint uiUpdateTimeInterval);

        /**
         * Sets the serial port number, baud rate and data update rate for the 
         * GPS device on Linux platform.
         *
         * This function only sets the GPS parameters without actually starting the
         * the GPS device. To start the GPS device, call ladybugStartGPS(). 
         *
         * The value of uiUpdateTimeInterval depends on the NMEA data update rate of
         * the GPS device. For example, if the GPS device data update rate is 4 Hz,
         * then uiUpdateTimeInterval should be set to no less  than 250 ms.
         *
         * If uiUpdateTimeInterval is less than 1 ms or more, LADYBUG_INVALID_ARGUMENT is
         * returned.
         *
         * @param context              - The LadybugGPSContext to access.
         * @param deviceName           - The device name to access (e.g. /dev/ttyACM0)
         * @param uiBaud               - Baud rate of the serial port.
         * @param uiUpdateTimeInterval - The GPS data update time interval in milliseconds.
         *                               The default value is 1000 milliseconds.                                
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugInitializeGPS(), ladybugQueryGPSStatus(), ladybugStartGPS()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugInitializeGPSEx", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError InitializeGPSEx(
            IntPtr context,
            ref string deviceName,
            uint uiBaud,
            uint uiUpdateTimeInterval=1000);

        /**
         * Starts grabbing GPS data from the GPS device.
         *
         * This function starts grabbing GPS data using the parameters set by
         * ladybugInitializeGPS(). If ladybugInitializeGPS() is not called, the
         * default parameters set by ladybugCreateGPSContext() are used.
         *
         * @param context - The LadybugGPSContext to access.                              
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see   ladybugCreateGPSContext(), ladybugInitializeGPS(), ladybugStopGPS()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugStartGPS", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError StartGPS(IntPtr context);

        /**
         * Stops the GPS device.
         *
         * @param context - The LadybugGPSContext to access.                              
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugStartGPS()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugStopGPS", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError StopGPS(IntPtr context);

        /**
         * Sets GpsTimeSyncSettings for current context.
         * 
         * @param context     - The LadybugContext to access.
         * @param GpsSettings - Update GpsTimeSync information for struct members
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugSetGpsTimeSync", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError SetGpsTimeSync(IntPtr context, ref GpsTimeSyncSettings GpsSettings);

        /**
         * Gets GpsTimeSyncSettings for current context.
         * 
         * @param context     - The LadybugContext to access.
         * @param GpsSettings - Retrieve struct containing current GpsTimeSyncSettings
         * 
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetGpsTimeSync", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetGpsTimeSync(IntPtr context, ref GpsTimeSyncSettings GpsSettings);
        

        /*@}*/

        /** 
         * @defgroup ManagedGPSDataRetrievalMethods GPS Device Data Retrieval Methods
         *
         * This group of functions is used to access GPS status and data from 
         * the GPS Device when using the Ladybug camera.
         * 
         * @ingroup LadybugGPS_cs
         */

        /*@{*/

        /**
         * Queries the current GPS status.
         *
         * @param context              - The LadybugGPSContext to access.   
         * @param bGPSDeviceStarted    - Whether or not the GPS device is started.
         * @param uiStartedPortNumber  - The port number of the GPS device.
         * @param bGPSDataIsAvailable  - Whether or not GPS data is available.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugInitializeGPS()
         */
		[DllImport(LADYBUG_DLL, EntryPoint = "ladybugQueryGPSStatus", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError QueryGPSStatus(
			IntPtr context,
			out bool bGPSDeviceStarted,
			out uint uiStartedPortNumber,
			out bool bGPSDataIsAvailable);

        /**
         * Gets all the GPS NMEA sentences from the GPS device.
         *
         * If the returned value pointed by puiLength is 0, this means that there are
         * no NMEA sentences available. 
         *
         * The recommended buffer size is between 600 bytes and 1024 bytes. 
         *
         * @param context      - The LadybugGPSContext to access.   
         * @param buffer       - The pointer to the buffer to receive GPS sentences.
         * @param uiBufferSize - The size of pBuffer.
         * @param uiLength     - Total size of the NMEA sentences received (in bytes).
         *
         * @return   A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetGPSNMEASentences", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetGPSNMEASentences(
            IntPtr context,
            out byte buffer,
			out uint uiBufferSize,
			out uint uiLength);

        /**
         * Gets the specified NMEA sentence from the GPS device.
         *
         * To get GPGGA data from a GPS device registered with the LadybugGPSContext
         * pointed to by context, use the following function call, where 
         * pNMEAGGA_Data is a pointer to a LadybugNMEAGPGGA structure:   
         *
         * ladybugGetGPSNMEAData(m_context, "GPGGA", pNMEAGGA_Data);
         *
         * @param context           - The LadybugGPSContext to access.   
         * @param pszNMEASentenceId - A string specifying the NMEA sentence to get.
         * @param pNMEADataBuffer   - The pointer to the data structure to receive GPS data.
         *                            The structure should match the NMEA sentence 
         *                            specified above by pszNMEASentenceId.
         *
         * @return   A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetGPSNMEAData", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetGPSNMEAData(
            IntPtr context,
            string pszNMEASentenceId,
            void* pNMEADataBuffer);

        /**
         * Gets all GPS data from the GPS device that is currently running.
         *
         * @param context   - The LadybugGPSContext to access.   
         * @param pNMEAData - A pointer to a LadybugNMEAGPSData structure to receive all the
         *                    NMEA data.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetGPSNMEAData()
         */
		[DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetAllGPSNMEAData", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetAllGPSNMEAData(
            IntPtr context,
            out LadybugNMEAGPSData pNMEAData);

        /*@}*/

        /** 
         * @defgroup ManagedGPSImageDataAccessorMethods GPS Image Data Accessor Methods
         *
         * This group of functions is used to access GPS data from a Ladybug image.
         * These functions do not require a GPS context to be used.
         * 
         * @ingroup LadybugGPS_cs
         */

        /*@{*/

        /**
         *
         * Gets all the GPS NMEA sentences from a Ladybug image.
         *
         * If the returned value pointed by puiLength is 0, this means that there are
         * no NMEA sentences available. 
         *
         * The recommended buffer size is between 600 bytes and 1024 bytes. 
         *
         * @param pImage       - A pointer to the raw image in which GPS NMEA sentences are
         *                       included.
         * @param pBuffer      - A  pointer to a buffer to receive the NMEA sentences.
         * @param uiBufferSize - The size of the specified buffer.
         * @param puiLength    - Total size of the GPS NMEA sentences received (in bytes).
         *
         * @return   A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetGPSNMEASentencesFromImage", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetGPSNMEASentencesFromImage(ref LadybugImage image, out byte buffer, uint bufferSize, out uint outLength);

        /**
         *
         * Writes the GPS NMEA sentences to a Ladybug image.
         * 
         *
         * The max buffer size is 1024 bytes. 
         *
         * @param context      - The LadybugGPSContext to access.   
         * @param pImage       - A pointer to the image in which GPS NMEA sentences are
         *                       located.
         * @param pBuffer      - A pointer to a NMEA sentence buffer.
         * @param uiBufferSize - The size of the NMEA sentences.
         *
         * @return   A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugWriteGPSDataToImage", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError WriteGPSDataToImage(IntPtr context, ref LadybugImage image, string buffer, uint bufferSize);

        /**
         * Gets the GPS data for the specified NMEA sentences from a Ladybug image.
         *
         * To get GPGGA data from a Ladybug image pointed to by pImage, use the
         * following function call, where pNMEAGGA_Data is a pointer to a 
         * LadybugNMEAGPGGA structure:
         *
         * ladybugGetGPSNMEAData( m_context, "GPGGA", pNMEAGGA_Data );
         *
         * @param pImage            - A pointer to the raw image in which GPS NMEA sentences are
         *                            included.
         * @param pszNMEASentenceId - A string specifying the NMEA sentence to get.
         * @param pNMEADataBuffer   - The pointer to the data structure to receive GPS data.
         *                            The structure should match the NMEA sentence 
         *                            specified above by pszNMEASentenceId.
         *
         * @return A LadybugError indicating the success of the function.
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetGPSNMEADataFromImage", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetGPSNMEADataFromImage(
            ref LadybugImage pImage,
            string pszNMEASentenceId,
            void* pNMEADataBuffer);

        /**
         * Gets all GPS data from a Ladybug image.
         *
         * @param pImage    - A pointer to the raw image in which GPS NMEA sentences are
         *                    included.
         * @param pNMEAData - A pointer to a LadybugNMEAGPSData structure to receive all the
         *                    NMEA data.
         *
         * @return A LadybugError indicating the success of the function.
         *
         * @see ladybugGetGPSNMEAData()
         */
        [DllImport(LADYBUG_DLL, EntryPoint = "ladybugGetAllGPSNMEADataFromImage", CallingConvention = CallingConvention.Cdecl)]
        public static extern LadybugError GetAllGPSNMEADataFromImage(
            ref LadybugImage pImage,
            out LadybugNMEAGPSData pNMEAData);

        /*@}*/
    }
}

/*@}*/