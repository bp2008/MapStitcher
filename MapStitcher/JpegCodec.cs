using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStitcher
{
	/// <summary>
	/// Uses libjpeg-turbo to read and write jpeg images.  Uses BGR pixel format by default, because Microsoft's "RGB" is actually "BGR".
	/// </summary>
	public class JpegCodec : ImageCodec
	{
		public override byte[] Decode(byte[] compressed, out int width, out int height)
		{
			using (turbojpegCLI.TJDecompressor dec = new turbojpegCLI.TJDecompressor())
			{
				dec.setSourceImage(compressed, compressed.Length);
				width = dec.getWidth();
				height = dec.getHeight();
				return dec.decompress(turbojpegCLI.PixelFormat.BGR, turbojpegCLI.Flag.NONE);
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
			dec.setSourceImage(compressed, compressed.Length);
			width = dec.getWidth();
			height = dec.getHeight();
			if (raw == null)
				raw = dec.decompress(turbojpegCLI.PixelFormat.BGR, turbojpegCLI.Flag.NONE);
			else
				dec.decompress(raw, turbojpegCLI.PixelFormat.BGR, turbojpegCLI.Flag.NONE);
		}

		public override byte[] Encode(byte[] rgb, int width, int height)
		{
			return Encode(rgb, width, height, turbojpegCLI.SubsamplingOption.SAMP_420, 80, turbojpegCLI.PixelFormat.BGR);
		}

		public byte[] Encode(byte[] rgb, int width, int height, turbojpegCLI.SubsamplingOption subsamplingOption, int quality, turbojpegCLI.PixelFormat pixelFormat)
		{
			using (turbojpegCLI.TJCompressor comp = new turbojpegCLI.TJCompressor(rgb, width, height, pixelFormat))
			{
				comp.setSubsamp(subsamplingOption);
				comp.setJPEGQuality(quality);
				return comp.compressToExactSize();
			}
		}
	}
}
