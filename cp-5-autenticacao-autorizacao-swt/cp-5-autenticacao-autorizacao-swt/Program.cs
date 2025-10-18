using cp_5_autenticacao_autorizacao_swt.Application;
using cp_5_autenticacao_autorizacao_swt.Application.Interfaces;
using cp_5_autenticacao_autorizacao_swt.Application.Services;
using cp_5_autenticacao_autorizacao_swt.Application.Validators;
using cp_5_autenticacao_autorizacao_swt.Domain.Interfaces;
using cp_5_autenticacao_autorizacao_swt.Infrastructure.Data;
using cp_5_autenticacao_autorizacao_swt.Infrastructure.Repositories;
using cp_5_autenticacao_autorizacao_swt.Presentation.Middleware;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

// Criação do builder da aplicação
var builder = WebApplication.CreateBuilder(args);

// Configuração do Serilog para logging estruturado
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Configuração dos serviços da aplicação

// Configuração do Entity Framework Core para acesso ao banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do AutoMapper para mapeamento entre DTOs e entidades
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configuração do FluentValidation para validação de dados
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

// Configuração do JWT para autenticação
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret não configurado");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

// Configuração da autorização baseada em roles
builder.Services.AddAuthorization();

// Registro dos serviços da aplicação (padrão Dependency Injection)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

// Configuração dos controllers com serialização JSON em camelCase
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// Configuração do Swagger/OpenAPI para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Autenticação e Autorização com JWT",
        Version = "v1",
        Description = "API para autenticação e autorização de usuários usando JWT (JSON Web Tokens)",
        Contact = new OpenApiContact
        {
            Name = "Equipe de Desenvolvimento",
            Email = "dev@exemplo.com"
        }
    });

    // Configuração para autenticação JWT no Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Requisito de segurança para todos os endpoints
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

    // Incluir comentários XML para documentação automática dos endpoints
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Construção da aplicação
var app = builder.Build();

// Configuração da pipeline de requisições HTTP

// Middleware de tratamento global de exceções (deve ser o primeiro)
app.UseExceptionHandling();

// Middleware de logging de requisições HTTP
app.UseSerilogRequestLogging();

// Redirecionamento HTTPS para segurança
app.UseHttpsRedirection();

// Middleware de autenticação e autorização JWT
app.UseAuthentication();
app.UseAuthorization();

// Configuração do Swagger UI para documentação da API
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Autenticação JWT v1");
    c.RoutePrefix = "swagger";
});

// Mapeamento dos controllers
app.MapControllers();

// Migração automática do banco de dados na inicialização
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        context.Database.Migrate();
        Log.Information("Banco de dados migrado com sucesso");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Erro ao migrar o banco de dados");
    }
}

Log.Information("Aplicação iniciada com sucesso");

// Inicialização da aplicação
app.Run();
