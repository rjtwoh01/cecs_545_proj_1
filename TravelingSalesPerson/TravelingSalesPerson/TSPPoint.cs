using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TravelingSalesPerson
{
    class TSPPoint
    {
        public int matrixIndex;
        public Point point;

        public TSPPoint(Point point, int matrixIndex)
        {
            this.point = point;
            this.matrixIndex = matrixIndex;
        }
    }
}
