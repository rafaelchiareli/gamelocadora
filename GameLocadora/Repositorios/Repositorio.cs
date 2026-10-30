using GameLocadora.Data;

namespace GameLocadora.Repositorios;

public class Repositorio<T> : IRepositorio<T> where T : class
{
    protected readonly LocadoraContext _db;

    public Repositorio(LocadoraContext db)
    {
        _db = db;
    }

    public List<T> ObterTodos()
    {
        return _db.Set<T>().ToList();
    }

    public T? ObterPorId(int id)
    {
        return _db.Set<T>().Find(id);
    }

    public void Adicionar(T entidade)
    {
        _db.Set<T>().Add(entidade);
        _db.SaveChanges();
    }

    public void Atualizar(T entidade)
    {
        _db.Set<T>().Update(entidade);
        _db.SaveChanges();
    }

    public void Remover(T entidade)
    {
        _db.Set<T>().Remove(entidade);
        _db.SaveChanges();
    }
}
