using AutoMapper;
using Crud.Application.Services.Customers.GetCustomer.Models;
using Crud.Application.Services.Customers.GetCustomers.Models;
using Crud.Domain.Entities;

namespace Crud.Application.Core.AutoMapperProfiles.Sales;

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
        CreateMap<Sale, SaleForListDto>()
            .ForMember(dto => dto.ProductName, expression => expression.MapFrom(sale => sale.Product.Name));

        CreateMap<Sale, SaleForDetailDto>()
            .ForMember(dto => dto.ProductName, expression => expression.MapFrom(sale => sale.Product.Name));


    }
}
