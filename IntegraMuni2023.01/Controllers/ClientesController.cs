using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegraMuni2023._01.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IntegraMuni2023._01.Controllers
{
    //[Authorize]
    public class ClientesController : Controller
    {
        private readonly IntegraMuni2023Context _context;
        private int clienteid;
        public ClientesController(IntegraMuni2023Context context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Administrador"))
                return View(await _context.Clientes.ToListAsync());
            return RedirectToAction("Index", "Home");
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details()
        {
            // Obtiene el ID del usuario autenticado

            var cliente = _context.Clientes.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);
            if (cliente == null)
            {
                return NotFound();
            }
            
            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreCompleto, Identificacion, CorreoElectronico, Direccion, Telefono, Estado, Clave, ConfirmarClave")] Cliente cliente)
        {
            try
            {
                var existingClient = await _context.Clientes.FirstOrDefaultAsync(c =>
                c.Identificacion == cliente.Identificacion &&
                c.CorreoElectronico == cliente.CorreoElectronico &&
                c.NombreCompleto == cliente.NombreCompleto);

                if (existingClient != null)
                {
                    TempData["ErrorMessage"] = "Cliente existente. Verifica los datos e intentalo de nuevo.";
                    return View(cliente);
                }

                if (ModelState.IsValid)
                {
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Registrado correctamente.";
                }
                TempData["ErrorMessage"] = "Error en el modelo. Verifica los datos e intentalo de nuevo.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al registrar el cliente: " + ex.Message;
                // Para obtener detalles específicos de la excepción interna, puedes usar ex.InnerException.Message o ex.InnerException.StackTrace
                if (ex.InnerException != null)
                {
                    TempData["ErrorMessage"] += " Detalles: " + ex.InnerException.Message;
                }
            }
            return RedirectToAction("Create", "Clientes");
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null || _context.Clientes == null)
                {
                    return NotFound();
                }

                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }
                string identidad = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (cliente.CorreoElectronico.Equals(User.Identity.Name) || User.IsInRole("Administrador"))
                    return View(cliente);
                return NotFound();
            }
            return RedirectToAction("Login", "Login");
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ClienteId,NombreCompleto,Identificacion,CorreoElectronico,Direccion,Telefono,Estado,Clave, ConfirmarClave")] Cliente cliente)
        {
            if (id != cliente.ClienteId)
            {
                return NotFound();
            }

            var usuario = await _context.Clientes.FindAsync(cliente.ClienteId);

            if (usuario == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (usuario.Clave != cliente.Clave)
                {
                    TempData["ErrorMessage"] = "Clave incorrecta.";
                }
                else
                {
                    try
                    {
                        // Actualiza las propiedades del usuario existente con los valores del cliente enviado
                        _context.Entry(usuario).CurrentValues.SetValues(cliente);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Editado correctamente.";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ClienteExists(cliente.ClienteId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            if(User.IsInRole("Administrador"))
                return RedirectToAction("Index");
            // Si hay algún error, regresa a la vista con la entidad usuario
            return View(usuario);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (User.IsInRole("Administrador")) { 
                if (id == null || _context.Clientes == null)
                {
                    return NotFound();
                }

                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(m => m.ClienteId == id);
                if (cliente == null)
                {
                    return NotFound();
                }

                return View(cliente);
            }
            return RedirectToAction("Index", "Home");

        }



        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, [Bind("ClienteId,NombreCompleto,Identificacion,CorreoElectronico,Direccion,Telefono,Estado,Clave, ConfirmarClave")] Cliente cliente)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'IntegraMuni2023Context.Clientes'  is null.");
            }

            if (id != cliente.ClienteId)
            {
                return NotFound();
            }
            try
            {
                cliente.Estado = "Desactivado";
                _context.Update(cliente);
                await _context.SaveChangesAsync();
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Login");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(cliente.ClienteId))
                {
                    return NotFound();

                }
                else
                {
                    throw;
                }
            }
        }

        private bool ClienteExists(int? id)
        {
            return (_context.Clientes?.Any(e => e.ClienteId == id)).GetValueOrDefault();
        }

        public IActionResult Activar()
        {
            if (User.IsInRole("Administrador"))
                return View();
            return RedirectToAction("Index", "Home");
        }

        //metodo para activar clientes
        [HttpPost, ActionName("Activar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activar(string CorreoElectronico)
        {
            // Buscar al cliente por su ID
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.CorreoElectronico == CorreoElectronico);

            if (cliente == null)
            {
                return NotFound(); // Si no se encuentra el cliente, devolver un error 404
            }

            // Actualizar el estado del cliente a "Activo"
            cliente.Estado = "Activo";

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Puedes redirigir a una acción específica después de activar al cliente
            TempData["SuccessMessage"] = "Cuenta correctamente activada.";
            return RedirectToAction("Login", "Login");
        }

    }

}
