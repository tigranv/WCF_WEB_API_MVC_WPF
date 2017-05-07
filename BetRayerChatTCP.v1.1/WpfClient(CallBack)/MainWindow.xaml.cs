using ServiceClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace WpfClient_CallBack_
{
    public partial class MainWindow : Window, IMessageCallback, IDisposable
    {
        public static IMessage pipeProxy = null;
        Dictionary<string, PrivateMessWindow> rooms = new Dictionary<string, PrivateMessWindow>();
        bool flag = false;
        private bool JustChecked;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Bt_LogIn_Click(object sender, RoutedEventArgs e)
        {
            if (!flag)
            {
                if (Connect())
                {
                    status.Text = $"{txtUserName.Text} is connected";
                    pipeProxy.SendOnlineUsers();
                    flag = true;
                    Bt_LogIn.Content = "Log Out";
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }
            else
            {
                status.Text = $"Disconnected";
                flag = false;
                Bt_LogIn.Content = "Log In";
                ListBox_OnlineUsers.ItemsSource = null;
                pipeProxy.Unsubscribe(txtUserName.Text);
            }   
        }

        private void Bt_Send_Click(object sender, RoutedEventArgs e)
        {
            bool converterMode = (bool)RadioBtnConverter.IsChecked;
            try
            {
                pipeProxy.AddMessage(messageTextbox.Text, txtUserName.Text, converterMode);
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
      
        public void OnMessageAdded(string message, DateTime timestamp)
        {
            Dispatcher.Invoke(() =>
            {
                rtbMessages.Text+= message + ": " + timestamp.ToString("hh:mm:ss") + "\n";
            });
        }        

        public void SendNames(ObservableCollection<string> names)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (Action)(() =>
                {
                    ListBox_OnlineUsers.DataContext = names;
                    Binding binding = new Binding();
                    ListBox_OnlineUsers.SetBinding(ItemsControl.ItemsSourceProperty, binding);
                }));
        }

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
            if ((bool)RadioBtnConverter.IsChecked)
                s.IsChecked = false;
        }

        private void ClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(pipeProxy != null)
            {
                pipeProxy.Unsubscribe(txtUserName.Text);
            }      
        }

        public void Dispose()
        {
            if (pipeProxy != null)
            {
                pipeProxy.Unsubscribe(txtUserName.Text);
            }
        }

        private void PM_ChatClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!rooms.ContainsKey(ListBox_OnlineUsers.SelectedItem.ToString()))
            {
                PrivateMessWindow win2 = new PrivateMessWindow(txtUserName.Text, ListBox_OnlineUsers.SelectedItem.ToString());
                rooms.Add(ListBox_OnlineUsers.SelectedItem.ToString(), win2);
                win2.Show();
            }
            else
            {
                rooms[ListBox_OnlineUsers.SelectedItem.ToString()].Show();
            }
            
        }

        public void OnPrivateMessageAdded(string message, string sender, DateTime timestamp)
        {
            if (!rooms.ContainsKey(sender))
            {
                PrivateMessWindow win2 = new PrivateMessWindow(txtUserName.Text, sender);
                rooms.Add(sender, win2);
            }

            Dispatcher.Invoke(() =>
            {
                rooms[sender].PrivatertbMessages.Text += $"{sender} ---> {message + ": " + DateTime.Now.ToString("hh:mm:ss")}" + "\n";
            });
            rooms[sender].Show();
        }
    }
}
