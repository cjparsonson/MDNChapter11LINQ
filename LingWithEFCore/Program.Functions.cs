using Northwind.EntityModels;
using Microsoft.EntityFrameworkCore;
partial class Program
{
    private static void FilterAndSort()
    {
        SectionTitle("Filter and sort");
        using NorthwindDb db = new();
        DbSet<Product> allProducts = db.Products;
        IQueryable<Product> filteredProducts =
            allProducts.Where(product => product.UnitPrice < 10M);
        IOrderedQueryable<Product> sortedAndFilteredProducts =
            filteredProducts.OrderByDescending(product => product.UnitPrice);
        WriteLine("Products that cost less than $10:");

    }
}
