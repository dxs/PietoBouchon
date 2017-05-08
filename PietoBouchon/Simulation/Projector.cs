using PietoBouchon.Utilities;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace PietoBouchon.Simulation
{
	public class Projector
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


		public Projector(double width, double height, double _angle = 0)
		{
			Angle = _angle;
			Draw = new Rectangle();
			Draw.Width = width; Draw.Height = height;
			Draw.Fill = new SolidColorBrush(Colors.DarkSeaGreen);
		}

		public List<Pieton> CreatePieton()
		{
			List<Pieton> list = new List<Pieton>();
			for(int i = 0; i < CNST.NBCreate; i++)
			{
				Pieton p = new Pieton(CNST.PietonId++)
				{
					Direction = this.Angle,
					Position = new Coordinate { X = Draw.Width + this.Position.X, Y = (i * Draw.Height / CNST.NBCreate) + this.Position.Y}
				};
				list.Add(p);
			}
			return list;
		}
	}
}
