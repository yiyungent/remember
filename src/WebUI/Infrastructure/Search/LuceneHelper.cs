using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using PanGu;
/****************************************************************************
 *Copyright (c) 2016 All Rights Reserved.
 *CLR版本： 4.0.30319.42000
 *机器名称：DESKTOP-V7CFIC3
 *公司名称：
 *命名空间：SearchDemo.Common
 *文件名：  LuceneHelper
 *版本号：  V1.0.0.0
 *唯一标识：88d536d1-1717-4d0e-9d64-a15b158031be
 *当前的用户域：DESKTOP-V7CFIC3
 *创建人：  zouqi
 *电子邮箱：zouyujie@126.com
 *创建时间：2016/7/9 10:59:41

 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：2016/7/9 10:59:41
 *修改人： zouqi
 *版本号： V1.0.0.0
 *描述：
 *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebUI.Infrastructure.Search
{
    public class LuceneHelper
    {
        public static List<string> PanGuSplitWord(string msg)
        {
            Analyzer analyzer = new PanGuAnalyzer();
            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(msg));
            Lucene.Net.Analysis.Token token = null;
            List<string> list = new List<string>();
            while ((token = tokenStream.Next()) != null)
            {
                list.Add(token.TermText());
            }
            return list;
        }
        // /创建HTMLFormatter,参数为高亮单词的前后缀
        public static string CreateHightLight(string keywords, string Content)
        {
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter =
             new PanGu.HighLight.SimpleHTMLFormatter("<font color=\"red\">", "</font>");
            //创建Highlighter ，输入HTMLFormatter 和盘古分词对象Semgent
            PanGu.HighLight.Highlighter highlighter =
            new PanGu.HighLight.Highlighter(simpleHTMLFormatter,
            new Segment());
            //设置每个摘要段的字符数
            highlighter.FragmentSize = 150;
            //获取最匹配的摘要段
            return highlighter.GetBestFragment(keywords, Content);
        }
        static public string GetKeyWordsSplitBySpace(string keywords, PanGuTokenizer ktTokenizer)
        {
            StringBuilder result = new StringBuilder();

            ICollection<WordInfo> words = ktTokenizer.SegmentToWordInfos(keywords);

            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }

                result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
            }

            return result.ToString().Trim();
        }

    }
}