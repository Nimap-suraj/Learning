//using System.Text;
//using System.Text.Json.Serialization;
//using Hospital_OPD.Data;
//using Hospital_OPD.Services.Implementation;
//using Hospital_OPD.Services.Interface;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;

//var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.ConfigureKestrel(serverOp =>serverOp.ListenAnyIP(80));



//// Add controllers and configure JSON converters
//builder.Services.AddDbContext<AppDbContext>((services, options) =>
//{
//    var configuration = services.GetRequiredService<IConfiguration>();
//    options.UseSqlServer(
//        configuration.GetConnectionString("db_connection"),
//        sqlOptions =>
//        {
//            sqlOptions.EnableRetryOnFailure(
//                maxRetryCount: 5,
//                maxRetryDelay: TimeSpan.FromSeconds(10),
//                errorNumbersToAdd: null);
//            sqlOptions.CommandTimeout(60); // 60 seconds command timeout
//        });
//    options.EnableDetailedErrors(builder.Environment.IsDevelopment());
//    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
//});

//// Add database context
////builder.Services.AddDbContext<AppDbContext>(options =>
////  options.UseSqlServer(builder.Configuration.GetConnectionString("db_connection")));
////builder.Services.AddDbContext<AppDbContext>(options =>
////  options.UseSqlServer(builder.Configuration.GetConnectionString("db_connection"),
////                       sqlOptions => sqlOptions.EnableRetryOnFailure()));

//// Register custom services
//builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();
//builder.Services.AddScoped<IPatientServices, PatientServices>();
//builder.Services.AddScoped<IDoctorServices, DoctorServices>();
//builder.Services.AddScoped<IAppointmentServices, AppointmentService>();
//builder.Services.AddScoped<IMedicalRecord, MedicalService>();
//builder.Services.AddScoped<ITokenService, TokenService>();
//builder.Services.AddScoped<IReportService, AppointmentService>();
//builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
//builder.Services.AddTransient<IMailService, MailService>();


//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();

//// JWT Authentication configuration


//var jwtKey = builder.Configuration["JwtSettings:Key"];
//var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
//var jwtAudience = builder.Configuration["JwtSettings:Audience"];

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidIssuer = jwtIssuer,
//            ValidateAudience = true,
//            ValidAudience = jwtAudience,
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
//            ValidateLifetime = true
//        };
//    });

//builder.Services.AddAuthorization();

//// Swagger with JWT Bearer Token Support
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "Hospital_OPD API",
//        Version = "v1",
//        Description = "API documentation for the Hospital Outpatient Management System"
//    });

//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer"
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            Array.Empty<string>()
//        }
//    });
//});


//var app = builder.Build();
//app.UseMiddleware<GlobalExceptionMiddleware>();


//// Configure middleware pipeline
//if (app.Environment.IsDevelopment()  || app.Environment.IsProduction())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hospital_OPD API v1");
//    });
//}

//app.UseHttpsRedirection();

//app.UseAuthentication(); // Must come before UseAuthorization
//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using System.Text;
using System.Text.Json.Serialization;
using Hospital_OPD.Data;
using Hospital_OPD.Services.Implementation;
using Hospital_OPD.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(serverOptions => serverOptions.ListenAnyIP(80));

// Configure DbContext with SQL Server and retry policy
builder.Services.AddDbContext<AppDbContext>((services, options) =>
{
    var configuration = services.GetRequiredService<IConfiguration>();
    options.UseSqlServer(
        configuration.GetConnectionString("db_connection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
            sqlOptions.CommandTimeout(60); // 60 seconds timeout
        });

    options.EnableDetailedErrors(builder.Environment.IsDevelopment());
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

// Register services
builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();
builder.Services.AddScoped<IPatientServices, PatientServices>();
builder.Services.AddScoped<IDoctorServices, DoctorServices>();
builder.Services.AddScoped<IAppointmentServices, AppointmentService>();
builder.Services.AddScoped<IMedicalRecord, MedicalService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IReportService, AppointmentService>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// JWT Authentication configuration
var jwtKey = builder.Configuration["JwtSettings:Key"];
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
var jwtAudience = builder.Configuration["JwtSettings:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

// Swagger with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hospital_OPD API",
        Version = "v1",
        Description = "API documentation for the Hospital Outpatient Management System"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Enable Swagger only in development environment (you can add production if needed)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hospital_OPD API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
