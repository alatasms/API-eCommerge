using ETicaretAPI.Application;
using ETicaretAPI.Application.Abstraction.Token;
using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
//Storage
//builder.Services.AddStorage(StorageType.Azure);
builder.Services.AddStorage<AzureStorage>();

builder.Services.AddInfrastructureServices();
builder.Services.AddCors(options=>options.AddPolicy("angularClient",policy=>policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()));
//builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
builder.Services.AddControllers(options=>options.Filters.Add<VaidationFilter>())//kendi filter'ımızı mimariye ekliyoruz.
    .AddFluentValidation(configuration=>configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())//bu assebly'i içerenen dizinden gelen tüm validator'lar için çalışır. //controller'a gelmeden çalışır.
    .ConfigureApiBehaviorOptions(options=>options.SuppressModelStateInvalidFilter=true); //default filter'ı devre dışı bırakıyoruz. true false ModelState'a düşüyor.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin",options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = tokenOptions.Audience,
            ValidIssuer=tokenOptions.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
            LifetimeValidator=(notBefore, expires, securityToken,validationParameters) =>expires!=null?expires>DateTime.UtcNow:false

           
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseCors("angularClient");
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
