namespace LadybugCSharpEx
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
            this.streamBtn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.camSelectBtn = new System.Windows.Forms.Button();
            this.camCtrlBtn = new System.Windows.Forms.Button();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.gpsLabel = new System.Windows.Forms.Label();
            this.viewTypeCombo = new System.Windows.Forms.ComboBox();
            this.colorProcCombo = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.serialLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openStreamFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // streamBtn
            // 
            this.streamBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.streamBtn.Location = new System.Drawing.Point(9, 6);
            this.streamBtn.Name = "streamBtn";
            this.streamBtn.Size = new System.Drawing.Size(98, 30);
            this.streamBtn.TabIndex = 1;
            this.streamBtn.Text = "Open stream file";
            this.streamBtn.UseVisualStyleBackColor = true;
            this.streamBtn.Click += new System.EventHandler(this.streamBtn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(11, 75);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(985, 475);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // camSelectBtn
            // 
            this.camSelectBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.camSelectBtn.Location = new System.Drawing.Point(113, 6);
            this.camSelectBtn.Name = "camSelectBtn";
            this.camSelectBtn.Size = new System.Drawing.Size(91, 30);
            this.camSelectBtn.TabIndex = 4;
            this.camSelectBtn.Text = "Select camera";
            this.camSelectBtn.UseVisualStyleBackColor = true;
            this.camSelectBtn.Click += new System.EventHandler(this.selectBtn_Click);
            // 
            // camCtrlBtn
            // 
            this.camCtrlBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.camCtrlBtn.Location = new System.Drawing.Point(210, 6);
            this.camCtrlBtn.Name = "camCtrlBtn";
            this.camCtrlBtn.Size = new System.Drawing.Size(90, 30);
            this.camCtrlBtn.TabIndex = 6;
            this.camCtrlBtn.Text = "Camera control";
            this.camCtrlBtn.UseVisualStyleBackColor = true;
            this.camCtrlBtn.Visible = false;
            this.camCtrlBtn.Click += new System.EventHandler(this.camCtlBtn_Click);
            // 
            // fpsLabel
            // 
            this.fpsLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.fpsLabel.AutoSize = true;
            this.fpsLabel.Location = new System.Drawing.Point(703, 14);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(94, 13);
            this.fpsLabel.TabIndex = 7;
            this.fpsLabel.Text = "FPS: 0.0 / second";
            this.fpsLabel.Visible = false;
            // 
            // gpsLabel
            // 
            this.gpsLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.gpsLabel.AutoSize = true;
            this.gpsLabel.Location = new System.Drawing.Point(803, 14);
            this.gpsLabel.Name = "gpsLabel";
            this.gpsLabel.Size = new System.Drawing.Size(58, 13);
            this.gpsLabel.TabIndex = 8;
            this.gpsLabel.Text = " GPS: N/A";
            // 
            // viewTypeCombo
            // 
            this.viewTypeCombo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.viewTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.viewTypeCombo.FormattingEnabled = true;
            this.viewTypeCombo.Items.AddRange(new object[] {
            "Panoramic",
            "Dome",
            "All Cameras",
            "Spherical"});
            this.viewTypeCombo.Location = new System.Drawing.Point(306, 10);
            this.viewTypeCombo.Name = "viewTypeCombo";
            this.viewTypeCombo.Size = new System.Drawing.Size(141, 21);
            this.viewTypeCombo.TabIndex = 9;
            this.viewTypeCombo.SelectedIndexChanged += new System.EventHandler(this.viewTypeCombo_SelectedIndexChanged);
            // 
            // colorProcCombo
            // 
            this.colorProcCombo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.colorProcCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorProcCombo.FormattingEnabled = true;
            this.colorProcCombo.Items.AddRange(new object[] {
            "Downsample4",
            "Downsample16",
            "Nearest Neighbor",
            "Edge Sensing",
            "HQ Linear",
            "HQ Linear (GPU)",
            "Directional Filter",
            "Weighted Directional Filter",
            "Rigorous",
            "Mono",
            "Disabled"});
            this.colorProcCombo.Location = new System.Drawing.Point(453, 10);
            this.colorProcCombo.Name = "colorProcCombo";
            this.colorProcCombo.Size = new System.Drawing.Size(141, 21);
            this.colorProcCombo.TabIndex = 10;
            this.colorProcCombo.SelectedIndexChanged += new System.EventHandler(this.colorProcCombo_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.serialLabel, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.gpsLabel, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.colorProcCombo, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.fpsLabel, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.viewTypeCombo, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.camSelectBtn, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.camCtrlBtn, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.streamBtn, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(985, 42);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // serialLabel
            // 
            this.serialLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.serialLabel.AutoSize = true;
            this.serialLabel.Location = new System.Drawing.Point(600, 14);
            this.serialLabel.Name = "serialLabel";
            this.serialLabel.Size = new System.Drawing.Size(97, 13);
            this.serialLabel.TabIndex = 11;
            this.serialLabel.Text = "Serial number: N/A";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openStreamFileToolStripMenuItem,
            this.selectCameraToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openStreamFileToolStripMenuItem
            // 
            this.openStreamFileToolStripMenuItem.Name = "openStreamFileToolStripMenuItem";
            this.openStreamFileToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.openStreamFileToolStripMenuItem.Text = "Open stream file...";
            this.openStreamFileToolStripMenuItem.Click += new System.EventHandler(this.openStreamFileToolStripMenuItem_Click);
            // 
            // selectCameraToolStripMenuItem
            // 
            this.selectCameraToolStripMenuItem.Name = "selectCameraToolStripMenuItem";
            this.selectCameraToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.selectCameraToolStripMenuItem.Text = "Select camera...";
            this.selectCameraToolStripMenuItem.Click += new System.EventHandler(this.selectCameraToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(167, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "LadybugCSharpEx";
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.form1_MouseWheel);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button streamBtn;
	private System.Windows.Forms.Button camSelectBtn;
	private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button camCtrlBtn;
	private System.Windows.Forms.Label fpsLabel;
	private System.Windows.Forms.Label gpsLabel;
       private System.Windows.Forms.ComboBox viewTypeCombo;
       private System.Windows.Forms.ComboBox colorProcCombo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openStreamFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectCameraToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label serialLabel;
    }
}

