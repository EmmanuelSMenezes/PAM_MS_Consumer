using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.Model;
using Npgsql;
using Serilog;

namespace Infrastructure.Repository
{
  public class ConsumerRepository : IConsumerRepository
  {
    private readonly string _connectionString;
    private readonly ILogger _logger;
    public ConsumerRepository(string connectionString, ILogger logger)
    {
      _connectionString = connectionString;
      _logger = logger;
    }

    public CreateConsumerResponse CreateConsumer(CreateConsumerRequest createConsumerRequest)
    {
      try
      {
        using (var connection = new NpgsqlConnection(_connectionString))
        {

          var sql = $@"
            INSERT INTO consumer.consumer
              (
                legal_name
              , fantasy_name
              , document
              , email
              , phone_number
              , active
              , created_at
              , user_id
            ) VALUES (
                '{createConsumerRequest.Legal_name}'
              , '{createConsumerRequest.Fantasy_name}'
              , '{createConsumerRequest.Document}'
              , '{createConsumerRequest.Email}'
              , '{createConsumerRequest.Phone_number}'
              , true
              , CURRENT_TIMESTAMP
              , '{createConsumerRequest.User_id}'
            ) RETURNING *;
          ";

          var insertedConsumer = connection.Query<Consumer>(sql).FirstOrDefault();

          if (insertedConsumer == null)
          {
            throw new Exception("errorWhileInsertConsumerOnDB");
          }

          return new CreateConsumerResponse()
          {
            User_id = insertedConsumer.User_id,
            Active = insertedConsumer.Active,
            Addresses = new List<Address>(),
            Consumer_id = insertedConsumer.Consumer_id,
            Created_at = insertedConsumer.Created_at,
            Deleted_at = insertedConsumer.Deleted_at,
            Document = insertedConsumer.Document,
            Email = insertedConsumer.Email,
            Fantasy_name = insertedConsumer.Fantasy_name,
            Legal_name = insertedConsumer.Legal_name,
            Phone_number = insertedConsumer.Phone_number,
            Updated_at = insertedConsumer.Updated_at
          };
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public Consumer GetConsumerByConsumerId(Guid consumer_id)
    {
      try
      {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
          var sql = @$"SELECT * FROM consumer.consumer WHERE consumer_id = '{consumer_id}';";
          var response = connection.Query<Consumer>(sql).FirstOrDefault<Consumer>();

          if (response != null)
          {
            var sqlAddress = @$"SELECT * FROM consumer.address where consumer_id='{response.Consumer_id}';";
            var responseAddress = connection.Query<Address>(sqlAddress).ToList<Address>();
            response.Addresses = responseAddress;
          }

          return response;
        }
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error while get consumer by consumer id");
        throw ex;
      }
    }

    public Consumer GetConsumerByUserId(Guid user_id)
    {
      try
      {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
          var sql = @$"SELECT * FROM consumer.consumer WHERE user_id = '{user_id}';";
          var response = connection.Query<Consumer>(sql).FirstOrDefault<Consumer>();

          if (response != null)
          {
            var sqlAddress = @$"SELECT * FROM consumer.address where consumer_id='{response.Consumer_id}';";
            var responseAddress = connection.Query<Address>(sqlAddress).ToList<Address>();
            response.Addresses = responseAddress;
          }

          return response;
        }
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error while get consumer by user id");
        throw ex;
      }
    }

    public List<Consumer> GetConsumers()
    {
      try
      {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
          var sql = @$"SELECT * FROM consumer.consumer;";
          var response = connection.Query<Consumer>(sql).ToList<Consumer>();

          return response;
        }
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error while get all consumers");
        throw ex;
      }
    }

    public Consumer UpdateConsumer(Consumer consumer)
    {
      try
      {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
          var sqlUpdateConsumer = @$"
                                  UPDATE consumer.consumer
                                    SET
                                      legal_name='{consumer.Legal_name}'
                                    , fantasy_name='{consumer.Fantasy_name}'
                                    , document='{consumer.Document}'
                                    , email='{consumer.Email}'
                                    , phone_number='{consumer.Phone_number}'
                                    , active={consumer.Active}
                                    , default_address='{consumer.Default_address}'
                                    , updated_at=CURRENT_TIMESTAMP
                                  WHERE consumer_id='{consumer.Consumer_id}' and user_id='{consumer.User_id}' RETURNING *;
                                ";
          var response = connection.Query<Consumer>(sqlUpdateConsumer).FirstOrDefault<Consumer>();
          if (response != null)
          {
            var sqlAddress = @$"SELECT * FROM consumer.address where consumer_id='{response.Consumer_id}';";
            var responseAddress = connection.Query<Address>(sqlAddress).ToList<Address>();
            response.Addresses = responseAddress;
          }

          return response;
        }
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Error while update consumer");
        throw ex;
      }
    }
  }
}
