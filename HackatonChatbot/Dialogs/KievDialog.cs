using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace HackatonChatbot.Dialogs
{
    [Serializable]
    public class KievDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.Typing();
            await context.PostAsync("Kiev sounds great!");

            var card = new HeroCard(
                title: "What can I help you with?",
                buttons: new List<CardAction>
                {
                    new CardAction
                    (
                        ActionTypes.PostBack,
                        title: "Exchange rates",
                        value: "Exchange rates"
                    ),
                    new CardAction
                    (
                        ActionTypes.PostBack,
                        title: "Budget for the trip",
                        value: "Budget for the trip"
                    ),
                    new CardAction
                    (
                        ActionTypes.PostBack,
                        title: "Need a credit card",
                        value: "Need a credit card"
                    )
                }
            );

            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = new List<Attachment> { card.ToAttachment() };

            await context.Typing();
            await context.PostAsync(reply);

            context.Wait(Resume);
        }

        private async Task Resume(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var r = await result;

            switch (r.Text)
            {
                case "Exchange rates":
                    await ExchangeRates(context);
                    break;
                case "Budget for the trip":
                    await Budget(context);
                    break;
                case "Need a credit card":
                    await NeedACreditCard(context);
                    break;
            }
        }

        private async Task NeedACreditCard(IDialogContext context)
        {
            await context.Typing();
            await context.PostAsync("Unfotunately our developers did not implement this feature...");
            context.Done(string.Empty);
        }

        private async Task Budget(IDialogContext context)
        {
            await context.Typing();
            await context.PostAsync("Unfotunately our developers did not implement this feature 2...");
            context.Done(string.Empty);
        }

        private async Task ExchangeRates(IDialogContext context)
        {
            await context.Typing();
            await context.PostAsync("In Ukraine they use Hryvnja​");
            await context.PostAsync("100 UAH = 385,8 ISK");

            var card = new HeroCard(
                title: "Can I help you with anything else?",
                buttons: new List<CardAction>
                {
                    new CardAction
                    (
                        ActionTypes.PostBack,
                        title: "Budget for the trip",
                        value: "Budget for the trip"
                    ),
                    new CardAction
                    (
                        ActionTypes.PostBack,
                        title: "Need a credit card",
                        value: "Need a credit card"
                    )
                }
            );

            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = new List<Attachment> { card.ToAttachment() };

            await context.Typing();
            await context.PostAsync(reply);

            context.Wait(Resume);
        }
    }
}