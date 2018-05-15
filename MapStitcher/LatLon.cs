namespace MapStitcher
{
	public class LatLon
	{
		public double lat;
		public double lon;

		public LatLon(double lat, double lon)
		{
			this.lat = lat;
			this.lon = lon;
		}
		public override string ToString()
		{
			return lat + "° latitude, " + lon + "° longitude";
		}
	}
}
