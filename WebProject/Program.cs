using Microsoft.EntityFrameworkCore;
using WebApiProjectWithDto.Data;
using WebApiProjectWithDto.Services;
using WebProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  --- 
builder.Services.AddDbContext
    <AppDbContext>
    (options =>options.UseSqlServer
    (builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>(); // Add this line
builder.Services.AddScoped<ICategoryInterface, CategoryService>(); // Add this line



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
