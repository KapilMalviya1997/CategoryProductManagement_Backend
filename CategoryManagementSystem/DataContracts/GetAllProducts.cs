using Product_Category_Management_System.Models;

namespace Product_Category_Management_System.DataContracts
{
    public class GetAllProducts
    {
        public class PagedResult
        {
            public int TotalRecords { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public List<Product> Items { get; set; }
        }
    }
}
