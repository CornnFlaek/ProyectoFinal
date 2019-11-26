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
using System.Threading;
using System.Diagnostics;


namespace ProyectoFinal
{
    /// <summary>
    /// Interaction logic for Juego.xaml
    /// </summary>
    public partial class Juego : UserControl
    {
        bool jugando = true;
        Pepper pepper;
        Stopwatch stopwatch = new Stopwatch();
        TimeSpan tiempoAnterior;
        public Juego()
        {
            InitializeComponent();

            canvasPrincipal.Focus();

            pepper = new Pepper(spritePepper);

            stopwatch.Start();
            tiempoAnterior = stopwatch.Elapsed;


            ThreadStart threadStart = new ThreadStart(cicloPrincipal);
            Thread thread = new Thread(threadStart);
            thread.Start();


        }
        public void cicloPrincipal()
        {
            while (jugando)
            {
                Dispatcher.Invoke(actualizar);
            }
        }
        public void actualizar()
        {
            TimeSpan tiempoActual = stopwatch.Elapsed;
            double deltaTime = tiempoActual.TotalSeconds - tiempoAnterior.TotalSeconds;

            pepper.Mover(deltaTime);
          

            tiempoAnterior = tiempoActual;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            jugando = false;
        }

        private void canvasPrincipal_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.IsRepeat)
            {
                if (e.Key == Key.Left)
                {
                    pepper.CambiarDireccion(Pepper.Direccion.Izquierda);
                }
                if (e.Key == Key.Right)
                {
                    pepper.CambiarDireccion(Pepper.Direccion.Derecha);
                }
                if (e.Key == Key.Up)
                {
                    pepper.CambiarDireccion(Pepper.Direccion.Arriba);
                }
                if (e.Key == Key.Down)
                {
                    pepper.CambiarDireccion(Pepper.Direccion.Abajo);
                }
            }

        }
    }
}
