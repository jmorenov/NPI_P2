using System;
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

namespace NPI_P2
{
    public partial class FinishScreen : PageFunction<Double>
    {
        public FinishScreen(double time)
        {
            InitializeComponent();
            TextFinish.Text = "Ejercicio acabado con éxito.\nTiempo empleado en la realización: " + time + " segundos.";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("WelcomeScreen.xaml", UriKind.Relative));
        }
    }
}
