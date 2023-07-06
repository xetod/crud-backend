namespace RepositoryService.Data.Core.Specifications;

public class Sort<T>
{
    public Specification<T> Specification { get; set; }
    
    public bool Ascending { get; set; }
}