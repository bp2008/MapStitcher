using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPUtil;

namespace MapStitcher
{
	public class Settings : SerializableObjectBase
	{
		public string BaseURL = "http://khm0.googleapis.com/kh?v=800&hl=en-US&x={X}&y={Y}&z={Z}";
		public decimal startLat = 47.6215M;
		public decimal startLon = -122.352M;
		public decimal endLat = 47.6195M;
		public decimal endLon = -122.347M;
		public int zoom = 20;
		public int downloadThreads = 2;
		public int stitchThreads = 4;
	}
}
