using System.ComponentModel.DataAnnotations;

namespace ExcelParser.Models
{
    public class CostRem
    {
        [Key]
        public int Id { get; set; }
        public required string Coderab { get; set; }
        public required string Name { get; set; }
        public int Cost { get; set; }
        public int Nid { get; set; }
    }
}
