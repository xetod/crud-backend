﻿namespace Crud.Application.Core.ResourceParameters;

public class PaginationMetadata
{
    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }
}