using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace HackatonChatbot.Dialogs
{
    [Serializable]
    public class KievDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("So, you are going to Kiev...");
            context.Done(string.Empty);
        }
    }
}