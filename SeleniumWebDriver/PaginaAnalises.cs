using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumWebDriver
{
    public class PaginaAnalises
    {
        private readonly SeleniumConfigurations _seleniumConfigurations;
        private IWebDriver _webDriver;

        public PaginaAnalises(SeleniumConfigurations seleniumConfigurations)
        {
            _seleniumConfigurations = seleniumConfigurations;

            var optionsFF = new ChromeOptions();
            optionsFF.AddArgument("--headless");

            _webDriver = new ChromeDriver(_seleniumConfigurations.CaminhoDriverChrome, optionsFF);
        }


        public void CarregarPagina()
        {
            _webDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            _webDriver.Navigate().GoToUrl(_seleniumConfigurations.UrlAnalises);
        }

        public IList<Analises> GetAnalises()
        {
            var analises = new List<Analises>();
          
            var rowsAnalises = _webDriver.FindElement(By.ClassName("td-ss-main-content"))
                .FindElements(By.ClassName("td-block-row"));

            foreach (var rowsAnalise in rowsAnalises)
            {
                var tds = rowsAnalise.FindElements(By.ClassName("td-block-span6"));

                foreach (var webElement in tds)
                {
                    var descricaoElem = webElement
                        .FindElement(By.ClassName("item-details"))
                        .FindElement(By.TagName("h3"))
                        .FindElement(By.TagName("a"));

                    var urlImagemElem = webElement.FindElement(By.ClassName("td-module-thumb"))
                        .FindElement(By.TagName("a"))
                        .FindElement(By.TagName("img"));

                    var descricao = descricaoElem.Text;
                    var urlIntegra = descricaoElem.GetAttribute("href");
                    var urlImage = urlImagemElem.GetAttribute("src");

                    analises.Add(new Analises
                    {
                        Titulo = descricao,
                        UrlImagem = urlImage,
                        UrlNoticiaIntegra = urlIntegra
                    });
                }
            }

            return analises;
        }

        public void Fechar()
        {
            _webDriver.Quit();
            _webDriver = null;
        }
    }
}