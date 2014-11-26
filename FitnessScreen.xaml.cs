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
        private KinectController kinect = new KinectController();
        private double difficulty;
        private double angle = 30;

        public FitnessScreen(double difficulty)
        {
            InitializeComponent();
            this.difficulty = difficulty;
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            if (!kinect.isConnected() || !kinect.start())
                OnReturn(null);
            else
            {
                ImageSkeleton.Source = kinect.getImageSkeleton();
                ImageVideo.Source = kinect.getImageSource();
                kinect.movController.startExercise(difficulty, angle);
            }
        }

        private void PageClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (kinect.isConnected() && kinect.isStarted())
                kinect.close();
        }
    }
}