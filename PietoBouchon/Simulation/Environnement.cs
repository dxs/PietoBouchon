using PietoBouchon.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PietoBouchon.Simulation
{
	public class Environnement
	{
		public int SimWidth { get; set; }
		public int SimHeight { get; set; }
		public double RealWidth { get; set; }
		public double RealHeight { get; set; }

		public Environnement(double _realWidth, double _realHeight, int _SimWidth = 5000, int _SimHeight = 5000)
		{
			RealWidth = _realWidth;
			RealHeight = _realHeight;
			SimWidth = _SimWidth;
			SimHeight = _SimHeight;
		}
		
		/// <summary>
		/// Convert from real world dimension to simulation dimension
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public Coordinate ConvertRealToSim(Coordinate position)
		{
			Coordinate newPosition = new Coordinate();

			newPosition.X = position.X - RealWidth / 2;
			newPosition.Y = (position.Y - RealHeight / 2);

			return newPosition;
		}

		/// <summary>
		/// Convert from simulation dimension to real world dimension
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public Coordinate ConvertSimToReal(Coordinate position)
		{
			Coordinate newPosition = new Coordinate();

			newPosition.X = position.X + RealWidth / 2;
			newPosition.Y = (position.Y + RealHeight / 2);

			return newPosition;
		}
	}
}
