using System;
using System.Collections.Generic;

namespace GameLocadora.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public bool Ativo { get; set; }

    public virtual ICollection<Locacao> Locacaos { get; set; } = new List<Locacao>();
}
