using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Repositories.Implements;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace HomeBankingMindHub.Services.Impl
{
    public class AuthService : IAuthService
    {
        public IClientRepository _clientRepository;
        public AuthService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public ClaimsIdentity Login(ClientLoginDTO client)
        {
            Client user = _clientRepository.FindByEmail(client.Email);


            var claims = new List<Claim>
                {
                    new Claim("Client",user.Email),
                };
            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            return claimsIdentity;

        }
        public bool VerifyUser(ClientLoginDTO client)
        {
            Client user = _clientRepository.FindByEmail(client.Email);
            return user == null || !PasswordHasher.VerifyPassword(client.Password, user.HashedPassword, user.Salt);
        }
    }
}
