namespace Application.Dto;

public class AddressDto
{
    public string? AddressId { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int? PostalCode { get; set; }
    public string AdditionalDetails { get; set; }
}
