using System.ComponentModel.DataAnnotations;

namespace ExcelParser.Models
{
    public class CodeRem
    {
        [Key]
        public int Id { get; set; }
        public required string Coderem { get; set; }
        public required string Name { get; set; }
    }
}
