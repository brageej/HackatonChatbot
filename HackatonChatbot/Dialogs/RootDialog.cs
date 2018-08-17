using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Location.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;

namespace HackatonChatbot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            var message = activity.Text;
            if (message.ToLower().Contains("transaction"))
            {
                context.Call(new TransactionsDialog(), Resume);
            }
            else if (message.ToLower().Contains("hello"))
            {
                context.Call(new WelcomeDialog(), Resume);
            }
            else if (message.ToLower().Contains("stolen"))
            {
                context.Call(new StolenCreditCardDialog(), Resume);
            }
            else if (message.ToLower().Contains("kiev"))
            {
                context.Call(new KievDialog(), Resume);
            }
            else
            {
                await context.PostAsync("I didn't quite understand that!");
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task Resume(IDialogContext context, IAwaitable<object> result)
        {
            var r = await result;
            context.Done("Done");
        }
        private async Task TravelDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
        }
    }
}