using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using LatinTo_ArmClassLibrary;
using System.Threading.Tasks;

namespace WCF_WPF_ServiceTCP
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public partial class MainWindow : Window, IMessage
    {
        private static List<IMessageCallback> subscribers = new List<IMessageCallback>();
        private static List<string> onlinesList = new List<string>();
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
            labelStatus.Content = "Connected to Port net.tcp://localhost:7000";
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            host.Close();
            labelStatus.Content = "Disconnected";
        }

        public bool Subscribe(string name)
        {
            try
            {
                IMessageCallback callback = OperationContext.Current.GetCallbackChannel<IMessageCallback>();
                if (!subscribers.Contains(callback))
                {
                    subscribers.Add(callback);
                    onlinesList.Add(name);

                    //foreach(var callback1 in subscribers)
                    //{
                    //    if (((ICommunicationObject)callback1).State == CommunicationState.Opened)
                    //    {
                    //        callback1.SendNames(onlinesList);
                    //    }
                    //    else
                    //    {
                    //        subscribers.Remove(callback1);
                    //    }
                    //}
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public bool Unsubscribe(string name)
        {
            try
            {
                IMessageCallback callback = OperationContext.Current.GetCallbackChannel<IMessageCallback>();
                if (subscribers.Contains(callback))
                {
                    subscribers.Remove(callback);
                    onlinesList.Remove(name);
                    subscribers.ForEach(delegate (IMessageCallback callback1)
                    {
                        if (((ICommunicationObject)callback1).State == CommunicationState.Opened)
                        {
                            callback1.SendNames(onlinesList);
                        }
                        else
                        {
                            subscribers.Remove(callback1);
                        }
                    });
                    
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void AddMessage(string message, string sender)
        {
            string messText = $"{sender} ---> {message.LatToArmConverter1()}";
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
