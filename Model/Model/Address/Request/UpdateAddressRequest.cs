using System;

namespace Domain.Model
{
  public class UpdateAddressRequest
  {
    public string Street { get; set; }
    public string Description { get; set; }
    public string Number { get; set; }
    public string Complement { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip_code { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public Guid Consumer_id { get; set; }
    public Guid Address_id { get; set; }
  }
}
