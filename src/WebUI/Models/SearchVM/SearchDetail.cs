using System;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models.SearchVM
{
    public class SearchDetail
    {
        public Guid Id { get; set; }
        [StringLength(50)]
        public string KeyWords { get; set; }
        public Nullable<DateTime> SearchDateTime { get; set; }
    }

}