using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.Model;
using Npgsql;
using Serilog;

namespace Infrastructure.Repository
{
  public class CardRepository : ICardRepository
  {
    private readonly string _connectionString;
    private readonly ILogger _logger;
    public CardRepository(string connectionString, ILogger logger)
    {
      _connectionString = connectionString;
      _logger = logger;
    }

        public CardResponse CreateCardRepository(CreateCardRequest createCardRequest)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var sql = @$"INSERT INTO consumer.card
                                    (consumer_id, number, name, validity, created_by, document)
                                 VALUES('{createCardRequest.Consumer_id}','{createCardRequest.Number}', 
                                '{createCardRequest.Name}','{createCardRequest.Validity}','{createCardRequest.Created_by}', 
                                '{createCardRequest.Document}') RETURNING *;";

                    var response = connection.Query<CardResponse>(sql).FirstOrDefault();

                    return response;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public CardResponse DeleteCardRepository(Guid card_id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var sql = @$"DELETE FROM consumer.card WHERE card_id='{card_id}' RETURNING *;";

                    var response = connection.Query<CardResponse>(sql).FirstOrDefault();

                    return response;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<CardResponse> GetCardRepository(Guid consumer_id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var sql = @$"SELECT * FROM consumer.card WHERE consumer_id = '{consumer_id}';";
                    var response = connection.Query<CardResponse>(sql).ToList();

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while get consumer by consumer id");
                throw;
            }
        }

        public Consumer GetConsumerByConsumerIdRepository(Guid consumer_id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var sql = @$"SELECT * FROM consumer.consumer WHERE consumer_id = '{consumer_id}';";
                    var response = connection.Query<Consumer>(sql).FirstOrDefault();

                   return response;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while get consumer by consumer id");
                throw;
            }
        }

        public CardResponse UpdateCardRepository(UpdateCardRequest updateCardRequest)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var sql = @$"UPDATE consumer.card
                                 SET number='{updateCardRequest.Number}'
                                   , name = '{updateCardRequest.Name}'
                                   , validity='{updateCardRequest.Validity}'
                                   , active={updateCardRequest.Active}
                                   , updated_by='{updateCardRequest.Updated_by}'
                                   , document='{updateCardRequest.Document}'
                                   , updated_at=CURRENT_TIMESTAMP
                                 WHERE card_id='{updateCardRequest.Card_id}' RETURNING *";

                    var response = connection.Query<CardResponse>(sql).FirstOrDefault();

                    return response;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
