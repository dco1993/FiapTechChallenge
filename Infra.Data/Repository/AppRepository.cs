using Domain.Entity;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repository
{
    public class AppRepository<T> : IRepository<T> where T : EntityBase
    {
        protected AppDbContext _context;
        protected DbSet<T> _dbSet;

        protected AppRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Atualizar(T entidade)
        {
            _context.Set<T>().Update(entidade);
            _context.SaveChanges();
        }

        public void Cadastrar(T entidade)
        {
            _context.Set<T>().Add(entidade);
            _context.SaveChanges();
        }

        public void Excluir(int id)
        {
            _context.Set<T>().Remove(ObterPorId(id));
            _context.SaveChanges();
        }

        public T ObterPorId(int id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public IList<T> ObterTodos()
        {
            return _context.Set<T>().ToList();
        }
    }
}
