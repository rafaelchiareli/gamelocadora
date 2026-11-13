using GameLocadora.Data;
using GameLocadora.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLocadora.Repositorios;

public interface ILocacaoRepositorio : IRepositorio<Locacao>
{
    List<Locacao> ObterTodosComDetalhes();

    bool JogoEstaLocado(int jogoId);

    List<Locacao> ObterPorCliente(int clienteId);
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

    // Todas as locações de um cliente, da mais recente para a mais antiga, já com o Jogo.
    public List<Locacao> ObterPorCliente(int clienteId)
    {
        return _db.Locacaos
            .Include(l => l.Jogo)
            .Where(l => l.ClienteId == clienteId)
            .OrderByDescending(l => l.DataLocacao)
            .ToList();
    }

    // O jogo está locado se existe uma locação dele ainda sem devolução.
    public bool JogoEstaLocado(int jogoId)
    {
        return _db.Locacaos.Any(l => l.JogoId == jogoId && l.DataDevolucao == null);
    }
}
