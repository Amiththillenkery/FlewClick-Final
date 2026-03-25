using System.Text;
using FlewClick.Api.Endpoints;
using FlewClick.Api.Hubs;
using FlewClick.Application;
using FlewClick.Domain.Exceptions;
using FlewClick.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "FlewClick-Dev-Secret-Key-Change-In-Production-256bit";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "FlewClick",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "FlewClick",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    context.Token = accessToken;
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FlewClick API", Version = "v1" });
});

builder.Services.AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection") ?? "Host=localhost;Port=5432;Database=flewclick;Username=postgres;Password=postgres",
        name: "database");

var app = builder.Build();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlewClick API v1"));

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        context.Response.ContentType = "application/json";

        if (exception is EntityNotFoundException notFound)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new { error = notFound.Message });
        }
        else if (exception is DomainException domain)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { error = domain.Message });
        }
        else if (exception is FluentValidation.ValidationException validation)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Validation failed",
                details = validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            });
        }
        else
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
        }
    });
});

app.MapHealthChecks("/healthz");
app.MapEndpointGroups();
app.MapHub<ChatHub>("/hubs/chat");

app.Run();
