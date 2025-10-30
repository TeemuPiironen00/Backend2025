
using Backend_2024.Middleware;
using Backend_2025.Middleware;
using Backend2025.Middleware;
using Backend2025.Models;
using Backend2025.Repositories;
using Backend2025.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
namespace Backend2025
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            builder.Services.AddDbContext<MessageContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Backend2025")));
            builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions,BasicAuthenticationHandler>("BasicAuthentication", null);
            builder.Services.AddControllers();
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            using (var scope = app.Services.CreateScope())
            {
                MessageContext dbcontext = scope.ServiceProvider.GetRequiredService<MessageContext>();
                dbcontext.Database.EnsureCreated();

            }
            app.UseHttpsRedirection();
            //app.UseMiddleware<>();
            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            

            app.MapControllers();

            app.Run();
        }
    }
}
