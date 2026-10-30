using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameLocadora.Pages
{
    public class OlaModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? Nome { get; set; }
        public string Mensagem { get; private set; } = "";
        public DateTime Hora { get; private set; }
        public void OnGet()
        {
            Mensagem = string.IsNullOrWhiteSpace(Nome)
                    ? "Olá Razor Pages"
                    : $"Olá,{Nome}";
            Hora  = DateTime.Now;
        }
    }
}
