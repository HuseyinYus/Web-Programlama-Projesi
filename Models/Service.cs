using System.ComponentModel.DataAnnotations;

namespace Web_Programlama_Projesi.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        [Required]
        [Display(Name = "Hizmet Adı")]
        public string ServiceName { get; set; } // Fitness, Pilates vb.

        [Display(Name = "Süre (Dakika)")]
        public int Duration { get; set; }

        [Display(Name = "Ücret")]
        public decimal Price { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
    }
}