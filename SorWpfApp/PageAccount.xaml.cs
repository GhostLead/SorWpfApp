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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SorWpfApp
{
    /// <summary>
    /// Interaction logic for PageAccount.xaml
    /// </summary>
    public partial class PageAccount : Page
    {
        string backgroundcolor = "#FF343538";
        private static bool modosit = false;
        
        public PageAccount()
        {
            InitializeComponent();
            
            
        }

        public bool IsUsingLightTheme()
        {
            // Check if any of the merged dictionaries contains lighttheme.xaml
            return Application.Current.Resources.MergedDictionaries
                .Any(dict => dict.Source != null && dict.Source.OriginalString.EndsWith("LightTheme.xaml", StringComparison.OrdinalIgnoreCase));
        }

        private void btnModosit_Click(object sender, RoutedEventArgs e)
        {
            if (!modosit)
            {
                txtUsername.IsEnabled = true;
                txtPassword.Visibility = Visibility.Visible;
                txtPassword.Text = "examplepassword";
                txtPassword.IsEnabled = true;
                passPassword.Visibility = Visibility.Hidden;
                txtEmail.IsEnabled = true;
                btnModosit.Content = "💾";
                modosit = true;
            }
            else
            {
                txtUsername.IsEnabled = false;
                txtPassword.Visibility = Visibility.Hidden;
                txtPassword.Text = "examplepassword";
                txtPassword.IsEnabled = false;
                passPassword.Visibility = Visibility.Visible;
                txtEmail.IsEnabled = false;
                btnModosit.Content = "✏";
                modosit = false;
            }

            
        }
    }
}
