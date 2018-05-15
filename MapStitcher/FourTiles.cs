using System.Collections.Generic;
using System.Drawing;

namespace MapStitcher
{
	public class FourTiles
	{
		public List<Point> Tiles;
		public int Zoom;
		public FourTiles(Point firstPoint, int zoom)
		{
			Tiles = new List<Point>();
			Tiles.Add(new Point(firstPoint.X, firstPoint.Y));
			if (zoom > 0)
			{
				Tiles.Add(new Point(firstPoint.X + 1, firstPoint.Y));
				Tiles.Add(new Point(firstPoint.X, firstPoint.Y + 1));
				Tiles.Add(new Point(firstPoint.X + 1, firstPoint.Y + 1));
			}
			Zoom = zoom;
		}
	}
}