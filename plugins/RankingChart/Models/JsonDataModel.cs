using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RankingChart.Models
{
    public class JsonDataModel
    {
        public string name { get; set; }

        public string type { get; set; }

        public string value { get; set; }

        /// <summary>
        /// YYYY-MM-DD
        /// </summary>
        public string date { get; set; }
    }
}