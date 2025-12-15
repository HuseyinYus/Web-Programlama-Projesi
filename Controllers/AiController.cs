using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json; // JSON işlemleri için
using System.Net.Http.Headers;

namespace Web_Programlama_Projesi.Controllers
{
    public class AiController : Controller
    {
        private readonly IConfiguration _configuration;

        public AiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 1. Formu Göster (GET)
        public IActionResult Index()
        {
            return View();
        }

        // 2. Yapay Zekaya Sor (POST)
        [HttpPost]
        public async Task<IActionResult> GetRecommendation(string yas, string kilo, string boy, string hedef)
        {
            // API Key'i ayarlardan çek
            var apiKey = _configuration["OpenAiSettings:ApiKey"];

            if (string.IsNullOrEmpty(apiKey))
            {
                ViewBag.Result = "Hata: API Anahtarı bulunamadı.";
                return View("Index");
            }

            // OpenAI'a sorulacak soruyu hazırla (Prompt)
            var prompt = $"Ben {yas} yaşında, {boy} cm boyunda ve {kilo} kg ağırlığında biriyim. " +
                         $"Hedefim: {hedef}. " +
                         $"Bana maddeler halinde kısa ve öz bir günlük beslenme ve egzersiz programı önerir misin? (Lütfen Türkçe cevap ver)";

            // HTTP İsteği Hazırlığı
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var requestBody = new
                {
                    model = "gpt-3.5-turbo", // Hızlı ve ucuz model
                    messages = new[]
                    {
                        new { role = "system", content = "Sen profesyonel bir spor ve diyet koçusun." },
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 500 // Cevap çok uzun olmasın
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                try
                {
                    // OpenAI'a isteği gönder
                    var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var resultJson = await response.Content.ReadAsStringAsync();
                        using (JsonDocument doc = JsonDocument.Parse(resultJson))
                        {
                            // Gelen karmaşık JSON içinden sadece cevabı al
                            string aiResponse = doc.RootElement
                                .GetProperty("choices")[0]
                                .GetProperty("message")
                                .GetProperty("content")
                                .GetString();

                            ViewBag.Result = aiResponse; // Cevabı View'e taşı
                        }
                    }
                    else
                    {
                        ViewBag.Result = "Hata oluştu. Lütfen bakiyenizi veya API anahtarınızı kontrol edin.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Result = "Bağlantı hatası: " + ex.Message;
                }
            }

            // Kullanıcının girdiği veriler kaybolmasın diye geri gönderiyoruz
            ViewBag.Yas = yas;
            ViewBag.Kilo = kilo;
            ViewBag.Boy = boy;
            ViewBag.Hedef = hedef;

            return View("Index");
        }
    }
}