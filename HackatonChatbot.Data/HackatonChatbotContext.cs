using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonChatbot.Data
{
    public class HackatonChatbotContext : DbContext
    {
        public DbSet<Transaction> Transaction { get; set; }

        public HackatonChatbotContext(string connectionString) : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public HackatonChatbotContext() : base("HackatonChatTestDB")
        {
        }
    }
}
