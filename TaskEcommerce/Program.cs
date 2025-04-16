using Microsoft.EntityFrameworkCore; // a library used to interact with the database.
using TaskEcommerce.Context;
using TaskEcommerce.Services.Implementaion;
using TaskEcommerce.Services.Implementation;
using TaskEcommerce.Services.Interface;
// Includes your custom DataContext class and
// Connection to database and maps Models into Table (Database)

var builder = WebApplication.CreateBuilder(args); // Start building a app

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();


// Adds support for Controllers, which handle HTTP requests (like GET, POST).
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

//Adds Swagger, a tool that automatically creates API documentation and a UI for testing your API
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registers your DataContext for dependency injection (so it can be used wherever needed).


builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("db_connection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
//Only shows Swagger when running in Development mode (not in Production).
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
