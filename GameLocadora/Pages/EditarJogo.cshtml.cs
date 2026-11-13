using GameLocadora.Data;
using GameLocadora.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace GameLocadora.Pages
{
    public class EditarJogoModel : PageModel
    {
        public static readonly string[] ExtensoesPermitidas = { ".jpg", ".png", ".jpeg", ".webp" };

        public JogoRepositorio _rptJogo;
        public readonly IWebHostEnvironment _env;
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
        [Display(Name = "Jogo ativo")]
        public bool Ativo { get; set; }

        [BindProperty]
        [Display(Name = "Nova Capa")]
        public IFormFile? Capa { get; set; }

        public string? CapaAtual { get; set; }
        public EditarJogoModel(LocadoraContext ctx, IWebHostEnvironment env)
        {
            _rptJogo = new JogoRepositorio(ctx);
            _env = env;
        }

        public IActionResult OnGet(int id)
        {
            var jogo = _rptJogo.ObterPorId(id);
            if (jogo == null) return NotFound();
            Titulo = jogo.Titulo;
            Plataforma = jogo.Plataforma;
            Genero = jogo.Genero;
            ValorDiaria = jogo.ValorDiariaCentavos / 100m;
            Ativo = jogo.Ativo;
            CapaAtual = jogo.CapaArquivo;
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var jogo = _rptJogo.ObterPorId(id);
            string? nomeArquivo = null;
            if (jogo == null) return NotFound();
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
            if (!ModelState.IsValid) return Page();
            var pastasCapas = Path.Combine(_env.WebRootPath, "capas");
            if (nomeArquivo != null)
            {
                Directory.CreateDirectory(pastasCapas);
                using var stream = System.IO.File.Create(Path.Combine(pastasCapas, nomeArquivo));
                Capa!.CopyTo(stream);
            }
            var capaAntiga = jogo.CapaArquivo;
            jogo.Titulo = Titulo.Trim();
            jogo.Genero = Genero.Trim();
            jogo.Plataforma = Plataforma.Trim();
            jogo.ValorDiariaCentavos = (int)Math.Round(ValorDiaria!.Value * 100, MidpointRounding.AwayFromZero);
            jogo.Ativo = Ativo;

            if (nomeArquivo != null)
            {
                jogo.CapaArquivo = nomeArquivo; 
            }
            try
            {
                _rptJogo.Atualizar(jogo);
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
