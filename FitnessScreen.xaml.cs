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
using System.IO;

namespace NPI_P2
{
    public partial class FitnessScreen : PageFunction<Double>
    {
        /// <summary>
        /// Controlador del Kinect.
        /// </summary>
        private KinectController kinect = new KinectController();

        /// <summary>
        /// Valores de porcentaje de error y ángulo.
        /// </summary>
        private double difficulty;
        private double angle = 30;

        /// <summary>
        /// Constructor de la página en el que se recibe el porcentaje de error.
        /// </summary>
        public FitnessScreen(double difficulty)
        {
            InitializeComponent();
            this.difficulty = difficulty;
        }

        /// <summary>
        /// Comprueba si está conectado el Kinect y lo inicia. Inicia el ejercicio y cuando termina llama a la página FinishScreen.
        /// </summary>
        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            if (!kinect.isConnected() || !kinect.start())
                OnReturn(null);
            else
            {
                ImageSkeleton.Source = kinect.getImageSkeleton();
                ImageVideo.Source = kinect.getImageSource();
                kinect.movController.startExercise(difficulty, angle);
                if (kinect.movController.isFinished())
                {
                    double record = kinect.movController.getRecord();
                    double time = kinect.movController.getTime();
                    FinishScreen FinishScreen = new FinishScreen(time, difficulty, record);
                    this.NavigationService.Navigate(FinishScreen);
                }
            }
        }

        /// <summary>
        /// Detiene la ejecución del Kinect.
        /// </summary>
        private void PageClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (kinect.isConnected() && kinect.isStarted())
                kinect.close();
        }
    }
}