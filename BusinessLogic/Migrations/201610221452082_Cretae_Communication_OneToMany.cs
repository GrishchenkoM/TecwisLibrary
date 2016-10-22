namespace BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cretae_Communication_OneToMany : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "AuthorId", c => c.Int());
            CreateIndex("dbo.Books", "AuthorId");
            AddForeignKey("dbo.Books", "AuthorId", "dbo.Authors", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Books", new[] { "AuthorId" });
            AlterColumn("dbo.Books", "AuthorId", c => c.Int(nullable: false));
        }
    }
}
