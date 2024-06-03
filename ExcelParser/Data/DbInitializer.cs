using System.Linq;
using ExcelParser.Models;

namespace ExcelParser.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Prices.Any())
            {
                return; // Database has been seeded
            }

            var prices = new Price[]
            {
                new() { Coderab="201", Name="Разборка", Cost=100},
                new() { Coderab="202", Name="Сборка", Cost=150},
                new() { Coderab="203", Name="Пайка", Cost=200},
                new() { Coderab="204", Name="Замена", Cost=300},
                new() { Coderab="205", Name="Сушка", Cost=250},
            };
                foreach (var price in prices)
                {
                    context.Prices.Add(price);
                }
                context.SaveChanges();

            var moveDB = new MoveDB[]
            {
                new() {Coderem="1.1", Coderab="201", Name=1},
                new() {Coderem="1.1", Coderab="202", Name=1},
                new() {Coderem="1.1", Coderab="203", Name=1},
                new() {Coderem="1.2", Coderab="204", Name=1},
                new() {Coderem="2.2", Coderab="204", Name=2},
                new() {Coderem="2.3", Coderab="205", Name=2},
            };
                foreach (var db in moveDB)
                {
                    context.MoveDBs.Add(db);
                }
                context.SaveChanges();
        }
    }
}
