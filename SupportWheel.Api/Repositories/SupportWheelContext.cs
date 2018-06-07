
using Microsoft.EntityFrameworkCore;
using SupportWheel.Api.Models;

namespace SupportWheel.Api.Repositories
{
    public class SupportWheelContext : DbContext
    {
        public SupportWheelContext()
            : base(new DbContextOptionsBuilder().UseInMemoryDatabase("SupportWheel").Options)
        {
        }

        public DbSet<Engineer> Engineers { get; set; }
    }
}