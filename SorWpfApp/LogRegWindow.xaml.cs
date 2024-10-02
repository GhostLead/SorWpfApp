using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for LogRegWindow.xaml
    /// </summary>
    public partial class LogRegWindow : Window
    {
        List<Bettors> fogadok = new();
        public string connectionString = "datasource = 127.0.0.1;port=3306;username=root;password=;database=fogadasok";
        private MySqlConnection? connection;
        static LogInPage logp;
        static RegistrationPage regp;
        bool vanHiba = false;
        string hibaÜzenet = "";
        string nev = "";
        string email = "";
        string jelszo = "";
        public LogRegWindow()
        {
            InitializeComponent(); 
            Container.Content = new LogInPage();
            loadUsers();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        

        private void btnLogReg_Click(object sender, RoutedEventArgs e)
        {
            if (Container.Content is RegistrationPage)
            {
                logp = new LogInPage();
                btnLogReg.Content = "Nincs fiókom / Regisztrálok";
                btnLogin.Content = "Bejelentkezés";
                btnLogin.Click -= (sender, e) => { checkRegistration(regp); };
                btnLogin.Click += (sender, e) => { checkUser(logp); };
                
            }
            else
            {
                regp = new RegistrationPage();
                Container.Content = regp;
                btnLogReg.Content = "Van fiókom / Bejelentkezek";
                btnLogin.Content = "Regisztráció";
                btnLogin.Click -= (sender, e) => { checkUser(logp); };
                btnLogin.Click += (sender, e) => { checkRegistration(regp); };
            }
        }
        private void loadUsers()
        {
            fogadok = new List<Bettors>();

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string lekerdezesSzoveg = "SELECT * FROM bettors ORDER BY BettorsID";

                MySqlCommand lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection);
                lekerdezes.CommandTimeout = 60;
                MySqlDataReader reader = lekerdezes.ExecuteReader();
                while (reader.Read())
                {
                    fogadok.Add(new Bettors(reader));
                }
                reader.Close();
                connection.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void checkUser(LogInPage page)
        {
            bool vanNev = false;
            bool vanJelszo = false;
            bool isAdmin = false;
            bool isOrganizer = false;
            foreach (var user in fogadok)
            {
                if (user.username == page.txtUsername.Text && user.password == page.passPassword.Password)
                {
                    vanNev = true;
                    vanJelszo = true;
                    if (user.username == "admin")
                    {
                        isAdmin = true;
                    }
                    else if (user.username == "organizer")
                    {
                        isOrganizer = true;
                    }
                    break;

                }
                else if (user.username == page.txtUsername.Text && user.password != page.passPassword.Password)
                {
                    vanNev = true;
                    break;

                }
                else if (user.username != page.txtUsername.Text && user.password == page.passPassword.Password)
                {
                    vanJelszo = true;
                    break;

                }


            }
            if (isAdmin)
            {
                MessageBox.Show("Ügyi vagy admin");
                //ApplicationWindow win = new ApplicationWindow();
                MainWindow mainwin = new MainWindow();
                mainwin.Show();
                Application.Current.MainWindow.Close();
            }
            else if (isOrganizer)
            {
                MessageBox.Show("Ügyi vagy Ork");
                // ApplicationWindow win = new ApplicationWindow();
                MainWindow mainwin = new MainWindow();
                mainwin.Show();
                Application.Current.MainWindow.Close();
            }
            else if (vanNev && vanJelszo)
            {
                //ApplicationWindow win = new ApplicationWindow();
                MainWindow mainwin = new MainWindow();
                mainwin.Show();
                Application.Current.MainWindow.Close();
            }
            else if (vanNev && !vanJelszo)
            {
                MessageBox.Show("Hibás a jelszó!");
            }
            else if (!vanNev && vanJelszo)
            {
                MessageBox.Show("Hibás a felhasználónév!");
            }
            else
            {
                MessageBox.Show("Nincs regisztálva ilyen fiók!");
            }
        }
        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                checkUser(logp);
            }
        }

        private void btnShutdown_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void userUpload()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string lekerdezesSzoveg = "INSERT INTO `bettors`(`Username`, `Password`, `Balance`, `Email`, `IsActive`) VALUES (@nev,@jelszo,'100',@email,true)";

                    using (var lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection))
                    {
                        lekerdezes.CommandTimeout = 60;

                        // Paraméterek hozzáadása
                        lekerdezes.Parameters.AddWithValue("@nev", nev);
                        lekerdezes.Parameters.AddWithValue("@jelszo", jelszo);
                        lekerdezes.Parameters.AddWithValue("@email", email);


                        lekerdezes.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void checkRegistration(RegistrationPage page)
        {
            nev = page.txtUsername.Text;
            email = page.txtEmail.Text;
            jelszo = page.passPassword.Text;
            string emailPattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            List<string> nevek = new();
            List<string> emailCimek = new();
            hibaÜzenet = "";
            vanHiba = false;
            foreach (var item in fogadok)
            {
                nevek.Add(item.username);
                emailCimek.Add(item.email);

            }
            if (nevek.Contains(nev) || nev == "")
            {
                hibaÜzenet += "\nMár van ilyen felhasználónév!";
                page.txtUsername.Focus();
                page.txtUsername.BorderBrush = Brushes.Red;
                vanHiba = true;
            }
            if (jelszo.Length < 8)
            {
                hibaÜzenet += "\nLegalább 8 karakternek kell lennie a jelszónak!";
                page.passPassword.Focus();
                page.passPassword.BorderBrush = Brushes.Red;
                vanHiba = true;
            }
            if (emailCimek.Contains(email) || email == "")
            {
                hibaÜzenet += "\nMár van ilyen email cím hozzárendelve egy fiókhoz!";
                page.txtEmail.Focus();
                page.txtEmail.BorderBrush = Brushes.Red;
                vanHiba = true;

            }
            if (!Regex.IsMatch(email, emailPattern))
            {
                hibaÜzenet += "\nHibás az email formátuma!";
                page.txtEmail.Focus();
                page.txtEmail.BorderBrush = Brushes.Red;
                vanHiba = true;
            }
            if (vanHiba)
            {
                MessageBox.Show(hibaÜzenet);
            }
            else
            {
                userUpload();
                MessageBox.Show("Köszönjük a regisztrálását!\nKöszönetünk jeleként jóváírtunk 100$ kezdő összeget a fiókján!");
                //ApplicationWindow appwin = new ApplicationWindow();
                MainWindow win = new MainWindow();
                win.Show();
                Application.Current.MainWindow.Close();
            }

        }
    }
}
