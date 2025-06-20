using Grad_Project.Database;
using Grad_Project.Entity;
using Grad_Project.Extensions;
using Grad_Project.Interface;
using Grad_Project.Mapper;
using Grad_Project.Repository;
using Grad_Project.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Database Context
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers
builder.Services.AddControllers();

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Grad Project API",
        Version = "v1",
        Description = "Backend API for Graduation Project",
        Contact = new OpenApiContact
        {
            Name = "Abdelrahman Khaled",
            Email = "a01013946828@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/abdelrhman-khaled-b58921241/")
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token.\nExample: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins("http://127.0.0.1:5500", "https://wattsguard.netlify.app", "https://wattsguardtest.netlify.app", "http://wattsguardak4529.runasp.net", "http://127.0.0.1:5501")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Authorization
builder.Services.AddAuthorization();

// Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

// AutoMapper
builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

// Logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// Dependency Injection
builder.Services.AddScoped<ICounterRep, CounterRep>();
builder.Services.AddScoped<PowerSummaryService>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var logger = provider.GetRequiredService<ILogger<PowerSummaryService>>();
    var env = provider.GetRequiredService<IWebHostEnvironment>();
    return new PowerSummaryService(config, logger, env);
});
builder.Services.AddScoped<ModelLookupService>();
builder.Services.AddScoped<UserInputHandler>();
builder.Services.AddScoped<DataContext>();

// Build App
var app = builder.Build();

// Global Error Handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandler = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        if (exceptionHandler?.Error != null)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(exceptionHandler.Error, "Unhandled exception occurred.");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected error occurred. Please check the server logs.");
        }
    });
});

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS redirection
app.UseHttpsRedirection(); // Re-enabled for HTTPS enforcement

// Middleware order
app.UseRouting();
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();

// Request logging
app.Use(async (context, next) =>
{
    app.Logger.LogInformation($"Received request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
    app.Logger.LogInformation($"Response status: {context.Response.StatusCode}");
});

app.MapControllers();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        await dbContext.SeedDataAsync(userManager, roleManager);
        logger.LogInformation("Data seeding completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error during data seeding.");
        Console.WriteLine("Error during seed data: " + ex.Message);
        throw;
    }
}

// Run App
app.Run();