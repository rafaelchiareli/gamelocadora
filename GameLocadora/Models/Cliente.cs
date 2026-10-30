using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameLocadora.Models;

public partial class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage= "Campo Obrigatório")]
    [MinLength(3, ErrorMessage ="O Campo nome deve possuir no mínimo 3 caracteres")]
    [MaxLength(50, ErrorMessage ="O Campo nome deve possuir no máximo 50 caracteres")]

    public string Nome { get; set; } = null!;
    [Required(ErrorMessage = "Campo Obrigatório")]
    [MinLength(5, ErrorMessage = "O Campo nome deve possuir no mínimo 5 caracteres")]
    [MaxLength(50, ErrorMessage = "O Campo nome deve possuir no máximo 50 caracteres")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Campo Obrigatório")]
    [MinLength(9, ErrorMessage = "O Campo nome deve possuir no mínimo 9 caracteres")]
    [MaxLength(14, ErrorMessage = "O Campo nome deve possuir no máximo 50 caracteres")]
    public string Telefone { get; set; } = null!;

    public bool Ativo { get; set; }

    public virtual ICollection<Locacao> Locacaos { get; set; } = new List<Locacao>();
}
