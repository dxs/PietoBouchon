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
			Line.Points = new PointCollection() { new Point(StartPoint.X, StartPoint.Y), new Point(EndPoint.X, EndPoint.Y) };
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
			offset = Slope * StartPoint.X - StartPoint.Y;
		}

		public void WallCheck(Pieton piet)
		{
			Coordinate End = piet.ComputeNewPosition(piet.Position);
			double X = End.X - piet.Position.X;
			double Y = End.Y - piet.Position.Y;
			if (X == 0) X = CNST.EPSIL;
			if (Y == 0) Y = CNST.EPSIL;
			double PietSlope = Y / X;
			double PietOff = PietSlope * piet.Position.X - piet.Position.Y;

			Coordinate shock = new Coordinate();
			/*x = (d - b) / (a - c)*/
			shock.X = (PietOff - offset) / (Slope - PietSlope);
			shock.Y = offset + Slope * shock.X;
			double 
		}
	}
}
