using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using IntegraMuni2023._01.Models;

namespace IntegraMuni2023._01.Models
{
    public class Email
    {
        public void Aprobado(Cliente cliente, Pago pago, Servicio servicio)
        {
            try
            {
                //se crea una instancia del object email
                MailMessage email = new MailMessage();
                //Asunto
                email.Subject = "Confirmación: Pagos de servicios contratados.";
                //Destinatarios
                email.To.Add(new MailAddress("IntegraMuniEsparza@outlook.es"));
                //Dirección de correo del usuario
                email.To.Add(new MailAddress(cliente.CorreoElectronico));
                //emisor del correo
                email.From = new MailAddress("IntegraMuniEsparza@outlook.es");

                // Construir la vista HTML del body del correo
                // Indicar que el contenido es en HTML
                string html = "<html><body>";
                html += "<h2>Confirmación de Pago de servicios</h2>";
                html += "<p>Estimado(a) " + cliente.NombreCompleto + ",</p>";
                html += "<p>Factura electronica de pago de servicios.</p>";
                html += "<p>A continuación se detallan los datos de su factura:</p>";
                html += "<ul>";
                html += "<li><b>Número de Solicitud:</b> " + pago.PagoId + "</li>";
                html += "<li><b>Cédula:</b> " + cliente.Identificacion + "</li>";
                html += "<li><b>Nombre Completo:</b> " + cliente.NombreCompleto + "</li>";
                html += "<li><b>Dirección:</b> " + cliente.Direccion + "</li>";
                html += "<li><b>Correo Electrónico:</b> " + cliente.CorreoElectronico + "</li>";
                html += "<li><b>Tipo de servicio:</b> " + servicio.NombreServicio + "</li>";
                html += "<li><b>Monto a pagar: ₡</b> " + pago.MontoPago.ToString("F") + "</li>";
                html += "<li><b>Plametodo de pago:</b> " + pago.MetodoPago + "</li>";
                html += "</ul>";
                html += "<p>Queremos recordarte que estamos a tu disposición para cualquier consulta o requerimiento adicional. ¡No dudes en contactarnos!</p>";
                html += "<p>¡Gracias por confiar en nosotros para tus necesidades financieras!</p>";
                html += "<p>Atentamente,</p>";
                html += "<p>Municipalida de Esparza</p>";
                html += "</body></html>";



                email.IsBodyHtml = true;
                //se indica la prioridad recomendación debe ser la prioridad normal
                email.Priority = MailPriority.Normal;

                //se instancia la vista del html para el cuerpo del body del email
                AlternateView view = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8,
                    MediaTypeNames.Text.Html);

                //se agrega la vista html al cuerpo del email
                email.AlternateViews.Add(view);

                //Configuración del protocolo de comunicación smtp
                SmtpClient smtp = new SmtpClient();

                //sevidor de correo a implementar
                smtp.Host = "smtp-mail.outlook.com";

                //puerto de comunicación
                smtp.Port = 587;

                //se indica si el buzon utiliza seguridad tipo SSL
                smtp.EnableSsl = true;

                //se indica las credenciales de autenticación
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("IntegraMuniEsparza@outlook.es", "integraEsparza");

                //Método para enviar el email
                smtp.Send(email);

                //se liberan los instancias de los objects
                email.Dispose();
                smtp.Dispose();
            }//Cierre del try
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");
            }//Cierre del catch
        }//cierre de aprobado

        public void TramiteSolicitado(Cliente cliente, PagosTramites pagosTramites)
        {
            try
            {
                //se crea una instancia del object email
                MailMessage email = new MailMessage();
                //Asunto
                email.Subject = "Confirmación: Solicitud de Tramite recibida.";
                //Destinatarios
                email.To.Add(new MailAddress("IntegraMuniEsparza@outlook.es"));
                //Dirección de correo del usuario
                email.To.Add(new MailAddress(cliente.CorreoElectronico));
                //emisor del correo
                email.From = new MailAddress("IntegraMuniEsparza@outlook.es");

                // Construir la vista HTML del body del correo
                // Indicar que el contenido es en HTML
                string html = "<html><body>";
                html += "<h2>Confirmación de Solicitud de Trámite</h2>";
                html += "<p>Estimado(a) " + cliente.NombreCompleto + ",</p>";
                html += "<p>A continuación se detallan los datos de su solicitud:</p>";
                html += "<ul>";
                html += "<li><b>Número de Trámite:</b> " + pagosTramites.TramiteID + "</li>";
                html += "<li><b>Fecha de Inicio:</b> " + pagosTramites.FechaPago.ToShortDateString() + "</li>";
                html += "<li><b>Descripción:</b> " + pagosTramites.Descripcion + "</li>";

                html += "<h3>Datos del Cliente:</h3>";
                html += "<ul>";
                html += "<li><b>Cédula:</b> " + cliente.Identificacion + "</li>";
                html += "<li><b>Nombre Completo:</b> " + cliente.NombreCompleto + "</li>";
                html += "<li><b>Correo Electrónico:</b> " + cliente.CorreoElectronico + "</li>";
                html += "<li><b>Dirección:</b> " + cliente.Direccion + "</li>";

                html += "</ul>";
                html += "<p>Quedamos a su disposición para cualquier consulta adicional. ¡Gracias!</p>";
                html += "<p>Atentamente,</p>";
                html += "<p>Municipalidad de Esparza</p>";
                html += "</body></html>";

                email.IsBodyHtml = true;
                //se indica la prioridad recomendación debe ser la prioridad normal
                email.Priority = MailPriority.Normal;

                //se instancia la vista del html para el cuerpo del body del email
                AlternateView view = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8,
                    MediaTypeNames.Text.Html);

                //se agrega la vista html al cuerpo del email
                email.AlternateViews.Add(view);

                //Configuración del protocolo de comunicación smtp
                SmtpClient smtp = new SmtpClient();

                //sevidor de correo a implementar
                smtp.Host = "smtp-mail.outlook.com";

                //puerto de comunicación
                smtp.Port = 587;

                //se indica si el buzon utiliza seguridad tipo SSL
                smtp.EnableSsl = true;

                //se indica las credenciales de autenticación
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("IntegraMuniEsparza@outlook.es", "integraEsparza");

                //Método para enviar el email
                smtp.Send(email);

                //se liberan los instancias de los objects
                email.Dispose();
                smtp.Dispose();
            }//Cierre del try
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");
            }//Cierre del catch
        }//cierre de TramiteSolicitado

        public void TramiteAprobado(Cliente cliente, Tramite tramite)
        {
            try
            {
                //se crea una instancia del object email
                MailMessage email = new MailMessage();
                //Asunto
                email.Subject = "Confirmación: Aprobación de tu solicitud de trámite.";
                //Destinatarios
                email.To.Add(new MailAddress("IntegraMuniEsparza@outlook.es"));
                //Dirección de correo del usuario
                email.To.Add(new MailAddress(cliente.CorreoElectronico));
                //emisor del correo
                email.From = new MailAddress("IntegraMuniEsparza@outlook.es");

                // Construir la vista HTML del body del correo
                // Indicar que el contenido es en HTML
                string html = "<html><body>";
                html += "<h2>Confirmación de Solicitud de Trámite Aprobada</h2>";
                html += "<p>Estimado(a) " + cliente.NombreCompleto + ",</p>";
                html += "<p>Se ha aprobado la solicitud del trámite:</p>";
                html += "<ul>";
                html += "<li><b>Número de Trámite:</b> " + tramite.TramiteId + "</li>";
                html += "<li><b>Fecha de Inicio:</b> " + tramite.FechaInicio.ToShortDateString() + "</li>";
                html += "<li><b>Descripción:</b> " + tramite.Descripcion + "</li>";
                // Agregar más detalles relevantes del trámite

                html += "<h3>Datos del Cliente:</h3>";
                html += "<ul>";
                html += "<li><b>Cédula:</b> " + cliente.Identificacion + "</li>";
                html += "<li><b>Nombre Completo:</b> " + cliente.NombreCompleto + "</li>";
                html += "<li><b>Correo Electrónico:</b> " + cliente.CorreoElectronico + "</li>";
                html += "<li><b>Dirección:</b> " + cliente.Direccion + "</li>";
                // Agregar más detalles relevantes del cliente

                html += "</ul>";
                html += "<p>¡Gracias por confiar en nosotros para tu trámite!</p>";
                html += "<p>Atentamente,</p>";
                html += "<p>Municipalidad de Esparza</p>";
                html += "</body></html>";



                email.IsBodyHtml = true;
                //se indica la prioridad recomendación debe ser la prioridad normal
                email.Priority = MailPriority.Normal;

                //se instancia la vista del html para el cuerpo del body del email
                AlternateView view = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8,
                    MediaTypeNames.Text.Html);

                //se agrega la vista html al cuerpo del email
                email.AlternateViews.Add(view);

                //Configuración del protocolo de comunicación smtp
                SmtpClient smtp = new SmtpClient();

                //sevidor de correo a implementar
                smtp.Host = "smtp-mail.outlook.com";

                //puerto de comunicación
                smtp.Port = 587;

                //se indica si el buzon utiliza seguridad tipo SSL
                smtp.EnableSsl = true;

                //se indica las credenciales de autenticación
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("IntegraMuniEsparza@outlook.es", "integraEsparza");

                //Método para enviar el email
                smtp.Send(email);

                //se liberan los instancias de los objects
                email.Dispose();
                smtp.Dispose();
            }//Cierre del try
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");
            }//Cierre del catch
        }//cierre de TramiteAprobado

        public void TramiteDenegado(Cliente cliente, Tramite tramite)
        {
            try
            {
                MailMessage email = new MailMessage();
                email.Subject = "Confirmación: Denegación de tu solicitud de trámite.";
                email.To.Add(new MailAddress(cliente.CorreoElectronico));
                email.From = new MailAddress("IntegraMuniEsparza@outlook.es");

                string html = "<html><body>";
                html += "<h2>Denegación de Solicitud de Trámite</h2>";
                html += "<p>Estimado(a) " + cliente.NombreCompleto + ",</p>";
                html += "<p>Lamentamos informarte que la solicitud del trámite ha sido denegada.</p>";
                html += "<p>Detalles del trámite denegado:</p>";
                html += "<ul>";
                html += "<li><b>Número de Trámite:</b> " + tramite.TramiteId + "</li>";
                html += "<li><b>Fecha de Inicio:</b> " + tramite.FechaInicio.ToShortDateString() + "</li>";
                html += "<li><b>Descripción:</b> " + tramite.Descripcion + "</li>";
                // Agregar más detalles relevantes del trámite

                html += "<h3>Datos del Cliente:</h3>";
                html += "<ul>";
                html += "<li><b>Cédula:</b> " + cliente.Identificacion + "</li>";
                html += "<li><b>Nombre Completo:</b> " + cliente.NombreCompleto + "</li>";
                html += "<li><b>Correo Electrónico:</b> " + cliente.CorreoElectronico + "</li>";
                html += "<li><b>Dirección:</b> " + cliente.Direccion + "</li>";
                // Agregar más detalles relevantes del cliente

                html += "</ul>";
                html += "<p>Si tienes alguna consulta, no dudes en contactarnos.</p>";
                html += "<p>Atentamente,</p>";
                html += "<p>Municipalidad de Esparza</p>";
                html += "</body></html>";

                email.IsBodyHtml = true;
                email.Priority = MailPriority.Normal;

                AlternateView view = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);
                email.AlternateViews.Add(view);

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("IntegraMuniEsparza@outlook.es", "integraEsparza");

                smtp.Send(email);

                email.Dispose();
                smtp.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");
            }
        }//cierre de TramiteDenegado
    }//cierre del class
}//cierre del namespace
