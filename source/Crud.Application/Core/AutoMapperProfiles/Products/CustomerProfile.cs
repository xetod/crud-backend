using AutoMapper;
using Crud.Application.Services.Products.GetProducts.Models;
using Crud.Domain.Entities.Products;

namespace Crud.Application.Core.AutoMapperProfiles.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductForListDto>();
        }
    }
}