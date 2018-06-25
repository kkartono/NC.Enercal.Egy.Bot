using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QnABot.DataServices
{
    public class FeedbackDbContext : DbContext
    {
        public FeedbackDbContext()
           : this("BotDataContextConnectionString")
        {
        }
        public FeedbackDbContext(string connectionStringName)
            : base(connectionStringName)
        {
        }
        public DbSet<FeedbackEntity> FeedbackData { get; set; }
    }
}