namespace CarDVR
{
    partial class settingsForm
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
			this.startWithWindows = new System.Windows.Forms.CheckBox();
			this.autostartRecording = new System.Windows.Forms.CheckBox();
			this.labelVideosource = new System.Windows.Forms.Label();
			this.videoSource = new System.Windows.Forms.ComboBox();
			this.groupBoxGps = new System.Windows.Forms.GroupBox();
			this.labelBaudRate = new System.Windows.Forms.Label();
			this.serialPortBaudRate = new System.Windows.Forms.ComboBox();
			this.serialPortName = new System.Windows.Forms.ComboBox();
			this.enableGps = new System.Windows.Forms.CheckBox();
			this.buttonOk = new System.Windows.Forms.Button();
			this.labelMinutesOnEachFile = new System.Windows.Forms.Label();
			this.aviDuration = new System.Windows.Forms.NumericUpDown();
			this.amountOfFiles = new System.Windows.Forms.NumericUpDown();
			this.labelStoreFiles = new System.Windows.Forms.Label();
			this.labelPath = new System.Windows.Forms.Label();
			this.textBoxPath = new System.Windows.Forms.TextBox();
			this.buttonBrowse = new System.Windows.Forms.Button();
			this.startMinimized = new System.Windows.Forms.CheckBox();
			this.comboResolution = new System.Windows.Forms.ComboBox();
			this.labelResolution = new System.Windows.Forms.Label();
			this.groupBoxGps.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.aviDuration)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.amountOfFiles)).BeginInit();
			this.SuspendLayout();
			// 
			// startWithWindows
			// 
			this.startWithWindows.AutoSize = true;
			this.startWithWindows.Location = new System.Drawing.Point(13, 5);
			this.startWithWindows.Name = "startWithWindows";
			this.startWithWindows.Size = new System.Drawing.Size(112, 17);
			this.startWithWindows.TabIndex = 0;
			this.startWithWindows.Text = "start with windows";
			this.startWithWindows.UseVisualStyleBackColor = true;
			// 
			// autostartRecording
			// 
			this.autostartRecording.AutoSize = true;
			this.autostartRecording.Location = new System.Drawing.Point(13, 28);
			this.autostartRecording.Name = "autostartRecording";
			this.autostartRecording.Size = new System.Drawing.Size(114, 17);
			this.autostartRecording.TabIndex = 1;
			this.autostartRecording.Text = "autostart recording";
			this.autostartRecording.UseVisualStyleBackColor = true;
			// 
			// labelVideosource
			// 
			this.labelVideosource.AutoSize = true;
			this.labelVideosource.Location = new System.Drawing.Point(175, 7);
			this.labelVideosource.Name = "labelVideosource";
			this.labelVideosource.Size = new System.Drawing.Size(68, 13);
			this.labelVideosource.TabIndex = 2;
			this.labelVideosource.Text = "video source";
			// 
			// videoSource
			// 
			this.videoSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.videoSource.FormattingEnabled = true;
			this.videoSource.Location = new System.Drawing.Point(178, 24);
			this.videoSource.Name = "videoSource";
			this.videoSource.Size = new System.Drawing.Size(140, 21);
			this.videoSource.TabIndex = 3;
			// 
			// groupBoxGps
			// 
			this.groupBoxGps.Controls.Add(this.labelBaudRate);
			this.groupBoxGps.Controls.Add(this.serialPortBaudRate);
			this.groupBoxGps.Controls.Add(this.serialPortName);
			this.groupBoxGps.Controls.Add(this.enableGps);
			this.groupBoxGps.Location = new System.Drawing.Point(178, 105);
			this.groupBoxGps.Name = "groupBoxGps";
			this.groupBoxGps.Size = new System.Drawing.Size(206, 96);
			this.groupBoxGps.TabIndex = 4;
			this.groupBoxGps.TabStop = false;
			this.groupBoxGps.Text = "gps";
			// 
			// labelBaudRate
			// 
			this.labelBaudRate.AutoSize = true;
			this.labelBaudRate.Location = new System.Drawing.Point(7, 51);
			this.labelBaudRate.Name = "labelBaudRate";
			this.labelBaudRate.Size = new System.Drawing.Size(52, 13);
			this.labelBaudRate.TabIndex = 9;
			this.labelBaudRate.Text = "baud rate";
			// 
			// serialPortBaudRate
			// 
			this.serialPortBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.serialPortBaudRate.FormattingEnabled = true;
			this.serialPortBaudRate.Location = new System.Drawing.Point(10, 67);
			this.serialPortBaudRate.Name = "serialPortBaudRate";
			this.serialPortBaudRate.Size = new System.Drawing.Size(95, 21);
			this.serialPortBaudRate.TabIndex = 3;
			// 
			// serialPortName
			// 
			this.serialPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.serialPortName.FormattingEnabled = true;
			this.serialPortName.Location = new System.Drawing.Point(10, 17);
			this.serialPortName.Name = "serialPortName";
			this.serialPortName.Size = new System.Drawing.Size(95, 21);
			this.serialPortName.TabIndex = 2;
			// 
			// enableGps
			// 
			this.enableGps.AutoSize = true;
			this.enableGps.Location = new System.Drawing.Point(142, 19);
			this.enableGps.Name = "enableGps";
			this.enableGps.Size = new System.Drawing.Size(58, 17);
			this.enableGps.TabIndex = 1;
			this.enableGps.Text = "enable";
			this.enableGps.UseVisualStyleBackColor = true;
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonOk.Location = new System.Drawing.Point(13, 271);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(90, 62);
			this.buttonOk.TabIndex = 5;
			this.buttonOk.Text = "OK";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// labelMinutesOnEachFile
			// 
			this.labelMinutesOnEachFile.AutoSize = true;
			this.labelMinutesOnEachFile.Location = new System.Drawing.Point(12, 105);
			this.labelMinutesOnEachFile.Name = "labelMinutesOnEachFile";
			this.labelMinutesOnEachFile.Size = new System.Drawing.Size(125, 13);
			this.labelMinutesOnEachFile.TabIndex = 6;
			this.labelMinutesOnEachFile.Text = "duration of each file (min)";
			// 
			// aviDuration
			// 
			this.aviDuration.Location = new System.Drawing.Point(15, 122);
			this.aviDuration.Name = "aviDuration";
			this.aviDuration.Size = new System.Drawing.Size(66, 20);
			this.aviDuration.TabIndex = 7;
			this.aviDuration.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// amountOfFiles
			// 
			this.amountOfFiles.Location = new System.Drawing.Point(13, 178);
			this.amountOfFiles.Name = "amountOfFiles";
			this.amountOfFiles.Size = new System.Drawing.Size(66, 20);
			this.amountOfFiles.TabIndex = 9;
			this.amountOfFiles.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// labelStoreFiles
			// 
			this.labelStoreFiles.AutoSize = true;
			this.labelStoreFiles.Location = new System.Drawing.Point(10, 161);
			this.labelStoreFiles.Name = "labelStoreFiles";
			this.labelStoreFiles.Size = new System.Drawing.Size(112, 13);
			this.labelStoreFiles.TabIndex = 8;
			this.labelStoreFiles.Text = "files amount in archive";
			// 
			// labelPath
			// 
			this.labelPath.AutoSize = true;
			this.labelPath.Location = new System.Drawing.Point(15, 223);
			this.labelPath.Name = "labelPath";
			this.labelPath.Size = new System.Drawing.Size(128, 13);
			this.labelPath.TabIndex = 10;
			this.labelPath.Text = "path to store video files to";
			// 
			// textBoxPath
			// 
			this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxPath.Location = new System.Drawing.Point(15, 239);
			this.textBoxPath.Name = "textBoxPath";
			this.textBoxPath.Size = new System.Drawing.Size(338, 20);
			this.textBoxPath.TabIndex = 11;
			// 
			// buttonBrowse
			// 
			this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBrowse.Location = new System.Drawing.Point(359, 236);
			this.buttonBrowse.Name = "buttonBrowse";
			this.buttonBrowse.Size = new System.Drawing.Size(25, 23);
			this.buttonBrowse.TabIndex = 12;
			this.buttonBrowse.Text = "...";
			this.buttonBrowse.UseVisualStyleBackColor = true;
			this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
			// 
			// startMinimized
			// 
			this.startMinimized.AutoSize = true;
			this.startMinimized.Location = new System.Drawing.Point(13, 51);
			this.startMinimized.Name = "startMinimized";
			this.startMinimized.Size = new System.Drawing.Size(94, 17);
			this.startMinimized.TabIndex = 13;
			this.startMinimized.Text = "start minimized";
			this.startMinimized.UseVisualStyleBackColor = true;
			// 
			// comboResolution
			// 
			this.comboResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboResolution.FormattingEnabled = true;
			this.comboResolution.Location = new System.Drawing.Point(178, 68);
			this.comboResolution.Name = "comboResolution";
			this.comboResolution.Size = new System.Drawing.Size(140, 21);
			this.comboResolution.TabIndex = 15;
			this.comboResolution.DropDown += new System.EventHandler(this.comboResolution_DropDown);
			// 
			// labelResolution
			// 
			this.labelResolution.AutoSize = true;
			this.labelResolution.Location = new System.Drawing.Point(175, 51);
			this.labelResolution.Name = "labelResolution";
			this.labelResolution.Size = new System.Drawing.Size(52, 13);
			this.labelResolution.TabIndex = 14;
			this.labelResolution.Text = "resolution";
			// 
			// settingsForm
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(396, 345);
			this.Controls.Add(this.comboResolution);
			this.Controls.Add(this.labelResolution);
			this.Controls.Add(this.startMinimized);
			this.Controls.Add(this.buttonBrowse);
			this.Controls.Add(this.textBoxPath);
			this.Controls.Add(this.labelPath);
			this.Controls.Add(this.amountOfFiles);
			this.Controls.Add(this.labelStoreFiles);
			this.Controls.Add(this.aviDuration);
			this.Controls.Add(this.labelMinutesOnEachFile);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.groupBoxGps);
			this.Controls.Add(this.videoSource);
			this.Controls.Add(this.labelVideosource);
			this.Controls.Add(this.autostartRecording);
			this.Controls.Add(this.startWithWindows);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "settingsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Settings";
			this.groupBoxGps.ResumeLayout(false);
			this.groupBoxGps.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.aviDuration)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.amountOfFiles)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox startWithWindows;
        private System.Windows.Forms.CheckBox autostartRecording;
        private System.Windows.Forms.Label labelVideosource;
        private System.Windows.Forms.ComboBox videoSource;
        private System.Windows.Forms.GroupBox groupBoxGps;
		private System.Windows.Forms.CheckBox enableGps;
        private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.ComboBox serialPortBaudRate;
		private System.Windows.Forms.ComboBox serialPortName;
		private System.Windows.Forms.Label labelMinutesOnEachFile;
		private System.Windows.Forms.NumericUpDown aviDuration;
		private System.Windows.Forms.NumericUpDown amountOfFiles;
		private System.Windows.Forms.Label labelStoreFiles;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonBrowse;
		private System.Windows.Forms.Label labelBaudRate;
		private System.Windows.Forms.CheckBox startMinimized;
        private System.Windows.Forms.ComboBox comboResolution;
        private System.Windows.Forms.Label labelResolution;
    }
}