using Microsoft.AspNetCore.Mvc;
using ChatBackend.Data;
using ChatBackend.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ChatBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        // Replace with Hugging Face Space URL
        private const string HuggingFaceUrl = "";


        public MessagesController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            if (string.IsNullOrWhiteSpace(message.Text))
                return BadRequest(new { message = "Text is required." });

            // Call Hugging Face AI
            var client = _httpClientFactory.CreateClient();
            var payload = JsonSerializer.Serialize(new { data = new[] { message.Text } });
            var response = await client.PostAsync(HuggingFaceUrl,
                new StringContent(payload, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, new { message = "AI service failed" });

            var resultJson = await response.Content.ReadAsStringAsync();


            message.Sentiment = resultJson;

            // Save to database
            message.CreatedAt = DateTime.Now;
            _context.Messages.Add(message);
            _context.SaveChanges();

            return Ok(message);
        }
    }
}
