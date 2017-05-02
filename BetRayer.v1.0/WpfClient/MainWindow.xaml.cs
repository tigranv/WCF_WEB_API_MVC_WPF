using ServiceAssembly;
using System.ServiceModel;
using System.Windows;
using System;
using System.Collections.Generic;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IChatCallback
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        public void IsWritingCallback(Client client)
        {
            throw new NotImplementedException();
        }

        public void Receive(Message msg)
        {
            throw new NotImplementedException();
        }

        public void ReceiverFile(FileMessage fileMsg, Client receiver)
        {
            throw new NotImplementedException();
        }

        public void ReceiveWhisper(Message msg, Client receiver)
        {
            throw new NotImplementedException();
        }

        public void RefreshClients(List<Client> clients)
        {
            throw new NotImplementedException();
        }

        public void UserJoin(Client client)
        {
            throw new NotImplementedException();
        }

        public void UserLeave(Client client)
        {
            throw new NotImplementedException();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            NetTcpBinding binding = new NetTcpBinding();

            IChat proxy = ChannelFactory<IChat>.CreateChannel(
                binding, new EndpointAddress("net.tcp://localhost:7998"));

            textBlock.Text = proxy.GetHashCode().ToString();
        }
    }
}
