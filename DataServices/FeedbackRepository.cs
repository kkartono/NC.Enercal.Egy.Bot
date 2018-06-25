using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace QnABot.DataServices
{
    public sealed class FeedbackRepository : IFeedbackRepository
    {
        private string _dbConString { get; set; }

        public FeedbackRepository()
        {
            _dbConString = ConfigurationManager.ConnectionStrings["BotDataContextConnectionString"].ConnectionString;
        }

        public async Task SaveAsync(FeedbackEntity entity)
        {
            using (var context = new FeedbackDbContext(_dbConString))
            {
                context.FeedbackData.Add(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}