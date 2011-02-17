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
			this.videoDrawer = new System.Windows.Forms.Timer(this.components);
			this.buttonMaximize = new System.Windows.Forms.Button();
			this.buttonExit = new System.Windows.Forms.Button();
			this.buttonBackup = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.camView)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonSettings
			// 
			resources.ApplyResources(this.buttonSettings, "buttonSettings");
			this.buttonSettings.Name = "buttonSettings";
			this.buttonSettings.UseVisualStyleBackColor = true;
			this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
			// 
			// buttonMinimize
			// 
			resources.ApplyResources(this.buttonMinimize, "buttonMinimize");
			this.buttonMinimize.Name = "buttonMinimize";
			this.buttonMinimize.UseVisualStyleBackColor = true;
			this.buttonMinimize.Click += new System.EventHandler(this.buttonMinimize_Click);
			// 
			// buttonStartStop
			// 
			resources.ApplyResources(this.buttonStartStop, "buttonStartStop");
			this.buttonStartStop.Name = "buttonStartStop";
			this.buttonStartStop.UseVisualStyleBackColor = true;
			this.buttonStartStop.Click += new System.EventHandler(this.buttonStartStop_Click);
			// 
			// camView
			// 
			resources.ApplyResources(this.camView, "camView");
			this.camView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.camView.Name = "camView";
			this.camView.TabStop = false;
			this.camView.Click += new System.EventHandler(this.camView_Click);
			// 
			// labelNoVideoSource
			// 
			resources.ApplyResources(this.labelNoVideoSource, "labelNoVideoSource");
			this.labelNoVideoSource.ForeColor = System.Drawing.SystemColors.ControlText;
			this.labelNoVideoSource.Name = "labelNoVideoSource";
			// 
			// videoDrawer
			// 
			this.videoDrawer.Tick += new System.EventHandler(this.videoDrawer_Tick);
			// 
			// buttonMaximize
			// 
			resources.ApplyResources(this.buttonMaximize, "buttonMaximize");
			this.buttonMaximize.Name = "buttonMaximize";
			this.buttonMaximize.UseVisualStyleBackColor = true;
			this.buttonMaximize.Click += new System.EventHandler(this.buttonMaximize_Click);
			// 
			// buttonExit
			// 
			resources.ApplyResources(this.buttonExit, "buttonExit");
			this.buttonExit.Name = "buttonExit";
			this.buttonExit.UseVisualStyleBackColor = true;
			this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
			// 
			// buttonBackup
			// 
			resources.ApplyResources(this.buttonBackup, "buttonBackup");
			this.buttonBackup.Name = "buttonBackup";
			this.buttonBackup.UseVisualStyleBackColor = true;
			this.buttonBackup.Click += new System.EventHandler(this.buttonBackup_Click);
			// 
			// MainForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.labelNoVideoSource);
			this.Controls.Add(this.camView);
			this.Controls.Add(this.buttonBackup);
			this.Controls.Add(this.buttonExit);
			this.Controls.Add(this.buttonMaximize);
			this.Controls.Add(this.buttonStartStop);
			this.Controls.Add(this.buttonMinimize);
			this.Controls.Add(this.buttonSettings);
			this.Name = "MainForm";
			this.Activated += new System.EventHandler(this.MainForm_Activated);
			this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
			this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
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
		private System.Windows.Forms.Timer videoDrawer;
		private System.Windows.Forms.Button buttonMaximize;
		private System.Windows.Forms.Button buttonExit;
		private System.Windows.Forms.Button buttonBackup;
    }
}

