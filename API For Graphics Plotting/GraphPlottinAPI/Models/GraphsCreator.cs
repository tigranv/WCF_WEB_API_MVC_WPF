using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;



namespace GraphPlottinAPI.Models
{
    public class GraphsCreator
    {
        private List<Point> xyList;

        public GraphsCreator()
        {
            xyList = new List<Point>();
        }

        public List<Point> GetXY(RequestParameters par)
        {

            for (int i = 0; i < 70; i++)
            {
                var x = i / 5.0;
                var y = par.Amplitude*Math.Sin(par.frequency*x);
                xyList.Add(new Point() { X = x, Y = y });
            }

            
            return xyList;
        }
        


    }
}