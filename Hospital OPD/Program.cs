using System.Text.Json.Serialization;
using Hospital_OPD.Data;
using Hospital_OPD.Services.Implementation;
using Hospital_OPD.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddScoped<ICategoryServices, CategoryServices>();

builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });
builder.Services.AddDbContext<AppDbContext>
    (Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("db_connection")));

builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();
builder.Services.AddScoped<IPatientServices, PatientServices>();
builder.Services.AddScoped<IDoctorServices, DoctorServices>();
builder.Services.AddScoped<IAppointmentServices, AppointmentService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
