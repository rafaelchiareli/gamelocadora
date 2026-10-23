using GameLocadora.Data;
using GameLocadora.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameLocadora.Pages
{
    public class ClientesModel : PageModel
    {
        private readonly LocadoraContext _db;

        public ClientesModel(LocadoraContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Cliente OCliente { get; set; } = new();

        public List<Cliente> ListaCllientes { get; set; } = [];
        public void OnGet()
        {
            ListaCllientes = _db.Clientes.ToList();

        }

        public IActionResult OnPost()
        {
            if (! ModelState.IsValid) return Page();

            _db.Clientes.Add(OCliente);
            _db.SaveChanges();
            TempData["Mensagem"] = "Cliente salvo com sucesso!";
            return Page();
        }
      
       

    }
}
