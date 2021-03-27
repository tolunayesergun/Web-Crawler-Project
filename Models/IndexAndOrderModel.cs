using System.Collections.Generic;

namespace WebCrawlerProject.Models
{
    public class IndexAndOrderModel
    {
        public IndexAndOrderModel()
        {
            SubSites = new List<UrlModel>();
        }

        public UrlModel BaseSite { get; set; }
        public List<UrlModel> SubSites { get; set; }
    }
}