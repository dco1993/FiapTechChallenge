
namespace Domain.Repository
{
    public interface IRepository<T>
    {
        IList<T> ObterTodos();
        T ObterPorId(int id);
        void Cadastrar(T entidade);
        void Atualizar(T entidade);
        void Excluir(int id);

    }
}
