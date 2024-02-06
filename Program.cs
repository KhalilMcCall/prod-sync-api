
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace prod_sync_api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var config = builder.Configuration;

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                }
            );
        });

        //Authentication Scheme is configured with Jwt Bearer
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config["JwtSettings:Issuer"]!,
                ValidAudience = config["JwtSettings:Audience"]!,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!))
            };
        });

        //Authorization Policies configured
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("isAdmin", policy =>
            {
                policy.RequireClaim("isAdmin", "true");
            });
        });

        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<IUserRolesService, UserRolesService>();
        builder.Services.AddScoped<IIdentityService, IdentityService>(provider =>
        {
            var context = provider.GetRequiredService<ProdSyncContext>();
            return new IdentityService(context, config["JwtSettings:Issuer"]!, config["JwtSettings:Audience"]!, config["JwtSettings:Key"]!, config["PassKeySettings:Key"]!);
        });
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddDbContext<ProdSyncContext>(options =>
        options.UseSqlServer(config["ProdSync:ConnectionString"]),
        ServiceLifetime.Scoped);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();
        app.UseExceptionHandler("/error");
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
