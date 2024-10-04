﻿using System;
using System.Collections.Generic;
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
    /// Interaction logic for PageAccount.xaml
    /// </summary>
    public partial class PageAccount : Page
    {
        string backgroundcolor = "#FF343538";
        
        public PageAccount()
        {
            InitializeComponent();
            if (IsUsingLightTheme())
            {
                title1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backgroundcolor));
                Title1.Foreground = Brushes.White;
                sectionTransactionHistory.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backgroundcolor));
                lb1.Foreground = Brushes.White;
                lb2.Foreground = Brushes.White;
                lb3.Foreground = Brushes.White;
                lb4.Foreground = Brushes.White;

                InitializeComponent();
            }
            else
            {

            }
            lblOsszegBalance.Content = UserAtkuldese.bejelentkezettFogado.balance+" Ft";
        }

        public bool IsUsingLightTheme()
        {
            // Check if any of the merged dictionaries contains lighttheme.xaml
            return Application.Current.Resources.MergedDictionaries
                .Any(dict => dict.Source != null && dict.Source.OriginalString.EndsWith("LightTheme.xaml", StringComparison.OrdinalIgnoreCase));
        }
    }
}
