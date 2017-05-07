using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfClient_CallBack_
{
    /// <summary>
    /// Interaction logic for PrivateMessWindow.xaml
    /// </summary>
    public partial class PrivateMessWindow : Window
    {
        string senderName;
        string reciverName;
        public PrivateMessWindow()
        {
            InitializeComponent();
        }

        public PrivateMessWindow(string sender, string reciver)
        {
            InitializeComponent();
            senderName = sender;
            reciverName = reciver;
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
    }
}
