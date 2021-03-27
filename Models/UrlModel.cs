using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawlerProject.Models
{
    public class UrlModel
    {
        public UrlModel()
        {
            ChildUrlList = new List<UrlModel>();
            KeywordList = new List<WordModel>();
            AllWordOffSite = new List<WordModel>();
        }

        public string Url { get; set; }
        public int Level { get; set; }
        public decimal IndexingScore { get; set; }
        public List<WordModel> WordList { get; set; }
        public List<WordModel> KeywordList { get; set; }
        public List<WordModel> AllWordOffSite { get; set; }
        public List<UrlModel> ChildUrlList { get; set; }
    }
}
