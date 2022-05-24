using Application.Extentions;
using Application.Services.Customer;
using Customers.API.Middlewares;
using Cutomers.INFRASTRUCTURE.DbConfiguration;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddDbContext<CustomerDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("CustomerDbConnection"),
        b => b.MigrationsAssembly("Customers.INFRASTRUCTURE"));
});




builder.Services.AddApplicationServiceCollection();
builder.Services.AddRepositories();

builder.Services.AddControllers().AddFluentValidation(
    fv => {
        fv.RegisterValidatorsFromAssembly(typeof(Program).Assembly);
        fv.AutomaticValidationEnabled = false;
    }
).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI  at https://aka.ms/aspnetcore/swashbuckle
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
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();