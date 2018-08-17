using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonChatbot.Data
{

    public sealed class SingletonHackatonDbContext
    {
        private static readonly SingletonHackatonDbContext instance =
            new SingletonHackatonDbContext(new HackatonChatbotContext("HackatonChatTestDB"));

        private readonly HackatonChatbotContext _context;

        private SingletonHackatonDbContext(HackatonChatbotContext context)
        {
            _context = context;
            if (_context.Database.CreateIfNotExists())
                new DbPopulator().Seed(_context);
        }

        public static SingletonHackatonDbContext Instance
        {
            get { return instance; }
        }

        public HackatonChatbotContext DbContext
        {
            get { return _context; }
        }
    }


}
