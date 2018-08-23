using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TravelingSalesPerson
{
    class TSP
    {
        public Point canvasOffset; //Not sure if we need this yet
        public double[,] matrix;
        public Point maxPoint;
        public Point minPoint;
        public List<TSPPoint> points;

        public TSP(List<Point> points)
        {
            this.points = new List<TSPPoint>(points.Count);

            this.minPoint = points.First();
            this.maxPoint = points.First();

            for (int i = 0; i < points.Count; i++)
            {
                TSPPoint point = new TSPPoint(points[i], i);
                this.points.Add(point);

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
        }
    }
}
