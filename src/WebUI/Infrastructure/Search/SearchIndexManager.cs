using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
/****************************************************************************
 *Copyright (c) 2016 All Rights Reserved.
 *CLR版本： 4.0.30319.42000
 *机器名称：DESKTOP-V7CFIC3
 *公司名称：
 *命名空间：SearchDemo.Models
 *文件名：  SearchIndexManager
 *版本号：  V1.0.0.0
 *唯一标识：892d9260-f3d7-4e81-b037-7fdadb8726c8
 *当前的用户域：DESKTOP-V7CFIC3
 *创建人：  zouqi
 *电子邮箱：zouyujie@126.com
 *创建时间：2016/7/9 10:09:05

 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：2016/7/9 10:09:05
 *修改人： zouqi
 *版本号： V1.0.0.0
 *描述：
 *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Configuration;
using WebUI.Models;
using WebUI.Models.SearchVM;
using System.Web.Hosting;

namespace WebUI.Infrastructure.Search
{
    public sealed class SearchIndexManager
    {
        private static readonly SearchIndexManager searchIndexManager = new SearchIndexManager();
        private SearchIndexManager()
        {
        }
        public static SearchIndexManager GetInstance()
        {
            return searchIndexManager;
        }
        Queue<IndexContent> queue = new Queue<IndexContent>();
        /// <summary>
        /// 向队列中添加数据
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public void AddQueue(string Id, string title, string content, DateTime createTime)
        {
            IndexContent indexContent = new IndexContent();
            indexContent.Id = Id;
            indexContent.Title = title;
            indexContent.Content = content;
            indexContent.LuceneEnum = LuceneEnum.AddType;// 添加
            indexContent.CreateTime = createTime.ToString();
            queue.Enqueue(indexContent);
        }
        /// <summary>
        /// 向队列中添加要删除数据
        /// </summary>
        /// <param name="Id"></param>
        public void DeleteQueue(string Id)
        {
            IndexContent indexContent = new IndexContent();
            indexContent.Id = Id;
            indexContent.LuceneEnum = LuceneEnum.DeleType;//删除
            queue.Enqueue(indexContent);
        }

        /// <summary>
        /// 开启线程，扫描队列，从队列中获取数据
        /// </summary>
        public void StartThread()
        {
            Thread myThread = new Thread(WriteIndexContent);
            myThread.IsBackground = true;
            myThread.Start();
        }
        private void WriteIndexContent()
        {
            while (true)
            {
                if (queue.Count > 0)
                {
                    CreateIndexContent();
                }
                else
                {
                    Thread.Sleep(5000);//避免造成CPU空转
                }
            }
        }
        private void CreateIndexContent()
        {
            string indexPath = ConfigurationManager.AppSettings["LuceneDir"];//注意和磁盘上文件夹的大小写一致，否则会报错。将创建的分词内容放在该目录下。
            // 是否为服务器相对路径
            if (indexPath.StartsWith("~"))
            {
                indexPath = HostingEnvironment.MapPath(indexPath);
            }
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());//指定索引文件(打开索引目录) FS指的是就是FileSystem
            bool isUpdate = IndexReader.IndexExists(directory);//IndexReader:对索引进行读取的类。该语句的作用：判断索引库文件夹是否存在以及索引特征文件是否存在。
            if (isUpdate)
            {
                //同时只能有一段代码对索引库进行写操作。当使用IndexWriter打开directory时会自动对索引库文件上锁。
                //如果索引目录被锁定（比如索引过程中程序异常退出），则首先解锁
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate, Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED);//向索引库中写索引。这时在这里加锁。

            while (queue.Count > 0)
            {
                IndexContent indexContent = queue.Dequeue();//将队列中的数据出队
                writer.DeleteDocuments(new Term("Id", indexContent.Id.ToString()));
                if (indexContent.LuceneEnum == LuceneEnum.DeleType)
                {
                    continue;
                }
                Document document = new Document();//表示一篇文档。
                //Field.Store.YES:表示是否存储原值。只有当Field.Store.YES在后面才能用doc.Get("Id")取出值来.Field.Index. NOT_ANALYZED:不进行分词保存
                document.Add(new Field("Id", indexContent.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));

                //Field.Index. ANALYZED:进行分词保存:也就是要进行全文的字段要设置分词 保存（因为要进行模糊查询）
                //Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS:不仅保存分词还保存分词的距离。
                document.Add(new Field("Title", indexContent.Title, Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                document.Add(new Field("Content", indexContent.Content, Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                document.Add(new Field("CreateTime", indexContent.CreateTime, Field.Store.YES, Field.Index.NOT_ANALYZED));

                writer.AddDocument(document);
            }

            writer.Close();//会自动解锁。
            directory.Close();//不要忘了Close，否则索引结果搜不到
        }
    }
}