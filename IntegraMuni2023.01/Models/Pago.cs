using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntegraMuni2023._01.Models;

public partial class Pago
{
    public int PagoId { get; set; }

    public int ServicioContratadoId { get; set; }

    public int ClienteId { get; set; }

    [Required(ErrorMessage = "El campo Identificación es obligatorio.")]
    public string Identificacion { get; set; } = null!;

    [Display(Name = "Monto del Pago")]
    public decimal MontoPago
    {
        get
        {
            // Asumiendo que tienes acceso a los modelos relacionados
            decimal tarifaServicio = this.ServicioContratado?.Servicio?.Tarifa ?? 0;
            decimal consumoServicio = this.ServicioContratado?.Consumo ?? 0;

            return tarifaServicio * consumoServicio;
        } set { }
    }

    public DateTime FechaPago { get; set; } = DateTime.Now;

    public string MetodoPago { get; set; } = "Tarjeta";

    public string EstadoPago { get; set; } = "Pagado";

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ServiciosContratado ServicioContratado { get; set; } = null!;
}
