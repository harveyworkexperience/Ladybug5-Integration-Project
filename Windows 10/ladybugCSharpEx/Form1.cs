//=============================================================================
// Copyright © 2009 Point Grey Research, Inc. All Rights Reserved.
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
// $Id$
//=============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using System.Runtime.InteropServices;
using LadybugAPI;
using System.Runtime.CompilerServices;

namespace LadybugCSharpEx
{
    unsafe public partial class Form1 : Form
    {
        // contexts and some key components
        private IntPtr m_ladybugContext;
        private IntPtr m_streamContext;
        private IntPtr m_guiContext;
        private OpenGLBase m_openGlBase;

        // the current image
        private LadybugImage m_currentImage;
        private System.Object m_currentImageLock = new System.Object();

        // textures
        private byte[] m_textureBuffer;
        private bool m_textureSizeChanged = false;

        // view-related
        private LadybugOutputImage m_viewType = LadybugOutputImage.LADYBUG_PANORAMIC;
        private System.Object m_viewTypeLock = new System.Object();
        private bool m_isImageUpdated = false;
        private float pan = 0.0f;
        private float tilt = 0.0f;
        private float zoom = 50.0f;
        private int prev_x = 0;
        private int prev_y = 0;

        // other variables
        private FrameRate m_frameRate = new FrameRate();
        private bool m_isInitialized = false;
        private bool m_continueGrabbing = false;
        private Thread m_grabThread;
        private string m_gpsInfoText = String.Empty;
        private TrackBar m_streamPositionTrackBar;

        public Form1()
        {
            this.Text = "Ladybug C# Example";

            // Set up the TrackBar.
            this.m_streamPositionTrackBar = new System.Windows.Forms.TrackBar();
            this.Controls.Add(m_streamPositionTrackBar);
            this.m_streamPositionTrackBar.Location = new System.Drawing.Point(600, 762);
            this.m_streamPositionTrackBar.Size = new System.Drawing.Size(700, 35);
            this.m_streamPositionTrackBar.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            this.m_streamPositionTrackBar.Maximum = 500;
            this.m_streamPositionTrackBar.TickFrequency = 50;
            this.m_streamPositionTrackBar.LargeChange = 10;
            this.m_streamPositionTrackBar.SmallChange = 1;
            this.m_streamPositionTrackBar.Enabled = false;
            this.m_streamPositionTrackBar.Visible = false;

            InitializeComponent();

            this.viewTypeCombo.SelectedIndex = 0;
            this.colorProcCombo.SelectedIndex = 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            cleanup();

            // This fixes the issue of not being able to close the form.
            e.Cancel = false;

            base.OnFormClosing(e);
        }

        private void trackBar1_Scroll(object sender, System.EventArgs e)
        {
            if (m_isImageUpdated) // if the previously processed image is not displayed, do nothing.
                return;

            LadybugError error = Ladybug.GoToImage(m_streamContext, (uint)m_streamPositionTrackBar.Value);
            handleError(error);

            lock (m_currentImageLock)
            {
                error = Ladybug.ReadImageFromStream(m_streamContext, out m_currentImage);
                handleError(error);
            }

            processAndDisplay();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            lock (this)
            {
                if (m_isInitialized)
                {
                    m_isImageUpdated = false;

                    gpsLabel.Text = "GPS: " + m_gpsInfoText;

                    m_openGlBase.bind();

                    GL.glViewport(0, 0, pictureBox1.Width, pictureBox1.Height);

                    lock (m_viewTypeLock)
                    {
                        LadybugError error = Ladybug.DisplayImage(m_ladybugContext, m_viewType);
                        handleError(error);
                    }

                    m_openGlBase.swapBuffers();
                    m_openGlBase.unbind();

                    if (m_continueGrabbing)
                    {
                        m_frameRate.grab();
                        double frame_rate = m_frameRate.getFPS();
                        String fpsStr = frame_rate.ToString();
                        if (fpsStr.Length > 5) fpsStr = fpsStr.Remove(5);
                        fpsLabel.Text = "FPS: " + fpsStr + "/ second";
                    }
                }
            }
        }

