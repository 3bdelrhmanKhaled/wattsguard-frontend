using Grad_Project.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Grad_Project.Database
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        private readonly ILogger<DataContext> _logger;

        public DataContext(DbContextOptions<DataContext> options, ILogger<DataContext> logger) : base(options)
        {
            _logger = logger;
        }

        public DbSet<Counter> counters { get; set; } // جدول العدادات
        public DbSet<CounterData> counterData { get; set; } // جدول بيانات القراءات (تصحيح التسمية)
        public DbSet<Address> addresses { get; set; } // جدول العناوين (تصحيح التسمية)
        public DbSet<OfficialReading> officialReadings { get; set; } // جدول القراءات الرسمية (تصحيح التسمية)

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // علاقة واحد إلى واحد بين AppUser و Address
            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);

            // علاقة واحد إلى متعدد بين AppUser و Counters
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Counters)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.userId);

            // علاقة واحد إلى متعدد بين Counter و CounterData
            modelBuilder.Entity<Counter>()
                .HasMany(c => c.CounterData)
                .WithOne(d => d.Counter)
                .HasForeignKey(d => d.CounterId);

            // علاقة واحد إلى متعدد بين Counter و OfficialReading
            modelBuilder.Entity<OfficialReading>()
                .HasOne(or => or.Counter)
                .WithMany()
                .HasForeignKey(or => or.CounterId);

            // جعل CounterId فريد في جدول counters
            modelBuilder.Entity<Counter>()
                .HasIndex(c => c.CounterId)
                .IsUnique();
        }

        public async Task SeedDataAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!Users.Any())
            {
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                var user = await userManager.FindByNameAsync("TestUser");
                if (user == null)
                {
                    user = new AppUser
                    {
                        UserName = "TestUser",
                        Email = "test@example.com",
                        idNumber = "123456789",
                        PhoneNumber = "0123456789"
                    };
                    var result = await userManager.CreateAsync(user, "Password123!");
                    if (!result.Succeeded)
                    {
                        _logger.LogError("Failed to create TestUser: {Errors}", string.Join(", ", result.Errors));
                        throw new Exception("Failed to create TestUser: " + string.Join(", ", result.Errors));
                    }
                }

                var adminUser = await userManager.FindByNameAsync("AdminUser");
                if (adminUser == null)
                {
                    adminUser = new AppUser
                    {
                        UserName = "AdminUser",
                        Email = "admin@example.com",
                        idNumber = "987654321",
                        PhoneNumber = "0987654321"
                    };
                    var result = await userManager.CreateAsync(adminUser, "Admin123!");
                    if (!result.Succeeded)
                    {
                        _logger.LogError("Failed to create AdminUser: {Errors}", string.Join(", ", result.Errors));
                        throw new Exception("Failed to create AdminUser: " + string.Join(", ", result.Errors));
                    }
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                if (string.IsNullOrEmpty(user.Id))
                {
                    _logger.LogError("User ID is null or empty for TestUser after creation");
                    throw new Exception("User ID is null or empty for TestUser after creation");
                }

                var counter = await counters.FirstOrDefaultAsync(c => c.CounterId == "COUNTER_001");
                if (counter == null)
                {
                    counter = new Counter
                    {
                        id = Guid.NewGuid().ToString(),
                        CounterId = "COUNTER_001",
                        userId = user.Id
                    };
                    await counters.AddAsync(counter);
                }

                var address = await addresses.FirstOrDefaultAsync(a => a.UserId == user.Id);
                if (address == null)
                {
                    address = new Address
                    {
                        UserId = user.Id,
                        Governorate = "الدقهلية",
                        City = "المنصورة",
                        Region = "احمد ماهر",
                        Street = "شارع 1"
                    };
                    await addresses.AddAsync(address);
                }

                var officialReading = await officialReadings.FirstOrDefaultAsync(or => or.CounterId == "COUNTER_001");
                if (officialReading == null)
                {
                    officialReading = new OfficialReading
                    {
                        CounterId = "COUNTER_001",
                        Reading = 703920,
                        ReadingDate = DateTime.UtcNow
                    };
                    await officialReadings.AddAsync(officialReading);
                }

                await SaveChangesAsync();
            }
        }
    }
}