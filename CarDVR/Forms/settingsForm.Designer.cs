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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(settingsForm));
			this.startWithWindows = new System.Windows.Forms.CheckBox();
			this.autostartRecording = new System.Windows.Forms.CheckBox();
			this.labelVideosource = new System.Windows.Forms.Label();
			this.videoSource = new System.Windows.Forms.ComboBox();
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
			this.button1 = new System.Windows.Forms.Button();
			this.comboRotateAngle = new System.Windows.Forms.ComboBox();
			this.enableRotate = new System.Windows.Forms.CheckBox();
			this.labelDegrees = new System.Windows.Forms.Label();
			this.gbAutostart = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.outputRate = new System.Windows.Forms.ComboBox();
			this.labelDelayAutostart = new System.Windows.Forms.Label();
			this.delayBeforeStart = new System.Windows.Forms.NumericUpDown();
			this.groupVideoArchive = new System.Windows.Forms.GroupBox();
			this.comboLanguage = new System.Windows.Forms.ComboBox();
			this.labelLanguage = new System.Windows.Forms.Label();
			this.tabPageSettings = new System.Windows.Forms.TabControl();
			this.tabStartOptions = new System.Windows.Forms.TabPage();
			this.startWithFullWindowedVideo = new System.Windows.Forms.CheckBox();
			this.tabVideoSource = new System.Windows.Forms.TabPage();
			this.buttonSettings = new System.Windows.Forms.Button();
			this.tabCompression = new System.Windows.Forms.TabPage();
			this.labelSelectedCodec = new System.Windows.Forms.Label();
			this.labelSelectedCodecTitle = new System.Windows.Forms.Label();
			this.listCodecs = new System.Windows.Forms.ListBox();
			this.tabOutput = new System.Windows.Forms.TabPage();
			this.tabGps = new System.Windows.Forms.TabPage();
			this.tabLocalization = new System.Windows.Forms.TabPage();
			((System.ComponentModel.ISupportInitialize)(this.aviDuration)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.amountOfFiles)).BeginInit();
			this.gbAutostart.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.delayBeforeStart)).BeginInit();
			this.groupVideoArchive.SuspendLayout();
			this.tabPageSettings.SuspendLayout();
			this.tabStartOptions.SuspendLayout();
			this.tabVideoSource.SuspendLayout();
			this.tabCompression.SuspendLayout();
			this.tabOutput.SuspendLayout();
			this.tabGps.SuspendLayout();
			this.tabLocalization.SuspendLayout();
			this.SuspendLayout();
			// 
			// startWithWindows
			// 
			resources.ApplyResources(this.startWithWindows, "startWithWindows");
			this.startWithWindows.Name = "startWithWindows";
			this.startWithWindows.UseVisualStyleBackColor = true;
			// 
			// autostartRecording
			// 
			resources.ApplyResources(this.autostartRecording, "autostartRecording");
			this.autostartRecording.Name = "autostartRecording";
			this.autostartRecording.UseVisualStyleBackColor = true;
			// 
			// labelVideosource
			// 
			resources.ApplyResources(this.labelVideosource, "labelVideosource");
			this.labelVideosource.Name = "labelVideosource";
			// 
			// videoSource
			// 
			resources.ApplyResources(this.videoSource, "videoSource");
			this.videoSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.videoSource.FormattingEnabled = true;
			this.videoSource.Name = "videoSource";
			// 
			// labelBaudRate
			// 
			resources.ApplyResources(this.labelBaudRate, "labelBaudRate");
			this.labelBaudRate.Name = "labelBaudRate";
			// 
			// serialPortBaudRate
			// 
			resources.ApplyResources(this.serialPortBaudRate, "serialPortBaudRate");
			this.serialPortBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.serialPortBaudRate.FormattingEnabled = true;
			this.serialPortBaudRate.Name = "serialPortBaudRate";
			// 
			// serialPortName
			// 
			resources.ApplyResources(this.serialPortName, "serialPortName");
			this.serialPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.serialPortName.FormattingEnabled = true;
			this.serialPortName.Name = "serialPortName";
			// 
			// enableGps
			// 
			resources.ApplyResources(this.enableGps, "enableGps");
			this.enableGps.Name = "enableGps";
			this.enableGps.UseVisualStyleBackColor = true;
			// 
			// buttonOk
			// 
			resources.ApplyResources(this.buttonOk, "buttonOk");
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// labelMinutesOnEachFile
			// 
			resources.ApplyResources(this.labelMinutesOnEachFile, "labelMinutesOnEachFile");
			this.labelMinutesOnEachFile.Name = "labelMinutesOnEachFile";
			// 
			// aviDuration
			// 
			resources.ApplyResources(this.aviDuration, "aviDuration");
			this.aviDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.aviDuration.Name = "aviDuration";
			this.aviDuration.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// amountOfFiles
			// 
			resources.ApplyResources(this.amountOfFiles, "amountOfFiles");
			this.amountOfFiles.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.amountOfFiles.Name = "amountOfFiles";
			this.amountOfFiles.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// labelStoreFiles
			// 
			resources.ApplyResources(this.labelStoreFiles, "labelStoreFiles");
			this.labelStoreFiles.Name = "labelStoreFiles";
			// 
			// labelPath
			// 
			resources.ApplyResources(this.labelPath, "labelPath");
			this.labelPath.Name = "labelPath";
			// 
			// textBoxPath
			// 
			resources.ApplyResources(this.textBoxPath, "textBoxPath");
			this.textBoxPath.Name = "textBoxPath";
			// 
			// buttonBrowse
			// 
			resources.ApplyResources(this.buttonBrowse, "buttonBrowse");
			this.buttonBrowse.Name = "buttonBrowse";
			this.buttonBrowse.UseVisualStyleBackColor = true;
			this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
			// 
			// startMinimized
			// 
			resources.ApplyResources(this.startMinimized, "startMinimized");
			this.startMinimized.Name = "startMinimized";
			this.startMinimized.UseVisualStyleBackColor = true;
			// 
			// comboResolution
			// 
			resources.ApplyResources(this.comboResolution, "comboResolution");
			this.comboResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboResolution.FormattingEnabled = true;
			this.comboResolution.Name = "comboResolution";
			this.comboResolution.DropDown += new System.EventHandler(this.comboResolution_DropDown);
			// 
			// labelResolution
			// 
			resources.ApplyResources(this.labelResolution, "labelResolution");
			this.labelResolution.Name = "labelResolution";
			// 
			// button1
			// 
			resources.ApplyResources(this.button1, "button1");
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// comboRotateAngle
			// 
			resources.ApplyResources(this.comboRotateAngle, "comboRotateAngle");
			this.comboRotateAngle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboRotateAngle.FormattingEnabled = true;
			this.comboRotateAngle.Name = "comboRotateAngle";
			// 
			// enableRotate
			// 
			resources.ApplyResources(this.enableRotate, "enableRotate");
			this.enableRotate.Name = "enableRotate";
			this.enableRotate.UseVisualStyleBackColor = true;
			// 
			// labelDegrees
			// 
			resources.ApplyResources(this.labelDegrees, "labelDegrees");
			this.labelDegrees.Name = "labelDegrees";
			// 
			// gbAutostart
			// 
			resources.ApplyResources(this.gbAutostart, "gbAutostart");
			this.gbAutostart.Controls.Add(this.label2);
			this.gbAutostart.Controls.Add(this.label1);
			this.gbAutostart.Controls.Add(this.outputRate);
			this.gbAutostart.Controls.Add(this.labelDegrees);
			this.gbAutostart.Controls.Add(this.comboRotateAngle);
			this.gbAutostart.Controls.Add(this.enableRotate);
			this.gbAutostart.Name = "gbAutostart";
			this.gbAutostart.TabStop = false;
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// outputRate
			// 
			resources.ApplyResources(this.outputRate, "outputRate");
			this.outputRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.outputRate.FormattingEnabled = true;
			this.outputRate.Items.AddRange(new object[] {
            ((object)(resources.GetObject("outputRate.Items"))),
            ((object)(resources.GetObject("outputRate.Items1"))),
            ((object)(resources.GetObject("outputRate.Items2"))),
            ((object)(resources.GetObject("outputRate.Items3"))),
            ((object)(resources.GetObject("outputRate.Items4"))),
            ((object)(resources.GetObject("outputRate.Items5")))});
			this.outputRate.Name = "outputRate";
			// 
			// labelDelayAutostart
			// 
			resources.ApplyResources(this.labelDelayAutostart, "labelDelayAutostart");
			this.labelDelayAutostart.Name = "labelDelayAutostart";
			// 
			// delayBeforeStart
			// 
			resources.ApplyResources(this.delayBeforeStart, "delayBeforeStart");
			this.delayBeforeStart.Name = "delayBeforeStart";
			this.delayBeforeStart.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// groupVideoArchive
			// 
			resources.ApplyResources(this.groupVideoArchive, "groupVideoArchive");
			this.groupVideoArchive.Controls.Add(this.labelMinutesOnEachFile);
			this.groupVideoArchive.Controls.Add(this.aviDuration);
			this.groupVideoArchive.Controls.Add(this.labelStoreFiles);
			this.groupVideoArchive.Controls.Add(this.amountOfFiles);
			this.groupVideoArchive.Name = "groupVideoArchive";
			this.groupVideoArchive.TabStop = false;
			// 
			// comboLanguage
			// 
			resources.ApplyResources(this.comboLanguage, "comboLanguage");
			this.comboLanguage.DisplayMember = "English";
			this.comboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboLanguage.FormattingEnabled = true;
			this.comboLanguage.Items.AddRange(new object[] {
            resources.GetString("comboLanguage.Items"),
            resources.GetString("comboLanguage.Items1")});
			this.comboLanguage.Name = "comboLanguage";
			// 
			// labelLanguage
			// 
			resources.ApplyResources(this.labelLanguage, "labelLanguage");
			this.labelLanguage.Name = "labelLanguage";
			// 
			// tabPageSettings
			// 
			resources.ApplyResources(this.tabPageSettings, "tabPageSettings");
			this.tabPageSettings.Controls.Add(this.tabStartOptions);
			this.tabPageSettings.Controls.Add(this.tabVideoSource);
			this.tabPageSettings.Controls.Add(this.tabCompression);
			this.tabPageSettings.Controls.Add(this.tabOutput);
			this.tabPageSettings.Controls.Add(this.tabGps);
			this.tabPageSettings.Controls.Add(this.tabLocalization);
			this.tabPageSettings.Name = "tabPageSettings";
			this.tabPageSettings.SelectedIndex = 0;
			// 
			// tabStartOptions
			// 
			resources.ApplyResources(this.tabStartOptions, "tabStartOptions");
			this.tabStartOptions.Controls.Add(this.startWithFullWindowedVideo);
			this.tabStartOptions.Controls.Add(this.startWithWindows);
			this.tabStartOptions.Controls.Add(this.startMinimized);
			this.tabStartOptions.Controls.Add(this.autostartRecording);
			this.tabStartOptions.Controls.Add(this.labelDelayAutostart);
			this.tabStartOptions.Controls.Add(this.delayBeforeStart);
			this.tabStartOptions.Name = "tabStartOptions";
			this.tabStartOptions.UseVisualStyleBackColor = true;
			// 
			// startWithFullWindowedVideo
			// 
			resources.ApplyResources(this.startWithFullWindowedVideo, "startWithFullWindowedVideo");
			this.startWithFullWindowedVideo.Name = "startWithFullWindowedVideo";
			this.startWithFullWindowedVideo.UseVisualStyleBackColor = true;
			// 
			// tabVideoSource
			// 
			resources.ApplyResources(this.tabVideoSource, "tabVideoSource");
			this.tabVideoSource.Controls.Add(this.buttonSettings);
			this.tabVideoSource.Controls.Add(this.comboResolution);
			this.tabVideoSource.Controls.Add(this.labelVideosource);
			this.tabVideoSource.Controls.Add(this.videoSource);
			this.tabVideoSource.Controls.Add(this.labelResolution);
			this.tabVideoSource.Name = "tabVideoSource";
			this.tabVideoSource.UseVisualStyleBackColor = true;
			// 
			// buttonSettings
			// 
			resources.ApplyResources(this.buttonSettings, "buttonSettings");
			this.buttonSettings.Name = "buttonSettings";
			this.buttonSettings.UseVisualStyleBackColor = true;
			this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
			// 
			// tabCompression
			// 
			resources.ApplyResources(this.tabCompression, "tabCompression");
			this.tabCompression.Controls.Add(this.labelSelectedCodec);
			this.tabCompression.Controls.Add(this.labelSelectedCodecTitle);
			this.tabCompression.Controls.Add(this.listCodecs);
			this.tabCompression.Name = "tabCompression";
			this.tabCompression.UseVisualStyleBackColor = true;
			// 
			// labelSelectedCodec
			// 
			resources.ApplyResources(this.labelSelectedCodec, "labelSelectedCodec");
			this.labelSelectedCodec.Name = "labelSelectedCodec";
			// 
			// labelSelectedCodecTitle
			// 
			resources.ApplyResources(this.labelSelectedCodecTitle, "labelSelectedCodecTitle");
			this.labelSelectedCodecTitle.Name = "labelSelectedCodecTitle";
			// 
			// listCodecs
			// 
			resources.ApplyResources(this.listCodecs, "listCodecs");
			this.listCodecs.FormattingEnabled = true;
			this.listCodecs.Name = "listCodecs";
			this.listCodecs.SelectedValueChanged += new System.EventHandler(this.listCodecs_SelectedValueChanged);
			// 
			// tabOutput
			// 
			resources.ApplyResources(this.tabOutput, "tabOutput");
			this.tabOutput.Controls.Add(this.groupVideoArchive);
			this.tabOutput.Controls.Add(this.gbAutostart);
			this.tabOutput.Controls.Add(this.buttonBrowse);
			this.tabOutput.Controls.Add(this.labelPath);
			this.tabOutput.Controls.Add(this.textBoxPath);
			this.tabOutput.Name = "tabOutput";
			this.tabOutput.UseVisualStyleBackColor = true;
			// 
			// tabGps
			// 
			resources.ApplyResources(this.tabGps, "tabGps");
			this.tabGps.Controls.Add(this.labelBaudRate);
			this.tabGps.Controls.Add(this.serialPortName);
			this.tabGps.Controls.Add(this.serialPortBaudRate);
			this.tabGps.Controls.Add(this.enableGps);
			this.tabGps.Name = "tabGps";
			this.tabGps.UseVisualStyleBackColor = true;
			// 
			// tabLocalization
			// 
			resources.ApplyResources(this.tabLocalization, "tabLocalization");
			this.tabLocalization.Controls.Add(this.comboLanguage);
			this.tabLocalization.Controls.Add(this.labelLanguage);
			this.tabLocalization.Name = "tabLocalization";
			this.tabLocalization.UseVisualStyleBackColor = true;
			// 
			// settingsForm
			// 
			this.AcceptButton = this.buttonOk;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabPageSettings);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.buttonOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "settingsForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.settingsForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.aviDuration)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.amountOfFiles)).EndInit();
			this.gbAutostart.ResumeLayout(false);
			this.gbAutostart.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.delayBeforeStart)).EndInit();
			this.groupVideoArchive.ResumeLayout(false);
			this.groupVideoArchive.PerformLayout();
			this.tabPageSettings.ResumeLayout(false);
			this.tabStartOptions.ResumeLayout(false);
			this.tabStartOptions.PerformLayout();
			this.tabVideoSource.ResumeLayout(false);
			this.tabVideoSource.PerformLayout();
			this.tabCompression.ResumeLayout(false);
			this.tabCompression.PerformLayout();
			this.tabOutput.ResumeLayout(false);
			this.tabOutput.PerformLayout();
			this.tabGps.ResumeLayout(false);
			this.tabGps.PerformLayout();
			this.tabLocalization.ResumeLayout(false);
			this.tabLocalization.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox startWithWindows;
        private System.Windows.Forms.CheckBox autostartRecording;
        private System.Windows.Forms.Label labelVideosource;
		private System.Windows.Forms.ComboBox videoSource;
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
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox comboRotateAngle;
		private System.Windows.Forms.CheckBox enableRotate;
		private System.Windows.Forms.Label labelDegrees;
        private System.Windows.Forms.GroupBox gbAutostart;
        private System.Windows.Forms.Label labelDelayAutostart;
        private System.Windows.Forms.NumericUpDown delayBeforeStart;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox outputRate;
		private System.Windows.Forms.GroupBox groupVideoArchive;
		private System.Windows.Forms.ComboBox comboLanguage;
		private System.Windows.Forms.Label labelLanguage;
		private System.Windows.Forms.TabControl tabPageSettings;
		private System.Windows.Forms.TabPage tabStartOptions;
		private System.Windows.Forms.TabPage tabVideoSource;
		private System.Windows.Forms.TabPage tabOutput;
		private System.Windows.Forms.TabPage tabGps;
		private System.Windows.Forms.TabPage tabLocalization;
		private System.Windows.Forms.TabPage tabCompression;
		private System.Windows.Forms.ListBox listCodecs;
		private System.Windows.Forms.Label labelSelectedCodecTitle;
		private System.Windows.Forms.Label labelSelectedCodec;
		private System.Windows.Forms.CheckBox startWithFullWindowedVideo;
		private System.Windows.Forms.Button buttonSettings;
    }
}