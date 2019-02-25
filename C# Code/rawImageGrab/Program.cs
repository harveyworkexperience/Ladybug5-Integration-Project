/*
===========================================================================================
 rawImageGrab.cs

Created by Kieran Hunt on 08/JAN/2019

Code to use the Ladybug 5 camera to take 6 unstitched images 

===========================================================================================
 */


using System;
using System.IO;
using System.Text;
using System.Threading;
using LadybugAPI;
using LadybugFunctors.cs;

namespace ladybugExp1
{
    class Program
    {

        //initialise Ladybug context to be assigned to control camera
        private static IntPtr ladybugContext;
        private static LadybugImage currentImage;

        private static LadybugProcessedImage processedImage;
        private static LadybugError error;
        private static int ladybugSerial = 16146039;

        //private static LadybugPixelFormat pixelFormat = LadybugPixelFormat.LADYBUG_UNSPECIFIED_PIXEL_FORMAT;

        static LadybugColorProcessingMethod colorProc;

        static int LADYBUG_NUM_CAMERAS = 6;

        static private byte[] textureBuffer;

        private static LadybugDataFormat dataFormat;
        

        //default error handling function
        public static void handleError(LadybugError errorCode)
        {
            if (errorCode != LadybugError.LADYBUG_OK)
            {
                Console.Out.WriteLine(errorCode);
            }
            else
            {
                Console.WriteLine(errorCode);
            }
        }





        //completes all the required operations to start using the
        //Ladybug 5 for image capture. Must be run before anything else will function.
        public static void cameraStartup()
        {
            Console.WriteLine("Starting up...");

            //assign ladybug context with error management
            Console.WriteLine("Creating context...");
            error = Ladybug.CreateContext(out ladybugContext);
            handleError(error);

            //initialising camera
            Console.WriteLine("Initialising camera...");
            error = Ladybug.InitializeFromSerialNumber(ladybugContext, ladybugSerial);
            handleError(error);

            //retrieve camera info

            Console.WriteLine("Retreiving camera info...");
            LadybugCameraInfo camInfo = new LadybugCameraInfo();
            error = Ladybug.GetCameraInfo(ladybugContext, ref camInfo);
            handleError(error);

            //load config
            Console.WriteLine("Loading config...");
            error = Ladybug.LoadConfig(ladybugContext, null);
            handleError(error);

            //start the camera
            //dataFormat = LADYBUG_DATAFORMAT_RAW8;

            Console.WriteLine("Starting camera...");
            error = Ladybug.Start(ladybugContext, dataFormat);
            handleError(error);

            //Console.WriteLine("Declaring bitdepth...");

            /*
            // Account for 16 bit data
            bool isHighBitDepth = dataFormat.isHighBitDepth(currentImage.dataFormat);
            
            
            Console.WriteLine("Declaring buffer...");
            //texture buffer
            textureBuffer = new byte[Ladybug.LADYBUG_NUM_CAMERAS * currentImage.uiRows * currentImage.uiCols * 4];// * (isHighBitDepth ? 2 : 1)];
            */

            Console.WriteLine("Startup complete.");
        }



        unsafe static void Main(string[] args)
        {
            cameraStartup();

            //loops grab attempts to acquire uncorrupted image
            int retry = 5;
            do
            {
                error = Ladybug.GrabImage(ladybugContext, out currentImage);
                retry++;


            } while (error != LadybugError.LADYBUG_OK && retry > 0);
            handleError(error);


            //setting colour processing method
            colorProc = LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE4;
            error = Ladybug.SetColorProcessingMethod(ladybugContext, colorProc);
            handleError(error);

            /*fixed (byte* texBufPtr = &textureBuffer[0])
            {
                // Account for 16 bit data
                bool isHighBitDepth = dataFormat.isHighBitDepth(currentImage.dataFormat);

                // this is a trick to make a pointer of arrays.
                byte** texBufPtrArray = stackalloc byte*[6];
                for (int i = 0; i < 6; i++)
                {
                    texBufPtrArray[i] = texBufPtr + (isHighBitDepth ? i * 2 : i) * currentImage.uiRows * currentImage.uiCols * 4;
                }

            }*/

            Console.WriteLine("converting images...");
            ladybugConvertImage(ladybugContext, ref currentImage, null, LADYBUG_BGRU);

            Console.WriteLine("saving images... \n");
            
            for (int uiCamera = 0; uiCamera < LADYBUG_NUM_CAMERAS; uiCamera++){
                
                LadybugProcessedImage *pProcessedImage = new LadybugProcessedImage;
                string *savePath = new string;
                savePath = "C:/Users/Ladybug/Documents/Code/rawImageGrab/";

                ladybugSaveImage(ladybugContext, pProcessedImage, savePath, LADYBUG_FILEFORMAT_JPG);
            }

        }
    }
}
