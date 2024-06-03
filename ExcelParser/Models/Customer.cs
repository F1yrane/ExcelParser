namespace ExcelParser.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int Nid1 { get; set; }
        public required string Name { get; set; }
        public required string Date {  get; set; }
        public int EndCost { get; set; }
    }
}
