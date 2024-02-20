using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Repositories.Implements;
using HomeBankingMindHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;


namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]

    [ApiController]

    public class ClientsController : ControllerBase

    {

        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ICardRepository _cardRepository;
        private readonly IClientService _clientService;
        private readonly IAccountService _accountService;
        private readonly ICardService _cardService;



        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository, ICardRepository cardRepository,IClientService clientService,IAccountService accountService,ICardService cardService)

        {

            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
            _clientService = clientService;
            _accountService = accountService;
            _cardService = cardService;
        }



        [HttpGet]

        public IActionResult Get()

        {

            try

            {

                return Ok(_clientService.GetAllClients());

            }

            catch (Exception ex)

            {

                return StatusCode(500, ex.Message);

            }

        }



        [HttpGet("{id}")]

        public IActionResult Get(long id)

        {

            try

            {

                var obj = _clientService.GetClient(id);

                if (obj.message!="ok")

                {

                    return StatusCode(403,obj.message);

                }
                return Ok(obj.Object);

            }
            

            catch (Exception ex)

            {
                
                return StatusCode(500, ex.Message);

            }

        }
        //[HttpPost]
        //public IActionResult Post([FromBody] ClientCreateDTO model)
        //{
        //    if(model.Email.IsNullOrEmpty() || model.FirstName.IsNullOrEmpty() || model.LastName.IsNullOrEmpty())
        //    {
        //        return BadRequest("Se requieren todos los campos");
        //    }
        //    try
        //    {
        //        var client= new Client();
        //        client.Email = model.Email;
        //            client.FirstName = model.FirstName;
        //        client.LastName = model.LastName;
        //        client.Password = "141516";
        //        _clientRepository.Save(client);
        //        return Created();


        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}
        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                var client = _clientService.GetCurrent(email);
                if (client.code != 200) return StatusCode(client.code, client.message);
                return Ok(client.Object);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post([FromBody] ClientCreateDTO client)
        {
            
            try
            {
                //validamos datos antes
                var newclient= _clientService.CreateClient(client);
                if(newclient.message != "ok")
                    return StatusCode(newclient.code,newclient.message);
               
                

                var newAccount = _accountService.CreateAccount(newclient.Object.Id);
                if(newAccount.code!=200) return StatusCode(newAccount.code,newAccount.message);
                
                

                return Created("",newclient.Object);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        
        [HttpPost("current/accounts")]
        public IActionResult Post()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                
                var client = _clientService.GetClientEmail(email);
                if (client.code != 200)
                {
                    return StatusCode(client.code,client.message);
                }
                long id = client.Object.Id;
                var newaccount= _accountService.CreateAccount(id);
                if(newaccount.code!=200)return StatusCode(newaccount.code,newaccount.message);
                return Created("", newaccount.Object);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("current/accounts")]
        public IActionResult GetAccounts()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
               var client = _clientService.GetClientEmail(email);
                if(client.code != 200)
                {
                    return StatusCode(client.code,client.message);
                }
                
                
                var accounts = _accountService.GetAllClientAccounts(client.Object.Id);
                return Ok(accounts);
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("current/accounts/{id}")]
        public IActionResult GetAccount(long id)
        {
            try
            {
                var account = _accountService.GetAccountById(id);
                if (account.code != 200)
                {
                    return StatusCode(account.code,account.message);
                }
               
                return Ok(account.Object);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }
        [HttpPost("current/cards")]
        public IActionResult Post(CardDTO cardFront)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
               
                var client = _clientService.GetClientEmail(email);
                if (client.code != 200)
                {
                    return StatusCode(client.code,client.message);
                }
                
                
                var newcard = _cardService.CreateCard(cardFront, client.Object);
                if(newcard.code != 200)
                {
                    return StatusCode(newcard.code,newcard.message);
                }
                return Created("", newcard.Object);
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("current/cards")]
        public IActionResult GetCard (){
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                var client=_clientService.GetClientEmail(email);
                if (client.code != 200)
                {
                    return StatusCode(client.code ,client.message);
                }
                var cardsDTO = _cardService.GetCards(client.Object.Cards);
                return Ok(cardsDTO);
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //[HttpPost("changepassword")]
        //public async Task<IActionResult> Post([FromBody] ChangePasswordDTO changePasswordDTO)
        //{
        //    try
        //    {
        //        if (String.IsNullOrEmpty(changePasswordDTO.CurrentPassword)||String.IsNullOrEmpty(changePasswordDTO.Email)||String.IsNullOrEmpty(changePasswordDTO.NewPassword))
        //            return StatusCode(403, "datos invalidos");
        //        Client Client = _clientRepository.FindByEmail(changePasswordDTO.Email);
        //        if (Client == null)
        //        {
        //            return StatusCode(404, "Cliente no encontrado");
        //        }
        //        if (Client.Password!=changePasswordDTO.CurrentPassword)
        //        {
        //            return StatusCode(400, "Contraseña no valida");
        //        }
        //        (byte[] salt, byte[] hash) = PasswordHasher.HashPassword(changePasswordDTO.NewPassword);
        //        Client.Salt = salt;
        //        Client.HashedPassword = hash;
        //        _clientRepository.UpdateClient(Client);


        //        return StatusCode(200, "Cambiado correctamente");
        //    }catch (Exception ex)
        //    {
        //        return StatusCode(500,ex.Message);
        //    }
        //}
    }
}
