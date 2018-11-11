using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VKManager.View;
using VKApi.Help;
using NLog;
using VKApi.Model;
using VKApi;

namespace VKManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string LoginName { get; set; }
        public static GroupModel groupModel;

        public MainWindow()
        {
            InitializeComponent();
           // this.DataContext = groupModel;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (GlobalConfig.AccessToken == default)
            {
                LoginWindows();
            }
        }

        private void StackPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lvLogin_Click(object sender, MouseButtonEventArgs e)
        {
            LoginWindows();
        }

        private void LoginWindows()
        {
            LoginForm loginForm = new LoginForm();
            this.IsEnabled = false;
            loginForm.Owner = this;
            loginForm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            loginForm.Show();   
        }
    }
}
