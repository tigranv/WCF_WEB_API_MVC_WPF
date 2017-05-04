using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WpfClient_CallBack_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
                
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void Bt_Send_Click(object sender, RoutedEventArgs e)
        {
            //Dispatcher.Invoke(() => 
            //{
            //    rtbMessages.Text += txtUserName.Text + " : " + "\t" + messageTextbox.Text + "\n";
            //});
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
            //note the "DuplexChannelFactory".  This is necessary for Callbacks.
            // A regular "ChannelFactory" won't work with callbacks.
            DuplexChannelFactory<IMessage> pipeFactory =
                      new DuplexChannelFactory<IMessage>(
                      new InstanceContext(this),
                      new NetTcpBinding(),
                      new EndpointAddress("net.tcp://localhost:7000/ISubscribe"));

            try
            {
                //Open the channel to the server
                pipeProxy = pipeFactory.CreateChannel();
                //Now tell the server who is connecting
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


        //This is the function that the SERVER will call
        public void OnMessageAdded(string message, DateTime timestamp)
        {
            Dispatcher.Invoke(() =>
            {
                rtbMessages.Text+= message + ": " + timestamp.ToString("hh:mm:ss") + "\n";
            });
        }

        //We need to tell the server that we are leaving
        public void Dispose()
        {
            pipeProxy.Unsubscribe();
        }

    }
}
