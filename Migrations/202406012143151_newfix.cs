namespace ProjectManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newfix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FieldModels", "Description", c => c.String(nullable: false, maxLength: 250));
            DropColumn("dbo.FieldModels", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FieldModels", "Type", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.FieldModels", "Description");
        }
    }
}
