
using MySql.Data.MySqlClient;
using System.Configuration;

namespace CornJob
{
    public class KeyWordsTotalService
    {
        private string _connStr;

        public KeyWordsTotalService()
        {
            this._connStr = ConfigurationManager.ConnectionStrings["CornJobDbContext"].ConnectionString;
        }

        /// <summary>
        /// 将统计的明细表的数据插入。
        /// </summary>
        /// <returns></returns>
        public bool InsertKeyWordsRank()
        {
            using (MySqlConnection conn = new MySqlConnection(_connStr))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                string sql = "insert into SearchTotals(Id,KeyWords,SearchCounts) select uuid(),KeyWords,count(*)  from SearchDetails where DateDiff(SearchDetails.SearchDateTime,now())<=30 group by SearchDetails.KeyWords";

                cmd.CommandText = sql;

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// 删除汇总中的数据。
        /// </summary>
        /// <returns></returns>
        public bool DeleteAllKeyWordsRank()
        {
            using (MySqlConnection conn = new MySqlConnection(_connStr))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                string sql = "truncate table SearchTotals";

                cmd.CommandText = sql;

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
