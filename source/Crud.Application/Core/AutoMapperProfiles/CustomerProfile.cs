using AutoMapper;
using Crud.Application.Services.Customers.GetCustomer.Models;
using Crud.Application.Services.Customers.GetCustomers.Models;
using Crud.Domain.Entities;

namespace Crud.Application.Core.AutoMapperProfiles
{
    /// <summary>
    /// AutoMapper profile for mapping properties of <see cref="Customer"/>.
    /// </summary>
    public class CustomerProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerProfile"/> class.
        /// </summary>
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerForListDto>();
            CreateMap<Customer, CustomerForDetailDto>();
        }
    }
}

