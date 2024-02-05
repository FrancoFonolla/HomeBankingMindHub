namespace HomeBankingMindHub.Models
{
    public class DbInitializer
    {
        public static void Initilize(HomeBankingContext context)
        {
            if(!context.Account.Any())
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

