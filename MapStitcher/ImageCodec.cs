using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStitcher
{
	public abstract class ImageCodec
	{
		public abstract byte[] Decode(byte[] compressed, out int width, out int height);
		public abstract byte[] Encode(byte[] rgb, int width, int height);

		public static ImageCodec FactoryNew(byte[] compressedImgData)
		{
			ImageFormat format = GetFormat(compressedImgData);
			if (format == ImageFormat.Jpeg)
				return new JpegCodec();
			else if (format == ImageFormat.Png)
				return new PngCodec();
			throw new Exception("Unsupported image format: " + format);
		}
		#region Image Format Identification
		private static Dictionary<byte[], ImageFormat> imageFormatIdentificationMap = new Dictionary<byte[], ImageFormat>()
		{
			{ new byte[] { 0x42, 0x4D }, ImageFormat.Bmp },
			{ new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, ImageFormat.Gif },
			{ new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }, ImageFormat.Gif },
			{ new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, ImageFormat.Png},
			{ new byte[] { 0xff, 0xd8 }, ImageFormat.Jpeg },
		};
		public static ImageFormat GetFormat(byte[] imageData)
		{
			return imageFormatIdentificationMap.FirstOrDefault(kvp => StartsWith(imageData, kvp.Key)).Value;
		}
		private static bool StartsWith(byte[] a, byte[] b)
		{
			if (a == null || a.Length < b.Length)
				return false;
			for (int i = 0; i < b.Length; i += 1)
				if (a[i] != b[i])
					return false;
			return true;
		}
		#endregion
	}
}
