namespace CarDVR
{
    partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.buttonSettings = new System.Windows.Forms.Button();
			this.buttonMinimize = new System.Windows.Forms.Button();
			this.buttonStartStop = new System.Windows.Forms.Button();
			this.camView = new System.Windows.Forms.PictureBox();
			this.labelNoVideoSource = new System.Windows.Forms.Label();
			this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.timerDebug = new System.Windows.Forms.Timer(this.components);
			this.FpsDisplayer = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.camView)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonSettings
			// 
			this.buttonSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSettings.Location = new System.Drawing.Point(256, 12);
			this.buttonSettings.Name = "buttonSettings";
			this.buttonSettings.Size = new System.Drawing.Size(90, 62);
			this.buttonSettings.TabIndex = 0;
			this.buttonSettings.Text = "Settings";
			this.buttonSettings.UseVisualStyleBackColor = true;
			this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
			// 
			// buttonMinimize
			// 
			this.buttonMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonMinimize.Location = new System.Drawing.Point(256, 214);
			this.buttonMinimize.Name = "buttonMinimize";
			this.buttonMinimize.Size = new System.Drawing.Size(90, 62);
			this.buttonMinimize.TabIndex = 1;
			this.buttonMinimize.Text = "Minimize";
			this.buttonMinimize.UseVisualStyleBackColor = true;
			this.buttonMinimize.Click += new System.EventHandler(this.buttonMinimize_Click);
			// 
			// buttonStartStop
			// 
			this.buttonStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonStartStop.Location = new System.Drawing.Point(12, 214);
			this.buttonStartStop.Name = "buttonStartStop";
			this.buttonStartStop.Size = new System.Drawing.Size(90, 62);
			this.buttonStartStop.TabIndex = 2;
			this.buttonStartStop.Text = "Start";
			this.buttonStartStop.UseVisualStyleBackColor = true;
			this.buttonStartStop.Click += new System.EventHandler(this.buttonStartStop_Click);
			// 
			// camView
			// 
			this.camView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.camView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.camView.Location = new System.Drawing.Point(12, 12);
			this.camView.Name = "camView";
			this.camView.Size = new System.Drawing.Size(224, 190);
			this.camView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.camView.TabIndex = 3;
			this.camView.TabStop = false;
			// 
			// labelNoVideoSource
			// 
			this.labelNoVideoSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelNoVideoSource.AutoSize = true;
			this.labelNoVideoSource.ForeColor = System.Drawing.Color.Red;
			this.labelNoVideoSource.Location = new System.Drawing.Point(114, 239);
			this.labelNoVideoSource.Name = "labelNoVideoSource";
			this.labelNoVideoSource.Size = new System.Drawing.Size(128, 13);
			this.labelNoVideoSource.TabIndex = 4;
			this.labelNoVideoSource.Text = "No video source selected";
			// 
			// trayIcon
			// 
			this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
			this.trayIcon.Text = "CarDVR";
			this.trayIcon.Visible = true;
			this.trayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseClick);
			// 
			// timerDebug
			// 
			this.timerDebug.Interval = 500;
			this.timerDebug.Tick += new System.EventHandler(this.timerDebug_Tick);
			// 
			// FpsDisplayer
			// 
			this.FpsDisplayer.Enabled = true;
			this.FpsDisplayer.Interval = 1000;
			this.FpsDisplayer.Tick += new System.EventHandler(this.FpsDisplayer_Tick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(357, 288);
			this.Controls.Add(this.labelNoVideoSource);
			this.Controls.Add(this.camView);
			this.Controls.Add(this.buttonStartStop);
			this.Controls.Add(this.buttonMinimize);
			this.Controls.Add(this.buttonSettings);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(373, 326);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Car DVR";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			((System.ComponentModel.ISupportInitialize)(this.camView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Button buttonMinimize;
        private System.Windows.Forms.Button buttonStartStop;
        private System.Windows.Forms.PictureBox camView;
        private System.Windows.Forms.Label labelNoVideoSource;
		private System.Windows.Forms.NotifyIcon trayIcon;
		private System.Windows.Forms.Timer timerDebug;
		private System.Windows.Forms.Timer FpsDisplayer;
    }
}

