using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PietoBouchon.Utilities
{
	public class Coordinate
	{
		public double X { get; set; }
		public double Y { get; set; }
		public Coordinate()
		{

		}

		public static double Distance(Coordinate A, Coordinate B)
		{
			double dX = Math.Abs(B.X - A.X);
			double dY = Math.Abs(B.Y - A.Y);
			return Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));
		}
	}
}
