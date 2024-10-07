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
    /// Interaction logic for Befizetes.xaml
    /// </summary>
    public partial class Befizetes : Window
    {
        public Befizetes()
        {
            InitializeComponent();
        }


        private void cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void paybtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