        private void initStream()
        {
            initContexts();

            LadybugError error = Ladybug.CreateStreamContext(out m_streamContext);
            handleError(error);

            OpenFileDialog streamFileSelector = new OpenFileDialog();
            streamFileSelector.Filter = "Stream files (*.pgr)|*.pgr|All files (*.*)|*.*";

            DialogResult dlgRes = streamFileSelector.ShowDialog();
            if (dlgRes != DialogResult.OK)
                return;

            string path = streamFileSelector.FileName;

            error = Ladybug.InitializeStreamForReading(m_streamContext, path, true);
            handleError(error);

            string config_path = System.IO.Path.GetTempFileName();
            error = Ladybug.GetStreamConfigFile(m_streamContext, config_path);
            handleError(error);

            LadybugStreamHeadInfo streamHeadInfo;
            error = Ladybug.GetStreamHeader(m_streamContext, out streamHeadInfo, null);
            handleError(error);

            string caption = "LadybugCSharpEx - " + path;
            this.Text = caption;

            if (streamHeadInfo.serialHead != 0)
                serialLabel.Text = "Serial number: " + streamHeadInfo.serialHead.ToString();
            else
                serialLabel.Text = "Serial number: N/A";

            error = Ladybug.SetColorTileFormat(m_ladybugContext, streamHeadInfo.stippledFormat);
            handleError(error);

            error = Ladybug.LoadConfig(m_ladybugContext, config_path);
            handleError(error);

            System.IO.File.Delete(config_path);

            lock (m_currentImageLock)
            {
                error = Ladybug.ReadImageFromStream(m_streamContext, out m_currentImage);
                handleError(error);
            }

            uint numImages = 0;
            error = Ladybug.GetStreamNumOfImages(m_streamContext, out numImages);
            handleError(error);

            this.m_streamPositionTrackBar.Minimum = 0;
            this.m_streamPositionTrackBar.Maximum = (int)numImages - 1;
            this.m_streamPositionTrackBar.Value = 0;
            this.m_streamPositionTrackBar.Enabled = true;
            this.m_streamPositionTrackBar.Visible = true;
            this.camCtrlBtn.Visible = false;
            this.fpsLabel.Visible = false;
            this.gpsLabel.Text = "GPS: N/A";
            m_gpsInfoText = "";

            bool res = commonInitialize();
            processAndDisplay();
        }

