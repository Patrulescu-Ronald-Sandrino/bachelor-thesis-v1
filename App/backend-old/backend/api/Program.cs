using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using DbContext = bll.Data.DbContext;

var builder = WebApplication.CreateBuilder(args);
var configurationRoot = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<DbContext>(options =>
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
// Add authentication and authorization
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configurationRoot["JwtConfig:Issuer"],
        ValidAudience = configurationRoot["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationRoot["JwtConfig:Secret"]))
    });

var app = builder.Build();

const string routePrefix = "api";

app.UsePathBase("/" + routePrefix);
app.UseRouting();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.DisplayOperationId(); });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();