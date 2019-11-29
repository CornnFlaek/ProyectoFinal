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
using System.Windows.Threading;
using System.Threading;
using System.Diagnostics;

using NAudio;
using NAudio.Wave;
using NAudio.Dsp;
using NAudio.Wave.SampleProviders;


namespace ProyectoFinal
{
    /// <summary>
    /// Interaction logic for Juego.xaml
    /// </summary>
    public partial class Juego : UserControl
    {
        WaveIn waveIn; //Conexion con microfono
        WaveFormat formato; //Formato de audio
        DispatcherTimer timer;
        Stopwatch cronometro;

        public bool jugando = true;
        Pepper pepper;
        List<Balita> balitas = new List<Balita>();
        Stopwatch stopwatch = new Stopwatch();
        TimeSpan tiempoAnterior;
        float frecuenciaFundamental = 0;
    

        public Juego()
        {
            InitializeComponent();

            canvasPrincipal.Focus();


            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;


            cronometro = new Stopwatch();

           

            pepper = new Pepper(spritePepper);
            balitas.Add(new Balita(balito));
            balitas.Add(new Balita(balite));

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
            foreach (Balita balita in balitas)
            {
                balita.Mover(deltaTime);
                double xPepper = Canvas.GetLeft(spritePepper);
                double xBalitas = Canvas.GetLeft(balita.Imagen);
                double yPepper = Canvas.GetTop(spritePepper);
                double yBalitas = Canvas.GetTop(balita.Imagen);

                if (xBalitas + balita.Imagen.Width >= xPepper && xBalitas <= xPepper + spritePepper.Width && yBalitas + balita.Imagen.Height >= yPepper && yBalitas <= yPepper + spritePepper.Height)
                {
                    jugando = false;
                    gameover.Visibility = Visibility.Visible;
                }
            }

            tiempoAnterior = tiempoActual;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            jugando = false;
        }

        private void canvasPrincipal_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.Key == Key.Left )
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

        private void Timer_Tick(object sender, EventArgs e)
        {


          

            if (pepper.PosicionY <= 80)
            {
                pepper.CambiarDireccion(Pepper.Direccion.Estatico);
            }

            if (frecuenciaFundamental >= 200 && frecuenciaFundamental <=500)
            {
                pepper.CambiarDireccion(Pepper.Direccion.Izquierda);
            }
            if (frecuenciaFundamental>=501 && frecuenciaFundamental<=700)
            {
                pepper.CambiarDireccion(Pepper.Direccion.Derecha);

            }
            if (frecuenciaFundamental>=701 && frecuenciaFundamental <=900)
            {
                pepper.CambiarDireccion(Pepper.Direccion.Arriba);   
            }
            if (frecuenciaFundamental>=901)
            {
                pepper.CambiarDireccion(Pepper.Direccion.Abajo);
            }
        }

        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            jugando = true;
            timer.Start();
            //Inicializar la conexion
            waveIn = new WaveIn();

            //Establecer el formato
            waveIn.WaveFormat =
                new WaveFormat(44100, 16, 1);
            formato = waveIn.WaveFormat;

            //Duracion del buffer
            waveIn.BufferMilliseconds = 250;

            //Con que funcion respondemos
            //cuando se llena el buffer
            waveIn.DataAvailable += WaveIn_DataAvailable;

            waveIn.StartRecording();

        }
        private void WaveIn_DataAvailable(object sender,
            WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesGrabados = e.BytesRecorded;
            float acumulador = 0.0f;

            double numeroMuestras = bytesGrabados / 2;

            int exponente = 1;
            int numeroMuestrasComplejas = 0;
            int bitsMaximos = 0;

            do
            {
                bitsMaximos = (int)Math.Pow(2, exponente);
                exponente++;

            } while (bitsMaximos < numeroMuestras);

            numeroMuestrasComplejas = bitsMaximos / 2;
            exponente -= 2;

            Complex[] señalCompleja = new Complex[numeroMuestrasComplejas];

            for (int i = 0; i < bytesGrabados; i += 2)
            {


                short muestra = (short)(buffer[i + 1] << 8 | buffer[i]);
                float muestra32bits = (float)muestra / 32768.0f;
                acumulador += Math.Abs(muestra32bits);

                if (i / 2 < numeroMuestrasComplejas)
                {
                    señalCompleja[i / 2].X = muestra32bits;
                }

            }
            float promedio = acumulador / (bytesGrabados / 2.0f);

            if (promedio > 0)
            {

                FastFourierTransform.FFT(true, exponente, señalCompleja);
                float[] valoresAbsolutos = new float[señalCompleja.Length];
                for (int i = 0; i < señalCompleja.Length; i++)
                {

                    valoresAbsolutos[i] = (float)Math.Sqrt(
                        (señalCompleja[i].X * señalCompleja[i].X) +
                        (señalCompleja[i].Y * señalCompleja[i].Y)
                        );

                }
                int indiceSeñalConMasPresencia = valoresAbsolutos.ToList().IndexOf(valoresAbsolutos.Max());

                frecuenciaFundamental = (float)(indiceSeñalConMasPresencia * waveIn.WaveFormat.SampleRate) / (float)valoresAbsolutos.Length;

                lblHertz.Text =
                frecuenciaFundamental.ToString("n") +
                " Hz";

        }
        }

        private void btnDetener_Click(object sender, RoutedEventArgs e)
        {
            jugando = false;
            waveIn.StopRecording();
        }
    }
}
