using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegraMuni2023._01.Models;

namespace IntegraMuni2023._01.Controllers
{
    public class TareasController : Controller
    {
        private readonly IntegraMuni2023Context _context;

        public TareasController(IntegraMuni2023Context context)
        {
            _context = context;
        }

        // GET: Tareas
        public async Task<IActionResult> Index()
        {
            var integraMuni2023Context = _context.Tareas.Include(t => t.Empleado);
            return View(await integraMuni2023Context.ToListAsync());
        }

        // GET: Tareas/Create
        public IActionResult Create()
        {
            if (User.IsInRole("Administrador") || User.IsInRole("Jefe"))
            {
                // Filtrar la lista de Empleados excluyendo aquellos con roles específicos
                var empleadosExcluidos = new List<string> { "Administrador", "Jefe" };
                //var empleadosFiltrados = _context.Empleados.Where(e => !e.Rol.Equals("Administrador"));
                var empleadosFiltrados = _context.Empleados.Where(e => !empleadosExcluidos.Contains(e.Rol)).ToList();

                ViewData["EmpleadoId"] = new SelectList(empleadosFiltrados, "EmpleadoId", "Nombre");
                return View();
                //ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "Nombre");
                //return View();
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Tareas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TareaId,Titulo,Descripcion,Prioridad,FechaInicio,FechaFin,EmpleadoId,Estado")] Tarea tarea)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(tarea);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "La tarea se ha agregado correctamente." });
                }
                else
                {
                    // Si hay un error en el modelo, necesitas volver a cargar la lista de empleados
                    var empleados = await _context.Empleados.ToListAsync();
                    var empleadosSelectList = new SelectList(empleados, "EmpleadoId", "Nombre");
                    ViewData["EmpleadoId"] = empleadosSelectList;

                    return Json(new { success = false, message = "Error en el modelo. Por favor, verifica los datos e inténtalo de nuevo." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al agregar la tarea: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
            {
                return NotFound();
            }

            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "Nombre");

            // Renderiza la vista de edición directamente
            return PartialView("Edit", tarea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Tarea editedTask)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Busca la tarea original en la base de datos
                    var tarea = await _context.Tareas.FindAsync(editedTask.TareaId);

                    if (tarea != null)
                    {
                        // Actualiza solo los campos que deben ser modificados
                        tarea.Titulo = editedTask.Titulo;
                        tarea.Descripcion = editedTask.Descripcion;
                        tarea.Prioridad = editedTask.Prioridad;
                        tarea.EmpleadoId = editedTask.EmpleadoId;
                        tarea.FechaInicio = editedTask.FechaInicio;
                        tarea.FechaFin = editedTask.FechaFin;

                        _context.Update(tarea);
                        await _context.SaveChangesAsync();

                        return Json(new { success = true, message = "La tarea se ha modificado correctamente." });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Tarea no encontrada.");
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al modificar la tarea: " + ex.Message });
                }
            }
            return View("Edit", editedTask);
        }

        // GET: Tareas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var tareas = await _context.Tareas
               .Include(t => t.Empleado)
               .ToListAsync();

            if (tareas == null || tareas.Count == 0)
            {
                TempData["Message"] = "No hay tareas registradas.";
                return RedirectToAction(nameof(Index));
            }

            return View(tareas);
        }

        // POST: Tareas/DeleteMultiple
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMultiple(List<int> selectedTasks)
        {
            try
            {
                if (selectedTasks == null || selectedTasks.Count == 0)
                {
                    return Json(new { success = false, message = "No se seleccionaron tareas para finalizar." });
                }

                if (_context.Tareas == null)
                {
                    return Json(new { success = false, message = "No hay tareas registradas." });
                }

                foreach (var taskId in selectedTasks)
                {
                    var tarea = await _context.Tareas.FindAsync(taskId);
                    if (tarea != null)
                    {
                        _context.Tareas.Remove(tarea);
                    }
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Tareas finalizadas correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al finalizar las tareas: " + ex.Message });
            }
        }

        // GET: Tareas/EditDate/5
        public async Task<IActionResult> EditDate(int? id)
        {
            var tareas = await _context.Tareas
               .Include(t => t.Empleado)
               .ToListAsync();

            if (tareas == null || tareas.Count == 0)
            {
                TempData["Message"] = "No hay tareas registradas.";
                return RedirectToAction(nameof(Index));
            }

            return View(tareas);
        }

        // POST: Tareas/EditDate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDate(int tareaId, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                // Busca la tarea original en la base de datos
                var tarea = await _context.Tareas.FindAsync(tareaId);

                if (tarea != null)
                {
                    // Actualiza solo las fechas
                    tarea.FechaInicio = fechaInicio;
                    tarea.FechaFin = fechaFin;

                    _context.Update(tarea);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Fechas actualizadas correctamente." });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tarea no encontrada.");
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al actualizar las fechas: " + ex.Message });
            }

            return Json(new { success = false, message = "Error en la validación del modelo." });
        }
        private bool TareaExists(int id)
        {
            return (_context.Tareas?.Any(e => e.TareaId == id)).GetValueOrDefault();
        }
    }
}
