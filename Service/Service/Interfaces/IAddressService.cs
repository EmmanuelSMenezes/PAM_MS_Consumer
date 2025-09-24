using System;
using System.Collections.Generic;
using Domain.Model;

namespace Application.Service
{
    public interface IAddressService
    {
        CreateAddressResponse CreateAddressService(CreateAddressRequest createAddressRequest);
        List<Address> GetAdressesByConsumerId(Guid consumer_id);
        Address GetAddressByAddressId(Guid address_id);
        bool DeleteAddress(List<Guid> ids);
        UpdateAddressResponse UpdateAddress(UpdateAddressRequest updateAddressRequest);
    }
}
