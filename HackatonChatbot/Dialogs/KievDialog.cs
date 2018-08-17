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
            await context.PostAsync("That sounds like a good idea!");

            await context.Typing();
            await context.PostAsync("When are you leaving?");

            context.Wait(DateFrom);
        }

        private async Task DateFrom(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var r = await result;

            await context.Typing();
            await context.PostAsync("When do you get back home?");

            context.Wait(DatesDone);
        }

        private async Task DatesDone(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var r = await result;

            await context.Typing();
            await context.PostAsync("What is your budget?");

            context.Wait(BudgetDone);
        }

        private async Task BudgetDone(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var r = await result;

            var card = new HeroCard(
                title: "Do you want me to send you updates on your spendings?",
                buttons: new List<CardAction>
                {
                    new CardAction
                    (
                        ActionTypes.PostBack,
                        title: "No, thank you",
                        value: "No, thank you"
                    ),
                    new CardAction
                    (
                        ActionTypes.PostBack,
                        title: "Yes, please",
                        value: "Yes, please"
                    )
                }
            );

            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = new List<Attachment> { card.ToAttachment() };

            await context.Typing();
            await context.PostAsync(reply);

            context.Wait(AfterSettingUpdates);
        }

        private async Task AfterSettingUpdates(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var r = await result;

            if (r.Text.ToLowerInvariant().Contains("yes"))
            {
                await context.Typing();
                await context.PostAsync("We will send you daily updates on your updates.");
            }

            await context.Typing();
            await context.PostAsync("Your budget has been set up.");

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