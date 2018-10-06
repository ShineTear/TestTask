namespace TestTask.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appartments", "MeterId", "dbo.Meters");
            DropIndex("dbo.Appartments", new[] { "MeterId" });
            AddColumn("dbo.Meters", "AppartmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Meters", "AppartmentId");
            AddForeignKey("dbo.Meters", "AppartmentId", "dbo.Appartments", "Id", cascadeDelete: true);
            DropColumn("dbo.Appartments", "MeterId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appartments", "MeterId", c => c.Int());
            DropForeignKey("dbo.Meters", "AppartmentId", "dbo.Appartments");
            DropIndex("dbo.Meters", new[] { "AppartmentId" });
            DropColumn("dbo.Meters", "AppartmentId");
            CreateIndex("dbo.Appartments", "MeterId");
            AddForeignKey("dbo.Appartments", "MeterId", "dbo.Meters", "Id");
        }
    }
}
