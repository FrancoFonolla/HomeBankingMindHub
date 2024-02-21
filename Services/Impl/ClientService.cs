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
        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        //metodo para traer todos los clientes
        public List<ClientDTO> GetAllClients()
        {
            var clients = _clientRepository.GetAllClients();

            var clientsDTO = new List<ClientDTO>();
            foreach (Client client in clients)

            {
                var newClientDTO = new ClientDTO(client.Id,
                    client.FirstName,
                    client.LastName,
                    client.Email,
                    client.Accounts.Select(ac => new AccountDTO(ac.Id
                    , ac.Number,
                    ac.Balance,
                    ac.CreationDate)).ToList(),
                    client.ClientLoans.Select(cl => new ClientLoanDTO(cl.Id,
                    cl.LoanId,
                    cl.Loan.Name,
                    cl.Amount,
                    int.Parse(cl.Payments))).ToList(),
                    client.Cards.Select(car => new CardDTO(car.Id,
                    car.CardHolder,
                    car.Type.ToString(),
                    car.Color.ToString(),
                    car.Number,
                    car.Cvv,
                    car.FromDate,
                    car.ThruDate)).ToList()
                    );
                clientsDTO.Add(newClientDTO);
            }
            return clientsDTO;
        }
        //metodo para traer un cliente por email
        public responseClass<Client> GetClientEmail(string email)
        {
            var client = _clientRepository.FindByEmail(email);
            //verificamos que el cliente exista
            if (client == null) return new responseClass<Client>(null, "No existe el cliente", 400);
            return new responseClass<Client>(client, "ok", 200);
        }
        //metodo para traer un cliente por id
        public responseClass<ClientDTO> GetClient(long id)
        {
            var client = _clientRepository.FindById(id);
            if (client == null) 
            {
                //verificamos que exista el cliente
                return new responseClass<ClientDTO>(null, "No existe el cliente", 400);
            }
            var clientDTO = new ClientDTO(client.Id,
                    client.FirstName,
                    client.LastName,
                    client.Email,
                    client.Accounts.Select(ac => new AccountDTO(ac.Id
                    , ac.Number,
                    ac.Balance,
                    ac.CreationDate)).ToList(),
                    client.ClientLoans.Select(cl => new ClientLoanDTO(cl.Id,
                    cl.LoanId,
                    cl.Loan.Name,
                    cl.Amount,
                    int.Parse(cl.Payments))).ToList(),
                    client.Cards.Select(car => new CardDTO(car.Id,
                    car.CardHolder,
                    car.Type.ToString(),
                    car.Color.ToString(),
                    car.Number,
                    car.Cvv,
                    car.FromDate,
                    car.ThruDate)).ToList()
                    );
            return new responseClass<ClientDTO>(clientDTO,"created", 200);

        }
        //metodo para traer el cliente autenticado
        public responseClass<ClientDTO> GetCurrent(string email)
        {
            if (email == string.Empty)
            {
                //verificamos que el email no este vacio
                return new responseClass<ClientDTO>(null,"Email vacio",400);
            }
            Client client = _clientRepository.FindByEmail(email);
            if (client == null)
            {
                //verificamos que el cliente exista
                return new responseClass<ClientDTO>(null,"No existe el cliente",404);
            }
            var clientDTO = new ClientDTO(client.Id,
                    client.FirstName,
                    client.LastName,
                    client.Email,
                    client.Accounts.Select(ac => new AccountDTO(ac.Id
                    , ac.Number,
                    ac.Balance,
                    ac.CreationDate)).ToList(),
                    client.ClientLoans.Select(cl => new ClientLoanDTO(cl.Id,
                    cl.LoanId,
                    cl.Loan.Name,
                    cl.Amount,
                    int.Parse(cl.Payments))).ToList(),
                    client.Cards.Select(car => new CardDTO(car.Id,
                    car.CardHolder,
                    car.Type.ToString(),
                    car.Color.ToString(),
                    car.Number,
                    car.Cvv,
                    car.FromDate,
                    car.ThruDate)).ToList()
                    );
            
            return new responseClass<ClientDTO>(clientDTO, "ok", 200);
        }
        //metodo para creacion de cliente
        public responseClass<Client> CreateClient(ClientCreateDTO client)
        {
            if (Utiles.IsValidEmail(client.Email))
            {
                //verificamos que sea valida la direccion de correo
                return new responseClass<Client>(null, "La direccion de correo no es valida", 400);
            }
            //verificamos que los datos no sean nulos
            if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || String.IsNullOrEmpty(client.FirstName) || String.IsNullOrEmpty(client.LastName))
                return new responseClass<Client>(null,"Datos invalidos", 400);
            Client user = _clientRepository.FindByEmail(client.Email);
            if (user != null) {
                //verificamos que el email no este usado por otro cliente
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
                //verificamos que sea alla creado la cuenta en la base de datos
                return new responseClass<Client>(null, "Error al crear la cuenta", 500);
            }





            return new responseClass<Client>(clientnew,"ok", 200);
        }


    }
}
