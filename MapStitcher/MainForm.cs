using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BPUtil;
using Microsoft.MapPoint;

namespace MapStitcher
{
	public partial class MainForm : Form
	{
		BackgroundWorker bwWorker = null;
		bool queuedPreviewFrame = false;
		Settings settings = new Settings();
		bool saveEnabled = false;
		string preprocessError = null;

		public MainForm()
		{
			CertificateValidation.RegisterCallback(CertificateValidation.DoNotValidate_ValidationCallback);
			InitializeComponent();
			settings.Load();
			txtBaseUrl.Text = settings.BaseURL;
			nudStartLat.Value = settings.startLat;
			nudStartLon.Value = settings.startLon;
			nudEndLat.Value = settings.endLat;
			nudEndLon.Value = settings.endLon;
			nudZoom.Value = settings.zoom;
			nudDownloadThreads.Value = settings.downloadThreads;
			nudStitchThreads.Value = settings.stitchThreads;
			nudOutputQuality.Value = settings.jpegQuality;
			saveEnabled = true;
			if (BuildTileInfoLabel())
				GetPreviewFrame();
		}

		private void SaveSettings()
		{
			if (saveEnabled)
			{
				settings.BaseURL = txtBaseUrl.Text;
				settings.startLat = nudStartLat.Value;
				settings.startLon = nudStartLon.Value;
				settings.endLat = nudEndLat.Value;
				settings.endLon = nudEndLon.Value;
				settings.zoom = (int)nudZoom.Value;
				settings.downloadThreads = (int)nudDownloadThreads.Value;
				settings.stitchThreads = (int)nudStitchThreads.Value;
				settings.jpegQuality = (int)nudOutputQuality.Value;
				settings.Save();
				if (BuildTileInfoLabel())
					GetPreviewFrame();
			}
		}

		private bool BuildTileInfoLabel()
		{
			preprocessError = null;
			Point start = Util.LatLonToTileCoordinate((double)nudStartLat.Value, (double)nudStartLon.Value, (int)nudZoom.Value);
			Point end = Util.LatLonToTileCoordinate((double)nudEndLat.Value, (double)nudEndLon.Value, (int)nudZoom.Value);
			if (start.X > end.X || start.Y > end.Y)
			{
				lblTileInfo.Text = preprocessError = "Upper-Left coordinate must be above and to the left of Lower-Right coordinate.";
				return false;
			}
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Upper-Left Tile:");
			sb.AppendLine("    " + start.X + ", " + start.Y);
			sb.AppendLine();
			sb.AppendLine("Lower-Right Tile:");
			sb.AppendLine("    " + end.X + ", " + end.Y);
			sb.AppendLine();
			long tilesWide = ((end.X - (long)start.X) + 1);
			long tilesTall = ((end.Y - (long)start.Y) + 1);
			long totalTiles = tilesWide * tilesTall;
			sb.AppendLine("Tiles: " + totalTiles + " (" + tilesWide + "x" + tilesTall + ")");
			long maxMapTiles = 10922;
			if (totalTiles > 10922)
			{
				sb.AppendLine("TOO MANY MAP TILES!");
				sb.AppendLine("Please choose " + maxMapTiles + " or fewer!");
				preprocessError = "You have currently selected " + totalTiles + " map tiles, which is too large for this program to stitch because the uncompressed format would exceed the capacity of a byte array." + Environment.NewLine + Environment.NewLine + "Please reduce the selection size or zoom out until you have selected " + maxMapTiles + " or fewer tiles.";
			}
			sb.AppendLine();
			sb.AppendLine("Megapixels: " + ((tilesWide * 256 * tilesTall * 256) / 1000000d).ToString("0") + " (" + (tilesWide * 256) + "x" + (tilesTall * 256) + ")");
			sb.AppendLine();
			double groundRes = TileSystem.GroundResolution((double)nudStartLat.Value, (int)nudZoom.Value);
			string sGroundRes;
			if (groundRes > 0 && groundRes < 1)
				sGroundRes = (1 / groundRes).ToString("0.##") + " pixels/m";
			else
				sGroundRes = groundRes.ToString("0.##") + " m/pixel";
			sb.Append("Ground Resolution: " + sGroundRes);
			lblTileInfo.Text = sb.ToString();
			return true;
		}

