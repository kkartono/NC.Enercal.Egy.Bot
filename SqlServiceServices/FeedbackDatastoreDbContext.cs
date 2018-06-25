using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QnABot.SqlServiceServices
{
    public class FeedbackDatastoreDbContext : DbContext
    {
        public FeedbackDatastoreDbContext()
           : this("BotDatastoreContextConnectionString")
        {
        }
        public FeedbackDatastoreDbContext(string connectionStringName)
            : base(connectionStringName)
        {
        }
        public DbSet<FeedbackEntity> FeedbackData { get; set; }
    }
}