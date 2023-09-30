using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Entities
{
    public class WebSite
    {
        public string Url { get; set; }
        public string ChromeExecutablePath { get; set; }
        public string ItemClassName { get; set; }
        public string ItemPriceClassName { get; set; }
    }
}
