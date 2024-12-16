using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Inputs
{
    public class ContatoUpdate
    {
        public required int Id { get; set; }
        public required string NomeCompleto { get; set; }
        public required string Email { get; set; }
        public required int TelefoneDdd { get; set; }
        public required string TelefoneNum { get; set; }
    }
}
