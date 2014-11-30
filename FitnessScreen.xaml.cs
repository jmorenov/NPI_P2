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
using System.ComponentModel;
using Microsoft.Kinect;
using System.Threading;
using System.Windows.Forms;

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
        private int n_replays = 5;

        private Thread thread;

        /// <summary>
        /// Constructor de la página en el que se recibe el porcentaje de error.
        /// </summary>
        public FitnessScreen(double difficulty)
        {
            InitializeComponent();
            this.difficulty = difficulty;
            thread = new Thread(new ThreadStart(controlKinect));
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
                kinect.setImageSkeleton(ref ImageSkeleton);
                kinect.setTextTime(ref time);
                kinect.setCorrectImage(ref correct);
                //kinect.setImageSource(ref ImageVideo);
                thread.Start();
            }
        }

        private void controlKinect()
        {
                kinect.movController.startExercise(difficulty, angle, n_replays);
                while (!kinect.movController.isFinished())
                {}
                if (kinect.movController.isFinished())
                {
                    double record = kinect.movController.getRecord();
                    double time = kinect.movController.getTime();
                    this.Dispatcher.Invoke(new FinMovement(OnMovementComplete), time, difficulty, record);
                }
        }

        public delegate void FinMovement(double time, double difficulty, double record);

        private void OnMovementComplete(double time, double difficulty, double record)
        {
            thread.Abort();
            FinishScreen FinishScreen = new FinishScreen(time, difficulty, record);
            this.NavigationService.Navigate(FinishScreen);
        }

        /// <summary>
        /// Detiene la ejecución del Kinect.
        /// </summary>
        ~FitnessScreen()
        {
            if (kinect.isConnected() && kinect.isStarted())
                kinect.close();
        }
    }
}