        private void initCamera()
        {
            initContexts();

            int serialNumber, dialogStatus;
            CameraGUIError guierror = CameraGUI.ShowCameraSelectionModal(m_guiContext, m_ladybugContext, out serialNumber, out dialogStatus);
            handleError(guierror);

            if (serialNumber == 0)
                return;

            LadybugError error = Ladybug.InitializeFromSerialNumber(m_ladybugContext, serialNumber);
            handleError(error);

            LadybugCameraInfo camInfo = new LadybugCameraInfo();
            error = Ladybug.GetCameraInfo(m_ladybugContext, ref camInfo);
            handleError(error);

            string caption = "LadybugCSharpEx - " + camInfo.pszModelName + "(" + serialNumber + ")";
            this.Text = caption;

            serialLabel.Text = "Serial number: " + serialNumber;

            guierror = CameraGUI.InitializeSettingsDialog(m_guiContext, m_ladybugContext);
            handleError(guierror);

            error = Ladybug.LoadConfig(m_ladybugContext, null);
            handleError(error);

            error = Ladybug.Start(
                        m_ladybugContext,
                        LadybugDataFormat.LADYBUG_DATAFORMAT_ANY);
            handleError(error);

            // grabs a frame to obtain the image size needed for initialization.
            int retry = 5;
            do
            {
                lock (m_currentImageLock)
                {
                    error = Ladybug.GrabImage(m_ladybugContext, out m_currentImage);
                    retry++;
                }
            } while (error != LadybugError.LADYBUG_OK && retry > 0);
            handleError(error);

            this.m_streamPositionTrackBar.Enabled = false;
            this.m_streamPositionTrackBar.Visible = false;
            this.fpsLabel.Visible = true;
            this.camCtrlBtn.Visible = true;
            this.gpsLabel.Text = "GPS: N/A";
            m_gpsInfoText = "";

            bool res = commonInitialize();
            processAndDisplay();

            // start a grab thread which keeps grabbing and processing images.
            m_grabThread = new Thread(new ThreadStart(grabLoop));
            m_continueGrabbing = true;
            m_grabThread.Start();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void processAndDisplay()
        {
            if (m_isImageUpdated) // if the previously processed image is not displayed, do nothing.
            {
                return;
            }

            lock (m_currentImageLock)
            {
                Console.WriteLine("process_and_display seqid = " + m_currentImage.imageInfo.ulSequenceId);

                LadybugError error;
                fixed (byte* texBufPtr = &m_textureBuffer[0])
                {
                    // Account for 16 bit data
                    bool isHighBitDepth = dataFormat.isHighBitDepth(m_currentImage.dataFormat);

                    // this is a trick to make a pointer of arrays.
                    byte** texBufPtrArray = stackalloc byte*[6];
                    for (int i = 0; i < 6; i++)
                    {
                        texBufPtrArray[i] = texBufPtr + (isHighBitDepth ? i * 2 : i) * m_currentImage.uiRows * m_currentImage.uiCols * 4;
                    }

                    if (m_textureSizeChanged)
                    {
                        m_textureSizeChanged = false;
                        initializeAlphaMasks();
                    }

                    error = Ladybug.ConvertImage(m_ladybugContext, ref m_currentImage, texBufPtrArray, (isHighBitDepth ? LadybugPixelFormat.LADYBUG_BGRU16 : LadybugPixelFormat.LADYBUG_BGRU));
                    handleError(error);
                    if (error == LadybugError.LADYBUG_OK)
                    {
                        error = Ladybug.UpdateTextures(m_ladybugContext, Ladybug.LADYBUG_NUM_CAMERAS, texBufPtrArray, (isHighBitDepth ? LadybugPixelFormat.LADYBUG_BGRU16 : LadybugPixelFormat.LADYBUG_BGRU));
                        handleError(error);
                    }
                }

                // obtain GPS info ( only checking GPGGA sentense)
                LadybugNMEAGPGGA GGA;
                void* pGGA = &GGA;
                error = Ladybug.GetGPSNMEADataFromImage(ref m_currentImage, "GPGGA", pGGA);
                if (error == LadybugError.LADYBUG_OK && GGA.bValidData)
                {
                    m_gpsInfoText = "LAT " + GGA.dGGALatitude + " LON " + GGA.dGGALongitude;
                }
                else
                {
                    m_gpsInfoText = "N/A";
                }

                m_isImageUpdated = true;
            }

            this.Invalidate(); // this will invoke OnPaint();
        }

        private void grabLoop()
        {
            Console.Out.WriteLine("grabLoop started");
            LadybugError error;
            while (m_continueGrabbing)
            {
                lock (m_currentImageLock)
                {
                    error = Ladybug.GrabImage(m_ladybugContext, out m_currentImage);
                    handleError(error);
                }

                processAndDisplay();
            }
            Console.Out.WriteLine("grabLoop done");
        }

        // clicked "Open stream file"
        private void streamBtn_Click(object sender, EventArgs e)
        {
            cleanup();
            initStream();
        }

        // clicked "Select Camera"
        private void selectBtn_Click(object sender, EventArgs e)
        {
            cleanup();
            initCamera();
        }

        private void initContexts()
        {
            LadybugError error = Ladybug.CreateContext(out m_ladybugContext);
            handleError(error);

            CameraGUIError guierror = CameraGUI.CreateContext(out m_guiContext);
            handleError(guierror);

            m_openGlBase = new OpenGLBase();
            if (!m_openGlBase.initialize(pictureBox1))
                MessageBox.Show("Failed to initialize OpenGL.");
        }

        private bool initializeAlphaMasks()
        {
            LadybugError error;

            if (m_ladybugContext == IntPtr.Zero)
            {
                return false;
            }

            LadybugColorProcessingMethod curMethod;
            Ladybug.GetColorProcessingMethod(m_ladybugContext, out curMethod);

            uint texture_width = m_currentImage.uiCols;
            uint texture_height = m_currentImage.uiRows;

            if (curMethod == LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE4 ||
                curMethod == LadybugColorProcessingMethod.LADYBUG_MONO)
            {
                texture_width /= 2;
                texture_height /= 2;
            }
            else if (curMethod == LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE16)
            {
                texture_width /= 4;
                texture_height /= 4;
            }

            error = Ladybug.LoadAlphaMasks(m_ladybugContext, texture_width, texture_height);
            if (error != LadybugError.LADYBUG_OK)
            {
                error = Ladybug.InitializeAlphaMasks(m_ladybugContext, texture_width, texture_height, false);
                handleError(error);
            }

            return (error == 0);
        }

        public bool commonInitialize()
        {
            LadybugColorProcessingMethod colorProc = LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE4; // fast
            LadybugError error = Ladybug.SetColorProcessingMethod(m_ladybugContext, colorProc);
            handleError(error);

            // Account for 16 bit data
            bool isHighBitDepth = dataFormat.isHighBitDepth(m_currentImage.dataFormat);

            m_textureBuffer = new byte[Ladybug.LADYBUG_NUM_CAMERAS * m_currentImage.uiRows * m_currentImage.uiCols * 4 * (isHighBitDepth ? 2 : 1)];

            initializeAlphaMasks();

            error = Ladybug.ConfigureOutputImages(m_ladybugContext, (0x1 << 12) /*LadybugOutputImage.LADYBUG_PANORAMIC*/);
            handleError(error);

            m_openGlBase.bind();
            error = Ladybug.SetDisplayWindow(m_ladybugContext);
            handleError(error);
            m_openGlBase.unbind();

            m_isImageUpdated = false;

            if (error == 0)
            {
                m_isInitialized = true;
            }
            return (error == 0);
        }

        private void cleanup()
        {
            LadybugError error;
            CameraGUIError guierror;

            m_isInitialized = false;
            m_isImageUpdated = false;

            if (m_continueGrabbing)
            {
                m_continueGrabbing = false;
                int cnt = 0;
                while (m_grabThread.IsAlive)
                {
                    Thread.Sleep(10);
                    cnt++;
                }

                error = Ladybug.Stop(m_ladybugContext);
                handleError(error);
            }

            if (m_streamContext != IntPtr.Zero)
            {
                error = Ladybug.StopStream(m_streamContext);
                handleError(error);

                error = Ladybug.DestroyStreamContext(ref m_streamContext);
                handleError(error);

                m_streamContext = IntPtr.Zero;
            }

            if (m_guiContext != IntPtr.Zero)
            {
                guierror = CameraGUI.DestroyContext(m_guiContext);
                handleError(guierror);

                m_guiContext = IntPtr.Zero;
            }

            if (m_ladybugContext != IntPtr.Zero)
            {
                error = Ladybug.DestroyContext(ref m_ladybugContext);
                handleError(error);

                m_ladybugContext = IntPtr.Zero;
            }

            if (m_openGlBase != null)
            {
                m_openGlBase.finish();
                m_openGlBase = null;
            }
        }

        // clicked "Cam control"
        private void camCtlBtn_Click(object sender, EventArgs e)
        {
            // if grab thread is not running, we can assume the camera is not selected.
            if (!m_continueGrabbing)
            {
                return;
            }

            if (m_guiContext != IntPtr.Zero)
            {
                IntPtr hWnd = this.Handle;
                CameraGUIError error = CameraGUI.ToggleSettingsWindowState(m_guiContext, hWnd);
                handleError(error);
            }
        }

        public void handleError(LadybugError errorCode)
        {
            if (errorCode != LadybugError.LADYBUG_OK)
            {
                //Console.Out.WriteLine(Ladybug.ErrorToString(errorCode));
                MessageBox.Show(System.Runtime.InteropServices.Marshal.PtrToStringAnsi(Ladybug.ErrorToString(errorCode)));
            }
        }

        public void handleError(CameraGUIError errorCode)
        {
            if (errorCode != CameraGUIError.PGRCAMGUI_OK)
                MessageBox.Show("CameraGUIError error code = " + errorCode.ToString());

        }

        private void viewTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (m_viewTypeLock)
            {
                switch (viewTypeCombo.SelectedIndex)
                {
                    case 0: m_viewType = LadybugOutputImage.LADYBUG_PANORAMIC; break;
                    case 1: m_viewType = LadybugOutputImage.LADYBUG_DOME; break;
                    case 2: m_viewType = LadybugOutputImage.LADYBUG_ALL_CAMERAS_VIEW; break;
                    case 3: m_viewType = LadybugOutputImage.LADYBUG_SPHERICAL; break;
                }

                Invalidate();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pan -= (e.X - prev_x) / 200.0f;
                tilt += (e.Y - prev_y) / 200.0f;

                Ladybug.SetSphericalViewParams(m_ladybugContext, zoom, 0.0f, tilt, pan, 0.0f, 0.0f, 0.0f);
                this.Invalidate(); // this will invoke OnPaint();

                prev_x = e.X;
                prev_y = e.Y;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            prev_x = e.X;
            prev_y = e.Y;

        }

        private void form1_MouseWheel(object sender, MouseEventArgs e)
        {
            zoom += e.Delta / 20.0f;
            if (zoom > 170.0f) zoom = 170.0f;
            if (zoom < 0.0f) zoom = 0.0f;

            Ladybug.SetSphericalViewParams(m_ladybugContext, zoom, 0.0f, tilt, pan, 0.0f, 0.0f, 0.0f);
            this.Invalidate(); // this will invoke OnPaint();
        }

        private void colorProcCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_ladybugContext == IntPtr.Zero)
            {
                return;
            }

