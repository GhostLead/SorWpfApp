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
using System.Windows.Media.Effects;
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
            loadBets();
            loadEvents();
            addEventCards();
            lbusername.Content = user.username.ToUpper();
            txtUsername.Text = user.username;
            lblFogadasok.Content = fogadasokSzama;
            egyenleglabel.Content = user.balance + " Ft";
            lblElkoltott.Content = osszesKoltes + " Ft";
            lblEmail.Text = user.email;
            txtEmail.Text = user.email;
            lblJoinDate.Content = user.joinDate;
        }

        public bool IsDarkThemeApplied()
        {
            var darkThemeUri = new Uri("DarkTheme.xaml", UriKind.Relative);
            return Application.Current.Resources.MergedDictionaries.Any(
                d => d.Source == darkThemeUri);
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
                    Event ujEvent = new Event(reader);
                    events.Add(ujEvent);
                    EventManager.Pliz[ujEvent] = "Hány ember fog meghalni az esemény alatt?";
                }
                reader.Close();
                connection.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void AddCard(string eventName, string date, string price, string odds, int active, int betId)
        {
            int sorokSzama = scrollContent.RowDefinitions.Count;
            int oszlopokSzama = scrollContent.ColumnDefinitions.Count;
            
            // Create and add the labels (title and description)
            
                
                    Label titleLabel = new Label();
                    Label lblDate = new Label();
                    Label lblPrice = new Label();
                    Label lblOdds = new Label();
                    Label lblActive = new Label();

                    titleLabel.Content = eventName;
                    titleLabel.FontWeight = FontWeights.Bold;
                    titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
                    titleLabel.VerticalAlignment = VerticalAlignment.Center;
                    titleLabel.MouseDoubleClick += (s, e) => { DeleteBet(betId); };
                    scrollContent.Children.Add(titleLabel);
                    Grid.SetColumn(titleLabel, 0);
                    Grid.SetRow(titleLabel, kartyaIndex);


                    lblDate.Content = date;
                    lblDate.HorizontalAlignment = HorizontalAlignment.Center;
                    lblDate.VerticalAlignment = VerticalAlignment.Center;
                    lblDate.MouseDoubleClick += (s, e) => { DeleteBet(betId); };
                    scrollContent.Children.Add(lblDate);
                    Grid.SetColumn(lblDate, 1);
                    Grid.SetRow(lblDate, kartyaIndex);

                    lblOdds.Content = odds;
                    lblOdds.HorizontalAlignment = HorizontalAlignment.Center;
                    lblOdds.VerticalAlignment = VerticalAlignment.Center;
                    lblOdds.MouseDoubleClick += (s, e) => { DeleteBet(betId); };
                    scrollContent.Children.Add(lblOdds);
                    Grid.SetColumn(lblOdds, 2);
                    Grid.SetRow(lblOdds, kartyaIndex);

                    lblPrice.Content = price;
                    lblPrice.HorizontalAlignment = HorizontalAlignment.Center;
                    lblPrice.VerticalAlignment = VerticalAlignment.Center;
                    lblPrice.MouseDoubleClick += (s, e) => { DeleteBet(betId); };
                    scrollContent.Children.Add(lblPrice);
                    Grid.SetColumn(lblPrice, 3);
                    Grid.SetRow(lblPrice, kartyaIndex);


                    if (active == 1)
                    {
                        lblActive.Content = "Active";


                    }
                    else
                    {
                        lblActive.Content = "Inactive";

                    }
                    lblActive.HorizontalAlignment = HorizontalAlignment.Center;
                    lblActive.VerticalAlignment = VerticalAlignment.Center;
                    lblActive.MouseDoubleClick += (s, e) => { DeleteBet(betId); };
                    scrollContent.Children.Add(lblActive);
                    Grid.SetColumn(lblActive, 4);
                    Grid.SetRow(lblActive, kartyaIndex);            
                    kartyaIndex++;
        }

        private void DeleteBet(int betId)
        {
            Bet eventToRemove = fogadasok.FirstOrDefault(x => x.BetsID == betId);

            MessageBoxResult messageBoxresult = MessageBox.Show("Biztos hogy törli ezt a fogadását?", "Fogadás törlése", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxresult == MessageBoxResult.Yes)
            {
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    string lekerdezesSzoveg = $"DELETE FROM bets WHERE BetsID = {betId};";
                    MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                    lekerdezes.CommandTimeout = 60;
                    lekerdezes.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("A fogadás törlése sikeres!");
                    loadBets();
                    addEventCards();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }


        }
        private void addEventCards()
        {
            kartyaIndex = 0;
            scrollContent.Children.Clear();
            scrollContent.RowDefinitions.Clear();
            for (int i = 0; i <= fogadasokSzama; i++)
            {
                scrollContent.RowDefinitions.Add(new RowDefinition());
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
                AddCard(eventNevek[index], item.BetDate.ToString(), item.Amount.ToString(), item.Odds.ToString(), item.Status, item.BetsID);
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



        private void btnModosit_Click(object sender, RoutedEventArgs e)
        {
            if (!modosit)
            {
                txtUsername.IsEnabled = true;
                txtPassword.Visibility = Visibility.Visible;
                txtPassword.Text = user.password;
                txtPassword.IsEnabled = true;
                passPassword.Visibility = Visibility.Hidden;
                txtEmail.IsEnabled = true;
                btnModosit.Content = "💾";
                modosit = true;
                
            }
            else
            {
                MessageBoxResult messageBoxresult = MessageBox.Show("Biztos hogy módosíja a fiókja adatait?\nA módosítás után ismét be kell jelentkezni!", "Fiók módosítása", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxresult == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection = new MySqlConnection(connectionString);
                        connection.Open();
                        string lekerdezesSzoveg = $"UPDATE `bettors` SET `Username`='{txtUsername.Text}',`Password`='{txtPassword.Text}',`Email`='{txtEmail.Text}' WHERE Username = '{user.username}'";
                        MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                        lekerdezes.CommandTimeout = 60;
                        lekerdezes.ExecuteNonQuery();
                        connection.Close();
                        
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                    LogRegWindow logRegWindow = new LogRegWindow();
                    logRegWindow.Show();
                    var ablakBezarasa = Window.GetWindow(this);
                    ablakBezarasa.Close();
                }

                txtUsername.IsEnabled = false;
                txtPassword.Visibility = Visibility.Hidden;
                txtPassword.Text = user.password;
                txtPassword.IsEnabled = false;
                passPassword.Visibility = Visibility.Visible;
                txtEmail.IsEnabled = false;
                btnModosit.Content = "✏";
                modosit = false;
            }


        }

        private void btnBefizetes_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect blurEffect = new BlurEffect();
            blurEffect.Radius = 22;
            this.Effect = blurEffect;
            Befizetes windowbefiz = new Befizetes();
            windowbefiz.Owner = Application.Current.Windows.OfType<MainWindow>().First();
            windowbefiz.ShowDialog();
            this.Effect = null;
        }

        private void btnFelvetel_Click(object sender, RoutedEventArgs e)
        {
            if (user.balance < 1000)
            {
                MessageBox.Show("Túl kevés összeg van a fiókján hogy pénzt vegyen ki!", "Pénz felvétel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                BlurEffect blurEffect = new BlurEffect();
                blurEffect.Radius = 22;
                this.Effect = blurEffect;
                Felvetel windowbefiz = new Felvetel();
                windowbefiz.Owner = Application.Current.Windows.OfType<MainWindow>().First();
                windowbefiz.ShowDialog();
                this.Effect = null;
            }
            
        }
    }
}
