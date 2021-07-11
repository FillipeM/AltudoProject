using AltudoProjectModels;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AltudoProjectServices
{
    public class AltudoProjectService : IAltudoProjectService
    {
        private readonly WebClient client;
        private AltudoModel model;

        public AltudoProjectService()
        {
            client = new WebClient();
            model = new AltudoModel();
        }

        public async Task<AltudoModel> processaURL(string url)
        {
            model.ImageList = new List<string>();
            model.WordsRanking = new List<WordsRanking>();
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();

            IWebDriver webDriver = new ChromeDriver();
            try
            {
                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(120);
                webDriver.Navigate().GoToUrl(url);
                IJavaScriptExecutor scriptExecutor = (IJavaScriptExecutor)webDriver;
                var pageSize = (Int64)scriptExecutor.ExecuteScript("return document.body.scrollHeight");
                int scroll = 10;
                while (scroll < pageSize)
                {
                    scriptExecutor.ExecuteScript($"window.scrollTo(0, {scroll})");
                    scroll += 10;
                    pageSize = (Int64)scriptExecutor.ExecuteScript("return document.body.scrollHeight");
                }

                document.LoadHtml(webDriver.PageSource);

                foreach (var item in document.DocumentNode.Descendants("img").Select(f => f.Attributes["src"]))
                {
                    if (item != null)
                        model.ImageList.Add(item.Value);
                }

                var texto = document.DocumentNode.InnerText;
                texto = texto.Replace(",", "").Replace(".", "").Replace("\\n", " ").Replace("\\t", " ").Replace("\\r", " ");
                var arrayTexto = texto.Split(" ");
                foreach (var palavra in arrayTexto)
                {
                    if (!string.IsNullOrEmpty(palavra.Trim()))
                    {
                        if (model.WordsRanking.Any(f => f.Word == palavra.Trim()))
                        {
                            model.WordsRanking.Where(f => f.Word == palavra.Trim()).FirstOrDefault().TimesAppears += 1;
                        }
                        else
                        {
                            model.WordsRanking.Add(new WordsRanking() { Word = palavra.Trim(), TimesAppears = 1 });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                webDriver.Quit();
            }
            model.WordsRanking = model.WordsRanking.OrderByDescending(f=> f.TimesAppears).Take(10).ToList();
            return model;
        }
    }
}
