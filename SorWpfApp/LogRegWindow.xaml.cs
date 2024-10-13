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
using System.Security.Cryptography;

namespace SorWpfApp
{
    /// <summary>
    /// Interaction logic for LogRegWindow.xaml
    /// </summary>
    public partial class LogRegWindow : Window
    {
        static List<Bettor> fogadok = new();
        public static string connectionString = dbConnection.connection;
        private static MySqlConnection? connection;
        static LogInPage logp = new LogInPage();
        static RegistrationPage regp = new RegistrationPage();
        static bool vanHiba = false;
        static string hibaÜzenet = "";
        static string nev = "";
        static string email = "";
        static string jelszo = "";
        static int egyenleg = 0;
        bool hasznalva = false;
        static bool vanNev = false;
        static bool vanJelszo = false;
        
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
            loadUsers();
            if (!hasznalva || !vanJelszo || !vanNev)
            {
                HandleLogin(sender,e);
                hasznalva = true;
            }
            
        }

        

        private void btnLogReg_Click(object sender, RoutedEventArgs e)
        {
            hasznalva = true;
            vanJelszo = true;
            vanNev = true;
            
            if (Container.Content is RegistrationPage)
            {
                logp = new LogInPage();
                Container.Content = logp;
                btnLogReg.Content = "Nincs fiókom / Regisztrálok";
                btnLogin.Content = "Bejelentkezés";
                btnLogin.Click -= HandleRegister;
                btnLogin.Click += HandleLogin;
                

            }
            else
            {
                regp = new RegistrationPage();
                Container.Content = regp;
                btnLogReg.Content = "Van fiókom / Bejelentkezek";
                btnLogin.Content = "Regisztráció";
                btnLogin.Click -= HandleLogin;
                btnLogin.Click += HandleRegister;
            }
        }

