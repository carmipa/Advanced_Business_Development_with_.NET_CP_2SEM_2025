# 🔐 API de Autenticação e Autorização com JWT

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=for-the-badge&logo=aspnetcore&logoColor=white)](https://docs.microsoft.com/aspnet/core/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-8.0-512BD4?style=for-the-badge&logo=entityframework&logoColor=white)](https://docs.microsoft.com/ef/)
[![JWT](https://img.shields.io/badge/JWT-Authentication-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)](https://jwt.io/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server/)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)](https://swagger.io/)
[![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)](LICENSE)

> 🚀 **API REST completa para autenticação e autorização de usuários usando JWT (JSON Web Tokens) implementada com Clean Architecture e Clean Code**

## 📋 Índice

- [🎯 Visão Geral](#-visão-geral)
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
- [🤝 Contribuição](#-contribuição)
- [📄 Licença](#-licença)

## 🎯 Visão Geral

Esta API foi desenvolvida seguindo os princípios de **Clean Architecture** e **Clean Code**, fornecendo um sistema robusto e escalável para autenticação e autorização de usuários. O projeto implementa JWT (JSON Web Tokens) para autenticação stateless, com suporte a refresh tokens e controle de acesso baseado em roles.

### ✨ Principais Características

- 🔐 **Autenticação JWT** com tokens seguros
- 🔄 **Refresh Tokens** para renovação automática
- 👥 **Sistema de Roles** (Admin, User, Moderator)
- 🛡️ **Validação robusta** de dados
- 📚 **Documentação Swagger** completa
- 🏗️ **Clean Architecture** bem estruturada
- 📊 **Logging detalhado** com Serilog
- 🗄️ **Entity Framework Core** para persistência

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

```
cp-5-autenticacao-autorizacao-swt/
├── 📁 Domain/                           # 🏛️ Camada de Domínio
│   ├── 📁 Entities/                     # Entidades do domínio
│   │   ├── 📄 BaseEntity.cs            # Entidade base
│   │   └── 📄 User.cs                  # Entidade User
│   ├── 📁 Interfaces/                   # Contratos do domínio
│   │   └── 📄 IUserRepository.cs       # Interface do repositório
│   └── 📁 Enums/                        # Enumeradores
│       ├── 📄 UserRole.cs              # Roles de usuário
│       └── 📄 UserStatus.cs            # Status do usuário
├── 📁 Application/                      # 💼 Camada de Aplicação
│   ├── 📁 DTOs/                         # Data Transfer Objects
│   │   └── 📁 Auth/                    # DTOs de autenticação
│   │       ├── 📄 LoginRequestDto.cs   # DTO de login
│   │       ├── 📄 LoginResponseDto.cs  # DTO de resposta
│   │       ├── 📄 RegisterRequestDto.cs # DTO de registro
│   │       └── 📄 RefreshTokenRequestDto.cs # DTO de refresh
│   ├── 📁 Interfaces/                   # Contratos de serviços
│   │   ├── 📄 IAuthService.cs          # Interface de autenticação
│   │   └── 📄 IUserService.cs          # Interface de usuários
│   ├── 📁 Services/                     # Serviços de aplicação
│   │   ├── 📄 AuthService.cs           # Serviço de autenticação
│   │   └── 📄 UserService.cs           # Serviço de usuários
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
│   │   └── 📄 UsersController.cs       # Controller de usuários
│   └── 📁 Middleware/                   # Middleware customizado
│       └── 📄 ExceptionHandlingMiddleware.cs # Tratamento de exceções
├── 📄 Program.cs                        # Configuração da aplicação
├── 📄 appsettings.json                  # Configurações
├── 📄 appsettings.Development.json     # Configurações de desenvolvimento
├── 📄 TestEndpoints.http               # Arquivo de testes HTTP
└── 📄 README.md                        # Este arquivo
```

## 🚀 Funcionalidades

### 🔐 Autenticação (`/api/auth`)

| Endpoint | Método | Descrição | Autenticação |
|----------|--------|-----------|--------------|
| `/register` | `POST` | Registro de novo usuário | ❌ |
| `/login` | `POST` | Login com email/senha | ❌ |
| `/refresh-token` | `POST` | Renovação de token JWT | ❌ |
| `/logout` | `POST` | Logout do usuário | ✅ |
| `/validate` | `GET` | Validação de token | ✅ |

### 👥 Gerenciamento de Usuários (`/api/users`)

| Endpoint | Método | Descrição | Autenticação | Autorização |
|----------|--------|-----------|--------------|-------------|
| `/` | `GET` | Listar todos os usuários | ✅ | Admin |
| `/{id}` | `GET` | Buscar usuário por ID | ✅ | Próprio/Admin |
| `/profile` | `GET` | Perfil do usuário atual | ✅ | Próprio |
| `/{id}` | `PUT` | Atualizar usuário | ✅ | Próprio/Admin |
| `/{id}` | `DELETE` | Remover usuário | ✅ | Admin |
| `/{id}/toggle-block` | `PATCH` | Bloquear/desbloquear | ✅ | Admin |

### 🎭 Sistema de Roles

- **👑 Admin**: Acesso total ao sistema
- **👤 User**: Acesso limitado (próprios dados)
- **🛡️ Moderator**: Privilégios intermediários

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
   git clone https://github.com/seu-usuario/cp-5-autenticacao-autorizacao-swt.git
   cd cp-5-autenticacao-autorizacao-swt
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
   - Swagger UI: `http://localhost:5210`
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

#### 3. Acesso a Endpoint Protegido

```http
GET /api/users/profile
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### 4. Renovação de Token

```http
POST /api/auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "base64encodedtoken..."
}
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

## 🔐 Segurança

### 🛡️ Medidas de Segurança Implementadas

- **🔒 Hash de Senhas**: BCrypt com salt automático
- **🎫 JWT Seguro**: Tokens com expiração e assinatura
- **🔄 Refresh Tokens**: Renovação segura de tokens
- **🚫 Rate Limiting**: Proteção contra ataques de força bruta
- **✅ Validação**: FluentValidation para todos os inputs
- **🔍 Logging**: Auditoria completa de ações
- **🛡️ CORS**: Configuração segura de origens
- **🔐 HTTPS**: Comunicação criptografada

### 🚨 Controle de Acesso

```mermaid
graph TD
    A[Requisição] --> B{Token Válido?}
    B -->|Não| C[401 Unauthorized]
    B -->|Sim| D{Usuário Ativo?}
    D -->|Não| E[403 Forbidden]
    D -->|Sim| F{Role Adequado?}
    F -->|Não| G[403 Forbidden]
    F -->|Sim| H[200 OK]
    
    style C fill:#ffebee
    style E fill:#ffebee
    style G fill:#ffebee
    style H fill:#e8f5e8
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

### 🗄️ Estrutura da Tabela Users

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

### 📈 Índices

- **Email**: Índice único para busca rápida
- **RefreshToken**: Índice para validação de tokens
- **Status**: Índice para filtros de usuários ativos

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

Use o [GitHub Issues](https://github.com/seu-usuario/cp-5-autenticacao-autorizacao-swt/issues) para reportar bugs:

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

<div align="center">

### 🎯 **Desenvolvido com ❤️ seguindo Clean Architecture e Clean Code**

[![GitHub stars](https://img.shields.io/github/stars/seu-usuario/cp-5-autenticacao-autorizacao-swt?style=social)](https://github.com/seu-usuario/cp-5-autenticacao-autorizacao-swt/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/seu-usuario/cp-5-autenticacao-autorizacao-swt?style=social)](https://github.com/seu-usuario/cp-5-autenticacao-autorizacao-swt/network)
[![GitHub issues](https://img.shields.io/github/issues/seu-usuario/cp-5-autenticacao-autorizacao-swt)](https://github.com/seu-usuario/cp-5-autenticacao-autorizacao-swt/issues)

**⭐ Se este projeto foi útil para você, considere dar uma estrela!**

</div>
