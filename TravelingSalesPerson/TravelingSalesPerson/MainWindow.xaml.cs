using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Diagnostics;

namespace TravelingSalesPerson
{
    public partial class MainWindow : Window
    {
        private string tspFileName;
        private List<Point> tspPoints;
        
        public MainWindow()
        {
            InitializeComponent();
            tspFileName = "";
            hideRunTime();
            tspPoints = new List<Point>();
        }

        //Slightly modified for my purposes from: https://stackoverflow.com/questions/10315188/open-file-dialog-and-select-a-file-using-wpf-controls-and-c-sharp
        private void btnFileUpload_Click(object sender, RoutedEventArgs e)
        {
            emptyCanvas();
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = ".tsp";
            dlg.Filter = "TSP Files|*.tsp";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string fileName = dlg.FileName;
                Debug.WriteLine(fileName);
                this.tspFileName = fileName;
                populatePoints();                
            }

            hideRunTime();
        }

        public void displayRunTime()
        {
            this.lblRunTime.Visibility = Visibility.Visible;
            this.UpdateLayout();
        }

        public void hideRunTime()
        {
            this.lblRunTime.Visibility = Visibility.Hidden;
            this.UpdateLayout();
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Implement solving code
            //After solving code runs, then display run time
            displayRunTime();
        }

        private void populatePoints()
        {
            List<Point> newPoints = new List<Point>();
            using (StreamReader stream = new StreamReader(tspFileName))
            {
                string fileLine;
                bool coordinates = false;

                while ((fileLine = stream.ReadLine()) != null)
                {
                    string[] parts = fileLine.Split(' ');
                    if (coordinates)
                    {
                        if (parts.Length >= 3)
                        {
                            Point newPoint = new Point(Convert.ToDouble(parts[1]), Convert.ToDouble(parts[2]));
                            tspPoints.Add(newPoint);
                            newPoints.Add(newPoint);
                            Debug.WriteLine(newPoint);
                        }
                    }
                    else if (fileLine == "NODE_COORD_SECTION")
                    {
                        coordinates = true;
                    }
                }
            }
            plotPoints(newPoints);
        }

        private void plotPoints(List<Point> points)
        {
            // Create tab
            TabItem tab = new TabItem();
            tab.Header = System.IO.Path.GetFileNameWithoutExtension(this.tspFileName);

            int city = 1; //we start at the first city
            TSP tsp = new TSP(points);

            // Create grid
            Grid grid = new Grid();

            // Create viewbox
            Viewbox viewbox = new Viewbox();
            viewbox.HorizontalAlignment = HorizontalAlignment.Stretch;
            viewbox.VerticalAlignment = VerticalAlignment.Stretch;

            // Create canvas for drawing points on
            Canvas canvas = new Canvas();

            foreach (Point point in points)
            {
                Debug.WriteLine("City: " + city);
                Ellipse ellipse = new Ellipse();
                ellipse.Width = 4;
                ellipse.Height = 4;
                ellipse.Fill = Brushes.Red;
                ellipse.Stroke = Brushes.Black;

                ellipse.ToolTip = "(" + point.X + "," + point.Y + ")";
                Label cityLabel = new Label();
                cityLabel.Content = city;
                cityLabel.Foreground = Brushes.White;
                cityLabel.ToolTip = ellipse.ToolTip;

                if (city > 9)
                {
                    cityLabel.FontSize = 1.5;
                    cityLabel.Padding = new Thickness(1.1, 0.9, 0, 0);
                }
                else
                {
                    cityLabel.FontSize = 2;
                    cityLabel.Padding = new Thickness(1.4, 0.6, 0, 0);
                }

                // Position point on canvas
                Canvas.SetLeft(ellipse, point.X + tsp.canvasOffset.X);
                Canvas.SetTop(ellipse, point.Y + tsp.canvasOffset.Y);

                // Position point label on canvas
                Canvas.SetLeft(cityLabel, point.X + tsp.canvasOffset.X);
                Canvas.SetTop(cityLabel, point.Y + tsp.canvasOffset.Y);

                canvas.Children.Add(ellipse);
                canvas.Children.Add(cityLabel);

                city++;
            }

            canvas.Height = tsp.maxPoint.Y - tsp.minPoint.Y + 80;
            canvas.Width = tsp.maxPoint.X - tsp.minPoint.X + 80;
            Debug.WriteLine(canvas.Height);
            Debug.WriteLine(canvas.Width);

            // Add canvas to viewbox
            viewbox.Child = canvas;

            // Add viewbox to grid
            grid.Children.Add(viewbox);

            // Add grid to tab
            tab.Content = grid;

            // Add tab to tabs control
            tabs.Items.Add(tab);

            Debug.WriteLine(grid.Children[0]);
            //Debug.WriteLine(this.mainViewBox.Child);

            //this.UpdateLayout();
            Debug.WriteLine("Finished populating points");
        }

        public void emptyCanvas()
        {
            //this.mainCanvas.Children.Clear();
            this.tspPoints.Clear();
        }
    }
}