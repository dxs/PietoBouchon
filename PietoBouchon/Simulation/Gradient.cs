using PietoBouchon.Utilities;
using System;

namespace PietoBouchon.Simulation
{
	public class Gradient
	{
		public double Direction { get; set; }
		public double Intensity { get; set; }

		public Gradient(Coordinate start, Coordinate end)
		{
			Direction = Math.Atan2(end.Y - start.Y, end.X - start.X);
			Intensity = 10;
		}
	}
}
