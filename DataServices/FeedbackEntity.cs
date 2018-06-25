using Microsoft.Bot.Connector;

namespace QnABot.DataServices
{
    public class FeedbackEntity
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public string Feedback { get; set; }
    }
}