		private void InitializeBackgroundWorker()
		{
			btnDownload.Enabled = btnStitchIntoOne.Enabled = false;
			bwWorker = new BackgroundWorker();
			bwWorker.DoWork += BwWorker_DoWork;
			bwWorker.ProgressChanged += BwWorker_ProgressChanged;
			bwWorker.RunWorkerCompleted += BwWorker_RunWorkerCompleted;
			bwWorker.WorkerReportsProgress = true;
			bwWorker.WorkerSupportsCancellation = true;
		}

		private void BwWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Result == null)
			{
				lblStatus.Text = "";
				SetPercentProgress(0);
			}
			else if (e.Result is Image)
			{
				SetPreviewImage((Image)e.Result);
				lblStatus.Text = "";
				SetPercentProgress(0);
			}
			else if (e.Result is StitchingFinished)
			{
				StitchingFinished result = (StitchingFinished)e.Result;
				lblStatus.Text = "Stitching finished";
				SetPercentProgress(100);
				Process.Start(result.filePath);
			}
			else
			{
				lblStatus.Text = e.Result.ToString();
				SetPercentProgress(100);
			}
			btnDownload.Enabled = btnStitchIntoOne.Enabled = true;
			bwWorker = null;
			if (queuedPreviewFrame)
			{
				queuedPreviewFrame = false;
				GetPreviewFrame();
			}
		}
		private void SetPreviewImage(Image img)
		{
			if (picPreview.Image != null)
				picPreview.Image.Dispose();
			picPreview.Image = img;
		}

		private void BwWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			lblStatus.Text = e.UserState.ToString();
			SetPercentProgress(e.ProgressPercentage);
		}
		private void SetPercentProgress(int percent)
		{
			pbWorking.Value = percent;
			lblPercent.Text = percent + "%";
		}

		private void BwWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				e.Result = ((Func<object>)e.Argument)();
			}
			catch (Exception ex)
			{
				Logger.Debug(ex);
				MessageBox.Show(ex.ToString());
				e.Result = "Error";
			}
		}

		private void GetPreviewFrame()
		{
			if (bwWorker != null)
			{
				queuedPreviewFrame = true;
				return;
			}
			SetPreviewImage(null);
			string BaseURL = txtBaseUrl.Text;
			FourTiles previewTiles = Find4TilesWhichFitPoints((double)nudStartLat.Value, (double)nudStartLon.Value, (double)nudEndLat.Value, (double)nudEndLon.Value);
			Func<object> doWork = () =>
			{
				bwWorker.ReportProgress(0, "Generating Preview");
				foreach (Point tile in previewTiles.Tiles)
					SynchronousDownloadTileIfNotCached(BaseURL, tile, previewTiles.Zoom);
				List<Point> t = previewTiles.Tiles;
				string outPath = "MapCache/Preview_" + previewTiles.Zoom + "_" + t[0].X + "x" + t[0].Y + "_" + t[t.Count - 1].X + "x" + t[t.Count - 1].Y + ".jpg";
				if (!File.Exists(outPath))
				{
					byte[] merged = MergeImages(previewTiles.Tiles, previewTiles.Zoom, out int tilesWide, out int tilesTall, out Size chunkSize);
					if (merged == null)
						return null;
					WriteCompressedImage(outPath, merged, tilesWide * chunkSize.Width, tilesTall * chunkSize.Height);
				}
				if (File.Exists(outPath))
				{
					Bitmap bmp = (Bitmap)Util.ImageFromFile(outPath);
					DrawRectInBmp(bmp, (double)nudStartLat.Value, (double)nudStartLon.Value, (double)nudEndLat.Value, (double)nudEndLon.Value, previewTiles.Zoom, previewTiles.Tiles.Count == 4);
					return bmp;
				}
				return null;
			};
			InitializeBackgroundWorker();
			bwWorker.RunWorkerAsync(doWork);
		}

		private void DrawRectInBmp(Bitmap bmp, double lat1, double lon1, double lat2, double lon2, int zoom, bool isBuiltFrom4)
		{
			RelativePixel topLeft = Util.GetRelativePixel(lat1, lon1, zoom);
			RelativePixel botRight = Util.GetRelativePixel(lat2, lon2, zoom);
			if (topLeft.tile.X != botRight.tile.X)
				botRight.pixel.X += 256;
			if (topLeft.tile.Y != botRight.tile.Y)
				botRight.pixel.Y += 256;

			using (Graphics graphics = Graphics.FromImage(bmp))
			{
				using (Pen myPen = new Pen(Color.Red, 1))
				{
					graphics.DrawRectangle(myPen, new Rectangle(topLeft.pixel.X - 1, topLeft.pixel.Y - 1, botRight.pixel.X - topLeft.pixel.X + 2, botRight.pixel.Y - topLeft.pixel.Y + 2));
				}
			}
		}



		private void WriteCompressedImage(string outPath, byte[] img, int width, int height)
		{
			JpegCodec imgCodec = new JpegCodec();
			byte[] compressed = imgCodec.Encode(img, width, height, turbojpegCLI.SubsamplingOption.SAMP_420, settings.jpegQuality, turbojpegCLI.PixelFormat.BGR);
			File.WriteAllBytes(outPath, compressed);
		}

		private void SynchronousDownloadTileIfNotCached(string BaseURL, Point tile, int zoom)
		{
			if (IsCached(zoom, tile.X, tile.Y))
				return;
			string url = GetImageUrl(BaseURL, tile, zoom);
			byte[] image = DownloadImage(url);
			if (image != null && image.Length > 0)
				WriteImageToCache(zoom, tile.X, tile.Y, image);
			else
				throw new Exception("Failed to receive preview image.");
		}

		/// <summary>
		/// Finds a set of 4 tiles that includes both of the specified points.  Ensure that lat1,lon1 is to the upper left of lat2,lon2 before calling this function!
		/// </summary>
		/// <param name="lat1"></param>
		/// <param name="lon1"></param>
		/// <param name="lat2"></param>
		/// <param name="lon2"></param>
		/// <returns></returns>
		private FourTiles Find4TilesWhichFitPoints(double lat1, double lon1, double lat2, double lon2)
		{
			int zoom = (int)nudZoom.Value + 1;
			while (--zoom > 0)
			{
				Point one = Util.LatLonToTileCoordinate(lat1, lon1, zoom);
				Point two = Util.LatLonToTileCoordinate(lat2, lon2, zoom);
				int diff = two.X - one.X;
				if (diff > 1)
					continue;
				diff = two.Y - one.Y;
				if (diff > 1)
					continue;
				return new FourTiles(one, zoom);
			}
			return new FourTiles(new Point(0, 0), 0);
		}

		private void btnDownload_Click(object sender, EventArgs e)
		{
			if (BuildTileInfoLabel())
			{
				if (!string.IsNullOrWhiteSpace(preprocessError))
				{
					MessageBox.Show(preprocessError);
					return;
				}
			}
			else
			{
				MessageBox.Show("Unable to process request.  Please check input.");
				return;
			}
			int threadCount = Math.Max(1, Math.Min(10, (int)nudDownloadThreads.Value));
			int z = (int)nudZoom.Value;
			Point start = Util.LatLonToTileCoordinate((double)nudStartLat.Value, (double)nudStartLon.Value, (int)nudZoom.Value);
			Point end = Util.LatLonToTileCoordinate((double)nudEndLat.Value, (double)nudEndLon.Value, (int)nudZoom.Value);
			long tilesWide = ((end.X - (long)start.X) + 1);
			long tilesTall = ((end.Y - (long)start.Y) + 1);
			long totalChunks = tilesWide * tilesTall;
			string BaseURL = txtBaseUrl.Text;
			Func<string> doWork = () =>
			{
				bwWorker.ReportProgress(0, "Checking image cache");
				long failed = 0;
				long downloaded = 0;
				ConcurrentQueue<Point> toDownload = new ConcurrentQueue<Point>();
				for (int x = start.X; x <= end.X; x++)
				{
					for (int y = start.Y; y <= end.Y; y++)
					{
						if (!IsCached(z, x, y))
							toDownload.Enqueue(new Point(x, y));
					}
				}
				double totalToDownload = (double)toDownload.Count;
				bwWorker.ReportProgress(0, "Beginning " + totalToDownload + " image downloads");
				List<Thread> threads = new List<Thread>();
				for (int i = 0; i < threadCount; i++)
				{
					Thread thr = new Thread(() =>
					{
						try
						{
							while (toDownload.TryDequeue(out Point p))
							{
								string url = GetImageUrl(BaseURL, p, z);
								byte[] image = null;

								try
								{
									image = DownloadImage(url);
									Interlocked.Increment(ref downloaded);
								}
								catch (ThreadAbortException) { }
								catch (Exception ex)
								{
									Logger.Debug(ex, url);
									Interlocked.Increment(ref failed);
									bwWorker.ReportProgress(0, "Error: " + ex.Message);
									return;
								}
								if (image != null && image.Length > 0)
								{
									WriteImageToCache(z, p.X, p.Y, image);
									long dl = Interlocked.Read(ref downloaded);
									int progress = (int)Math.Round((dl / totalToDownload) * 100);
									bwWorker.ReportProgress(progress, "Downloaded " + dl + " of " + totalToDownload + " images (" + progress + "%)");
								}
								else
								{
									Logger.Info("Image " + url + " was " + (image == null ? "null" : "empty"));
									Interlocked.Increment(ref failed);
								}
							}
						}
						catch (ThreadAbortException) { }
						catch (Exception ex)
						{
							Logger.Debug(ex);
							MessageBox.Show(ex.ToString());
						}
					});
					thr.Name = "Download Thread " + i;
					thr.IsBackground = true;
					thr.Start();
					threads.Add(thr);
				}
				while (!bwWorker.CancellationPending && toDownload.Count > 0)
				{
					Thread.Sleep(33);
				}
				foreach (Thread thr in threads)
				{
					if (bwWorker.CancellationPending)
						thr.Abort();
					thr.Join();
				}
				if (failed > 0)
					return failed + " images failed to download.";
				else
					return "Finished downloading images.";
			};
			InitializeBackgroundWorker();
			bwWorker.RunWorkerAsync(doWork);
		}

		private static string GetImageUrl(string BaseURL, Point p, int z)
		{
			return BaseURL.Replace("{X}", p.X.ToString()).Replace("{Y}", p.Y.ToString()).Replace("{Z}", z.ToString());
		}

		private static WebRequestUtility wru = new WebRequestUtility("Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
		private static byte[] DownloadImage(string url)
		{
			string[] headers = new string[]{
				"Accept", "text/html, application/xhtml+xml, image/jxr, */*"
			};
			BpWebResponse response = wru.GET(url, headers);
			if (response.StatusCode == 0)
				throw new Exception("Connection failed to " + url);
			return response.data;
		}

		private void WriteImageToCache(int z, int x, int y, byte[] data)
		{
			Directory.CreateDirectory("MapCache");
			File.WriteAllBytes(GetFileName(z, x, y), data);
		}
		private bool IsCached(int z, int x, int y)
		{
			return File.Exists(GetFileName(z, x, y));
		}
		private string GetFileName(int z, int x, int y)
		{
			return "MapCache/" + z + "_" + x + "_" + y + ".jpg";
		}

		private void btnStitchIntoOne_Click(object sender, EventArgs e)
		{
			if (BuildTileInfoLabel())
			{
				if (!string.IsNullOrWhiteSpace(preprocessError))
				{
					MessageBox.Show(preprocessError);
					return;
				}
			}
			else
			{
				MessageBox.Show("Unable to process request.  Please check input.");
				return;
			}
			int threadCount = Math.Max(1, Math.Min(10, (int)nudStitchThreads.Value));
			int z = (int)nudZoom.Value;
			Point start = Util.LatLonToTileCoordinate((double)nudStartLat.Value, (double)nudStartLon.Value, (int)nudZoom.Value);
			Point end = Util.LatLonToTileCoordinate((double)nudEndLat.Value, (double)nudEndLon.Value, (int)nudZoom.Value);
			long tilesWide = ((end.X - (long)start.X) + 1);
			long tilesTall = ((end.Y - (long)start.Y) + 1);
			long totalChunks = tilesWide * tilesTall;
			Func<object> doWork = () =>
			{
				bwWorker.ReportProgress(0, "Finding images...");
				ConcurrentQueue<Point> imagesToLoad = new ConcurrentQueue<Point>();
				for (int y = start.Y; y <= end.Y; y++)
				{
					for (int x = start.X; x <= end.X; x++)
					{
						if (IsCached(z, x, y))
							imagesToLoad.Enqueue(new Point(x, y));
						else
							return "Download images before attempting to stitch!";
					}
				}
				if (imagesToLoad.Count == 0)
					return "No images!";
				Size chunkSize = new Size();
				if (imagesToLoad.TryPeek(out Point p1))
				{
					string path = GetFileName(z, p1.X, p1.Y);
					byte[] compressed = File.ReadAllBytes(path);
					byte[] raw = ImageCodec.FactoryNew(compressed).Decode(compressed, out int w, out int h);
					chunkSize.Width = w;
					chunkSize.Height = h;
				}
				int stride = (int)(3 * tilesWide * chunkSize.Width);
				byte[] stitchData = new byte[stride * (tilesTall * chunkSize.Height)];
				List<Thread> threads = new List<Thread>();
				for (int i = 0; i < threadCount; i++)
				{
					Thread thr = new Thread(() =>
					{
						try
						{
							using (turbojpegCLI.TJDecompressor dec = new turbojpegCLI.TJDecompressor())
							{
								byte[] raw = null;
								double totalImages = imagesToLoad.Count;
								while (imagesToLoad.TryDequeue(out Point p))
								{
									string path = GetFileName(z, p.X, p.Y);
									try
									{
										string error = MergeImage(p, path, stitchData, dec, ref raw, chunkSize, start, stride);
										int remaining = imagesToLoad.Count;
										bwWorker.ReportProgress((int)Math.Round((remaining / totalImages) * 100), error != null ? error : (remaining + " images remaining to load."));
									}
									catch (ThreadAbortException) { }
									catch (Exception ex)
									{
										Logger.Debug(ex, path);
										bwWorker.ReportProgress(0, path + " Error: " + ex.Message);
										return;
									}
								}
							}
						}
						catch (ThreadAbortException) { }
						catch (Exception ex)
						{
							Logger.Debug(ex);
							MessageBox.Show(ex.ToString());
						}
					});
					thr.Name = "Stitch Thread " + i;
					thr.IsBackground = true;
					thr.Start();
					threads.Add(thr);
				}
				while (!bwWorker.CancellationPending && imagesToLoad.Count > 0)
				{
					Thread.Sleep(33);
				}
				foreach (Thread thr in threads)
				{
					if (bwWorker.CancellationPending)
						thr.Abort();
					thr.Join();
				}
				bwWorker.ReportProgress(100, "Compressing stitched image");
				string outPath = z + "_" + start.X + "x" + start.Y + "_" + end.X + "x" + end.Y + ".jpg";
				WriteCompressedImage(outPath, stitchData, (int)tilesWide * chunkSize.Width, (int)tilesTall * chunkSize.Height);
				return new StitchingFinished(outPath);
			};
			InitializeBackgroundWorker();
			bwWorker.RunWorkerAsync(doWork);
		}

		private byte[] MergeImages(List<Point> tiles, int zoom, out int tilesWide, out int tilesTall, out Size chunkSize)
		{
			chunkSize = new Size(256, 256);
			if (tiles.Count == 4)
				tilesWide = tilesTall = 2;
			else if (tiles.Count == 1)
				tilesWide = tilesTall = 1;
			else
			{
				tilesWide = tilesTall = 0;
				Logger.Debug("MergeImages called with " + tiles.Count + " tiles. Requires 4 or 1.");
				return null;
			}
			int stride = (int)(3 * tilesWide * chunkSize.Width);
			byte[] stitchData = new byte[stride * (tilesTall * chunkSize.Height)];
			using (turbojpegCLI.TJDecompressor dec = new turbojpegCLI.TJDecompressor())
			{
				byte[] raw = null;
				foreach (Point tile in tiles)
				{
					string path = GetFileName(zoom, tile.X, tile.Y);
					try
					{
						string error = MergeImage(tile, path, stitchData, dec, ref raw, chunkSize, tiles[0], stride);
					}
					catch (ThreadAbortException) { }
					catch (Exception ex)
					{
						Logger.Debug(ex, path);
						return null;
					}
				}
			}
			return stitchData;
		}
		/// <summary>
		/// Merges one tile into a larger image.
		/// </summary>
		/// <param name="tile">The tile to merge into a larger image.</param>
		/// <param name="zoom">The zoom level of the tile.</param>
		/// <param name="dest">The byte array that is the raw larger image.</param>
		/// <param name="chunkSize">The required dimensions of a chunk.</param>
		/// <param name="startTile">The upper-left tile (for calculating the correct position in the [dest] array).</param>
		/// <param name="stride">The number of bytes used by each line of pixels in the [dest] array.</param>
		private string MergeImage(Point tile, string path, byte[] dest, turbojpegCLI.TJDecompressor dec, ref byte[] raw, Size chunkSize, Point upperLeftTile, int stride)
		{
			string error = null;
			byte[] data = File.ReadAllBytes(path);
			ImageCodec imgCodec = ImageCodec.FactoryNew(data);
			int w, h;
			if (imgCodec is JpegCodec)
				((JpegCodec)imgCodec).Decode(data, out w, out h, dec, ref raw);
			else
				raw = imgCodec.Decode(data, out w, out h);
			int rawW = w * 3;
			if (chunkSize.Width != w || chunkSize.Height != h)
			{
				error = path + ": " + "Image chunk size \"" + w + "x" + h + "\" does not match the previous chunk size \"" + chunkSize.Width + "x" + chunkSize.Height + "\"";
				Logger.Info(error);
				w = Math.Min(w, chunkSize.Width);
				h = Math.Min(h, chunkSize.Height);
			}
			// Copy this image data into the destination array
			int yRel = tile.Y - upperLeftTile.Y;
			int xRel = tile.X - upperLeftTile.X;
			int offset1 = yRel * stride * chunkSize.Height;
			for (int y = 0; y < h; y++)
			{
				int offset2 = offset1 + (y * stride) + (3 * xRel * chunkSize.Width);
				int rawOffset = (rawW * y);
				for (int x = 0; x < w * 3; x++)
				{
					dest[offset2 + x] = raw[rawOffset + x];
				}
			}
			return error;
		}


		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			bwWorker?.CancelAsync();
		}

		private void txtBaseUrl_TextChanged(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void nudStartLat_ValueChanged(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void nudStartLon_ValueChanged(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void nudEndLat_ValueChanged(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void nudEndLon_ValueChanged(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void nudZoom_ValueChanged(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void nudDownloadThreads_ValueChanged(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void nudStitchThreads_ValueChanged(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void nudOutputQuality_ValueChanged(object sender, EventArgs e)
		{
			SaveSettings();
		}
	}
}
