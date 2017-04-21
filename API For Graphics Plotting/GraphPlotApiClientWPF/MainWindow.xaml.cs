using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Newtonsoft.Json;


namespace GraphPlotApiClientWPF
{

    public partial class MainWindow : Window
    {
        private Polyline polyline;

        public MainWindow()
        {
            polyline = new Polyline { Stroke = Brushes.Black };

            InitializeComponent();
        }

        private void Draw_Graph_Click(object sender, RoutedEventArgs e)
        {
            RequestParameters param = new RequestParameters();
            param.Amplitude = double.Parse(AmplitudeTexXox.Text);
            param.frequency = double.Parse(FrequencyTextBox.Text);

            if ((Boolean)SinRadioBt.IsChecked)
            {
                AddChart(SinRadioBt.Name, param);
            }

            else if ((Boolean)CosRadioBt.IsChecked)
            {
                AddChart(CosRadioBt.Name, param);
            }
            else if ((Boolean)PowRadioBt.IsChecked)
            {
                param.X = double.Parse(PowX.Text);
                param.N = int.Parse(PowN.Text);
                AddChart(PowRadioBt.Name, param);
            }
            else if ((Boolean)LogRadioBt.IsChecked)
            {
                param.X = double.Parse(LogX.Text);
                AddChart(LogRadioBt.Name, param);
            }

        }

        private void AddChart(string FuncName, RequestParameters parameters)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();

            var paramsasjson = jss.Serialize(parameters);
            HttpClient client = new HttpClient();
            string url = string.Format("http://localhost:53578/api/PlotGraph?function={0}",  Uri.EscapeDataString(FuncName));

            //HttpContent contentPost = new StringContent(paramsasjson, System.Text.Encoding.UTF8);

            //client.PostAsync(url, contentPost)
            //   .ContinueWith(response =>
            //   {
            //       if (response.Exception != null)
            //       {
            //           MessageBox.Show(response.Exception.Message);
            //       }
            //       else
            //       {
            //           HttpResponseMessage message = response.Result;
            //           string responseText = message.Content.ReadAsStringAsync().Result;
            //           List<Point> list = jss.Deserialize<List<Point>>(responseText);

            //           Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            //               (Action)(() =>
            //               {
            //                   foreach (var item in list)
            //                   {
            //                       polyline.Points.Add(CorrespondingPoint(new Point(item.X, item.Y)));
            //                   }
            //                   canvas.Children.Add(polyline);
            //               }));
            //       }
            //   });


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


