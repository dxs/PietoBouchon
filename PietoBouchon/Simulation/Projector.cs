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
	public class Projector
	{
		/// <summary>
		/// Id of the projector
		/// </summary>
		public double Id { get; set; }

		/// <summary>
		/// Number of pieton to create
		/// </summary>
		public int PietonToCreate { get; set; }

		/// <summary>
		/// Time to delivery all the pietons, in seconds
		/// </summary>
		public double DeliveryTime { get; set; }

		/// <summary>
		/// Position of the Projector
		/// </summary>
		public Coordinate Position { get; set; }

		/// <summary>
		/// Frequency of pietons creation
		/// </summary>
		public double Rate { get; private set; }

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
				Pieton p = new Pieton()
				{
					Direction = this.Angle,
					Velocity = CNST.Velocity,
					Position = new Coordinate() { X = Draw.Width - this.Position.X, Y = (i * Draw.Height / CNST.NBCreate / 2) + this.Position.Y}
				};
				list.Add(p);
			}
			return list;
		}

		public void ComputeTimeToCreate(bool linear = true)
		{
			Rate = PietonToCreate - DeliveryTime;
		}
	}
}
