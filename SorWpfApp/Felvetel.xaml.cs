using MySql.Data.MySqlClient;
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
    /// Interaction logic for Felvetel.xaml
    /// </summary>
    public partial class Felvetel : Window
    {
        public static string connectionString = dbConnection.connection;
        private MySqlConnection? connection;
        public Felvetel()
        {
            InitializeComponent();
            sliFelvesz.Minimum = 1000;
            sliFelvesz.Maximum = UserAtkuldese.bejelentkezettFogado.balance;
            sliFelvesz.Value = UserAtkuldese.bejelentkezettFogado.balance / 2;
        }

        private void cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnFelvetel_Click(object sender, RoutedEventArgs e)
        {
            var account = Application.Current.Windows.OfType<MainWindow>().First();
            string[] split = account.lblBalance.Content.ToString().Split();
            int ujOsszeg = Convert.ToInt32(split[0]) - (int)sliFelvesz.Value;
            account.lblBalance.Content = $"{ujOsszeg} Ft";
            UserAtkuldese.bejelentkezettFogado.balance = ujOsszeg;
            MessageBox.Show("Sikeres pénzfelvétel!", "Befizetés", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string lekerdezesSzoveg = $"UPDATE `bettors` SET `Balance`='{ujOsszeg}'WHERE Username = '{UserAtkuldese.bejelentkezettFogado.username}'";

                MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                lekerdezes.CommandTimeout = 60;
                lekerdezes.ExecuteNonQuery();

                connection.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            account.Container.Content = new PageAccount();



            this.Close();
        }
    }
}
