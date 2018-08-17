using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackatonChatbot
{
    public static class BudgetData
    {
        public static DateTime From { get; set; } = DateTime.Now.AddDays(1).Date;
        public static DateTime To { get; set; } = DateTime.Now.AddDays(4).Date;
        public static string Amount { get; set; } = "4000 ISK";
    }
}