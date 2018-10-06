namespace TestTask.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewMeter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meters", "StartValue", c => c.Double(nullable: false));
            AddColumn("dbo.Meters", "LastValue", c => c.Double(nullable: false));
            AddColumn("dbo.Meters", "InstallationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Meters", "LastMeterId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Meters", "LastMeterId");
            DropColumn("dbo.Meters", "InstallationDate");
            DropColumn("dbo.Meters", "LastValue");
            DropColumn("dbo.Meters", "StartValue");
        }
    }
}
