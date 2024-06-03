using System.ComponentModel.DataAnnotations;

namespace ExcelParser.Models
{
    public class Price
    {
        [Key]
        public int Id { get; set; }
        public required string Coderab { get; set; }
        public required string Name { get; set; }
        public int Cost { get; set; }
    }
}
