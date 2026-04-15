using System.Security.Cryptography;
using ApiKeyAuth;
using Data;
using Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at
// https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Smart";
        options.DefaultChallengeScheme = "Smart";
        options.DefaultAuthenticateScheme = "Smart";
    })
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthHandler>("ApiKey", _ => { })
    .AddJwtBearer(
        "Jwt",
        options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                {
                    var publicKey = RSA.Create();
                    publicKey.ImportFromPem(
                        File.ReadAllText(builder.Configuration["Jwt:PublicKeyPath"]!)
                    );
                    return new[] { new RsaSecurityKey(publicKey) };
                },
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["jwt"];
                    return Task.CompletedTask;
                },
            };
        }
    )
    .AddPolicyScheme(
        "Smart",
        "ApiKey or JWT",
        options =>
        {
            options.ForwardDefaultSelector = context =>
            {
                return context.Request.Headers.ContainsKey("X-Api-Key") ? "ApiKey" : "Jwt";
            };
        }
    );
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
