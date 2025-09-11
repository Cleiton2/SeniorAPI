using SeniorAPI.Middleware;
using SeniorAPI.Service;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IConfigurationSection appSettings = builder.Configuration.GetSection("AppSettings");

string secrets = appSettings["Secret"] ?? throw new("Token não encontrado em appsettings");

builder.Services.AddSingleton(sp => new TokenService(secrets));

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

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapWhen(context => context.Request.Path.StartsWithSegments("/api/Autenticacao/login"), appBuilder =>
{
    appBuilder.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
});

app.UseMiddleware<ValidacaoJWTMiddleware>();

app.UseAuthentication();

app.MapControllers();

app.Run();
