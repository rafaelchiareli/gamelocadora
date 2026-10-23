using System;
using System.Collections.Generic;

namespace GameLocadora.Models;

public partial class Locacao
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public int JogoId { get; set; }

    public DateTime DataLocacao { get; set; }

    public DateTime DataPrevistaDevolucao { get; set; }

    public DateTime? DataDevolucao { get; set; }

    public int ValorDiariaCentavos { get; set; }

    public int? ValorTotalCentavos { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual Jogo Jogo { get; set; } = null!;
}
