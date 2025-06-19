using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using backend.Dto;
using backend.Models;
using backend.Seed;
using backend.Data;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");

var configuration = builder.Configuration;

// PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", p =>
    {
        p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Admin API", Version = "v1" });
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

// Создание БД + сиды
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DataSeeder.Seed(db);
}

// === ЭНДПОЙНТЫ ===

app.MapPost("/api/auth/login", (LoginDto creds) =>
{
    if (creds.Email == "admin@mirra.dev" && creds.Password == "admin123")
    {
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                new System.Security.Claims.Claim("email", creds.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Results.Ok(new { token = jwt });
    }
    return Results.Unauthorized();
});

app.MapGet("/api/clients", [Microsoft.AspNetCore.Authorization.Authorize] async (AppDbContext db) => await db.Clients.ToListAsync());

app.MapGet("/api/clients/{id}", [Microsoft.AspNetCore.Authorization.Authorize] async (int id, AppDbContext db) =>
    await db.Clients.FindAsync(id) is Client c ? Results.Ok(c) : Results.NotFound());

app.MapPost("/api/clients", [Microsoft.AspNetCore.Authorization.Authorize] async (Client c, AppDbContext db) => {
    db.Clients.Add(c);
    await db.SaveChangesAsync();
    return Results.Created($"/api/clients/{c.Id}", c);
});

app.MapPut("/api/clients/{id}", [Microsoft.AspNetCore.Authorization.Authorize] async (int id, Client input, AppDbContext db) => {
    var c = await db.Clients.FindAsync(id);
    if (c == null) return Results.NotFound();
    c.Name = input.Name;
    c.Email = input.Email;
    c.BalanceT = input.BalanceT;
    await db.SaveChangesAsync();
    return Results.Ok(c);
});

app.MapDelete("/api/clients/{id}", [Microsoft.AspNetCore.Authorization.Authorize] async (int id, AppDbContext db) => {
    var c = await db.Clients.FindAsync(id);
    if (c == null) return Results.NotFound();
    db.Clients.Remove(c);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/api/payments", [Microsoft.AspNetCore.Authorization.Authorize] async (AppDbContext db, int take = 5) =>
    await db.Payments.Include(p => p.Client)
                     .OrderByDescending(p => p.Timestamp)
                     .Take(take)
                     .ToListAsync());

app.MapGet("/api/rate", [Microsoft.AspNetCore.Authorization.Authorize] async (AppDbContext db) =>
    await db.Rates.FirstOrDefaultAsync());

app.MapPost("/api/rate", [Microsoft.AspNetCore.Authorization.Authorize] async (AppDbContext db, Rate updatedRate) =>
{
    var rate = await db.Rates.FirstOrDefaultAsync();
    if (rate != null)
    {
        rate.TokenRate = updatedRate.TokenRate;
        await db.SaveChangesAsync();
    }
    return Results.Ok(rate);
});

app.Run();