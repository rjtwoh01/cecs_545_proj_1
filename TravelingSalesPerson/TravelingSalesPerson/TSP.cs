using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Collections;

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
        public List<Point> tempFinalList;

        private double shortestDistance;

        public TSP(List<Point> points)
        {
            this.tspPoints = new List<TSPPoint>(points.Count);
            this.points = new List<Point>();
            this.minPoint = points.First();
            this.maxPoint = points.First();
            this.tempFinalList = new List<Point>();

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

            this.shortestDistance = 0;
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
            shortestDistance = 0;
            int totalPermutations = 0;
            int initialCount = 0;

            foreach (Point point in this.points)
            {
                tempList.Add(point);
            }

            initialCount = tempList.Count();

            Point firstElement = tempList.First();
            List<Point> rest = tempList;
            rest.RemoveAt(0);

            foreach(var perm in Permutate(rest, rest.Count()))
            {
                double shortestSoFar = shortestDistance;
                localDistance = 0;
                newList.Clear();
                newList.Add(firstElement); //we start with the same city every time
                foreach (var i in perm)
                {
                    //Console.WriteLine(i.ToString());
                    string[] parts = i.ToString().Split(',');
                    Point tempPoint = new Point(Convert.ToDouble(parts[0]), Convert.ToDouble(parts[1]));
                    newList.Add(tempPoint);
                }
                newList.Add(firstElement); //we end with the same city every time
                for (int i = 0; i < newList.Count(); i++)
                {
                    if ((i + 1) != newList.Count())
                        localDistance = distance(newList[i], newList[i + 1]);
                }
                if (shortestDistance > localDistance || shortestDistance == 0)
                {
                    shortestDistance = localDistance;
                    finalList.Clear();
                    finalList = newList.ToList(); //Save computation time of foreach
                }
            }

            int city = 1;
            Debug.WriteLine("\nFinal list: ");
            foreach (Point point in finalList)
            {
                Debug.WriteLine(city + ": " + point);
                city++;
            }
            Debug.WriteLine("\nTotal Run Distance: " + shortestDistance + "\nTotal Permutations: " + totalPermutations);

            return finalList;
        }

        #region Permutation

        public static void RotateRight(IList sequence, int count)
        {
            object tmp = sequence[count - 1];
            sequence.RemoveAt(count - 1);
            sequence.Insert(0, tmp);
        }

        public static IEnumerable<IList> Permutate(IList sequence, int count)
        {
            if (count == 1) yield return sequence;
            else
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (var perm in Permutate(sequence, count - 1))
                        yield return perm;
                    RotateRight(sequence, count);
                }
            }
        }

        #endregion
    }
}
