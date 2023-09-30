using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Entities;
using WebScraping.Interface;

namespace WebScraping.Services
{
    public class GenericScraper : IScraper, ISearch, IApplyFilter
    {

        private readonly string _url;
        private readonly string _chromeExecutablePath;
        private readonly string _itemClassName;
        private readonly string _itemPriceClassName;

        public GenericScraper(WebSite webSite)
        {
            _url = webSite.Url;
            _chromeExecutablePath = webSite.ChromeExecutablePath;
            _itemClassName = webSite.ItemClassName;
            _itemPriceClassName = webSite.ItemPriceClassName;
        }

        public async Task ScrapeAsync()
        {
            try
            {
                using var browserFetcher = new BrowserFetcher();
                var result = await browserFetcher.DownloadAsync();
            }
            catch (PuppeteerException pe)
            {
                Console.WriteLine("Error en la descarga del navegador:", pe.Message);
            }

            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = _chromeExecutablePath
            });

            await using var page = await browser.NewPageAsync();
            await page.GoToAsync(_url);

            var articles = await SearchItemNames(page);

            var prices = await SearchItemPrices(page);

            ApplyPriceFilter(prices);
        }

        public async Task<List<string>> SearchItemNames(Page page)
        {
            string javaScript= "() => {" +
            $"const a = document.querySelectorAll('" + _itemClassName + "'); " +
            "const res = []; " +
            "for(let i = 0; i < 6; i++){" +
            "    res.push(a[i].innerHTML); " +
            "}" +
            "return res;" +
            "}";

            var itemNames = await page.EvaluateFunctionAsync<string[]>(javaScript);

            int id = 1;

            List<string> articles = new List<string>();

            foreach (var item in itemNames)
            {

                articles.Add("[" + ((id++).ToString()) + "] " + item.ToString());
            }
            
            foreach (var article in articles)
            {
                Console.WriteLine(article);
            }

            return articles;
        }

        public async Task<List<double>> SearchItemPrices(Page page)
        {
            string javaScript = "() => {" +
            $"const a = document.querySelectorAll('" + _itemPriceClassName + "'); " +
            "const res = []; " +
            "for(let i = 0; i < 6; i++){" +
            "    res.push(a[i].innerHTML); " +
            "}" +
            "return res;" +
            "}";

            var prices = await page.EvaluateFunctionAsync<string[]>(javaScript);

            var priceList = prices.Select(price => double.Parse(price, CultureInfo.InvariantCulture));

            List<double> priceListResult = priceList.ToList();

            return priceListResult;
        }

        public Tuple<double, double> ApplyPriceFilter(List<double> priceList)
        {
            var maxPrice = priceList.Max();
            var minPrice = priceList.Min();

            Console.WriteLine("\n\n================== Estadistica de precios ============================");
            Console.WriteLine("Precio más alto: " + maxPrice);
            Console.WriteLine("Precio más bajo: " + minPrice + "\n\n");

            return Tuple.Create((Double)maxPrice, (Double)minPrice);
        }
    }
}
