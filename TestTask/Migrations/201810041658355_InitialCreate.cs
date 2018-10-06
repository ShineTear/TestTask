namespace TestTask.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appartments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        MeterId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Meters", t => t.MeterId)
                .Index(t => t.MeterId);
            
            CreateTable(
                "dbo.Meters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MeterNumber = c.String(),
                        LastCheckData = c.DateTime(),
                        NextCheckData = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MeterDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Double(nullable: false),
                        CheckData = c.DateTime(nullable: false),
                        MeterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Meters", t => t.MeterId, cascadeDelete: true)
                .Index(t => t.MeterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appartments", "MeterId", "dbo.Meters");
            DropForeignKey("dbo.MeterDatas", "MeterId", "dbo.Meters");
            DropIndex("dbo.MeterDatas", new[] { "MeterId" });
            DropIndex("dbo.Appartments", new[] { "MeterId" });
            DropTable("dbo.MeterDatas");
            DropTable("dbo.Meters");
            DropTable("dbo.Appartments");
        }
    }
}
