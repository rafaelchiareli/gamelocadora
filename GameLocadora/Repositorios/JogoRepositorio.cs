using GameLocadora.Data;
using GameLocadora.Models;

namespace GameLocadora.Repositorios;

public interface IJogoRepositorio : IRepositorio<Jogo>
{
    List<Jogo> ObterDisponiveis();
}

public class JogoRepositorio : Repositorio<Jogo>, IJogoRepositorio
{
    public JogoRepositorio(LocadoraContext db) : base(db)
    {
    }

    // Jogos ativos que não estão locados no momento (sem locação em aberto).
    public List<Jogo> ObterDisponiveis()
    {
        return _db.Jogos
            .Where(j => j.Ativo
                && !_db.Locacaos.Any(l => l.JogoId == j.Id && l.DataDevolucao == null))
            .OrderBy(j => j.Titulo)
            .ToList();
    }
}
