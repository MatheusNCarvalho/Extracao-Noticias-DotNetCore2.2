using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumWebDriver
{
    public class PaginaNoticias : IDisposable
    {
        private readonly SeleniumConfigurations _seleniumConfigurations;
        private IWebDriver _webDriver;

        public PaginaNoticias(SeleniumConfigurations seleniumConfigurations)
        {
            _seleniumConfigurations = seleniumConfigurations;

            var optionsFF = new ChromeOptions();
            optionsFF.AddArgument("--headless");

            _webDriver = new ChromeDriver(_seleniumConfigurations.CaminhoDriverChrome, optionsFF);

        }

        public void CarregarPagina()
        {
            _webDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            _webDriver.Navigate().GoToUrl(_seleniumConfigurations.UrlPaginaNoticias);
        }


        public List<Noticia> ObterNoticias()
        {
            var noticias = new List<Noticia>();

            var rowsNoticias = _webDriver.FindElement(By.ClassName("td_block_inner"))
                .FindElements(By.ClassName("td-block-row"));

            var count = 0;
            foreach (var rowsNoticia in rowsNoticias)
            {
                var tds = rowsNoticia.FindElements(By.ClassName("td-block-span4"));


                foreach (var webElement in tds)
                {
                    var urlNoticiaElem = webElement.FindElement(By.TagName("h3")).FindElement(By.TagName("a"));
                    var urlImagemElem = webElement.FindElement(By.ClassName("td-module-thumb"))
                        .FindElement(By.TagName("a"))
                        .FindElement(By.TagName("img"));

                    var noticia = new Noticia();

                    if (count < 3)
                    {
                        var descricaoElem = webElement.FindElement(By.ClassName("td-excerpt"));
                        noticia.Descricao = descricaoElem.Text;
                        noticia.Titulo = urlNoticiaElem.Text;
                        noticia.UrlImagem = urlImagemElem.GetAttribute("src");
                        noticia.UrlNoticiaIntegra = urlNoticiaElem.GetAttribute("href");

                        noticias.Add(noticia);
                        count++;
                        continue;
                    }

                    noticia.Titulo = urlNoticiaElem.Text;
                    noticia.UrlImagem = urlImagemElem.GetAttribute("src");
                    noticia.UrlNoticiaIntegra = urlNoticiaElem.GetAttribute("href");

                    noticias.Add(noticia);
                    count++;

                }
            }

            return noticias;
        }

        public void Fechar()
        {
            _webDriver.Quit();
            _webDriver = null;
        }

        public void Dispose()
        {
            _webDriver?.Dispose();
        }
    }
}