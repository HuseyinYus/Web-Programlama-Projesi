using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Programlama_Projesi.Data;
using Web_Programlama_Projesi.Models;

namespace Web_Programlama_Projesi.Controllers
{
    [Authorize] // Sadece üyeler girebilir
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AppointmentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            // Admin hepsini görür, Üye sadece kendininkini
            if (User.IsInRole("Admin"))
            {
                var allAppointments = _context.Appointments
                    .Include(a => a.Member).Include(a => a.Service).Include(a => a.Trainer);
                return View(await allAppointments.ToListAsync());
            }
            else
            {
                var userId = _userManager.GetUserId(User);
                var myAppointments = _context.Appointments
                    .Include(a => a.Member).Include(a => a.Service).Include(a => a.Trainer)
                    .Where(a => a.MemberId == userId);
                return View(await myAppointments.ToListAsync());
            }
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            // Dropdown listelerini dolduruyoruz
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "TrainerId", "FullName");
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceName");
            return View();
        }

        // RANDEVU ONAYLA (Sadece Admin)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = "Onaylandı"; // Durumu güncelle
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // RANDEVU İPTAL ET / REDDET (Sadece Admin)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = "İptal Edildi"; // Durumu güncelle
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,Time,TrainerId,ServiceId")] Appointment appointment)
        {
            // 1. Üyeyi otomatik ata
            var userId = _userManager.GetUserId(User);
            appointment.MemberId = userId;
            appointment.Status = "Onay Bekliyor";

            // 2. ANTRENÖR ÇALIŞMA SAATİ KONTROLÜ
            var trainer = await _context.Trainers.FindAsync(appointment.TrainerId);
            if (trainer != null && !string.IsNullOrEmpty(trainer.WorkingHours))
            {
                try
                {
                    var hours = trainer.WorkingHours.Split('-'); // "09:00-17:00" formatı
                    var startWork = TimeSpan.Parse(hours[0].Trim());
                    var endWork = TimeSpan.Parse(hours[1].Trim());

                    if (appointment.Time < startWork || appointment.Time > endWork)
                    {
                        ModelState.AddModelError("", $"Bu antrenör sadece {trainer.WorkingHours} saatleri arasında çalışmaktadır.");
                    }
                }
                catch { }
            }

            // 3. ÇAKIŞMA KONTROLÜ
            bool isConflict = _context.Appointments.Any(a =>
                a.TrainerId == appointment.TrainerId &&
                a.Date == appointment.Date &&
                a.Time == appointment.Time);

            if (isConflict)
            {
                ModelState.AddModelError("", "Seçilen antrenör bu saatte dolu.");
            }

            // 4. Kaydet
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Hata varsa listeleri tekrar doldur
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "TrainerId", "FullName", appointment.TrainerId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceName", appointment.ServiceId);
            return View(appointment);
        }

        // Diğer metodlar (Edit, Delete, Details) buranın altında standart olarak kalabilir veya
        // Scaffolding ile gelen kodları kullanabilirsin. Create ve Index en önemlileriydi.
    }
}
