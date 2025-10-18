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
builder.Services.AddScoped<INoteService, NoteService>();

// Registro do serviço de blacklist como Singleton (conforme especificado no PDF)
builder.Services.AddSingleton<ITokenBlacklistService, InMemoryTokenBlacklistService>();

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
        Title = "🔐 (CP5) Autenticação e Autorização com JWT - SafeScribe API",
        Version = "v1.0.0",
        Description = @"
## 🚀 **SafeScribe - Plataforma de Gestão de Documentos Sensíveis**

A startup **SafeScribe** está desenvolvendo uma plataforma inovadora para gestão de notas e documentos sensíveis voltada para equipes corporativas. A segurança e o controle de acesso são os pilares do produto. Eles precisam de um backend robusto que garanta que apenas usuários autenticados tenham acesso ao sistema e que suas permissões sejam aplicadas de forma rigorosa.

### 🎯 **Missão**
Construir o núcleo da API RESTful da SafeScribe, implementando um sistema de autenticação e autorização seguro utilizando JSON Web Tokens (JWT).

### 🛠️ **Requisitos Técnicos**
- **Framework**: ![.NET](https://img.shields.io/badge/.NET-8.0-purple?style=flat-square&logo=dotnet)
- **Tipo de Projeto**: ![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-blue?style=flat-square&logo=aspnet)
- **Autenticação**: ![JWT](https://img.shields.io/badge/JWT-JSON%20Web%20Tokens-orange?style=flat-square&logo=jsonwebtokens)
- **Pacote**: `Microsoft.AspNetCore.Authentication.JwtBearer`

### 📅 **Informações do Projeto**
- **Data de Entrega**: 20/10/2025
- **Grupo**: Até 3 pessoas
- **Status**: ![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow?style=flat-square)

### 👥 **Integrantes do CP**
| Nome | RM | GitHub |
|------|----|---------|
| Amanda Mesquita Cirino Da Silva | RM559177 | [![GitHub](https://img.shields.io/badge/GitHub-mandyy14-black?style=flat-square&logo=github)](https://github.com/mandyy14) |
| Journey Tiago Lopes Ferreira | RM556071 | [![GitHub](https://img.shields.io/badge/GitHub-JouTiago-black?style=flat-square&logo=github)](https://github.com/JouTiago) |
| Paulo André Carminati | RM557881 | [![GitHub](https://img.shields.io/badge/GitHub-carmipa-black?style=flat-square&logo=github)](https://github.com/carmipa) |

### 🔗 **Repositórios**
- **Repositório CP**: [![GitHub](https://img.shields.io/badge/GitHub-Advanced%20Business%20Development-black?style=flat-square&logo=github)](https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025)
- **Repositório Projeto**: [![GitHub](https://img.shields.io/badge/GitHub-CP5%20JWT%20API-black?style=flat-square&logo=github)](https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025/tree/main/cp-5-autenticacao-autorizacao-swt)

### 🔐 **Funcionalidades da API**
- ✅ **Autenticação JWT** - Login, registro e logout com blacklist
- ✅ **Autorização por Roles** - Controle de acesso (Leitor, Editor, Admin)
- ✅ **Refresh Tokens** - Renovação automática de tokens
- ✅ **Gestão de Notas** - CRUD completo para documentos sensíveis
- ✅ **Logout Avançado** - Sistema de blacklist para invalidar tokens
- ✅ **Validação de Dados** - Validação robusta com FluentValidation
- ✅ **Documentação Swagger** - Interface interativa para testes
- ✅ **Logging Estruturado** - Logs detalhados com Serilog
- ✅ **Tratamento de Erros** - Middleware global de exceções

### 🚀 **Como Usar**
1. **Registre-se** usando `POST /api/auth/register`
2. **Faça login** usando `POST /api/auth/login`
3. **Use o token JWT** no header `Authorization: Bearer {token}`
4. **Gerencie notas** usando `POST /api/v1/notas` (Editor/Admin)
5. **Faça logout** usando `POST /api/auth/logout` (invalida token)
6. **Teste os endpoints** protegidos com autenticação

### 📚 **Tecnologias Utilizadas**
![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=c-sharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=flat-square&logo=aspnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-512BD4?style=flat-square&logo=entity-framework&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat-square&logo=microsoft-sql-server&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=flat-square&logo=jsonwebtokens&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=flat-square&logo=swagger&logoColor=black)
![Serilog](https://img.shields.io/badge/Serilog-000000?style=flat-square&logo=serilog&logoColor=white)
",
        Contact = new OpenApiContact
        {
            Name = "👥 Equipe SafeScribe - CP5",
            Email = "safescribe@fiap.com.br",
            Url = new Uri("https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
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
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// Middleware de logging de requisições HTTP
app.UseSerilogRequestLogging();

// Redirecionamento HTTPS para segurança
app.UseHttpsRedirection();

// Middleware de autenticação e autorização JWT
app.UseAuthentication();
app.UseAuthorization();

// Middleware personalizado para verificar tokens na blacklist (conforme especificado no PDF)
app.UseMiddleware<JwtBlacklistMiddleware>();

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
