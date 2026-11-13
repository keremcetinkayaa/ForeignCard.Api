using FluentValidation;
using ForeignCard.Api.BackgroundServices;
using ForeignCard.Api.Middleware;
using ForeignCard.Common.Abstractions;
using ForeignCard.Operation.Cache;
using ForeignCard.Operation.Customers;
using ForeignCard.Operation.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connStr = builder.Configuration.GetConnectionString("Bank")!;

builder.Services.AddSingleton<IDbConnectionFactory>(new SqlConnectionFactory(connStr));
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "ForeignCard:";
});

builder.Services.AddSingleton<ICacheService, RedisCacheService>();
builder.Services.AddSingleton<IApiLogWriter, ForeignCard.Operation.Data.ApiLogWriter>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerHandler).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(CreateCustomerHandler).Assembly);

builder.Services.AddHttpClient<IExchangeRateProvider, TcmbExchangeRateProvider>();
builder.Services.AddHostedService<ExchangeRateJob>();
builder.Services.AddSingleton<ICountryProvider, StaticCountryProvider>();
builder.Services.AddHostedService<CountryLoaderJob>();
builder.Services.AddHostedService<AdminSeederJob>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var idValue = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                              ?? context.Principal?.FindFirst("nameid")?.Value;

                if (!int.TryParse(idValue, out var customerId))
                {
                    context.Fail("Gecersiz token");
                    return;
                }

                var factory = context.HttpContext.RequestServices.GetRequiredService<IDbConnectionFactory>();
                using var conn = factory.CreateConnection();

                var row = await Dapper.SqlMapper.QuerySingleOrDefaultAsync<dynamic>(
                    conn,
                    ForeignCard.Common.Constants.ProcedureConstants.SP_CustomerGet,
                    new { CustomerId = customerId },
                    commandType: System.Data.CommandType.StoredProcedure);

                if (row == null || !(bool)row.IsActive)
                    context.Fail("Musteri pasif");
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ApiLogMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();