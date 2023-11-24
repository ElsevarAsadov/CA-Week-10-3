using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.Admin.Models;

namespace WebApplication1.Areas.Admin.Data
{
    public class AdminDbContext : DbContext
    {
        public DbSet<SliderModel> Slider { get; set; }
        public AdminDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
