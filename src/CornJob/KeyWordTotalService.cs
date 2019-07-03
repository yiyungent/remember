using MySql.Data.MySqlClient;
using System.Configuration;

namespace CornJob
{
    public class KeyWordTotalService
    {
        private string _connStr;

        public KeyWordTotalService()
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

                string sql = "insert into SearchTotal(KeyWord,SearchCount) select KeyWord,count(*)  from SearchDetail where DateDiff(SearchDetail.SearchTime,now())<=30 group by SearchDetail.KeyWord";

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

                string sql = "truncate table SearchTotal";

                cmd.CommandText = sql;

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
