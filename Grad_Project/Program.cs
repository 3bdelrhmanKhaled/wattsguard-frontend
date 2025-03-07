using Grad_Project.Database;
using Grad_Project.Entity;
using Grad_Project.Extensions;
using Grad_Project.Interface;
using Grad_Project.Mapper;
using Grad_Project.Repository;
using Grad_Project.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenJwtAuth();

builder.Services.AddCustomJwtAuth(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<DataContext>();
builder.Services.AddScoped<IPicService, PicService>();
builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

builder.Services.AddScoped<IAreaRep, AreaRep>();
builder.Services.AddScoped<IDeviceRep, DeviceRep>();
builder.Services.AddScoped<ICounterRep, CounterRep>();
builder.Services.AddScoped<IScheduleRep, ScheduleRep>();
builder.Services.AddScoped<ISubAreaRep, SubAreaRep>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
