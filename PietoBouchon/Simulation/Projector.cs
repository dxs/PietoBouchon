using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		/// Frequency of pietons creation
		/// </summary>
		public double Rate { get; private set; }

		public Projector()
		{

		}

		public void ComputeTimeToCreate(bool linear = true)
		{
			Rate = PietonToCreate - DeliveryTime;
		}
	}
}
