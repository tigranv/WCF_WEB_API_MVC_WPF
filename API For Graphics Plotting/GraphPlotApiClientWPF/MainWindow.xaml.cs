using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GraphPlotApiClientWPF
{

    public partial class MainWindow : Window
    {
        private Polyline polyline;
        private HttpClient client;

        public MainWindow()
        {
            polyline = new Polyline { Stroke = Brushes.Black };

            InitializeComponent();
        }

        private void Draw_Sin_Click(object sender, RoutedEventArgs e)
        {
            AddChart();
        }

        private void AddChart()
        {
            HttpClient client = new HttpClient();

            string url = string.Format("http://localhost:53578/api/PlotGraph?id={0}", Uri.EscapeDataString("1"));

            client.GetAsync(url)
               .ContinueWith(response =>
               {
                   if (response.Exception != null)
                   {
                       MessageBox.Show(response.Exception.Message);
                   }
                   else
                   {
                       HttpResponseMessage message = response.Result;
                       string responseText = message.Content.ReadAsStringAsync().Result;

                       JavaScriptSerializer jss = new JavaScriptSerializer();
                       List<Point> list = jss.Deserialize<List<Point>>(responseText);
                       
                       Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                           (Action)(() =>
                           {
                               foreach (var item in list)
                               {
                                   polyline.Points.Add(CorrespondingPoint(new Point(item.X, item.Y)));
                               }
                               canvas.Children.Add(polyline);
                           }));
                   }
               });             
            
        }

        private Point CorrespondingPoint(Point pt)
        {
        double xmin = 0;
        double xmax = 6.5;
        double ymin = -1.1;
        double ymax = 1.1;

        var result = new Point
            {
                X = (pt.X - xmin) * canvas.Width / (xmax - xmin),
                Y = canvas.Height - (pt.Y - ymin) * canvas.Height / (ymax - ymin)
            };
            return result;
        }

    }
}


