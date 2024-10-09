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
    /// Interaction logic for PageOrganizer.xaml
    /// </summary>
    public partial class PageOrganizer : Page
    {
        public static string connectionString = "datasource = 127.0.0.1;port=3306;username=root;password=;database=fogadasok";
        private MySqlConnection? connection;
        ObservableCollection<Event> events;
        int rowIndex = 0;
        public PageOrganizer()
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

        private void AddCard(Event eventCard)
        {
            StackPanel containerPanel = new StackPanel();
            containerPanel.Width = 750;
            containerPanel.Height = 110;
            containerPanel.Margin = new Thickness(20);
            containerPanel.Background = new SolidColorBrush(Brushes.DarkOrange.Color);

            Label lblCategory = new Label();
            lblCategory.Content = eventCard.Category;
            lblCategory.FontSize = 20;
            lblCategory.FontWeight = FontWeights.Bold;
            lblCategory.FontStyle = FontStyles.Italic;
            
            containerPanel.Children.Add(lblCategory);

            Grid grdLabels = new Grid();
            grdLabels.ColumnDefinitions.Add(new ColumnDefinition());
            grdLabels.ColumnDefinitions.Add(new ColumnDefinition());
            grdLabels.ColumnDefinitions.Add(new ColumnDefinition());

            TextBox txtGp = new TextBox();
            txtGp.Text = eventCard.EventName;
            txtGp.FontSize = 15;
            txtGp.IsEnabled = false;
            Grid.SetColumn(txtGp, 0);
            grdLabels.Children.Add(txtGp);

            DatePicker dpDate = new DatePicker();
            dpDate.SelectedDate = eventCard.EventDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
            dpDate.FontSize = 15;
            dpDate.IsEnabled = false;
            Grid.SetColumn(dpDate, 1);
            grdLabels.Children.Add(dpDate);

            TextBox txtCountry = new TextBox();
            txtCountry.Text = eventCard.Location;
            txtCountry.FontSize = 15;
            txtCountry.IsEnabled = false;
            Grid.SetColumn(txtCountry, 2);
            grdLabels.Children.Add(txtCountry);
            containerPanel.Children.Add(grdLabels);


            Grid grdButtons = new Grid();
            grdButtons.ColumnDefinitions.Add(new ColumnDefinition());
            grdButtons.ColumnDefinitions.Add(new ColumnDefinition());

            Button btnDelete = new Button();
            btnDelete.Content = "Törlés";
            btnDelete.Width = 150;
            btnDelete.Height = 25;
            btnDelete.Margin = new Thickness(0, 5, 0, 0);
            btnDelete.Style = (Style)FindResource("GeneralButton");
            btnDelete.Background = new SolidColorBrush(Brushes.Red.Color);
            btnDelete.Foreground = new SolidColorBrush(Brushes.White.Color);
            Grid.SetColumn(btnDelete, 0);
            grdButtons.Children.Add(btnDelete);

            Button btnEdit = new Button();
            btnEdit.Content = "Szerkesztés";
            btnEdit.Width = 150;
            btnEdit.Height = 25;
            btnEdit.Margin = new Thickness(0, 5, 0, 0);
            btnEdit.Style = (Style)FindResource("GeneralButton");
            btnEdit.Background = new SolidColorBrush(Brushes.Green.Color);
            btnEdit.Foreground = new SolidColorBrush(Brushes.White.Color);
            btnEdit.Click += (s, e) =>
            {
                if (txtCountry.IsEnabled)
                {
                    MessageBoxResult messageBoxresult = MessageBox.Show("Biztos hogy módosíja ezt az eseményt?", "Esemény módosítása", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxresult == MessageBoxResult.Yes)
                    {
                        txtCountry.IsEnabled = false;
                        txtGp.IsEnabled = false;
                        dpDate.IsEnabled = false;
                        try
                        {
                            connection = new MySqlConnection(connectionString);
                            connection.Open();
                            string lekerdezesSzoveg = $"UPDATE `events` SET `EventName`='{txtGp.Text}',`EventDate`='{dpDate.SelectedDate.Value.Year}-{dpDate.SelectedDate.Value.Month}-{dpDate.SelectedDate.Value.Day}', `Location`='{txtCountry.Text}' WHERE EventID = '{eventCard.EventID}'";
                            MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                            lekerdezes.CommandTimeout = 60;
                            lekerdezes.ExecuteNonQuery();
                            connection.Close();
                            
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);
                        }

                    }


                    
                }
                else
                {

                    txtCountry.IsEnabled = true;
                    txtGp.IsEnabled = true;
                    dpDate.IsEnabled = true;

                }
            };
            Grid.SetColumn(btnEdit, 1);
            grdButtons.Children.Add(btnEdit);

            containerPanel.Children.Add(grdButtons);

            Grid.SetRow(containerPanel, rowIndex);
            grdContainer.Children.Add(containerPanel);
            
            rowIndex++;
        }









        private void addEventCards()
        {
            rowIndex = 0;
            for (int i = 0; i < events.Count; i++)
            {
                grdContainer.RowDefinitions.Add(new RowDefinition());
            }
            foreach (var item in events)
            {
                AddCard(item);
            }
        }
    }
}
