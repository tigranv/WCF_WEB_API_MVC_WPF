using System;
using System.Collections.Generic;



namespace GraphPlottinAPI.Models
{
    public class GraphsCreator
    {

        public List<Point> GetXY(string functName, RequestParameters par)
        {
            
            List<Point> xyList = new List<Point>();

            switch (functName)
            {
                case "Sin":
                    {
                        for (int i = 0; i < 2000; i++)
                        {
                            var x = i / 5.0;
                            var y = par.Amplitude * 0.1 * Math.Sin(x);
                            xyList.Add(new Point() { X = x, Y = y });
                        }
                        break;
                    }

                case "Cos":
                    {
                        for (int i = 0; i < 2000; i++)
                        {
                            var x = i / 5.0;
                            var y = par.Amplitude * 0.1 * Math.Cos(x);
                            xyList.Add(new Point() { X = x, Y = y });
                        }
                        break;
                    }

                case "SinAsceRadioBt":
                    {
                        for (int i = 0; i < 2000; i++)
                        {
                            var x = i / 5.0;
                            var y = par.Amplitude * 0.1 * Math.Sin(x) * i/100;
                            xyList.Add(new Point() { X = x, Y = y });
                        }
                        break;
                    }

                case "CosDesc":
                    {
                        for (int i = 0; i < 2000; i++)
                        {
                            var x = i / 5.0;
                            var y = par.Amplitude * 0.01 * Math.Cos(x) * (2000-i) / 100;
                            xyList.Add(new Point() { X = x, Y = y });
                        }
                        break;
                    }

            }
            

            
            return xyList;
        }
        


    }
}