            LadybugColorProcessingMethod newMethod = LadybugColorProcessingMethod.LADYBUG_DISABLE;
            LadybugColorProcessingMethod curMethod;
            Ladybug.GetColorProcessingMethod(m_ladybugContext, out curMethod);

            int textureScaleFactor = 1;
            switch (colorProcCombo.SelectedIndex)
            {
                case 0: newMethod = LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE4; textureScaleFactor = 2; break;
                case 1: newMethod = LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE16; textureScaleFactor = 4; break;
                case 2: newMethod = LadybugColorProcessingMethod.LADYBUG_NEAREST_NEIGHBOR_FAST; break;
                case 3: newMethod = LadybugColorProcessingMethod.LADYBUG_EDGE_SENSING; break;
                case 4: newMethod = LadybugColorProcessingMethod.LADYBUG_HQLINEAR; break;
                case 5: newMethod = LadybugColorProcessingMethod.LADYBUG_HQLINEAR_GPU; break;
                case 6: newMethod = LadybugColorProcessingMethod.LADYBUG_DIRECTIONAL_FILTER; break;
                case 7: newMethod = LadybugColorProcessingMethod.LADYBUG_WEIGHTED_DIRECTIONAL_FILTER; break;
                case 8: newMethod = LadybugColorProcessingMethod.LADYBUG_RIGOROUS; break;
                case 9: newMethod = LadybugColorProcessingMethod.LADYBUG_MONO; textureScaleFactor = 2; break;
                case 10: newMethod = LadybugColorProcessingMethod.LADYBUG_DISABLE; break;
            }

