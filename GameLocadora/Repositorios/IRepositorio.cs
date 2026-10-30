namespace GameLocadora.Repositorios;

public interface IRepositorio<T> where T : class
{
    List<T> ObterTodos();

    T? ObterPorId(int id);

    void Adicionar(T entidade);

    void Atualizar(T entidade);

    void Remover(T entidade);
}
