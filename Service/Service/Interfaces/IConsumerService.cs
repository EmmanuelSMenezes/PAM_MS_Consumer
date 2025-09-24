using System;
using System.Collections.Generic;
using Domain.Model;

namespace Application.Service
{
  public interface IConsumerService
  {
    CreateConsumerResponse CreateConsumerService(CreateConsumerRequest createConsumerRequest);
    UpdateConsumerResponse UpdateConsumerService(UpdateConsumerRequest updateConsumerRequest);
    List<Consumer> GetConsumers();
    Consumer GetConsumerByUserId(Guid user_id);
    Consumer GetConsumerByConsumerId(Guid consumer_id);
  }
}
