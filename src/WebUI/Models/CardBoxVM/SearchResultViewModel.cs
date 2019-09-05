using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.CardBoxVM
{
    public class SearchResultViewModel
    {
        public SearchResultViewModel()
        {
            this.List = new List<SearchResultItem>();
        }

        public IList<SearchResultItem> List { get; set; }
    }

    public class SearchResultItem
    {
        public int CardBoxId { get; set; }

        public int CardInfoId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }
    }
}