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
using System.Windows.Forms;

namespace NPI_P2
{
    /// <summary>
    /// Lógica de interacción para Page1.xaml
    /// </summary>
    public partial class LoadingScreen : Page
    {
        public LoadingScreen()
        {
            InitializeComponent();
        }

        private Timer timer1 = new Timer();
        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            progressbar.Maximum = 10;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 100;
            timer1.Enabled = true;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressbar.Value < 10)
            {
                progressbar.Value++;
            }
            else
            {
                timer1.Stop();
                this.NavigationService.Navigate(new Uri("FitnessScreen.xaml", UriKind.Relative));
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        ~LoadingScreen()
        {

        }
    }
}
