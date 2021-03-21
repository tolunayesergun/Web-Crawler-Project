using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawlerProject.Models
{
    public class WordModel
    {
        public string Word { get; set; }
        public int Frequency { get; set; }
        public int Score { get; set; }
    }
}
