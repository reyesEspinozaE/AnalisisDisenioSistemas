using IntegraMuni2023._01.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IntegraMuni2023._01.Controllers
{
    public class LoginController : Controller
    {
        //variable encargada de manejar el contexto del ORM 
        private readonly IntegraMuni2023Context _context;


        /// <summary>
        /// Constructor con parámetros, revise la instacia del ORM
        /// </summary>
        /// <param name="context"></param>
        ///

        // si hay un error 
        public LoginController(IntegraMuni2023Context context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string correoElectronico, string clave, bool esEmpleado)
        {
            if (esEmpleado)
            {
                var empleado = _context.Empleados.FirstOrDefault(u => u.CorreoElectronico == correoElectronico);
                if (empleado != null)
                {
                    if (empleado.Estado == "Desactivado")
                    {
                        TempData["WarningMessage"] = "Su cuenta se encuentra desactivada.";
                    }
                    Empleado emp = new Empleado();
                    emp.CorreoElectronico = correoElectronico;
                    emp.Contraseña = clave;

                    //se utiliza el metodo para validar los datos el usuario
                    var temp = this.ValidarEmpleado(emp);
                    if (temp != null)
                    {
                        //se crea la instacia para la entidad del usuario y el tipo de autenticación
                        var userClaims = new List<Claim>() { new Claim(ClaimTypes.Name, temp.CorreoElectronico), new Claim(ClaimTypes.Role, temp.Rol.ToString()) };
                        var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                        var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });

                        //se realiza la autenticacion dentro del contextohttp
                        HttpContext.SignInAsync(userPrincipal);

                        //se ubica al ususario en la pagina de inicio
                        return RedirectToAction("Index", "Home");
                    }
            }
        }
            else
            {
                // Validar como Cliente
                var cliente = _context.Clientes.FirstOrDefault(u => u.CorreoElectronico == correoElectronico);
                if (cliente != null)
                {
                    if (cliente.Estado == "Desactivado")
                    {
                        TempData["WarningMessage"] = "Su cuenta se encuentra desactivada.";
                    }
                    Cliente user = new Cliente();
                    user.CorreoElectronico = correoElectronico;
                    user.Clave = clave;

                    //se utiliza el metodo para validar los datos el usuario
                    var temp = this.ValidarCliente(user);
                    if (temp != null)
                    {
                        //se crea la instacia para la entidad del usuario y el tipo de autenticación
                        var userClaims = new List<Claim>() { new Claim(ClaimTypes.Name, temp.CorreoElectronico), new Claim(ClaimTypes.Role, temp.Rol.ToString()) };
                        var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                        var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });

                        //se realiza la autenticacion dentro del contextohttp
                        HttpContext.SignInAsync(userPrincipal);

                        //se ubica al ususario en la pagina de inicio
                        return RedirectToAction("Index", "Home");
                    }
                    TempData["ErrorMessage"] = "Error el Usuario/Contraseña son incorrectos...";
                    return View(user);
                }
            }
            TempData["ErrorMessage"] = "Error: El usuario no es empleado.";

            return View();
        }

        private Cliente ValidarCliente(Cliente temp)
        {
            Cliente autorizado = null;
            //se busca el ususario en la base de datos con el email
            var user = _context.Clientes.FirstOrDefault(u => u.CorreoElectronico == temp.CorreoElectronico);

            //se pregunta si existe el usuario
            if (user != null)
            {
                //vemos si el usuario no ha sido eliminado
                if (user.Estado == "Activo")
                {
                    //se verifica su password
                    if (user.Clave.Equals(temp.Clave))
                    {
                        autorizado = user;
                    }
                }
            }
            return autorizado;
        }//cierre de Usuario metodo

            private Empleado ValidarEmpleado(Empleado temp)
            {
                Empleado autorizado = null;
                //se busca el ususario en la base de datos con el email
                var user = _context.Empleados.FirstOrDefault(u => u.CorreoElectronico == temp.CorreoElectronico);

                //se pregunta si existe el usuario
                if (user != null)
                {
                    //vemos si el usuario no ha sido eliminado
                    if (user.Estado == "Activo")
                    {
                        //se verifica su password
                        if (user.Contraseña.Equals(temp.Contraseña))
                        {
                            autorizado = user;
                        }
                    }
                }
                return autorizado;
            }//cierre de Usuario metodo



            //[HttpPost]
            //[ValidateAntiForgeryToken]
            //public IActionResult Login([Bind] Cliente user)
            //{
            //    var cliente = _context.Clientes.FirstOrDefault(u => u.CorreoElectronico == user.CorreoElectronico);
            //    if (cliente.Estado == "Desactivado")
            //    {
            //        TempData["WarningMessage"] = "Su cuenta se encuentra desactivada.";
            //    }

            //    //se utiliza el metodo para validar los datos el usuario
            //    var temp = this.ValidarCliente(user);
            //    if (temp != null)
            //    {
            //        //se crea la instacia para la entidad del usuario y el tipo de autenticación
            //        var userClaims = new List<Claim>() { new Claim(ClaimTypes.Name, temp.CorreoElectronico) };
            //        var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
            //        var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });

            //        //se realiza la autenticacion dentro del contextohttp
            //        HttpContext.SignInAsync(userPrincipal);

            //        //se ubica al ususario en la pagina de inicio
            //        return RedirectToAction("Index", "Home");
            //    }
            //    TempData["ErrorMessage"] = "Error el Usuario/Contraseña son incorrectos...";



            //    return View(user);
            //}

            //private Cliente ValidarCliente(Cliente temp)
            //{
            //    Cliente autorizado = null;
            //    //se busca el ususario en la base de datos con el email
            //    var user = _context.Clientes.FirstOrDefault(u => u.CorreoElectronico == temp.CorreoElectronico);

            //    //se pregunta si existe el usuario
            //    if (user != null)
            //    {
            //        //vemos si el usuario no ha sido eliminado
            //        if (user.Estado == "Activo")
            //        {
            //            //se verifica su password
            //            if (user.Clave.Equals(temp.Clave))
            //            {
            //                autorizado = user;
            //            }
            //        }
            //    }
            //    return autorizado;
            //}//cierre de Usuario metodo

            public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }//cierre del salir
    }
}
