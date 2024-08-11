using System;
using System.Collections.Generic;

namespace IntegraMuni2023._01.Models;

public partial class Servicio
{
    public int ServicioId { get; set; }

    public string NombreServicio { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal Tarifa { get; set; }

    public string TipoTarifa { get; set; } = null!;

    public virtual ICollection<ServiciosContratado> ServiciosContratados { get; set; } = new List<ServiciosContratado>();
}
