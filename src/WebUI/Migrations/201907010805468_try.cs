namespace WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _try : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SearchDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        KeyWords = c.String(maxLength: 50, storeType: "nvarchar"),
                        SearchDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SearchTotals",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        KeyWords = c.String(maxLength: 50, storeType: "nvarchar"),
                        SearchCounts = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SearchTotals");
            DropTable("dbo.SearchDetails");
        }
    }
}
