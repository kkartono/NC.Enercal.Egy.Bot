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
using Microsoft.Bot.Builder.Azure;

namespace Microsoft.Bot.Sample.QnABot
{

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            /* Wait until the first message is received from the conversation and call MessageReceviedAsync 
            *  to process that message. */
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            /* When MessageReceivedAsync is called, it's passed an IAwaitable<IMessageActivity>. To get the message,
             *  await the result. */
            var message = await result;

            var qnaAuthKey = ConfigurationManager.AppSettings["QnAAuthKey"];
            var qnaKBId = ConfigurationManager.AppSettings["QnAKnowledgebaseId"];
            var endpointHostName = ConfigurationManager.AppSettings["QnAEndpointHostName"];

            // QnA Subscription Key and KnowledgeBase Id null verification
            if (!string.IsNullOrEmpty(qnaAuthKey) && !string.IsNullOrEmpty(qnaKBId))
            {
                // Forward to the appropriate Dialog based on whether the endpoint hostname is present
                if (string.IsNullOrEmpty(endpointHostName))
                    await context.Forward(new QnaDialog(), AfterAnswerAsync, message, CancellationToken.None);
                else
                    await context.Forward(new QnaDialog(), AfterAnswerAsync, message, CancellationToken.None);
            }
            else
            {
                await context.PostAsync("Please set QnAKnowledgebaseId, QnAAuthKey and QnAEndpointHostName (if applicable) in App Settings. Learn how to get them at https://aka.ms/qnaabssetup.");
            }
        }

        private async Task AfterAnswerAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            // wait for the next user message
            context.Wait(MessageReceivedAsync);
        }

        public static string GetSetting(string key)
        {
            var value = Utils.GetAppSetting(key);
            if (String.IsNullOrEmpty(value) && key == "QnAAuthKey")
            {
                value = Utils.GetAppSetting("QnASubscriptionKey"); // QnASubscriptionKey for backward compatibility with QnAMaker (Preview)
            }
            return value;
        }
    }

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

        protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            //// get the URL
            //var answer = result.Answers.First().Answer;
            //string[] qnaAnswerData = answer.Split(';');
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