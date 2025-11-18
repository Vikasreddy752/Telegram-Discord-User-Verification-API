using Microsoft.AspNetCore.Mvc;
using TelegramAPI.Data;
using TelegramAPI.Data.DTO;

namespace TelegramAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TelegramController : Controller
    {
        private readonly Appdb appdb;

        public TelegramController(Appdb _appdb)
        {
            //appdb = _appdb;
        }

        [HttpPost]
        public async Task<IActionResult> GetUser( long telegramUserId)
        {
            if (telegramUserId == null || telegramUserId<= 0)
            {
                return BadRequest("Invalid user data.");
            }

            bool exists = appdb.Users.Any(u => u.TelegramUserId == telegramUserId);


            if (exists)
            {
                Console.WriteLine("✅ User exists in the database.");
                return Ok(new { message = "User Exists", userId = telegramUserId });
            }
            else
            {
                Console.WriteLine("❌ User not found in the database.");
                return Ok(new { message = "User does not exist", userId = "" });
            }
        }


    }
}
