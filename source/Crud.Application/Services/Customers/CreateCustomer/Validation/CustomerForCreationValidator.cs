using Crud.Application.Services.Customers.CreateCustomer.Models;
using FluentValidation;

namespace Crud.Application.Services.Customers.CreateCustomer.Validation;

/// <summary>
/// Validator for the CustomerForCreateDto class.
/// </summary>
public class CustomerForCreationValidator : AbstractValidator<CustomerForCreateDto>
{
    public CustomerForCreationValidator()
    {
        // Rule: The FirstName property must not be empty
        RuleFor(customer => customer.FirstName).NotEmpty();

        // Rule: The LastName property must not be empty
        RuleFor(customer => customer.LastName).NotEmpty();

        // Rule: The Email property must not be empty and must be a valid email address
        RuleFor(customer => customer.Email).NotEmpty().EmailAddress();

        // Rule: The Address property must not be empty
        RuleFor(customer => customer.Address).NotEmpty();

        // Rule: The PhoneNumber property must not be empty
        RuleFor(customer => customer.PhoneNumber).NotEmpty();

        // Rule: The Sales collection must not be null and each sale must be validated using the SaleForUpdateValidator
        RuleFor(customer => customer.Sales).NotNull().ForEach(sale =>
        {
            // Set the SaleForUpdateValidator as the validator for each sale
            sale.SetValidator(new SaleForCreationValidator());
        });
    }
}