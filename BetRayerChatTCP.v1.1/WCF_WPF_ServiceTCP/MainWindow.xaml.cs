using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WCF_WPF_ServiceTCP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public partial class MainWindow : Window, IMessage
    {
        private static List<IMessageCallback> subscribers = new List<IMessageCallback>();
        public ServiceHost host = null;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            host = new ServiceHost(typeof(MainWindow), new Uri("net.tcp://localhost:7000"));
            host.AddServiceEndpoint(typeof(IMessage), new NetTcpBinding(), "ISubscribe");
            host.Open();
            labelStatus.Content = "Connected to Port localhost:7000";
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            host.Close();
            labelStatus.Content = "Disconnected";
        }

        public bool Subscribe()
        {
            try
            {
                IMessageCallback callback = OperationContext.Current.GetCallbackChannel<IMessageCallback>();
                if (!subscribers.Contains(callback))
                    subscribers.Add(callback);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Unsubscribe()
        {
            try
            {
                IMessageCallback callback = OperationContext.Current.GetCallbackChannel<IMessageCallback>();
                if (subscribers.Contains(callback))
                    subscribers.Remove(callback);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void AddMessage(string message, string sender)
        {
            string messText = $"{sender} ---> {message}";
            //ListOfOperations.Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() => { ListOfOperations.Text = $"New message - {message}"; }));
            subscribers.ForEach(delegate (IMessageCallback callback)
            {
                if (((ICommunicationObject)callback).State == CommunicationState.Opened)
                {
                    callback.OnMessageAdded(messText, DateTime.Now);
                }
                else
                {
                    subscribers.Remove(callback);
                }
            });    
        }
    }
}