            Ladybug.SetColorProcessingMethod(m_ladybugContext, newMethod);

            if (curMethod == LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE16 &&
                textureScaleFactor != 4)
            {
                m_textureSizeChanged = true;
            }
            else if ((curMethod == LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE4 ||
                 curMethod == LadybugColorProcessingMethod.LADYBUG_MONO) &&
                 textureScaleFactor != 2)
            {
                m_textureSizeChanged = true;
            }
            else if ((curMethod != LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE4 &&
                      curMethod != LadybugColorProcessingMethod.LADYBUG_DOWNSAMPLE16 &&
                      curMethod != LadybugColorProcessingMethod.LADYBUG_MONO) &&
                      textureScaleFactor != 1)
            {
                m_textureSizeChanged = true;
            }

            processAndDisplay();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.m_streamPositionTrackBar.Location =
               new Point(this.ClientSize.Width - this.m_streamPositionTrackBar.Size.Width - 20,
                         this.ClientSize.Height - this.m_streamPositionTrackBar.Size.Height + 12);
            this.pictureBox1.Height = this.ClientSize.Height - this.m_streamPositionTrackBar.Size.Height - 22;
            this.pictureBox1.Width = this.ClientSize.Width;
        }

        private void openStreamFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            streamBtn_Click(sender, e);
        }

        private void selectCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectBtn_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class FrameRate
    {
        // frame rate is calculated based on the past [numIndex] frames grabbed.

        const int numIndex = 30; // if this is bigger, the changes get smoother but slower to adapt.
        private int index = 0;
        private int numGrabs = 0;
        private DateTime[] grabTime;

        public FrameRate()
        {
            grabTime = new DateTime[numIndex];
        }

        // Call this when you grabbed something.
        // This method just records the time stamp.
        public void grab()
        {
            DateTime now = DateTime.Now;
            index = numGrabs % numIndex;
            grabTime[index] = now;
            numGrabs++;
        }

        // Returns frames per second
        public double getFPS()
        {
            int latestGrab = index;
            int oldestGrab = (numGrabs > numIndex - 1) ? (index + 1) % numIndex : 0;

            int grabs = (numGrabs > numIndex - 1) ? numIndex - 1 : numGrabs;
            TimeSpan span = grabTime[latestGrab] - grabTime[oldestGrab];

            if (span.TotalMilliseconds == 0)
                return 0.0;

            double frameRate = grabs * 1000.0 / span.TotalMilliseconds;
            return frameRate;

        }

        public int getGrabCount()
        {
            return numGrabs;
        }
    }
}