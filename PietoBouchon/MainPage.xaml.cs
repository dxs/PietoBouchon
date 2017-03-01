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
		List<Ellipse> People;
		Environ _Environnement;
		DispatcherTimer time;

		public MainPage()
        {
			time = new DispatcherTimer();
			time.Interval = new TimeSpan(0, 0, 0, 0, 50);
			time.Tick += Time_Tick;
			pietons = new List<Pieton>();
			People = new List<Ellipse>();
			pietons.Add(new Pieton() { Id = 1, Direction = 0, Position = new Coordinate() { X = 0, Y = 0 }, Velocity = 0.1 });
			this.InitializeComponent();
        }

		private void Load_Click(object sender, RoutedEventArgs e)
		{
			foreach(Pieton p in pietons)
			{
				People.Add(p.GetEllipse());
			}
			int i = 0;
			foreach (Ellipse el in People)
			{
				Coordinate newCoord = _Environnement.PositionCorrection(pietons[i].Position);
				Canvas.SetLeft(el, newCoord.X);
				Canvas.SetTop(el, newCoord.Y);
				SimulationCanvas.Children.Add(el);

				i++;
			}
		}

		private void Start_Click(object sender, RoutedEventArgs e)
		{
			if ((sender as Button).Content.ToString() == "Start")
			{
				time.Start();
				(sender as Button).Content = "Stop";
			}
			else
			{
				time.Stop();
				(sender as Button).Content = "Start";
			}
			
		}

		private void Time_Tick(object sender, object e)
		{
			int i = 0;
			Coordinate old = new Coordinate() { X = 0, Y = 0 };

			foreach (Pieton p in pietons)
			{
				old = p.Position;	
				p.ComputeNewPosition();
				TranslateTransform move = (People[i].RenderTransform as TranslateTransform);
				try
				{
					move.X = old.X - p.Position.X;
					move.Y = old.Y - p.Position.Y;
				}
				catch(Exception ex)
				{
					Debug.WriteLine(ex);
				}
				i++;
			}
		}

		private void SimulationCanvas_Loaded(object sender, RoutedEventArgs e)
		{
			_Environnement = new Environ((sender as Canvas).ActualWidth, (sender as Canvas).ActualHeight);
		}
	}
}
