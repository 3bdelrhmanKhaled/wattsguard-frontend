using Grad_Project.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Grad_Project.Database
{
    public class DataContext:IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserDevice>()
            .HasKey(ud => new { ud.userId, ud.deviceId });
            base.OnModelCreating(builder);
           

        }

        public DbSet<Device> devices { get; set; }
        public DbSet<Area> areas { get; set; }
        public DbSet<Counter> counters { get; set; }
        public DbSet<CounterData> counterDatas { get; set; }
        public DbSet<Schedule> schedules { get; set; }
        public DbSet<SubArea> subAreas { get; set; }
        public DbSet<UserDevice> userDevices { get; set; }
        
    }
}
