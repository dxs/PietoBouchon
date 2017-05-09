using PietoBouchon.Simulation;
using PietoBouchon.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PietoBouchon
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		List<Pieton> pietons = new List<Pieton>();
		List<Projector> projectors = new List<Projector>();
		List<Absorbeur> absorbeurs = new List<Absorbeur>();
		List<Wall> walls = new List<Wall>();
		Environnement _Environnement;
		DispatcherTimer time = new DispatcherTimer();
		DispatcherTimer timeProjectors = new DispatcherTimer();
		List<Gradient> Parcours;

		public MainPage()
        {
			time.Interval = new TimeSpan(0, 0, 0, 0, CNST.CLOCK);
			time.Tick += Time_Tick;
			timeProjectors.Interval = new TimeSpan(0, 0, 1);
			timeProjectors.Tick += Projectors_Tick;
			this.InitializeComponent();
			Setup();
		}

		private void Setup()
		{
			projectors.Add(new Projector(10, 50)
			{
				Position = new Coordinate() { X = -50, Y = 100 },
			});
			absorbeurs.Add(new Absorbeur(10, 50)
			{
				Position = new Coordinate() { X = 200, Y = -50 },
			});
			walls.Add(new Wall(new Coordinate() { X = 0, Y = 0 }, new Coordinate() { X = 140, Y = 30 }));
		}

		private void Load_Click(object sender, RoutedEventArgs e)
		{
			timeProjectors.Stop();
			time.Stop();
			StartButton.Content = "Start";
			SimulationCanvas.Children.Clear();
			foreach (Pieton p in pietons)
			{
				Coordinate newCoord = _Environnement.ConvertSimToReal(p.Position);
				Ellipse ellipse = p.Draw;
				Canvas.SetLeft(ellipse, newCoord.X);
				Canvas.SetTop(ellipse, newCoord.Y);
				SimulationCanvas.Children.Add(ellipse);
			}

			foreach(Projector p in projectors)
			{
				Coordinate newCoord = _Environnement.ConvertSimToReal(p.Position);
				Rectangle rect = p.Draw;
				Canvas.SetLeft(rect, newCoord.X);
				Canvas.SetTop(rect, newCoord.Y);
				SimulationCanvas.Children.Add(rect);
			}
			foreach(Absorbeur a in absorbeurs)
			{
				Coordinate newCoord = _Environnement.ConvertSimToReal(a.Position);
				Rectangle rect = a.Draw;
				Canvas.SetLeft(rect, newCoord.X);
				Canvas.SetTop(rect, newCoord.Y);
				SimulationCanvas.Children.Add(rect);
			}
			foreach(Wall w in walls)
			{
				Coordinate tmpStart = _Environnement.ConvertSimToReal(w.StartPoint);
				Coordinate tmpEnd = _Environnement.ConvertSimToReal(w.EndPoint);
				w.Line.Points = new PointCollection() { new Point(tmpStart.X, tmpStart.Y), new Point(tmpEnd.X, tmpEnd.Y) };
				SimulationCanvas.Children.Add(w.Line);
			}
			GenerateGradient();
		}

		private void GenerateGradient()
		{
			if (Parcours == null)
				Parcours = new List<Gradient>();
			Parcours.Add(new Gradient(projectors[0].Position, absorbeurs[0].Position));
		}

		private void Start_Click(object sender, RoutedEventArgs e)
		{
			if ((sender as Button).Content.ToString() == "Start")
			{
				time.Start();
				timeProjectors.Start();
				(sender as Button).Content = "Stop";
			}
			else
			{
				time.Stop();
				timeProjectors.Stop();
				(sender as Button).Content = "Start";
			}
			
		}

		private void Projectors_Tick(object sender, object e)
		{
			timeProjectors.Stop();
			foreach (Projector p in projectors)
			{
				List<Pieton> list = p.CreatePieton();
				foreach (Pieton pi in list)
				{
					LoadPieton(pi);
					pietons.Add(pi);
				}
			}
		}

		private void Time_Tick(object sender, object e)
		{
			Coordinate old = new Coordinate() { X = 0, Y = 0 };
			List<Pieton> ToDelete = new List<Pieton>();

			foreach (Pieton p in pietons)
			{
				double FirstDirection = p.Direction;
				p.MoveGradient(Parcours[0]);

				if (ChekcIfPietonInStep(p))
					continue;
				p.Direction = CheckWall(p);
				FirstDirection = p.Direction;

				old.X = p.Position.X;
				old.Y = p.Position.Y;
				p.Position = p.ComputeNewPosition(p.Position);
				try
				{
					p.Trans.X += p.Position.X - old.X;
					p.Trans.Y += p.Position.Y - old.Y;
				}
				catch(Exception ex)
				{
					Debug.WriteLine(ex.Message);
				}
				if (!CheckPieton(p))
					ToDelete.Add(p);
			}
			foreach (Pieton item in ToDelete)
			{
				var piet = SimulationCanvas.FindName(item.Id.ToString()) as Ellipse;
				SimulationCanvas.Children.Remove(piet);
				pietons.Remove(item);
			}
		}

		private bool ChekcIfPietonInStep(Pieton piet)
		{
			foreach(Pieton p in pietons)
			{
				if (p.IsWaiting)
					continue;
				if (p == piet)
					continue;
				Coordinate newPos = piet.ComputeNewPosition(piet.Position);
				if (Coordinate.Distance(newPos, p.Position) > 2 * CNST.Radius)
					continue;
				piet.IsWaiting = true;
				return true;//there is a pieton in step
			}
			piet.IsWaiting = false;
			return false; //Clear
		}

		private double CheckWall(Pieton p)
		{
			foreach (Wall w in walls)
			{
				double di = w.WallCheck(p);
				if (di != p.Direction)
					return di;
			}
			return p.Direction;
		}

		private bool CheckPieton(Pieton p)
		{
			foreach (Absorbeur a in absorbeurs)
				if (a.TouchPieton(p.Position))
					foreach (var child in SimulationCanvas.Children)
						if (child == p.Draw)
							return false;
			return true;
		}

		private void LoadPieton(Pieton p)
		{
			Coordinate newCoord = _Environnement.ConvertSimToReal(p.Position);
			Ellipse ellipse = p.Draw;
			Canvas.SetLeft(ellipse, newCoord.X);
			Canvas.SetTop(ellipse, newCoord.Y);
			SimulationCanvas.Children.Add(ellipse);
		}

		private void SimulationCanvas_Loaded(object sender, RoutedEventArgs e)
		{
			_Environnement = new Environnement((sender as Canvas).ActualWidth, (sender as Canvas).ActualHeight);
		}
	}
}
