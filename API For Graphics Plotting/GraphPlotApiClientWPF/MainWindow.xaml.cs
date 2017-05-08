using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Net.Http.Formatting;
using System.Windows.Controls;

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
            double temp = 20;
            double.TryParse(AmplitudeTexXox.Text, out temp);
            param.Amplitude = temp;

            if ((bool)SinRadioBt.IsChecked)
            {
                AddChart("Sin", param);
            }
            else if ((bool)CosRadioBt.IsChecked)
            {
                AddChart("Cos", param);
            }
            else if ((bool)CosDescRadioBt.IsChecked)
            {
                AddChart("CosDesc", param);
            }
            else if ((bool)SinAsceRadioBt.IsChecked)
            {
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
            ColorList.Add(Brushes.Orange);
            ColorList.Add(Brushes.Black);

            if (!(bool)ComparerRadioBt.IsChecked)
            {
                if (canvas.Children.Count != 0) { canvas.Children.Clear(); }
            }
            else
            {
                Random rd = new Random();
                polyline.Stroke =  ColorList[rd.Next(0, 4)];
            }

            


            string url = string.Format("http://localhost:53578/api/PlotGraph?function={0}",  Uri.EscapeDataString(FuncName));


            //HttpResponseMessage message = client.PostAsync(url, parameters, new JsonMediaTypeFormatter()).Result;

            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //string responseText = message.Content.ReadAsStringAsync().Result;
            //List<Point> list = jss.Deserialize<List<Point>>(responseText);

            //foreach (var item in list)
            //{
            //    polyline.Points.Add(CorrespondingPoint(new Point(item.X, item.Y)));
            //}
            //canvas.Children.Add(polyline);




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

        private bool JustChecked;

        private void RB_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton s = (RadioButton)sender;
            JustChecked = true;
        }

        private void RB_Clicked(object sender, RoutedEventArgs e)
        {
            if (JustChecked)
            {
                JustChecked = false;
                e.Handled = true;
                return;
            }
            RadioButton s = (RadioButton)sender;
            if ((bool)ComparerRadioBt.IsChecked)
                s.IsChecked = false;
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