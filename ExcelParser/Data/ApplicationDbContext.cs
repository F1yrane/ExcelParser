using Microsoft.EntityFrameworkCore;
using ExcelParser.Models;

namespace ExcelParser.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Price> Prices { get; set; }
        public DbSet<CodeRem> CodeRems { get; set; }
        public DbSet<CostRem> CostRems { get; set; }
        public DbSet<MoveDB> MoveDBs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
