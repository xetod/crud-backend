namespace Crud.Domain.Entities;

public class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Address { get; set; }

    public string PhoneNumber { get; set; }

    public ICollection<Sale> Sales { get; set; }
}