namespace HomeBankingMindHub.Models
{
    public class DbInitializer
    {
        public static void Initilize(HomeBankingContext context)
        {
            if (!context.Account.Any())
            {
                var accountFranco = context.Clients.FirstOrDefault(c => c.Email == "francofonolla@gmail.com");
                if (accountFranco != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountFranco.Id, CreationDate = DateTime.Now, Number = "FRA001", Balance = 0 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Account.Add(account);
                    }
                    context.SaveChanges();

                }
            }
            if (!context.Transaction.Any())

            {

                var account1 = context.Account.FirstOrDefault(c => c.Number == "FRA001");

                if (account1 != null)

                {

                    var transactions = new Transaction[]

                    {

                        new Transaction { AccountId= account1.Id, Amount = 15000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia reccibida", Type = TransactionType.CREDIT.ToString() },

                        new Transaction { AccountId= account1.Id, Amount = -2500, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT.ToString() },

                        new Transaction { AccountId= account1.Id, Amount = -7000, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda xxxx", Type = TransactionType.DEBIT.ToString() },

                    };
                    double newBalance = 0;
                    foreach (Transaction transaction in transactions)

                    {
                        newBalance += transaction.Amount;

                        context.Transaction.Add(transaction);

                    }
                    account1.Balance = newBalance;

                    context.SaveChanges();



                }

            }
            if (!context.Account.Any())
            {
                var accountVictor= context.Clients.FirstOrDefault(c=> c.Email=="vcoronado@gmail.com");
                if(accountVictor!=null)
                {
                    var accounts = new Account[]
                    {
                        new Account
                        {
                            ClientId=accountVictor.Id, CreationDate=DateTime.Now,Number=string.Empty,Balance=0
                        }
                    };
                    foreach(Account account in accounts)
                    {
                        context.Account.Add(account);
                    }
                    context.SaveChanges();
                }
            }
           
                
            }
        }
    }

