namespace Volatus.Domain.View;

public class PaginationParams
{
    public string SortColumn { get; set; } = "createdAt";
    public string SortDirection { get; set; } = "asc";
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int Count { get; set; }
    public bool isLastPage { get; set; } = false;
}