using GameLocadora.Data;
using GameLocadora.Models;
using GameLocadora.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GameLocadora.Pages
{
    public class JogosModel : PageModel
    {
        public static readonly string[] ExtensoesPermitidas = { ".jpg", ".png", ".jpeg", ".webp" };

        public JogoRepositorio _rptJogo;
        public readonly IWebHostEnvironment _env;

        public JogosModel(LocadoraContext ctx, IWebHostEnvironment env)
        {
            _rptJogo = new JogoRepositorio(ctx);
            _env = env;

        }

        [BindProperty]
        [Required(ErrorMessage = "Informe o título")]
        [StringLength(120, ErrorMessage = "O tamanho máximo do título não pode ser maoir que 120 caracteres")]
        [Display(Name = "Titulo")]
        public string Titulo { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Informe a plataforma")]
        [StringLength(40, ErrorMessage = "O tamanho máximo da plataforma não pode ser maoir que 40 caracteres")]
        [Display(Name = "Plataforma")]
        public string Plataforma { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Informe o gênero")]
        [StringLength(40, ErrorMessage = "O tamanho máximo do gênero não pode ser maoir que 40 caracteres")]
        [Display(Name = "Gênero")]
        public string Genero { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Informe o valor da diária")]
        [Range(0.01, 1000, ErrorMessage = "A diária deve ser de R$0,01 a R$1000,00")]
        public decimal? ValorDiaria { get; set; }

        [BindProperty]
        [Display(Name = "Capa")]
        public IFormFile? Capa { get; set; }

        public List<Jogo> ListaJogos { get; set; } = [];


        public void OnGet()
        {
            ListaJogos = _rptJogo.ObterTodos();
        }

        public IActionResult OnPost()
        {
            string? nomeArquivo = null;

            if (Capa != null && Capa.Length > 0)
            {
                var extensao = Path.GetExtension(Capa.FileName).ToLowerInvariant();
                if (!ExtensoesPermitidas.Contains(extensao))
                {
                    ModelState.AddModelError(nameof(Capa), "A capa dever ser uma imagem JPG, PNG, ou WEBP");
                }
                else
                {
                    nomeArquivo = Guid.NewGuid().ToString("N") + extensao;
                }
            }
            if (!ModelState.IsValid)
            {
                ListaJogos = _rptJogo.ObterTodos();
                return Page();
            }

            var pastasCapas = Path.Combine(_env.WebRootPath, "capas");
            if (nomeArquivo != null)
            {
                Directory.CreateDirectory(pastasCapas);
                using var stream = System.IO.File.Create(Path.Combine(pastasCapas, nomeArquivo));
                Capa!.CopyTo(stream);
            }

            var jogo = new Jogo()
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
                _rptJogo.Adicionar(jogo);
            }
            catch
            {
                if (nomeArquivo != null)
                {
                    System.IO.File.Delete(Path.Combine(pastasCapas, nomeArquivo));
                }
                throw;
            }
            TempData["Mensagem"] = "Jogo salvo com sucesso";
            return RedirectToPage();
        }
    }



}

