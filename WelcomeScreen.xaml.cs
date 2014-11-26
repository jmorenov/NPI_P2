using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace NPI_P2
{
    public partial class WelcomeScreen : Page
    {
        private KinectController kinect = new KinectController();
        private double difficulty;
        public WelcomeScreen()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            if (!kinect.isConnected())
            {
                button1.Visibility = System.Windows.Visibility.Hidden;
                NumberTextBox.Visibility = System.Windows.Visibility.Hidden;
                ERROR.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            bool correct = false;
            if (NumberTextBox.Text != "")
            {
                try
                {
                    int percent = Convert.ToInt32(NumberTextBox.Text);
                    if (percent < 100)
                    {
                        difficulty = percent / 100;
                        correct = true;
                    }
                }
                catch (Exception)
                {
                }
            }
            if (correct)
            {
                if (kinect.isConnected())
                    this.NavigationService.Navigate(new Uri("FitnessScreen.xaml", UriKind.Relative));
                else
                    this.NavigationService.Navigate(new Uri("ErrorScreen.xaml", UriKind.Relative));
            }
            else
            {
                NumberTextBox.Clear();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
