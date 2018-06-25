using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Threading.Tasks;
using QnABot.DataServices;
using System.Configuration;

namespace Microsoft.Bot.Sample.QnABot
{
    [Serializable]
    public class FeedbackDialog : IDialog<IMessageActivity>
    {
        private string qnaURL;
        private string userQuestion;

        private IFeedbackRepository _repository;

        public FeedbackDialog(string url, string question)
        {
            string conn = ConfigurationManager.ConnectionStrings["BotDataContextConnectionString"].ConnectionString;
            _repository = new FeedbackRepository(conn);

            // keep track of data associated with feedback
            this.qnaURL = url;
            this.userQuestion = question;
        }

        public async Task StartAsync(IDialogContext context)
        {
            var feedback = ((Activity)context.Activity).CreateReply("Avez-vous trouvé ce que vous cherchiez");

            feedback.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){ Title = "👍", Type=ActionTypes.PostBack, Value=$"yes-positive-feedback" },
                    new CardAction(){ Title = "👎", Type=ActionTypes.PostBack, Value=$"no-negative-feedback" }
                }
            };

            await context.PostAsync(feedback);

            context.Wait(this.MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var userFeedback = await result;

            switch (userFeedback.Text.ToLower())
            {
                case "yes-positive-feedback":
                    await _repository.SaveAsync(new FeedbackEntity()
                    {
                        Question = userQuestion,
                        Feedback = "satisfied"
                    });
                    break;

                case "no-negative-feedback":
                    await _repository.SaveAsync(new FeedbackEntity()
                    {
                        Question = userQuestion,
                        Feedback = "not satisfied"
                    });
                    break;

                default:
                    context.Done(userFeedback);
                    return;
            }

            await context.PostAsync("Merci de votre retour!");
            context.Done<IMessageActivity>(null);
        }
    }
}



