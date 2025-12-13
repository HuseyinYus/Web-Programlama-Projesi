using System.ComponentModel.DataAnnotations;

namespace Web_Programlama_Projesi.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        [Display(Name = "Hizmet Adı")]
        public string ServiceName { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; } // ✨ YENİ EKLENEN KISIM (Boş bırakılabilir olsun diye ?)

        [Display(Name = "Süre (Dakika)")]
        public int Duration { get; set; }

        [Display(Name = "Ücret (TL)")]
        public decimal Price { get; set; }
    }
}