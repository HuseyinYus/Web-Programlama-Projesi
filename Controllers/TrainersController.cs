using Microsoft.AspNetCore.Authorization;
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
    public class TrainersController : Controller
    {
        private readonly ApplicationDbContext _context;
        // Ortak Branş Listesi (ServicesController ile aynı olmalı)
        private readonly List<string> _branches = new List<string>
{
    "Fitness", "Yoga", "Pilates", "Crossfit", "Yüzme", "Boks", "Kick Boks", "Zumba", "Spinning"
};

        public TrainersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trainers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trainers.ToListAsync());
        }

        // GET: Trainers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .FirstOrDefaultAsync(m => m.TrainerId == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // GET: Trainers/Create

        // 1. Ekleme Sayfasını Göster (GET)
        [Authorize] // Sadece giriş yapmış olmak yeterli, rolü içeride kontrol edeceğiz
        public IActionResult Create()
        {
            // KONTROL: Eğer kullanıcı Admin DEĞİLSE VE E-postası seninki DEĞİLSE -> Erişim Engellendi
            if (!User.IsInRole("Admin") && User.Identity?.Name != "g231210022@sakarya.edu.tr")
            {
                return Forbid(); // Veya return RedirectToAction("AccessDenied", "Account");
            }

            ViewBag.Branches = new SelectList(_branches);
            return View();
        }

        // 2. Veriyi Kaydet (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("TrainerId,FullName,Specialization,WorkingHours,Biography")] Trainer trainer)
        {
            // KONTROL: POST işleminde de aynı güvenlik kontrolü şart
            if (!User.IsInRole("Admin") && User.Identity?.Name != "g231210022@sakarya.edu.tr")
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                _context.Add(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        // GET: Trainers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }
            ViewBag.Branches = new SelectList(_branches);
            return View(trainer);
        }

        // POST: Trainers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainerId,FullName,Specialization,Bio,WorkingHours")] Trainer trainer)
        {
            if (id != trainer.TrainerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerExists(trainer.TrainerId))
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
            return View(trainer);
        }

        // GET: Trainers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .FirstOrDefaultAsync(m => m.TrainerId == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerExists(int id)
        {
            return _context.Trainers.Any(e => e.TrainerId == id);
        }
    }
}
