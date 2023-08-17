using System.Text;
using bll;
using bll.Data;
using bll.Services;
using bll.Services.Helpers;
using domain.Repository;
using domain.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using DbContext = bll.Data.DbContext;

var builder = WebApplication.CreateBuilder(args);

#region services

var configuration = builder.Configuration.GetSection(nameof(AppSettings));
builder.Services.Configure<AppSettings>(configuration);
var appSettings = configuration.Get<AppSettings>();

builder.Services.AddDbContext<DbContext>(options =>
{
    options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection);
});

builder.Services.AddSingleton<JwtTokenHelper>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddOptions();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = appSettings.JwtConfig.Issuer,
        ValidAudience = appSettings.JwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtConfig.Secret))
    });

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomOperationIds(description =>
        description.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null);
});

#endregion

var app = builder.Build();

#region pipeline

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.DisplayOperationId(); });
}

app.UseHttpsRedirection();

app.UseRouting();

// app.UseCors(options => options
//     .WithOrigins(new[] { appSettings.FrontendUrl })
//     .AllowAnyHeader()
//     .AllowAnyMethod()
//     .AllowCredentials()
// );

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

#endregion

app.Run();