using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace Microsoft.Bot.Sample.QnABot
{
    [Serializable]
    public class QnaDialog : QnAMakerDialog
    {
        public QnaDialog() : base(new QnAMakerService(new QnAMakerAttribute
            (ConfigurationManager.AppSettings["QnAAuthKey"],
            ConfigurationManager.AppSettings["QnAKnowledgebaseId"],
            "Désolé, je n'ai pas trouvé de réponse à votre question :(", 0.5, 1,
            ConfigurationManager.AppSettings["QnAEndpointHostName"])))
        {

        }

        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            // answer is a string
            var answer = result.Answers.First().Answer;

            Activity reply = ((Activity)context.Activity).CreateReply();

            string[] qnaAnswerData = answer.Split(';');
            //int dataSize = qnaAnswerData.Length;

            //string title = qnaAnswerData[0];
            string title = "Title";
            string description = qnaAnswerData[0];
            string imageURL = "https://www.w3schools.com/w3css/img_lights.jpg";
            string url = "https://www.w3schools.com/w3css/img_lights.jpg";
            //string url = qnaAnswerData[2];
            //string imageURL = qnaAnswerData[3];

            HeroCard card = new HeroCard
            {
                Title = title,
                Subtitle = description,
            };

            //card.Buttons = new List<CardAction>
            //{
            //    new CardAction(ActionTypes.OpenUrl, "Learn More", value: url)
            //};

            card.Images = new List<CardImage>
            {
                new CardImage( url = imageURL)
            };

            reply.Attachments.Add(card.ToAttachment());


            await context.PostAsync(reply);
        }

        protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            //// get the URL
            var answer = result.Answers.First().Answer;
            string[] qnaAnswerData = answer.Split(';');
            //string qnaURL = qnaAnswerData[2];
            string qnaURL = "";

            //// pass user's question
            var userQuestion = (context.Activity as Activity).Text;

            context.Call(new FeedbackDialog(qnaURL, userQuestion), ResumeAfterFeedback);
        }

        private async Task ResumeAfterFeedback(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (await result != null)
            {
                await MessageReceivedAsync(context, result);
            }
            else
            {
                context.Done<IMessageActivity>(null);
            }
        }

    }
}