using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Repositories.Implements;

namespace HomeBankingMindHub.Services.Impl
{
    public class CardService : ICardService
    {
        private ICardRepository _cardRepository;
        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }
        //metodo para creacion de una tarjeta
        public responseClass<Card> CreateCard(CardDTO cardFront, Client client)
        {
            IEnumerable<Card> cards = _cardRepository.GetCardsByClient(client.Id);
            //verificamos que el cliente no tenga una tarjeta del mismo color y tipo
            foreach (Card card1 in cards)
            {
                if (cardFront.Type == card1.Type.ToString())
                    if (cardFront.Color == card1.Color.ToString())
                        return new responseClass<Card>(null, "Ya tiene una tarjeta de este tipo", 400);
            }
            Card card = new Card()
            {
                Type = cardFront.Type == CardType.DEBIT.ToString() ? CardType.DEBIT : CardType.CREDIT,
                CardHolder = client.FirstName + " " + client.LastName,
                FromDate = DateTime.Now,
                Color = cardFront.Color == CardColor.TITANIUM.ToString() ? CardColor.TITANIUM : cardFront.Color == CardColor.SILVER.ToString() ? CardColor.SILVER : CardColor.GOLD,
                ThruDate = cardFront.Type == CardType.DEBIT.ToString() ? DateTime.Now.AddYears(4) : DateTime.Now.AddYears(5),
                ClientId = client.Id,
            };
            _cardRepository.Save(card);
            return new responseClass<Card>(card, "ok", 200);
        }
        //metodo para pasar tarjetas al front
        public List<CardDTO> GetCards(IEnumerable<Card> cards)
        {
            var cardsDTO = new List<CardDTO>();
            foreach (Card card in cards)
            {
                CardDTO cardDTO = new CardDTO(card.Id,
                    card.CardHolder,
                    card.Type.ToString(),
                    card.Color.ToString(),
                    card.Number, card.Cvv,
                    card.FromDate, card.ThruDate);
                cardsDTO.Add(cardDTO);
            }
            return cardsDTO;
        }
    }
}