using System.ComponentModel.DataAnnotations;

namespace Web_Programlama_Projesi.Models
{
    public class Trainer
    {
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Uzmanlık Alanı")]
        public string Specialization { get; set; } // Örn: Yoga, Fitness, Pilates

        [Display(Name = "Biyografi")]
        public string? Biography { get; set; } // Soru işareti (?) boş geçilebilir demek

        // Çalışma saatleri (Basitlik için string tutabiliriz örn: "09:00 - 18:00")
        [Display(Name = "Çalışma Saatleri")]
        public string WorkingHours { get; set; }

        // Bir antrenörün birden fazla randevusu olabilir
        public ICollection<Appointment>? Appointments { get; set; }
    }
}