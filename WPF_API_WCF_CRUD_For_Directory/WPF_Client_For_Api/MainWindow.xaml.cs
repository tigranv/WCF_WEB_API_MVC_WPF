using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using System.Web.Script.Serialization;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Net.Http.Formatting;

namespace WPF_Client_For_Api
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<string> files;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Bt_Show_All_Files_Click(object sender, RoutedEventArgs e)
        {
            
            if(files != null) files.Clear();
            if(Directory_Name_TextBox.Text != string.Empty && Directory_Name_TextBox.Text != null)
            {
                HttpClient client = new HttpClient();
                string url = string.Format("http://localhost:60523/api/main?dirName={0}", Uri.EscapeDataString(Directory_Name_TextBox.Text));

                await client.GetAsync(url)
                    .ContinueWith(response =>
                    {
                        if (response.Exception != null)
                        {
                            MessageBox.Show($"No Directory with Name {Directory_Name_TextBox.Text}");
                        }
                        else
                        {
                            HttpResponseMessage message = response.Result;
                            string responseText = message.Content.ReadAsStringAsync().Result;

                            JavaScriptSerializer jss = new JavaScriptSerializer();
                            files = jss.Deserialize<ObservableCollection<string>>(responseText);

                            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                (Action)(() =>
                                {


                                    ListBox_FilesName.DataContext = files;

                                    Binding binding = new Binding();
                                    ListBox_FilesName.SetBinding(ItemsControl.ItemsSourceProperty, binding);

                                    (ListBox_FilesName.ItemsSource as ObservableCollection<string>).RemoveAt(0);
                                }));
                        }
                    });
            }
            else
            {
                MessageBox.Show($"Please Enter Directory Name!!!");
            }

        }

        private async void SelectItemEvent(object sender, SelectionChangedEventArgs e)
        {
            if(ListBox_FilesName.SelectedItem != null)
            {
                HttpClient client = new HttpClient();
                string url = $"http://localhost:60523/api/main?dirName={Directory_Name_TextBox.Text}&fileName={ListBox_FilesName.SelectedItem.ToString()}";

                await client.GetAsync(url)
                    .ContinueWith(response =>
                    {
                        if (response.Exception != null)
                        {
                            MessageBox.Show(response.Exception.Message);
                        }
                        else
                        {
                            HttpResponseMessage message = response.Result;
                            string responseText = message.Content.ReadAsStringAsync().Result;

                            JavaScriptSerializer jss = new JavaScriptSerializer();
                            string content = jss.Deserialize<string>(responseText);

                            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                (Action)(() => { ContentTextbox.Text = content; }));
                        }
                    });
            }
            

        }

        private void Delete_Button_click(object sender, RoutedEventArgs e)
        {
            if(ListBox_FilesName.SelectedItem != null)
            {
                HttpClient client = new HttpClient();
                string url = $"http://localhost:60523/api/main?dirName={Directory_Name_TextBox.Text}&fileName={ListBox_FilesName.SelectedItem.ToString()}";
                client.DeleteAsync(url);
                if (files != null) files.RemoveAt(files.IndexOf(ListBox_FilesName.SelectedItem.ToString()));
            }

        }

        

        private async void Create_New_Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ListBox_FilesName.SelectedItem != null && ContentTextbox.Text != null)
            {
                HttpClient client = new HttpClient();
                string url = $"http://localhost:60523/api/main?dirName={Directory_Name_TextBox.Text}&fileName={ListBox_FilesName.SelectedItem.ToString()}";

                await client.PutAsync(url, ContentTextbox.Text, new JsonMediaTypeFormatter())
                    .ContinueWith(response =>
                    {
                        if (response.Exception != null)
                        {
                            MessageBox.Show(response.Exception.Message);
                        }
                        else
                        {
                            MessageBox.Show("File successfully updated");
                        }
                    });
            }
        }

        private async void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox_FilesName.SelectedItem != null && ContentTextbox.Text != null)
            {
                HttpClient client = new HttpClient();
                string url = $"http://localhost:60523/api/main?dirName={Directory_Name_TextBox.Text}&fileName={ListBox_FilesName.SelectedItem.ToString()}";

                await client.PutAsync(url, ContentTextbox.Text, new JsonMediaTypeFormatter())
                    .ContinueWith(response =>
                    {
                        if (response.Exception != null)
                        {
                            MessageBox.Show(response.Exception.Message);
                        }
                        else
                        {
                            MessageBox.Show("File successfully updated");
                        }
                    });
            }

        }
    }
}
