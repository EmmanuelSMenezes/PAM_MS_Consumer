using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.Model;
using Npgsql;
using Serilog;

namespace Infrastructure.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;
        public AddressRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }
        public Address CreateAddress(CreateAddressRequest createAddressRequest)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var sql = @$"
            INSERT INTO consumer.address
            (
              street
            , description
            , number
            , complement
            , district
            , city
            , state
            , zip_code
            , active
            , created_at
            , latitude
            , longitude
            , consumer_id
            )
            VALUES
            (
               '{createAddressRequest.Street}'
              ,'{createAddressRequest.Description}'
              ,'{createAddressRequest.Number}'
              ,'{createAddressRequest.Complement}'
              ,'{createAddressRequest.District}'
              ,'{createAddressRequest.City}'
              ,'{createAddressRequest.State}'
              ,'{createAddressRequest.Zip_code}'
              ,true
              ,CURRENT_TIMESTAMP
              ,'{createAddressRequest.Latitude}'
              ,'{createAddressRequest.Longitude}'
              ,'{createAddressRequest.Consumer_id}'
            ) RETURNING *;
          ";
                    var response = connection.Query<Address>(sql).FirstOrDefault<Address>();

                    _logger.Information("[AddressRepository - CreateAddress]: Address Created Successfully.");
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[AddressRepository - CreateAddress]: Error while create address on db.");
                throw ex;
            }
        }

        public List<Address> GetAllAddresses()
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var sql = @$"SELECT * FROM consumer.address;";
                    var response = connection.Query<Address>(sql).ToList<Address>();

                    _logger.Information("[AddressRepository - GetAllAddresses]: Addresses Returned Successfully.");
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[AddressRepository - GetAdressesByConsumerId]: Error while list addresses.");
                throw ex;
            }
        }

        public List<Address> GetAdressesByConsumerId(Guid consumer_id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var sql = @$"SELECT * FROM consumer.address WHERE consumer_id = '{consumer_id}';";
                    var response = connection.Query<Address>(sql).ToList<Address>();

                    _logger.Information("[AddressRepository - GetAdressesByConsumerId]: Addresses Returned Successfully.");
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[AddressRepository - GetAdressesByConsumerId]: Error while list addresses.");
                throw ex;
            }
        }
        public Address GetAddressByAddressId(Guid address_id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var sql = @$"SELECT * FROM consumer.address WHERE address_id = '{address_id}';";
                    var response = connection.Query<Address>(sql).FirstOrDefault<Address>();

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while get address by address id");
                throw ex;
            }
        }
        public bool DeleteAddress(Address address)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var sql = @$"DELETE FROM consumer.address WHERE address_id = '{address.Address_id}';";
                    var affectedRows = connection.Execute(sql);

                    if (affectedRows > 0) return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while delete address");
                throw ex;
            }
        }
        public Address UpdateAddress(Address address)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var sql = @$"UPDATE consumer.address
                    SET 
                      street='{address.Street}'
                    , number='{address.Number}'
                    , complement='{address.Complement}'
                    , district='{address.District}'
                    , city='{address.City}'
                    , state='{address.State}'
                    , zip_code='{address.Zip_code}'
                    , active={address.Active}
                    , updated_at=CURRENT_TIMESTAMP
                    , latitude='{address.Latitude}'
                    , longitude='{address.Longitude}'
                    , description='{address.Description}'
                    WHERE address_id = '{address.Address_id}' RETURNING *;
                  ";
                    var response = connection.Query<Address>(sql).FirstOrDefault<Address>();

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while updating address");
                throw ex;
            }
        }

    }
}
