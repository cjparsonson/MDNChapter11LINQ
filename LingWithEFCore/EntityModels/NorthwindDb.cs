using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace Northwind.EntityModels;
public class NorthwindDb : DbContext
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string database = "Northwind.db";
        string dir = Environment.CurrentDirectory;
        string path = String.Empty;
        // The database file will stay in the project folder.
        // We will automatically adjust the relative path to 
        // account for running in VS2022 or from terminal.
        if (dir.EndsWith("net8.0"))
        {
            // Running in the <project>\bin\<Debug|Release>\net8.0 directory.
            path = Path.Combine("..", "..", "..", database);
        }
        else
        {
            // Running in the <project> directory
            path = database;
        }
        path = Path.GetFullPath(path); // Convert to absolute path
        WriteLine($"SQLite Database Path: {path}");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                message: $"{path} not found.", fileName: path );
        }
        optionsBuilder.UseSqlite($"Data Source={path}");

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (Database.ProviderName is not null && Database.ProviderName.Contains("Sqlite"))
        {
            // SQLite data provider does not directly support the
            // decimal type so we can convert to double instead.
            modelBuilder.Entity<Product>()
                .Property(product => product.UnitPrice)
                .HasConversion<double>();
        }
    }
}