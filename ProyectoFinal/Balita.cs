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


namespace ProyectoFinal
{
    public class Balita
    {
        public Image Imagen { get; set; }

        double PosicionX { get; set; }
        double PosicionY { get; set; }
        public double Velocidad { get; set; }

        public Balita(Image imagen)
        {
            Imagen = imagen;
            PosicionX = Canvas.GetLeft(imagen);
            PosicionY = Canvas.GetTop(imagen);

            Imagen.Source = new BitmapImage( new Uri("balita.png", UriKind.Relative));

            Velocidad = 20;
        }

        public void Mover(double deltaTime)
        {
            PosicionX += Velocidad * deltaTime;
            Canvas.SetLeft(Imagen, PosicionX);
        }
    }
  
    
    }
