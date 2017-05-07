using System;
using System.Windows;

namespace WpfClient_CallBack_
{

    public partial class PrivateMessWindow : Window
    {
        static string senderName;
        static string reciverName;

        public PrivateMessWindow()
        {
            InitializeComponent();
        }

        public PrivateMessWindow(string sender, string reciver)
        {
            InitializeComponent();
            senderName = sender;
            reciverName = reciver;
            NameOfUser.Text = reciver;
        }

        private void Bt_Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.pipeProxy.AddPrivateMessage(PrivatemessageTextbox.Text, senderName, reciverName);
                Dispatcher.Invoke(() =>
                {
                    PrivatertbMessages.Text += $"{senderName} ---> {PrivatemessageTextbox.Text + ": " + DateTime.Now.ToString("hh:mm:ss")}"  + "\n";
                });
                PrivatemessageTextbox.Clear();       
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.rooms.Remove(reciverName);
        }
    }
}
