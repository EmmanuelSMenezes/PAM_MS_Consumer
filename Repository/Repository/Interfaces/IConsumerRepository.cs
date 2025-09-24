using System;
using System.Collections.Generic;
using Domain.Model;

namespace Infrastructure.Repository
{
  public interface IConsumerRepository
  {
    CreateConsumerResponse CreateConsumer(CreateConsumerRequest createConsumerRequest);
    Consumer UpdateConsumer(Consumer consumer);
    List<Consumer> GetConsumers();
    Consumer GetConsumerByUserId(Guid user_id);
    Consumer GetConsumerByConsumerId(Guid consumer_id);
  }
}
