using System;
using System.Collections.Generic;

namespace IntegraMuni2023._01.Models;

public partial class ServiciosContratado
{
    public int ServicioContratadoId { get; set; }

    public int ServicioId { get; set; }

    public int ClienteId { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFinalizacion { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal Consumo { get; set; }

    public string EstadoServicio { get; set; } = null!;

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual Servicio Servicio { get; set; } = null!;
}
