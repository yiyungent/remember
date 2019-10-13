using System;

namespace WebUI.Models.SearchVM
{
    public class SearchResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public string Url { get; set; }
    }

}