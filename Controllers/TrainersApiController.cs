using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Programlama_Projesi.Data; // KENDİ PROJE İSMİNİ KONTROL ET
using Web_Programlama_Projesi.Models; // KENDİ PROJE İSMİNİ KONTROL ET

namespace Web_Programlama_Projesi.Controllers
{
    [Route("api/[controller]")] // Adres: /api/trainers
    [ApiController]
    public class TrainersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Tüm Antrenörleri Getir (GET: api/trainers)
        [HttpGet]
        public async Task<IActionResult> GetTrainers()
        {
            var trainers = await _context.Trainers
                .Select(t => new
                {
                    t.TrainerId,
                    t.FullName,
                    t.Specialization,
                    t.WorkingHours
                })
                .ToListAsync();

            return Ok(trainers);
        }

        // 2. Uzmanlık Alanına Göre Filtrele (GET: api/trainers/filter?skill=Yoga)
        // PDF'teki LINQ şartını sağlayan kısım burası.
        [HttpGet("filter")]
        public async Task<IActionResult> GetTrainersBySkill(string skill)
        {
            if (string.IsNullOrEmpty(skill))
            {
                return BadRequest("Lütfen bir uzmanlık alanı (skill) girin.");
            }

            var trainers = await _context.Trainers
                .Where(t => t.Specialization.Contains(skill)) // LINQ Sorgusu
                .Select(t => new
                {
                    t.TrainerId,
                    t.FullName,
                    t.Specialization
                })
                .ToListAsync();

            if (trainers.Count == 0)
            {
                return NotFound("Bu uzmanlık alanında antrenör bulunamadı.");
            }

            return Ok(trainers);
        }
    }
}