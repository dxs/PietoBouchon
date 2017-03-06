using PietoBouchon.Simulation;
using PietoBouchon.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
		List<Pieton> pietons;
		List<Projector> projectors;
		List<Absorbeur> absorbeurs;
		Environ _Environnement;
		DispatcherTimer time;
		DispatcherTimer timeProjectors;

		public MainPage()
        {
			time = new DispatcherTimer();
			time.Interval = new TimeSpan(0, 0, 0, 0, 5);
			time.Tick += Time_Tick;
			timeProjectors = new DispatcherTimer();
			timeProjectors.Interval = new TimeSpan(0, 0, 1);
			timeProjectors.Tick += Projectors_Tick;
			pietons = new List<Pieton>();
			projectors = new List<Projector>();
			absorbeurs = new List<Absorbeur>();
			Setup();
			this.InitializeComponent();
        }

		private void Setup()
		{
			projectors.Add(new Projector(10, 50)
			{
				Position = new Coordinate() { X = -250, Y = 0 },
				PietonToCreate = 100,
			});
			absorbeurs.Add(new Absorbeur(10, 50)
			{
				Position = new Coordinate() { X = +250, Y = 0}
			});
			//pietons.Add(new Pieton()
			//{
			//	Position = new Coordinate() { X = 100, Y = -100 },
			//	Direction = 0.4,
			//	Velocity = CNST.Velocity
			//});
		}

		private void Load_Click(object sender, RoutedEventArgs e)
		{
			timeProjectors.Stop();
			time.Stop();
			StartButton.Content = "Start";
			SimulationCanvas.Children.Clear();
			foreach (Pieton p in pietons)
			{
				Coordinate newCoord = _Environnement.PositionCorrection(p.Position);
				Ellipse ellipse = p.Draw;
				Canvas.SetLeft(ellipse, newCoord.X);
				Canvas.SetTop(ellipse, newCoord.Y);
				SimulationCanvas.Children.Add(ellipse);
			}

			foreach(Projector p in projectors)
			{
				Coordinate newCoord = _Environnement.PositionCorrection(p.Position);
				Rectangle rect = p.Draw;
				Canvas.SetLeft(rect, newCoord.X);
				Canvas.SetTop(rect, newCoord.Y);
				SimulationCanvas.Children.Add(rect);
			}

			foreach (Absorbeur a in absorbeurs)
			{
				Coordinate newCoord = _Environnement.PositionCorrection(a.Position);
				Rectangle rect = a.Draw;
				Canvas.SetLeft(rect, newCoord.X);
				Canvas.SetTop(rect, newCoord.Y);
				SimulationCanvas.Children.Add(rect);
			}
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
			NbPeopleInSimulation.Text = pietons.Count.ToString();
			foreach (Pieton p in pietons)
			{
				p.MoveRandomly();
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
				CheckPieton(p);
			}
		}

		private void CheckPieton(Pieton p)
		{
			foreach (Absorbeur a in absorbeurs)
				if (a.TouchPieton(p.Position))
				{
					foreach (var child in SimulationCanvas.Children)
						if (child == p.Draw)
						{
							absorbeurs.Remove(a);
							break;
						}
				}
		}

		private void LoadPieton(Pieton p)
		{
			Coordinate newCoord = _Environnement.PositionCorrection(p.Position);
			Ellipse ellipse = p.Draw;
			Canvas.SetLeft(ellipse, newCoord.X);
			Canvas.SetTop(ellipse, newCoord.Y);
			SimulationCanvas.Children.Add(ellipse);
		}

		private void SimulationCanvas_Loaded(object sender, RoutedEventArgs e)
		{
			_Environnement = new Environ((sender as Canvas).ActualWidth, (sender as Canvas).ActualHeight);
		}
	}
}
