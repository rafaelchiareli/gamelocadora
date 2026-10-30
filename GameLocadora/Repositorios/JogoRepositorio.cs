using GameLocadora.Data;
using GameLocadora.Models;

namespace GameLocadora.Repositorios;

public interface IJogoRepositorio : IRepositorio<Jogo>
{
}

public class JogoRepositorio : Repositorio<Jogo>, IJogoRepositorio
{
    public JogoRepositorio(LocadoraContext db) : base(db)
    {
    }
}
