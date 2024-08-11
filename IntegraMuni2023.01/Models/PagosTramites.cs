using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntegraMuni2023._01.Models
{
    public class PagosTramites
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PagoTramiteID { get; set; }

        [DisplayName("ID del Trámite")]
        public int TramiteID { get; set; }

        [DisplayName("ID del Cliente")]
        public int ClienteID { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [DisplayName("Fecha de Pago")]
        public DateTime FechaPago { get; set; } = DateTime.Now;

        [DisplayName("Monto del Pago")]
        public decimal MontoPago { get; set; } = 100000;

        [DisplayName("Pago Pendiente")]
        public string PagoPendiente { get; set; } = "Listo";

        [DisplayName("Formulario de Pago Completado")]
        public string FormularioPagoCompletado { get; set; } = "Listo";

        [DisplayName("Identificación")]
        public string Identificacion { get; set; }

        [DisplayName("Método de Pago")]
        public string MetodoPago { get; set; } = "Tarjeta";

        [DisplayName("Correo Electrónico")]
        public string CorreoElectronico { get; set; }

        [DisplayName("Nombre Completo en la Tarjeta")]
        public string NombreCompletoTarjeta { get; set; }
    }
}
