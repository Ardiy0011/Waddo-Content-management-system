namespace ProjectManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecreateDeletedTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectModels",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FieldModels",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Description = c.String(),
                    ProjectId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectModels", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.FieldModels", "ProjectId", "dbo.ProjectModels");
            DropIndex("dbo.FieldModels", new[] { "ProjectId" });
            DropTable("dbo.FieldModels");
            DropTable("dbo.ProjectModels");
        }
    }
}
