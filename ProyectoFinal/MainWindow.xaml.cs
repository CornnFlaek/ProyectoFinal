﻿using System;
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
using System.Threading;
using System.Diagnostics;

namespace ProyectoFinal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Juego.Visibility = Visibility.Visible;
            Juego.jugando = true;
        }

        private void btnTutorial_Click(object sender, RoutedEventArgs e)
        {
            panelTutorial.Visibility = Visibility.Visible;            
            panelTutorial.Children.Add(new Tutorial());
            
            
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            panelTutorial.Visibility = Visibility.Hidden;
        }
    }
}
