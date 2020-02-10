namespace chat.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncludeDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "DateSend", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sessions", "DateCreate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Rooms", "DateCreate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "DateCreate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "DateCreate");
            DropColumn("dbo.Rooms", "DateCreate");
            DropColumn("dbo.Sessions", "DateCreate");
            DropColumn("dbo.Messages", "DateSend");
        }
    }
}
