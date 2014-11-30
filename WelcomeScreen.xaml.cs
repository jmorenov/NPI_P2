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
using System.Windows.Forms;

namespace NPI_P2
{
    public partial class WelcomeScreen : Page
    {
        /// <summary>
        /// Controlador del Kinect.
        /// </summary>
        private KinectController kinect = new KinectController();

        /// <summary>
        /// Argumento de porcentaje de error con el que se realizará el ejercicio.
        /// </summary>
        private double difficulty = 0;

        public WelcomeScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Comprueba si el kinect está conectado, si no lo está se define un mensaje de error y se termina la aplicación.
        /// </summary>
        private void PageLoaded(object sender, RoutedEventArgs e)
        {
           if (!kinect.isConnected())
            {
                button1.Visibility = System.Windows.Visibility.Hidden;
                NumberTextBox.Visibility = System.Windows.Visibility.Hidden;
                ERROR.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// Función que controla el Click del botón Iniciar. Comprueba que el valor del porcentaje de error sea correcto.
        /// </summary>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (NumberTextBox.Visibility == System.Windows.Visibility.Visible)
            {
                bool correct = false;
                if (NumberTextBox.Text != "")
                {
                    try
                    {
                        int percent = Convert.ToInt32(NumberTextBox.Text);
                        if (percent < 100)
                        {
                            difficulty = percent / 100.0;
                            correct = true;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                if (correct)
                {
                    FitnessScreen FitnessScreen = new FitnessScreen(difficulty);
                    this.NavigationService.Navigate(FitnessScreen);
                }
                else
                {
                    NumberTextBox.Clear();
                }
            }
            else if(difficulty != 0)
            {
                FitnessScreen FitnessScreen = new FitnessScreen(difficulty);
                this.NavigationService.Navigate(FitnessScreen);
            }
            else
                System.Windows.Forms.MessageBox.Show("Debes seleccionar un nivel de dificultad.", "Nivel de dificultad sin seleccionar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// Comprobación sintáctica del valor del porcentaje de error.
        /// </summary>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Controla el Click del botón About y llama al cuadro de diálogo de About_NPI_Fitness.
        /// </summary>
        private void About_Click(object sender, RoutedEventArgs e)
        {
            About_NPI_Fitness about = new About_NPI_Fitness();
            about.ShowDialog();
        }

        private void button_easy_Click(object sender, RoutedEventArgs e)
        {
            difficulty = 20;
        }

        private void button_medium_Click(object sender, RoutedEventArgs e)
        {
            difficulty = 10;
        }

        private void button_hard_Click(object sender, RoutedEventArgs e)
        {
            difficulty = 5;
        }
    }
}
