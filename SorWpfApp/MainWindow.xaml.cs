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
        private bool IsLightModeActive = true;
        private string backgroundcolor = "#FF2D2E35";
        private string navbarBackgroundColor = "#FF2E2E31";
        static Bettor bejelentkezettBettor;
        public MainWindow()
        {
            InitializeComponent();
            btnFiok_Click(btnFiok, new RoutedEventArgs());
            bejelentkezettBettor = UserAtkuldese.bejelentkezettFogado;
            lblUsername.Content = bejelentkezettBettor.username;
            lblBalance.Content = bejelentkezettBettor.balance+" Ft";
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
            dropShadowEffect.Color = Colors.Yellow;
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
            dropShadowEffect.Color = Colors.Yellow;
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
            dropShadowEffect.Color = Colors.Yellow;
            btnAdmin.Effect = dropShadowEffect;
            btnFiok.Effect= null;
            btnFogadas.Effect= null;
            btnOrganizer.Effect = null;
            Container.Content = new PageFogadas();
        }

        private void btnOrganizer_Click(object sender, RoutedEventArgs e)
        {
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Opacity = 1;
            dropShadowEffect.BlurRadius = 10;
            dropShadowEffect.ShadowDepth = 1;
            dropShadowEffect.Color = Colors.Yellow;
            btnOrganizer.Effect = dropShadowEffect;
            btnAdmin.Effect= null;
            btnFiok.Effect= null;
            btnFogadas.Effect= null;
            Container.Content = new PageFogadas();
        }

        private void ShutdownButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnThemeToggle_Click(object sender, RoutedEventArgs e)
        {
            if (IsLightModeActive)
            {
                btnThemeToggle.Content = "🌙";
                IsLightModeActive = false;
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("LightTheme.xaml", UriKind.Relative)
                });
                window.Background = Brushes.White;
                

            }
            else
            {
                btnThemeToggle.Content = "☀";
                IsLightModeActive = true;
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("DarkTheme.xaml", UriKind.Relative)
                });
                window.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF262627"));

            }
        }

        private void ToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}