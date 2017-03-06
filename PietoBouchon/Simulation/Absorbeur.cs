using PietoBouchon.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace PietoBouchon.Simulation
{
	public class Absorbeur
	{
		/// <summary>
		/// Id of the projector
		/// </summary>
		public double Id { get; set; }


		/// <summary>
		/// Position of the Projector
		/// </summary>
		public Coordinate Position { get; set; }


		/// <summary>
		/// Rectangle representing the projector
		/// </summary>
		public Rectangle Draw { get; set; }

		/// <summary>
		/// normal's angle (from horizontal)
		/// </summary>
		public double Angle { get; set; }

		public Absorbeur(double _width, double _height, double _angle = 0)
		{
			Angle = _angle;
			Draw = new Rectangle();
			Draw.Width = _width; Draw.Height = _height;
			Draw.Fill = new SolidColorBrush(Colors.Red);
		}

		internal bool TouchPieton(Coordinate position)
		{
			bool inHeight = false;
			bool inWidth = false;
			if (position.X > this.Position.X && position.X < this.Position.X + this.Draw.Width)
				inWidth = true;
			if (position.Y > this.Position.Y && position.Y < this.Position.Y + this.Draw.Height)
				inHeight = true;
			if (inWidth && inHeight)
				return true;
			else
				return false;
		}
	}
}
