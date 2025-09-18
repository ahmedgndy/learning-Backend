
public class PageResponse<T>
{
    //items in this page
    public IEnumerable<T> Items { get; set; }
    
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set;  }
    public int TotalPages  => (int)Math.Ceiling(TotalCount / (double) PageSize);
 

    public bool HasNext => CurrentPage < TotalPages;
    public bool HasPrevious => CurrentPage > 1;

    private PageResponse() { }


    public static PageResponse<T> Create(IEnumerable<T> items , int pageSize , int totalCount , int page ) {
        return new PageResponse<T>
        {
            Items = items,
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = totalCount,
        };

    }


}