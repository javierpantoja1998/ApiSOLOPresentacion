using ApiTiendaZapatillasJPL.Data;
using ApiTiendaZapatillasJPL.Helper;
using ApiTiendaZapatillasJPL.Repositories;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient(builder.Configuration.GetSection("KeyVault"));
});



//DEBEMOS RECUPERAR, DE FORMA EXPLICITA EL SECRETCLIENT INYECTADO



SecretClient secretClient =



builder.Services.BuildServiceProvider().GetService<SecretClient>();



KeyVaultSecret keyVaultSecret = await secretClient.GetSecretAsync("SqlAzure");



// Add services to the container.



string connectionString = keyVaultSecret.Value;
builder.Services.AddTransient<RepositoryZapatillas>();
builder.Services.AddTransient<RepositoryUsuarios>();
builder.Services.AddSingleton<HelperOAuthToken>();
HelperOAuthToken helper = new HelperOAuthToken(builder.Configuration);
builder.Services.AddSingleton<HelperCriptography>();



//SEGURIDAD
builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();

//AÑADIMOS LAS OPCIONES DE AUTENTIFICACION
builder.Services.AddAuthentication(helper.GetAuthenticationOptions())
    .AddJwtBearer(helper.GetJwtOptions());

builder.Services.AddDbContext<ZapatillasContext>
    (options => options.UseSqlServer(connectionString));

builder.Services.AddDbContext<UsuariosContext>
    (options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Api Outh Trabajo Tienda Zapatillas",
        Version = "v1",
        Description = "Tienda de zapatillas JPL"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json",
        "Api Tienda Zapatillas");
    options.RoutePrefix = "";
});

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
