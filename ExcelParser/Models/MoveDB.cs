using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using Org.BouncyCastle.Asn1.Ocsp;


namespace ExcelParser.Models
{
    public class MoveDB
    {
        public int Id { get; set; }
        public required string Coderem { get; set; }
        public required string Coderab { get; set; }
        public int Name { get; set; }
    }
}
