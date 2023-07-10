using Crud.Application.Services.Customers.UpdateCustomer.Models;
using FluentValidation;

namespace Crud.Application.Services.Customers.UpdateCustomer.Validation;

/// <summary>
/// Validator for the SaleForUpdateDto class.
/// </summary>
public class SaleForUpdateValidator : AbstractValidator<SaleForUpdateDto>
{
    public SaleForUpdateValidator()
    {
        // Rule: The ProductId should not be empty
        RuleFor(sale => sale.ProductId).NotEmpty().WithMessage("Product ID is required.");

        // Rule: The Quantity must be greater than zero
        RuleFor(sale => sale.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        // Rule: The UnitPrice must be greater than zero
        RuleFor(sale => sale.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than zero.");
    }
}
