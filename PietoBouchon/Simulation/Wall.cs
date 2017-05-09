using PietoBouchon.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace PietoBouchon.Simulation
{
	public class Wall
	{
		public Coordinate StartPoint;
		public Coordinate EndPoint;
		/*Maths representation*/
		private double Slope = CNST.EPSIL;
		private double offset = CNST.EPSIL;

		public double Width = 10;
		public Polyline Line;
		public Wall(Coordinate _start, Coordinate _end)
		{
			StartPoint = _start; EndPoint = _end;
			Line = new Polyline();
			Line.Stroke = new SolidColorBrush(Windows.UI.Colors.OrangeRed);
			Line.StrokeThickness = 1;
			Line.Name = CNST.WallId++.ToString();
			ComputeSlope();
		}

		private void ComputeSlope()
		{
			double X = EndPoint.X - StartPoint.X;
			double Y = EndPoint.Y - StartPoint.Y;
			if (X == 0) X = CNST.EPSIL;
			if (Y == 0) Y = CNST.EPSIL;

			Slope = Y / X;
			//Compute offset
			offset = StartPoint.Y - Slope * StartPoint.X;
		}

		public double WallCheck(Pieton piet)
		{
			Coordinate Start = new Coordinate() { X = piet.Position.X, Y = piet.Position.Y };
			Coordinate End = piet.ComputeNewPosition(piet.Position);
			double X = End.X - Start.X;
			double Y = End.Y - Start.Y;
			if (X == 0) X = CNST.EPSIL;
			if (Y == 0) Y = CNST.EPSIL;
			double PietSlope = Y / X;
			double PietOff = Start.Y - PietSlope * Start.X;

			Coordinate shock = new Coordinate();
			/*x = (d - b) / (a - c)*/
			shock.X = (PietOff - offset) / (Slope - PietSlope);
			shock.Y = offset + Slope * shock.X;

			double distToWall = Coordinate.Distance(shock, Start);
			if (distToWall > CNST.Radius * Coordinate.Distance(End, Start))
				return piet.Direction;

			if (EndPoint.X - StartPoint.X > 0)
			{
				if (shock.X > EndPoint.X || shock.X < StartPoint.X)
					return piet.Direction;
			}
			else
			{
				if (shock.X < EndPoint.X || shock.X > StartPoint.X)
					return piet.Direction;
			}
			return Math.Tan(Slope);
		}
	}
}
