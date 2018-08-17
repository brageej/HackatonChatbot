using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace HackatonChatbot.Dialogs
{
    [Serializable]
    public class TransactionsDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Here are your transactions");
            context.Done(string.Empty);
        }
    }
}