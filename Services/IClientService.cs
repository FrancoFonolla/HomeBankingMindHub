using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using System.Diagnostics.Eventing.Reader;

namespace HomeBankingMindHub.Services
{
    public interface IClientService
    {
        
        List<ClientDTO> GetAllClients();
        responseClass<ClientDTO> GetClient(long id);
        responseClass<Client> GetClientEmail(string email);
        responseClass<Client> CreateClient(ClientCreateDTO client);
        responseClass<ClientDTO> GetCurrent(string email);
    }
    
}
