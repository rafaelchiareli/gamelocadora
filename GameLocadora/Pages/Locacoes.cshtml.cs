using System.ComponentModel.DataAnnotations;
using GameLocadora.Data;
using GameLocadora.Models;
using GameLocadora.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameLocadora.Pages
{
    public class LocacoesModel : PageModel
    {
        private readonly IClienteRepositorio _clientes;
        private readonly IJogoRepositorio _jogos;
        private readonly ILocacaoRepositorio _locacoes;

        public LocacoesModel(LocadoraContext db)
        {
            _clientes = new ClienteRepositorio(db);
            _jogos = new JogoRepositorio(db);
            _locacoes = new LocacaoRepositorio(db);
        }

        [BindProperty]
        [Required(ErrorMessage = "Selecione o cliente.")]
        [Display(Name = "Cliente")]
        public int? ClienteId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Selecione o jogo.")]
        [Display(Name = "Jogo")]
        public int? JogoId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Informe a quantidade de dias.")]
        [Range(1, 30, ErrorMessage = "A locação deve ter de 1 a 30 dias.")]
        [Display(Name = "Dias de locação")]
        public int? Dias { get; set; }

        // A combo de clientes é montada aqui no servidor.
        // A combo de jogos NÃO: ela é carregada por JSON (veja OnGetJogos).
        public SelectList Clientes { get; set; } = null!;

        public List<Locacao> ListaLocacoes { get; set; } = [];

        private void Carregar()
        {
            Clientes = new SelectList(_clientes.ObterAtivos(), nameof(Cliente.Id), nameof(Cliente.Nome));
            ListaLocacoes = _locacoes.ObterTodosComDetalhes();
        }

        public void OnGet()
        {
            Carregar();
        }

        // Chamado pelo JavaScript em /Locacoes?handler=Jogos
        // Devolve só os jogos que podem ser locados agora, em JSON.
        public JsonResult OnGetJogos()
        {
            var jogos = _jogos.ObterDisponiveis()
                .Select(j => new
                {
                    id = j.Id,
                    titulo = j.Titulo,
                    plataforma = j.Plataforma,
                    valorDiariaCentavos = j.ValorDiariaCentavos
                })
                .ToList();

            return new JsonResult(jogos);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Carregar();
                return Page();
            }

            // Nunca confie nos dados que vieram do navegador: confere tudo no banco de novo.
            var cliente = _clientes.ObterPorId(ClienteId!.Value);
            if (cliente == null || !cliente.Ativo)
                ModelState.AddModelError(nameof(ClienteId), "Cliente inválido ou inativo.");

            var jogo = _jogos.ObterPorId(JogoId!.Value);
            if (jogo == null || !jogo.Ativo)
                ModelState.AddModelError(nameof(JogoId), "Jogo inválido ou inativo.");
            else if (_locacoes.JogoEstaLocado(jogo.Id))
                ModelState.AddModelError(nameof(JogoId), "Esse jogo já está locado.");

            if (!ModelState.IsValid)
            {
                Carregar();
                return Page();
            }

            var agora = DateTime.Now;

            var locacao = new Locacao
            {
                ClienteId = cliente!.Id,
                JogoId = jogo!.Id,
                DataLocacao = agora,
                DataPrevistaDevolucao = agora.AddDays(Dias!.Value),
                // O valor da diária é copiado do jogo no momento da locação,
                // assim um reajuste futuro não muda locações antigas.
                ValorDiariaCentavos = jogo.ValorDiariaCentavos
            };

            try
            {
                _locacoes.Adicionar(locacao);
            }
            catch (DbUpdateException)
            {
                // Duas pessoas locaram o mesmo jogo ao mesmo tempo: o índice único do banco barrou a segunda.
                ModelState.AddModelError(nameof(JogoId), "Esse jogo acabou de ser locado por outra pessoa.");
                Carregar();
                return Page();
            }

            TempData["Mensagem"] = "Locação registrada com sucesso!";
            return RedirectToPage();
        }
    }
}
