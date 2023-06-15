using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
var configurationRoot = builder.Configuration.AddUserSecrets<Program>().Build();

// Add services to the container.
builder.Services.AddDbContext<dal.Data.DbContext>(options =>
{
    options.UseSqlServer(configurationRoot
        .GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomOperationIds(description => description.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null);
});

var app = builder.Build();

const string routePrefix = "api";

app.UsePathBase("/" + routePrefix);
app.UseRouting();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayOperationId();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();