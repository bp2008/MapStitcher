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
	public class PngCodec : ImageCodec
	{
		public override byte[] Decode(byte[] compressed, out int width, out int height)
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

		public override byte[] Encode(byte[] rgb, int width, int height)
		{
			using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb))
			{
				BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
				Marshal.Copy(rgb, 0, bitmapData.Scan0, rgb.Length);
				bmp.UnlockBits(bitmapData);
				using (MemoryStream ms = new MemoryStream())
				{
					bmp.Save(ms, ImageFormat.Png);
					return ms.ToArray();
				}
			}
		}
	}
}
