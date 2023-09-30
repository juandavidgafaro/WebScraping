using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Interface
{
    public interface IScraper
    {
        Task ScrapeAsync();
    }
}
