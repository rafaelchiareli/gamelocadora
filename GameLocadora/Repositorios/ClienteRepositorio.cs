using GameLocadora.Data;
using GameLocadora.Models;

namespace GameLocadora.Repositorios;

public interface IClienteRepositorio : IRepositorio<Cliente>
{
}

public class ClienteRepositorio : Repositorio<Cliente>, IClienteRepositorio
{
    public ClienteRepositorio(LocadoraContext db) : base(db)
    {
    }
}
