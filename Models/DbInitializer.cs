namespace HomeBankingMindHub.Models
{
    public class DbInitializer
    {
        public static void Initilize(HomeBankingContext context)
        {
            if (!context.Loans.Any())
            {
                //crearemos 3 prestamos Hipotecario, Personal y Automotriz
                var loans = new Loan[]
                {
                    new Loan { Name = "Hipotecario", MaxAmount = 500000, Payments = "12,24,36,48,60" },
                    new Loan { Name = "Personal", MaxAmount = 100000, Payments = "6,12,24" },
                    new Loan { Name = "Automotriz", MaxAmount = 300000, Payments = "6,12,24,36" },
                };

                foreach (Loan loan in loans)
                {
                    context.Loans.Add(loan);
                }

                context.SaveChanges();

                //ahora agregaremos los clientloan (Prestamos del cliente)
                //usaremos al único cliente que tenemos y le agregaremos un préstamo de cada item
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (client1 != null)
                {
                    //ahora usaremos los 3 tipos de prestamos
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 400000,
                            ClientId = client1.Id,
                            LoanId = loan1.Id,
                            Payments = "60"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }

                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 50000,
                            ClientId = client1.Id,
                            LoanId = loan2.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }

                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 100000,
                            ClientId = client1.Id,
                            LoanId = loan3.Id,
                            Payments = "24"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }

                    //guardamos todos los prestamos
                    context.SaveChanges();

                }

            }
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

