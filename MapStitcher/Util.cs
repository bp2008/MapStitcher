using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.MapPoint;

namespace MapStitcher
{
	public static class Util
	{
		public static Point LatLonToTileCoordinate(double latitude, double longitude, int zoomFactor)
		{
			TileSystem.LatLongToPixelXY(latitude, longitude, zoomFactor, out int pixelX, out int pixelY);
			TileSystem.PixelXYToTileXY(pixelX, pixelY, out int tileX, out int tileY);
			return new Point(tileX, tileY);
		}
		public static LatLon TileCoordinateToLatLon(int tileX, int tileY, int zoomFactor)
		{
			if (zoomFactor < 0 || zoomFactor > 23)
				throw new ArgumentOutOfRangeException("zoomFactor", zoomFactor, "zoomFactor must be between 0 and 23");
			int tileCount = IntPow(2, zoomFactor);
			int tileMax = tileCount - 1;
			if (tileX < 0 || tileX > tileMax)
				throw new ArgumentOutOfRangeException("tileX", tileX, "With zoomFactor " + zoomFactor + ", tileX must be between 0 and " + tileMax);
			if (tileY < 0 || tileY > tileMax)
				throw new ArgumentOutOfRangeException("tileY", tileY, "With zoomFactor " + zoomFactor + ", tileY must be between 0 and " + tileMax);

			double dTileCount = tileCount;

			double lon = ((tileX / dTileCount) * 360d) - 180d;

			double n = Math.PI - (2 * ((Math.PI * tileY) / dTileCount));
			double lat = (180d / Math.PI) * Math.Atan(0.5 * (Math.Exp(n) - Math.Exp(-n)));
			return new LatLon(lat, lon);
		}
		public static RelativePixel GetRelativePixel(double lat, double lon, int zoom)
		{
			TileSystem.LatLongToPixelXY(lat, lon, zoom, out int pixelX, out int pixelY);
			TileSystem.PixelXYToTileXY(pixelX, pixelY, out int tileX, out int tileY);
			return new RelativePixel()
			{
				pixel = new Point(pixelX - (256 * tileX), pixelY - (256 * tileY)),
				tile = new Point(tileX, tileY)
			};
		}

		public static int IntPow(int a, int b)
		{
			int result = 1;
			for (int i = 0; i < b; i++)
				result *= a;
			return result;
		}
		//public static Point LatLonToTileCoordinate(double latitude, double longitude, int zoomFactor)
		//{
		//	double latLimitLeft = -180d;
		//	double latLimitRight = 180d;
		//	double lonLimitTop = 85.05;
		//	double lonLimitBottom = -85.05;
		//	int tileCount = IntPow(2, zoomFactor);
		//	int tileMax = tileCount - 1;
		//	if (latitude < -180 || latitude > 180)
		//		throw new ArgumentOutOfRangeException("latitude", latitude, "latitude must be between -180 and 180");
		//	if (longitude < -85.05 || longitude > 85.05)
		//		throw new ArgumentOutOfRangeException("longitude", longitude, "longitude must be between -85 and 85");
		//	if (zoomFactor < 0 || zoomFactor > 23)
		//		throw new ArgumentOutOfRangeException("zoomFactor", zoomFactor, "zoomFactor must be between 0 and 23");
		//	return new Point();
		//}
		public static Image ImageFromFile(string path)
		{
			byte[] data = File.ReadAllBytes(path);
			MemoryStream ms = new MemoryStream(data);
			return Image.FromStream(ms);
		}
	}
}
