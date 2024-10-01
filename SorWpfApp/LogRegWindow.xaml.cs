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

namespace SorWpfApp
{
    /// <summary>
    /// Interaction logic for LogRegWindow.xaml
    /// </summary>
    public partial class LogRegWindow : Window
    {
        public LogRegWindow()
        {
            InitializeComponent(); 
            Container.Content = new LogInPage();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        

        private void btnLogReg_Click(object sender, RoutedEventArgs e)
        {
            if (Container.Content is RegistrationPage)
            {
                Container.Content = new LogInPage();
                btnLogReg.Content = "Nincs fiókom / Bejelentkezek";
                btnLogin.Content = "Bejelentkezés";
            }
            else
            {
                Container.Content = new RegistrationPage();
                btnLogReg.Content = "Van fiókom / Regisztrálok";
                btnLogin.Content = "Regisztráció";
            }
        }
    }
}
