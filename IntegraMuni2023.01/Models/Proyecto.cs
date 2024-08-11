using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntegraMuni2023._01.Models;

public partial class Proyecto
{
    public int NoticiaId { get; set; }

    [Required(ErrorMessage = "El título de la noticia es requerido.")]
    [StringLength(300, ErrorMessage = "El título no puede exceder los 300 caracteres.")]
    public string TituloNoticia { get; set; } = null!;

    [Required(ErrorMessage = "La descripción de la noticia es requerida.")]
    public string Desarrollo { get; set; } = null!;

    [Required(ErrorMessage = "El nivel de prioridad es requerido.")]
    [StringLength(20, ErrorMessage = "El nivel de prioridad no puede exceder los 20 caracteres.")]
    public string NivelPrioridad { get; set; } = null!;

    [Required(ErrorMessage = "La fecha de publicación es requerida.")]
    [DataType(DataType.Date)]
    public DateTime FechaPublicacion { get; set; } = DateTime.Now; // Establecer la fecha actual como valor por defecto

    [Required(ErrorMessage = "El estado de la noticia es requerido.")]
    [StringLength(30, ErrorMessage = "El estado no puede exceder los 30 caracteres.")]
    public string Estado { get; set; } = "Activa";
}
