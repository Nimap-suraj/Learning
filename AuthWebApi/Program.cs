using System.Text;
using AuthWebApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>
    (Options => Options.UseSqlServer
    (builder.Configuration.GetConnectionString("ConnectionStrings")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    Options =>
    {
        Options.Password.RequiredLength = 6;
        Options.Password.RequireNonAlphanumeric = false;
        Options.Password.RequireDigit = false;
        Options.Password.RequireLowercase = false;
        Options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(Options =>
{
    Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    Options =>
    {
        Options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["jwt:Issuer"]
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"]))

        };
    });
builder.Services.AddAuthorization(Options =>
{
    Options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    Options.AddPolicy("DoctorPolicy", policy => policy.RequireRole("Doctor"));
    Options.AddPolicy("ReceptionistPolicy", policy => policy.RequireRole("Receptionist"));
});
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
