using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video.VFW;
using AForge.Video.DirectShow;
using AForge.Video;
using System.IO;
using System.Threading;
using System.Resources;
using System.Globalization;

namespace CarDVR
{
	public partial class MainForm : Form
	{
		private AutoStartDelayer autoStartDelayer;
		private static RecordingState recordingState = RecordingState.Stopped;

		static GpsReceiver gps = new GpsReceiver();
		public VideoManager videoManager = new VideoManager(gps);

		private void buttonSettings_Click(object sender, EventArgs e)
		{
			using (settingsForm settingsForm = new settingsForm())
			{
				FormColorSetter.Do
				(
					settingsForm, 
					Color.FromArgb(Program.settings.InterfaceForeColor), 
					Color.FromArgb(Program.settings.InterfaceBackgroundColor)
				);

				if (settingsForm.ShowDialog() == DialogResult.Cancel)
					return;

				settingsForm.ProcessResult();

				AutorunHelper.SetEnabled(Program.settings.StartWithWindows);
				IsWebCamAvaliable();

				// reinit video source
				{
					bool isRecording = videoManager.IsRecording();

					if (isRecording)
						StopRecording();

					try
					{
						videoManager.Initialize();

						if (isRecording)
							StartRecording();
					}
					catch
					{
						Reporter.SeriousError(Resources.CurrentWebCamNotAvaliable);
					}
				}

				InitializeGps();

				CommonInitialization();
			}
		}

		private void StartGpsReceiver()
		{
			if (!Program.settings.GpsEnabled)
				return;

			try
			{
				gps.Open();
			}
			catch (Exception e)
			{
				Reporter.UnseriousError
				(
					string.Format
					(
						Resources.CantOpenGpsOnPort,
						Program.settings.GpsSerialPort,
						e.Message
					)
				);
			}
		}

		private void StartRecording()
		{
			if (recordingState == RecordingState.Started)
				return;

			ButtonStartStopDisable();

			// Update settings, may be web cam became not avaliable
			Program.settings.Read();
			IsWebCamAvaliable();

			InitializeGps();
			StartGpsReceiver();

			videoManager.Start();
			videoDrawer.Enabled = true;
			
			buttonStartStop.Text = Resources.Stop;
			recordingState = RecordingState.Started;

			ButtonStartStopEnable();
		}

		public void StopRecording()
		{
			if (recordingState == RecordingState.Stopped)
				return;

			ButtonStartStopDisable();

			gps.Close();
			videoManager.Stop();
			videoDrawer.Enabled = false;

			// TODO: make class ImageDrawer (use empty)
			camView.Image = new Bitmap(Program.settings.VideoWidth, Program.settings.VideoHeight);

			buttonStartStop.Text = Resources.Start;
			recordingState = RecordingState.Stopped;

			ButtonStartStopEnable();
		}

		private void StartStopRecording()
		{
			switch (recordingState)
			{
				case RecordingState.Stopped:
					StartRecording();
					break;

				case RecordingState.Started:
					StopRecording();
					break;
			}
		}

		private void StopRecordingAndWaitForFileClosing()
		{
			if (videoManager.IsRecording())
			{
				StopRecording();
				videoManager.Close();
			}
		}

		private void buttonStartStop_Click(object sender, EventArgs e)
		{
			StartStopRecording();
		}

		private void AutostartDelayer_Handler(object sender, EventArgs e)
		{
			// TODO: make class BitmapDrawer
			camView.Image = new Bitmap(Program.settings.VideoWidth, Program.settings.VideoHeight);

			ButtonStartStopEnable();
			VideoInitialization();
		}

		private void camView_Click(object sender, EventArgs e)
		{
			if (VideoWindowMode == FillMode.Normal)
				MakeFullWindowVideo();
			else if (VideoWindowMode == FillMode.Full)
				MakeSmallSizedVideo();
		}

		private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			StopRecordingAndWaitForFileClosing();
		}

		private void buttonExit_Click(object sender, EventArgs e)
		{
			StopRecordingAndWaitForFileClosing();
			this.Close();
		}

		private void videoDrawer_Tick(object sender, EventArgs e)
		{
			DrawFrameOnForm();
		}

		private void buttonBackup_Click(object sender, EventArgs e)
		{
			buttonBackup.Enabled = false;
			FileInfo[] files = null;

			if (recordingState == RecordingState.Started)
			{
				StopRecording();
				files = FileInfoSorter.Get(Program.settings.PathForVideo);
				StartRecording();
			}
			else
				files = FileInfoSorter.Get(Program.settings.PathForVideo);

			VideoBackuper backuper = new VideoBackuper
			(
				files,
				BackupFinished
			);

			backuper.Do();
		}

		private void BackupFinished(object sender, EventArgs args)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new EventHandler(BackupFinished), new object[] { sender, args });
				return;
			}

			VideoBackuper backuper = sender as VideoBackuper;

			int count = 0;
			if (backuper != null)
			{
				count = backuper.GetFilesCopied();
			}
			
			buttonBackup.Enabled = true;			
		}
	}
}
