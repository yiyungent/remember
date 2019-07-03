using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebUI.Models.SearchVM
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class SearchDbContext : DbContext
    {
        public SearchDbContext() : base("name=SearchDbContext") { }

        //下面两个属性是新增加的
        public DbSet<SearchTotal> SearchTotal { get; set; }
        public DbSet<SearchDetail> SearchDetail { get; set; }
    }
}