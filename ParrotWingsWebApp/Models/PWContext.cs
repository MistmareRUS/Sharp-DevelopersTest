using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ParrotWingsWebApp.Models
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
            context.Users.Add(new User { Name = "Вася", EMail = "User_1@mail.com", Password = "Admin", Balance = 1000.0 });
            context.Users.Add(new User { Name = "Петя", EMail = "User_2@mail.com", Password = "Admin", Balance = 200.0 });
            context.Users.Add(new User { Name = "Коля", EMail = "User_3@mail.com", Password = "Admin", Balance = 200.0 });
            context.Users.Add(new User { Name = "Иван Иваныч", EMail = "User_4@mail.com", Password = "Admin", Balance = 500.0 });
            context.Users.Add(new User { Name = "Шурик", EMail = "User_5@mail.com", Password = "Admin", Balance = 600.0 });
            context.Transactions.Add(new Transaction { SenderId = 2, ReceiverId=1, Date = DateTime.Now.AddHours(-2), Sum = 300.0, SenderBalance= 200.0, ReceiverBalance=800.0 });
            context.Transactions.Add(new Transaction { SenderId =3 , ReceiverId = 1, Date = DateTime.Now.AddMinutes(-15), Sum = 300.0, SenderBalance = 200.0, ReceiverBalance = 1100.0 });
            context.Transactions.Add(new Transaction { SenderId = 1, ReceiverId = 5, Date = DateTime.Now.AddHours(-1).AddMinutes(-16), Sum = 100.0, SenderBalance = 1000.0, ReceiverBalance = 600.0 });
            context.Transactions.Add(new Transaction { SenderId = 1, ReceiverId = 4, Date = DateTime.Now.AddMinutes(-54), Sum = 400.0, SenderBalance = 600.0, ReceiverBalance = 900.0 });
            base.Seed(context);
        }
    }
}