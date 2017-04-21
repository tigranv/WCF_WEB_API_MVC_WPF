using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphPlottinAPI.Models
{
    public class RequestParameters
    {
        public double X { get; set; }
        public int N { get; set; }

        public double Amplitude { get; set; }

        public RequestParameters()
        {
            X = 1;
            N = 1;
            Amplitude = 1;
        }
    }
}