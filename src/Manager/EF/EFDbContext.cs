using Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Manager.EF
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class EFDbContext : DbContext
    {
        public EFDbContext() : base("name=EFDbContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //移除自动建表时自动加上s的复数形式
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<SearchTotal> SearchTotal { get; set; }

        public DbSet<SearchDetail> SearchDetail { get; set; }

    }
}