using System;
using System.ComponentModel.DataAnnotations;

namespace IntegraMuni2023._01.Models
{
    public partial class Tramite
    {
        public int TramiteId { get; set; }

        public int ClienteId { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(int.MaxValue, ErrorMessage = "La descripción no puede estar vacía.")]
        public string Descripcion { get; set; } = null!;
        public DateTime FechaInicio { get; set; } = DateTime.Now;

        public DateTime FechaFinalizacion { get; set; }
        public string PagoPendiente { get; set; } = "Listo";

        [Required(ErrorMessage = "El campo Formulario de Pago Completado es obligatorio.")]
        [StringLength(5, ErrorMessage = "El campo Formulario de Pago Completado debe tener una longitud máxima de 5 caracteres.")]
        public string FormularioPagoCompletado { get; set; } = "Listo";
        public string Identificacion { get; set; } = null!;

        public string EstadoSolicitud { get; set; } = "En proceso";

        public virtual Cliente Cliente { get; set; } = null!;
    }
}
