using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace HackatonChatbot.Data
{
    public class DbPopulator
    {
        public void Seed(HackatonChatbotContext context)
        {
            context.SaveChanges();
        }


        private static void Transactions(HackatonChatbotContext context)
        {
            context.Transaction.AddOrUpdate(
                new Transaction()
                {
                    Id = 1
                });
        }
    }
}