using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using YourNamespace.Controllers;
using YourNamespace.Models;

namespace TelegramAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly TelegramSettings _telegramSettings;

        public UserController(HttpClient httpClient, IOptions<TelegramSettings> telegramSettings)
        {
            _httpClient = httpClient;
            _telegramSettings = telegramSettings.Value;
        }




        [HttpGet("getuser")]
        public async Task<IActionResult> CheckUser([FromQuery] long id)
        {
            if (id <= 0)
                return BadRequest("UserId must be greater than 0");

            string url = $"https://api.telegram.org/bot{_telegramSettings.BotToken}/getChatMember" +
                         $"?chat_id={_telegramSettings.GroupId}&user_id={id}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Telegram API response: {content}");

                if (!response.IsSuccessStatusCode)
                {
                    // Telegram returned 400, 403, etc.
                    return BadRequest(new { error = content });
                }

                var jsonDoc = JsonDocument.Parse(content);
                var root = jsonDoc.RootElement;

                if (!root.GetProperty("ok").GetBoolean())
                    return Ok(new { success = false, message = "Failed to check user" });

                string status = root.GetProperty("result").GetProperty("status").GetString();
                bool isMember = status == "member" || status == "administrator" || status == "creator";

                return Ok(new
                {
                    userId = id,
                    status,
                    isMember
                });
            }
            catch (Exception ex)
            {
                // Catch unexpected errors
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }


    }
}
