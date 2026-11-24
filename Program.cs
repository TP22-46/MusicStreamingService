using Microsoft.AspNetCore.Mvc;
using MusicStreamingService;
using MusicStreamingService.Data;
using Microsoft.EntityFrameworkCore;
using MusicStreamingService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MusicStreamingService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<SongService>();
builder.Services.AddScoped<FavoriteService>();


builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var message = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault() ?? "Некорректные данные";

            return new BadRequestObjectResult(new { message });
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Вставьте токен"
    });

    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", doc)] = []
    });
});
builder.Services.AddSingleton<JwtService>();

builder.Services.AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]!))
            };
    });
builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("Default")));

const string FrontPolicy = "front";
builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontPolicy, policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
    );
});

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var error = context.Features
            .Get<IExceptionHandlerFeature>()?.Error;
        var logger = context.RequestServices
            .GetRequiredService<ILogger<Program>>();
        logger.LogError(error, "Ошибка в {Path}",
            context.Request.Path);

        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new
        {
            message = "Ошибка на сервере. Попробуйте позже"
        });
    });
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    if (!db.Users.Any(u => u.Role == Roles.Admin))
    {
        var admin = new User
        {
            Name = "Administrator",
            Phone = "+7 900 000-00-00",
            Role = Roles.Admin
        };
        admin.PasswordHash =
            new PasswordHasher<User>().HashPassword(admin, "admin123");
        db.Users.Add(admin);
        db.SaveChanges();
    }
}

app.MapSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseCors(FrontPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
