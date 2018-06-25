using Autofac;
using System.Web.Http;
using System.Configuration;
using System.Reflection;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using QnABot.SqlServiceServices;

namespace SimpleEchoBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //var builder = new ContainerBuilder();
            //builder.RegisterModule(new DialogModule());

            //builder.Register(c => new FeedbackDataStore("BotDatastoreContextConnectionString"))
            //    .As<IBotDataStore<BotData>>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();

            //builder.Update(Conversation.Container);
        }
    }
}
