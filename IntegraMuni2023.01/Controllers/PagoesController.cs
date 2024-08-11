using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegraMuni2023._01.Models;
using System.Security.Claims;

namespace IntegraMuni2023._01.Controllers
{
    public class PagoesController : Controller
    {
        private readonly IntegraMuni2023Context _context;

        public PagoesController(IntegraMuni2023Context context)
        {
            _context = context;
        }

        // GET: Pagoes
        public async Task<IActionResult> Index()
        {
            var cliente = _context.Clientes.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);

            if (cliente != null)
            {
                var serviciosPendientes = await _context.ServiciosContratados
                    .Include(sc => sc.Servicio) // Incluir la información del servicio asociado
                    .Where(sc => sc.ClienteId == cliente.ClienteId && sc.EstadoServicio == "Pendiente")
                    .ToListAsync();

                return View(serviciosPendientes);
            }
            else
            {
                var serviciosPendientes = await _context.ServiciosContratados
                    .Include(sc => sc.Servicio) // Incluir la información del servicio asociado
                    .ToListAsync();

                return View(serviciosPendientes);
            }
        }

        // GET: Pagoes/Create
        public async Task<IActionResult> Create()
        {
            var cliente = _context.Clientes.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);
            var servicioContratado = _context.ServiciosContratados.FirstOrDefault(s => s.ClienteId == cliente.ClienteId);
            var servicio = _context.Servicios.FirstOrDefault(s => s.ServicioId == servicioContratado.ServicioId);
            if (cliente != null)
            {
                ViewData["ClienteIdentificacion"] = cliente.Identificacion;
                ViewData["ClienteNombre"] = cliente.NombreCompleto;
                ViewData["ClienteId"] = cliente.ClienteId;

                //ViewData["ServicioContratadoId"] = new SelectList(_context.ServiciosContratados);
                ViewData["ServicioContratadoId"] = servicioContratado.ServicioContratadoId;

                // Calcular MontoPago
                decimal tarifaServicio = servicio?.Tarifa ?? 0; // Corregir esta línea para obtener directamente la tarifa
                decimal consumoServicio = servicioContratado?.Consumo ?? 0;
                decimal montoPago = tarifaServicio * consumoServicio;
                decimal numeroRedondeado = Math.Round(montoPago, 0);

                // Asignar el valor a ViewBag
                ViewBag.MontoPago = numeroRedondeado;
                ViewBag.MetodoPago = "Tarjeta"; // Valor por defecto para Método de Pago

                return View();
            }
            return RedirectToAction("Index", "Home");
        }


        // POST: Pagoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PagoId,ServicioContratadoId,ClienteId,Identificacion,MontoPago,FechaPago,MetodoPago,EstadoPago")] Pago pago)
        {
            var servicioContratadoPaga = _context.ServiciosContratados.FirstOrDefault(s => s.ClienteId == pago.ClienteId);
            pago.ServicioContratado = servicioContratadoPaga;
            var clientePaga = _context.Clientes.FirstOrDefault(s => s.ClienteId == pago.ClienteId);
            pago.Cliente = clientePaga;
            var servicioPaga = _context.Servicios.FirstOrDefault(s => s.ServicioId == servicioContratadoPaga.ServicioId);
            pago.Cliente = clientePaga;

            if (pago != null)
            {
                // Agregar el pago
                _context.Add(pago);
                await _context.SaveChangesAsync();

                // Actualizar el estado del servicio contratado a "Pagado"
                var serviciosContratado = await _context.ServiciosContratados.FindAsync(pago.ServicioContratadoId);
                if (serviciosContratado != null)
                {
                    serviciosContratado.EstadoServicio = "Pagado";
                    _context.Update(serviciosContratado);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "¡Pago realizado con exito!";

                            Email email = new Email();
                            email.Aprobado(clientePaga,pago,servicioPaga);

                    ViewBag.ShowSuccessMessage = true;
                }
            }
            var cliente = _context.Clientes.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);
            var servicioContratado = _context.ServiciosContratados.FirstOrDefault(s => s.ClienteId == cliente.ClienteId);
            var servicio = _context.Servicios.FirstOrDefault(s => s.ServicioId == servicioContratado.ServicioId);
            decimal tarifaServicio = servicioContratado?.Servicio?.Tarifa ?? 0;
            decimal consumoServicio = servicioContratado?.Consumo ?? 0;
            decimal montoPago = tarifaServicio * consumoServicio;
            // Si el modelo no es válido, recargamos los datos en la vista y asignamos valores a ViewBag
            ViewData["ClienteId"] = cliente.ClienteId;
            ViewData["ServicioContratadoId"] = servicioContratado.ServicioContratadoId;
            ViewBag.MontoPago = montoPago;
            ViewBag.MetodoPago = "Tarjeta"; // Valor por defecto para Método de Pago

            return View(pago);
        }
        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.PagoId == id);
        }
    }
}
