namespace AutoUpdater
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
            this.components = new System.ComponentModel.Container();
            this.lblConnectMessage = new System.Windows.Forms.Label();
            this.ConnectPanel = new System.Windows.Forms.Panel();
            this.updateDownloadProgressTimer = new System.Windows.Forms.Timer(this.components);
            this.startUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.exitTimer = new System.Windows.Forms.Timer(this.components);
            this.ConnectPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblConnectMessage
            // 
            this.lblConnectMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnectMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectMessage.Location = new System.Drawing.Point(12, 19);
            this.lblConnectMessage.Name = "lblConnectMessage";
            this.lblConnectMessage.Size = new System.Drawing.Size(431, 86);
            this.lblConnectMessage.TabIndex = 0;
            this.lblConnectMessage.Text = "Connecting..";
            this.lblConnectMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConnectPanel
            // 
            this.ConnectPanel.Controls.Add(this.lblConnectMessage);
            this.ConnectPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectPanel.Location = new System.Drawing.Point(0, 0);
            this.ConnectPanel.Name = "ConnectPanel";
            this.ConnectPanel.Size = new System.Drawing.Size(455, 119);
            this.ConnectPanel.TabIndex = 1;
            // 
            // updateDownloadProgressTimer
            // 
            this.updateDownloadProgressTimer.Tick += new System.EventHandler(this.updateDownloadProgressTimer_Tick);
            // 
            // startUpdateTimer
            // 
            this.startUpdateTimer.Tick += new System.EventHandler(this.startUpdateTimer_Tick);
            // 
            // exitTimer
            // 
            this.exitTimer.Interval = 5000;
            this.exitTimer.Tick += new System.EventHandler(this.exitTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 119);
            this.Controls.Add(this.ConnectPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(471, 158);
            this.Name = "Form1";
            this.Text = "Auto Updater";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ConnectPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblConnectMessage;
		private System.Windows.Forms.Panel ConnectPanel;
		private System.Windows.Forms.Timer updateDownloadProgressTimer;
		private System.Windows.Forms.Timer startUpdateTimer;
		private System.Windows.Forms.Timer exitTimer;
	}
}

