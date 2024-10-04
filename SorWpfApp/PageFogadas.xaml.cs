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

        private void AddCard(string titleText, string descriptionText)
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

            // Create and add the buttons
            Button button1 = new Button();
            button1.Content = "Action 1";
            button1.Margin = new Thickness(5);
            button1.Click += (s, e) => MessageBox.Show("Action 1 clicked!");
            buttonPanel.Children.Add(button1);

            Button button2 = new Button();
            button2.Content = "Action 2";
            button2.Margin = new Thickness(5);
            button2.Click += (s, e) => MessageBox.Show("Action 2 clicked!");
            buttonPanel.Children.Add(button2);

            // Add the button panel to the main card panel
            cardPanel.Children.Add(buttonPanel);

            // Finally, add the card panel to the StackPanel
            scrollContent.Children.Add(cardPanel);
        }
        private void addEventCards()
        {
            foreach (var item in events)
            {
                AddCard(item.Category, item.EventName);
            }
        }
    }
}
