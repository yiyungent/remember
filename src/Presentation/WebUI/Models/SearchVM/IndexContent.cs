namespace WebUI.Models.SearchVM
{
    public class IndexContent
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string CreateTime { get; set; }

        public string Url { get; set; }

        public LuceneEnum LuceneEnum { get; set; }
    }

}