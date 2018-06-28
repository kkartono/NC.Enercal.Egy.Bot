using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Bot.Sample.QnABot
{
    [LuisModel("ce9fa0fa-56ae-4c64-9c4a-58278d34f5b4", "6589625b9e81496492ad77d50eaa5fc7")]
    [Serializable]
    public class RootLuisDialog : LuisDialog<object>
    {
        private const string EntityMonthName = "Mois";
        private const string EntityNumber = "numero";

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Désolé je ne comprend pas votre question '{result.Query}'. Saisir 'Aide' si vous avez besoin d'assistance.";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Aide")]
        public async Task Aide(IDialogContext context, LuisResult result)
        {
            var resultMessage = context.MakeMessage();

            //TODO: Mettre image the EGY dans la card
            HeroCard welcomeCard = new HeroCard()
            {
                Title = "EGY Bot",
                Text = "Bonjour, je suis EGY votre assistant virtuel, posez-moi des questions comme par exemple: " +
                "'quel est le montant de ma dernière facture?' ou bien 'montre-moi mes trois dernières factures?'",
                Images = new List<CardImage>()
                {
                    new CardImage() { Url = "https://cmkt-image-prd.global.ssl.fastly.net/0.1.0/ps/3410318/1162/776/m1/fpnw/wm0/1-.jpg?1508027464&s=423375e6a847fd2714d4d8a97c708206" }
                },
            };

            resultMessage.Attachments.Add(welcomeCard.ToAttachment());
            await context.PostAsync(resultMessage);
        }

        [LuisIntent("ConsulterFactures")]
        public async Task ConsulterFacture(IDialogContext context, LuisResult result)
        {
            var resultMessage = context.MakeMessage();
            var userQuestion = (context.Activity as Activity).Text;
            EntityRecommendation moisEntityRecommendation;
            EntityRecommendation numberEntityRecommendation;

            if (result.TryFindEntity(EntityMonthName, out moisEntityRecommendation))
            {
                await context.PostAsync($"Voici votre factures du mois de {moisEntityRecommendation.Entity}. Cliquez sur l'image pour consulter la facture en ligne");

                HeroCard card = new HeroCard()
                {
                    Title = $"Facture du mois de {moisEntityRecommendation.Entity}",
                    Text = "Rien à payer",
                    Images = new List<CardImage>()
                {
                    new CardImage() { Url = "https://www.dolistore.com/2334-thickbox_default/Moderna---Facture-avec-le-logo-du-projet.jpg" }
                },
                    Tap = new CardAction(ActionTypes.OpenUrl, "Consulter la facture en ligne", value: "https://www.enercal.nc/user/login")
                };

                resultMessage.Attachments.Add(card.ToAttachment());
            }
            else if (result.TryFindEntity(EntityNumber, out numberEntityRecommendation))
            {
                //TODO: Afficher le nombre de card demandé
                await context.PostAsync($"Voici vos {numberEntityRecommendation.Entity} derières factures. Cliquez sur l'image pour consulter la facture en ligne");
            }

            else
            {
                await context.PostAsync("Voici vos trois dernières factures. Cliquez sur l'image pour consulter la facture en ligne");

                //TODO: Récupérer les factures from database

                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();

                HeroCard card1 = new HeroCard()
                {
                    Title = "Facture du 05/03/2018 au 04/04/2018",
                    Text = "Rien à payer",
                    Images = new List<CardImage>()
                {
                    new CardImage() { Url = "https://www.dolistore.com/2334-thickbox_default/Moderna---Facture-avec-le-logo-du-projet.jpg" }
                },
                    Tap = new CardAction(ActionTypes.OpenUrl, "Consulter la facture en ligne", value: "https://www.enercal.nc/user/login")
                };

                HeroCard card2 = new HeroCard()
                {
                    Title = "Facture du 05/04/2018 au 04/05/2018",
                    Text = "Rien à payer",
                    Images = new List<CardImage>()
                {
                    new CardImage() { Url = "https://www.dolistore.com/2334-thickbox_default/Moderna---Facture-avec-le-logo-du-projet.jpg" }
                },
                    Tap = new CardAction(ActionTypes.OpenUrl, "Consulter la facture en ligne", value: "https://www.enercal.nc/user/login")
                };

                HeroCard card3 = new HeroCard()
                {
                    Title = "Facture du 05/05/2018 au 04/06/2018",
                    Text = "Rien à payer",
                    Images = new List<CardImage>()
                {
                    new CardImage() { Url = "https://www.dolistore.com/2334-thickbox_default/Moderna---Facture-avec-le-logo-du-projet.jpg" }
                },
                    Tap = new CardAction(ActionTypes.OpenUrl, "Consulter la facture en ligne", value: "https://www.enercal.nc/user/login")
                };

                resultMessage.Attachments.Add(card1.ToAttachment());
                resultMessage.Attachments.Add(card2.ToAttachment());
                resultMessage.Attachments.Add(card3.ToAttachment());
            }

            await context.PostAsync(resultMessage);

            context.Call(new FeedbackDialog(userQuestion), this.MessageReceived);
        }

        [LuisIntent("Fin")]
        public async Task FinConversation(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("A votre service! N'hésitez pas à me consulter pour toute autre question");
        }

        [LuisIntent("ReglerFacture")]
        public async Task ReglerFacture(IDialogContext context, LuisResult result)
        {
            EntityRecommendation moisEntityRecommendation;
            var resultMessage = context.MakeMessage();

            if (result.TryFindEntity(EntityMonthName, out moisEntityRecommendation))
            {
                await context.PostAsync("Est-ce bien cette facture que vous voulez régulariser?");

                HeroCard card = new HeroCard()
                {
                    Title = $"Facture du mois de {moisEntityRecommendation.Entity}",
                    Text = "Total à payer: 12000.00 CFP",
                    Images = new List<CardImage>()
                {
                    new CardImage() { Url = "https://www.dolistore.com/2334-thickbox_default/Moderna---Facture-avec-le-logo-du-projet.jpg" }
                },
                    Tap = new CardAction(ActionTypes.OpenUrl, "Consulter la facture en ligne", value: "https://www.enercal.nc/user/login")
                };

                resultMessage.Attachments.Add(card.ToAttachment());
                await context.PostAsync(resultMessage);

                context.Wait(MessageReceivedAsync);
            }
            else
            {
                await context.PostAsync("Félicitation! Vous n'avez aucun réglement à effectuer, toutes vos factures sont à jour");
            }
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var userAnswer = await result;
            var resultMessage = context.MakeMessage();

            if (userAnswer.Text.ToLower().Equals("oui"))
            {
                HeroCard card = new HeroCard()
                {
                    Text = "Cliquez sur le bouton ci-dessous pour régler votre facture en ligne",
                    Buttons = new List<CardAction>()
                        {
                            new CardAction(ActionTypes.OpenUrl, "Régler la facture en ligne", value: "https://www.enercal.nc/user/login"),
                        }
                };

                resultMessage.Attachments.Add(card.ToAttachment());
                await context.PostAsync(resultMessage);
                context.Wait(this.MessageReceived);
            }
            else
            {
                await context.PostAsync("Je n'ai peut-être pas très bien compris votre requête, pourriez-vous la reformuler ou bien la préciser");
                context.Wait(this.MessageReceived);
            }
        }
    }
}