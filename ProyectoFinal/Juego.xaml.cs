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
        public bool jugando = true;
        Pepper pepper;
        Stopwatch stopwatch = new Stopwatch();
        TimeSpan tiempoAnterior;
        float frecuenciaFundamental = 0;
        DispatcherTimer timer;
        Stopwatch cronometro;
        public Juego()
        {
            InitializeComponent();

            canvasPrincipal.Focus();


            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;


            cronometro = new Stopwatch();

           

            pepper = new Pepper(spritePepper);

            stopwatch.Start();
            tiempoAnterior = stopwatch.Elapsed;


            ThreadStart threadStart = new ThreadStart(cicloPrincipal);
            Thread thread = new Thread(threadStart);
            thre ad.Start();


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

        private void canvasPrincipal_KeyDown(object sender, EventArgs e)
        {


            if (frecuenciaFundamental <=500)
            {
                pepper.CambiarDireccion(Pepper.Direccion.Izquierda);
            }
            if (frecuenciaFundamental >=600)
            {
                pepper.CambiarDireccion(Pepper.Direccion.Derecha);

            }
            if (frecuenciaFundamental>=700)
            {
                pepper.CambiarDireccion(Pepper.Direccion.Arriba);
            }
            if (frecuenciaFundamental>=200)
            {
                pepper.CambiarDireccion(Pepper.Direccion.Abajo);
            }

        }

    

        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            jugando = true;
            //Inicializar la conexion
            waveIn = new WaveIn();

            //Establecer el formato
            waveIn.WaveFormat =
                new WaveFormat(44100, 16, 1);
            formato = waveIn.WaveFormat;

            //Duracion del buffer
            waveIn.BufferMilliseconds = 500;

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

            int numMuestras = bytesGrabados / 2;

            int exponente = 0;
            int numeroBits = 0;

            do
            {
                exponente++;
                numeroBits = (int)
                    Math.Pow(2, exponente);
            } while (numeroBits < numMuestras);
            exponente -= 1;
            numeroBits = (int)
                Math.Pow(2, exponente);
            Complex[] muestrasComplejas =
                new Complex[numeroBits];

            for (int i = 0; i < bytesGrabados; i += 2)
            {
                short muestra =
                    (short)(buffer[i + 1] << 8 | buffer[i]);
                float muestra32bits =
                    (float)muestra / 32768.0f;
                if (i / 2 < numeroBits)
                {
                    muestrasComplejas[i / 2].X = muestra32bits;
                }

            }

            FastFourierTransform.FFT(true,
                exponente, muestrasComplejas);

            float[] valoresAbsolutos =
                new float[muestrasComplejas.Length];

            for (int i = 0; i < muestrasComplejas.Length;
                i++)
            {
                valoresAbsolutos[i] = (float)
                    Math.Sqrt(
                    (muestrasComplejas[i].X * muestrasComplejas[i].X) +
                    (muestrasComplejas[i].Y * muestrasComplejas[i].Y));

            }

            var mitadValoresAbsolutos =
                valoresAbsolutos.Take(valoresAbsolutos.Length / 2).ToList();

            int indiceValorMaximo =
                mitadValoresAbsolutos.IndexOf(
                mitadValoresAbsolutos.Max());

            frecuenciaFundamental =
               (float)(indiceValorMaximo * formato.SampleRate)
               / (float)valoresAbsolutos.Length;

            lblHertz.Text =
                frecuenciaFundamental.ToString("n") +
                " Hz";




        }
        private void btnDetener_Click(object sender, RoutedEventArgs e)
        {
            jugando = false;
            waveIn.StopRecording();
        }
    }
}
