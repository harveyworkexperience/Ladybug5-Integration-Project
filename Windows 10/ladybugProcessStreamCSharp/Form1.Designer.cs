namespace ladybugProcessStream_CSharp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.text_inputFilename = new System.Windows.Forms.TextBox();
            this.button_inputFilename = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.combo_minFrame = new System.Windows.Forms.ComboBox();
            this.combo_maxFrame = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.text_outputPath = new System.Windows.Forms.TextBox();
            this.button_outputPath = new System.Windows.Forms.Button();
            this.text_gpsPath = new System.Windows.Forms.TextBox();
            this.button_gpsPath = new System.Windows.Forms.Button();
            this.text_imageSizeWidth = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.text_imageSizeHeight = new System.Windows.Forms.TextBox();
            this.combo_renderingType = new System.Windows.Forms.ComboBox();
            this.combo_colorProcessingType = new System.Windows.Forms.ComboBox();
            this.text_blendingWidth = new System.Windows.Forms.TextBox();
            this.text_falloffValue = new System.Windows.Forms.TextBox();
            this.check_enableFalloff = new System.Windows.Forms.CheckBox();
            this.check_enableSwRendering = new System.Windows.Forms.CheckBox();
            this.check_enableStabilization = new System.Windows.Forms.CheckBox();
            this.text_stabilization_numTemplates = new System.Windows.Forms.TextBox();
            this.text_stabilization_maxRegion = new System.Windows.Forms.TextBox();
            this.text_stabilization_decayRate = new System.Windows.Forms.TextBox();
            this.text_fov = new System.Windows.Forms.TextBox();
            this.text_rotAngleX = new System.Windows.Forms.TextBox();
            this.button_quit = new System.Windows.Forms.Button();
            this.button_reset = new System.Windows.Forms.Button();
            this.button_process = new System.Windows.Forms.Button();
            this.text_rotAngleY = new System.Windows.Forms.TextBox();
            this.text_rotAngleZ = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.radio_saveJpeg = new System.Windows.Forms.RadioButton();
            this.radio_saveBMP = new System.Windows.Forms.RadioButton();
            this.group_saveFormat = new System.Windows.Forms.GroupBox();
            this.radio_saveTIFF = new System.Windows.Forms.RadioButton();
            this.radio_savePNG = new System.Windows.Forms.RadioButton();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label_status = new System.Windows.Forms.Label();
            this.button_abort = new System.Windows.Forms.Button();
            this.check_enableAntiAliasing = new System.Windows.Forms.CheckBox();
            this.check_enableSharpening = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.group_saveFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // text_inputFilename
            // 
            this.text_inputFilename.Location = new System.Drawing.Point(109, 12);
            this.text_inputFilename.Name = "text_inputFilename";
            this.text_inputFilename.Size = new System.Drawing.Size(425, 20);
            this.text_inputFilename.TabIndex = 0;
            this.text_inputFilename.Leave += new System.EventHandler(this.text_inputFilename_Leave);
            // 
            // button_inputFilename
            // 
            this.button_inputFilename.Location = new System.Drawing.Point(540, 12);
            this.button_inputFilename.Name = "button_inputFilename";
            this.button_inputFilename.Size = new System.Drawing.Size(75, 23);
            this.button_inputFilename.TabIndex = 1;
            this.button_inputFilename.Text = "Browse";
            this.button_inputFilename.UseVisualStyleBackColor = true;
            this.button_inputFilename.Click += new System.EventHandler(this.button_inputFilename_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Stream File Path:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Range of Frames to Process:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Output Path:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "GPS Output Path:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "Output Image Size:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(526, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(153, 13);
            this.label6.TabIndex = 38;
            this.label6.Text = "Output Image Rendering Type:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 168);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(134, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Debayering Method:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 198);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "Blending Width:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(23, 247);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(119, 13);
            this.label10.TabIndex = 37;
            this.label10.Text = "Falloff Correction Value:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(542, 248);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(193, 13);
            this.label14.TabIndex = 44;
            this.label14.Text = "Stabilization Parameter (# of templates):";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(542, 272);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(237, 13);
            this.label15.TabIndex = 45;
            this.label15.Text = "Stabilization Parameter (Maximum search region):";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(542, 296);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(178, 13);
            this.label16.TabIndex = 46;
            this.label16.Text = "Stabilization Parameter (Decay rate):";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(553, 126);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(83, 13);
            this.label17.TabIndex = 39;
            this.label17.Text = "FOV in degrees:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(553, 149);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(107, 13);
            this.label18.TabIndex = 40;
            this.label18.Text = "Euler Rotation Angle:";
            // 
            // combo_minFrame
            // 
            this.combo_minFrame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_minFrame.FormattingEnabled = true;
            this.combo_minFrame.Location = new System.Drawing.Point(157, 99);
            this.combo_minFrame.Name = "combo_minFrame";
            this.combo_minFrame.Size = new System.Drawing.Size(121, 21);
            this.combo_minFrame.TabIndex = 6;
            // 
            // combo_maxFrame
            // 
            this.combo_maxFrame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_maxFrame.FormattingEnabled = true;
            this.combo_maxFrame.Location = new System.Drawing.Point(306, 99);
            this.combo_maxFrame.Name = "combo_maxFrame";
            this.combo_maxFrame.Size = new System.Drawing.Size(121, 21);
            this.combo_maxFrame.TabIndex = 7;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(284, 103);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(16, 13);
            this.label19.TabIndex = 32;
            this.label19.Text = "to";
            // 
            // text_outputPath
            // 
            this.text_outputPath.Location = new System.Drawing.Point(109, 39);
            this.text_outputPath.Name = "text_outputPath";
            this.text_outputPath.Size = new System.Drawing.Size(425, 20);
            this.text_outputPath.TabIndex = 2;
            // 
            // button_outputPath
            // 
            this.button_outputPath.Location = new System.Drawing.Point(540, 39);
            this.button_outputPath.Name = "button_outputPath";
            this.button_outputPath.Size = new System.Drawing.Size(75, 23);
            this.button_outputPath.TabIndex = 3;
            this.button_outputPath.Text = "Browse";
            this.button_outputPath.UseVisualStyleBackColor = true;
            this.button_outputPath.Click += new System.EventHandler(this.button_outputPath_Click);
            // 
            // text_gpsPath
            // 
            this.text_gpsPath.Location = new System.Drawing.Point(109, 67);
            this.text_gpsPath.Name = "text_gpsPath";
            this.text_gpsPath.Size = new System.Drawing.Size(425, 20);
            this.text_gpsPath.TabIndex = 4;
            // 
            // button_gpsPath
            // 
            this.button_gpsPath.Location = new System.Drawing.Point(540, 66);
            this.button_gpsPath.Name = "button_gpsPath";
            this.button_gpsPath.Size = new System.Drawing.Size(75, 23);
            this.button_gpsPath.TabIndex = 5;
            this.button_gpsPath.Text = "Browse";
            this.button_gpsPath.UseVisualStyleBackColor = true;
            this.button_gpsPath.Click += new System.EventHandler(this.button_gpsPath_Click);
            // 
            // text_imageSizeWidth
            // 
            this.text_imageSizeWidth.Location = new System.Drawing.Point(157, 134);
            this.text_imageSizeWidth.Name = "text_imageSizeWidth";
            this.text_imageSizeWidth.Size = new System.Drawing.Size(100, 20);
            this.text_imageSizeWidth.TabIndex = 8;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(288, 137);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(14, 13);
            this.label20.TabIndex = 34;
            this.label20.Text = "X";
            // 
            // text_imageSizeHeight
            // 
            this.text_imageSizeHeight.Location = new System.Drawing.Point(310, 134);
            this.text_imageSizeHeight.Name = "text_imageSizeHeight";
            this.text_imageSizeHeight.Size = new System.Drawing.Size(100, 20);
            this.text_imageSizeHeight.TabIndex = 9;
            // 
            // combo_renderingType
            // 
            this.combo_renderingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_renderingType.FormattingEnabled = true;
            this.combo_renderingType.Location = new System.Drawing.Point(691, 99);
            this.combo_renderingType.Name = "combo_renderingType";
            this.combo_renderingType.Size = new System.Drawing.Size(121, 21);
            this.combo_renderingType.TabIndex = 16;
            this.combo_renderingType.SelectedIndexChanged += new System.EventHandler(this.combo_renderingType_SelectedIndexChanged);
            // 
            // combo_colorProcessingType
            // 
            this.combo_colorProcessingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_colorProcessingType.FormattingEnabled = true;
            this.combo_colorProcessingType.Location = new System.Drawing.Point(157, 164);
            this.combo_colorProcessingType.Name = "combo_colorProcessingType";
            this.combo_colorProcessingType.Size = new System.Drawing.Size(121, 21);
            this.combo_colorProcessingType.TabIndex = 10;
            // 
            // text_blendingWidth
            // 
            this.text_blendingWidth.Location = new System.Drawing.Point(157, 194);
            this.text_blendingWidth.Name = "text_blendingWidth";
            this.text_blendingWidth.Size = new System.Drawing.Size(100, 20);
            this.text_blendingWidth.TabIndex = 11;
            // 
            // text_falloffValue
            // 
            this.text_falloffValue.Location = new System.Drawing.Point(144, 243);
            this.text_falloffValue.Name = "text_falloffValue";
            this.text_falloffValue.Size = new System.Drawing.Size(61, 20);
            this.text_falloffValue.TabIndex = 13;
            // 
            // check_enableFalloff
            // 
            this.check_enableFalloff.AutoSize = true;
            this.check_enableFalloff.Location = new System.Drawing.Point(7, 224);
            this.check_enableFalloff.Name = "check_enableFalloff";
            this.check_enableFalloff.Size = new System.Drawing.Size(141, 17);
            this.check_enableFalloff.TabIndex = 12;
            this.check_enableFalloff.Text = "Enable Falloff Correction";
            this.check_enableFalloff.UseVisualStyleBackColor = true;
            this.check_enableFalloff.CheckedChanged += new System.EventHandler(this.check_enableFalloff_CheckedChanged);
            // 
            // check_enableSwRendering
            // 
            this.check_enableSwRendering.AutoSize = true;
            this.check_enableSwRendering.Location = new System.Drawing.Point(7, 312);
            this.check_enableSwRendering.Name = "check_enableSwRendering";
            this.check_enableSwRendering.Size = new System.Drawing.Size(156, 17);
            this.check_enableSwRendering.TabIndex = 14;
            this.check_enableSwRendering.Text = "Enable Software Rendering";
            this.check_enableSwRendering.UseVisualStyleBackColor = true;
            // 
            // check_enableStabilization
            // 
            this.check_enableStabilization.AutoSize = true;
            this.check_enableStabilization.Location = new System.Drawing.Point(526, 224);
            this.check_enableStabilization.Name = "check_enableStabilization";
            this.check_enableStabilization.Size = new System.Drawing.Size(118, 17);
            this.check_enableStabilization.TabIndex = 21;
            this.check_enableStabilization.Text = "Enable Stabilization";
            this.check_enableStabilization.UseVisualStyleBackColor = true;
            this.check_enableStabilization.CheckedChanged += new System.EventHandler(this.check_enableStabilization_CheckedChanged);
            // 
            // text_stabilization_numTemplates
            // 
            this.text_stabilization_numTemplates.Location = new System.Drawing.Point(780, 244);
            this.text_stabilization_numTemplates.Name = "text_stabilization_numTemplates";
            this.text_stabilization_numTemplates.Size = new System.Drawing.Size(64, 20);
            this.text_stabilization_numTemplates.TabIndex = 22;
            // 
            // text_stabilization_maxRegion
            // 
            this.text_stabilization_maxRegion.Location = new System.Drawing.Point(780, 268);
            this.text_stabilization_maxRegion.Name = "text_stabilization_maxRegion";
            this.text_stabilization_maxRegion.Size = new System.Drawing.Size(64, 20);
            this.text_stabilization_maxRegion.TabIndex = 23;
            // 
            // text_stabilization_decayRate
            // 
            this.text_stabilization_decayRate.Location = new System.Drawing.Point(780, 292);
            this.text_stabilization_decayRate.Name = "text_stabilization_decayRate";
            this.text_stabilization_decayRate.Size = new System.Drawing.Size(64, 20);
            this.text_stabilization_decayRate.TabIndex = 24;
            // 
            // text_fov
            // 
            this.text_fov.Location = new System.Drawing.Point(691, 122);
            this.text_fov.Name = "text_fov";
            this.text_fov.Size = new System.Drawing.Size(100, 20);
            this.text_fov.TabIndex = 17;
            // 
            // text_rotAngleX
            // 
            this.text_rotAngleX.Location = new System.Drawing.Point(691, 145);
            this.text_rotAngleX.Name = "text_rotAngleX";
            this.text_rotAngleX.Size = new System.Drawing.Size(61, 20);
            this.text_rotAngleX.TabIndex = 18;
            // 
            // button_quit
            // 
            this.button_quit.Location = new System.Drawing.Point(760, 336);
            this.button_quit.Name = "button_quit";
            this.button_quit.Size = new System.Drawing.Size(103, 23);
            this.button_quit.TabIndex = 28;
            this.button_quit.Text = "Quit";
            this.button_quit.UseVisualStyleBackColor = true;
            this.button_quit.Click += new System.EventHandler(this.button_quit_Click);
            // 
            // button_reset
            // 
            this.button_reset.Location = new System.Drawing.Point(651, 336);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(103, 23);
            this.button_reset.TabIndex = 27;
            this.button_reset.Text = "Reset Dialog";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // button_process
            // 
            this.button_process.Location = new System.Drawing.Point(542, 336);
            this.button_process.Name = "button_process";
            this.button_process.Size = new System.Drawing.Size(103, 23);
            this.button_process.TabIndex = 26;
            this.button_process.Text = "Process";
            this.button_process.UseVisualStyleBackColor = true;
            this.button_process.Click += new System.EventHandler(this.button_process_Click);
            // 
            // text_rotAngleY
            // 
            this.text_rotAngleY.Location = new System.Drawing.Point(691, 169);
            this.text_rotAngleY.Name = "text_rotAngleY";
            this.text_rotAngleY.Size = new System.Drawing.Size(61, 20);
            this.text_rotAngleY.TabIndex = 19;
            // 
            // text_rotAngleZ
            // 
            this.text_rotAngleZ.Location = new System.Drawing.Point(691, 192);
            this.text_rotAngleZ.Name = "text_rotAngleZ";
            this.text_rotAngleZ.Size = new System.Drawing.Size(61, 20);
            this.text_rotAngleZ.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(666, 149);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 13);
            this.label7.TabIndex = 41;
            this.label7.Text = "X:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(666, 173);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 13);
            this.label11.TabIndex = 42;
            this.label11.Text = "Y:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(666, 196);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 13);
            this.label12.TabIndex = 43;
            this.label12.Text = "Z:";
            // 
            // radio_saveJpeg
            // 
            this.radio_saveJpeg.AutoSize = true;
            this.radio_saveJpeg.Location = new System.Drawing.Point(6, 19);
            this.radio_saveJpeg.Name = "radio_saveJpeg";
            this.radio_saveJpeg.Size = new System.Drawing.Size(52, 17);
            this.radio_saveJpeg.TabIndex = 1;
            this.radio_saveJpeg.TabStop = true;
            this.radio_saveJpeg.Text = "JPEG";
            this.radio_saveJpeg.UseVisualStyleBackColor = true;
            // 
            // radio_saveBMP
            // 
            this.radio_saveBMP.AutoSize = true;
            this.radio_saveBMP.Location = new System.Drawing.Point(6, 42);
            this.radio_saveBMP.Name = "radio_saveBMP";
            this.radio_saveBMP.Size = new System.Drawing.Size(48, 17);
            this.radio_saveBMP.TabIndex = 0;
            this.radio_saveBMP.TabStop = true;
            this.radio_saveBMP.Text = "BMP";
            this.radio_saveBMP.UseVisualStyleBackColor = true;
            // 
            // group_saveFormat
            // 
            this.group_saveFormat.Controls.Add(this.radio_saveTIFF);
            this.group_saveFormat.Controls.Add(this.radio_savePNG);
            this.group_saveFormat.Controls.Add(this.radio_saveJpeg);
            this.group_saveFormat.Controls.Add(this.radio_saveBMP);
            this.group_saveFormat.Location = new System.Drawing.Point(691, 11);
            this.group_saveFormat.Name = "group_saveFormat";
            this.group_saveFormat.Size = new System.Drawing.Size(165, 64);
            this.group_saveFormat.TabIndex = 15;
            this.group_saveFormat.TabStop = false;
            this.group_saveFormat.Text = "Save Files As";
            // 
            // radio_saveTIFF
            // 
            this.radio_saveTIFF.AutoSize = true;
            this.radio_saveTIFF.Location = new System.Drawing.Point(80, 41);
            this.radio_saveTIFF.Name = "radio_saveTIFF";
            this.radio_saveTIFF.Size = new System.Drawing.Size(47, 17);
            this.radio_saveTIFF.TabIndex = 3;
            this.radio_saveTIFF.TabStop = true;
            this.radio_saveTIFF.Text = "TIFF";
            this.radio_saveTIFF.UseVisualStyleBackColor = true;
            // 
            // radio_savePNG
            // 
            this.radio_savePNG.AutoSize = true;
            this.radio_savePNG.Location = new System.Drawing.Point(80, 19);
            this.radio_savePNG.Name = "radio_savePNG";
            this.radio_savePNG.Size = new System.Drawing.Size(48, 17);
            this.radio_savePNG.TabIndex = 2;
            this.radio_savePNG.TabStop = true;
            this.radio_savePNG.Text = "PNG";
            this.radio_savePNG.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(7, 336);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(200, 23);
            this.progressBar.TabIndex = 47;
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.Location = new System.Drawing.Point(213, 341);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(72, 13);
            this.label_status.TabIndex = 48;
            this.label_status.Text = "Status Output";
            // 
            // button_abort
            // 
            this.button_abort.Location = new System.Drawing.Point(433, 336);
            this.button_abort.Name = "button_abort";
            this.button_abort.Size = new System.Drawing.Size(103, 23);
            this.button_abort.TabIndex = 25;
            this.button_abort.Text = "Abort";
            this.button_abort.UseVisualStyleBackColor = true;
            this.button_abort.Click += new System.EventHandler(this.button_abort_Click);
            // 
            // check_enableAntiAliasing
            // 
            this.check_enableAntiAliasing.AutoSize = true;
            this.check_enableAntiAliasing.Location = new System.Drawing.Point(7, 268);
            this.check_enableAntiAliasing.Name = "check_enableAntiAliasing";
            this.check_enableAntiAliasing.Size = new System.Drawing.Size(119, 17);
            this.check_enableAntiAliasing.TabIndex = 49;
            this.check_enableAntiAliasing.Text = "Enable Anti-Aliasing";
            this.check_enableAntiAliasing.UseVisualStyleBackColor = true;
            // 
            // check_enableSharpening
            // 
            this.check_enableSharpening.AutoSize = true;
            this.check_enableSharpening.Location = new System.Drawing.Point(7, 290);
            this.check_enableSharpening.Name = "check_enableSharpening";
            this.check_enableSharpening.Size = new System.Drawing.Size(116, 17);
            this.check_enableSharpening.TabIndex = 50;
            this.check_enableSharpening.Text = "Enable Sharpening";
            this.check_enableSharpening.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(260, 137);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(24, 13);
            this.label13.TabIndex = 51;
            this.label13.Text = "(W)";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(413, 137);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(21, 13);
            this.label21.TabIndex = 52;
            this.label21.Text = "(H)";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(261, 197);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(18, 13);
            this.label22.TabIndex = 53;
            this.label22.Text = "px";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 367);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.check_enableSharpening);
            this.Controls.Add(this.check_enableAntiAliasing);
            this.Controls.Add(this.button_abort);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.group_saveFormat);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.text_rotAngleZ);
            this.Controls.Add(this.text_rotAngleY);
            this.Controls.Add(this.button_process);
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.button_quit);
            this.Controls.Add(this.text_rotAngleX);
            this.Controls.Add(this.text_fov);
            this.Controls.Add(this.text_stabilization_decayRate);
            this.Controls.Add(this.text_stabilization_maxRegion);
            this.Controls.Add(this.text_stabilization_numTemplates);
            this.Controls.Add(this.check_enableStabilization);
            this.Controls.Add(this.check_enableSwRendering);
            this.Controls.Add(this.check_enableFalloff);
            this.Controls.Add(this.text_falloffValue);
            this.Controls.Add(this.text_blendingWidth);
            this.Controls.Add(this.combo_colorProcessingType);
            this.Controls.Add(this.combo_renderingType);
            this.Controls.Add(this.text_imageSizeHeight);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.text_imageSizeWidth);
            this.Controls.Add(this.button_gpsPath);
            this.Controls.Add(this.text_gpsPath);
            this.Controls.Add(this.button_outputPath);
            this.Controls.Add(this.text_outputPath);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.combo_maxFrame);
            this.Controls.Add(this.combo_minFrame);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_inputFilename);
            this.Controls.Add(this.text_inputFilename);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Ladybug Process Stream File";
            this.group_saveFormat.ResumeLayout(false);
            this.group_saveFormat.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox text_inputFilename;
        private System.Windows.Forms.Button button_inputFilename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox combo_minFrame;
        private System.Windows.Forms.ComboBox combo_maxFrame;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox text_outputPath;
        private System.Windows.Forms.Button button_outputPath;
        private System.Windows.Forms.TextBox text_gpsPath;
        private System.Windows.Forms.Button button_gpsPath;
        private System.Windows.Forms.TextBox text_imageSizeWidth;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox text_imageSizeHeight;
        private System.Windows.Forms.ComboBox combo_renderingType;
        private System.Windows.Forms.ComboBox combo_colorProcessingType;
        private System.Windows.Forms.TextBox text_blendingWidth;
        private System.Windows.Forms.TextBox text_falloffValue;
        private System.Windows.Forms.CheckBox check_enableFalloff;
        private System.Windows.Forms.CheckBox check_enableSwRendering;
        private System.Windows.Forms.CheckBox check_enableStabilization;
        private System.Windows.Forms.TextBox text_stabilization_numTemplates;
        private System.Windows.Forms.TextBox text_stabilization_maxRegion;
        private System.Windows.Forms.TextBox text_stabilization_decayRate;
        private System.Windows.Forms.TextBox text_fov;
        private System.Windows.Forms.TextBox text_rotAngleX;
        private System.Windows.Forms.Button button_quit;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.Button button_process;
        private System.Windows.Forms.TextBox text_rotAngleY;
        private System.Windows.Forms.TextBox text_rotAngleZ;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton radio_saveJpeg;
        private System.Windows.Forms.RadioButton radio_saveBMP;
        private System.Windows.Forms.GroupBox group_saveFormat;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Button button_abort;
       private System.Windows.Forms.RadioButton radio_saveTIFF;
       private System.Windows.Forms.RadioButton radio_savePNG;
       private System.Windows.Forms.CheckBox check_enableAntiAliasing;
       private System.Windows.Forms.CheckBox check_enableSharpening;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;

    }
}

