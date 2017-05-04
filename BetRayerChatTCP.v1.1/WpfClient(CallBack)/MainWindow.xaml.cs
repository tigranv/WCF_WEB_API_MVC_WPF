using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows;

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
            if (Connect() == true)
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
                pipeProxy.Subscribe();
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
            pipeProxy.Unsubscribe();
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
            pipeProxy.Unsubscribe();
        }

    }
}
