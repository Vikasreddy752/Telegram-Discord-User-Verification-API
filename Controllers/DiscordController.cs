using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace TelegramAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscordController : Controller
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public DiscordController(IConfiguration config)
        {
            _config = config;
            _httpClient = new HttpClient();
        }

        [HttpGet("getuser")]
        public async Task<IActionResult> CheckUser([FromQuery] string userId)
        {
            Console.WriteLine(userId);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new { status = false, message = "userId is required" });
            }

            userId = userId.Trim(); // Remove any trailing/leading spaces

            string botToken = _config["Discord:BotToken"];
            string guildId = _config["Discord:GuildId"];
            string url = $"https://discord.com/api/v10/guilds/{guildId}/members/{userId}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bot", botToken);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new
                {
                    status = true,
                    message = "User is present in the Discord server"
                });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(new
                {
                    status = false,
                    message = "User NOT found in the Discord server"
                });
            }

            return StatusCode((int)response.StatusCode, new
            {
                status = false,
                message = "Unexpected error",
                error = response.StatusCode.ToString()
            });
        }



    }
}
