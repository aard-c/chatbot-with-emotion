using Microsoft.AspNetCore.Mvc;
using ChatBackend.Data;
using ChatBackend.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ChatBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MessagesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            if (string.IsNullOrWhiteSpace(message.Text))
                return BadRequest(new { message = "Text is required." });

            string apiUrl = "http://localhost:7860/gradio_api/predict/"; 

            using var client = new HttpClient();

            var requestBody = new
            {
                data = new[] { message.Text } 
            };

            var response = await client.PostAsJsonAsync(apiUrl, requestBody);
            string responseText = "";

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
                if (result != null && result.ContainsKey("data"))
                {
                    var output = result["data"] as Newtonsoft.Json.Linq.JArray;
                    responseText = output?[0]?.ToString() ?? "No response";
                }
                else
                {
                    responseText = "Unexpected response format.";
                }
            }
            else
            {
                responseText = $"AI service failed ({response.StatusCode})";
            }

            message.Sentiment = responseText;
            message.CreatedAt = DateTime.Now;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok(message);
        }

    }
}
