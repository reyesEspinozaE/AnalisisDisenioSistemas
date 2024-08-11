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
    //[Authorize]
    public class ProyectoesController : Controller
    {
        private readonly IntegraMuni2023Context _context;

        public ProyectoesController(IntegraMuni2023Context context)
        {
            _context = context;
        }

        // GET: Proyectoes
        public async Task<IActionResult> Index(string? filterType)
        {
            IEnumerable<Proyecto> proyectos = null;

            DateTime currentDate = DateTime.Now;
            DateTime threeMonthsAgo = currentDate.AddMonths(-3);
            DateTime oneYearAgo = currentDate.AddYears(-1);

            if (filterType == "recent")
            {
                proyectos = await _context.Proyectos.Where(p => p.FechaPublicacion > threeMonthsAgo && p.Estado == "Activa").ToListAsync();
                if (!proyectos.Any())
                {
                    TempData["ErrorMessage"] = "No se encontraron noticias recientes.";
                }
            }
            else if (filterType == "threeMonths")
            {
                proyectos = await _context.Proyectos.Where(p => p.FechaPublicacion <= threeMonthsAgo && p.FechaPublicacion > oneYearAgo && p.Estado == "Activa").ToListAsync();
                if (!proyectos.Any())
                {
                    TempData["ErrorMessage"] = "No se encontraron noticias con este filtro.";
                }
            }
            else if (filterType == "oneYear")
            {
                proyectos = await _context.Proyectos.Where(p => p.FechaPublicacion <= oneYearAgo && p.Estado == "Activa").ToListAsync();
                if (!proyectos.Any())
                {
                    TempData["ErrorMessage"] = "No se encontraron noticias con este filtro.";
                }
            }
            else if (filterType == null)
            {
                proyectos = await _context.Proyectos.Where(n => n.Estado == "Activa").ToListAsync();
            }
            else
            {
                TempData["ErrorMessage"] = "Filtro no válido.";
            }

            return View("Index", proyectos.ToList());
        }

        // GET: Proyectoes/Create

        public IActionResult Create()
        {
            if (User.IsInRole("Administrador") || User.IsInRole("Blogger"))
                return View();
            else return RedirectToAction("Index", "Home");
        }

        // POST: Proyectoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NoticiaId,TituloNoticia,Desarrollo,NivelPrioridad,FechaPublicacion,Estado")] Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proyecto);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "La noticia se ha agregado correctamente." });
            }
            return Json(new { success = false, message = "Error al agregar la noticia." });
        }

        // GET: Proyectoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null)
            {
                return NotFound();
            }

            // Renderiza una vista parcial en lugar de la vista completa
            return PartialView("Edit", proyecto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string nivelPrioridad)
        {
            if (id <= 0 || string.IsNullOrEmpty(nivelPrioridad))
            {
                return Json(new { success = false, message = "Parámetros inválidos para la actualización." });
            }

            var proyecto = await _context.Proyectos.FindAsync(id);

            if (proyecto == null)
            {
                return NotFound();
            }

            try
            {
                proyecto.NivelPrioridad = nivelPrioridad;
                _context.Update(proyecto);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "La prioridad ha sido actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al actualizar la prioridad: " + ex.Message });
            }
        }


        // GET: Proyectoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.IsInRole("Administrador") || User.IsInRole("Blogger"))
            {
                var proyectos = await _context.Proyectos
                .Where(p => p.Estado == "Activa")
                .OrderBy(p => p.FechaPublicacion) // Ordenar de la más antigua a la más reciente
                .ToListAsync();

                return View(proyectos);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Proyectoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto != null)
            {
                // Cambiar el estado o realizar la lógica de inhabilitar aquí
                proyecto.Estado = "Inhabilitado"; // Ejemplo de cambio de estado

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true, message = "La noticia se ha inhabilitado correctamente." });
        }

        private bool ProyectoExists(int id)
        {
            return _context.Proyectos.Any(e => e.NoticiaId == id);
        }
    }
}
