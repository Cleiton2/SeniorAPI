using Microsoft.OpenApi.Models;
using SeniorAPI.Middleware;
using SeniorAPI.Service;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IConfigurationSection appSettings = builder.Configuration.GetSection("AppSettings");

string secrets = appSettings["Secret"] ?? throw new InvalidOperationException("Token não encontrado em appsettings");

builder.Services.AddSingleton(sp => new TokenService(secrets));
builder.Services.AddSingleton<PessoaService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secrets)),
            ValidIssuer = "localhost",
            ValidAudience = "localhost"
        };
    });

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Definir o esquema de segurança para o JWT Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor insira o seu token JWT com o prefixo 'Bearer' na caixa de texto abaixo.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    // Definir a segurança global para a API (aplicando o JWT a todos os endpoints)
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ValidacaoJWTMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
