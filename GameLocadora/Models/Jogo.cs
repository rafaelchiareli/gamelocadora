using System;
using System.Collections.Generic;

namespace GameLocadora.Models;

public partial class Jogo
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Plataforma { get; set; } = null!;

    public string Genero { get; set; } = null!;

    public int ValorDiariaCentavos { get; set; }

    public string? CapaArquivo { get; set; }

    public bool Ativo { get; set; }

    public virtual Locacao? Locacao { get; set; }
}
