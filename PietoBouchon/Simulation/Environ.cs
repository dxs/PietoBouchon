using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PietoBouchon.Utilities;
using Windows.Foundation;

namespace PietoBouchon.Simulation
{
	/// <summary>
	/// This class represent the environment of the simulation. 
	/// </summary>
	public class Environ
	{

		public double ViewWidth { get; set; }
		public double ViewHeight { get; set; }
		public double Zoom { get; set; }
		public double ViewBorderLeft { get; private set; }
		public double ViewBorderRight { get; private set; }
		public double ViewBorderTop { get; private set; }
		public double ViewBorderBotton { get; private set; }
		public Point Center{get;set;}

		public Environ()
		{
			Zoom = 1;
			ViewWidth = 5000;
			ViewHeight = 5000;
			SetNewBorder(new Point(0,0));
		}

		public Environ(double _width, double _height)
		{
			Zoom = 1;
			ViewWidth = _width;
			ViewHeight = _height;
			SetNewBorder(new Point(0, 0));
		}

		private void SetNewBorder(Point _center)
		{
			Center = _center;
			ViewBorderLeft = Center.X - ViewWidth / 2;
			ViewBorderRight = Center.X + ViewWidth / 2;
			ViewBorderTop = Center.Y - ViewHeight / 2;
			ViewBorderBotton = Center.Y + ViewHeight / 2;
		}

		internal Coordinate PositionCorrection(Coordinate position)
		{
			position.X += this.ViewWidth / 2 + Center.X;
			position.Y += this.ViewHeight / 2 + Center.Y;
			return position;
		}
	}
}
