namespace WebCrawlerProject.Models
{
    public class WordModel
    {
        public string Word { get; set; }
        public int Frequency { get; set; }
        public int Score { get; set; }
        public SynonymDTO SynmonsList { get; set; }
    }
}