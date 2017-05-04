using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;

namespace WCF_WPF_ServiceTCP
{
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
