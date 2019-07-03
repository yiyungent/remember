using WebUI.Models;
/****************************************************************************
 *Copyright (c) 2016 All Rights Reserved.
 *CLR版本： 4.0.30319.42000
 *机器名称：DESKTOP-V7CFIC3
 *公司名称：
 *命名空间：SearchDemo.Common
 *文件名：  KeyWordsTotalService
 *版本号：  V1.0.0.0
 *唯一标识：9261d285-e9d5-48b0-8f47-d17abc326f50
 *当前的用户域：DESKTOP-V7CFIC3
 *创建人：  zouqi
 *电子邮箱：zouyujie@126.com
 *创建时间：2016/7/9 15:03:35

 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：2016/7/9 15:03:35
 *修改人： zouqi
 *版本号： V1.0.0.0
 *描述：
 *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WebUI.Models.SearchVM;

namespace WebUI.Infrastructure.Search
{
    public class KeyWordsTotalService
    {
        private SearchDbContext db = new SearchDbContext();

        public List<string> GetSearchMsg(string term)
        {
            try
            {
                //存在SQL注入的安全隐患
                //string sql = "select KeyWords from SearchTotals where KeyWords like '"+term.Trim()+"%'";
                //return db.Database.SqlQuery<string>(sql).ToList();
                string sql = "select KeyWords from SearchTotals where KeyWords like @term";
                return db.Database.SqlQuery<string>(sql, new SqlParameter("@term", term + "%")).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}