using Microsoft.AspNetCore.Mvc;
using ChatBackend.Data;
using ChatBackend.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System;

namespace ChatBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        // Hugging Face Space endpoint
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

            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(300); // increase timeout

            var payload = JsonSerializer.Serialize(new { data = new[] { message.Text } });

            var response = await client.PostAsync(
                HuggingFaceUrl,
                new StringContent(payload, Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, new { message = "AI service failed" });

            var resultJson = await response.Content.ReadAsStringAsync();

            // Using JsonNode to parse response
            var jsonDoc = JsonNode.Parse(resultJson);
            message.Sentiment = jsonDoc?["data"]?[0]?.ToString() ?? "unknown";

            message.CreatedAt = DateTime.Now;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok(message);
        }
    }


    public class HfResponse
    {
        public string[] Data { get; set; }
    }
}
