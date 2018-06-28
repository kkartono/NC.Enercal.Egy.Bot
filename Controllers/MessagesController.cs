using System;
using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Web.Http.Description;
using System.Net.Http;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace Microsoft.Bot.Sample.QnABot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            // check if activity is of type message
            if (activity.GetActivityType() == ActivityTypes.Message)
            {
                //await Conversation.SendAsync(activity, () => new RootDialog());
                await Conversation.SendAsync(activity, () => new RootLuisDialog());
            }
            else
            {
                HandleSystemMessageAsync(activity);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private async Task<Activity> HandleSystemMessageAsync(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels

                IConversationUpdateActivity update = message;
                var client = new ConnectorClient(new Uri(message.ServiceUrl), new MicrosoftAppCredentials());
                if (update.MembersAdded != null && update.MembersAdded.Any())
                {
                    foreach (var newMember in update.MembersAdded)
                    {
                        if (newMember.Id != message.Recipient.Id)
                        {
                            var reply = message.CreateReply();

                            HeroCard welcomeCard = new HeroCard()
                            {
                                Title = "EGY Bot",
                                Text = $"Bonjour {newMember.Name}, je suis EGY votre assistant virtuel, posez-moi des questions comme par exemple: 'Consulter facture de janvier' ou bien 'Régler ma facture de juin'",
                                Images = new List<CardImage>()
                                {
                                    new CardImage() { Url = "https://cmkt-image-prd.global.ssl.fastly.net/0.1.0/ps/3410318/1162/776/m1/fpnw/wm0/1-.jpg?1508027464&s=423375e6a847fd2714d4d8a97c708206" }
                                },
                            };
                            reply.Attachments.Add(welcomeCard.ToAttachment());
                            //reply.Text = $"Welcome {newMember.Name}!";
                            client.Conversations.ReplyToActivityAsync(reply);
                        }
                    }
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}