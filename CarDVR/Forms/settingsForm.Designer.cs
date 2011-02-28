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
			this.dontDisplayWhenAppInactive = new System.Windows.Forms.CheckBox();
			this.startWithFullWindowedVideo = new System.Windows.Forms.CheckBox();
			this.tabVideoSource = new System.Windows.Forms.TabPage();
			this.camFps = new System.Windows.Forms.NumericUpDown();
			this.labelFrameRate = new System.Windows.Forms.Label();
			this.buttonSettings = new System.Windows.Forms.Button();
			this.tabCompression = new System.Windows.Forms.TabPage();
			this.labelSelectedCodec = new System.Windows.Forms.Label();
			this.labelSelectedCodecTitle = new System.Windows.Forms.Label();
			this.listCodecs = new System.Windows.Forms.ListBox();
			this.tabOutput = new System.Windows.Forms.TabPage();
			this.labelBackupHotkey = new System.Windows.Forms.Label();
			this.backupHotkey = new exscape.HotkeyControl();
			this.backupFilesAmount = new System.Windows.Forms.NumericUpDown();
			this.labelBackupFiles = new System.Windows.Forms.Label();
			this.buttonBrowseBackup = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.pathForBackup = new System.Windows.Forms.TextBox();
			this.tabGps = new System.Windows.Forms.TabPage();
			this.tabLocalization = new System.Windows.Forms.TabPage();
			this.hideMouse = new System.Windows.Forms.CheckBox();
			this.buttonBackColor = new System.Windows.Forms.Button();
			this.labelColors = new System.Windows.Forms.Label();
			this.buttonTextColor = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.labelTestColor = new System.Windows.Forms.Label();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			((System.ComponentModel.ISupportInitialize)(this.aviDuration)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.amountOfFiles)).BeginInit();
			this.gbAutostart.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.delayBeforeStart)).BeginInit();
			this.groupVideoArchive.SuspendLayout();
			this.tabPageSettings.SuspendLayout();
			this.tabStartOptions.SuspendLayout();
			this.tabVideoSource.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.camFps)).BeginInit();
			this.tabCompression.SuspendLayout();
			this.tabOutput.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.backupFilesAmount)).BeginInit();
			this.tabGps.SuspendLayout();
			this.tabLocalization.SuspendLayout();
			this.panel1.SuspendLayout();
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
			this.videoSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.videoSource.FormattingEnabled = true;
			resources.ApplyResources(this.videoSource, "videoSource");
			this.videoSource.Name = "videoSource";
			// 
			// labelBaudRate
			// 
			resources.ApplyResources(this.labelBaudRate, "labelBaudRate");
			this.labelBaudRate.Name = "labelBaudRate";
			// 
			// serialPortBaudRate
			// 
			this.serialPortBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.serialPortBaudRate.FormattingEnabled = true;
			resources.ApplyResources(this.serialPortBaudRate, "serialPortBaudRate");
			this.serialPortBaudRate.Name = "serialPortBaudRate";
			// 
			// serialPortName
			// 
			this.serialPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.serialPortName.FormattingEnabled = true;
			resources.ApplyResources(this.serialPortName, "serialPortName");
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
			this.comboResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboResolution.FormattingEnabled = true;
			resources.ApplyResources(this.comboResolution, "comboResolution");
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
			this.comboRotateAngle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboRotateAngle.FormattingEnabled = true;
			resources.ApplyResources(this.comboRotateAngle, "comboRotateAngle");
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
			this.gbAutostart.Controls.Add(this.label2);
			this.gbAutostart.Controls.Add(this.label1);
			this.gbAutostart.Controls.Add(this.outputRate);
			this.gbAutostart.Controls.Add(this.labelDegrees);
			this.gbAutostart.Controls.Add(this.comboRotateAngle);
			this.gbAutostart.Controls.Add(this.enableRotate);
			resources.ApplyResources(this.gbAutostart, "gbAutostart");
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
			this.outputRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.outputRate.FormattingEnabled = true;
			this.outputRate.Items.AddRange(new object[] {
            ((object)(resources.GetObject("outputRate.Items"))),
            ((object)(resources.GetObject("outputRate.Items1"))),
            ((object)(resources.GetObject("outputRate.Items2"))),
            ((object)(resources.GetObject("outputRate.Items3"))),
            ((object)(resources.GetObject("outputRate.Items4"))),
            ((object)(resources.GetObject("outputRate.Items5"))),
            ((object)(resources.GetObject("outputRate.Items6"))),
            ((object)(resources.GetObject("outputRate.Items7"))),
            ((object)(resources.GetObject("outputRate.Items8"))),
            ((object)(resources.GetObject("outputRate.Items9"))),
            ((object)(resources.GetObject("outputRate.Items10"))),
            ((object)(resources.GetObject("outputRate.Items11")))});
			resources.ApplyResources(this.outputRate, "outputRate");
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
			this.groupVideoArchive.Controls.Add(this.labelMinutesOnEachFile);
			this.groupVideoArchive.Controls.Add(this.aviDuration);
			this.groupVideoArchive.Controls.Add(this.labelStoreFiles);
			this.groupVideoArchive.Controls.Add(this.amountOfFiles);
			resources.ApplyResources(this.groupVideoArchive, "groupVideoArchive");
			this.groupVideoArchive.Name = "groupVideoArchive";
			this.groupVideoArchive.TabStop = false;
			// 
			// comboLanguage
			// 
			this.comboLanguage.DisplayMember = "English";
			this.comboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboLanguage.FormattingEnabled = true;
			this.comboLanguage.Items.AddRange(new object[] {
            resources.GetString("comboLanguage.Items"),
            resources.GetString("comboLanguage.Items1")});
			resources.ApplyResources(this.comboLanguage, "comboLanguage");
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
			this.tabStartOptions.Controls.Add(this.dontDisplayWhenAppInactive);
			this.tabStartOptions.Controls.Add(this.startWithFullWindowedVideo);
			this.tabStartOptions.Controls.Add(this.startWithWindows);
			this.tabStartOptions.Controls.Add(this.startMinimized);
			this.tabStartOptions.Controls.Add(this.autostartRecording);
			this.tabStartOptions.Controls.Add(this.labelDelayAutostart);
			this.tabStartOptions.Controls.Add(this.delayBeforeStart);
			resources.ApplyResources(this.tabStartOptions, "tabStartOptions");
			this.tabStartOptions.Name = "tabStartOptions";
			this.tabStartOptions.UseVisualStyleBackColor = true;
			// 
			// dontDisplayWhenAppInactive
			// 
			resources.ApplyResources(this.dontDisplayWhenAppInactive, "dontDisplayWhenAppInactive");
			this.dontDisplayWhenAppInactive.Name = "dontDisplayWhenAppInactive";
			this.dontDisplayWhenAppInactive.UseVisualStyleBackColor = true;
			// 
			// startWithFullWindowedVideo
			// 
			resources.ApplyResources(this.startWithFullWindowedVideo, "startWithFullWindowedVideo");
			this.startWithFullWindowedVideo.Name = "startWithFullWindowedVideo";
			this.startWithFullWindowedVideo.UseVisualStyleBackColor = true;
			// 
			// tabVideoSource
			// 
			this.tabVideoSource.Controls.Add(this.camFps);
			this.tabVideoSource.Controls.Add(this.labelFrameRate);
			this.tabVideoSource.Controls.Add(this.buttonSettings);
			this.tabVideoSource.Controls.Add(this.comboResolution);
			this.tabVideoSource.Controls.Add(this.labelVideosource);
			this.tabVideoSource.Controls.Add(this.videoSource);
			this.tabVideoSource.Controls.Add(this.labelResolution);
			resources.ApplyResources(this.tabVideoSource, "tabVideoSource");
			this.tabVideoSource.Name = "tabVideoSource";
			this.tabVideoSource.UseVisualStyleBackColor = true;
			// 
			// camFps
			// 
			resources.ApplyResources(this.camFps, "camFps");
			this.camFps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.camFps.Name = "camFps";
			this.camFps.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// labelFrameRate
			// 
			resources.ApplyResources(this.labelFrameRate, "labelFrameRate");
			this.labelFrameRate.Name = "labelFrameRate";
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
			this.tabCompression.Controls.Add(this.labelSelectedCodec);
			this.tabCompression.Controls.Add(this.labelSelectedCodecTitle);
			this.tabCompression.Controls.Add(this.listCodecs);
			resources.ApplyResources(this.tabCompression, "tabCompression");
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
			this.listCodecs.FormattingEnabled = true;
			resources.ApplyResources(this.listCodecs, "listCodecs");
			this.listCodecs.Name = "listCodecs";
			this.listCodecs.SelectedValueChanged += new System.EventHandler(this.listCodecs_SelectedValueChanged);
			// 
			// tabOutput
			// 
			this.tabOutput.Controls.Add(this.labelBackupHotkey);
			this.tabOutput.Controls.Add(this.backupHotkey);
			this.tabOutput.Controls.Add(this.backupFilesAmount);
			this.tabOutput.Controls.Add(this.labelBackupFiles);
			this.tabOutput.Controls.Add(this.buttonBrowseBackup);
			this.tabOutput.Controls.Add(this.label3);
			this.tabOutput.Controls.Add(this.pathForBackup);
			this.tabOutput.Controls.Add(this.groupVideoArchive);
			this.tabOutput.Controls.Add(this.gbAutostart);
			this.tabOutput.Controls.Add(this.buttonBrowse);
			this.tabOutput.Controls.Add(this.labelPath);
			this.tabOutput.Controls.Add(this.textBoxPath);
			resources.ApplyResources(this.tabOutput, "tabOutput");
			this.tabOutput.Name = "tabOutput";
			this.tabOutput.UseVisualStyleBackColor = true;
			// 
			// labelBackupHotkey
			// 
			resources.ApplyResources(this.labelBackupHotkey, "labelBackupHotkey");
			this.labelBackupHotkey.Name = "labelBackupHotkey";
			// 
			// backupHotkey
			// 
			this.backupHotkey.Hotkey = System.Windows.Forms.Keys.None;
			this.backupHotkey.HotkeyModifiers = System.Windows.Forms.Keys.None;
			resources.ApplyResources(this.backupHotkey, "backupHotkey");
			this.backupHotkey.Name = "backupHotkey";
			// 
			// backupFilesAmount
			// 
			resources.ApplyResources(this.backupFilesAmount, "backupFilesAmount");
			this.backupFilesAmount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.backupFilesAmount.Name = "backupFilesAmount";
			this.backupFilesAmount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// labelBackupFiles
			// 
			resources.ApplyResources(this.labelBackupFiles, "labelBackupFiles");
			this.labelBackupFiles.Name = "labelBackupFiles";
			// 
			// buttonBrowseBackup
			// 
			resources.ApplyResources(this.buttonBrowseBackup, "buttonBrowseBackup");
			this.buttonBrowseBackup.Name = "buttonBrowseBackup";
			this.buttonBrowseBackup.UseVisualStyleBackColor = true;
			this.buttonBrowseBackup.Click += new System.EventHandler(this.buttonBrowseBackup_Click);
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// pathForBackup
			// 
			resources.ApplyResources(this.pathForBackup, "pathForBackup");
			this.pathForBackup.Name = "pathForBackup";
			// 
			// tabGps
			// 
			this.tabGps.Controls.Add(this.labelBaudRate);
			this.tabGps.Controls.Add(this.serialPortName);
			this.tabGps.Controls.Add(this.serialPortBaudRate);
			this.tabGps.Controls.Add(this.enableGps);
			resources.ApplyResources(this.tabGps, "tabGps");
			this.tabGps.Name = "tabGps";
			this.tabGps.UseVisualStyleBackColor = true;
			// 
			// tabLocalization
			// 
			this.tabLocalization.Controls.Add(this.hideMouse);
			this.tabLocalization.Controls.Add(this.buttonBackColor);
			this.tabLocalization.Controls.Add(this.labelColors);
			this.tabLocalization.Controls.Add(this.buttonTextColor);
			this.tabLocalization.Controls.Add(this.panel1);
			this.tabLocalization.Controls.Add(this.comboLanguage);
			this.tabLocalization.Controls.Add(this.labelLanguage);
			resources.ApplyResources(this.tabLocalization, "tabLocalization");
			this.tabLocalization.Name = "tabLocalization";
			this.tabLocalization.UseVisualStyleBackColor = true;
			// 
			// hideMouse
			// 
			resources.ApplyResources(this.hideMouse, "hideMouse");
			this.hideMouse.Name = "hideMouse";
			this.hideMouse.UseVisualStyleBackColor = true;
			// 
			// buttonBackColor
			// 
			resources.ApplyResources(this.buttonBackColor, "buttonBackColor");
			this.buttonBackColor.Name = "buttonBackColor";
			this.buttonBackColor.UseVisualStyleBackColor = true;
			this.buttonBackColor.Click += new System.EventHandler(this.buttonBackColor_Click);
			// 
			// labelColors
			// 
			resources.ApplyResources(this.labelColors, "labelColors");
			this.labelColors.Name = "labelColors";
			// 
			// buttonTextColor
			// 
			resources.ApplyResources(this.buttonTextColor, "buttonTextColor");
			this.buttonTextColor.Name = "buttonTextColor";
			this.buttonTextColor.UseVisualStyleBackColor = true;
			this.buttonTextColor.Click += new System.EventHandler(this.buttonTextColor_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this.labelTestColor);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// labelTestColor
			// 
			this.labelTestColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			resources.ApplyResources(this.labelTestColor, "labelTestColor");
			this.labelTestColor.Name = "labelTestColor";
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
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.settingsForm_FormClosed);
			this.Shown += new System.EventHandler(this.settingsForm_Shown);
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
			((System.ComponentModel.ISupportInitialize)(this.camFps)).EndInit();
			this.tabCompression.ResumeLayout(false);
			this.tabCompression.PerformLayout();
			this.tabOutput.ResumeLayout(false);
			this.tabOutput.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.backupFilesAmount)).EndInit();
			this.tabGps.ResumeLayout(false);
			this.tabGps.PerformLayout();
			this.tabLocalization.ResumeLayout(false);
			this.tabLocalization.PerformLayout();
			this.panel1.ResumeLayout(false);
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
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.Button buttonBackColor;
		private System.Windows.Forms.Label labelColors;
		private System.Windows.Forms.Button buttonTextColor;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label labelTestColor;
		private System.Windows.Forms.CheckBox dontDisplayWhenAppInactive;
		private System.Windows.Forms.NumericUpDown camFps;
		private System.Windows.Forms.Label labelFrameRate;
		private System.Windows.Forms.CheckBox hideMouse;
		private System.Windows.Forms.Button buttonBrowseBackup;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox pathForBackup;
		private System.Windows.Forms.Label labelBackupFiles;
		private System.Windows.Forms.NumericUpDown backupFilesAmount;
		private exscape.HotkeyControl backupHotkey;
		private System.Windows.Forms.Label labelBackupHotkey;
    }
}