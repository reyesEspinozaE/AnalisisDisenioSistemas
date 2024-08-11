using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegraMuni2023._01.Models;
using Newtonsoft.Json;

namespace IntegraMuni2023._01.Controllers
{
    public class TramitesController : Controller
    {
        private readonly IntegraMuni2023Context _context;

        public TramitesController(IntegraMuni2023Context context)
        {
            _context = context;
        }

        // GET: Tramites
        public async Task<IActionResult> Index()
        {
            var cliente = _context.Clientes.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);
            if (User.IsInRole("Administrador") || User.IsInRole("Empleado") || User.IsInRole("Jefe"))
            {
                var integraMuni2023Context = _context.Tramites.Include(t => t.Cliente);
                return View(await integraMuni2023Context.ToListAsync());
            }
            else
            {
                var serviciosPendientes = await _context.Tramites
                    .Include(t => t.Cliente) // Incluir la información del cliente
                    .Where(sc => sc.ClienteId == cliente.ClienteId)
                    .ToListAsync();

                return View(serviciosPendientes);
            }
        }

        // GET: Tramites/Create
        public IActionResult Create()
        {
            var cliente = _context.Clientes.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);
            if (cliente != null) 
            {
                ViewData["ClienteId"] = cliente.ClienteId;
                return View();
            }
           return RedirectToAction("Index","Home");
        }

        // POST: Tramites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TramiteId,ClienteId,Descripcion,FechaInicio,FechaFinalizacion,PagoPendiente,FormularioPagoCompletado,Identificacion, EstadoSolicitud")] Tramite tramite)
        {
            var cliente = _context.Clientes.FirstOrDefault(s => s.ClienteId == tramite.ClienteId);
            tramite.Cliente = cliente;
            tramite.FechaFinalizacion = tramite.FechaInicio.AddDays(15);
            tramite.Identificacion = cliente.Identificacion;
            if (tramite!=null)
            {
                _context.Add(tramite);
                await _context.SaveChangesAsync();

                var lastTramite = _context.Tramites.OrderByDescending(t => t.TramiteId).FirstOrDefault();
                int lastTramiteId = lastTramite?.TramiteId ?? 0;
                TempData["LastTramiteId"] = lastTramiteId;

                TempData["SuccessMessage"] = "Continue con el Proceso de Pago del tramite.";
                ViewBag.ShowSuccessMessage = true;
            }
            var clienteElse = _context.Clientes.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);
            ViewData["ClienteId"] = clienteElse.ClienteId;
            return View(tramite);
        }
        
        public IActionResult ProcesarPago(int tramiteId)
        {
            var tramite = _context.Tramites.FirstOrDefault(t => t.TramiteId == tramiteId);
            var pagosTramites = new PagosTramites
            {
                Identificacion = tramite.Identificacion,
                Descripcion = tramite.Descripcion,
                TramiteID = tramite.TramiteId,
                ClienteID = tramite.ClienteId
            };
            return View(pagosTramites);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcesarPago([Bind("TramiteID,ClienteID,Descripcion,FechaInicio,FechaFinalizacion,PagoPendiente,FormularioPagoCompletado,Identificacion, CorreoElectronico, NombreCompletoTarjeta")] PagosTramites pagosTramites)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var cliente = _context.Clientes.FirstOrDefault(t => t.ClienteId == pagosTramites.ClienteID);

                    _context.PagosTramites.Add(pagosTramites);
                    _context.SaveChanges();
                    Email email = new Email();
                    email.TramiteSolicitado(cliente, pagosTramites);

                    TempData["SuccessMessage"] = "Proceso de Pago finalizado con exito";
                    ViewBag.ShowSuccessMessage = true;

                }
                catch (Exception ex)
                {
                    TempData["Mensaje"] = "Error al procesar el pago: " + ex.Message;
                }
            }

            return View(pagosTramites);
        }

        // POST: Tramites/ChangeStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstadoDePago(int id, string newStatus)
        {
            var tramite = await _context.Tramites.FindAsync(id);
            if (tramite == null)
            {
                return NotFound();
            }
            tramite.EstadoSolicitud = newStatus; // Actualiza el estado de la solicitud
            try
            {
                await _context.SaveChangesAsync();
                var cliente = _context.Clientes.FirstOrDefault(t => t.ClienteId == tramite.ClienteId);
                Email email = new Email();
                if (newStatus.Equals("Habilitado"))
                {
                    email.TramiteAprobado(cliente, tramite);
                }
                else if (newStatus.Equals("Inhabilitado"))
                {
                    email.TramiteDenegado(cliente, tramite);
                }
                TempData["SuccessMessage"] = "Tramite Procesado";
                ViewBag.ShowSuccessMessage = true;

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TramiteExists(tramite.TramiteId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool TramiteExists(int id)
        {
            return _context.Tramites.Any(e => e.TramiteId == id);
        }
    }
}
