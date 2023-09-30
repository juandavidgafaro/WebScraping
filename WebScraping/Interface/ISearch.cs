using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Interface
{
    public interface ISearch
    {
        Task<List<string>> SearchItemNames(Page page);
        Task<List<double>> SearchItemPrices(Page page);
    }
}
