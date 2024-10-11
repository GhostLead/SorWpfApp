using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
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
    /// Interaction logic for PageFogadas.xaml
    /// </summary>
    public partial class PageFogadas : Page
    {
        public static string connectionString = "datasource = 127.0.0.1;port=3306;username=root;password=;database=fogadasok";
        private MySqlConnection? connection;
        ObservableCollection<Event> events;
        int eventIndex = 1;
        public PageFogadas()
        {
            InitializeComponent();

            loadEvents();
            
            addEventCards();
        }

        private void loadEvents()
        {
            events = new ObservableCollection<Event>();

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string lekerdezesSzoveg = "SELECT * FROM events ORDER BY EventID DESC";

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

        private void AddCard(Event eventCard)
        {

            Grid cardGrid = new Grid();
            cardGrid.Style = (Style)FindResource("EventStackpanelStyle");


            for (int i = 0; i < 4; i++)
            {
                cardGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(135) });
            }


            cardGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            cardGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });


            Label titleLabel = new Label
            {
                Style = (Style)FindResource("EventLabelCategory"),
                Content = eventCard.Category,
                FontWeight = FontWeights.Bold
            };
            Grid.SetRow(titleLabel, 0);
            Grid.SetColumn(titleLabel, 0);
            cardGrid.Children.Add(titleLabel);


            Label descriptionLabel = new Label
            {
                Style = (Style)FindResource("EventLabelDescription"),
                Content = eventCard.EventName,
                VerticalAlignment = VerticalAlignment.Center,
            };
            Grid.SetRow(descriptionLabel, 0);
            Grid.SetColumn(descriptionLabel, 0);
            cardGrid.Children.Add(descriptionLabel);

            Random rnd = new Random();
            double ujSzorzo = Math.Round((rnd.NextDouble() * 1 + 1), 2);
            Label lblOdds = new Label
            {
                Style = (Style)FindResource("EventLabelCategory"),

                Content = $"Szorzó: \t{ujSzorzo}",
                FontWeight = FontWeights.Bold,
                FontSize = 15,
                Margin = new Thickness(0, -20, 0, 0)

            };
            Grid.SetRow(lblOdds, 2);
            Grid.SetColumn(lblOdds, 0);
            cardGrid.Children.Add(lblOdds);

            StackPanel betInfoStackPanel = new StackPanel();
            betInfoStackPanel.Orientation = Orientation.Vertical;
            betInfoStackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            // Bet Type Label - Row 0, Column 1
            Label betType = new Label
            {
                Style = (Style)FindResource("EventLabelCounter"),
                Content = EventManager.Pliz.Where(x=>x.Key.EventID == eventCard.EventID).First().Value,
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            betInfoStackPanel.Children.Add(betType);

            // Bet Value Label - Row 1, Column 1
            Label betLabel = new Label
            {
                Style = (Style)FindResource("EventLabelCounter"),
                Content = "0",
                Margin = new Thickness(5),
                Name = $"lblEvent{eventCard.EventID}", // Use eventID for naming
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            betInfoStackPanel.Children.Add(betLabel);

            // Create a nested StackPanel for increment and decrement buttons
            StackPanel buttonStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };







            // Decrement Button

            Button decrementButton = new Button
            {
                Style = (Style)FindResource("EventValueChangeButton"),
                Content = "-",
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            decrementButton.Click += (s, e) =>
            {
                int currentValue = int.Parse(betLabel.Content.ToString());
                if (currentValue > 0)
                {
                    betLabel.Content = (currentValue - 1).ToString();
                    ujSzorzo = Math.Round(ujSzorzo / 1.1d, 2);
                    lblOdds.Content = $"Szorzó: \t{ujSzorzo}";

                }
            };
            buttonStackPanel.Children.Add(decrementButton);

            // Increment Button
            Button incrementButton = new Button
            {
                Style = (Style)FindResource("EventValueChangeButton"),
                Content = "+",
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            incrementButton.Click += (s, e) =>
            {
                int currentValue = int.Parse(betLabel.Content.ToString());
                betLabel.Content = (currentValue + 1).ToString();
                ujSzorzo = Math.Round(ujSzorzo * 1.1d, 2);
                lblOdds.Content = $"Szorzó: \t{ujSzorzo}";
            };
            buttonStackPanel.Children.Add(incrementButton);

            // Add buttonStackPanel to betInfoStackPanel
            betInfoStackPanel.Children.Add(buttonStackPanel);

            // Place the betInfoStackPanel in the main grid at Row 0, Column 1
            Grid.SetRow(betInfoStackPanel, 0);
            Grid.SetColumn(betInfoStackPanel, 1);
            Grid.SetColumnSpan(betInfoStackPanel, 2);
            cardGrid.Children.Add(betInfoStackPanel);


            StackPanel spPay = new StackPanel();

            Label lblPay = new Label
            {
                Content = "100",
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 17,
                FontStyle = FontStyles.Italic,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Lime)
            };
            spPay.Children.Add(lblPay);

            Slider sliPay = new Slider
            {
                Value = 100,
                TickFrequency = 100,
                Width = 130,
                Margin = new Thickness(0, 20, 0, -20),
                IsSnapToTickEnabled = true,

            };
            if (UserAtkuldese.bejelentkezettFogado.balance >= 100)
            {
                sliPay.Maximum = UserAtkuldese.bejelentkezettFogado.balance;
                sliPay.Minimum = 100;
            }

            sliPay.ValueChanged += (s, e) =>
            {
                lblPay.Content = sliPay.Value.ToString();
            };
            spPay.Children.Add(sliPay);
            Grid.SetRow(spPay, 0);
            Grid.SetColumn(spPay, 3);
            cardGrid.Children.Add(spPay);


            // Save Bet Button - Row 1, Column 3
            Button saveBet = new Button
            {
                Style = (Style)FindResource("GeneralButton"),
                Height = 30,
                Width = 111,
                Content = "Fogadás leadása",
                Margin = new Thickness(20, 5, 5, 5),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            saveBet.Click += (s, e) =>
            {
                try
                {
                    
                    
                    
                    using (var connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = $"INSERT INTO `bets`(`BetDate`, `Odds`, `Amount`, `BettorsID`, `EventID`, `Status`) VALUES ('{DateTime.Now:yyyy-MM-dd}','{ujSzorzo.ToString().Replace(',','.')}','{sliPay.Value}','{UserAtkuldese.bejelentkezettFogado.bettorsID}','{eventCard.EventID}','1');";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.CommandTimeout = 60;
                        command.ExecuteNonQuery();

                        MessageBox.Show("Sikeresen leadta a fogadást!");
                        UserAtkuldese.bejelentkezettFogado.balance -= (int)sliPay.Value;
                        var ablak = Application.Current.Windows.OfType<MainWindow>().First();
                        ablak.lblBalance.Content = $"{Convert.ToString(UserAtkuldese.bejelentkezettFogado.balance)} Ft";

                        query = $"UPDATE `bettors` SET `Balance`='{UserAtkuldese.bejelentkezettFogado.balance}'WHERE Username = '{UserAtkuldese.bejelentkezettFogado.username}'";
                        command = new MySqlCommand(query, connection);
                        command.CommandTimeout = 60;
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            };
            Grid.SetRow(saveBet, 1);
            Grid.SetColumn(saveBet, 3);
            cardGrid.Children.Add(saveBet);
            
            // Border around the card
            Border border = new Border
            {
                MinWidth = 522,
                Width = double.NaN,
                // Height is set to Auto to ensure all content is visible
                Height = double.NaN, // This allows the Border to auto-size based on its content
                CornerRadius = new CornerRadius(3),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2),
                Margin = new Thickness(7),
                Child = cardGrid
            };

            
            // Add the scroll viewer to the parent container
            scrollContent.Children.Add(border);
        }

        private void addEventCards()
        {
            eventIndex = 1;
            foreach (var item in events)
            {

                AddCard(item);
            }
        }
    }
}
