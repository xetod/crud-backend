namespace Crud.Application.Core.ResourceParameters;

public class ResourceParameter
{
    public int CurrentPage { get; set; } = 1;
    
    public int PageSize { get; set; } = 10;
    
    public bool IsAscending { get; set; } = true;
}