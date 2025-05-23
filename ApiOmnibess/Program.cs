using Infrastructure.Configuration;
using Infrastructure.Configuration.Context;
using Ioc.Config;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddLogging();

builder.Services.AddEndpointsApiExplorer();
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var appsettingsConfig = builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Omnibees",
            Version = "v1",
            Description = "Omnibees Description",
            Contact = new OpenApiContact
            {
                Name = "Contact",
                Url = new Uri("https://omnibees.com/")
            },
        });

    // Configuração para autenticação com Bearer Token
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        BearerFormat = "JWT",
        Description = "Insira o token JWT.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
        {
            { securityScheme, new[] { "Bearer" } }
        };

    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.MapBusinessDependencies();
builder.Services.MapDependencies();
builder.Services.AddRepositoryDependencies(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Omnibees V1");
        c.RoutePrefix = string.Empty;
    });
    app.UseCors("AllowSpecificOrigin");
    //app.MapControllers().AllowAnonymous(); // Método para desabilitar a autenticação
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowSpecificOrigin");

#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Omnibees");
    });
});
#pragma warning restore ASP0014 // Suggest using top level route registrations

app.Run();
