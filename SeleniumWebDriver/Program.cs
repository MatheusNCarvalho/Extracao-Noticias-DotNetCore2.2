using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace SeleniumWebDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");

            Console.WriteLine("Iniciando a extração das noticias...");

            var seleniumConfigurations = new SeleniumConfigurations();
            new ConfigureFromConfigurationOptions<SeleniumConfigurations>(
                    configuration.GetSection("SeleniumConfigurations"))
                .Configure(seleniumConfigurations);

            var pagina = new PaginaNoticias(seleniumConfigurations);
            var paginaAnalise = new PaginaAnalises(seleniumConfigurations);
            pagina.CarregarPagina();
            paginaAnalise.CarregarPagina();
            try
            {
                var noticias = pagina.ObterNoticias();
                var analises = paginaAnalise.GetAnalises();

                var json = JsonConvert.SerializeObject(analises);
            }
            catch (Exception e)
            {
                pagina.Fechar();
                paginaAnalise.Fechar();
                Console.WriteLine(e);
                throw;
            }
            pagina.Fechar();
            paginaAnalise.Fechar();
        }
    }
}
