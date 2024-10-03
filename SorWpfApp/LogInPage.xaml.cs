using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SorWpfApp
{
    /// <summary>
    /// Interaction logic for LogInPage.xaml
    /// </summary>
    public partial class LogInPage : Page
    {
        
        public LogInPage()
        {
            InitializeComponent();
            txtUsername.KeyDown += new KeyEventHandler(LogRegWindow.HandleLoginInput);
            passPassword.KeyDown += new KeyEventHandler(LogRegWindow.HandleLoginInput);
        }

        private void btnShutdown_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginClass.Username = txtUsername.Text;
        }

        private void passPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            LoginClass.Password = passPassword.Password;
        }
    }
}
