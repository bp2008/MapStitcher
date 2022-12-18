using BPUtil;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MapStitcher
{
	/// <summary>
	/// Uses libjpeg-turbo to read and write jpeg images.  Uses BGR pixel format by default, because Microsoft's "RGB" is actually "BGR".
	/// </summary>
	public class JpegCodec : ImageCodec
	{
		private static bool useFrameworkCodec = false;
		public override byte[] Decode(byte[] compressed, out int width, out int height)
		{
			if (useFrameworkCodec)
			{
				using (MemoryStream ms = new MemoryStream(compressed))
				{
					using (Bitmap bmp = (Bitmap)Bitmap.FromStream(ms))
					{
						width = bmp.Width;
						height = bmp.Height;
						BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
						byte[] data = new byte[Math.Abs(bitmapData.Stride * bitmapData.Height)];
						Marshal.Copy(bitmapData.Scan0, data, 0, data.Length);
						bmp.UnlockBits(bitmapData);
						return data;
					}
				}
			}
			else
			{
				try
				{
					using (turbojpegCLI.TJDecompressor dec = new turbojpegCLI.TJDecompressor())
					{
						dec.setSourceImage(compressed, compressed.Length);
						width = dec.getWidth();
						height = dec.getHeight();
						return dec.decompress(turbojpegCLI.PixelFormat.BGR, turbojpegCLI.Flag.NONE);
					}
				}
				catch (Exception ex)
				{
					useFrameworkCodec = true;
					Logger.Debug(ex, "turbojpegCLI decoder failed. Falling back to .NET framework decoder.");
					return Decode(compressed, out width, out height);
				}
			}
		}

		/// <summary>
		/// A more-efficient overload that resuses a TJDecompressor instance and byte array.
		/// </summary>
		/// <param name="compressed"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="dec"></param>
		/// <param name="raw">May be null</param>
		public void Decode(byte[] compressed, out int width, out int height, turbojpegCLI.TJDecompressor dec, ref byte[] raw)
		{
			if (useFrameworkCodec)
			{
				raw = Decode(compressed, out width, out height);
			}
			else
			{
				try
				{
					dec.setSourceImage(compressed, compressed.Length);
					width = dec.getWidth();
					height = dec.getHeight();
					if (raw == null)
						raw = dec.decompress(turbojpegCLI.PixelFormat.BGR, turbojpegCLI.Flag.NONE);
					else
						dec.decompress(raw, turbojpegCLI.PixelFormat.BGR, turbojpegCLI.Flag.NONE);
				}
				catch (Exception ex)
				{
					useFrameworkCodec = true;
					Logger.Debug(ex, "turbojpegCLI decoder failed. Falling back to .NET framework decoder.");
					Decode(compressed, out width, out height, dec, ref raw);
				}
			}
		}

		public override byte[] Encode(byte[] rgb, int width, int height)
		{
			return Encode(rgb, width, height, turbojpegCLI.SubsamplingOption.SAMP_420, 80, turbojpegCLI.PixelFormat.BGR);
		}

		public byte[] Encode(byte[] rgb, int width, int height, turbojpegCLI.SubsamplingOption subsamplingOption, int quality, turbojpegCLI.PixelFormat pixelFormat)
		{
			if (useFrameworkCodec)
			{
				ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
				EncoderParameters myEncoderParameters = new EncoderParameters(1);
				myEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

				using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb))
				{
					BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
					Marshal.Copy(rgb, 0, bitmapData.Scan0, rgb.Length);
					bmp.UnlockBits(bitmapData);
					using (MemoryStream ms = new MemoryStream())
					{
						bmp.Save(ms, jpgEncoder, myEncoderParameters);
						return ms.ToArray();
					}
				}
			}
			else
			{
				try
				{
					using (turbojpegCLI.TJCompressor comp = new turbojpegCLI.TJCompressor(rgb, width, height, pixelFormat))
					{
						comp.setSubsamp(subsamplingOption);
						comp.setJPEGQuality(quality);
						return comp.compressToExactSize();
					}
				}
				catch (Exception ex)
				{
					useFrameworkCodec = true;
					Logger.Debug(ex, "turbojpegCLI encoder failed. Falling back to .NET framework encoder.");
					return Encode(rgb, width, height, subsamplingOption, quality, pixelFormat);
				}
			}
		}
		private ImageCodecInfo GetEncoder(ImageFormat format)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.FormatID == format.Guid)
				{
					return codec;
				}
			}
			return null;
		}
	}
}
