using Core;
using Domain;
using Framework.HtmlHelpers;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Manager.EF;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using WebUI.Infrastructure.Search;
using WebUI.Models.SearchVM;

namespace WebUI.Controllers
{
    public class SearchController : Controller
    {
        #region Fields
        private EFDbContext _efDbContext = new EFDbContext();

        private SearchTotalService _searchTotalService;

        private string _indexPath;
        #endregion

        #region Ctor
        public SearchController()
        {
            string indexPath = System.Configuration.ConfigurationManager.AppSettings["LuceneDir"];
            // 是否为服务器相对路径
            if (indexPath.StartsWith("~"))
            {
                indexPath = HostingEnvironment.MapPath(indexPath);
            }
            this._indexPath = indexPath;
            this._searchTotalService = Container.Instance.Resolve<SearchTotalService>();
        }
        #endregion

        #region 首页
        public ActionResult Index(string txtSearch, bool? hidfIsOr, int id = 1)
        {
            PagedList<SearchResult> viewModel = null;
            if (!string.IsNullOrEmpty(txtSearch))//如果点击的是查询按钮
            {
                viewModel = (hidfIsOr == null || hidfIsOr.Value == false) ? AndSearch(txtSearch, id) : OrSearch(txtSearch, id);
            }
            var keyWords = _efDbContext.SearchTotal.OrderByDescending(a => a.SearchCount).Select(x => x.KeyWord).Skip(0).Take(6).ToList();
            ViewBag.KeyWords = keyWords;

            return View(viewModel);
        }
        #endregion

