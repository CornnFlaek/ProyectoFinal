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
    public class Pepper
    {
        List<BitmapImage> arriba = new List<BitmapImage>();
        List<BitmapImage> abajo = new List<BitmapImage>();
        List<BitmapImage> izquierda = new List<BitmapImage>();
        List<BitmapImage> derecha = new List<BitmapImage>();

        Image Imagen { get; set; }
        public enum Direccion { Izquierda, Derecha, Arriba, Abajo };
        Direccion DireccionActual { get; set; }

        double PosicionX { get; set; }
        double PosicionY { get; set; }
        public double Velocidad { get; set; }

        int spriteActual = 0;
        double tiempoTranscurridoEnSprite = 0;
        double tiempoPorSprite = 0.25;

        public Pepper(Image imagen)
        {
            Imagen = imagen;
            arriba.Add(new BitmapImage(new Uri("arriba1.png", UriKind.Relative)));
            arriba.Add(new BitmapImage(new Uri("arriba2.png", UriKind.Relative)));

            abajo.Add(new BitmapImage(new Uri("abajo1.png", UriKind.Relative)));
            abajo.Add(new BitmapImage(new Uri("abajo2.png", UriKind.Relative)));

            izquierda.Add(new BitmapImage(new Uri("izquierda1.png", UriKind.Relative)));
            izquierda.Add(new BitmapImage(new Uri("izquierda2.png", UriKind.Relative)));

            derecha.Add(new BitmapImage(new Uri("derecha1.png", UriKind.Relative)));
            derecha.Add(new BitmapImage(new Uri("derecha2.png", UriKind.Relative)));
            Imagen.Source = derecha[0];

            PosicionX = Canvas.GetLeft(imagen);
            PosicionY = Canvas.GetTop(imagen);

            DireccionActual = Direccion.Derecha;

            Velocidad = 20;

        }


    }
}
