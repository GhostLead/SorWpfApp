using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static string connectionString = "datasource = 127.0.0.1;port=3306;username=root;password=;database=fogadasok";
        private MySqlConnection? connection;
        Bettor user = UserAtkuldese.bejelentkezettFogado;
        ObservableCollection<Bet> fogadasok;
        string backgroundcolor = "#FF343538";
        int fogadasokSzama = 0;
        int osszesKoltes = 0;
        int kartyaIndex = 0;
        ObservableCollection<Event> events;
        private static bool modosit = false;
        
        public PageAccount()
        {
            InitializeComponent();
            lblOsszegBalance.Content = user.balance + " Ft";
            lbusername.Content = user.username;
            loadBets();
            lblFogadasok.Content = fogadasokSzama;
            lblElkoltott.Content = osszesKoltes + " Ft";
            lblEmail.Text = user.email;
            lblJoinDate.Content = user.joinDate;
            loadEvents();
            addEventCards();
        }

        private void loadBets()
        {
            fogadasok = new ObservableCollection<Bet>();
            
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string lekerdezesSzoveg = $"SELECT * FROM `bets` WHERE BettorsID  = '{user.bettorsID}' ORDER BY BetsID; ";

                MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                lekerdezes.CommandTimeout = 60;
                MySqlDataReader reader = lekerdezes.ExecuteReader();
                while (reader.Read())
                {
                    fogadasok.Add(new Bet(reader));
                }
                reader.Close();
                connection.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            fogadasokSzama = fogadasok.Count;
            osszesKoltes = fogadasok.Sum(x => x.Amount);
        }

        private void loadEvents()
        {
            events = new ObservableCollection<Event>();

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string lekerdezesSzoveg = "SELECT * FROM events ORDER BY EventID";

                MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                lekerdezes.CommandTimeout = 60;
                MySqlDataReader reader = lekerdezes.ExecuteReader();
                while (reader.Read())
                {
                    events.Add(new Event(reader));
                }
                reader.Close();
                connection.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void AddCard(string eventName, string date, string price, string odds, int active)
        {
            int sorokSzama = grdElozmeny.RowDefinitions.Count;
            int oszlopokSzama = grdElozmeny.ColumnDefinitions.Count;
            
            // Create and add the labels (title and description)
            
                
                    Label titleLabel = new Label();
                    Label lblDate = new Label();
                    Label lblPrice = new Label();
                    Label lblOdds = new Label();
                    Label lblActive = new Label();

                    titleLabel.Content = eventName;
                    titleLabel.FontWeight = FontWeights.Bold;
                    grdElozmeny.Children.Add(titleLabel);
                    Grid.SetColumn(titleLabel, 0);
                    Grid.SetRow(titleLabel, kartyaIndex);


                    lblDate.Content = "\t"+date;
                    grdElozmeny.Children.Add(lblDate);
                    Grid.SetColumn(lblDate, 1);
                    Grid.SetRow(lblDate, kartyaIndex);

                    lblOdds.Content = "\t" + odds;
                    grdElozmeny.Children.Add(lblOdds);
                    Grid.SetColumn(lblOdds, 2);
                    Grid.SetRow(lblOdds, kartyaIndex);

                    lblPrice.Content = "\t" + price;
                    grdElozmeny.Children.Add(lblPrice);
                    Grid.SetColumn(lblPrice, 3);
                    Grid.SetRow(lblPrice, kartyaIndex);


                    if (active == 1)
                    {
                        lblActive.Content = "\tActive";


                    }
                    else
                    {
                        lblActive.Content = "\tInactive";

                    }
                    grdElozmeny.Children.Add(lblActive);
                    Grid.SetColumn(lblActive, 4);
                    Grid.SetRow(lblActive, kartyaIndex);
                
            
            kartyaIndex++;
            
            

           

        }
        private void addEventCards()
        {
            kartyaIndex = 0;
            for (int i = 0; i <= fogadasokSzama; i++)
            {
                grdElozmeny.RowDefinitions.Add(new RowDefinition());
            }
            List<string> eventNevek = new();
            int index = 0;
            
            foreach (var item in fogadasok)
            {
                foreach (var esemeny in events)
                {
                    if (esemeny.EventID == item.EventID)
                    {
                        eventNevek.Add(esemeny.EventName) ;

                    }
                }
            }
            foreach (var item in fogadasok)
            {
                AddCard(eventNevek[index], item.BetDate.ToString(), item.Amount.ToString(), item.Odds.ToString(), item.Status);
                index++;
            }
            
        }



        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {

            LogRegWindow logRegWindow = new LogRegWindow();
            logRegWindow.Show();

            var main = Window.GetWindow(this);
            main.Close();
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
