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
                        Title = "VISA: 1234 1234 1234 1234",
                        Value = "VISA: 1234 1234 1234 1234"
                    },
                    new CardAction()
                    {
                        Type =  ActionTypes.PostBack,
                        Title = "Mastercard: 1726 1234 1234 1234",
                        Value = "Mastercard: 1726 1234 1234 1234"
                    },
                    new CardAction()
                    {
                        Type =  ActionTypes.PostBack,
                        Title = "None of those cards",
                        Value = "None"
                    },
                }
            };
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.List;
            reply.Attachments = new List<Attachment> { heroCard.ToAttachment() };

            await context.Typing();
            await context.PostAsync(reply);

            context.Wait(ConfirmBlocking);
        }

        private async Task ConfirmBlocking(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var r = await result;

            if (r.Text != "None")
            {
                await context.PostAsync("Do you want me to block the card: " + r.Text + "?");

                var heroCard = new HeroCard()
                {
                    Title = "Confirm?",
                    Buttons = new List<CardAction>
                {
                    new CardAction()
                    {
                        Type = ActionTypes.PostBack,
                        Title = "YES",
                        Value = "YES"
                    },
                    new CardAction()
                    {
                        Type =  ActionTypes.PostBack,
                        Title = "NO",
                        Value = "NO"
                    },
                    new CardAction()
                    {
                        Type =  ActionTypes.PostBack,
                        Title = "Back",
                        Value = "Back"
                    },
                }
                };

                var reply = context.MakeMessage();
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                reply.Attachments = new List<Attachment> { heroCard.ToAttachment() };

                //await context.Typing();
                await context.PostAsync(reply);

                context.Wait(MessageReceivedAsync);
            }
            else
            {
                await context.PostAsync("Your card cannot be closed using this tool.");
                context.Done(string.Empty);
            }
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (!string.IsNullOrEmpty(message.Text))
            {
                switch (message.Text.ToLower())
                {
                    case "yes":
                        await context.PostAsync("The card has been blocked. We will send you a new card right away.");
                        context.Done(string.Empty);
                        break;
                    case "no" :
                        await context.PostAsync("Ok, your card will not be blocked.");
                        context.Done(string.Empty);
                        break;
                    default:
                        //context.Wait(StartAsync);
                        context.Done(string.Empty);
                        break;
                }
            }
        }




    }
}