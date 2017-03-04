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

		public Projector(double width, double height)
		{
			Draw = new Rectangle();
			Draw.Width = width; Draw.Height = height;
			Draw.Fill = new SolidColorBrush(Colors.DarkSeaGreen);
		}

		public List<Pieton> CreatePieton()
		{
			List<Pieton> list = new List<Pieton>();
			for(int i = 0; i < 5; i++)
			{
				Pieton p = new Pieton();
				p.Direction = 0;
				p.Velocity = 2;
				p.Position = new Coordinate();
				p.Position.Y = i * (Draw.Height / 5);
				p.Position.X = Draw.Width;
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
