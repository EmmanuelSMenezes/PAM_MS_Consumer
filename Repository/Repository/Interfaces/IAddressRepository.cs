using System;
using System.Collections.Generic;
using Domain.Model;

namespace Infrastructure.Repository
{
    public interface IAddressRepository
    {
        Address CreateAddress(CreateAddressRequest createAddressRequest);
        List<Address> GetAllAddresses();
        List<Address> GetAdressesByConsumerId(Guid consumer_id);
        Address GetAddressByAddressId(Guid address_id);
        bool DeleteAddress(Address address);
        Address UpdateAddress(Address address);
    }
}
