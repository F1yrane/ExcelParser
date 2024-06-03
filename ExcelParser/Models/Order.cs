using System.ComponentModel.DataAnnotations;

namespace ExcelParser.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int Num { get; set; }
        public required string Name { get; set; }
        public required string Date { get; set; }
        public required string NameCustomers { get; set; }
    }
}
