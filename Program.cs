
using Book_Rental.Data;
using Book_Rental.Interfaces;
using Book_Rental.Middleware;
using Book_Rental.Repositories;
using Book_Rental.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Book_Rental
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // SQL connection
            builder.Services.AddDbContext<AppDbContext>(opts =>
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Dependency Injection
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IBookRequestRepository, BookRequestRepository>();
            builder.Services.AddScoped<IBookRequestService, BookRequestService>();
            builder.Services.AddScoped<IRentalHistoryRepository, RentalHistoryRepository>();
            builder.Services.AddScoped<IRentalHistoryService, RentalHistoryService>();

            // CORS Configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Development", policy =>
                {
                    policy.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:5173")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });

                options.AddPolicy("Production", policy =>
                {
                    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
                        ?? new[] { "https://yourdomain.com", "https://www.yourdomain.com" };
                    
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // JWT Authentication
            var jwtSecret = builder.Configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new InvalidOperationException("JWT Secret is not configured. Please set it in User Secrets (Development) or Environment Variables (Production).");
            }

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
                };
            });

            var app = builder.Build();

            // Seed database with admin user
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    var configuration = services.GetRequiredService<IConfiguration>();
                    var seeder = new DbSeeder(context, configuration);
                    await seeder.SeedAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.UseMiddleware<SecurityHeadersMiddleware>();

            app.UseHttpsRedirection();

            // Use CORS - must be after UseHttpsRedirection and before UseAuthentication
            app.UseCors(app.Environment.IsDevelopment() ? "Development" : "Production");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
