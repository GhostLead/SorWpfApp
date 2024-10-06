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

        private void AddCard(string titleText, string descriptionText, int eventID)
        {
            // Create a StackPanel to hold the labels and buttons
            StackPanel cardPanel = new StackPanel();
            cardPanel.Margin = new Thickness(10);
            cardPanel.Orientation = Orientation.Horizontal;

            // Create and add the labels (title and description)
            Label titleLabel = new Label();
            titleLabel.Content = titleText;
            titleLabel.FontWeight = FontWeights.Bold;
            cardPanel.Children.Add(titleLabel);

            Label descriptionLabel = new Label();
            descriptionLabel.Content = descriptionText;
            cardPanel.Children.Add(descriptionLabel);

            // Create a StackPanel for buttons (so they appear next to each other)
            StackPanel buttonPanel = new StackPanel();
            buttonPanel.Orientation = Orientation.Horizontal;

            Label betType = new Label();
            if (titleText == "Race") {
                betType.Content = "Biztonsági autók száma a versenyben: ";
                betType.Margin = new Thickness(5);
                cardPanel.Children.Add(betType);
            }
            else
            {
                betType.Content = "Bocsika, még nincs ilyen fogadás típus: ";
                betType.Margin = new Thickness(5);
                cardPanel.Children.Add(betType);
            }

            // Create a Label to display the bet value
            Label betLabel = new Label();
            betLabel.Content = "0";
            betLabel.Margin = new Thickness(5);
            betLabel.Name = $"lblEvent{eventIndex}";
            cardPanel.Children.Add(betLabel);

            // Create the '-' button to decrement the bet value
            Button decrementButton = new Button();
            decrementButton.Content = "-";
            decrementButton.Margin = new Thickness(5);
            decrementButton.Click += (s, e) =>
            {
                // Parse the current bet value and decrement it
                int currentValue = int.Parse(betLabel.Content.ToString());
                if (currentValue > 0) // Ensure value doesn't go below 0
                {
                    betLabel.Content = (currentValue - 1).ToString();
                }
            };
            buttonPanel.Children.Add(decrementButton);

            // Create the '+' button to increment the bet value
            Button incrementButton = new Button();
            incrementButton.Content = "+";
            incrementButton.Margin = new Thickness(5);
            incrementButton.Click += (s, e) =>
            {
                // Parse the current bet value and increment it
                int currentValue = int.Parse(betLabel.Content.ToString());
                betLabel.Content = (currentValue + 1).ToString();
            };
            buttonPanel.Children.Add(incrementButton);

            // Add the button panel to the main card panel
            cardPanel.Children.Add(buttonPanel);

            Button saveBet = new Button();
            saveBet.Content = "Fogadás leadása";
            saveBet.Margin = new Thickness(5);
            saveBet.Click += (s, e) =>
            {
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    string lekerdezesSzoveg = $"INSERT INTO `bets`(`BetDate`, `Odds`, `Amount`, `BettorsID`, `EventID`, `Status`) VALUES ('{System.DateTime.Now.Year}-{System.DateTime.Now.Month}-{System.DateTime.Now.Day}','0.25','200','{UserAtkuldese.bejelentkezettFogado.bettorsID}','{eventID}','1'); ";

                    MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                    lekerdezes.CommandTimeout = 60;
                    lekerdezes.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Sikeresen leadta a fogadást!");
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            };
            cardPanel.Children.Add(saveBet);


            // Finally, add the card panel to the StackPanel
            scrollContent.Children.Add(cardPanel);
        }
        private void addEventCards()
        {
            eventIndex = 1;
            foreach (var item in events)
            {
                AddCard(item.Category, item.EventName, item.EventID);
            }
        }
    }
}
