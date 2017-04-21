using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Newtonsoft.Json;
using System.Net.Http.Formatting;



namespace GraphPlotApiClientWPF
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Draw_Graph_Click(object sender, RoutedEventArgs e)
        {        
            RequestParameters param = new RequestParameters();
               
            if ((bool)SinRadioBt.IsChecked)
            {
                param.Amplitude = double.Parse(AmplitudeTexXox.Text);
                AddChart("Sin", param);
            }
            else if ((bool)CosRadioBt.IsChecked)
            {
                param.Amplitude = double.Parse(AmplitudeTexXox.Text);
                AddChart("Cos", param);
            }
            else if ((bool)CosDescRadioBt.IsChecked)
            {
                param.Amplitude = double.Parse(AmplitudeTexXox.Text);
                AddChart("CosDesc", param);
            }
            else if ((bool)SinAsceRadioBt.IsChecked)
            {
                param.Amplitude = double.Parse(AmplitudeTexXox.Text);
                AddChart("SinAsceRadioBt", param);
            }
        }

        private void AddChart(string FuncName, RequestParameters parameters)
        {

            HttpClient client = new HttpClient();
            Polyline polyline = new Polyline { Stroke = Brushes.Black };

            List<SolidColorBrush> ColorList = new List<SolidColorBrush>();
            ColorList.Add(Brushes.Red);
            ColorList.Add(Brushes.Green);
            ColorList.Add(Brushes.Blue);
            ColorList.Add(Brushes.Yellow);
            ColorList.Add(Brushes.Orange);
            ColorList.Add(Brushes.Black);

            if (!(bool)ComparerRadioBt.IsChecked)
            {
                if (canvas.Children.Count != 0) { canvas.Children.Clear(); }
            }
            else
            {
                Random rd = new Random();
                polyline.Stroke =  ColorList[rd.Next(0, 5)];
            }

            


            string url = string.Format("http://localhost:53578/api/PlotGraph?function={0}",  Uri.EscapeDataString(FuncName));

            client.PostAsync(url, parameters, new JsonMediaTypeFormatter())
               .ContinueWith(response =>
               {
                   if (response.Exception != null)
                   {
                       MessageBox.Show(response.Exception.Message);
                   }
                   else
                   {
                       JavaScriptSerializer jss = new JavaScriptSerializer();
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
                               //canvas.Children.Clear();
                               canvas.Children.Add(polyline);
                           }));
                   }
               });
        }

        private Point CorrespondingPoint(Point pt)
        {
        double xmin = 0;
        double xmax = 20;
        double ymin = -1.1;
        double ymax = 1.1;
        double.TryParse(FrequencyTextBox.Text, out xmax);


            var result = new Point
            {
                X = (pt.X - xmin) * canvas.Width / (xmax*10 - xmin),
                Y = canvas.Height - (pt.Y - ymin) * canvas.Height / (ymax - ymin)
            };
            return result;
        }

        private void ClearButtn_Click(object sender, RoutedEventArgs e)
        {
            if (canvas.Children.Count != 0) canvas.Children.Clear();
        }
    }

    
}


//client.GetAsync(url)
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