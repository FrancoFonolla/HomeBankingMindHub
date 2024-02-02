namespace HomeBankingMindHub.Models
{
    public class DbInitializer
    {
        public static void Initilize(HomeBankingContext context)
        {
            if(context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client{ Email="francofonolla@gmail.com", FirstName="Franco",LastName="Fonolla",Password="141516",
                    }
                };
                context.Clients.AddRange(clients);
                //guardamos
                context.SaveChanges();
            }
        }
    }
}
