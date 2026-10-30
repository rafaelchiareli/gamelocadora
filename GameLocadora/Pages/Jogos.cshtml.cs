using System.ComponentModel.DataAnnotations;
using GameLocadora.Data;
using GameLocadora.Models;
using GameLocadora.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameLocadora.Pages
{
    public class JogosModel : PageModel
    {
        private const long TamanhoMaximoCapa = 2 * 1024 * 1024; // 2 MB
        private static readonly string[] ExtensoesPermitidas = { ".jpg", ".jpeg", ".png", ".webp" };

        private readonly IJogoRepositorio _jogos;
        private readonly IWebHostEnvironment _env;

        public JogosModel(LocadoraContext db, IWebHostEnvironment env)
        {
            _jogos = new JogoRepositorio(db);
            _env = env;
        }

        [BindProperty]
        [Required(ErrorMessage = "Informe o título.")]
        [StringLength(120, ErrorMessage = "O título deve ter no máximo 120 caracteres.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Informe a plataforma.")]
        [StringLength(40, ErrorMessage = "A plataforma deve ter no máximo 40 caracteres.")]
        [Display(Name = "Plataforma")]
        public string Plataforma { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Informe o gênero.")]
        [StringLength(40, ErrorMessage = "O gênero deve ter no máximo 40 caracteres.")]
        [Display(Name = "Gênero")]
        public string Genero { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Informe o valor da diária.")]
        [Range(0.01, 1000, ErrorMessage = "A diária deve ficar entre R$ 0,01 e R$ 1.000,00.")]
        [Display(Name = "Valor da diária (R$)")]
        public decimal? ValorDiaria { get; set; }

        [BindProperty]
        [Display(Name = "Capa (foto)")]
        public IFormFile? Capa { get; set; }

        public List<Jogo> ListaJogos { get; set; } = [];

        public void OnGet()
        {
            ListaJogos = _jogos.ObterTodos();
        }

        public IActionResult OnPost()
        {
            string? nomeArquivo = null;

            if (Capa != null && Capa.Length > 0)
            {
                var extensao = Path.GetExtension(Capa.FileName).ToLowerInvariant();

                if (!ExtensoesPermitidas.Contains(extensao))
                    ModelState.AddModelError(nameof(Capa), "A capa deve ser uma imagem JPG, PNG ou WEBP.");
                else if (Capa.Length > TamanhoMaximoCapa)
                    ModelState.AddModelError(nameof(Capa), "A capa deve ter no máximo 2 MB.");
                else
                    nomeArquivo = Guid.NewGuid().ToString("N") + extensao;
            }

            if (!ModelState.IsValid)
            {
                ListaJogos = _jogos.ObterTodos();
                return Page();
            }

            var pastaCapas = Path.Combine(_env.WebRootPath, "capas");

            if (nomeArquivo != null)
            {
                Directory.CreateDirectory(pastaCapas);
                using var stream = System.IO.File.Create(Path.Combine(pastaCapas, nomeArquivo));
                Capa!.CopyTo(stream);
            }

            var jogo = new Jogo
            {
                Titulo = Titulo.Trim(),
                Plataforma = Plataforma.Trim(),
                Genero = Genero.Trim(),
                ValorDiariaCentavos = (int)Math.Round(ValorDiaria!.Value * 100, MidpointRounding.AwayFromZero),
                CapaArquivo = nomeArquivo,
                Ativo = true
            };

            try
            {
                _jogos.Adicionar(jogo);
            }
            catch
            {
                // Se não salvou no banco, não deixa a foto órfã na pasta.
                if (nomeArquivo != null)
                    System.IO.File.Delete(Path.Combine(pastaCapas, nomeArquivo));
                throw;
            }

            TempData["Mensagem"] = "Jogo salvo com sucesso!";
            return RedirectToPage();
        }
    }
}
