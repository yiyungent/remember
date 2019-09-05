using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.CourseBoxVM
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
        public int CourseBoxId { get; set; }

        public int CourseInfoId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }
    }
}