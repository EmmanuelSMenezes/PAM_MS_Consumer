using System;
using System.Collections.Generic;
using Domain.Model;
using Infrastructure.Repository;
using Serilog;

namespace Application.Service
{
    public class AddressService : IAddressService
    {
        public readonly IAddressRepository _repository;
        public readonly IConsumerService _consumerService;
        private readonly ILogger _logger;
        public AddressService(
          IAddressRepository repository,
          IConsumerService consumerService,
          ILogger logger
        )
        {
            _repository = repository;
            _consumerService = consumerService;
            _logger = logger;
        }

        public CreateAddressResponse CreateAddressService(CreateAddressRequest createAddressRequest)
        {
            try
            {
                Consumer consumer = _consumerService.GetConsumerByConsumerId(createAddressRequest.Consumer_id);
                if (consumer == null) throw new Exception("consumerNotExists");

                if (string.IsNullOrEmpty(createAddressRequest.Description))
                {
                    createAddressRequest.Description = createAddressRequest.Street;
                }


                var response = _repository.CreateAddress(createAddressRequest);
                if (response == null) throw new Exception("addressNotCreated");

                List<Address> addresses = GetAdressesByConsumerId(consumer.Consumer_id);
                if (addresses.Count == 1)
                {
                    var updatedConsumer = _consumerService.UpdateConsumerService(
                      new UpdateConsumerRequest()
                      {
                          Active = consumer.Active,
                          Consumer_id = consumer.Consumer_id,
                          Default_address = response.Address_id,
                          Document = consumer.Document,
                          Email = consumer.Email,
                          Fantasy_name = consumer.Fantasy_name,
                          Legal_name = consumer.Legal_name,
                          Phone_number = consumer.Phone_number,
                          User_id = consumer.User_id
                      }
                    );
                    if (updatedConsumer == null) _logger.Error("[AddressService - CreateAddressService]: Error while updating default address.");
                }

                _logger.Information("[AddressService - CreateAddressService]: Address created successfully.");

                return new CreateAddressResponse()
                {
                    Active = response.Active,
                    Address_id = response.Address_id,
                    City = response.City,
                    Complement = response.Complement,
                    Consumer_id = response.Consumer_id,
                    Created_at = response.Created_at,
                    Deleted_at = response.Deleted_at,
                    District = response.District,
                    Latitude = response.Latitude,
                    Longitude = response.Longitude,
                    Number = response.Number,
                    State = response.State,
                    Street = response.Street,
                    Updated_at = response.Updated_at,
                    Zip_code = response.Zip_code
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[AddressService - CreateAddressService]: Error while create address.");
                throw ex;
            }
        }

        public List<Address> GetAdressesByConsumerId(Guid consumer_id)
        {
            try
            {
                Consumer consumer = _consumerService.GetConsumerByConsumerId(consumer_id);
                if (consumer == null) throw new Exception("consumerNotExists");

                var response = _repository.GetAdressesByConsumerId(consumer_id);

                _logger.Information("[AddressService - GetAdressesByConsumerId]: Addresses returned successfully.");
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[AddressService - GetAdressesByConsumerId]: Error while retrieve Addresses.");
                throw ex;
            }
        }

        public Address GetAddressByAddressId(Guid address_id)
        {
            try
            {
                var response = _repository.GetAddressByAddressId(address_id);
                if (response == null) throw new Exception("addressNotExists");

                _logger.Information("[AddressService - GetAddressByAddressId]: Address returned sucessfully.");
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[AddressService - GetAddressByAddressId]: Error while retrieve Address.");
                throw ex;
            }
        }

        public bool DeleteAddress(List<Guid> ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var address = GetAddressByAddressId(id);
                    _repository.DeleteAddress(address);
                }
                _logger.Information("[AddressService - DeleteAddress]: One or more adresses deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[AddressService - DeleteAddress]: Error while delete one or more addresses.");
                throw ex;
            }
        }
        public UpdateAddressResponse UpdateAddress(UpdateAddressRequest updateAddressRequest)
        {
            try
            {
                var address = GetAddressByAddressId(updateAddressRequest.Address_id);
                if (address == null) throw new Exception("addressNotExists");

                address.Street = updateAddressRequest.Street;
                address.Description = updateAddressRequest.Description;
                address.Number = updateAddressRequest.Number;
                address.Complement = updateAddressRequest.Complement;
                address.District = updateAddressRequest.District;
                address.City = updateAddressRequest.City;
                address.State = updateAddressRequest.State;
                address.Zip_code = updateAddressRequest.Zip_code;
                address.Latitude = updateAddressRequest.Latitude;
                address.Longitude = updateAddressRequest.Longitude;

                var response = _repository.UpdateAddress(address);
                if (address == null) throw new Exception("addressNotUpdated");

                return new UpdateAddressResponse()
                {
                    Active = response.Active,
                    Address_id = response.Address_id,
                    City = response.City,
                    Complement = response.Complement,
                    Created_at = response.Created_at,
                    Deleted_at = response.Deleted_at,
                    District = response.District,
                    Latitude = response.Latitude,
                    Longitude = response.Longitude,
                    Number = response.Number,
                    State = response.State,
                    Street = response.Street,
                    Updated_at = response.Updated_at,
                    Consumer_id = response.Consumer_id,
                    Zip_code = response.Zip_code
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error");
                throw ex;
            }
        }

    }
}
