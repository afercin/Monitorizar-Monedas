using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;
using System.IO;
using System.Reflection;

namespace Monitorizar_Monedas
{
    /// <summary>
    /// Clase que encapsula funciones de Selenium para obtener el valor de una moneda.
    /// </summary>
    class CoinRequestBrowser : IDisposable
    {
        /// <summary>
        /// URL base sobre la que se traja en esta clase.
        /// </summary>
        private readonly string URLBase = "https://es.investing.com/crypto/";
        /// <summary>
        /// Instancia del navegador
        /// </summary>
        private readonly IWebDriver driver;
        /// <summary>
        /// Constructor no más. Inicializa la isntancia del navegador.
        /// </summary>
        public CoinRequestBrowser()
        {
            PhantomJSOptions options = new PhantomJSOptions();
            options.AddAdditionalCapability("IsJavaScriptEnabled", true);
            driver = new PhantomJSDriver(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), options);
        }
        /// <summary>
        /// Obtiene el precio actual de la moneda en la página "https://es.investing.com/crypto/".
        /// </summary>
        /// <param name="coinURL">La última parte de la URL.</param>
        /// <returns>Devuelve el valor de la moneda como un string.</returns>
        public string GetCoinValue(string coinURL)
        {
            try
            {
                string webResult, html;

                driver.Navigate().GoToUrl(URLBase + coinURL);
                html = driver.PageSource;

                webResult = ExtractCoinValue(html, 1295, "\"last_last\">", "</span>");

                return webResult;
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// A partir del código html, la línea y los delimitadores, obtiene el precio de la moneda.
        /// </summary>
        /// <param name="html">Código HTML de la página.</param>
        /// <param name="lineNumber">Línea en la que aparece el precio actual de la moneda</param>
        /// <param name="delimiterL">Parte de la cadena que tiene hacia la izquierda.</param>
        /// <param name="delimiterR">Parte de la cadena que tiene hacia la derecha</param>
        /// <returns>Devuelve el precio de la moneda</returns>
        private string ExtractCoinValue(string html, int lineNumber, string delimiterL, string delimiterR)
        {
            string line, value;

            line = html.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)[lineNumber];
            value = line.Split(new string[] { delimiterL }, StringSplitOptions.None)[1].Split(new string[] { delimiterR }, StringSplitOptions.None)[0];

            return value;
        }
        /// <summary>
        /// Metodo dispose para cerrar la ventana y liberar el espacio en memoria.
        /// </summary>
        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}