        #region 与查询
        //与查询
        private PagedList<SearchResult> AndSearch(string keyword, int pageIndex, int pageSize = 4)
        {
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(_indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            PhraseQuery query = new PhraseQuery();//查询条件
            PhraseQuery titleQuery = new PhraseQuery();//标题查询条件
            List<string> lstkw = LuceneHelper.PanGuSplitWord(keyword);//对用户输入的搜索条件进行拆分。

            foreach (string word in lstkw)
            {
                query.Add(new Term("Content", word));//contains("Content",word)
                titleQuery.Add(new Term("Title", word));
            }
            query.SetSlop(100);//两个词的距离大于100（经验值）就不放入搜索结果，因为距离太远相关度就不高了

            BooleanQuery bq = new BooleanQuery();
            //Occur.Should 表示 Or , Must 表示 and 运算
            bq.Add(query, BooleanClause.Occur.SHOULD);
            bq.Add(titleQuery, BooleanClause.Occur.SHOULD);

            TopScoreDocCollector collector = TopScoreDocCollector.create(1000, true);//盛放查询结果的容器
            searcher.Search(bq, null, collector);//使用query这个查询条件进行搜索，搜索结果放入collector

            int recCount = collector.GetTotalHits();//总的结果条数
            ScoreDoc[] docs = collector.TopDocs((pageIndex - 1) * pageSize, pageIndex * pageSize).scoreDocs;//从查询结果中取出第m条到第n条的数据

            List<SearchResult> list = new List<SearchResult>();
            string msg = string.Empty;
            string title = string.Empty;

            for (int i = 0; i < docs.Length; i++)//遍历查询结果
            {
                int docId = docs[i].doc;//拿到文档的id，因为Document可能非常占内存（思考DataSet和DataReader的区别）
                //所以查询结果中只有id，具体内容需要二次查询
                Document doc = searcher.Doc(docId);//根据id查询内容。放进去的是Document，查出来的还是Document
                SearchResult result = new SearchResult();
                result.Id = Convert.ToInt32(doc.Get("Id"));
                msg = doc.Get("Content");//只有 Field.Store.YES的字段才能用Get查出来
                result.Content = LuceneHelper.CreateHightLight(keyword, msg);//将搜索的关键字高亮显示。
                title = doc.Get("Title");
                foreach (string word in lstkw)
                {
                    title = title.Replace(word, "<span style='color:red;'>" + word + "</span>");
                }
                //result.Title=LuceneHelper.CreateHightLight(kw, title);
                result.Title = title;
                result.CreateTime = Convert.ToDateTime(doc.Get("CreateTime"));
                // 找出此文章的 url
                result.Url = doc.Get("Url");
                list.Add(result);
            }
            //先将搜索的词插入到明细表。
            SearchDetail searchDetail = new SearchDetail { ID = Guid.NewGuid(), KeyWord = keyword, SearchTime = DateTime.Now };
            _efDbContext.SearchDetail.Add(searchDetail);
            int r = _efDbContext.SaveChanges();

            PagedList<SearchResult> lst = new PagedList<SearchResult>(list, pageIndex, pageSize, recCount);

            return lst;
        }
        #endregion

        #region 或查询
        //或查询
        private PagedList<SearchResult> OrSearch(string keyword, int pageIndex, int pageSize = 4)
        {
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(_indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            List<PhraseQuery> lstQuery = new List<PhraseQuery>();
            List<string> lstkw = LuceneHelper.PanGuSplitWord(keyword);//对用户输入的搜索条件进行拆分。

            foreach (string word in lstkw)
            {
                PhraseQuery query = new PhraseQuery();//查询条件
                query.SetSlop(100);//两个词的距离大于100（经验值）就不放入搜索结果，因为距离太远相关度就不高了
                query.Add(new Term("Content", word));//contains("Content",word)

                PhraseQuery titleQuery = new PhraseQuery();//查询条件
                titleQuery.Add(new Term("Title", word));

                lstQuery.Add(query);
                lstQuery.Add(titleQuery);
            }

            BooleanQuery bq = new BooleanQuery();
            foreach (var v in lstQuery)
            {
                //Occur.Should 表示 Or , Must 表示 and 运算
                bq.Add(v, BooleanClause.Occur.SHOULD);
            }
            TopScoreDocCollector collector = TopScoreDocCollector.create(1000, true);//盛放查询结果的容器
            searcher.Search(bq, null, collector);//使用query这个查询条件进行搜索，搜索结果放入collector

            int recCount = collector.GetTotalHits();//总的结果条数
            ScoreDoc[] docs = collector.TopDocs((pageIndex - 1) * pageSize, pageIndex * pageSize).scoreDocs;//从查询结果中取出第m条到第n条的数据

            List<SearchResult> list = new List<SearchResult>();
            string msg = string.Empty;
            string title = string.Empty;

            for (int i = 0; i < docs.Length; i++)//遍历查询结果
            {
                int docId = docs[i].doc;//拿到文档的id，因为Document可能非常占内存（思考DataSet和DataReader的区别）
                //所以查询结果中只有id，具体内容需要二次查询
                Document doc = searcher.Doc(docId);//根据id查询内容。放进去的是Document，查出来的还是Document
                SearchResult result = new SearchResult();
                result.Id = Convert.ToInt32(doc.Get("Id"));
                msg = doc.Get("Content");//只有 Field.Store.YES的字段才能用Get查出来
                title = doc.Get("Title");
                //将搜索的关键字高亮显示。
                foreach (string word in lstkw)
                {
                    title = title.Replace(word, "<span style='color:red;'>" + word + "</span>");
                }
                result.Content = LuceneHelper.CreateHightLight(keyword, msg);
                result.Title = title;
                result.CreateTime = Convert.ToDateTime(doc.Get("CreateTime"));
                // 找出此文章的 url
                result.Url = doc.Get("Url");
                list.Add(result);
            }
            //先将搜索的词插入到明细表。
            SearchDetail searchDetail = new SearchDetail { ID = Guid.NewGuid(), KeyWord = keyword, SearchTime = DateTime.Now };
            _efDbContext.SearchDetail.Add(searchDetail);
            int r = _efDbContext.SaveChanges();

            PagedList<SearchResult> lst = new PagedList<SearchResult>(list, pageIndex, pageSize, recCount);

            return lst;
        }
        #endregion

        #region 获得搜索下拉热词
        public JsonResult GetKeyWordList(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return null;
            }

            IList<string> searchTotals = _searchTotalService.GetKeyWordList(term);

            return Json(searchTotals, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}