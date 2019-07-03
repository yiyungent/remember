using System;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models.SearchVM
{
    public class SearchTotal
    {
        public Guid Id { get; set; }
        [StringLength(50)]
        public string KeyWords { get; set; }
        public int SearchCounts { get; set; }
    }

}