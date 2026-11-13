using System.Globalization;
using GameLocadora.Data;
using GameLocadora.Models;
using GameLocadora.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameLocadora.Pages
{
    public class PerfilModel : PageModel
    {
        private static readonly CultureInfo PtBr = new("pt-BR");

        private readonly IClienteRepositorio _rptCliente;
        private readonly ILocacaoRepositorio _rptLocacao;

        public PerfilModel(LocadoraContext db)
        {
            _rptCliente = new ClienteRepositorio(db);
            _rptLocacao = new LocacaoRepositorio(db);
        }

        public Cliente OCliente { get; set; } = null!;

        public List<Locacao> ListaLocacoes { get; set; } = [];

        // Resumo mostrado nos cartões do topo da página.
        public int TotalLocacoes => ListaLocacoes.Count;

        public int EmAberto => ListaLocacoes.Count(l => l.DataDevolucao == null);

        public int Atrasadas => ListaLocacoes.Count(EstaAtrasada);

        // Só locações já devolvidas têm o total calculado.
        public int TotalGastoCentavos => ListaLocacoes.Sum(l => l.ValorTotalCentavos ?? 0);

        public IActionResult OnGet(int id)
        {
            var cliente = _rptCliente.ObterPorId(id);
            if (cliente == null) return NotFound();

            OCliente = cliente;
            ListaLocacoes = _rptLocacao.ObterPorCliente(id);

            return Page();
        }

        // Atrasada: ainda não foi devolvida e já passou da data prevista.
        public static bool EstaAtrasada(Locacao l)
        {
            return l.DataDevolucao == null && l.DataPrevistaDevolucao < DateTime.Now;
        }

        public static string Situacao(Locacao l)
        {
            if (l.DataDevolucao != null) return "Devolvida";
            return EstaAtrasada(l) ? "Atrasada" : "Em aberto";
        }

        public static string ClasseSituacao(Locacao l)
        {
            return Situacao(l) switch
            {
                "Devolvida" => "bg-success",
                "Atrasada" => "bg-danger",
                _ => "bg-warning text-dark"
            };
        }

        // O banco guarda dinheiro em centavos; aqui vira "R$ 12,50".
        public static string Moeda(int centavos)
        {
            return (centavos / 100m).ToString("C", PtBr);
        }
    }
}
