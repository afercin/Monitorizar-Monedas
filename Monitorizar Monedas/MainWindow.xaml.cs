using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Monitorizar_Monedas
{
    /// <summary>
    /// Clase que hace referencia a la ventana pricipal.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Instancia del navegador PhantomJS.
        /// </summary>
        private CoinRequestBrowser browser;
        /// <summary>
        /// Constructor de la ventana principal.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            browser = new CoinRequestBrowser();

            Thread t = new Thread(() =>
            {
                while (true)
                {
                    Task.Run(() =>
                    {
                        string coinValue = browser.GetCoinValue("ethereum-classic?cid=1061986");
                        Dispatcher.Invoke(() => AppendLog(coinValue));
                    });

                    Thread.Sleep(25000); // La página se actualiza cada 25 segundos (tiempo en milisegundos).
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        /// <summary>
        /// Escribe por pantalla la cadena introducida.
        /// </summary>
        /// <param name="text">La caden a escribir por pantalla.</param>
        private void AppendLog(string text)
        {
            Log.AppendText(DateTime.Now.ToLongTimeString() + " -> " + text + "\r\n");
            Log.ScrollToEnd();
        }
        /// <summary>
        /// Evento que ocurre al cerrar la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            browser.Dispose();
        }
    }
}
