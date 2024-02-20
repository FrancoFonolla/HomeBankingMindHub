using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Repositories.Implements;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace HomeBankingMindHub.Services.Impl
{
    public class ClientService : IClientService 
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ICardRepository _cardRepository;
        public ClientService(IClientRepository clientRepository, IAccountRepository accountRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
        }

        public List<ClientDTO> GetAllClients()
        {
            var clients = _clientRepository.GetAllClients();

            var clientsDTO = new List<ClientDTO>();
            foreach (Client client in clients)

            {

                var newClientDTO = new ClientDTO

                {

                    Id = client.Id,

                    Email = client.Email,

                    FirstName = client.FirstName,

                    LastName = client.LastName,

                    Accounts = client.Accounts.Select(ac => new AccountDTO

                    {

                        Id = ac.Id,

                        Balance = ac.Balance,

                        CreationDate = ac.CreationDate,

                        Number = ac.Number

                    }).ToList(),
                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = nameof(c.Color),
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = nameof(c.Type),
                    }).ToList()


                };



                clientsDTO.Add(newClientDTO);


            }
            return clientsDTO;
        }
        public responseClass<Client> GetClientEmail(string email)
        {
            var client = _clientRepository.FindByEmail(email);
            if (client == null) return new responseClass<Client>(null, "no existe el cliente", 400);
            return new responseClass<Client>(client, "ok", 200);
        }
        public responseClass<ClientDTO> GetClient(long id)
        {
            var client = _clientRepository.FindById(id);
            if (client == null) 
            {
                return new responseClass<ClientDTO>(null, "no existe el cliente", 400);
            
            }
            var clientDTO = new ClientDTO

            {

                Id = client.Id,

                Email = client.Email,

                FirstName = client.FirstName,

                LastName = client.LastName,

                Accounts = client.Accounts.Select(ac => new AccountDTO

                {

                    Id = ac.Id,

                    Balance = ac.Balance,

                    CreationDate = ac.CreationDate,

                    Number = ac.Number

                }).ToList(),
                Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                {
                    Id = cl.Id,
                    LoanId = cl.LoanId,
                    Name = cl.Loan.Name,
                    Amount = cl.Amount,
                    Payments = int.Parse(cl.Payments)
                }).ToList(),
                Cards = client.Cards.Select(c => new CardDTO
                {
                    Id = c.Id,
                    CardHolder = c.CardHolder,
                    Color = nameof(c.Color),
                    Cvv = c.Cvv,
                    FromDate = c.FromDate,
                    Number = c.Number,
                    ThruDate = c.ThruDate,
                    Type = nameof(c.Type),
                }).ToList()

            };
            return new responseClass<ClientDTO>(clientDTO,"created", 200);

        }
        public responseClass<ClientDTO> GetCurrent(string email)
        {
            if (email == string.Empty)
            {
                return new responseClass<ClientDTO>(null,"Email vacio",400);
            }
            Client client = _clientRepository.FindByEmail(email);
            if (client == null)
            {
                return new responseClass<ClientDTO>(null,"No existe el cliente",404);
            }
            var clientDTO = new ClientDTO
            {
                Id = client.Id,
                Email = client.Email,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Accounts = client.Accounts.Select(ac => new AccountDTO
                {
                    Id = ac.Id,
                    Balance = ac.Balance,
                    CreationDate = ac.CreationDate,
                    Number = ac.Number,
                }).ToList(),
                Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                {
                    Id = cl.Id,
                    LoanId = cl.LoanId,
                    Name = cl.Loan.Name,
                    Amount = cl.Amount,
                    Payments = int.Parse(cl.Payments)
                }).ToList(),
                Cards = client.Cards.Select(c => new CardDTO
                {
                    Id = c.Id,
                    CardHolder = c.CardHolder,
                    Color = c.Color.ToString(),
                    Cvv = c.Cvv,
                    FromDate = c.FromDate,
                    Number = c.Number,
                    ThruDate = c.ThruDate,
                    Type = c.Type.ToString()
                }).ToList()
            };
            return new responseClass<ClientDTO>(clientDTO, "ok", 200);
        }
        public responseClass<Client> CreateClient(ClientCreateDTO client)
        {
            if (Utiles.IsValidEmail(client.Email))
            {
                return new responseClass<Client>(null, "La direccion de correo no es valida", 400);
            }
            if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || String.IsNullOrEmpty(client.FirstName) || String.IsNullOrEmpty(client.LastName))
                return new responseClass<Client>(null,"Datos invalidos", 400);
            Client user = _clientRepository.FindByEmail(client.Email);
            if (user != null) {
                return new responseClass<Client>(null, "Email esta en uso",403);
            }
            var password = client.Password;
            (byte[] saltn, byte[] hashn) = PasswordHasher.HashPassword(password);
            Client newClient = new Client
            {
                Email = client.Email,
                Salt = saltn,
                HashedPassword = hashn,
                FirstName = client.FirstName,
                LastName = client.LastName,
            };
            _clientRepository.Save(newClient);
            Client clientnew = _clientRepository.FindByEmail(client.Email);
            if(clientnew == null) 
            {
                return new responseClass<Client>(null, "Error al crear la cuenta", 500);
            }





            return new responseClass<Client>(clientnew,"ok", 200);
        }


    }
}
