using System;
using System.Collections.Generic;
using Domain.Model;

namespace Infrastructure.Repository
{
    public interface ICardRepository
    {
        CardResponse CreateCardRepository(CreateCardRequest createCardRequest);
        Consumer GetConsumerByConsumerIdRepository(Guid consumer_id);
        CardResponse UpdateCardRepository(UpdateCardRequest updateCardRequest);
        List<CardResponse> GetCardRepository(Guid consumer_id);
        CardResponse DeleteCardRepository(Guid card_id);
    }
}
