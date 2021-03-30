using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebCrawlerProject.Helpers;
using WebCrawlerProject.Models;

namespace WebCrawlerProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string[] Site)
        {
            var allSiteData = new List<UrlModel>();
            if (Site != null && Site.Length > 0)
            {
                foreach (var item in Site)
                {
                    allSiteData.Add(HtmlParser.GetPageInfoByUrl(item));
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public JsonResult GetData(string weburl)
        {
            var WordList = HtmlParser.GetPageInfoByUrl(weburl, false);
            return Json(WordList);
        }

        [HttpPost]
        public JsonResult GetKeywordResult(string weburl)
        {
            var WordList = HtmlParser.GetPageInfoByUrl(weburl, false);
            WordList.WordList = WordList.WordList.OrderByDescending(x => x.Score).Take(10).ToList();
            return Json(WordList);
        }

        [HttpPost]
        public JsonResult CompareTwoSite(string weburl1, string weburl2)
        {
            var result = CompareFunction(weburl1, weburl2);
            return Json(result);
        }

        [HttpPost]
        public JsonResult IndexAndOrderSites(string baseUrl, string[] siteList)
        {
            var result = IndexAndOrder(baseUrl, siteList);

            return Json(result);
        }

        [HttpPost]
        public JsonResult SymanticIndexAndOrder(string baseUrl, string[] siteList)
        {
            var result = Symantic(baseUrl, siteList);

            return Json(result);
        }

        [HttpPost]
        public ActionResult viewPart1()
        {

            return PartialView("~/Views/PartialViews/partialPart1.cshtml");
        }

        [HttpPost]
        public ActionResult viewPart2()
        {

            return PartialView("~/Views/PartialViews/partialPart2.cshtml");
        }

        [HttpPost]
        public ActionResult viewPart3()
        {

            return PartialView("~/Views/PartialViews/partialPart3.cshtml");
        }

        [HttpPost]
        public ActionResult viewPart4()
        {

            return PartialView("~/Views/PartialViews/partialPart4.cshtml");
        }

        [HttpPost]
        public ActionResult viewPart5()
        {

            return PartialView("~/Views/PartialViews/partialPart5.cshtml");
        }

        private CompareModel CompareFunction(string weburl1, string weburl2)
        {
            var WordList1 = HtmlParser.GetPageInfoByUrl(weburl1, false);
            var WordList2 = HtmlParser.GetPageInfoByUrl(weburl2, false);
            WordList1.WordList = WordList1.WordList.OrderByDescending(x => x.Score).Take(10).ToList();

            var matchList = new CompareModel
            {
                FirstSite = WordList1,
                SecondSite = WordList2,
                Score = 1
            };

            foreach (var item in WordList1.WordList)
            {
                if (WordList2.WordList.Any(x => x.Word.ToUpper().Replace("İ", "I") == item.Word.ToUpper().Replace("İ", "I")))
                {
                    var word = WordList2.WordList.FirstOrDefault(x => x.Word.ToUpper().Replace("İ", "I") == item.Word.ToUpper().Replace("İ", "I"));
                    matchList.WordList.Add(new WordModel { Word = item.Word, Frequency = word.Frequency });
                    matchList.Score *= word.Frequency;
                }
            }

            var secondTotalCount = WordList2.WordList.Sum(x => x.Frequency);
            matchList.Score /= secondTotalCount;
            matchList.Divide = secondTotalCount;

            matchList.Score = decimal.Round(matchList.Score, 4, MidpointRounding.AwayFromZero);

            WordList1.WordList = WordList1.WordList.OrderByDescending(x => x.Frequency).Take(10).ToList();
            WordList2.WordList = WordList2.WordList.OrderByDescending(x => x.Frequency).Take(10).ToList();
            matchList.WordList = matchList.WordList.OrderByDescending(x => x.Frequency).ToList();
            return matchList;
        }

        private IndexAndOrderModel IndexAndOrder(string baseUrl, string[] siteList)
        {
            var resultModel = new IndexAndOrderModel
            {
                BaseSite = HtmlParser.GetPageInfoByUrl(baseUrl, true)
            };

            foreach (var siteUrl in siteList)
            {
                var subSiteInfo = HtmlParser.GetPageInfoByUrl(siteUrl, true);
                subSiteInfo.KeywordList = new List<WordModel>();
                subSiteInfo.IndexingScore = 0;

                subSiteInfo.IndexingScore = CalcIndexScore(subSiteInfo, subSiteInfo.IndexingScore, resultModel.BaseSite.KeywordList);
                if (subSiteInfo.ChildUrlList != null)
                {
                    foreach (var childs in subSiteInfo.ChildUrlList)
                    {
                        var res = CalcIndexScore(childs, subSiteInfo.IndexingScore, resultModel.BaseSite.KeywordList);

                        if (childs.ChildUrlList != null)
                        {
                            foreach (var subChilds in childs.ChildUrlList)
                            {
                                var subRes = CalcIndexScore(subChilds, subSiteInfo.IndexingScore, resultModel.BaseSite.KeywordList);
                            }
                        }
                    }
                }

                var secondTotalCount = subSiteInfo.AllWordOffSite.Sum(x => x.Frequency);

                subSiteInfo.IndexingScore /= secondTotalCount;

                subSiteInfo.IndexingScore = decimal.Round(subSiteInfo.IndexingScore, 4, MidpointRounding.AwayFromZero);
                resultModel.SubSites.Add(subSiteInfo);
            }

            resultModel.SubSites = resultModel.SubSites.OrderByDescending(x => x.IndexingScore).ToList();
            resultModel.BaseSite.KeywordList = resultModel.BaseSite.KeywordList.Where(x => x.SynmonsList != null).ToList();
            return resultModel;
        }

        private decimal CalcIndexScore(UrlModel model, decimal score, List<WordModel> keys)
        {
            model.KeywordList = new List<WordModel>();
            foreach (var item in keys)
            {
                if (model.WordList.Any(x => x.Word.ToUpper().Replace("İ", "I") == item.Word.ToUpper().Replace("İ", "I")))
                {
                    score = score != 0 ? score : 1;
                    var word = model.WordList.FirstOrDefault(x => x.Word.ToUpper().Replace("İ", "I") == item.Word.ToUpper().Replace("İ", "I"));
                    model.KeywordList.Add(word);
                    try
                    {
                        score *= word.Frequency;
                    }
                    catch (Exception)
                    {
                        score = decimal.MaxValue;
                    }
                }
            }
            return score;
        }

        private decimal CalcScore(UrlModel model, decimal score, List<String> tempList)
        {
            model.KeywordList = new List<WordModel>();
            foreach (var item in tempList)
            {
                if (model.WordList.Any(x => x.Word.Trim().ToUpper().Replace("İ", "I") == item.Trim().ToUpper().Replace("İ", "I")))
                {
                    score = score != 0 ? score : 1;
                    var word = model.WordList.FirstOrDefault(x => x.Word.Trim().ToUpper().Replace("İ", "I") == item.Trim().ToUpper().Replace("İ", "I"));
                    model.KeywordList.Add(word);
                    try
                    {
                        score *= word.Frequency;
                    }
                    catch (Exception)
                    {
                        score = decimal.MaxValue;
                    }
                }
            }
            return score;
        }

        private IndexAndOrderModel Symantic(string baseUrl, string[] siteList)
        {
            var resultModel = new IndexAndOrderModel
            {
                BaseSite = HtmlParser.GetPageInfoByUrl(baseUrl, true)
            };

            var tempList = new List<String>();
            foreach (var keyword in resultModel.BaseSite.KeywordList)
            {
                tempList.AddRange(HtmlParser.FindSynonyms(keyword));
            }

            tempList = tempList.GroupBy(x => x).Select(x => x.Key).ToList();

            foreach (var siteUrl in siteList)
            {
                var subSiteInfo = HtmlParser.GetPageInfoByUrl(siteUrl, true);
                subSiteInfo.KeywordList = new List<WordModel>();
                subSiteInfo.IndexingScore = 0;

                subSiteInfo.IndexingScore = CalcScore(subSiteInfo, subSiteInfo.IndexingScore, tempList);
                if (subSiteInfo.ChildUrlList != null)
                {
                    foreach (var childs in subSiteInfo.ChildUrlList)
                    {
                        var res = CalcScore(childs, subSiteInfo.IndexingScore, tempList);

                        if (childs.ChildUrlList != null)
                        {
                            foreach (var subChilds in childs.ChildUrlList)
                            {
                                var subRes = CalcScore(subChilds, subSiteInfo.IndexingScore, tempList);
                            }
                        }
                    }
                }

                var secondTotalCount = subSiteInfo.AllWordOffSite.Sum(x => x.Frequency);

                subSiteInfo.IndexingScore /= secondTotalCount;

                subSiteInfo.IndexingScore = decimal.Round(subSiteInfo.IndexingScore, 4, MidpointRounding.AwayFromZero);
                resultModel.SubSites.Add(subSiteInfo);
            }

            resultModel.SubSites = resultModel.SubSites.OrderByDescending(x => x.IndexingScore).ToList();
            resultModel.BaseSite.KeywordList = resultModel.BaseSite.KeywordList.Where(x => x.SynmonsList != null).ToList();
            return resultModel;
        }
    }
}
