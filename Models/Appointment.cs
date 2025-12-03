using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Web_Programlama_Projesi.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        [Required]
        [Display(Name = "Randevu Tarihi")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Randevu Saati")]
        public TimeSpan Time { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Approved, Cancelled

        // İlişkiler (Foreign Keys)

        // Hangi Üye? (IdentityUser kullanacağız)
        public string MemberId { get; set; }
        public IdentityUser Member { get; set; }

        // Hangi Antrenör?
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        // Hangi Hizmet?
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}