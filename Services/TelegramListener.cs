using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using YourNamespace.Models;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelegramController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly TelegramSettings _telegramSettings;

        public TelegramController(HttpClient httpClient, IOptions<TelegramSettings> telegramSettings)
        {
            _httpClient = httpClient;
            _telegramSettings = telegramSettings.Value;
        }

        // ✅ POST: /api/telegram/check-user
        [HttpPost("check-user")]
        public async Task<IActionResult> CheckUser([FromBody] TelegramUserRequest request)
        {
            if (request == null || request.UserId == 0)
                return BadRequest("UserId is required");

            string url = $"https://api.telegram.org/bot{_telegramSettings.BotToken}/getChatMember" +
                         $"?chat_id={_telegramSettings.GroupId}&user_id={request.UserId}";

            var response = await _httpClient.GetStringAsync(url);
            var jsonDoc = JsonDocument.Parse(response);
            var root = jsonDoc.RootElement;

            // Read Telegram response
            if (!root.GetProperty("ok").GetBoolean())
                return Ok(new { success = false, message = "Failed to check user" });

            string status = root.GetProperty("result").GetProperty("status").GetString();

            bool isMember = status == "member" || status == "administrator" || status == "creator";

            return Ok(new
            {
                userId = request.UserId,
                status,
                isMember
            });
        }
    }

    public class TelegramUserRequest
    {
        public long UserId { get; set; }
    }
}
