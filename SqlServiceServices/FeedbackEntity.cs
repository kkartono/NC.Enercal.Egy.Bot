using Microsoft.Bot.Connector;

namespace QnABot.SqlServiceServices
{
    public class FeedbackEntity : BotData
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public string Feedback { get; set; }
    }
}