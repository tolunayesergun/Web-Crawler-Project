using HtmlAgilityPack;
using System.Net;
using System.Text;

namespace WebCrawlerProject.Helpers
{
    public static class Crawler
    {
        public static HtmlDocument GetHtmlContent(string url)
        {
            try
            {
                var client = new WebClient();
                var document = new HtmlDocument();
                document.Load(client.OpenRead(url), Encoding.UTF8);
                return document;

                #region CommentBlock
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //if (response.StatusCode == HttpStatusCode.OK)
                //{
                //    Stream receiveStream = response.GetResponseStream();
                //    StreamReader readStream = null;

                //    if (string.IsNullOrWhiteSpace(response.CharacterSet))
                //        readStream = new StreamReader(receiveStream);
                //    else
                //        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                //    string data = readStream.ReadToEnd();

                //    response.Close();
                //    readStream.Close();

                //    return data;
                //}
                //else
                //{
                //    return response.StatusCode.ToString();
                //}
                #endregion CommentBlock
            }
            catch
            {
                return null;
            }
        }
    }
}
