using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+");


        public RegistrationPage()
        {
            InitializeComponent();
            
            txtUsername.KeyDown += new KeyEventHandler(LogRegWindow.HandleRegisterInput);
            passPassword.KeyDown += new KeyEventHandler(LogRegWindow.HandleRegisterInput);
            txtEmail.KeyDown += new KeyEventHandler(LogRegWindow.HandleRegisterInput);
        }

       

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegisterClass.Username = txtUsername.Text;
        }

        private void passPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegisterClass.Password = passPassword.Text;
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegisterClass.Email = txtEmail.Text;
        }

        private void txtEgyenleg_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_regex.IsMatch(txtEgyenleg.Text) && txtEgyenleg.Text != "")
            {
                RegisterClass.Balance = int.Parse(txtEgyenleg.Text);
            }
            else 
            {
                if(txtEgyenleg.Text == "")
                {

                }
                else
                {
                    txtEgyenleg.Text = "";
                    MessageBox.Show("Csak számokat adhat meg ebbe a bezőbe!");
                    txtEgyenleg.BorderThickness = new Thickness(2);
                    txtEgyenleg.BorderBrush = Brushes.Red;
                    txtEgyenleg.Focus();

                }
            }

        }
    }
}
