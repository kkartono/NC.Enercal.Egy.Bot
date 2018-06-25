namespace QnABot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeedbackDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FeedbackEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        question = c.String(),
                        feedback = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FeedbackEntities");
        }
    }
}
