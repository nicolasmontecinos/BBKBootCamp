using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BBKBootCamp.Data;
using BBKBootCamp.Models;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;

namespace BBKBootCamp.Controllers
{
    public class SolicitantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SolicitantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Solicitantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Solicitante.ToListAsync());
        }

        // GET: Solicitantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitante = await _context.Solicitante
                .FirstOrDefaultAsync(m => m.Id == id);
            if (solicitante == null)
            {
                return NotFound();
            }

            return View(solicitante);
        }

        // GET: Solicitantes/Create
        public IActionResult Create()
        {
            return View();
        }
       
        public IActionResult SendMail([Bind("Id,CorreoElectronico,Nombre,Apellido,FechaNacimiento,SexoTipo,Pregunta1,Pregunta2,Pregunta3,Pregunta4,Pregunta5,Pregunta6,Proceso")] Solicitante solicitante)
        {
            //////ES UNA PRUEBA PARA EL ENVIO AUTOMATICO DE CORREO//////
            string correo = solicitante.CorreoElectronico;
            System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient("smtp.live.com");
            var mail = new MailMessage();
            mail.From = new MailAddress("nicolas-m-s@hotmail.com");//correo de noticador
            mail.To.Add(solicitante.CorreoElectronico);//a quien lo envio
            mail.Subject = "BBK BOOTCAMP";
            mail.IsBodyHtml = true;
            string htmlBody;
            htmlBody = "!!Gracias por inscribirte!! "+ solicitante.Nombre +" \n En breve nos pondremos en contacto contigo para concertar un entrevista";
            mail.Body = htmlBody;
            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("nicolas-m-s@hotmail.com", "n1c0l45-m0nt3");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
            ////////ES UNA PRUEBA PARA EL ENVIO AUTOMATICO DE CORREO//////
            return View();  //DESCOMENTAR SI GENERA ERROR
            //return View(VistaNotificacion);
        }
        // POST: Solicitantes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CorreoElectronico,Nombre,Apellido,FechaNacimiento,SexoTipo,Pregunta1,Pregunta2,Pregunta3,Pregunta4,Pregunta5,Pregunta6,Proceso")] Solicitante solicitante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(solicitante);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(VistaNotificacion));
                //return RedirectToAction(nameof(SendMail));   //DESCOMENTAR SI GENERA ERROR

            }
            //return View(SendMail(solicitante));
            return RedirectToAction("SendMail",solicitante);
        }
        //////////ESTO ES PARA QUE NO VUELVA AL INDEX POR DEFECTO//////
        public IActionResult VistaNotificacion()
        {
            return View();
        }
        //////////ESTO ES PARA QUE NO VUELVA AL INDEX POR DEFECTO//////

        // GET: Solicitantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitante = await _context.Solicitante.FindAsync(id);
            if (solicitante == null)
            {
                return NotFound();
            }
            return View(solicitante);
        }

        // POST: Solicitantes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CorreoElectronico,Nombre,Apellido,FechaNacimiento,SexoTipo,Pregunta1,Pregunta2,Pregunta3,Pregunta4,Pregunta5,Pregunta6,Proceso")] Solicitante solicitante)
        {
            if (id != solicitante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solicitante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolicitanteExists(solicitante.Id))
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
            return View(solicitante);
        }

        // GET: Solicitantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitante = await _context.Solicitante
                .FirstOrDefaultAsync(m => m.Id == id);
            if (solicitante == null)
            {
                return NotFound();
            }

            return View(solicitante);
        }

        // POST: Solicitantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var solicitante = await _context.Solicitante.FindAsync(id);
            _context.Solicitante.Remove(solicitante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SolicitanteExists(int id)
        {
            return _context.Solicitante.Any(e => e.Id == id);
        }
    }
}
