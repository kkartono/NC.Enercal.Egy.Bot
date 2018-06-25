using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;

namespace QnABot.SqlServiceServices
{
    public class FeedbackDataStore : IBotDataStore<BotData>
    {
        string _connectionStringName { get; set; }

        public FeedbackDataStore(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        public async Task<BotData> LoadAsync(IAddress key, BotStoreType botStoreType, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync(IAddress key, BotStoreType botStoreType, BotData data, CancellationToken cancellationToken)
        {
            using (var context = new FeedbackDatastoreDbContext(_connectionStringName))
            {
                var d = data as FeedbackEntity;
                context.FeedbackData.Add(d);
                context.SaveChanges();
            }
        }

        public async Task<bool> FlushAsync(IAddress key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}