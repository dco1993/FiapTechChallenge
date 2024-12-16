using Domain.Entity;

namespace Domain.Repository
{
    public interface IContatoRepository : IRepository<Contato>
    {
        IList<Contato> ObterContatosPorDdd(int ddd);
    }
}
