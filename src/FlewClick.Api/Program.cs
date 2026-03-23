using FlewClick.Api.Endpoints;
using FlewClick.Application;
using FlewClick.Domain.Exceptions;
using FlewClick.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

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

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlewClick API v1"));

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

app.Run();
