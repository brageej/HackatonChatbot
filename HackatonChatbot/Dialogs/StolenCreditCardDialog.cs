using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HackatonChatbot.Dialogs
{
    [Serializable]
    public class StolenCreditCardDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("I'm very sorry to hear that!");
            var heroCard = new HeroCard()
            {
                Title = "Which card has been stolen?",
                Buttons = new List<CardAction>
                {
                    new CardAction()
                    {
                        Type = ActionTypes.PostBack,
                        Title = "1234 1234 1234 1234",
                        Value = "1234 1234 1234 1234"
                    },
                    new CardAction()
                    {
                        Type =  ActionTypes.PostBack,
                        Title = "1726 1234 1234 1234",
                        Value = "1726 1234 1234 1234"
                    },
                }
            };
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = new List<Attachment> { heroCard.ToAttachment() };

            await context.Typing();
            await context.PostAsync(reply);

            context.Wait(Resume);
        }

        private async Task Resume(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var r = await result;
            await context.PostAsync("Do you want me to block the card: " + r.Text + "?");

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (!string.IsNullOrEmpty(message.Text))
            {
                if (message.Text.ToLower().Equals("yes"))
                {
                    await context.PostAsync("The card has been blocked");
                    context.Done(string.Empty);
                }
                else
                {
                    await context.PostAsync("Ok, your card will not be blocked");
                    context.Done(string.Empty);
                }
            }
        }
    }
}