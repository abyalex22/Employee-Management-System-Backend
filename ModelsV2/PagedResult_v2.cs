namespace EmployeeManagement.API.ModelsV2.DTOs
{
    public class PagedResult_v2<T>
    {
        public List<T> Data { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
