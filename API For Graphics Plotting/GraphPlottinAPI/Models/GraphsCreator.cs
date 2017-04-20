using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;



namespace GraphPlottinAPI.Models
{
    public class GraphsCreator
    {
        private double xmin = 0;

        private double xmax = 6.5;
        private double ymin = -1.1;
        private double ymax = 1.1;
        private Polyline polyline;


        private void AddChart()
        {
            // Draw sine chart:
            polyline = new Polyline { Stroke = Brushes.Black };

            for (int i = 0; i < 70; i++)
            {
                var x = i / 5.0;
                var y = Math.Sin(x);
                polyline.Points.Add(CorrespondingPoint(new Point(x, y)));
            }
            // Draw cosine chart:
            //polyline = new Polyline
            //{
            //    Stroke = Brushes.Black,
            //    StrokeDashArray = new DoubleCollection(new double[] { 4, 3 })
            //};
            //for (int i = 0; i < 70; i++)
            //{
            //    var x = i / 5.0;
            //    var y = Math.Cos(x);
            //    polyline.Points.Add(CorrespondingPoint(new Point(x, y)));
            //}
        }
        private Point CorrespondingPoint(Point pt)
        {
            var result = new Point
            {
                X = (pt.X - xmin) * 500 / (xmax - xmin),
                Y = 250 - (pt.Y - ymin) * 250
                    / (ymax - ymin)
            };
            return result;
        }


    }
}