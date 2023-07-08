using AutoMapper;
using Crud.Application.Services.Customers.GetCustomers;
using Crud.Domain.Entities;

namespace Crud.Application.Core.AutoMapperProfiles;

/// <summary>
/// AutoMapper profile for mapping properties of <see cref="Sale"/>.
/// </summary>
public class SaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SaleProfile"/> class.
    /// </summary>
    public SaleProfile()
    {
        CreateMap<Sale, SalesForListDto>()
            .ForMember(dto => dto.ProductName, expression => expression.MapFrom(sale => sale.Product.Name));
    }
}
