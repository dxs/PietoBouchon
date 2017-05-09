using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PietoBouchon.Utilities
{
	public static class CNST
	{
		public static double Radius = 10;
		public static double CONTACT = 1e-16;
		public static double Velocity = 1;
		public static int NBCreate = 5;
		public static int CLOCK = 5;

		public static double EPSIL = 1e-5;
		public static double EPSILDIRECTION = 1e-2;

		public static Random Random = new Random();
		public static double PietonId = 0;
		public static double WallId = 0;
		public static double ProjId = 0;
		public static double AbsoId = 0;
	}
}
