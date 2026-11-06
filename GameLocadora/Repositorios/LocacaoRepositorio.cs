using GameLocadora.Data;
using GameLocadora.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLocadora.Repositorios;

public interface ILocacaoRepositorio : IRepositorio<Locacao>
{
    List<Locacao> ObterTodosComDetalhes();

    bool JogoEstaLocado(int jogoId);
}

public class LocacaoRepositorio : Repositorio<Locacao>, ILocacaoRepositorio
{
    public LocacaoRepositorio(LocadoraContext db) : base(db)
    {
    }

    // Traz a locação já com o Cliente e o Jogo, para mostrar os nomes na tela.
    public List<Locacao> ObterTodosComDetalhes()
    {
        return _db.Locacaos
            .Include(l => l.Cliente)
            .Include(l => l.Jogo)
            .OrderByDescending(l => l.DataLocacao)
            .ToList();
    }

    // O jogo está locado se existe uma locação dele ainda sem devolução.
    public bool JogoEstaLocado(int jogoId)
    {
        return _db.Locacaos.Any(l => l.JogoId == jogoId && l.DataDevolucao == null);
    }
}
