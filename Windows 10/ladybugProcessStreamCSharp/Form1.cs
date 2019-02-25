//=============================================================================
// Copyright © 2014 Point Grey Research, Inc. All Rights Reserved.
//
// This software is the confidential and proprietary information of Point
// Grey Research, Inc. ("Confidential Information").  You shall not
// disclose such Confidential Information and shall use it only in
// accordance with the terms of the license agreement you entered into
// with Point Grey Research, Inc. (PGR).
//
// PGR MAKES NO REPRESENTATIONS OR WARRANTIES ABOUT THE SUITABILITY OF THE
// SOFTWARE, EITHER EXPRESSED OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE, OR NON-INFRINGEMENT. PGR SHALL NOT BE LIABLE FOR ANY DAMAGES
// SUFFERED BY LICENSEE AS A RESULT OF USING, MODIFYING OR DISTRIBUTING
// THIS SOFTWARE OR ITS DERIVATIVES.
//=============================================================================

//=============================================================================
//
// Form1.cs
//
// This example shows users how to process a range of frames in a stream file.
// The program processes each frame and outputs an image file sequentially.
// If the stream file contains GPS information, the program outputs the
// information to a separate text file.
//
// This example is based on the ladybugProcessStream command line example.
//
//=============================================================================
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using LadybugAPI;
using System.Threading;

namespace ladybugProcessStream_CSharp
{
    public partial class Form1 : Form
    {
        private bool bProcessImage = false;
        private uint m_numImages = 0;
        private string outputPath;
        private string gpsOutputPath;
        private string OUT_FILE_PREFIX = "ladybugImageOutput";
        private string GPS_FILE_PREFIX = "ladybugGPSOutput";
        private FileStream gpsFile;
        private StreamWriter gpsStreamWriter;

        private uint frameRangeMin, frameRangeMax;
        private uint imageSizeWidth, imageSizeHeight;
        private LadybugColorProcessingMethod colorProcessingMethod;
        private LadybugOutputImage outputRenderingType;
        private float fov;
        private float rotAngleX, rotAngleY, rotAngleZ;
        private uint blendingWidth;
        private bool enableFalloff;
        private float falloffValue;
        private bool enableSwRendering;
        private bool enableStabilization;
        private uint stabilizationNumTemplates;
        private uint stabilizationMaxRegion;
        private float stabilizationDecayRate;
        private bool enableAntiAliasing;
        private bool enableSharpening;

        private LadybugSaveFileFormat saveFileFormat;
        private System.ComponentModel.BackgroundWorker processingThread;

        public Form1()
        {
            InitializeComponent();

            combo_colorProcessingType.Items.AddRange(new string[] {
                "HQ Linear",
                "HQ Linear(GPU)",
                "Directional Filter",
                "Weighted Directional Filter",
                "Rigorous",
                "Edge Sensing",
                "Nearest Neighbour",
                "Downsample4",
                "Downsample16",
                "Monochrome",
                "Disable Color Processing"
            });

            combo_renderingType.Items.AddRange(new string[] {
                "Panoramic",
                "Dome",
                "Spherical",
                "Camera 0 - Rectified",
                "Camera 1 - Rectified",
                "Camera 2 - Rectified",
                "Camera 3 - Rectified",
                "Camera 4 - Rectified",
                "Camera 5 - Rectified"});

            resetToDefaults();

            processingThread = new System.ComponentModel.BackgroundWorker();
            processingThread.WorkerReportsProgress = true;
            processingThread.WorkerSupportsCancellation = true;
            initProcessingThread();
        }

        // Sets up handlers for worker thread
        private void initProcessingThread()
        {
            processingThread.DoWork += new DoWorkEventHandler(processingThread_DoWork);
            processingThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(processingThread_RunWorkerCompleted);
            processingThread.ProgressChanged += new ProgressChangedEventHandler(processingThread_ProgressChanged);
        }

        // Sets up worker thread function
        private void processingThread_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            e.Result = doProcessing(worker, e);
        }

