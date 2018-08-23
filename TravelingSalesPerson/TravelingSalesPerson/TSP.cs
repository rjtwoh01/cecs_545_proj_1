using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace TravelingSalesPerson
{
    class TSP
    {
        public Point canvasOffset; //Not sure if we need this yet
        public double[,] matrix;
        public Point maxPoint;
        public Point minPoint;
        public List<TSPPoint> tspPoints;
        public List<Point> points;

        private double totalDistance;

        public TSP(List<Point> points)
        {
            this.tspPoints = new List<TSPPoint>(points.Count);
            this.points = new List<Point>();
            this.minPoint = points.First();
            this.maxPoint = points.First();

            foreach(Point point in points)
            {
                this.points.Add(point);
            }

            for (int i = 0; i < points.Count; i++)
            {
                TSPPoint point = new TSPPoint(points[i], i);
                this.tspPoints.Add(point);

                if (point.point.X < this.minPoint.X) { this.minPoint.X = point.point.X; }
                else if (point.point.X > this.maxPoint.X) { this.maxPoint.X = point.point.X; }
                if (point.point.Y < this.minPoint.Y) { this.minPoint.Y = point.point.Y; }                
                else if (point.point.Y > this.maxPoint.Y) { this.maxPoint.Y = point.point.Y; }
            }

            this.canvasOffset = new Point(10, 10);

            if (this.minPoint.X > 0) { this.canvasOffset.X -= this.minPoint.X; }
            else { this.canvasOffset.X += this.minPoint.X; }
            if (this.minPoint.Y > 0) { this.canvasOffset.X -= this.minPoint.X; }
            else { this.canvasOffset.X += this.minPoint.X; }

            this.totalDistance = 0;
        }

        public double distance(Point pointOne, Point pointTwo)
        {
            return Math.Sqrt(Math.Pow((pointTwo.X - pointOne.X), 2) + Math.Pow((pointTwo.Y - pointOne.Y), 2));
        }

        public List<Point> BruteForce()
        {
            //This final list will represent the correct order - or path - to take
            List<Point> finalList = new List<Point>();
            var tempList = new List<Point>();
            var newList = new List<Point>();
            double localDistance = 0;
            totalDistance = 0;
            int totalPermutations = 0;

            foreach (Point point in this.points)
            {
                tempList.Add(point);
            }

            for (int i = 0; i < tempList.Count() - 1; i++)
            {
                newList.Clear();
                if (i > 0)
                {
                    newList.Add(tempList.First());
                }
                double prevTotalDistance = totalDistance;
                newList.Add(tempList[i]);
                for (int j = 0; j < tempList.Count(); j++)
                {
                    if (i == j || j == 0) { continue; }
                    localDistance = distance(tempList[i], tempList[j]);
                    totalDistance += localDistance;
                    newList.Add(tempList[j]);
                    totalPermutations++;
                }

                if (totalDistance > prevTotalDistance)
                {
                    finalList.Clear();
                    foreach (Point point in newList)
                    {
                        finalList.Add(point);
                    }
                }
            }


            int city = 1;
            Debug.WriteLine("\nFinal list: ");
            foreach (Point point in finalList)
            {
                Debug.WriteLine(city + ": " + point);
                city++;
            }
            Debug.WriteLine("\nTotal Run Distance: " + totalDistance + "\nTotal Permutations: " + totalPermutations);

            return finalList;
        }

    }
}
