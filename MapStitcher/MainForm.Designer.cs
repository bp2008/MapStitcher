namespace MapStitcher
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtBaseUrl = new System.Windows.Forms.TextBox();
			this.nudZoom = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.pbWorking = new System.Windows.Forms.ProgressBar();
			this.label7 = new System.Windows.Forms.Label();
			this.lblStatus = new System.Windows.Forms.Label();
			this.btnDownload = new System.Windows.Forms.Button();
			this.btnStitchIntoOne = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.nudDownloadThreads = new System.Windows.Forms.NumericUpDown();
			this.nudStitchThreads = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.nudStartLon = new System.Windows.Forms.NumericUpDown();
			this.label12 = new System.Windows.Forms.Label();
			this.nudStartLat = new System.Windows.Forms.NumericUpDown();
			this.label13 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.nudEndLat = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.nudEndLon = new System.Windows.Forms.NumericUpDown();
			this.lblPercent = new System.Windows.Forms.Label();
			this.picPreview = new System.Windows.Forms.PictureBox();
			this.lblTileInfo = new System.Windows.Forms.Label();
			this.nudOutputQuality = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudZoom)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudDownloadThreads)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudStitchThreads)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudStartLon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudStartLat)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudEndLat)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudEndLon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOutputQuality)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(59, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Base URL:";
			// 
			// txtBaseUrl
			// 
			this.txtBaseUrl.Location = new System.Drawing.Point(78, 12);
			this.txtBaseUrl.Name = "txtBaseUrl";
			this.txtBaseUrl.Size = new System.Drawing.Size(622, 20);
			this.txtBaseUrl.TabIndex = 1;
			this.txtBaseUrl.TextChanged += new System.EventHandler(this.txtBaseUrl_TextChanged);
			// 
			// nudZoom
			// 
			this.nudZoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nudZoom.Location = new System.Drawing.Point(109, 90);
			this.nudZoom.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
			this.nudZoom.Name = "nudZoom";
			this.nudZoom.Size = new System.Drawing.Size(58, 31);
			this.nudZoom.TabIndex = 50;
			this.nudZoom.ValueChanged += new System.EventHandler(this.nudZoom_ValueChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(31, 92);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 25);
			this.label6.TabIndex = 10;
			this.label6.Text = "Zoom:";
			// 
			// pbWorking
			// 
			this.pbWorking.Location = new System.Drawing.Point(14, 645);
			this.pbWorking.MarqueeAnimationSpeed = 10;
			this.pbWorking.Name = "pbWorking";
			this.pbWorking.Size = new System.Drawing.Size(686, 23);
			this.pbWorking.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.pbWorking.TabIndex = 12;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(15, 620);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(43, 13);
			this.label7.TabIndex = 13;
			this.label7.Text = "Status: ";
			// 
			// lblStatus
			// 
			this.lblStatus.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblStatus.Location = new System.Drawing.Point(64, 619);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(560, 23);
			this.lblStatus.TabIndex = 14;
			// 
			// btnDownload
			// 
			this.btnDownload.Location = new System.Drawing.Point(14, 432);
			this.btnDownload.Name = "btnDownload";
			this.btnDownload.Size = new System.Drawing.Size(169, 23);
			this.btnDownload.TabIndex = 60;
			this.btnDownload.Text = "Download Maps";
			this.btnDownload.UseVisualStyleBackColor = true;
			this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
			// 
			// btnStitchIntoOne
			// 
			this.btnStitchIntoOne.Location = new System.Drawing.Point(14, 515);
			this.btnStitchIntoOne.Name = "btnStitchIntoOne";
			this.btnStitchIntoOne.Size = new System.Drawing.Size(169, 23);
			this.btnStitchIntoOne.TabIndex = 90;
			this.btnStitchIntoOne.Text = "Stitch Into One Map";
			this.btnStitchIntoOne.UseVisualStyleBackColor = true;
			this.btnStitchIntoOne.Click += new System.EventHandler(this.btnStitchIntoOne_Click);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(21, 408);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(100, 13);
			this.label8.TabIndex = 18;
			this.label8.Text = "Download Threads:";
			// 
			// nudDownloadThreads
			// 
			this.nudDownloadThreads.Location = new System.Drawing.Point(127, 406);
			this.nudDownloadThreads.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudDownloadThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudDownloadThreads.Name = "nudDownloadThreads";
			this.nudDownloadThreads.Size = new System.Drawing.Size(44, 20);
			this.nudDownloadThreads.TabIndex = 55;
			this.nudDownloadThreads.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.nudDownloadThreads.ValueChanged += new System.EventHandler(this.nudDownloadThreads_ValueChanged);
			// 
			// nudStitchThreads
			// 
			this.nudStitchThreads.Location = new System.Drawing.Point(118, 466);
			this.nudStitchThreads.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudStitchThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudStitchThreads.Name = "nudStitchThreads";
			this.nudStitchThreads.Size = new System.Drawing.Size(44, 20);
			this.nudStitchThreads.TabIndex = 70;
			this.nudStitchThreads.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.nudStitchThreads.ValueChanged += new System.EventHandler(this.nudStitchThreads_ValueChanged);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(33, 468);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(79, 13);
			this.label9.TabIndex = 20;
			this.label9.Text = "Stitch Threads:";
			// 
			// nudStartLon
			// 
			this.nudStartLon.DecimalPlaces = 12;
			this.nudStartLon.Location = new System.Drawing.Point(409, 38);
			this.nudStartLon.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.nudStartLon.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
			this.nudStartLon.Name = "nudStartLon";
			this.nudStartLon.Size = new System.Drawing.Size(131, 20);
			this.nudStartLon.TabIndex = 29;
			this.nudStartLon.ValueChanged += new System.EventHandler(this.nudStartLon_ValueChanged);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(346, 40);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(57, 13);
			this.label12.TabIndex = 28;
			this.label12.Text = "Longitude:";
			// 
			// nudStartLat
			// 
			this.nudStartLat.DecimalPlaces = 12;
			this.nudStartLat.Location = new System.Drawing.Point(189, 38);
			this.nudStartLat.Maximum = new decimal(new int[] {
            -84821714,
            1,
            0,
            524288});
			this.nudStartLat.Minimum = new decimal(new int[] {
            -84821714,
            1,
            0,
            -2146959360});
			this.nudStartLat.Name = "nudStartLat";
			this.nudStartLat.Size = new System.Drawing.Size(131, 20);
			this.nudStartLat.TabIndex = 27;
			this.nudStartLat.ValueChanged += new System.EventHandler(this.nudStartLat_ValueChanged);
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(135, 40);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(48, 13);
			this.label13.TabIndex = 26;
			this.label13.Text = "Latitude:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(91, 13);
			this.label2.TabIndex = 34;
			this.label2.Text = "Upper-Left Corner";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(13, 66);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(98, 13);
			this.label3.TabIndex = 39;
			this.label3.Text = "Lower-Right Corner";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(346, 66);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(57, 13);
			this.label4.TabIndex = 37;
			this.label4.Text = "Longitude:";
			// 
			// nudEndLat
			// 
			this.nudEndLat.DecimalPlaces = 12;
			this.nudEndLat.Location = new System.Drawing.Point(189, 64);
			this.nudEndLat.Maximum = new decimal(new int[] {
            -84821714,
            1,
            0,
            524288});
			this.nudEndLat.Minimum = new decimal(new int[] {
            -84821714,
            1,
            0,
            -2146959360});
			this.nudEndLat.Name = "nudEndLat";
			this.nudEndLat.Size = new System.Drawing.Size(131, 20);
			this.nudEndLat.TabIndex = 36;
			this.nudEndLat.ValueChanged += new System.EventHandler(this.nudEndLat_ValueChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(135, 66);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 13);
			this.label5.TabIndex = 35;
			this.label5.Text = "Latitude:";
			// 
			// nudEndLon
			// 
			this.nudEndLon.DecimalPlaces = 12;
			this.nudEndLon.Location = new System.Drawing.Point(409, 64);
			this.nudEndLon.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.nudEndLon.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
			this.nudEndLon.Name = "nudEndLon";
			this.nudEndLon.Size = new System.Drawing.Size(131, 20);
			this.nudEndLon.TabIndex = 40;
			this.nudEndLon.ValueChanged += new System.EventHandler(this.nudEndLon_ValueChanged);
			// 
			// lblPercent
			// 
			this.lblPercent.Font = new System.Drawing.Font("Consolas", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPercent.Location = new System.Drawing.Point(12, 541);
			this.lblPercent.Name = "lblPercent";
			this.lblPercent.Size = new System.Drawing.Size(169, 78);
			this.lblPercent.TabIndex = 41;
			this.lblPercent.Text = "--%";
			this.lblPercent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// picPreview
			// 
			this.picPreview.Location = new System.Drawing.Point(189, 90);
			this.picPreview.Name = "picPreview";
			this.picPreview.Size = new System.Drawing.Size(512, 512);
			this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picPreview.TabIndex = 42;
			this.picPreview.TabStop = false;
			// 
			// lblTileInfo
			// 
			this.lblTileInfo.Location = new System.Drawing.Point(12, 124);
			this.lblTileInfo.Name = "lblTileInfo";
			this.lblTileInfo.Size = new System.Drawing.Size(169, 279);
			this.lblTileInfo.TabIndex = 43;
			this.lblTileInfo.Text = "...";
			// 
			// nudOutputQuality
			// 
			this.nudOutputQuality.Location = new System.Drawing.Point(118, 492);
			this.nudOutputQuality.Name = "nudOutputQuality";
			this.nudOutputQuality.Size = new System.Drawing.Size(44, 20);
			this.nudOutputQuality.TabIndex = 80;
			this.nudOutputQuality.ValueChanged += new System.EventHandler(this.nudOutputQuality_ValueChanged);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(8, 494);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(104, 13);
			this.label10.TabIndex = 45;
			this.label10.Text = "Jpeg Quality [1-100]:";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(712, 680);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.nudOutputQuality);
			this.Controls.Add(this.lblTileInfo);
			this.Controls.Add(this.picPreview);
			this.Controls.Add(this.lblPercent);
			this.Controls.Add(this.nudEndLon);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.nudEndLat);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.nudStartLon);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.nudStartLat);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.nudStitchThreads);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.nudDownloadThreads);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.btnStitchIntoOne);
			this.Controls.Add(this.btnDownload);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.pbWorking);
			this.Controls.Add(this.nudZoom);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtBaseUrl);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "MainForm";
			this.Text = "Map Stitcher";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.nudZoom)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudDownloadThreads)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudStitchThreads)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudStartLon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudStartLat)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudEndLat)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudEndLon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOutputQuality)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtBaseUrl;
		private System.Windows.Forms.NumericUpDown nudZoom;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ProgressBar pbWorking;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Button btnDownload;
		private System.Windows.Forms.Button btnStitchIntoOne;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown nudDownloadThreads;
		private System.Windows.Forms.NumericUpDown nudStitchThreads;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.NumericUpDown nudStartLon;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.NumericUpDown nudStartLat;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown nudEndLat;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown nudEndLon;
		private System.Windows.Forms.Label lblPercent;
		private System.Windows.Forms.PictureBox picPreview;
		private System.Windows.Forms.Label lblTileInfo;
		private System.Windows.Forms.NumericUpDown nudOutputQuality;
		private System.Windows.Forms.Label label10;
	}
}

