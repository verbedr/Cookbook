namespace Cookbook.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Setup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsedIn_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Recipes", t => t.UsedIn_Id, cascadeDelete: true)
                .Index(t => t.UsedIn_Id);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uri = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HowToSteps",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        NextStep_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Recipes", t => t.Id)
                .ForeignKey("dbo.HowToSteps", t => t.NextStep_Id)
                .Index(t => t.Id)
                .Index(t => t.NextStep_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        From_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stores", t => t.From_Id)
                .Index(t => t.From_Id);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "From_Id", "dbo.Stores");
            DropForeignKey("dbo.HowToSteps", "NextStep_Id", "dbo.HowToSteps");
            DropForeignKey("dbo.HowToSteps", "Id", "dbo.Recipes");
            DropForeignKey("dbo.Ingredients", "UsedIn_Id", "dbo.Recipes");
            DropIndex("dbo.Products", new[] { "From_Id" });
            DropIndex("dbo.HowToSteps", new[] { "NextStep_Id" });
            DropIndex("dbo.HowToSteps", new[] { "Id" });
            DropIndex("dbo.Ingredients", new[] { "UsedIn_Id" });
            DropTable("dbo.Stores");
            DropTable("dbo.Products");
            DropTable("dbo.HowToSteps");
            DropTable("dbo.Recipes");
            DropTable("dbo.Ingredients");
        }
    }
}
