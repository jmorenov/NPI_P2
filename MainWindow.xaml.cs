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

namespace NPI_P2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Inicio de la aplicación. mainFrame se inicializa con la página LoadingScreen.
        /// </summary>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new LoadingScreen());
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}