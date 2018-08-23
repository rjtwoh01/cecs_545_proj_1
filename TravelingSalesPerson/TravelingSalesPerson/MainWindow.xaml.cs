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
        private Canvas canvas;
        private Viewbox viewbox;
        private TSP tsp;


        public MainWindow()
        {
            InitializeComponent();
            tspFileName = "";
            hideRunTime();
            hideSolveButton();
            tspPoints = new List<Point>();
        }

        #region Data Points

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
            int city = 1; //we start at the first city
            tsp = new TSP(points);

            // Initiate viewbox
            viewbox = new Viewbox();
            viewbox.HorizontalAlignment = HorizontalAlignment.Stretch;
            viewbox.VerticalAlignment = VerticalAlignment.Stretch;

            // Initiate canvas for drawing points on
            canvas = new Canvas();

            foreach (Point point in points)
            {
                Debug.WriteLine("City: " + city);
                Ellipse ellipse = new Ellipse();
                ellipse.Width = 4;
                ellipse.Height = 4;
                ellipse.Fill = Brushes.Red;
                ellipse.Stroke = Brushes.Black;

                ellipse.ToolTip = city + ": (" + point.X + "," + point.Y + ")";

                // Position point on canvas
                Canvas.SetLeft(ellipse, point.X + tsp.canvasOffset.X);
                Canvas.SetTop(ellipse, point.Y + tsp.canvasOffset.Y);

                canvas.Children.Add(ellipse);
                //canvas.Children.Add(cityLabel);

                city++;
            }

            canvas.Height = tsp.maxPoint.Y - tsp.minPoint.Y + 80;
            canvas.Width = tsp.maxPoint.X - tsp.minPoint.X + 80;
            Debug.WriteLine(canvas.Height);
            Debug.WriteLine(canvas.Width);

            // Add canvas to viewbox
            viewbox.Child = canvas;

            // Add viewbox to grid
            mainGrid.Children.Add(viewbox);

            Debug.WriteLine(mainGrid.Children[0]);
            //Debug.WriteLine(this.mainViewBox.Child);

            //this.UpdateLayout();
            Debug.WriteLine("Finished populating points");
            
        }

        #endregion

        #region UI Elements

        public void emptyCanvas()
        {
            if (canvas != null)
                this.canvas.Children.Clear();
            this.tspPoints.Clear();
        }

        public void hideSolveButton()
        {
            this.btnSolve.Visibility = Visibility.Hidden;
        }

        public void showSolveButton()
        {
            this.btnSolve.Visibility = Visibility.Visible;
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

        #endregion

        #region UI Events

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
            showSolveButton();
        }


        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            List<Point> tempResult = tsp.BruteForce();
            sw.Stop();

            TimeSpan elapsedTime = sw.Elapsed;
            this.lblRunTime.Content = "Run Time: " + elapsedTime.ToString();

            displayRunTime();
        }

        #endregion
    }
}