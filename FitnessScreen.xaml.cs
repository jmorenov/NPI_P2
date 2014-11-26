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
    public partial class FitnessScreen : PageFunction<String>
    {
        private KinectController kinect = new KinectController();

        public FitnessScreen()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            if (!kinect.isConnected() || !kinect.start())
                this.NavigationService.Navigate(new Uri("ErrorScreen.xaml", UriKind.Relative));
            else
            {
                ImageSkeleton.Source = kinect.getImageSkeleton();
                ImageVideo.Source = kinect.getImageSource();
                double difficulty = 0.05;
                kinect.movController.startExercise(difficulty, 60);
            }
        }

        private void PageClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (kinect.isConnected() && kinect.isStarted())
                kinect.close();
        }
    }
}