        private void HandleLogin(object sender, RoutedEventArgs e)
        {
            checkUser(logp);
        }
        private void HandleRegister(object sender, RoutedEventArgs e)
        {
            checkRegistration(regp);
        }
        public static void HandleLoginInput(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                checkUser(logp);
            }
        }
        public static void HandleRegisterInput(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                checkRegistration(regp);
                
            }
        }
        private static void loadUsers()
        {
            fogadok = new List<Bettor>();

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
                    fogadok.Add(new Bettor(reader));
                }
                reader.Close();
                connection.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private static void checkUser(LogInPage page)
        {
            vanNev = false;
            vanJelszo = false;
            bool isAdmin = false;
            bool isOrganizer = false;
            string loginUser = LoginClass.Username;
            string loginPassword = LoginClass.Password;
            loadUsers();
            foreach (var user in fogadok)
            {
                if (user.username == loginUser && user.password == loginPassword)
                {
                    vanNev = true;
                    vanJelszo = true;
                    if (user.username.Contains("admin"))
                    {
                        isAdmin = true;
                    }
                    else if (user.username.Contains("organizer"))
                    {
                        isOrganizer = true;
                    }
                    UserAtkuldese.bejelentkezettFogado = user;
                    break;

                }
                else if (user.username == loginUser && user.password != loginPassword)
                {
                    vanNev = true;
                    break;

                }
                else if (user.username != loginUser && user.password == loginPassword)
                {
                    vanJelszo = true;
                    break;

                }
                

            }
            if (isAdmin)
            {


                MainWindow mainwin = new MainWindow();
                mainwin.Show();
                var logRegWindow = Application.Current.Windows.OfType<LogRegWindow>().FirstOrDefault();
                if (logRegWindow != null)
                {
                    logRegWindow.Close();
                }
            }
            else if (isOrganizer)
            {


                MainWindow mainwin = new MainWindow();
                mainwin.Show();
                var logRegWindow = Application.Current.Windows.OfType<LogRegWindow>().FirstOrDefault();
                if (logRegWindow != null)
                {
                    logRegWindow.Close();
                }
            }
            else if (vanNev && vanJelszo)
            {

                MainWindow mainwin = new MainWindow();
                mainwin.Show();
                var logRegWindow = Application.Current.Windows.OfType<LogRegWindow>().FirstOrDefault();
                if (logRegWindow != null)
                {
                    logRegWindow.Close();
                }
            }
            else if (vanNev && !vanJelszo)
            {
                MessageBox.Show("Hibás a jelszó!","Hiba!",MessageBoxButton.OK,MessageBoxImage.Stop);
            }
            else if (!vanNev && vanJelszo)
            {
                MessageBox.Show("Hibás a felhasználónév!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                MessageBox.Show("Nincs regisztálva ilyen fiók!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        
        private static void userUpload()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string lekerdezesSzoveg = "INSERT INTO `Bettors`(`Username`, `Password`, `Balance`, `Email`, `IsActive`) VALUES (@nev,@jelszo,@egyenleg,@email,true)";

                    using (var lekerdezes = new MySqlCommand(lekerdezesSzoveg, connection))
                    {
                        lekerdezes.CommandTimeout = 60;

                        // Paraméterek hozzáadása
                        lekerdezes.Parameters.AddWithValue("@nev", nev);
                        lekerdezes.Parameters.AddWithValue("@jelszo", jelszo);
                        lekerdezes.Parameters.AddWithValue("@email", email);
                        lekerdezes.Parameters.AddWithValue("@egyenleg", egyenleg);


                        lekerdezes.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public static void checkRegistration(RegistrationPage page)
        {
            nev = RegisterClass.Username;
            email = RegisterClass.Email;
            jelszo = RegisterClass.Password;
            egyenleg = RegisterClass.Balance;
            string emailPattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            List<string> nevek = new();
            List<string> emailCimek = new();
            List<string> jelszavak = new();
            hibaÜzenet = "";
            vanHiba = false;
            foreach (var item in fogadok)
            {
                nevek.Add(item.username);
                emailCimek.Add(item.email);
                jelszavak.Add(item.password);

            }
            if (nevek.Contains(nev) || nev == "")
            {
                hibaÜzenet += "\nMár van ilyen felhasználónév!";
                page.txtUsername.Focus();
                page.txtUsername.BorderThickness = new Thickness(2);
                page.txtUsername.BorderBrush = Brushes.Red;
                vanHiba = true;
            }
            if (nev == null)
            {
                hibaÜzenet += "\nÜres a felhasználónév mező";
            }
            if (jelszavak.Contains(jelszo))
            {
                hibaÜzenet += "\nMár létezik ilyen jelszó, kérem adjon másikat!";
                page.passPassword.Focus();
                page.passPassword.BorderThickness = new Thickness(2);
                page.passPassword.BorderBrush = Brushes.Red;
                vanHiba = true;
            }
            if ( jelszo == null || jelszo.Length < 8 || jelszo == "")
            {
                hibaÜzenet += "\nLegalább 8 karakternek kell lennie a jelszónak!";
                page.passPassword.Focus();
                page.passPassword.BorderThickness = new Thickness(2);
                page.passPassword.BorderBrush = Brushes.Red;
                vanHiba = true;
            }
            if (emailCimek.Contains(email) || email == "")
            {
                hibaÜzenet += "\nMár van ilyen email cím hozzárendelve egy fiókhoz!";
                page.txtEmail.Focus();
                page.txtEmail.BorderThickness = new Thickness(2);
                page.txtEmail.BorderBrush = Brushes.Red;
                vanHiba = true;

            }
            if (email == null || !Regex.IsMatch(email, emailPattern))
            {
                hibaÜzenet += "\nHibás az email formátuma!";
                page.txtEmail.Focus();
                page.txtEmail.BorderThickness = new Thickness(2);
                page.txtEmail.BorderBrush = Brushes.Red;
                vanHiba = true;
            }
            if (vanHiba)
            {
                MessageBox.Show(hibaÜzenet, "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                userUpload();
                
                MessageBox.Show("Köszönjük a regisztrálását!","",MessageBoxButton.OK,MessageBoxImage.Information);
                ////ApplicationWindow appwin = new ApplicationWindow();
                var logRegWindow = Application.Current.Windows.OfType<LogRegWindow>().FirstOrDefault();
                if (logRegWindow != null)
                {
                    logRegWindow.Container.Content = new LogInPage();
                    logRegWindow.btnLogReg.Content = "Nincs fiókom / Regisztrálok";
                    logRegWindow.btnLogin.Content = "Bejelentkezés";
                    logRegWindow.btnLogin.Click -= logRegWindow.HandleRegister;
                    logRegWindow.btnLogin.Click += logRegWindow.HandleLogin;
                }

                
            }

        }
        
    }



}
