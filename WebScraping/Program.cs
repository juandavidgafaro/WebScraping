using WebScraping.Entities;
using WebScraping.Services;

Console.Write("Ingresa la url del sitio que deseas screperar: \n");
string url = Console.ReadLine();
Console.Write("\nIngresa la ruta en donde se encuentra el navegador: \n");
string browserPath = Console.ReadLine();
Console.Write("\nIngrese el nombre de la clase CSS del elemento que contiene los titulos: \n");
string cssClassTitleName = Console.ReadLine();
Console.Write("\nIngrese el nombre de la clase CSS del elemento que contiene los precios: \n");
string cssClassPriceName = Console.ReadLine();
Console.Write("\nEspera un momento mientras obtenemos la información....... \n\n");


var webSite = new WebSite
{
    Url = url,
    ChromeExecutablePath = browserPath,
    ItemClassName = "." + cssClassTitleName,
    ItemPriceClassName = "." + cssClassPriceName
};

GenericScraper genericScraper = new GenericScraper(webSite);
await genericScraper.ScrapeAsync();
