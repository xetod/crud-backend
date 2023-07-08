using AutoMapper;
using Crud.Application.Core.ResourceParameters;
using Crud.Application.Core.Result;
using Crud.Application.Services.Customers.GetCustomers.Models;
using Crud.Application.Services.Customers.GetCustomers.Specifications;
using Crud.Data.Core.PagedLists;
using Crud.Data.Core.Specifications;
using Crud.Data.Repositories.Core.UnitOfWorks;
using Crud.Domain.Entities;

namespace Crud.Application.Services.Customers.GetCustomers;

/// <summary>
/// Implementation of the <see cref="IGetCustomers"/> interface for retrieving customers with pagination.
/// </summary>
public class GetCustomers : IGetCustomers
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCustomers"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work providing access to customer data.</param>
    /// <param name="mapper">The mapper used for object-to-object mapping.</param>
    public GetCustomers(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a collection of customers with pagination based on the provided parameters.
    /// </summary>
    /// <param name="parameter">The customer resource parameter containing query parameters.</param>
    /// <returns>A result containing the collection of customers with pagination metadata.</returns>
    public async Task<Result<CollectionResource<CustomerForListDto>>> ExecuteAsync(CustomerResourceParameter parameter)
    {
        // Create the specification based on the provided parameter
        var specification = CreateSpecification(parameter);

        // Retrieve customers with pagination using the specification and parameters
        var customers = await _unitOfWork.Customer.GetCustomersWithPagination(specification, parameter.CurrentPage, parameter.PageSize);

        // Create pagination metadata based on the retrieved customers
        var paginationMetadata = new PaginationMetadata
        {
            TotalCount = customers.TotalCount,
            PageSize = customers.PageSize,
            CurrentPage = customers.CurrentPage,
            TotalPages = customers.TotalPages
        };

        // Map the retrieved customers to a list of CustomerForListDto using AutoMapper
        var collectionResource = new CollectionResource<CustomerForListDto>
        {
            Results = _mapper.Map<PagedList<Customer>, List<CustomerForListDto>>(customers),
            Pagination = paginationMetadata
        };

        // Return the success result with the collection resource
        return Result.Ok(collectionResource);
    }

    /// <summary>
    /// Creates a composite specification based on the search text and sort criteria specified in the customer resource parameter.
    /// </summary>
    /// <param name="parameter">The customer resource parameter containing the search text and sort criteria.</param>
    /// <returns>A composite specification for filtering and sorting customers.</returns>
    private Specification<Customer> CreateSpecification(CustomerResourceParameter parameter)
    {
        var specification = Specification<Customer>.All;

        // Add additional specifications based on search text
        specification = specification.And(new CustomerByNameSpecification(parameter.SearchText));

        // Determine sort order based on sort by property
        switch (parameter.SortBy.ToLower())
        {
            case "name":
                if (parameter.IsAscending)
                {
                    specification = specification.SortAscending(new SortCustomerByNameSpecification());
                }
                else
                {
                    specification = specification.SortDescending(new SortCustomerByNameSpecification());
                }
                break;
            default:
                specification = specification.SortAscending(new SortCustomerByNameSpecification());
                break;
        }

        return specification;
    }
}