        // Called when worker thread ends
        private void processingThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                label_status.Text = "Operation aborted!";
            }
            else
            {
                if ((bool)e.Result == true)
                {
                    label_status.Text = "Processing complete!";
                    if (!bProcessImage)
                    {
                        validateInputPath();
                        label_status.Text = "Stream loaded!";
                    }
                }
                else
                {
                    label_status.Text = "Processing failed!";
                    if (!bProcessImage)
                    {
                        label_status.Text = "Stream failed to load!";
                    }
                }
            }

            if (gpsStreamWriter != null)
                gpsStreamWriter.Close();
            if (gpsFile != null)
                gpsFile.Close();

            button_abort.Enabled = false;
            button_reset.Enabled = true;
            button_quit.Enabled = true;

            allFieldsControl(true);
        }

        // Updates progress bar and status line
        private void processingThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            label_status.Text = e.UserState.ToString();
        }

        // Worker thread
        unsafe private bool doProcessing(BackgroundWorker worker, DoWorkEventArgs e)
        {
            using (new WaitCursor())
            using (ContextHolder ldContexts = new ContextHolder())
            {
                LadybugError error;

                // Open stream file
                error = Ladybug.InitializeStreamForReading(ldContexts.GetStreamContext(), text_inputFilename.Text, true);
                if (error != LadybugError.LADYBUG_OK)
                {
                    // Display error and clean up
                	String errorStr = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(Ladybug.ErrorToString(error));
                	MessageBox.Show("Could not initialize stream file for reading! \nError: " + errorStr, "Input Stream File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                	resetBadInputFile();
                	return false;
           		}

                // Check operation performed
                // Only get number of images in stream if doProcessImage is false
                // This is added because of context creation/destroy issues due to multithreading (Bug 21117)
                bool doProcessImage = (bool)e.Argument;
                if (!doProcessImage)
                {
                    error = Ladybug.GetStreamNumOfImages(ldContexts.GetStreamContext(), out m_numImages);
                    if (error != LadybugError.LADYBUG_OK)
                    {
                        // Display error and clean up
                    		String errorStr = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(Ladybug.ErrorToString(error));
                    	MessageBox.Show("GetStreamNumImages\nError: " + errorStr, "Stream File Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    	resetBadInputFile();

                        return false;
                    }

                    return true;
                }

                LadybugImage image = new LadybugImage();
                LadybugStreamHeadInfo streamHeaderInfo = new LadybugStreamHeadInfo();
                uint textureWidth, textureHeight;
                LadybugStabilizationParams stabilizationParams = new LadybugStabilizationParams();
                LadybugNMEAGPGGA gpsData = new LadybugNMEAGPGGA();
                bool saveGpsToFile = true;
                int percentComplete = 0;

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return false;
                }

                string configFilename = Path.GetTempFileName();
                error = Ladybug.GetStreamConfigFile(ldContexts.GetStreamContext(), configFilename);
                if (!checkError(error, "GetStreamConfigFile()")) return false;

                error = Ladybug.GetStreamHeader(ldContexts.GetStreamContext(), out streamHeaderInfo, null);
                if (!checkError(error, "GetStreamHeader()")) return false;

                error = Ladybug.SetColorTileFormat(ldContexts.GetContext(), streamHeaderInfo.stippledFormat);
                if (!checkError(error, "SetColorTileFormat()")) return false;

                error = Ladybug.LoadConfig(ldContexts.GetContext(), configFilename);
                if (!checkError(error, "LoadConfig()")) return false;

                System.IO.File.Delete(configFilename);

                error = Ladybug.SetColorProcessingMethod(ldContexts.GetContext(), colorProcessingMethod);
                if (!checkError(error, "SetColorProcessingMethod()")) return false;

                // set falloff correction value and flag
                error = Ladybug.SetFalloffCorrectionFlag(ldContexts.GetContext(), enableFalloff);
                if (!checkError(error, "SetFalloffCorrectionFlag()")) return false;

                if (enableFalloff)
                {
                    error = Ladybug.SetFalloffCorrectionAttenuation(ldContexts.GetContext(), falloffValue);
                    if (!checkError(error, "SetFalloffCorrectionAttenuation()")) return false;
                }

                // read one frame from stream
                error = Ladybug.ReadImageFromStream(ldContexts.GetStreamContext(), out image);
                if (!checkError(error, "ReadImageFromStream()")) return false;

                if (colorProcessingMethod == LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE16)
                {
                    textureWidth = image.uiCols / 4;
                    textureHeight = image.uiRows / 4;
                }
                else if (colorProcessingMethod == LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE4 ||
                    colorProcessingMethod == LadybugColorProcessingMethod.LADYBUG_MONO)
                {
                    textureWidth = image.uiCols / 2;
                    textureHeight = image.uiRows / 2;
                }
                else
                {
                    textureWidth = image.uiCols;
                    textureHeight = image.uiRows;
                }

                // Set blending width
                error = Ladybug.SetBlendingParams(ldContexts.GetContext(), blendingWidth);
                if (!checkError(error, "SetBlendingParams()")) return false;

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return false;
                }
                worker.ReportProgress(percentComplete, "Initializing alpha masks...");

                // Initialize alpha mask size
                error = Ladybug.InitializeAlphaMasks(ldContexts.GetContext(), textureWidth, textureHeight, false);
                if (!checkError(error, "InitializeAlphaMasks()")) return false;

                // Make the rendering engine use the alpha mask
                error = Ladybug.SetAlphaMasking(ldContexts.GetContext(), true);
                if (!checkError(error, "SetAlphaMasking()")) return false;

                // Use software rendering if selected
                error = Ladybug.EnableSoftwareRendering(ldContexts.GetContext(), enableSwRendering);
                if (!checkError(error, "EnableSoftwareRendering()")) return false;

                // Stabilization
                if (enableStabilization)
                {
                    worker.ReportProgress(percentComplete, "Setting stabilization parameters...");
                    stabilizationParams.iNumTemplates = (int)stabilizationNumTemplates;
                    stabilizationParams.iMaximumSearchRegion = (int)stabilizationMaxRegion;
                    stabilizationParams.dDecayRate = stabilizationDecayRate;
                    error = Ladybug.EnableImageStabilization(ldContexts.GetContext(), enableStabilization, ref stabilizationParams);
                    if (!checkError(error, "EnableImageStabilization()")) return false;
                }

                // Use Anti-Aliasing if selected
                error = Ladybug.SetAntiAliasing(ldContexts.GetContext(), enableAntiAliasing);
                if (!checkError(error, "SetAntiAliasing()")) return false;

                // Use sharpening if selected
                error = Ladybug.SetSharpening(ldContexts.GetContext(), enableSharpening);
                if (!checkError(error, "SetSharpening()")) return false;

                // Configure output images in ladybug library
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return false;
                }
                worker.ReportProgress(percentComplete, "Configuring output images in ladybug library...");

                error = Ladybug.ConfigureOutputImages(ldContexts.GetContext(), (uint)outputRenderingType);
                if (!checkError(error, "ConfigureOutputImages()")) return false;

                error = Ladybug.SetOffScreenImageSize(ldContexts.GetContext(), outputRenderingType, (uint)imageSizeWidth, (uint)imageSizeHeight);
                if (!checkError(error, "SetOffScreenImageSize()")) return false;

                if (outputRenderingType == LadybugOutputImage.LADYBUG_SPHERICAL)
                {
                    worker.ReportProgress(percentComplete, "Setting spherical parametrs...");
                    error = Ladybug.SetSphericalViewParams(ldContexts.GetContext(), fov, rotAngleX, rotAngleY, rotAngleZ, 0.0f, 0.0f, 0.0f);
                    if (!checkError(error, "SetSphericalViewParams")) return false;
                }

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return false;
                }
                worker.ReportProgress(percentComplete, "Allocating texture buffers...");

                // Go to the first frame to process
                error = Ladybug.GoToImage(ldContexts.GetStreamContext(), frameRangeMin);

                // Account for 16bit data
                bool isHighBitDepth = dataFormat.isHighBitDepth(image.dataFormat);

                byte[] textureBuffer = new byte[Ladybug.LADYBUG_NUM_CAMERAS * textureWidth * textureHeight * 4 * (isHighBitDepth ? 2 : 1)];

                fixed (byte* textBufPtr = textureBuffer)
                {
                    try
                    {
                        gpsFile = new FileStream(gpsOutputPath, FileMode.Create, FileAccess.Write);
                        gpsStreamWriter = new StreamWriter(gpsFile);
                    }
                    catch
                    {
                        MessageBox.Show("Error creating GPS file. GPS output will be disabled.");
                        saveGpsToFile = false;

                        if (gpsStreamWriter != null)
                            gpsStreamWriter.Close();
                        if (gpsFile != null)
                            gpsFile.Close();
                    }

                    byte** textBufPtrArray = stackalloc byte*[Ladybug.LADYBUG_NUM_CAMERAS];
                    for (int i = 0; i < Ladybug.LADYBUG_NUM_CAMERAS; i++)
                    {
                        textBufPtrArray[i] = textBufPtr + (isHighBitDepth ? i * 2 : i) * textureWidth * textureHeight * 4;
                    }

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return false;
                    }
                    worker.ReportProgress(percentComplete, "Processing images...");

                    // Process frames in range
                    for (int iFrame = (int)frameRangeMin; iFrame <= frameRangeMax; iFrame++)
                    {
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return false;
                        }

                        error = Ladybug.ReadImageFromStream(ldContexts.GetStreamContext(), out image);
                        if (!checkError(error, "ReadImageFromStream()")) return false;

                        // Convert image to BGRU
                        error = Ladybug.ConvertImage(ldContexts.GetContext(), ref image, textBufPtrArray, (isHighBitDepth ? LadybugPixelFormat.LADYBUG_BGRU16 : LadybugPixelFormat.LADYBUG_BGRU));
                        if (!checkError(error, "ConvertImage()")) return false;

                        // Update the textures on the graphics card
                        error = Ladybug.UpdateTextures(ldContexts.GetContext(), Ladybug.LADYBUG_NUM_CAMERAS, textBufPtrArray, (isHighBitDepth ? LadybugPixelFormat.LADYBUG_BGRU16 : LadybugPixelFormat.LADYBUG_BGRU));
                        if (!checkError(error, "UpdateTextures()")) return false;

                        // Output GPS info to file if it exists in image
                        if (saveGpsToFile)
                        {
                            error = Ladybug.GetGPSNMEADataFromImage(ref image, "GPGGA", (void*)&gpsData);

                            if (error == LadybugError.LADYBUG_OK && gpsData.bValidData)
                            {
                                gpsStreamWriter.WriteLine("GPS INFO: LAT " + gpsData.dGGALatitude + ", LONG " + gpsData.dGGALongitude);
                            }
                        }

                        string outputFilename = String.Format("{0}\\{1}_{2}.", outputPath, OUT_FILE_PREFIX, iFrame.ToString("D8"));

                        switch (saveFileFormat)
                        {
                            case LadybugSaveFileFormat.LADYBUG_FILEFORMAT_JPG:
                                outputFilename = outputFilename + "jpg";
                                break;
                            case LadybugSaveFileFormat.LADYBUG_FILEFORMAT_BMP:
                                outputFilename = outputFilename + "bmp";
                                break;
                            case LadybugSaveFileFormat.LADYBUG_FILEFORMAT_PNG:
                                outputFilename = outputFilename + "png";
                                break;
                            case LadybugSaveFileFormat.LADYBUG_FILEFORMAT_TIFF:
                                outputFilename = outputFilename + "tiff";
                                break;
                        }

                        // Render and obtain the image in off-screen buffer
                        LadybugProcessedImage processedImage;
                        error = Ladybug.RenderOffScreenImage(ldContexts.GetContext(), outputRenderingType, (isHighBitDepth ? LadybugPixelFormat.LADYBUG_BGR16 : LadybugPixelFormat.LADYBUG_BGR), out processedImage);
                        if (!checkError(error, "RenderOffScreenImage()")) return false;

                        // Write the rendered image to a file
                        error = Ladybug.SaveImage(ldContexts.GetContext(), ref processedImage, outputFilename, saveFileFormat, true);
                        if (!checkError(error, "SaveImage()")) return false;

                        // Adding 1 here because index 0 counts as an image
                        // Otherwise we would be dividing by 0 when total # of selected frames is 1
                        float TotalFrames = (float)(frameRangeMax - frameRangeMin) + 1;
                        float ProcessedFrames = (float)(iFrame - frameRangeMin) + 1;
                        percentComplete = (int)(ProcessedFrames / TotalFrames * 100);

                        worker.ReportProgress(percentComplete, "Processing images...");
                    }

                    // Release image
                    error = Ladybug.ReleaseOffScreenImage(ldContexts.GetContext(), outputRenderingType);
                    if (!checkError(error, "ReleaseOffScreenImage()")) return false;

                }
            }

            return true;
        }

        // Resets dialog fields to default values
        private void resetToDefaults()
        {
            // Path's
            text_inputFilename.Text = "";
            text_outputPath.Text = "";
            text_gpsPath.Text = "";

            // Frame Range
            combo_minFrame.Text = "";
            combo_maxFrame.Text = "";
            combo_minFrame.Items.Clear();
            combo_maxFrame.Items.Clear();

            // Image size
            text_imageSizeWidth.Text = "2048";
            text_imageSizeHeight.Text = "1024";

            // Color processing method
            combo_colorProcessingType.SelectedIndex = 0;

            // Blending width
            text_blendingWidth.Text = "100";

            // Falloff
            check_enableFalloff.Checked = false;
            text_falloffValue.Text = "1.0";

            // Software Rendering
            check_enableSwRendering.Checked = false;

            // Output format
            radio_saveBMP.Checked = false;
            radio_saveJpeg.Checked = true;
            radio_savePNG.Checked = false;
            radio_saveTIFF.Checked = false;

            // Rendering type and Euler rotation
            combo_renderingType.SelectedItem = "Panoramic";
            text_rotAngleX.Text = "0";
            text_rotAngleY.Text = "0";
            text_rotAngleZ.Text = "0";
            text_fov.Text = "60.0";

            // Stabilzation
            check_enableStabilization.Checked = false;
            text_stabilization_numTemplates.Text = "6";
            text_stabilization_maxRegion.Text = "100";
            text_stabilization_decayRate.Text = "0.95";

            check_enableAntiAliasing.Checked = false;
            check_enableSharpening.Checked = false;

            // buttons and status label
            button_abort.Enabled = false;
            label_status.Text = "";

            // Progress bar
            progressBar.Value = 0;

            // disable all controls except for input file
            allFieldsControl(false);
            text_inputFilename.Enabled = true;
            button_inputFilename.Enabled = true;
            text_inputFilename.Focus();
        }

        // Resets dialog if input-file problem occurs
        private void resetBadInputFile()
        {
            allFieldsControl(false);
            text_inputFilename.Enabled = true;
            button_inputFilename.Enabled = true;
            combo_minFrame.Items.Clear();
            combo_maxFrame.Items.Clear();
            text_inputFilename.Focus();
        }

        // Handler for input-file browse button
        private void button_inputFilename_Click(object sender, EventArgs e)
        {
            // display file browser
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.Title = "Open Ladybug Stream file";
            fDialog.Filter = "Ladybug Stream File (*.pgr)|*.pgr";
            fDialog.InitialDirectory = string.IsNullOrEmpty(text_outputPath.Text) ? @"C:\" : text_outputPath.Text;

            if (fDialog.ShowDialog() != DialogResult.OK)
            {
                if (fDialog.FileName != "")
                {
                	// Don't do image processing, just get number of images in stream
                    bProcessImage = false;
                    processingThread.RunWorkerAsync(bProcessImage);
                }
                return;
            }

            label_status.Text = "Loading stream...";
            text_inputFilename.Text = fDialog.FileName;
            allFieldsControl(false);

            // Don't do image processing, just get number of images in stream
            bProcessImage = false;
            processingThread.RunWorkerAsync(bProcessImage);
        }

        // Handler for output-path browse button
        private void button_outputPath_Click(object sender, EventArgs e)
        {
            // display folder browser and update field
            FolderBrowserDialog fDialog = new FolderBrowserDialog();
            fDialog.Description = "Select Path to Save Image Files";
            fDialog.ShowNewFolderButton = true;
            fDialog.RootFolder = Environment.SpecialFolder.MyComputer;

            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                text_outputPath.Text = fDialog.SelectedPath;
            }
        }

        // Handler for gps-path browse button
        private void button_gpsPath_Click(object sender, EventArgs e)
        {
            // display folder browser and update field
            FolderBrowserDialog fDialog = new FolderBrowserDialog();
            fDialog.Description = "Select Path to Save GPS File";
            fDialog.ShowNewFolderButton = true;
            fDialog.RootFolder = Environment.SpecialFolder.MyComputer;

            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                text_gpsPath.Text = fDialog.SelectedPath;
            }
        }

        // Handler for rendering type combo box
        private void combo_renderingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            text_fov.Enabled = combo_renderingType.SelectedItem.Equals("Spherical");
            text_rotAngleX.Enabled = combo_renderingType.SelectedItem.Equals("Spherical");
            text_rotAngleY.Enabled = combo_renderingType.SelectedItem.Equals("Spherical");
            text_rotAngleZ.Enabled = combo_renderingType.SelectedItem.Equals("Spherical");
        }

        // Handler for falloff checkbox
        private void check_enableFalloff_CheckedChanged(object sender, EventArgs e)
        {
            text_falloffValue.Enabled = check_enableFalloff.Checked;
        }

        // Handler for stabilization checkbox
        private void check_enableStabilization_CheckedChanged(object sender, EventArgs e)
        {
            text_stabilization_numTemplates.Enabled = check_enableStabilization.Checked;
            text_stabilization_maxRegion.Enabled = check_enableStabilization.Checked;
            text_stabilization_decayRate.Enabled = check_enableStabilization.Checked;
        }

        // Handler  for process button
        private void button_process_Click(object sender, EventArgs e)
        {
            // Stores errors from field validation
            ArrayList errorList = new ArrayList();

            // Disable buttons
            button_process.Enabled = false;
            button_quit.Enabled = false;
            button_reset.Enabled = false;

            // Validate fields
            validateFields(ref errorList);

            if (errorList.Count != 0)
            {
                string errors = "";

                for (int i = 0; i < errorList.Count; i++)
                {
                    errors = errors + errorList[i].ToString();
                }

                // display error list
                MessageBox.Show(errors, "Errors Detected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button_process.Enabled = true;
                button_quit.Enabled = true;
                button_reset.Enabled = true;
                return;
            }

            // initialize progress bar
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;

            // start worker thread
            button_abort.Enabled = true;
            allFieldsControl(false);

            // Perform Image Processing
            label_status.Text = "Processing stream...";
            bProcessImage = true;
            processingThread.RunWorkerAsync(bProcessImage);
        }

        // Reset button
        private void button_reset_Click(object sender, EventArgs e)
        {
            resetToDefaults();
        }

        // Validate and update globals
        private void validateFields(ref ArrayList errorList)
        {
            // Clear error list
            errorList.Clear();

            // output path
            validateOutputPath(ref errorList);

            // gps path
            validatGpsPath(ref errorList);

            // frame range
            validateFrameRange(ref errorList);

            // save file format
            if (radio_saveBMP.Checked)
                saveFileFormat = LadybugSaveFileFormat.LADYBUG_FILEFORMAT_BMP;
            else if ( radio_saveJpeg.Checked)
                saveFileFormat = LadybugSaveFileFormat.LADYBUG_FILEFORMAT_JPG;
            else if (radio_savePNG.Checked)
                saveFileFormat = LadybugSaveFileFormat.LADYBUG_FILEFORMAT_PNG;
            else
                saveFileFormat = LadybugSaveFileFormat.LADYBUG_FILEFORMAT_TIFF;

            // software rendering
            enableSwRendering = check_enableSwRendering.Checked;

            // output image size
            validateOutputImageSize(ref errorList);

            // color-processing method
            switch (combo_colorProcessingType.SelectedIndex)
            {
               case 0: colorProcessingMethod = LadybugColorProcessingMethod.LADYBUG_HQLINEAR; break;
               case 1: colorProcessingMethod = LadybugColorProcessingMethod.LADYBUG_HQLINEAR_GPU; break;
               case 2: colorProcessingMethod = LadybugColorProcessingMethod.LADYBUG_DIRECTIONAL_FILTER; break;
               case 3: colorProcessingMethod = LadybugColorProcessingMethod.LADYBUG_WEIGHTED_DIRECTIONAL_FILTER; break;
               case 4: colorProcessingMethod = LadybugColorProcessingMethod.LADYBUG_RIGOROUS; break;
               case 5: colorProcessingMethod = LadybugColorProcessingMethod.LADYBUG_EDGE_SENSING; break;
               case 6: colorProcessingMethod = LadybugColorProcessingMethod.LADYBUG_NEAREST_NEIGHBOR_FAST; break;
               case 7: colorProcessingMethod = LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE4; break;
               case 8: colorProcessingMethod = LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE16; break;
               case 9: colorProcessingMethod = LadybugColorProcessingMethod.LADYBUG_MONO; break;
               case 10: colorProcessingMethod = LadybugColorProcessingMethod.LADYBUG_DISABLE; break;
            }

            // blending width
            validateBlendingWidth(ref errorList);

            // falloff correction
            validateFalloffCorrection(ref errorList);

            // image rendering type (fov, euler angle)
            validateRenderingType(ref errorList);

            // stabilization
            validateStabilization(ref errorList);

            // Sharpening and anti-aliasing
            enableSharpening = check_enableSharpening.Checked;
            enableAntiAliasing = check_enableAntiAliasing.Checked;
        }

        private void validateInputPath()
        {
            string path = text_inputFilename.Text;
            int index = path.LastIndexOf('\\');

            if (text_outputPath.Text.Equals(""))
                text_outputPath.Text = path.Substring(0, index);

            if (text_gpsPath.Text.Equals(""))
                text_gpsPath.Text = path.Substring(0, index);

            combo_minFrame.Items.Clear();
            combo_maxFrame.Items.Clear();

            for (int i = 0; i < m_numImages; i++)
            {
                combo_minFrame.Items.Add(i);
                combo_maxFrame.Items.Add(i);
            }

            combo_minFrame.SelectedIndex = 0;
            combo_maxFrame.SelectedIndex = (int)m_numImages - 1;

            allFieldsControl(true);
        }

        private void validateOutputPath(ref ArrayList errorList)
        {
            outputPath = text_outputPath.Text;

            if (!Path.IsPathRooted(outputPath))
            {
                errorList.Add("Ouptut Path: Absolute path required.\n");
                return;
            }
            try
            {
                if(!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
            }
            catch (Exception e)
            {
                errorList.Add("Output directory error: " + e.ToString());
            }
        }

        private void validateFrameRange(ref ArrayList errorList)
        {
            frameRangeMin = Convert.ToUInt32(combo_minFrame.Text);
            frameRangeMax = Convert.ToUInt32(combo_maxFrame.Text);

            if (frameRangeMin > frameRangeMax)
            {
                errorList.Add("Invalid frame range specified.\n");
            }
        }

        private void validatGpsPath(ref ArrayList errorList)
        {
            gpsOutputPath = text_gpsPath.Text;

            if (!Path.IsPathRooted(gpsOutputPath))
            {
                errorList.Add("GPS Path: Absolute path required.\n");
                return;
            }
            try
            {
                if (!Directory.Exists(gpsOutputPath))
                {
                    Directory.CreateDirectory(gpsOutputPath);
                }
            }
            catch (Exception)
            {
                errorList.Add("Output directory error.\n");
            }
            gpsOutputPath = gpsOutputPath + "\\" + GPS_FILE_PREFIX + frameRangeMin + "_" + frameRangeMax + ".txt";
        }

        private void validateOutputImageSize(ref ArrayList errorList)
        {
            try
            {
                imageSizeWidth = Convert.ToUInt32(text_imageSizeWidth.Text);
                imageSizeHeight = Convert.ToUInt32(text_imageSizeHeight.Text);
            }
            catch (Exception)
            {
                errorList.Add("Invalid image size specified.\n");
            }
        }

        private void validateBlendingWidth(ref ArrayList errorList)
        {
            try
            {
                blendingWidth = Convert.ToUInt32(text_blendingWidth.Text);
            }
            catch (Exception)
            {
                errorList.Add("Invalid blending width specified.\n");
            }
        }

        private void validateStabilization(ref ArrayList errorList)
        {
            enableStabilization = check_enableStabilization.Checked;

            if (enableStabilization)
            {
                try
                {
                    stabilizationNumTemplates = Convert.ToUInt32(text_stabilization_numTemplates.Text);
                    stabilizationMaxRegion = Convert.ToUInt32(text_stabilization_maxRegion.Text);
                    stabilizationDecayRate = (float)Convert.ToDouble(text_stabilization_decayRate.Text);
                }
                catch
                {
                    errorList.Add("Invalid stabilization parameters specified.");
                }
            }
        }

        private void validateFalloffCorrection(ref ArrayList errorList)
        {
            enableFalloff = check_enableFalloff.Checked;

            if (enableFalloff)
            {
                try
                {
                    falloffValue = (float)Convert.ToDouble(text_falloffValue.Text);
                }
                catch (Exception)
                {
                    errorList.Add("Invalid falloff value specified.\n");
                }
            }
        }

        private void text_inputFilename_Leave(object sender, EventArgs e)
        {
            if (button_quit.Focused || button_reset.Focused || button_inputFilename.Focused || text_inputFilename.Text == "")
                return;
            validateInputPath();
        }

        private void validateRenderingType(ref ArrayList errorList)
        {
            switch (combo_renderingType.SelectedIndex)
            {
                case 0:
                    outputRenderingType = LadybugOutputImage.LADYBUG_PANORAMIC;
                    break;
                case 1:
                    outputRenderingType = LadybugOutputImage.LADYBUG_DOME;
                    break;
                case 2:
                    outputRenderingType = LadybugOutputImage.LADYBUG_SPHERICAL;

                    try
                    {
                        fov = (float)Convert.ToDouble(text_fov.Text);
                        rotAngleX = (float)Convert.ToDouble(text_rotAngleX.Text);
                        rotAngleY = (float)Convert.ToDouble(text_rotAngleY.Text);
                        rotAngleZ = (float)Convert.ToDouble(text_rotAngleZ.Text);
                    }
                    catch(Exception)
                    {
                        errorList.Add("Invalid Rendering Type/FOV parameters specified.\n");
                    }
                    break;
                case 3:
                    outputRenderingType = LadybugOutputImage.LADYBUG_RECTIFIED_CAM0;
                    break;
                case 4:
                    outputRenderingType = LadybugOutputImage.LADYBUG_RECTIFIED_CAM1;
                    break;
                case 5:
                    outputRenderingType = LadybugOutputImage.LADYBUG_RECTIFIED_CAM2;
                    break;
                case 6:
                    outputRenderingType = LadybugOutputImage.LADYBUG_RECTIFIED_CAM3;
                    break;
                case 7:
                    outputRenderingType = LadybugOutputImage.LADYBUG_RECTIFIED_CAM4;
                    break;
                case 8:
                    outputRenderingType = LadybugOutputImage.LADYBUG_RECTIFIED_CAM5;
                    break;
                default:
                    errorList.Add("Invalid Rendering Type/FOV parameters specified.\n");
                    break;
            }
        }

        private void button_quit_Click(object sender, EventArgs e)
        {
            Form1.ActiveForm.Close();
        }

        private void button_abort_Click(object sender, EventArgs e)
        {
            processingThread.CancelAsync();
            button_abort.Enabled = false;
            label_status.Text = "Aborting operation, please wait...";
        }

        private void allFieldsControl(bool isEnabled)
        {
            // Path's
            text_inputFilename.Enabled = isEnabled;
            button_inputFilename.Enabled = isEnabled;
            text_outputPath.Enabled = isEnabled;
            button_outputPath.Enabled = isEnabled;
            text_gpsPath.Enabled = isEnabled;
            button_gpsPath.Enabled = isEnabled;

            // Frame Range
            combo_minFrame.Enabled = isEnabled;
            combo_maxFrame.Enabled = isEnabled;

            // Image size
            text_imageSizeWidth.Enabled = isEnabled;
            text_imageSizeHeight.Enabled = isEnabled;

            // Color processing method
            combo_colorProcessingType.Enabled = isEnabled;

            // Blending width
            text_blendingWidth.Enabled = isEnabled;

            // Falloff
            check_enableFalloff.Enabled = isEnabled;
            text_falloffValue.Enabled = isEnabled ? check_enableFalloff.Checked : false;

            // Software Rendering
            check_enableSwRendering.Enabled = isEnabled;

            check_enableAntiAliasing.Enabled = isEnabled;
            check_enableSharpening.Enabled = isEnabled;

            // Output format
            radio_saveBMP.Enabled = isEnabled;
            radio_saveJpeg.Enabled = isEnabled;
            radio_savePNG.Enabled = isEnabled;
            radio_saveTIFF.Enabled = isEnabled;

            // Rendering type and Euler rotation
            combo_renderingType.Enabled = isEnabled;
            text_fov.Enabled = isEnabled ? combo_renderingType.SelectedItem.Equals("Spherical") : false;
            text_rotAngleX.Enabled = isEnabled ? combo_renderingType.SelectedItem.Equals("Spherical") : false;
            text_rotAngleY.Enabled = isEnabled ? combo_renderingType.SelectedItem.Equals("Spherical") : false;
            text_rotAngleZ.Enabled = isEnabled ? combo_renderingType.SelectedItem.Equals("Spherical") : false;

            // Stabilization
            check_enableStabilization.Enabled = isEnabled;
            text_stabilization_numTemplates.Enabled = isEnabled ? check_enableStabilization.Checked : false;
            text_stabilization_maxRegion.Enabled = isEnabled ? check_enableStabilization.Checked : false;
            text_stabilization_decayRate.Enabled = isEnabled ? check_enableStabilization.Checked : false;

            // Progress bar
            progressBar.Value = 0;

            button_process.Enabled = isEnabled;
        }

        private bool checkError(LadybugError error, string msg)
        {
            if (error != LadybugError.LADYBUG_OK)
            {
                String errorStr = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(Ladybug.ErrorToString(error));
                MessageBox.Show(msg + "\nError: " + errorStr, "A Processing Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (gpsStreamWriter != null)
                    gpsStreamWriter.Close();
                if (gpsFile != null)
                    gpsFile.Close();
                return false;
            }
            return true;
        }

        private bool checkFatalError(LadybugError error, string msg)
        {
            if (error != LadybugError.LADYBUG_OK)
            {
                String errorStr = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(Ladybug.ErrorToString(error));
                MessageBox.Show(msg + "\nError: " + errorStr, "A Fatal Error Has Occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
     }
}