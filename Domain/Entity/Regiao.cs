using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Regiao : EntityBase
    {
        public int RegiaoDdd { get; set; }
        public string Nome { get; set; }
        public ICollection<Contato> Contatos { get; set; }
    }
}
