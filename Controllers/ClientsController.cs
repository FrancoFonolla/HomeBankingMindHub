using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Repositories.Implements;
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



        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository, ICardRepository cardRepository)

        {

            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
        }



        [HttpGet]

        public IActionResult Get()

        {

            try

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
                        Cards=client.Cards.Select(c=> new CardDTO
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





                return Ok(clientsDTO);

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

                var client = _clientRepository.FindById(id);

                if (client == null)

                {

                    return Forbid();

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
                    Credits = client.ClientLoans.Select(cl=> new ClientLoanDTO
                    {
                        Id= cl.Id,
                        LoanId = cl.LoanId,
                        Name= cl.Loan.Name,
                        Amount= cl.Amount,
                        Payments= int.Parse(cl.Payments)
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



                return Ok(clientDTO);

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
                if(email ==string.Empty)
                {
                    return Forbid();
                }
                Client client= _clientRepository.FindByEmail(email);
                if(client ==  null)
                {
                    return NotFound();
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
                return Ok(clientDTO);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post([FromBody] ClientCreateDTO client)
        {
            static bool IsValidEmail(string email)
            {
                // Expresión regular para verificar el formato de la dirección de correo electrónico
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

                // Verifica si la cadena coincide con el patrón
                return Regex.IsMatch(email, pattern);
            }
            try
            {
                //validamos datos antes
                if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || String.IsNullOrEmpty(client.FirstName) || String.IsNullOrEmpty(client.LastName))
                    return StatusCode(403, "datos invalidos");
                //buscamos si ya existe el usuario
                Client user = _clientRepository.FindByEmail(client.Email);
                if(!IsValidEmail(client.Email))
                {
                    return StatusCode(400, "La dirección de correo electronico no es valida");
                }
                if(user != null)
                {
                    return StatusCode(403, "Email esta en uso");
                }
                var password=client.Password;
                (byte[] saltn, byte[] hashn)= PasswordHasher.HashPassword(password);
                Client newClient = new Client
                {
                    Email = client.Email,
                    Salt=saltn,
                    HashedPassword=hashn,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                };
                _clientRepository.Save(newClient);
                Client clientnew = _clientRepository.FindByEmail(client.Email);
                if(clientnew == null)
                {
                    return StatusCode(500, "error al crear la cuenta");
                }
                if (clientnew.Accounts.Count >= 3)
                {
                    return Forbid();
                }

                var newAccount = new Account

                {
                    Balance = 0,
                    CreationDate = DateTime.Now,
                    Transactions = new List<Transaction>(),
                    ClientId = clientnew.Id,
                };
                _accountRepository.Save(newAccount);

                return Created("", newClient);
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
                if (email == string.Empty)
                {
                    return Forbid();
                }
                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return NotFound();
                }
                long id = client.Id;
                if (client.Accounts.Count >= 3)
                {
                    return Forbid();
                }

                var newAccount = new Account

                {
                    Balance = 0,
                    CreationDate = DateTime.Now,
                    Transactions = new List<Transaction>(),
                    ClientId = id,
                };
                _accountRepository.Save(newAccount);
                return Created("", newAccount);

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
                if (email == string.Empty)
                {
                    return Forbid();
                }
                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return NotFound();
                }
                long id = client.Id;
                IEnumerable<Card> cards= _cardRepository.GetCardsByClient(id);
                foreach (Card card in cards)
                {
                    if (cardFront.Type == card.Type.ToString())
                    {
                        if(cardFront.Color== card.Color.ToString())
                        {
                            return Forbid();
                        }
                    }
                }
                if (cardFront.Type == CardType.CREDIT.ToString())
                {

                }
                Card newcard = new Card
                {
                    Type = (cardFront.Type == CardType.DEBIT.ToString()) ? CardType.DEBIT : CardType.CREDIT,
                    CardHolder = client.FirstName + " " + client.LastName,
                    FromDate = DateTime.Now,
                    Color= (cardFront.Color== CardColor.TITANIUM.ToString()) ? CardColor.TITANIUM:(cardFront.Color==CardColor.SILVER.ToString()) ? CardColor.SILVER:CardColor.GOLD,
                    ThruDate = (cardFront.Type == CardType.DEBIT.ToString()) ? DateTime.Now.AddYears(4) : DateTime.Now.AddYears(5),
                    ClientId = client.Id,
                };
                _cardRepository.Save(newcard);
                return Created("", newcard);
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
                if (email == string.Empty)
                {
                    return Forbid();
                }
                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return NotFound();
                }
                var cards = _cardRepository.GetCardsByClient(client.Id);
                var cardsDTO=new List<CardDTO>();
                foreach (Card card in cards)
                {
                    CardDTO cardDTO = new CardDTO
                    {
                        CardHolder = card.CardHolder,
                        Number = card.Number,
                        Color = card.Color.ToString(),
                        Type = card.Type.ToString(),
                        Cvv = card.Cvv,
                        FromDate = card.FromDate,
                        ThruDate = card.ThruDate,


                    };
                    cardsDTO.Add(cardDTO);
                }
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
