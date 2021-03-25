using System.Collections.Generic;

namespace WebCrawlerProject.Models
{
    public class CompareModel
    {
        public CompareModel()
        {
            WordList = new List<WordModel>();
        }

        public int Score { get; set; }
        public UrlModel FirstSite { get; set; }
        public UrlModel SecondSite { get; set; }
        public List<WordModel> WordList { get; set; }
    }
}