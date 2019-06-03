using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ServerApp.Models
{
    public class PWContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        
    }
    public class PWDbInitializer : DropCreateDatabaseAlways<PWContext>
    {
        protected override void Seed(PWContext context)
        {
            context.Users.Add(new User { Name = "Admin", EMail = "Admin@mail.com", Password = "Admin", Balance = 1000.0 });
            context.Users.Add(new User { Name = "Admin_2", EMail = "Admin_2@mail.com", Password = "Admin", Balance = 800.0 });
            context.Transactions.Add(new Transaction { UserId = 0, Date = DateTime.Now, Sum = 300.0, Balance = 1000.0 });
            base.Seed(context);
        }
    }
}