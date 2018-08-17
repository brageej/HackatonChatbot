using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HackatonChatbot.Dialogs
{
    public static class DialogExtensions
    {
        public static async Task Typing(this IDialogContext context)
        {
            var msg = context.MakeMessage();
            msg.Text = string.Empty;
            msg.Type = ActivityTypes.Typing;
            await context.PostAsync(msg);
            await Task.Delay(800);
        }
    }
}
