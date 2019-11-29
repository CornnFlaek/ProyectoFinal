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
        public enum Direccion { Izquierda, Derecha, Arriba, Abajo, Estatico};
        Direccion DireccionActual { get; set; }

        public double PosicionX { get; set; }
        public double PosicionY { get; set; }
        public double Velocidad { get; set; }

        int spriteActual = 0;
        double tiempoTranscurridoEnSprite = 0;
        double tiempoPorSprite = 0.25;

        public Pepper(Image imagen)
        {
            Imagen = imagen;
            arriba.Add(new BitmapImage(new Uri("Up1.png", UriKind.Relative)));
            arriba.Add(new BitmapImage(new Uri("Up 2.png", UriKind.Relative)));
            arriba.Add(new BitmapImage(new Uri("Up3.png", UriKind.Relative)));

            abajo.Add(new BitmapImage(new Uri("Down1.png", UriKind.Relative)));
            abajo.Add(new BitmapImage(new Uri("Down2.png", UriKind.Relative)));
            abajo.Add(new BitmapImage(new Uri("Down3.png", UriKind.Relative)));

            izquierda.Add(new BitmapImage(new Uri("Left1.png", UriKind.Relative)));
            izquierda.Add(new BitmapImage(new Uri("Left2.png", UriKind.Relative)));
            izquierda.Add(new BitmapImage(new Uri("Left3.png", UriKind.Relative)));

            derecha.Add(new BitmapImage(new Uri("Right1.png", UriKind.Relative)));
            derecha.Add(new BitmapImage(new Uri("Right2.png", UriKind.Relative)));
            derecha.Add(new BitmapImage(new Uri("Right3.png", UriKind.Relative)));
            Imagen.Source = derecha[0];

            PosicionX = Canvas.GetLeft(imagen);
            PosicionY = Canvas.GetTop(imagen);

            DireccionActual = Direccion.Derecha;

            Velocidad = 20;

        }
        public void CambiarDireccion(Direccion nuevaDireccion)
        {
            DireccionActual = nuevaDireccion;
            switch (DireccionActual)
            {
                case Direccion.Abajo:
                    Imagen.Source = abajo[0];
                    break;
                case Direccion.Arriba:
                    Imagen.Source = arriba[0];
                    break;
                case Direccion.Izquierda:
                    Imagen.Source = izquierda[0];
                    break;
                case Direccion.Derecha:
                    Imagen.Source = derecha[0];
                    break;
                default:
                    break;
            }
        }

        public void Mover(double deltaTime)
        {
            tiempoTranscurridoEnSprite += deltaTime;
            int spriteAnterior = spriteActual;
            if (tiempoTranscurridoEnSprite >= tiempoPorSprite)
            {
                spriteActual++;
                tiempoTranscurridoEnSprite -= tiempoPorSprite;
                if (spriteActual > 2)
                {
                    spriteActual = 0;
                }
            }
            BitmapImage sprite = null;
            switch (DireccionActual)
            {
                case Direccion.Abajo:
                    PosicionY += Velocidad * deltaTime;
                    sprite = abajo[spriteActual];

                    break;
                case Direccion.Arriba:
                    PosicionY -= Velocidad * deltaTime;
                    sprite = arriba[spriteActual];

                    break;
                case Direccion.Izquierda:
                    PosicionX -= Velocidad * deltaTime;
                    sprite = izquierda[spriteActual];
                    break;
                case Direccion.Derecha:
                    PosicionX += Velocidad * deltaTime;
                    sprite = derecha[spriteActual];
                    break;
                default:
                    break;
            }
            if (spriteAnterior != spriteActual && sprite != null)
            {
                Imagen.Source = sprite;
            }
            Canvas.SetLeft(Imagen, PosicionX);
            Canvas.SetTop(Imagen, PosicionY);


        }
    }

}
