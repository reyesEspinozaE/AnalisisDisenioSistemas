using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntegraMuni2023._01.Models;

public partial class Tarea
{
    public int TareaId { get; set; }

    [Required(ErrorMessage = "El campo 'Titulo' es requerido.")]
    [StringLength(255, ErrorMessage = "El campo 'Titulo' no puede tener más de 255 caracteres.")]
    public string? Titulo { get; set; }

    [Required(ErrorMessage = "El campo 'Descripcion' es requerido.")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El campo 'Prioridad' es requerido.")]
    public string? Prioridad { get; set; }

    [Required(ErrorMessage = "El campo 'Fecha de Inicio' es requerido.")]
    public DateTime? FechaInicio { get; set; }

    [Required(ErrorMessage = "El campo 'Fecha de Fin' es requerido.")]
    public DateTime? FechaFin { get; set; }

    [Required(ErrorMessage = "Debes seleccionar un empleado.")]
    public int? EmpleadoId { get; set; }

    public string? Estado { get; set; } = "En proceso";

    public virtual Empleado? Empleado { get; set; }
}
