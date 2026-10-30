using GameLocadora.Data;
using GameLocadora.Models;

namespace GameLocadora.Repositorios;

public interface ILocacaoRepositorio : IRepositorio<Locacao>
{
}

public class LocacaoRepositorio : Repositorio<Locacao>, ILocacaoRepositorio
{
    public LocacaoRepositorio(LocadoraContext db) : base(db)
    {
    }
}
