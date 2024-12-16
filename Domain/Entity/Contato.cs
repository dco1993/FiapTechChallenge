using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Contato : EntityBase
    {
        public required string NomeCompleto { get; set; }
        public required string Email { get; set; }
        public required int TelefoneDdd { get; set; }
        public required string TelefoneNum { get; set; }
        public Regiao Regiao { get; set; }
    }
}
