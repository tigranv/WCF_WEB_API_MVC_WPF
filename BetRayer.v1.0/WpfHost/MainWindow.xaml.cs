using System;
using System.Windows;
using System.ServiceModel;
using System.ServiceModel.Description;


namespace WpfHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        ServiceHost host;

        private void buttonStart_Click(object sender,
                                       RoutedEventArgs e)
        {
            buttonStart.IsEnabled = false;

            //Define base addresses so all 
            //endPoints can go under it

            Uri tcpAdrs = new Uri("net.tcp://" +
                textBoxIP.Text.ToString() + ":" +
                textBoxPort.Text.ToString() + "/WpfHosting/");

            Uri httpAdrs = new Uri("http://" +
                textBoxIP.Text.ToString() + ":" +
                (int.Parse(textBoxPort.Text.ToString()) + 1).ToString() +
                "/WpfHosting/");

            Uri[] baseAdresses = { tcpAdrs, httpAdrs };

            host = new ServiceHost(
                   typeof(ServiceAssembly.ChatService), baseAdresses);
            host.Open();



            NetTcpBinding tcpBinding =
               new NetTcpBinding(SecurityMode.None, true);
            //Updated: to enable file transefer of 64 MB
            tcpBinding.MaxBufferPoolSize = (int)67108864;
            tcpBinding.MaxBufferSize = 67108864;
            tcpBinding.MaxReceivedMessageSize = (int)67108864;
            tcpBinding.TransferMode = TransferMode.Buffered;
            tcpBinding.ReaderQuotas.MaxArrayLength = 67108864;
            tcpBinding.ReaderQuotas.MaxBytesPerRead = 67108864;
            tcpBinding.ReaderQuotas.MaxStringContentLength = 67108864;


            tcpBinding.MaxConnections = 100;
            //To maxmize MaxConnections you have 
            //to assign another port for mex endpoint

            //and configure ServiceThrottling as well
            ServiceThrottlingBehavior throttle;
            throttle =
                     host.Description.Behaviors.Find<ServiceThrottlingBehavior>();
            if (throttle == null)
            {
                throttle = new ServiceThrottlingBehavior();
                throttle.MaxConcurrentCalls = 100;
                throttle.MaxConcurrentSessions = 100;
                host.Description.Behaviors.Add(throttle);
            }


            //Enable reliable session and keep 
            //the connection alive for 20 hours.
            tcpBinding.ReceiveTimeout = new TimeSpan(20, 0, 0);
            tcpBinding.ReliableSession.Enabled = true;
            tcpBinding.ReliableSession.InactivityTimeout =
                                       new TimeSpan(20, 0, 10);

            host.AddServiceEndpoint(typeof(ServiceAssembly.IChat),
                                                tcpBinding, "tcp");

            //Define Metadata endPoint, So we can 
            //publish information about the service
            ServiceMetadataBehavior mBehave =
                           new ServiceMetadataBehavior();
            host.Description.Behaviors.Add(mBehave);

            host.AddServiceEndpoint(typeof(IMetadataExchange),
                MetadataExchangeBindings.CreateMexTcpBinding(),
                "net.tcp://" + textBoxIP.Text.ToString() + ":" +
                (int.Parse(textBoxPort.Text.ToString()) - 1).ToString() +
                "/WpfHosting/mex");
            try
            {
                host.Open();
            }
            catch (Exception ex)
            {
                labelStatus.Content = ex.Message.ToString();
            }
            finally
            {
                if (host.State == CommunicationState.Opened)
                {
                    labelStatus.Content = "Opened";
                    buttonStop.IsEnabled = true;
                }
            }
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            if (host != null)
            {
                try
                {
                    host.Close();
                }
                catch (Exception ex)
                {
                    labelStatus.Content = ex.Message.ToString();
                }
                finally
                {
                    if (host.State == CommunicationState.Closed)
                    {
                        labelStatus.Content = "Closed";
                        buttonStart.IsEnabled = true;
                        buttonStop.IsEnabled = false;
                    }
                }
            }
        }

    }  
}
