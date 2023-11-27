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
        var projectedProducts = sortedAndFilteredProducts
            .Select(product => new // Anonymous type)
            {
                product.ProductId,
                product.ProductName,
                product.UnitPrice
            });
        WriteLine("Products that cost less than $10:");
        WriteLine(projectedProducts.ToQueryString());
        foreach (var p in projectedProducts)
        {
            WriteLine("{0}: {1} costs {2:$#,##0.00}",
                p.ProductId, p.ProductName, p.UnitPrice);
        }
        WriteLine();
    }

    private static void JoinCategoriesAndProducts()
    {
        SectionTitle("Join categories and products");
        using NorthwindDb db = new();
        // Join every product to its category to return 77 matches
        var queryJoin = db.Categories.Join(
            inner: db.Products,
            outerKeySelector: category => category.CategoryId,
            innerKeySelector: product => product.CategoryId,
            resultSelector: (c, p) =>
            new { c.CategoryName, p.ProductName, p.ProductId })
            .OrderBy(cp => cp.CategoryName);
        foreach (var p in  queryJoin)
        {
            WriteLine($"{p.ProductId}: {p.ProductName} in {p.CategoryName}");
        }
    }

    private static void GroupJoinCategoriesAndProducts()
    {
        SectionTitle("Group join categories and products");
        using NorthwindDb db = new();
        // Group all products by their category and return 8 matches
        var queryGroup = db.Categories.AsEnumerable().GroupJoin(
            inner: db.Products,
            outerKeySelector: category => category.CategoryId,
            innerKeySelector: product => product.CategoryId,
            resultSelector: (c, matchingProducts) => new
            {
                c.CategoryName,
                Products = matchingProducts.OrderBy(p => p.ProductName)
            });
        foreach (var c in queryGroup)
        {
            WriteLine($"{c.CategoryName} has {c.Products.Count()} products");
            foreach (var product in c.Products)
            {
                WriteLine($"    {product.ProductName}");
            }
        }
    }

    private static void ProductsLookup()
    {
        SectionTitle("Products lookup");
        using NorthwindDb db = new();
        // Join all products to their category to return 77 matches
        var productQuery = db.Categories.Join(
            inner: db.Products,
            outerKeySelector: category => category.CategoryId,
            innerKeySelector: product => product.CategoryId,
            resultSelector: (c, p) => new { c.CategoryName, Product = p });
        ILookup<string, Product> productLookup = productQuery.ToLookup(
            keySelector: cp => cp.CategoryName,
            elementSelector: cp => cp.Product);
        foreach (IGrouping<string, Product> group in productLookup)
        {
            // Key is beverages, condiments, etc
            WriteLine($"{group.Key} has {group.Count()} products");
            foreach(Product product in group)
            {
                WriteLine($"    {product.ProductName}");
            }
        }
        // We can look up the products by category name
        Write("Enter a category name: ");
        string categoryName = ReadLine()!;
        WriteLine();
        WriteLine($"Products in {categoryName}:");
        IEnumerable<Product> productsInCategory = productLookup[categoryName];
        foreach (Product product in productsInCategory)
        {
            WriteLine($"    {product.ProductName}");
        }
    }

    private static void AggregateProducts()
    {
        SectionTitle("Aggregate products");
        using NorthwindDb db = new();
        // Try to get an efficient count from EF core DbSet<T>
        if (db.Products.TryGetNonEnumeratedCount(out int countDbSet))
        {
            WriteLine($"{"The product count from DbSet",-25} {countDbSet,10}");
        }
        else
        {
            WriteLine("Products DbSet does not have a Count property");
        }
        // Try to get an efficient count from a List<T>
        List<Product> products = db.Products.ToList();
        if (products.TryGetNonEnumeratedCount(out int countList))
        {
            WriteLine($"{"The product count from List:",-25} {countList,10}");
        }
        else
        {
            WriteLine("Products list does not have a Count property");
        }
        WriteLine($"{"Product count:",-25} {db.Products.Count(),10}");
        WriteLine($"{"Discontinued product count:",-27} {db.Products
          .Count(product => product.Discontinued),8}");
        WriteLine($"{"Highest product price:",-25} {db.Products
          .Max(p => p.UnitPrice),10:$#,##0.00}");
        WriteLine($"{"Sum of units in stock:",-25} {db.Products
          .Sum(p => p.UnitsInStock),10:N0}");
        WriteLine($"{"Sum of units on order:",-25} {db.Products
          .Sum(p => p.UnitsOnOrder),10:N0}");
        WriteLine($"{"Average unit price:",-25} {db.Products
          .Average(p => p.UnitPrice),10:$#,##0.00}");
        WriteLine($"{"Value of units in stock:",-25} {db.Products
          .Sum(p => p.UnitPrice * p.UnitsInStock),10:$#,##0.00}");
    }
}
