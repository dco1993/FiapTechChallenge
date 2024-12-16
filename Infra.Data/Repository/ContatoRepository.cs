using Domain.Entity;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repository
{
    public class ContatoRepository : AppRepository<Contato>, IContatoRepository
    {
        public ContatoRepository(AppDbContext context) : base(context)
        {
        }

        public IList<Contato> ObterContatosPorDdd(int ddd)
        {
            var contatos = _context.Contato
                .Include(r => r.Regiao).ToList()
            ?? throw new Exception("Nenhum contato encontrado com esse DDD."); ;

            contatos = contatos
                .Where(c => c.TelefoneDdd == ddd)
                .Select(c => {
                    c.Regiao.Contatos = null;
                    return c;
                }).ToList()
            ?? throw new Exception("Nenhum contato encontrado com esse DDD.");

            return contatos;
        }
    }
}
