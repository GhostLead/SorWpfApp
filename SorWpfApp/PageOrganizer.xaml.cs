using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
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

        //private void AddCard(Event eventCard)
        //{
        //    DropShadowEffect shadowEffect = new DropShadowEffect
        //    {
        //        Color = Colors.Black,
        //        BlurRadius = 10,
        //        ShadowDepth = 0,
        //        Opacity = 1
        //    };



        //    StackPanel containerPanel = new StackPanel();
        //    containerPanel.Width = 750;
        //    containerPanel.Height = 110;
        //    containerPanel.Margin = new Thickness(20);
        //    containerPanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2A2A2D"));


        //    Label lblCategory = new Label();
        //    lblCategory.Content = eventCard.Category;
        //    lblCategory.FontSize = 20;
        //    lblCategory.Foreground = Brushes.White;
        //    lblCategory.FontWeight = FontWeights.Bold;
        //    lblCategory.FontStyle = FontStyles.Italic;

        //    containerPanel.Children.Add(lblCategory);

        //    Grid grdLabels = new Grid();
        //    grdLabels.ColumnDefinitions.Add(new ColumnDefinition());
        //    grdLabels.ColumnDefinitions.Add(new ColumnDefinition());
        //    grdLabels.ColumnDefinitions.Add(new ColumnDefinition());

        //    TextBox txtGp = new TextBox();
        //    txtGp.Text = eventCard.EventName;
        //    txtGp.FontSize = 15;
        //    txtGp.IsEnabled = false;
        //    Grid.SetColumn(txtGp, 0);
        //    grdLabels.Children.Add(txtGp);

        //    DatePicker dpDate = new DatePicker();
        //    dpDate.SelectedDate = eventCard.EventDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
        //    dpDate.FontSize = 15;
        //    dpDate.IsEnabled = false;
        //    Grid.SetColumn(dpDate, 1);
        //    grdLabels.Children.Add(dpDate);

        //    TextBox txtCountry = new TextBox();
        //    txtCountry.Text = eventCard.Location;
        //    txtCountry.FontSize = 15;
        //    txtCountry.IsEnabled = false;
        //    Grid.SetColumn(txtCountry, 2);
        //    grdLabels.Children.Add(txtCountry);
        //    containerPanel.Children.Add(grdLabels);


        //    Grid grdButtons = new Grid();
        //    grdButtons.ColumnDefinitions.Add(new ColumnDefinition());
        //    grdButtons.ColumnDefinitions.Add(new ColumnDefinition());

        //    Button btnDelete = new Button();
        //    btnDelete.Content = "Törlés";
        //    btnDelete.Width = 150;
        //    btnDelete.Height = 25;
        //    btnDelete.Margin = new Thickness(0, 5, 0, 0);
        //    btnDelete.Style = (Style)FindResource("GeneralButton");
        //    btnDelete.Background = new SolidColorBrush(Brushes.Red.Color);
        //    btnDelete.Foreground = new SolidColorBrush(Brushes.White.Color);
        //    btnDelete.Click += (s, e) =>
        //    {
        //        MessageBoxResult messageBoxresult = MessageBox.Show("Biztos hogy törli ezt az eseményt?", "Esemény törlése", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (messageBoxresult == MessageBoxResult.Yes)
        //        {
        //            txtCountry.IsEnabled = false;
        //            txtGp.IsEnabled = false;
        //            dpDate.IsEnabled = false;
        //            try
        //            {
        //                connection = new MySqlConnection(connectionString);
        //                connection.Open();
        //                string lekerdezesSzoveg = $"DELETE FROM `events` WHERE EventID = '{eventCard.EventID}'";
        //                MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
        //                lekerdezes.CommandTimeout = 60;
        //                lekerdezes.ExecuteNonQuery();
        //                connection.Close();
        //                events.Clear();
        //                grdContainer.Children.Clear();
        //                loadEvents();
        //                addEventCards();
        //            }
        //            catch (Exception ex)
        //            {

        //                MessageBox.Show(ex.Message);
        //            }
        //        };

        //    };
        //        Grid.SetColumn(btnDelete, 0);
        //        grdButtons.Children.Add(btnDelete);

        //        Button btnEdit = new Button();
        //        btnEdit.Content = "Szerkesztés";
        //        btnEdit.Width = 150;
        //        btnEdit.Height = 25;
        //        btnEdit.Margin = new Thickness(0, 5, 0, 0);
        //        btnEdit.Style = (Style)FindResource("GeneralButton");
        //        btnEdit.Background = new SolidColorBrush(Brushes.Green.Color);
        //        btnEdit.Foreground = new SolidColorBrush(Brushes.White.Color);
        //        btnEdit.Click += (s, e) =>
        //        {
        //            if (txtCountry.IsEnabled)
        //            {
        //                MessageBoxResult messageBoxresult = MessageBox.Show("Biztos hogy módosíja ezt az eseményt?", "Esemény módosítása", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //                if (messageBoxresult == MessageBoxResult.Yes)
        //                {
        //                    txtCountry.IsEnabled = false;
        //                    txtGp.IsEnabled = false;
        //                    dpDate.IsEnabled = false;
        //                    try
        //                    {
        //                        connection = new MySqlConnection(connectionString);
        //                        connection.Open();
        //                        string lekerdezesSzoveg = $"UPDATE `events` SET `EventName`='{txtGp.Text}',`EventDate`='{dpDate.SelectedDate.Value.Year}-{dpDate.SelectedDate.Value.Month}-{dpDate.SelectedDate.Value.Day}', `Location`='{txtCountry.Text}' WHERE EventID = '{eventCard.EventID}'";
        //                        MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
        //                        lekerdezes.CommandTimeout = 60;
        //                        lekerdezes.ExecuteNonQuery();
        //                        connection.Close();
        //                        events.Clear();
        //                        grdContainer.Children.Clear();
        //                        loadEvents();
        //                        addEventCards();
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                        MessageBox.Show(ex.Message);
        //                    }

        //                }



        //            }
        //            else
        //            {

        //                txtCountry.IsEnabled = true;
        //                txtGp.IsEnabled = true;
        //                dpDate.IsEnabled = true;

        //            }
        //        };
        //        Grid.SetColumn(btnEdit, 1);
        //        grdButtons.Children.Add(btnEdit);

        //        containerPanel.Children.Add(grdButtons);
        //    Border border = new Border();
        //    border.CornerRadius = new CornerRadius(10);
        //    border.Effect = shadowEffect;
        //    border.Child = containerPanel;

        //    Grid.SetRow(border, rowIndex);
        //        grdContainer.Children.Add(border);

        //        rowIndex++;

        //}

        private void AddCard(Event eventCard)
        {
            // Create the main Border
            Border border = new Border
            {
                Width = 700,
                Height = 150,
                CornerRadius = new CornerRadius(5),
                Style = (Style)FindResource("borderback2"),
                Margin = new Thickness(20),
                Effect = new DropShadowEffect
                {
                    BlurRadius = 10,
                    ShadowDepth = 0,
                    Opacity = 1,
                    Color = Colors.Black
                }
            };

            // StackPanel inside Border
            StackPanel mainStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0)
            };
            border.Child = mainStackPanel;

            // Left StackPanel (Category & Event Name)
            StackPanel leftStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(75, 0, 0, 0)
            };
            mainStackPanel.Children.Add(leftStackPanel);

            // Category Label and ComboBox
            leftStackPanel.Children.Add(new Label { Content = "Kategória", FontSize = 17 });
            TextBox txtCategory = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(2),
                Width = 150,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(5),
                IsEnabled = false,
                Text = eventCard.Category
            };
            leftStackPanel.Children.Add(txtCategory);

            // Event Name Label and TextBox
            leftStackPanel.Children.Add(new Label { Content = "Esemény Neve", FontSize = 17 });
            TextBox txtEventName = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(2),
                Width = 150,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(5),
                IsEnabled = false,
                Text= eventCard.EventName
            };
            leftStackPanel.Children.Add(txtEventName);

            // Middle StackPanel (Date & Location)
            StackPanel middleStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(40, 0, 0, 0)
            };
            mainStackPanel.Children.Add(middleStackPanel);

            // Date Label and DatePicker
            middleStackPanel.Children.Add(new Label { Content = "Dátum", FontSize = 17 });
            DatePicker dpStartDate = new DatePicker
            {
                IsTodayHighlighted = true,
                Margin = new Thickness(0, 5, 0, 0),
                Height = 30,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 150,
                IsEnabled = false,
                SelectedDate = eventCard.EventDate.ToDateTime(TimeOnly.Parse("10:00 PM"))
            };
            middleStackPanel.Children.Add(dpStartDate);

            // Location Label and TextBox
            middleStackPanel.Children.Add(new Label { Content = "Helyszín", FontSize = 17 });
            TextBox txtLocation = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(2),
                Width = 150,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(5),
                IsEnabled = false,
                Text = eventCard.Location
            };
            middleStackPanel.Children.Add(txtLocation);

            // Right StackPanel (Event Description and Button)
            StackPanel rightStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(40, 0, 0, 0)
            };
            mainStackPanel.Children.Add(rightStackPanel);

            // Event Description Label and TextBox
            rightStackPanel.Children.Add(new Label { Content = "Esemény leírása", FontSize = 17 });
            TextBox txtDesc = new TextBox
            {
                Text = EventManager.Pliz.Where(x=>x.Key.EventID == eventCard.EventID).First().Value,
                Width = 150,
                Height = 50,
                FontSize = 15,
                TextWrapping = TextWrapping.Wrap,
                Padding = new Thickness(2),
                VerticalContentAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(5),
                IsEnabled = false
            };
            rightStackPanel.Children.Add(txtDesc);

            StackPanel buttonStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                
            };
            // Add Event Button
            Button btnDelete = new Button
            {
                Content = "Törlés",
                Width = 95,
                Height = 35,
                Style = (Style)FindResource("ShutdownButton"),
                Margin = new Thickness(5, 10, 0, 0),
                Background = Brushes.Black,
                Foreground = Brushes.Red,
                BorderBrush = Brushes.Red
            }; 
            btnDelete.Click += (s, e) =>
            {
                MessageBoxResult messageBoxresult = MessageBox.Show("Biztos hogy törli ezt az eseményt?", "Esemény törlése", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxresult == MessageBoxResult.Yes)
                {
                    
                    try
                    {
                        connection = new MySqlConnection(connectionString);
                        connection.Open();
                        string lekerdezesSzoveg = $"DELETE FROM `events` WHERE EventID = '{eventCard.EventID}'";
                        MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                        lekerdezes.CommandTimeout = 60;
                        lekerdezes.ExecuteNonQuery();
                        connection.Close();
                        events.Clear();
                        grdContainer.Children.Clear();
                        loadEvents();
                        addEventCards();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Erre az eseményre már fogadtak, nem lehet törölni!", "Törlés error", MessageBoxButton.OK,MessageBoxImage.Error);
                    }
                };

            };
            buttonStackPanel.Children.Add(btnDelete);
            Button btnEdit = new Button
            {
                Content = "Szerkesztés",
                Width = 95,
                Height = 35,
                Style = (Style)FindResource("ShutdownButton"),
                Margin = new Thickness(5, 10, 0, 0),
                Background = Brushes.Black,
                Foreground = Brushes.Lime,
                BorderBrush = Brushes.Lime
            };
            btnEdit.Click += (s,e) =>
            {
                if (txtLocation.IsEnabled)
                {
                    MessageBoxResult messageBoxresult = MessageBox.Show("Biztos hogy módosíja ezt az eseményt?", "Esemény módosítása", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxresult == MessageBoxResult.Yes)
                    {
                        txtLocation.IsEnabled = false;
                        txtEventName.IsEnabled = false;
                        dpStartDate.IsEnabled = false;
                        txtDesc.IsEnabled = false;
                        try
                        {
                            connection = new MySqlConnection(connectionString);
                            connection.Open();
                            string lekerdezesSzoveg = $"UPDATE `events` SET `EventName`='{txtEventName.Text}',`EventDate`='{dpStartDate.SelectedDate.Value.Year}-{dpStartDate.SelectedDate.Value.Month}-{dpStartDate.SelectedDate.Value.Day}', `Location`='{txtLocation.Text}' WHERE EventID = '{eventCard.EventID}'";
                            MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                            lekerdezes.CommandTimeout = 60;
                            lekerdezes.ExecuteNonQuery();
                            connection.Close();
                            events.Clear();
                            grdContainer.Children.Clear();
                            loadEvents();
                            EventManager.Pliz[eventCard] = txtDesc.Text;
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

                    txtLocation.IsEnabled = true;
                    txtEventName.IsEnabled = true;
                    dpStartDate.IsEnabled = true;
                    txtDesc.IsEnabled = true;

                }
            };
            buttonStackPanel.Children.Add(btnEdit);
            rightStackPanel.Children.Add(buttonStackPanel);
            // Add the border to the main window or desired container

            grdContainer.Children.Add(border);
        }


        private void addEventCards()
        {
            rowIndex = 0;
            for (int i = 0; i < events.Count; i++)
            {
                //grdContainer.RowDefinitions.Add(new RowDefinition());
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
                    string lekerdezesSzoveg = $"INSERT INTO `events`(`EventName`, `EventDate`, `Category`, `Location`) VALUES ('{txtEventName.Text}','{dpStartDate.SelectedDate.Value.Year}-{dpStartDate.SelectedDate.Value.Month}-{dpStartDate.SelectedDate.Value.Day}','{txtCategory.Text}','{txtLocation.Text}')";
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

        
    }
}
