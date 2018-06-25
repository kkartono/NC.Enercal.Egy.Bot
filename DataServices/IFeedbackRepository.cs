using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QnABot.DataServices
{
    interface IFeedbackRepository
    {
        Task SaveAsync(FeedbackEntity entity);
    }
}
