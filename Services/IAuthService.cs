using HomeBankingMindHub.Models.DTOs;
using System.Security.Claims;

namespace HomeBankingMindHub.Services
{
    public interface IAuthService
    {
        ClaimsIdentity Login(ClientLoginDTO client);
        bool VerifyUser(ClientLoginDTO client);
    }
}
