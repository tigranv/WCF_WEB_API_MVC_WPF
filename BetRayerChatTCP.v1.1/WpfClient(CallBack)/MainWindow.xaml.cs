using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace WpfClient_CallBack_
{
    public partial class MainWindow : Window, IMessageCallback, IDisposable
    {
        IMessage pipeProxy = null;
        ObservableCollection<string> list;
        public MainWindow()
        {
            InitializeComponent();
            list = new ObservableCollection<string>();
        }

        private void Bt_LogIn_Click(object sender, RoutedEventArgs e)
        {
            if (Connect())
            {
                status.Text = $"{txtUserName.Text} is connected";
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void Bt_Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pipeProxy.AddMessage(messageTextbox.Text, txtUserName.Text);
                messageTextbox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        public bool Connect()
        {
            
            DuplexChannelFactory<IMessage> pipeFactory =
                      new DuplexChannelFactory<IMessage>(
                      new InstanceContext(this),
                      new NetTcpBinding(),
                      new EndpointAddress("net.tcp://localhost:7000/ISubscribe"));
            try
            {
                pipeProxy = pipeFactory.CreateChannel();
                pipeProxy.Subscribe(txtUserName.Text);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
        public new void Close()
        {
            pipeProxy.Unsubscribe(txtUserName.Text);
        }


        public void OnMessageAdded(string message, DateTime timestamp)
        {
            Dispatcher.Invoke(() =>
            {
                rtbMessages.Text+= message + ": " + timestamp.ToString("hh:mm:ss") + "\n";
            });
        }

        public void Dispose()
        {
            pipeProxy.Unsubscribe(txtUserName.Text);
        }

        public void SendNames(List<string> names)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (Action)(() =>
                {
                    ListBox_OnlineUsers.DataContext = names;
                    Binding binding = new Binding();
                    ListBox_OnlineUsers.SetBinding(ItemsControl.ItemsSourceProperty, binding);

                    (ListBox_OnlineUsers.ItemsSource as ObservableCollection<string>).RemoveAt(0);
                }));
        }
    }
}
