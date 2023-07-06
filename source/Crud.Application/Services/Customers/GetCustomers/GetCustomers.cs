using Crud.Application.Core.ResourceParameters;
using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.GetCustomers.Specifications;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Domain.Entities;
using RepositoryService.Data.Core.Specifications;
using System.Reflection.Metadata;

namespace Crud.Application.Services.Customers.GetCustomers;

public class GetCustomers
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomers(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CollectionResource<Customer>>> ExecuteAsync(CustomerResourceParameter parameter)
    {
        var specification = CreateSpecification(parameter);
        var customers = await _unitOfWork.Customer.GetCustomersWithPagination(specification, parameter.CurrentPage, parameter.PageSize);

        var paginationMetadata = new PaginationMetadata
        {
            TotalCount = customers.TotalCount,
            PageSize = customers.PageSize,
            CurrentPage = customers.CurrentPage,
            TotalPages = customers.TotalPages
        };

        var collectionResource = new CollectionResource<Customer>
        {
            Results = customers,
            Pagination = paginationMetadata
        };

        return Result.Ok(collectionResource);
    }

    private Specification<Customer> CreateSpecification(CustomerResourceParameter parameter)
    {
        var specification = Specification<Customer>.All;
        specification = specification.And(new CustomerByNameSpecification(parameter.CustomerName));

        switch (parameter.SortBy.ToLower())
        {
            case "Name":
                if (parameter.IsAscending)
                {
                    specification = specification.SortAscending(new CustomerSortByNameSpecification());
                    break;
                }
                specification = specification.SortDescending(new CustomerSortByNameSpecification());
                break;
            default:
                specification = specification.SortAscending(new CustomerSortByNameSpecification());
                break;
        }

        return specification;
    }
}