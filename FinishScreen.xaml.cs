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
        /// <summary>
        /// Constructor de la página que recibe como argumento el tiempo de ejecución del ejercicio.
        /// </summary>
        public FinishScreen(double time, double difficulty, double record)
        {
            InitializeComponent();
            TextFinish.Text = "Ejercicio acabado con éxito.\nDificultad: "+ difficulty + ".\nTiempo empleado en la realización: " + time/1000 + " segundos.\nPuntuación: "+ record +".";
        }

        /// <summary>
        /// Controla el Click del botón Reiniciar y llama a la página WelcomeScreen.
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("WelcomeScreen.xaml", UriKind.Relative));
        }
    }
}
