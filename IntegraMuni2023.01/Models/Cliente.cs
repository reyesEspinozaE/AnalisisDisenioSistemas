using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IntegraMuni2023._01.Models
{
    public partial class Cliente
    {
        public int ClienteId { get; set; }

        [Display(Name = "Nombre Completo")]
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "La longitud del nombre debe estar entre 2 y 100 caracteres.")]
        public string? NombreCompleto { get; set; }

        [Display(Name = "Identificación")]
        [Required(ErrorMessage = "La identificación es obligatoria.")]
        public string? Identificacion { get; set; }

        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
        public string? CorreoElectronico { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string? Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El numero de telefono debe tener 8 digitos")]
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string? Estado { get; set; } = "Activo";

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 20 caracteres.")]
        public string? Clave { get; set; }

        [NotMapped]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Clave", ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmarClave { get; set; }

        public string? Rol { get; set; } = "Cliente";

        public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

        public virtual ICollection<ServiciosContratado> ServiciosContratados { get; set; } = new List<ServiciosContratado>();

        public virtual ICollection<Tramite> Tramites { get; set; } = new List<Tramite>();
    }
}
