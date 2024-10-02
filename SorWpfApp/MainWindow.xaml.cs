using Microsoft.VisualBasic;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            Container.Content = new PageFogadas();
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
    }
}