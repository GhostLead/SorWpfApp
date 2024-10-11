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
    /// Interaction logic for PageAdmin.xaml
    /// </summary>
    public partial class PageAdmin : Page
    {
        public static string connectionString = "datasource = 127.0.0.1;port=3306;username=root;password=;database=fogadasok";
        private MySqlConnection? connection;
        Bettor user = UserAtkuldese.bejelentkezettFogado;
        ObservableCollection<Bettor> users = new ObservableCollection<Bettor>();
        int kartyaIndexRow = 0;
        int kartyaIndexCol = 0;
        int userCount = 0;
        public PageAdmin()
        {
            InitializeComponent();
            loadUsers();
            addEventCards();
        }

        private void loadUsers()
        {
            users = new ObservableCollection<Bettor>();

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string lekerdezesSzoveg = "SELECT * FROM Bettors ORDER BY BettorsID";

                MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                lekerdezes.CommandTimeout = 60;
                MySqlDataReader reader = lekerdezes.ExecuteReader();
                while (reader.Read())
                {
                    
                    if (reader.GetString(1) != user.username)
                    {
                        users.Add(new Bettor(reader));
                    }
                    
                }
                reader.Close();
                connection.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            userCount = users.Count();
        }

        private void AddCard(Bettor userCard)
        {
            int sorokSzama = grdUsers.RowDefinitions.Count;
            int oszlopokSzama = grdUsers.ColumnDefinitions.Count;

            //User Panel

            DropShadowEffect activeShadow = new DropShadowEffect
            {
                Color = Colors.Lime,
                BlurRadius = 15,
                ShadowDepth = 0,
                Opacity = 1
            };
            DropShadowEffect deactivatedShadow = new DropShadowEffect
            {
                Color = Colors.Red,
                BlurRadius = 15,
                ShadowDepth = 0,
                Opacity = 1
            };


            StackPanel userPanel = new StackPanel();
            userPanel.Width = 200;
            userPanel.Height = 350;
            userPanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2A2A2D"));
            userPanel.Margin = new Thickness(0,25,0,30);

            Image userImage = new Image();
            userImage.Width = 100;
            userImage.Height = 100;
            BitmapImage userBitmap = new BitmapImage();
            userBitmap.BeginInit();
            userBitmap.UriSource = new Uri("userimg.jpg", UriKind.RelativeOrAbsolute);
            userBitmap.EndInit();
            userImage.Source = userBitmap;
            userPanel.Children.Add(userImage);

            TextBox txtUsername = new TextBox();
            txtUsername.Text = userCard.username;
            txtUsername.FontSize = 20;
            txtUsername.FontWeight = FontWeights.Bold;
            txtUsername.HorizontalAlignment = HorizontalAlignment.Center;
            txtUsername.HorizontalContentAlignment = HorizontalAlignment.Center;
            txtUsername.Margin = new Thickness(0,5,0,0);
            userPanel.Children.Add(txtUsername);

            Label lblEmail = new Label();
            lblEmail.Content = "Email: ";
            lblEmail.FontSize = 14;
            lblEmail.Foreground = Brushes.White;
            lblEmail.FontWeight = FontWeights.DemiBold;
            userPanel.Children.Add(lblEmail);

            TextBox txtEmail = new TextBox();
            txtEmail.Text = userCard.email;
            txtEmail.TextWrapping = TextWrapping.Wrap;
            txtEmail.HorizontalContentAlignment = HorizontalAlignment.Center;
            txtEmail.Style = (Style)FindResource("LogRegTextbox");
            txtEmail.HorizontalAlignment = HorizontalAlignment.Center;
            userPanel.Children.Add(txtEmail);

            Label lblPassword = new Label();
            lblPassword.Content = "Jelszó: ";
            lblPassword.FontSize = 14;
            lblPassword.Foreground = Brushes.White;
            lblPassword.FontWeight = FontWeights.DemiBold;
            userPanel.Children.Add(lblPassword);

            TextBox txtPass = new TextBox();
            txtPass.Text = userCard.password;
            txtPass.TextWrapping = TextWrapping.Wrap;
            txtPass.HorizontalContentAlignment = HorizontalAlignment.Center;
            txtPass.Style = (Style)FindResource("LogRegTextbox");
            txtPass.HorizontalAlignment = HorizontalAlignment.Center;
            userPanel.Children.Add(txtPass);

            Label lblBalance = new Label();
            lblBalance.Content = "Egyenleg: ";
            lblBalance.FontSize = 14;
            lblBalance.Foreground = Brushes.White;
            lblBalance.FontWeight = FontWeights.DemiBold;
            userPanel.Children.Add(lblBalance);

            TextBox txtBalance = new TextBox();
            txtBalance.Text = userCard.balance.ToString();
            txtBalance.Width = 91.07;
            txtBalance.HorizontalContentAlignment = HorizontalAlignment.Center; 
            txtBalance.TextWrapping = TextWrapping.Wrap;
            txtBalance.Style = (Style)FindResource("LogRegTextbox");
            txtBalance.HorizontalAlignment = HorizontalAlignment.Center;
            userPanel.Children.Add(txtBalance);

            Button btnDeactivate = new Button();
            btnDeactivate.Width = 150;
            btnDeactivate.Height = 30;
            btnDeactivate.Style = (Style)FindResource("ShutdownButton");
            btnDeactivate.HorizontalAlignment = HorizontalAlignment.Center;
            if (userCard.isActive)
            {
                btnDeactivate.Content = "Deaktiválás";
                btnDeactivate.Background = new SolidColorBrush(Colors.Maroon);
                userPanel.Effect = activeShadow;

            }
            else
            {
                btnDeactivate.Content = "Aktiválás";
                btnDeactivate.Background = new SolidColorBrush(Colors.DarkGreen);
                userPanel.Effect = deactivatedShadow;
            }
            btnDeactivate.Margin = new Thickness(5);
            btnDeactivate.Click += (s, e) =>
            {
                if (!userCard.isActive)
                {
                    try
                    {
                        connection = new MySqlConnection(connectionString);
                        connection.Open();
                        string lekerdezesSzoveg = $"UPDATE `bettors` SET `IsActive`='1'WHERE Username = '{userCard.username}'";

                        MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                        lekerdezes.CommandTimeout = 60;
                        lekerdezes.ExecuteNonQuery();

                        connection.Close();
                        MessageBox.Show("Fiók sikeresen aktiválva!", "Fiók aktiválása", MessageBoxButton.OK, MessageBoxImage.Information);
                        btnDeactivate.Content = "Deaktiválás";
                        btnDeactivate.Background = new SolidColorBrush(Colors.Maroon);
                        userPanel.Effect = activeShadow;
                        userCard.isActive = true;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        connection = new MySqlConnection(connectionString);
                        connection.Open();
                        string lekerdezesSzoveg = $"UPDATE `bettors` SET `IsActive`='0'WHERE Username = '{userCard.username}'";

                        MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                        lekerdezes.CommandTimeout = 60;
                        lekerdezes.ExecuteNonQuery();

                        connection.Close();
                        MessageBox.Show("Fiók sikeresen deaktiválva!", "Fiók deaktiválása", MessageBoxButton.OK, MessageBoxImage.Information);
                        btnDeactivate.Content = "Aktiválás";
                        btnDeactivate.Background = new SolidColorBrush(Colors.DarkGreen);
                        userPanel.Effect = deactivatedShadow;
                        userCard.isActive = false;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
                
            };
            

            Button btnSave = new Button();
            btnSave.Width = 150;
            btnSave.Height = 30;
            btnSave.Content = "💾";
            btnSave.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE8A91E"));
            btnSave.FontSize = 19.5;
            btnSave.Style = (Style)FindResource("ThemeChanger");
            btnSave.Margin = new Thickness(0, 8.5, 0, 0);
            btnSave.Click += (s, e) =>
            {
                MessageBoxResult messageBoxresult = MessageBox.Show("Biztos hogy módosíja ennak a felhasználónak az adatait?", "Fiók módosítása", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxresult == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection = new MySqlConnection(connectionString);
                        connection.Open();
                        string lekerdezesSzoveg = $"UPDATE `bettors` SET `Username`='{txtUsername.Text}',`Password`='{txtPass.Text}',`Email`='{txtEmail.Text}', `Balance`='{txtBalance.Text}' WHERE Username = '{userCard.username}'";
                        MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                        lekerdezes.CommandTimeout = 60;
                        lekerdezes.ExecuteNonQuery();
                        connection.Close();
                        userCard.username = txtUsername.Text;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                    
                }
            };
            userPanel.Children.Add(btnSave);
            userPanel.Children.Add(btnDeactivate);



            Grid.SetColumn(userPanel,kartyaIndexCol);
            Grid.SetRow(userPanel, kartyaIndexRow);
            
            grdUsers.Children.Add(userPanel);
            if (kartyaIndexCol == 2)
            {
                kartyaIndexCol=0;
                kartyaIndexRow++;
            }
            else
            {
                kartyaIndexCol++;
            }
        }
        private void addEventCards()
        {
            kartyaIndexRow = 0;
            kartyaIndexCol = 0;
            
            for (int i = 0; i <= Math.Floor(Convert.ToDouble(userCount/3)); i++)
            {
                grdUsers.RowDefinitions.Add(new RowDefinition());
            }
            foreach (var user in users)
            {
                AddCard(user);
                
            }

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            grdUsers.Children.Clear();
            grdUsers.RowDefinitions.Clear();
            kartyaIndexRow = 0;
            kartyaIndexCol = 0;
            int numberOfUsers = 0;
            foreach (var item in users)
            {
                if (item.username.ToLower().StartsWith(txtSearch.Text.ToLower()))
                {
                    numberOfUsers++;    
                }
            }


            for (int i = 0; i <= Math.Floor(Convert.ToDouble(numberOfUsers / 3)); i++)
            {
                grdUsers.RowDefinitions.Add(new RowDefinition());
            }
            foreach (var user in users)
            {
                if (user.username.ToLower().StartsWith(txtSearch.Text.ToLower()))
                {
                    
                    AddCard(user);
                }

            }
        }
    }
}
