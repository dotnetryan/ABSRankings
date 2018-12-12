namespace ABSRankings.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTableOverallPercentages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OverallPercentages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlayerId = c.Int(nullable: false),
                        DifficultyId = c.Int(nullable: false),
                        MetricId = c.Int(nullable: false),
                        Percentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Difficulty", t => t.DifficultyId, cascadeDelete: true)
                .ForeignKey("dbo.Metric", t => t.MetricId, cascadeDelete: true)
                .ForeignKey("dbo.Player", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.PlayerId)
                .Index(t => t.DifficultyId)
                .Index(t => t.MetricId);            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OverallPercentages", "PlayerId", "dbo.Player");
            DropForeignKey("dbo.OverallPercentages", "MetricId", "dbo.Metric");
            DropForeignKey("dbo.OverallPercentages", "DifficultyId", "dbo.Difficulty");
            DropIndex("dbo.OverallPercentages", new[] { "MetricId" });
            DropIndex("dbo.OverallPercentages", new[] { "DifficultyId" });
            DropIndex("dbo.OverallPercentages", new[] { "PlayerId" });
        }
    }
}
