using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdater
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}



		private WebClient client = new WebClient();
		private int DownloadProgress = 0;
		private int ExitTimeout = 5;
		private int ExitTimerMS = 1000;
		private void Form1_Load(object sender, EventArgs e)
		{

			startUpdateTimer.Start();

		}



		private bool StartUpdate()
		{
			client.DownloadProgressChanged += ClientOnDownloadProgressChanged;

			lblConnectMessage.Text = "Reading";
			lblConnectMessage.ForeColor = Color.Blue;
			Application.DoEvents();

			string local = GetLocalHash();

#if !DEBUG
			if (local == "")
			{
				string[] files = Directory.GetFileSystemEntries(Directory.GetCurrentDirectory());
				if (files.Length != 1)
				{
					lblConnectMessage.Text = "Init Error";
					lblConnectMessage.ForeColor = Color.Red;
					MessageBox.Show("The Folder has to be empty for a Initial Installation", "Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
						MessageBoxOptions.DefaultDesktopOnly);
					return false;
				}
			}
#endif


			lblConnectMessage.Text = "Connecting";
			lblConnectMessage.ForeColor = Color.Blue;
			string remote = GetHashFromServer();

			if (remote == "COULDNOTREACHSERVER")
			{
				lblConnectMessage.Text = "Error";
				lblConnectMessage.ForeColor = Color.Red;
				return false;
			}

			if (local == remote)
			{
				lblConnectMessage.Text = "No Updates";
				lblConnectMessage.ForeColor = Color.Green;
			}
			else
			{

				lblConnectMessage.Text = "Cleaning";
				lblConnectMessage.ForeColor = Color.Orange;
				bool success = CleanDirectory();
				if (!success) return false;


				lblConnectMessage.Text = "Downloading";
				success = DownloadUpdate(out string update);
				if (!success) return false;


				lblConnectMessage.Text = "Applying";
				success = ApplyUpdate(update);
				if (!success) return false;

				success = WriteLocalHash(remote); //Update the File
				if (!success) return false;

				lblConnectMessage.Text = "Cleaning";
				lblConnectMessage.ForeColor = Color.Orange;
				success = CleanFile(update);
				if (!success) return false;

				lblConnectMessage.Text = "Done";
				lblConnectMessage.ForeColor = Color.Green;
			}

			return true;

		}


		private bool ApplyUpdate(string updateFile)
		{
			Task t = new Task(() => TaskApplyUpdate(updateFile));
			t.Start();
			while (!t.IsCompleted)
			{
				Application.DoEvents();
			}

			if (t.IsFaulted)
			{
#if DEBUG
				throw t.Exception;
#endif
				lblConnectMessage.Text = "Apply Error";
				lblConnectMessage.ForeColor = Color.Red;
				return false;
			}

			return true;
		}

		private void TaskApplyUpdate(string updateFile)
		{
			string dir = Path.GetDirectoryName(updateFile);
			ZipFile.ExtractToDirectory(updateFile, dir);
		}

		private void ClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			DownloadProgress = e.ProgressPercentage;
		}


		private bool DownloadUpdate(out string updateFile)
		{
			string dir = Directory.GetCurrentDirectory();
			string tempFile = Path.Combine(dir, Path.GetFileName(Path.GetTempFileName()));

			Task t = client.DownloadFileTaskAsync(Program.SERVER_PAYLOAD_PATH, tempFile);
			updateDownloadProgressTimer.Start();

			while (!t.IsCompleted)
			{
				Application.DoEvents();
			}

			updateDownloadProgressTimer.Stop();

			if (t.IsFaulted && t.Exception != null)
			{
#if DEBUG
				throw t.Exception;
#endif
				updateFile = null;
				lblConnectMessage.Text = "Download Error";
				lblConnectMessage.ForeColor = Color.Red;
				return false;
			}

			updateFile = tempFile;
			return true;
		}

		private bool CleanFile(string file)
		{
			Task t = new Task(() => File.Delete(file));
			t.Start();
			while (!t.IsCompleted)
			{
				Application.DoEvents();
			}

			if (t.IsFaulted && t.Exception != null)
			{
#if DEBUG
				throw t.Exception;
#endif
				lblConnectMessage.Text = "Clean Error";
				lblConnectMessage.ForeColor = Color.Red;
				return false;
			}

			return true;
		}

		private bool CleanDirectory()
		{
			Task t = new Task(TaskCleanDirectory);
			t.Start();
			while (!t.IsCompleted)
			{
				Application.DoEvents();
			}

			if (t.IsFaulted && t.Exception != null)
			{
#if DEBUG
				throw t.Exception;
#endif
				lblConnectMessage.Text = "Clean Error";
				lblConnectMessage.ForeColor = Color.Red;
				return false;
			}

			return true;
		}

		private void TaskCleanDirectory()
		{
			string dir = Directory.GetCurrentDirectory();
			string self = Path.GetFullPath(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
			List<string> files = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly).ToList();
			files.Remove(self);
#if DEBUG
			string dbgFileName = Path.GetFileNameWithoutExtension(self) + ".pdb";
			string dbgFile = Path.Combine(Path.GetDirectoryName(self), dbgFileName);
			files.Remove(dbgFile);
#endif

			foreach (string file in files)
			{
				File.Delete(file);
			}

			string[] folders = Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly);
			for (int i = 0; i < folders.Length; i++)
			{
				Directory.Delete(folders[i], true);
			}
		}

		private string GetHashFromServer()
		{
			Task<string> t = client.DownloadStringTaskAsync(Program.SERVER_CONFIG_PATH);
			while (!t.IsCompleted)
			{
				Application.DoEvents();
			}

			if (t.IsFaulted)
			{
#if DEBUG
				throw t.Exception;
#endif
				return "COULDNOTREACHSERVER";
			}

			return t.Result;
		}

		private bool WriteLocalHash(string hash)
		{
			try
			{
				File.WriteAllText(Program.CONFIG_PATH, hash);
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		private string GetLocalHash()
		{
			if (File.Exists(Program.CONFIG_PATH))
			{
				return File.ReadAllText(Program.CONFIG_PATH);
			}

			return "";
		}

		private void updateDownloadProgressTimer_Tick(object sender, EventArgs e)
		{
			lblConnectMessage.Text = $"Download: {DownloadProgress}%";
		}

		private void startUpdateTimer_Tick(object sender, EventArgs e)
		{
			startUpdateTimer.Stop();
			StartUpdate();
			exitTimer.Start();
		}

		private void exitTimer_Tick(object sender, EventArgs e)
		{
			if (ExitTimeout == 0)
			{
				exitTimer.Stop();
				Application.Exit();
				return;
			}
			exitTimer.Interval = ExitTimerMS;
			lblConnectMessage.Text = "Exiting in " + ExitTimeout + "s";
			ExitTimeout--;
		}
	}
}
