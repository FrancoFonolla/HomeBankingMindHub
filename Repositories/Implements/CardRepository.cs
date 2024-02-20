﻿using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.LibraryModel;

namespace HomeBankingMindHub.Repositories.Implements
{
    
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }

        public void Save(Card card)
        {
            card.Cvv=Utiles.GenerateRandomCardCvv();
            bool condition = true;
            string number= string.Empty;
            while (condition)
            {
                number = Utiles.GenerateRandomCardNumber();
                var car = FindCardByNumber(number);
                if (car == null)
                {
                    condition = false;
                }
            }
            card.Number = number;
            Create(card);
            SaveChanges();
        }
        public IEnumerable<Card> GetCardsByClient(long clientId)
        {
            return FindByCondition(card=> card.ClientId == clientId)
            .ToList();
        }
        public Card FindCardByNumber(string cardNumber)
        {
            return FindByCondition(card=> card.Number == cardNumber)
                .FirstOrDefault();
        }
    }
}
