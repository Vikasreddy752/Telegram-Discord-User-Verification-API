using Microsoft.EntityFrameworkCore;
using TelegramAPI.Data.Model;

namespace TelegramAPI.Data
{
    public class Appdb : DbContext
    {
        public Appdb(DbContextOptions<Appdb> options) : base(options)
        { }

        public DbSet<UserModel> Users { get; set; }
    }
}
