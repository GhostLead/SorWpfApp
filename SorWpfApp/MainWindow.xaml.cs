using Microsoft.VisualBasic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SorWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool IsDarkModeOn = true;
        static Bettor bejelentkezettBettor;
        public MainWindow()
        {
            InitializeComponent();
            btnFiok_Click(btnFiok, new RoutedEventArgs());
            bejelentkezettBettor = UserAtkuldese.bejelentkezettFogado;
            lblUsername.Content = bejelentkezettBettor.username;
            lblBalance.Content = bejelentkezettBettor.balance+" Ft";
            if (bejelentkezettBettor.username.ToLower().Contains("admin"))
            {
                btnAdmin.Visibility = Visibility.Visible;
                btnOrganizer.Visibility = Visibility.Hidden;
            }
            else if (bejelentkezettBettor.username.ToLower().Contains("organizer"))
            {
                btnAdmin.Visibility = Visibility.Collapsed;
                btnOrganizer.Visibility = Visibility.Visible;
            }
            else {
                btnAdmin.Visibility = Visibility.Hidden;
                btnOrganizer.Visibility = Visibility.Hidden;

            }
            if (!bejelentkezettBettor.isActive)
            {
                btnFogadas.Visibility = Visibility.Hidden;
                btnThemeToggle.Content = "🌙";
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("LightTheme.xaml", UriKind.Relative)
                });
                window.Background = Brushes.White;
                navbar.Background = Brushes.White;
                IsDarkModeOn = false;
                btnThemeToggle.Visibility = Visibility.Hidden;
            }
        }

        private void megnyit_Click(object sender, RoutedEventArgs e)
        {
            LogRegWindow logRegWindow = new LogRegWindow();
            logRegWindow.Show();
            this.Close();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnFogadas_Click(object sender, RoutedEventArgs e)
        {
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Opacity = 1;
            dropShadowEffect.BlurRadius = 10;
            dropShadowEffect.ShadowDepth = 1;
            dropShadowEffect.Color = Colors.DarkOrange;
            btnFogadas.Effect = dropShadowEffect;
            btnAdmin.Effect = null;
            btnOrganizer.Effect = null;
            btnFiok.Effect = null;
            Container.Content = new PageFogadas();
        }

        private void btnFiok_Click(object sender, RoutedEventArgs e)
        {
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Opacity = 1;
            dropShadowEffect.BlurRadius = 10;
            dropShadowEffect.ShadowDepth = 1;
            dropShadowEffect.Color = Colors.DarkOrange;
            btnFiok.Effect = dropShadowEffect;
            btnFogadas.Effect = null;
            btnAdmin.Effect = null;
            btnOrganizer.Effect = null;
            Container.Content = new PageAccount();
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Opacity = 1;
            dropShadowEffect.BlurRadius = 10;
            dropShadowEffect.ShadowDepth = 1;
            dropShadowEffect.Color = Colors.DarkOrange;
            btnAdmin.Effect = dropShadowEffect;
            btnFiok.Effect= null;
            btnFogadas.Effect= null;
            btnOrganizer.Effect = null;
            Container.Content = new PageAdmin();
        }

        private void btnOrganizer_Click(object sender, RoutedEventArgs e)
        {
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Opacity = 1;
            dropShadowEffect.BlurRadius = 10;
            dropShadowEffect.ShadowDepth = 1;
            dropShadowEffect.Color = Colors.DarkOrange;
            btnOrganizer.Effect = dropShadowEffect;
            btnAdmin.Effect= null;
            btnFiok.Effect= null;
            btnFogadas.Effect= null;
            Container.Content = new PageOrganizer();
        }

        private void ShutdownButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnThemeToggle_Click(object sender, RoutedEventArgs e)
        {
            if (IsDarkModeOn)
            {
                btnThemeToggle.Content = "🌙";
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("LightTheme.xaml", UriKind.Relative)
                });
                window.Background = Brushes.White;
                navbar.Background = Brushes.White;
                IsDarkModeOn = false;
                


            }
            else
            {
                btnThemeToggle.Content = "☀";
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("DarkTheme.xaml", UriKind.Relative)
                });
                window.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF262627"));
                navbar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF343538"));
                IsDarkModeOn = true;
                
            }
        }

        private void btnBefizetes_Click(object sender, RoutedEventArgs e)
        {
            
            BlurEffect blurEffect = new BlurEffect();
            blurEffect.Radius = 22;
            this.Effect = blurEffect;
            Befizetes windowbefiz = new Befizetes();
            windowbefiz.Owner = this;
            windowbefiz.ShowDialog();
            this.Effect = null;
        }
    }
}