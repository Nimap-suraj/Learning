using Microsoft.EntityFrameworkCore;
using Task.Data;
using Task.Services.Implementation;
using Task.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on port 8080 (make sure matches Docker ports)
builder.WebHost.ConfigureKestrel(serverOptions => serverOptions.ListenAnyIP(8080));

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register your services
builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();

var app = builder.Build();

// Enable Swagger UI always for now (or keep inside IsDevelopment)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task API V1");
});

// Comment out HTTPS redirect if running in Docker without HTTPS certs
// app.UseHttpsRedirection();

// Add authentication middleware if you use it (otherwise skip)
app.UseAuthentication();  // Add this if authentication configured

app.UseAuthorization();

app.MapControllers();

app.Run();
