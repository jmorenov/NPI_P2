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
    public partial class LoadingScreen : Page
    {
        public LoadingScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Variable Timer para controlar la barra de progreso.
        /// </summary>
        private Timer timer1 = new Timer();

        /// <summary>
        /// Función que se llama al cargar la página. Controla la ejecución de la barra de progreso.
        /// </summary>
        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            progressbar.Maximum = 10;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 100;
            timer1.Enabled = true;
            timer1.Start();
        }

        /// <summary>
        /// Función que controla el tiempo de ejecución de la barra de progreso. Cuando termina llama a la página WelcomeScreen.
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressbar.Value < 10)
            {
                progressbar.Value++;
            }
            else
            {
                timer1.Stop();
                this.NavigationService.Navigate(new Uri("WelcomeScreen.xaml", UriKind.Relative));
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
