using System;
using System.Collections.Generic;
using Domain.Model;
using Infrastructure.Repository;
using Serilog;

namespace Application.Service
{
  public class ConsumerService : IConsumerService
  {
    public readonly IConsumerRepository _repository;
    private readonly ILogger _logger;
    public ConsumerService(
      IConsumerRepository repository,
      ILogger logger
    )
    {
      _repository = repository;
      _logger = logger;
    }
    public CreateConsumerResponse CreateConsumerService(CreateConsumerRequest createConsumerRequest)
    {
      try
      {
        Consumer consumer = GetConsumerByUserId(createConsumerRequest.User_id);
        if (consumer != null) throw new Exception("consumerExistsToUser");


        var consumerCreated = _repository.CreateConsumer(createConsumerRequest);
        if (consumerCreated == null) throw new Exception("consumerNotCreated");

        _logger.Information("[ConsumerService - CreateConsumerService]: Consumer created successfully.");
        return consumerCreated;
      }
      catch (Exception ex)
      {
        _logger.Error(ex, $"[ConsumerService - CreateConsumerService]: Error while create consumer.");
        throw ex;
      }
    }

    public Consumer GetConsumerByUserId(Guid user_id)
    {
      try
      {
        _logger.Information("[ConsumerService - GetConsumerByUserId]: Consumer retrieved successfully.");
        return _repository.GetConsumerByUserId(user_id);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, $"[ConsumerService - GetConsumerByUserId]: Error while retrieve consumer by id {user_id}.");
        throw ex;
      }
    }

    public Consumer GetConsumerByConsumerId(Guid consumer_id)
    {
      try
      {
        _logger.Information("[ConsumerService - GetConsumerByConsumerId]: Consumer retrieved successfully.");
        return _repository.GetConsumerByConsumerId(consumer_id);
      }
      catch (Exception ex)
      {
        _logger.Error(ex, $"[ConsumerService - GetConsumerByConsumerId]: Error while retrieve consumer by id {consumer_id}.");
        throw ex;
      }
    }

    public List<Consumer> GetConsumers()
    {
      try
      {
        _logger.Information("[ConsumerService - GetConsumersService]: Consumers retrieved successfully.");
        return _repository.GetConsumers();
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "[ConsumerService - GetConsumersService]: Error while retrieve consumers.");
        throw ex;
      }
    }
    public UpdateConsumerResponse UpdateConsumerService(UpdateConsumerRequest updateConsumerRequest)
    {
      try
      {
        var consumer = GetConsumerByUserId(updateConsumerRequest.User_id);
        if (consumer == null) throw new Exception("consumerNotExists");

        consumer.Legal_name = updateConsumerRequest.Legal_name;
        consumer.Fantasy_name = updateConsumerRequest.Legal_name;
        consumer.Document = updateConsumerRequest.Document;
        consumer.Email = updateConsumerRequest.Email;
        consumer.Phone_number = updateConsumerRequest.Phone_number;
        consumer.Active = updateConsumerRequest.Active;
        consumer.Default_address = updateConsumerRequest.Default_address;

        var updatedConsumer = _repository.UpdateConsumer(consumer);
        if (updatedConsumer == null) throw new Exception("consumerNotUpdated");

        _logger.Information("[ConsumerService - UpdateConsumerService]: Consumer updated successfully.");

        return new UpdateConsumerResponse()
        {
          Consumer_id = updatedConsumer.Consumer_id,
          Active = updatedConsumer.Active,
          Addresses = updatedConsumer.Addresses,
          Created_at = updatedConsumer.Created_at,
          Deleted_at = updatedConsumer.Deleted_at,
          Document = updatedConsumer.Document,
          Email = updatedConsumer.Email,
          Fantasy_name = updatedConsumer.Fantasy_name,
          Legal_name = updatedConsumer.Legal_name,
          Phone_number = updatedConsumer.Phone_number,
          Updated_at = updatedConsumer.Updated_at,
          User_id = updatedConsumer.User_id,
          Default_address = updatedConsumer.Default_address
        };
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "[ConsumerService - UpdateConsumerService]: Error while updating consumer.");
        throw ex;
      }
    }
  }
}
