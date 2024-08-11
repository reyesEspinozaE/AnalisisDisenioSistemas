using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegraMuni2023._01.Models;
using Microsoft.AspNetCore.Authorization;

namespace IntegraMuni2023._01.Controllers
{
    [Authorize]
    public class ServiciosContratadoesController : Controller
    {
        private readonly IntegraMuni2023Context _context;

        public ServiciosContratadoesController(IntegraMuni2023Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var correoCliente = HttpContext.User.Identity.Name;
            var cliente = await _context.Clientes.FirstOrDefaultAsync(u => u.CorreoElectronico == correoCliente);

            if (cliente != null)
            {
                var servicios = await _context.ServiciosContratados
                    .Where(sc => sc.ClienteId == cliente.ClienteId)
                    .Include(sc => sc.Servicio)
                    .ToListAsync();
                // Hacer lo que necesites con la lista de servicios pendientes obtenida
                return View(servicios.ToList());
            }

            // En caso de que el cliente sea nulo, puedes redirigir a otra acción o mostrar una vista diferente.
            return RedirectToAction("Index","Home");
        }

        

        // GET: ServiciosContratadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviciosContratado = await _context.ServiciosContratados
                .Include(s => s.Cliente)
                .Include(s => s.Servicio)
                .FirstOrDefaultAsync(m => m.ServicioContratadoId == id);
            if (serviciosContratado == null)
            {
                return NotFound();
            }

            return View(serviciosContratado);
        }

        // GET: ServiciosContratadoes/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Clave");
            ViewData["ServicioId"] = new SelectList(_context.Servicios, "ServicioId", "ServicioId");
            return View();
        }

        // POST: ServiciosContratadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServicioContratadoId,ServicioId,ClienteId,FechaInicio,FechaFinalizacion,Descripcion,Consumo,EstadoServicio")] ServiciosContratado serviciosContratado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviciosContratado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Clave", serviciosContratado.ClienteId);
            ViewData["ServicioId"] = new SelectList(_context.Servicios, "ServicioId", "ServicioId", serviciosContratado.ServicioId);
            return View(serviciosContratado);
        }

        // GET: ServiciosContratadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviciosContratado = await _context.ServiciosContratados.FindAsync(id);
            if (serviciosContratado == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Clave", serviciosContratado.ClienteId);
            ViewData["ServicioId"] = new SelectList(_context.Servicios, "ServicioId", "ServicioId", serviciosContratado.ServicioId);
            return View(serviciosContratado);
        }

        // POST: ServiciosContratadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServicioContratadoId,ServicioId,ClienteId,FechaInicio,FechaFinalizacion,Descripcion,Consumo,EstadoServicio")] ServiciosContratado serviciosContratado)
        {
            if (id != serviciosContratado.ServicioContratadoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviciosContratado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiciosContratadoExists(serviciosContratado.ServicioContratadoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Clave", serviciosContratado.ClienteId);
            ViewData["ServicioId"] = new SelectList(_context.Servicios, "ServicioId", "ServicioId", serviciosContratado.ServicioId);
            return View(serviciosContratado);
        }

        // GET: ServiciosContratadoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviciosContratado = await _context.ServiciosContratados
                .Include(s => s.Cliente)
                .Include(s => s.Servicio)
                .FirstOrDefaultAsync(m => m.ServicioContratadoId == id);
            if (serviciosContratado == null)
            {
                return NotFound();
            }

            return View(serviciosContratado);
        }

        // POST: ServiciosContratadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviciosContratado = await _context.ServiciosContratados.FindAsync(id);
            if (serviciosContratado != null)
            {
                _context.ServiciosContratados.Remove(serviciosContratado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiciosContratadoExists(int id)
        {
            return _context.ServiciosContratados.Any(e => e.ServicioContratadoId == id);
        }
    }
}
