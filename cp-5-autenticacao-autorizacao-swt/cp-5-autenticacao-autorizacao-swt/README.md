# 🔐 SafeScribe API - Autenticação e Autorização com JWT

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=for-the-badge&logo=aspnetcore&logoColor=white)](https://docs.microsoft.com/aspnet/core/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-8.0-512BD4?style=for-the-badge&logo=entityframework&logoColor=white)](https://docs.microsoft.com/ef/)
[![JWT](https://img.shields.io/badge/JWT-Authentication-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)](https://jwt.io/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server/)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)](https://swagger.io/)
[![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)](LICENSE)

> 🚀 **API REST completa para gestão de documentos sensíveis com autenticação JWT robusta, implementada seguindo Clean Architecture e Clean Code para a startup SafeScribe**

## 📋 Índice

- [🎯 Visão Geral](#-visão-geral)
- [🏢 Sobre a SafeScribe](#-sobre-a-safescribe)
- [🏗️ Arquitetura](#️-arquitetura)
- [🛠️ Tecnologias](#️-tecnologias)
- [📁 Estrutura do Projeto](#-estrutura-do-projeto)
- [🚀 Funcionalidades](#-funcionalidades)
- [⚙️ Configuração](#️-configuração)
- [🔧 Instalação](#-instalação)
- [📖 Documentação da API](#-documentação-da-api)
- [🧪 Testes](#-testes)
- [🔐 Segurança](#-segurança)
- [📊 Banco de Dados](#-banco-de-dados)
- [📝 Logs](#-logs)
- [👥 Equipe](#-equipe)
- [🤝 Contribuição](#-contribuição)
- [📄 Licença](#-licença)

## 🎯 Visão Geral

A **SafeScribe API** é uma solução robusta e escalável para gestão de documentos sensíveis, desenvolvida especificamente para equipes corporativas que necessitam de controle rigoroso de acesso e segurança de dados. O projeto implementa autenticação JWT (JSON Web Tokens) com sistema de refresh tokens e controle de acesso baseado em roles, seguindo os princípios de **Clean Architecture** e **Clean Code**.

### ✨ Principais Características

- 🔐 **Autenticação JWT** com tokens seguros e renovação automática
- 🔄 **Refresh Tokens** para sessões persistentes e seguras
- 👥 **Sistema de Roles** (Leitor, Editor, Admin) com controle granular
- 📝 **Gestão de Notas** com controle de acesso por usuário
- 🛡️ **Validação robusta** de dados com FluentValidation
- 📚 **Documentação Swagger** completa e interativa
- 🏗️ **Clean Architecture** bem estruturada e testável
- 📊 **Logging detalhado** com Serilog para auditoria
- 🗄️ **Entity Framework Core** para persistência de dados
- 🚫 **Sistema de Blacklist** para logout seguro de tokens

## 🏢 Sobre a SafeScribe

A **SafeScribe** é uma startup inovadora focada no desenvolvimento de soluções para gestão de documentos sensíveis voltadas para equipes corporativas. Nossa missão é fornecer uma plataforma segura e intuitiva que garanta que apenas usuários autenticados tenham acesso ao sistema, com controle rigoroso de permissões e auditoria completa.

### 🎯 Missão

Construir o núcleo da API RESTful da SafeScribe, implementando um sistema de autenticação e autorização seguro utilizando JSON Web Tokens (JWT) para gestão de documentos corporativos sensíveis.

### 🛠️ Requisitos Técnicos

- **Framework**: ![.NET](https://img.shields.io/badge/.NET-8.0-purple?style=flat-square&logo=dotnet) .NET 8.0
- **Tipo de Projeto**: ![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-blue?style=flat-square&logo=aspnet) ASP.NET Core Web API
- **Autenticação**: ![JWT](https://img.shields.io/badge/JWT-JSON%20Web%20Tokens-orange?style=flat-square&logo=jsonwebtokens) JWT com Microsoft.AspNetCore.Authentication.JwtBearer

### 📅 Informações do Projeto

- **Data de Entrega**: 20/10/2025
- **Grupo**: Até 3 pessoas
- **Status**: ![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow?style=flat-square) Em Desenvolvimento

## 🏗️ Arquitetura

O projeto segue os princípios da **Clean Architecture**, separando as responsabilidades em camadas bem definidas:

```mermaid
graph TB
    subgraph "🌐 Presentation Layer"
        A[Controllers]
        B[Middleware]
        C[DTOs]
    end
    
    subgraph "💼 Application Layer"
        D[Services]
        E[Interfaces]
        F[Validators]
        G[AutoMapper]
    end
    
    subgraph "🏛️ Domain Layer"
        H[Entities]
        I[Enums]
        J[Domain Interfaces]
    end
    
    subgraph "🔧 Infrastructure Layer"
        K[Repositories]
        L[DbContext]
        M[Configurations]
    end
    
    subgraph "🗄️ External"
        N[(SQL Server)]
        O[Logs]
    end
    
    A --> D
    B --> D
    D --> H
    D --> K
    K --> L
    L --> N
    D --> O
    
    style A fill:#e1f5fe
    style D fill:#f3e5f5
    style H fill:#e8f5e8
    style K fill:#fff3e0
```

### 🎯 Princípios da Clean Architecture

- **Independência de Frameworks**: A lógica de negócio não depende de frameworks externos
- **Testabilidade**: Fácil de testar com mocks e stubs
- **Independência de UI**: A interface pode mudar sem afetar o sistema
- **Independência de Banco**: O sistema pode funcionar com qualquer banco de dados
- **Independência de Agentes Externos**: A lógica de negócio não conhece o mundo externo

## 🛠️ Tecnologias

### 🔧 Backend

- **[.NET 8.0](https://dotnet.microsoft.com/)** - Framework principal
- **[ASP.NET Core](https://docs.microsoft.com/aspnet/core/)** - Web API
- **[Entity Framework Core 8.0](https://docs.microsoft.com/ef/)** - ORM
- **[SQL Server LocalDB](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb)** - Banco de dados

### 🔐 Autenticação & Segurança

- **[JWT Bearer Authentication](https://jwt.io/)** - Autenticação stateless
- **[BCrypt.Net](https://github.com/BcryptNet/bcrypt.net)** - Hash de senhas
- **[FluentValidation](https://fluentvalidation.net/)** - Validação de dados

### 📚 Documentação & Logs

- **[Swagger/OpenAPI](https://swagger.io/)** - Documentação da API
- **[Serilog](https://serilog.net/)** - Sistema de logging
- **[AutoMapper](https://automapper.org/)** - Mapeamento de objetos

## 📁 Estrutura do Projeto

```text
cp-5-autenticacao-autorizacao-swt/
├── 📁 Domain/                           # 🏛️ Camada de Domínio
│   ├── 📁 Entities/                     # Entidades do domínio
│   │   ├── 📄 BaseEntity.cs            # Entidade base
│   │   ├── 📄 User.cs                  # Entidade User
│   │   └── 📄 Note.cs                  # Entidade Note
│   ├── 📁 Interfaces/                   # Contratos do domínio
│   │   └── 📄 IUserRepository.cs       # Interface do repositório
│   └── 📁 Enums/                        # Enumeradores
│       ├── 📄 UserRole.cs              # Roles de usuário
│       └── 📄 UserStatus.cs            # Status do usuário
├── 📁 Application/                      # 💼 Camada de Aplicação
│   ├── 📁 DTOs/                         # Data Transfer Objects
│   │   ├── 📁 Auth/                    # DTOs de autenticação
│   │   │   ├── 📄 LoginRequestDto.cs   # DTO de login
│   │   │   ├── 📄 LoginResponseDto.cs  # DTO de resposta
│   │   │   ├── 📄 RegisterRequestDto.cs # DTO de registro
│   │   │   └── 📄 RefreshTokenRequestDto.cs # DTO de refresh
│   │   └── 📁 Notes/                   # DTOs de notas
│   │       ├── 📄 CreateNoteRequestDto.cs # DTO de criação
│   │       ├── 📄 NoteResponseDto.cs   # DTO de resposta
│   │       └── 📄 UpdateNoteRequestDto.cs # DTO de atualização
│   ├── 📁 Interfaces/                   # Contratos de serviços
│   │   ├── 📄 IAuthService.cs          # Interface de autenticação
│   │   ├── 📄 IUserService.cs          # Interface de usuários
│   │   ├── 📄 INoteService.cs          # Interface de notas
│   │   └── 📄 ITokenBlacklistService.cs # Interface de blacklist
│   ├── 📁 Services/                     # Serviços de aplicação
│   │   ├── 📄 AuthService.cs           # Serviço de autenticação
│   │   ├── 📄 UserService.cs           # Serviço de usuários
│   │   ├── 📄 NoteService.cs           # Serviço de notas
│   │   └── 📄 InMemoryTokenBlacklistService.cs # Serviço de blacklist
│   ├── 📁 Validators/                   # Validações
│   │   ├── 📄 LoginRequestValidator.cs # Validador de login
│   │   └── 📄 RegisterRequestValidator.cs # Validador de registro
│   └── 📄 MappingProfile.cs            # Perfil do AutoMapper
├── 📁 Infrastructure/                   # 🔧 Camada de Infraestrutura
│   ├── 📁 Data/                         # Contexto do banco
│   │   └── 📄 ApplicationDbContext.cs  # DbContext do EF
│   └── 📁 Repositories/                 # Implementações
│       └── 📄 UserRepository.cs        # Repositório de usuários
├── 📁 Presentation/                     # 🌐 Camada de Apresentação
│   ├── 📁 Controllers/                  # Controllers da API
│   │   ├── 📄 AuthController.cs        # Controller de autenticação
│   │   ├── 📄 UsersController.cs       # Controller de usuários
│   │   └── 📄 NotasController.cs       # Controller de notas
│   └── 📁 Middleware/                   # Middleware customizado
│       ├── 📄 GlobalExceptionHandlingMiddleware.cs # Tratamento global
│       └── 📄 JwtBlacklistMiddleware.cs # Middleware de blacklist
├── 📄 Program.cs                        # Configuração da aplicação
├── 📄 appsettings.json                  # Configurações
├── 📄 appsettings.Development.json     # Configurações de desenvolvimento
├── 📄 TestEndpoints.http               # Arquivo de testes HTTP
├── 📄 exemplos-de-uso.md               # Exemplos práticos de uso
├── 📄 manual_de-testes.md              # Manual completo de testes
├── 📄 diagramas.md                     # Diagramas de fluxo e arquitetura
└── 📄 README.md                        # Este arquivo
```

## 🚀 Funcionalidades

### 🔐 Autenticação (`/api/auth`)

| Endpoint | Método | Descrição | Autenticação | Status |
|----------|--------|-----------|--------------|--------|
| `/register` | `POST` | Registro de novo usuário | ❌ | ✅ |
| `/login` | `POST` | Login com email/senha | ❌ | ✅ |
| `/refresh-token` | `POST` | Renovação de token JWT | ❌ | ✅ |
| `/logout` | `POST` | Logout com blacklist de token | ✅ | ✅ |
| `/validate` | `GET` | Validação de token | ✅ | ✅ |

### 👥 Gerenciamento de Usuários (`/api/users`)

| Endpoint | Método | Descrição | Autenticação | Autorização | Status |
|----------|--------|-----------|--------------|-------------|--------|
| `/` | `GET` | Listar todos os usuários | ✅ | Admin | ✅ |
| `/{id}` | `GET` | Buscar usuário por ID | ✅ | Próprio/Admin | ✅ |
| `/profile` | `GET` | Perfil do usuário atual | ✅ | Próprio | ✅ |
| `/{id}` | `PUT` | Atualizar usuário | ✅ | Próprio/Admin | ⚠️ |
| `/{id}` | `DELETE` | Remover usuário | ✅ | Admin | ✅ |
| `/{id}/toggle-block` | `PATCH` | Bloquear/desbloquear | ✅ | Admin | ✅ |

### 📝 Gestão de Notas (`/api/v1/notas`)

| Endpoint | Método | Descrição | Autenticação | Autorização | Status |
|----------|--------|-----------|--------------|-------------|--------|
| `/` | `POST` | Criar nova nota | ✅ | Editor/Admin | ✅ |
| `/{id}` | `GET` | Buscar nota por ID | ✅ | Própria/Admin | ✅ |
| `/{id}` | `PUT` | Atualizar nota | ✅ | Própria/Admin | ✅ |
| `/{id}` | `DELETE` | Excluir nota | ✅ | Admin | ✅ |
| `/` | `GET` | Listar notas do usuário | ✅ | Próprio | ✅ |

### 🎭 Sistema de Roles

- **👤 Leitor**: Pode apenas visualizar suas próprias notas
- **✏️ Editor**: Pode criar e editar suas próprias notas
- **👑 Admin**: Acesso total ao sistema, incluindo todas as notas e usuários

## ⚙️ Configuração

### 🔧 Variáveis de Ambiente

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AuthJwtDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Secret": "MinhaChaveSecretaSuperSeguraParaJWTQueDeveTerPeloMenos32Caracteres",
    "Issuer": "AuthJwtAPI",
    "Audience": "AuthJwtClient",
    "ExpiryMinutes": 60
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 7
        }
      }
    ]
  }
}
```

### 🗄️ Configuração do Banco de Dados

O projeto usa **SQL Server LocalDB** por padrão. Para usar outro banco:

1. Altere a connection string no `appsettings.json`
2. Instale o provider apropriado do EF Core
3. Execute as migrações

## 🔧 Instalação

### 📋 Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) (ou SQL Server)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

### 🚀 Passos para Instalação

1. **Clone o repositório**
   ```bash
   git clone https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025.git
   cd Advanced_Business_Development_with_.NET_CP_2SEM_2025/cp-5-autenticacao-autorizacao-swt
   ```

2. **Restaure as dependências**
   ```bash
   dotnet restore
   ```

3. **Configure o banco de dados**
   ```bash
   dotnet ef database update
   ```

4. **Execute o projeto**
   ```bash
   dotnet run
   ```

5. **Acesse a documentação**
   - Swagger UI: `http://localhost:5210/swagger`
   - API: `http://localhost:5210/api`

## 📖 Documentação da API

### 🔐 Fluxo de Autenticação

```mermaid
sequenceDiagram
    participant C as Cliente
    participant A as API
    participant D as Database
    
    C->>A: POST /api/auth/register
    A->>D: Criar usuário
    D-->>A: Usuário criado
    A-->>C: Token JWT + Refresh Token
    
    C->>A: POST /api/auth/login
    A->>D: Validar credenciais
    D-->>A: Usuário válido
    A-->>C: Token JWT + Refresh Token
    
    C->>A: GET /api/users/profile (com JWT)
    A->>A: Validar JWT
    A-->>C: Dados do usuário
    
    C->>A: POST /api/auth/refresh-token
    A->>A: Validar Refresh Token
    A-->>C: Novo JWT + Refresh Token
```

### 📝 Exemplos de Uso

#### 1. Registro de Usuário

```http
POST /api/auth/register
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@exemplo.com",
  "senha": "MinhaSenh@123",
  "confirmarSenha": "MinhaSenh@123"
}
```

**Resposta:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "base64encodedtoken...",
  "expiresAt": "2024-01-01T12:00:00Z",
  "user": {
    "id": 1,
    "nome": "João Silva",
    "email": "joao@exemplo.com",
    "role": "User"
  }
}
```

#### 2. Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "joao@exemplo.com",
  "senha": "MinhaSenh@123"
}
```

#### 3. Criar Nota (Editor/Admin)

```http
POST /api/v1/notas
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "title": "Reunião de Planejamento Q4",
  "content": "Discussão sobre estratégias para o próximo trimestre...",
  "isSensitive": true,
  "tags": "planejamento,estratégia,confidencial"
}
```

#### 4. Acesso a Endpoint Protegido

```http
GET /api/users/profile
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### 5. Renovação de Token

```http
POST /api/auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "base64encodedtoken..."
}
```

#### 6. Logout com Blacklist

```http
POST /api/auth/logout
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 🧪 Testes

### 📋 Arquivo de Testes HTTP

Use o arquivo `TestEndpoints.http` para testar todos os endpoints:

```http
### Teste de Registro
POST {{baseUrl}}/api/auth/register
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@exemplo.com",
  "senha": "MinhaSenh@123",
  "confirmarSenha": "MinhaSenh@123"
}

### Teste de Login
POST {{baseUrl}}/api/auth/login
Content-Type: application/json

{
  "email": "joao@exemplo.com",
  "senha": "MinhaSenh@123"
}
```

### 🔧 Executar o Projeto

```bash
# Executar o projeto
dotnet run

# Executar em modo de desenvolvimento
dotnet run --environment Development

# Executar com URLs específicas
dotnet run --urls "https://localhost:7000;http://localhost:5000"
```

### 📊 Testes Automatizados

Consulte o arquivo `manual_de-testes.md` para:

- Guia completo de testes
- Exemplos com diferentes ferramentas
- Scripts de automação
- Troubleshooting

## 🔐 Segurança

### 🛡️ Medidas de Segurança Implementadas

- **🔒 Hash de Senhas**: BCrypt com salt automático
- **🎫 JWT Seguro**: Tokens com expiração e assinatura
- **🔄 Refresh Tokens**: Renovação segura de tokens
- **🚫 Blacklist de Tokens**: Logout seguro com invalidação
- **✅ Validação**: FluentValidation para todos os inputs
- **🔍 Logging**: Auditoria completa de ações
- **🛡️ CORS**: Configuração segura de origens
- **🔐 HTTPS**: Comunicação criptografada
- **👥 Controle de Acesso**: Sistema de roles granular

### 🚨 Controle de Acesso

```mermaid
graph TD
    A[Requisição] --> B{Token Válido?}
    B -->|Não| C[401 Unauthorized]
    B -->|Sim| D{Token na Blacklist?}
    D -->|Sim| E[401 Unauthorized]
    D -->|Não| F{Usuário Ativo?}
    F -->|Não| G[403 Forbidden]
    F -->|Sim| H{Role Adequado?}
    H -->|Não| I[403 Forbidden]
    H -->|Sim| J[200 OK]
    
    style C fill:#ffebee
    style E fill:#ffebee
    style G fill:#ffebee
    style I fill:#ffebee
    style J fill:#e8f5e8
```

### 🔑 Configuração de Segurança

```csharp
// JWT Configuration
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero
        };
    });
```

## 📊 Banco de Dados

### 🗄️ Estrutura das Tabelas

#### Tabela Users

```sql
CREATE TABLE [Users] (
    [Id] int IDENTITY(1,1) PRIMARY KEY,
    [Nome] nvarchar(100) NOT NULL,
    [Email] nvarchar(100) NOT NULL UNIQUE,
    [SenhaHash] nvarchar(255) NOT NULL,
    [Role] int NOT NULL,
    [Status] int NOT NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [UltimoLogin] datetime2 NULL,
    [TentativasLoginFalhadas] int NOT NULL DEFAULT 0,
    [DataBloqueio] datetime2 NULL,
    [RefreshToken] nvarchar(500) NULL,
    [RefreshTokenExpiryTime] datetime2 NULL
);
```

#### Tabela Notes

```sql
CREATE TABLE [Notes] (
    [Id] int IDENTITY(1,1) PRIMARY KEY,
    [Title] nvarchar(200) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [UserId] int NOT NULL,
    [IsSensitive] bit NOT NULL DEFAULT 0,
    [Tags] nvarchar(500) NULL,
    FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
);
```

### 📈 Índices

- **Email**: Índice único para busca rápida
- **RefreshToken**: Índice para validação de tokens
- **Status**: Índice para filtros de usuários ativos
- **UserId (Notes)**: Índice para consultas de notas por usuário

### 🔄 Migrações

```bash
# Criar nova migração
dotnet ef migrations add NomeDaMigracao

# Aplicar migrações
dotnet ef database update

# Remover última migração
dotnet ef migrations remove
```

## 📝 Logs

### 📊 Sistema de Logging com Serilog

O projeto implementa logging estruturado com Serilog:

```csharp
// Configuração do Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
```

### 📁 Estrutura de Logs

```
logs/
├── log-20240101.txt
├── log-20240102.txt
└── log-20240103.txt
```

### 🔍 Exemplos de Logs

```json
{
  "Timestamp": "2024-01-01T10:00:00Z",
  "Level": "Information",
  "MessageTemplate": "Login realizado com sucesso para email: {Email}",
  "Properties": {
    "Email": "joao@exemplo.com",
    "UserId": 1,
    "SourceContext": "AuthController"
  }
}
```

## 👥 Equipe

### 🎯 Integrantes do CP

| Nome | RM | GitHub | Responsabilidade |
|------|----|---------|------------------|
| ![Amanda](https://img.shields.io/badge/Amanda-Mesquita%20Cirino%20Da%20Silva-green?style=flat-square) | ![RM](https://img.shields.io/badge/RM-559177-blue?style=flat-square) | [![GitHub](https://img.shields.io/badge/GitHub-mandyy14-black?style=flat-square&logo=github)](https://github.com/mandyy14) | Backend & JWT |
| ![Journey](https://img.shields.io/badge/Journey-Tiago%20Lopes%20Ferreira-green?style=flat-square) | ![RM](https://img.shields.io/badge/RM-556071-blue?style=flat-square) | [![GitHub](https://img.shields.io/badge/GitHub-JouTiago-black?style=flat-square&logo=github)](https://github.com/JouTiago) | Testes & Documentação |
| ![Paulo](https://img.shields.io/badge/Paulo-André%20Carminati-green?style=flat-square) | ![RM](https://img.shields.io/badge/RM-557881-blue?style=flat-square) | [![GitHub](https://img.shields.io/badge/GitHub-carmipa-black?style=flat-square&logo=github)](https://github.com/carmipa) | Arquitetura & DevOps |

### 🔗 Repositórios

- **Repositório CP**: [![GitHub](https://img.shields.io/badge/GitHub-Advanced%20Business%20Development-black?style=flat-square&logo=github)](https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025)
- **Repositório Projeto**: [![GitHub](https://img.shields.io/badge/GitHub-CP5%20JWT%20API-black?style=flat-square&logo=github)](https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025/tree/main/cp-5-autenticacao-autorizacao-swt)

## 🤝 Contribuição

### 🚀 Como Contribuir

1. **Fork** o projeto
2. **Crie** uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. **Commit** suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. **Push** para a branch (`git push origin feature/AmazingFeature`)
5. **Abra** um Pull Request

### 📋 Padrões de Código

- **Clean Code**: Código limpo e legível
- **SOLID**: Princípios SOLID aplicados
- **DRY**: Don't Repeat Yourself
- **Comentários**: Documentação em português
- **Testes**: Cobertura de testes adequada

### 🐛 Reportar Bugs

Use o [GitHub Issues](https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025/issues) para reportar bugs:

```markdown
**Descrição do Bug**
Descrição clara e concisa do bug.

**Passos para Reproduzir**
1. Vá para '...'
2. Clique em '....'
3. Veja o erro

**Comportamento Esperado**
O que deveria acontecer.

**Screenshots**
Se aplicável, adicione screenshots.

**Ambiente:**
- OS: [e.g. Windows 10]
- .NET Version: [e.g. 8.0]
- Browser: [e.g. Chrome, Safari]
```

## 📄 Licença

Este projeto está licenciado sob a Licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

---

---

## 🎯 **Desenvolvido com ❤️ pela equipe SafeScribe - FIAP 2025**

[![GitHub stars](https://img.shields.io/github/stars/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025?style=social)](https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025?style=social)](https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025/network)
[![GitHub issues](https://img.shields.io/github/issues/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025)](https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025/issues)

**⭐ Se este projeto foi útil para você, considere dar uma estrela!**

![FIAP](https://img.shields.io/badge/FIAP-Advanced%20Business%20Development-purple?style=for-the-badge)
