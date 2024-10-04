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
        public PageAccount()
        {
            InitializeComponent();
            lblOsszegBalance.Content = UserAtkuldese.bejelentkezettFogado.balance+" Ft";
            lbusername.Content = UserAtkuldese.bejelentkezettFogado.username;
            
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {

            LogRegWindow logRegWindow = new LogRegWindow();
            logRegWindow.Show();

            var main = Window.GetWindow(this);
            main.Close();
            
        }
    }
}
