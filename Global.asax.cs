using Autofac;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using QnABot.DataServices;

namespace SimpleEchoBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            /*var builder = new ContainerBuilder();

            builder.Register(c => new FeedbackRepository())
                .As<IFeedbackRepository>()
                .SingleInstance();*/

            //builder.RegisterModule(new DialogModule());

            //builder.Register(c => new FeedbackDataStore("BotDatastoreContextConnectionString"))
            //    .As<IBotDataStore<BotData>>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();

            //builder.Update(Conversation.Container);
        }
    }
}
