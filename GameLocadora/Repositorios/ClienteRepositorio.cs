using GameLocadora.Data;
using GameLocadora.Models;

namespace GameLocadora.Repositorios;

public interface IClienteRepositorio : IRepositorio<Cliente>
{
    List<Cliente> ObterAtivos();
}

public class ClienteRepositorio : Repositorio<Cliente>, IClienteRepositorio
{
    public ClienteRepositorio(LocadoraContext db) : base(db)
    {
    }

    public List<Cliente> ObterAtivos()
    {
        return _db.Clientes
            .Where(c => c.Ativo)
            .OrderBy(c => c.Nome)
            .ToList();
    }
}
