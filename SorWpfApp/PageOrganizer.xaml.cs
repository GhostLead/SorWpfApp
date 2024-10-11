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
            dpStartDate.DisplayDateStart = DateTime.Today;
            dpStartDate.SelectedDate = DateTime.Today;
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
            // DropShadowEffect for container panel
            DropShadowEffect shadowEffect = new DropShadowEffect
            {
                Color = Colors.Black,
                BlurRadius = 10,
                ShadowDepth = 0,
                Opacity = 1
            };

            // Main container StackPanel
            StackPanel containerPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Width = 750,
                Height = 110,
                Margin = new Thickness(20),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2A2A2D"))
            };

            // Category Label
            Label lblCategory = new Label
            {
                Content = eventCard.Category,
                FontSize = 20,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                FontStyle = FontStyles.Italic,
                Margin = new Thickness(10, 0, 0, 0)
            };
            containerPanel.Children.Add(lblCategory);

            // Grid for Labels and Inputs
            Grid grdLabels = new Grid();
            grdLabels.ColumnDefinitions.Add(new ColumnDefinition());
            grdLabels.ColumnDefinitions.Add(new ColumnDefinition());
            grdLabels.ColumnDefinitions.Add(new ColumnDefinition());

            // Event Name TextBox
            TextBox txtGp = new TextBox
            {
                Text = eventCard.EventName,
                FontSize = 15,
                IsEnabled = false,
                Margin = new Thickness(5)
            };
            Grid.SetColumn(txtGp, 0);
            grdLabels.Children.Add(txtGp);

            // Event Date DatePicker
            DatePicker dpDate = new DatePicker
            {
                SelectedDate = eventCard.EventDate.ToDateTime(TimeOnly.Parse("10:00 PM")),
                FontSize = 15,
                IsEnabled = false,
                Margin = new Thickness(5)
            };
            Grid.SetColumn(dpDate, 1);
            grdLabels.Children.Add(dpDate);

            // Location TextBox
            TextBox txtCountry = new TextBox
            {
                Text = eventCard.Location,
                FontSize = 15,
                IsEnabled = false,
                Margin = new Thickness(5)
            };
            Grid.SetColumn(txtCountry, 2);
            grdLabels.Children.Add(txtCountry);
            containerPanel.Children.Add(grdLabels);

            // Grid for Action Buttons
            Grid grdButtons = new Grid();
            grdButtons.ColumnDefinitions.Add(new ColumnDefinition());
            grdButtons.ColumnDefinitions.Add(new ColumnDefinition());

            // Delete Button
            Button btnDelete = new Button
            {
                Content = "Törlés",
                Width = 150,
                Height = 25,
                Margin = new Thickness(0, 5, 5, 0),
                Background = new SolidColorBrush(Colors.Red),
                Foreground = Brushes.White,
                Style = (Style)FindResource("GeneralButton")
            };
            btnDelete.Click += (s, e) =>
            {
                MessageBoxResult messageBoxresult = MessageBox.Show("Biztos hogy törli ezt az eseményt?", "Esemény törlése", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxresult == MessageBoxResult.Yes)
                {
                    try
                    {
                        txtCountry.IsEnabled = false;
                        txtGp.IsEnabled = false;
                        dpDate.IsEnabled = false;

                        // Database deletion logic
                        connection = new MySqlConnection(connectionString);
                        connection.Open();
                        string deleteQuery = $"DELETE FROM `events` WHERE EventID = '{eventCard.EventID}'";
                        MySqlCommand command = new MySqlCommand(deleteQuery, connection);
                        command.CommandTimeout = 60;
                        command.ExecuteNonQuery();
                        connection.Close();

                        // Refresh UI after deletion
                        events.Clear();
                        grdContainer.Children.Clear();
                        loadEvents();
                        addEventCards();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            };
            Grid.SetColumn(btnDelete, 0);
            grdButtons.Children.Add(btnDelete);

            // Edit Button
            Button btnEdit = new Button
            {
                Content = "Szerkesztés",
                Width = 150,
                Height = 25,
                Margin = new Thickness(5, 5, 0, 0),
                Background = new SolidColorBrush(Colors.Green),
                Foreground = Brushes.White,
                Style = (Style)FindResource("GeneralButton")
            };
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
                            // Database update logic
                            connection = new MySqlConnection(connectionString);
                            connection.Open();
                            string updateQuery = $"UPDATE `events` SET `EventName`='{txtGp.Text}',`EventDate`='{dpDate.SelectedDate.Value:yyyy-MM-dd}', `Location`='{txtCountry.Text}' WHERE EventID = '{eventCard.EventID}'";
                            MySqlCommand command = new MySqlCommand(updateQuery, connection);
                            command.CommandTimeout = 60;
                            command.ExecuteNonQuery();
                            connection.Close();

                            // Refresh UI after update
                            events.Clear();
                            grdContainer.Children.Clear();
                            loadEvents();
                            addEventCards();
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

            // Border for shadow effect
            Border border = new Border
            {
                CornerRadius = new CornerRadius(10),
                Effect = shadowEffect,
                Child = containerPanel
            };

            // Add to main grid container with row tracking
            Grid.SetRow(border, rowIndex);
            grdContainer.Children.Add(border);
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

        private void btnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            if (txtEventName.Text != null && txtEventName.Text != "" && txtLocation.Text != null && txtLocation.Text != "")
            {
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    string lekerdezesSzoveg = $"INSERT INTO `events`(`EventName`, `EventDate`, `Category`, `Location`) VALUES ('{txtEventName.Text}','{dpStartDate.SelectedDate.Value.Year}-{dpStartDate.SelectedDate.Value.Month}-{dpStartDate.SelectedDate.Value.Day}','{cbCategory.Text}','{txtLocation.Text}')";
                    MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                    lekerdezes.CommandTimeout = 60;
                    lekerdezes.ExecuteNonQuery();
                    connection.Close();
                    events.Clear();
                    grdContainer.Children.Clear();
                    loadEvents();
                    addEventCards();
                    MessageBox.Show("Az esemény sikeresen fel lett véve!", "Esemény felvétele", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Egyik mezőt se hagyja üresen!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





        private StackPanel AddCard()
        {
            // Main StackPanel
            StackPanel mainStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0),
            };

            // Apply DynamicResource to Style if needed in code-behind, like:
            // mainStackPanel.Style = (Style)FindResource("stackback");

            // Category and Event Name StackPanel
            StackPanel categoryStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(95, 0, 0, 0)
            };

            // Category Label and ComboBox
            Label lblCategory = new Label
            {
                Content = "Kategória",
                FontSize = 17
            };

            ComboBox cbCategory = new ComboBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(2),
                Width = 150,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(5)
            };
            cbCategory.Items.Add(new ComboBoxItem { Content = "Race", IsSelected = true });

            // Add Category Label and ComboBox to categoryStackPanel
            categoryStackPanel.Children.Add(lblCategory);
            categoryStackPanel.Children.Add(cbCategory);

            // Event Name Label and TextBox
            Label lblEventName = new Label
            {
                Content = "Esemény Neve",
                FontSize = 17
            };

            TextBox txtEventName = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(2),
                Width = 150,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(5)
            };

            // Add Event Name Label and TextBox to categoryStackPanel
            categoryStackPanel.Children.Add(lblEventName);
            categoryStackPanel.Children.Add(txtEventName);

            // Date and Location StackPanel
            StackPanel dateLocationStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(40, 0, 0, 0)
            };

            // Date Label and DatePicker
            Label lblDate = new Label
            {
                Content = "Dátum",
                FontSize = 17
            };

            DatePicker dpStartDate = new DatePicker
            {
                IsTodayHighlighted = true,
                Margin = new Thickness(0, 5, 0, 0),
                Height = 30,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 150
            };

            // Add Date Label and DatePicker to dateLocationStackPanel
            dateLocationStackPanel.Children.Add(lblDate);
            dateLocationStackPanel.Children.Add(dpStartDate);

            // Location Label and TextBox
            Label lblLocation = new Label
            {
                Content = "Helyszín",
                FontSize = 17
            };

            TextBox txtLocation = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(2),
                Width = 150,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(5)
            };

            // Add Location Label and TextBox to dateLocationStackPanel
            dateLocationStackPanel.Children.Add(lblLocation);
            dateLocationStackPanel.Children.Add(txtLocation);

            // Description and Button StackPanel
            StackPanel descButtonStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(40, 0, 0, 0)
            };

            // Description Label and TextBox
            Label lblDescription = new Label
            {
                Content = "Esemény leírása",
                FontSize = 17
            };

            TextBox txtDesc = new TextBox
            {
                Width = 150,
                Height = 50,
                FontSize = 15,
                TextWrapping = TextWrapping.Wrap,
                Padding = new Thickness(2),
                VerticalContentAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(5)
            };

            // Add Description Label and TextBox to descButtonStackPanel
            descButtonStackPanel.Children.Add(lblDescription);
            descButtonStackPanel.Children.Add(txtDesc);

            // Button for adding event
            Button btnAddEvent = new Button
            {
                Content = "Hozzáadás",
                Width = 95,
                Height = 35,
                Margin = new Thickness(5, 10, 0, 0),
                Background = new SolidColorBrush(Colors.Black),
                Foreground = new SolidColorBrush(Colors.Lime),
                BorderBrush = new SolidColorBrush(Colors.Lime)
            };

            // Event handler for button click
            btnAddEvent.Click += btnAddEvent_Click;

            // Add button to descButtonStackPanel
            descButtonStackPanel.Children.Add(btnAddEvent);

            // Adding all sub-stack panels to the main stack panel
            mainStackPanel.Children.Add(categoryStackPanel);
            mainStackPanel.Children.Add(dateLocationStackPanel);
            mainStackPanel.Children.Add(descButtonStackPanel);

            return mainStackPanel;
        }






    }
}
