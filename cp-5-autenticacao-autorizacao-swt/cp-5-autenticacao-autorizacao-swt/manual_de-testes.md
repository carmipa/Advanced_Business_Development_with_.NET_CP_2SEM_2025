# 🧪 Manual de Testes - SafeScribe API

![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow?style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-8.0-purple?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-blue?style=for-the-badge&logo=aspnet)
![JWT](https://img.shields.io/badge/JWT-JSON%20Web%20Tokens-orange?style=for-the-badge&logo=jsonwebtokens)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)

## 📋 Índice de Navegação

- [🏗️ Arquitetura da Aplicação](#️-arquitetura-da-aplicação)
- [🚀 Como Executar o Projeto](#-como-executar-o-projeto)
- [🔗 Acessando o Swagger UI](#-acessando-o-swagger-ui)
- [🔐 Testes de Autenticação](#-testes-de-autenticação)
- [👤 Testes de Usuários](#-testes-de-usuários)
- [🛡️ Testes de Segurança](#️-testes-de-segurança)
- [📊 Relatório de Testes](#-relatório-de-testes)
- [🔧 Troubleshooting](#-troubleshooting)

---

## 🏗️ Arquitetura da Aplicação

### Diagrama de Arquitetura

```mermaid
graph TB
    subgraph Client["🌐 Frontend/Client"]
        A[Swagger UI]
        C[Postman/Insomnia]
        D[Frontend App]
    end
    
    subgraph API["🎯 API Layer"]
        E[AuthController]
        F[UsersController]
        G[Middleware]
    end
    
    subgraph Business["🏢 Business Layer"]
        H[AuthService]
        I[UserService]
        J[Validators]
    end
    
    subgraph Data["🗄️ Data Layer"]
        K[UserRepository]
        L[ApplicationDbContext]
        M[(SQL Server Database)]
    end
    
    subgraph External["🔧 External Services"]
        N[JWT Authentication]
        O[BCrypt Hashing]
        P[Serilog Logging]
    end
    
    A --> E
    C --> E
    D --> E
    G --> E
    G --> F
    E --> H
    F --> I
    J --> H
    J --> I
    H --> K
    I --> K
    K --> L
    L --> M
    H --> N
    H --> O
    G --> P
    
    style A fill:#85EA2D
    style E fill:#512BD4,color:#fff
    style H fill:#512BD4,color:#fff
    style K fill:#CC2927,color:#fff
    style M fill:#CC2927,color:#fff
```

### Camadas da Aplicação

| Camada | Tecnologia | Responsabilidade |
|--------|------------|------------------|
| ![Presentation](https://img.shields.io/badge/Presentation-Controllers-green?style=flat-square) | ASP.NET Core Controllers | Recebe requisições HTTP e retorna respostas |
| ![Application](https://img.shields.io/badge/Application-Services-blue?style=flat-square) | Services + DTOs | Lógica de negócio e validações |
| ![Domain](https://img.shields.io/badge/Domain-Entities-purple?style=flat-square) | Entities + Enums | Modelos de domínio e regras de negócio |
| ![Infrastructure](https://img.shields.io/badge/Infrastructure-Data-orange?style=flat-square) | EF Core + Repositories | Acesso a dados e persistência |

---

## 🚀 Como Executar o Projeto

### 📋 Pré-requisitos

- ![.NET](https://img.shields.io/badge/.NET-8.0%20SDK-required-purple?style=flat-square) .NET 8.0 SDK ou superior
- ![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-required-red?style=flat-square) SQL Server LocalDB
- ![Git](https://img.shields.io/badge/Git-required-orange?style=flat-square) Git

### 🛠️ Passos para Execução

```bash
# 1. Clone o repositório
git clone https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025.git
cd Advanced_Business_Development_with_.NET_CP_2SEM_2025/cp-5-autenticacao-autorizacao-swt

# 2. Restaure as dependências
dotnet restore

# 3. Execute as migrações do banco de dados
dotnet ef database update

# 4. Execute o projeto
dotnet run --urls "http://localhost:5210"
```

### ✅ Verificação de Instalação

```bash
# Verificar se o .NET está instalado
dotnet --version

# Verificar se o projeto compila
dotnet build

# Verificar se o banco foi criado
dotnet ef database update
```

---

## 🔗 Acessando o Swagger UI

### 🌐 URLs de Acesso

| Serviço | URL | Status |
|---------|-----|--------|
| ![Swagger UI](https://img.shields.io/badge/Swagger-UI-green?style=flat-square) | [http://localhost:5210/swagger](http://localhost:5210/swagger) | ![Status](https://img.shields.io/badge/Status-Online-green?style=flat-square) |
| ![API JSON](https://img.shields.io/badge/API-JSON%20Schema-blue?style=flat-square) | [http://localhost:5210/swagger/v1/swagger.json](http://localhost:5210/swagger/v1/swagger.json) | ![Status](https://img.shields.io/badge/Status-Online-green?style=flat-square) |

### 🎯 Funcionalidades do Swagger UI

- ✅ **Documentação Interativa** - Todos os endpoints documentados
- ✅ **Teste Direto** - Execute requisições sem ferramentas externas
- ✅ **Autenticação JWT** - Botão "Authorize" para inserir tokens
- ✅ **Exemplos de Request/Response** - Modelos de dados completos
- ✅ **Comentários XML** - Documentação detalhada de cada endpoint

---

## 🔄 Fluxo de Autenticação JWT

```mermaid
sequenceDiagram
    participant C as Client
    participant API as API
    participant DB as Database
    participant JWT as JWT Service
    
    Note over C,JWT: 🔐 Fluxo de Login
    
    C->>API: POST /api/auth/login
    Note right of C: {email, senha}
    
    API->>DB: Validate Credentials
    DB-->>API: User Data
    
    API->>JWT: Generate JWT Token
    JWT-->>API: Token + Refresh Token
    
    API->>DB: Save Refresh Token
    API-->>C: 200 OK + Tokens
    
    Note over C,JWT: 🔄 Fluxo de Refresh Token
    
    C->>API: POST /api/auth/refresh-token
    Note right of C: {refreshToken}
    
    API->>DB: Validate Refresh Token
    DB-->>API: Token Valid
    
    API->>JWT: Generate New Tokens
    JWT-->>API: New Token + Refresh Token
    
    API->>DB: Update Refresh Token
    API-->>C: 200 OK + New Tokens
    
    Note over C,JWT: 🚪 Fluxo de Logout
    
    C->>API: POST /api/auth/logout
    Note right of C: Authorization: Bearer token
    
    API->>DB: Invalidate Refresh Token
    DB-->>API: Token Invalidated
    
    API-->>C: 200 OK + Success Message
```

## 🛡️ Fluxo de Autorização

```mermaid
flowchart TD
    A[Request com JWT] --> B{Token Válido?}
    B -->|Não| C[401 Unauthorized]
    B -->|Sim| D{Token Expirado?}
    D -->|Sim| E[Use Refresh Token]
    D -->|Não| F{Role Adequada?}
    E --> G{Refresh Token Válido?}
    G -->|Não| H[401 Unauthorized]
    G -->|Sim| I[Gerar Novos Tokens]
    I --> F
    F -->|Não| J[403 Forbidden]
    F -->|Sim| K[200 OK + Data]
    
    style C fill:#ff6b6b
    style H fill:#ff6b6b
    style J fill:#ffa726
    style K fill:#66bb6a
    style I fill:#42a5f5
```

---

## 🔐 Testes de Autenticação

### 1. 📝 Registro de Usuário

#### Endpoint
```
POST /api/auth/register
```

#### Request Body
```json
{
  "nome": "João Silva",
  "email": "joao@exemplo.com",
  "senha": "MinhaSenh@123",
  "confirmarSenha": "MinhaSenh@123"
}
```

#### Teste no Swagger
1. ![🔍](https://img.shields.io/badge/1-Abrir%20Swagger-blue?style=flat-square) Acesse [http://localhost:5210/swagger](http://localhost:5210/swagger)
2. ![📝](https://img.shields.io/badge/2-Expandir%20POST%20auth%2Fregister-green?style=flat-square) Clique em `POST /api/auth/register`
3. ![▶️](https://img.shields.io/badge/3-Clicar%20Try%20it%20out-orange?style=flat-square) Clique em "Try it out"
4. ![✏️](https://img.shields.io/badge/4-Preencher%20Request%20Body-blue?style=flat-square) Cole o JSON acima
5. ![🚀](https://img.shields.io/badge/5-Executar%20Execute-green?style=flat-square) Clique em "Execute"

#### Resultado Esperado
- **Status Code**: ![201](https://img.shields.io/badge/201-Created-green?style=flat-square)
- **Response**: Token JWT + Refresh Token + Dados do usuário

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "vgR6Hi51LZP7RB+THVOxPkOTeQq6VyeyuFv/ODPqvSw=",
  "expiresAt": "2025-10-18T16:00:13.3992562Z",
  "user": {
    "id": 1,
    "nome": "João Silva",
    "email": "joao@exemplo.com",
    "role": "User"
  }
}
```

### 2. 🔑 Login de Usuário

#### Endpoint
```
POST /api/auth/login
```

#### Request Body
```json
{
  "email": "joao@exemplo.com",
  "senha": "MinhaSenh@123"
}
```

#### Teste no Swagger
1. ![🔍](https://img.shields.io/badge/1-Abrir%20Swagger-blue?style=flat-square) Acesse o Swagger UI
2. ![📝](https://img.shields.io/badge/2-Expandir%20POST%20auth%2Flogin-green?style=flat-square) Clique em `POST /api/auth/login`
3. ![▶️](https://img.shields.io/badge/3-Try%20it%20out-orange?style=flat-square) Clique em "Try it out"
4. ![✏️](https://img.shields.io/badge/4-Preencher%20credenciais-blue?style=flat-square) Insira email e senha
5. ![🚀](https://img.shields.io/badge/5-Execute-green?style=flat-square) Execute a requisição

#### Resultado Esperado
- **Status Code**: ![200](https://img.shields.io/badge/200-OK-green?style=flat-square)
- **Response**: Token JWT + Refresh Token + Dados do usuário

### 3. 🔒 Validação de Token

#### Endpoint
```
GET /api/auth/validate
```

#### Headers
```
Authorization: Bearer {seu_token_jwt}
```

#### Teste no Swagger
1. ![🔑](https://img.shields.io/badge/1-Authorize-red?style=flat-square) Clique no botão "Authorize" (🔒)
2. ![✏️](https://img.shields.io/badge/2-Inserir%20Token-blue?style=flat-square) Cole o token JWT: `Bearer seu_token_aqui`
3. ![✅](https://img.shields.io/badge/3-Authorize-green?style=flat-square) Clique em "Authorize"
4. ![🔍](https://img.shields.io/badge/4-Testar%20validate-orange?style=flat-square) Execute `GET /api/auth/validate`

#### Resultado Esperado
- **Status Code**: ![200](https://img.shields.io/badge/200-OK-green?style=flat-square)
- **Response**: Informações do usuário autenticado

### 4. 🔄 Refresh Token

#### Endpoint
```
POST /api/auth/refresh-token
```

#### Request Body
```json
{
  "refreshToken": "seu_refresh_token_aqui"
}
```

#### Teste no Swagger
1. ![📝](https://img.shields.io/badge/1-Expandir%20refresh%2Dtoken-green?style=flat-square) Clique em `POST /api/auth/refresh-token`
2. ![▶️](https://img.shields.io/badge/2-Try%20it%20out-orange?style=flat-square) Clique em "Try it out"
3. ![✏️](https://img.shields.io/badge/3-Inserir%20refresh%20token-blue?style=flat-square) Cole o refresh token
4. ![🚀](https://img.shields.io/badge/4-Execute-green?style=flat-square) Execute

#### Resultado Esperado
- **Status Code**: ![200](https://img.shields.io/badge/200-OK-green?style=flat-square)
- **Response**: Novos tokens JWT e refresh token

### 5. 🚪 Logout

#### Endpoint
```
POST /api/auth/logout
```

#### Headers
```
Authorization: Bearer {seu_token_jwt}
```

#### Teste no Swagger
1. ![🔑](https://img.shields.io/badge/1-Authorize-red?style=flat-square) Certifique-se de estar autenticado
2. ![📝](https://img.shields.io/badge/2-Expandir%20logout-green?style=flat-square) Clique em `POST /api/auth/logout`
3. ![▶️](https://img.shields.io/badge/3-Try%20it%20out-orange?style=flat-square) Clique em "Try it out"
4. ![🚀](https://img.shields.io/badge/4-Execute-green?style=flat-square) Execute

#### Resultado Esperado
- **Status Code**: ![200](https://img.shields.io/badge/200-OK-green?style=flat-square)
- **Response**: Mensagem de logout bem-sucedido

---

## 👤 Testes de Usuários

### 1. 👤 Perfil do Usuário

#### Endpoint
```
GET /api/users/profile
```

#### Headers
```
Authorization: Bearer {seu_token_jwt}
```

#### Teste no Swagger
1. ![🔑](https://img.shields.io/badge/1-Authorize-red?style=flat-square) Autentique-se primeiro
2. ![📝](https://img.shields.io/badge/2-Expandir%20users%2Fprofile-green?style=flat-square) Clique em `GET /api/users/profile`
3. ![▶️](https://img.shields.io/badge/3-Try%20it%20out-orange?style=flat-square) Clique em "Try it out"
4. ![🚀](https://img.shields.io/badge/4-Execute-green?style=flat-square) Execute

#### Resultado Esperado
- **Status Code**: ![200](https://img.shields.io/badge/200-OK-green?style=flat-square)
- **Response**: Dados do usuário autenticado

### 2. 🔍 Buscar Usuário por ID

#### Endpoint
```
GET /api/users/{id}
```

#### Headers
```
Authorization: Bearer {seu_token_jwt}
```

#### Teste no Swagger
1. ![🔑](https://img.shields.io/badge/1-Authorize-red?style=flat-square) Autentique-se
2. ![📝](https://img.shields.io/badge/2-Expandir%20users%2F%7Bid%7D-green?style=flat-square) Clique em `GET /api/users/{id}`
3. ![▶️](https://img.shields.io/badge/3-Try%20it%20out-orange?style=flat-square) Clique em "Try it out"
4. ![✏️](https://img.shields.io/badge/4-Inserir%20ID-blue?style=flat-square) Digite um ID válido (ex: 1)
5. ![🚀](https://img.shields.io/badge/5-Execute-green?style=flat-square) Execute

#### Resultado Esperado
- **Status Code**: ![200](https://img.shields.io/badge/200-OK-green?style=flat-square)
- **Response**: Dados do usuário solicitado

### 3. 📋 Listar Todos os Usuários (Admin)

#### Endpoint
```
GET /api/users
```

#### Headers
```
Authorization: Bearer {token_de_admin}
```

#### Teste no Swagger
1. ![⚠️](https://img.shields.io/badge/1-ATENÇÃO%3A%20Requer%20Admin-red?style=flat-square) **Este endpoint requer role "Admin"**
2. ![📝](https://img.shields.io/badge/2-Expandir%20users-green?style=flat-square) Clique em `GET /api/users`
3. ![▶️](https://img.shields.io/badge/3-Try%20it%20out-orange?style=flat-square) Clique em "Try it out"
4. ![🚀](https://img.shields.io/badge/4-Execute-green?style=flat-square) Execute

#### Resultado Esperado
- **Com Admin**: ![200](https://img.shields.io/badge/200-OK-green?style=flat-square) Lista de usuários
- **Sem Admin**: ![403](https://img.shields.io/badge/403-Forbidden-red?style=flat-square) Acesso negado

---

## 🛡️ Testes de Segurança

### 1. ❌ Token Inválido

#### Teste
```bash
GET /api/auth/validate
Authorization: Bearer token_invalido
```

#### Resultado Esperado
- **Status Code**: ![401](https://img.shields.io/badge/401-Unauthorized-red?style=flat-square)

### 2. ❌ Credenciais Inválidas

#### Teste
```json
POST /api/auth/login
{
  "email": "inexistente@teste.com",
  "senha": "senhaerrada"
}
```

#### Resultado Esperado
- **Status Code**: ![401](https://img.shields.io/badge/401-Unauthorized-red?style=flat-square)
- **Response**: "Credenciais inválidas"

### 3. ❌ Email Duplicado

#### Teste
```json
POST /api/auth/register
{
  "nome": "Outro Usuário",
  "email": "joao@exemplo.com", // Email já existente
  "senha": "OutraSenh@123",
  "confirmarSenha": "OutraSenh@123"
}
```

#### Resultado Esperado
- **Status Code**: ![409](https://img.shields.io/badge/409-Conflict-red?style=flat-square)
- **Response**: "Email já está em uso"

### 4. ❌ Dados Inválidos

#### Teste
```json
POST /api/auth/register
{
  "nome": "",
  "email": "email-invalido",
  "senha": "123",
  "confirmarSenha": "456"
}
```

#### Resultado Esperado
- **Status Code**: ![400](https://img.shields.io/badge/400-Bad%20Request-red?style=flat-square)
- **Response**: Erros de validação detalhados

---

## 📊 Relatório de Testes

### 🎯 Estado dos Endpoints

```mermaid
graph LR
    subgraph Auth["🔐 Autenticação"]
        A1[POST /register] --> A1S[✅ OK]
        A2[POST /login] --> A2S[✅ OK]
        A3[GET /validate] --> A3S[✅ OK]
        A4[POST /refresh-token] --> A4S[⚠️ Problem]
        A5[POST /logout] --> A5S[✅ OK]
    end
    
    subgraph Users["👤 Usuários"]
        U1[GET /profile] --> U1S[✅ OK]
        U2[GET /{id}] --> U2S[✅ OK]
        U3[GET /] --> U3S[✅ OK]
        U4[PUT /{id}] --> U4S[⚠️ Error 500]
    end
    
    style A1S fill:#66bb6a
    style A2S fill:#66bb6a
    style A3S fill:#66bb6a
    style A4S fill:#ffa726
    style A5S fill:#66bb6a
    style U1S fill:#66bb6a
    style U2S fill:#66bb6a
    style U3S fill:#66bb6a
    style U4S fill:#ffa726
```

### ✅ Status dos Endpoints

| Endpoint | Método | Status | Autenticação | Observações |
|----------|--------|--------|--------------|-------------|
| `/api/auth/register` | POST | ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) | ❌ | Criação de usuários |
| `/api/auth/login` | POST | ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) | ❌ | Autenticação |
| `/api/auth/validate` | GET | ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) | ✅ | Validação de token |
| `/api/auth/refresh-token` | POST | ![⚠️](https://img.shields.io/badge/⚠️-Problemático-yellow?style=flat-square) | ❌ | Refresh token invalidação |
| `/api/auth/logout` | POST | ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) | ✅ | Logout |
| `/api/users/profile` | GET | ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) | ✅ | Perfil do usuário |
| `/api/users/{id}` | GET | ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) | ✅ | Busca por ID |
| `/api/users` | GET | ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) | ✅ | Lista (Admin only) |
| `/api/users/{id}` | PUT | ![⚠️](https://img.shields.io/badge/⚠️-Erro%20500-yellow?style=flat-square) | ✅ | Atualização com erro |

### 📈 Métricas de Testes

| Categoria | Total | ✅ Sucesso | ❌ Falha | ⚠️ Problemas |
|-----------|-------|------------|----------|--------------|
| ![Autenticação](https://img.shields.io/badge/Autenticação-5-blue?style=flat-square) | 5 | 4 | 0 | 1 |
| ![Usuários](https://img.shields.io/badge/Usuários-3-green?style=flat-square) | 3 | 2 | 0 | 1 |
| ![Segurança](https://img.shields.io/badge/Segurança-4-red?style=flat-square) | 4 | 4 | 0 | 0 |
| **Total** | **12** | **10** | **0** | **2** |

### 🎯 Taxa de Sucesso

![Taxa de Sucesso](https://img.shields.io/badge/Taxa%20de%20Sucesso-83.3%25-green?style=for-the-badge)

### 📅 Cronograma de Testes

```mermaid
gantt
    title Cronograma de Testes - SafeScribe API
    dateFormat  YYYY-MM-DD
    section Setup
    Configuração Ambiente    :done, setup, 2025-10-18, 1d
    Criação Banco Dados      :done, db, after setup, 1d
    
    section Autenticação
    Teste Registro           :done, reg, after db, 1d
    Teste Login              :done, login, after reg, 1d
    Teste Validação Token    :done, validate, after login, 1d
    Teste Refresh Token      :active, refresh, after validate, 1d
    Teste Logout             :done, logout, after refresh, 1d
    
    section Usuários
    Teste Perfil             :done, profile, after logout, 1d
    Teste Busca por ID       :done, getid, after profile, 1d
    Teste Lista Usuários     :done, list, after getid, 1d
    Teste Atualização        :crit, update, after list, 1d
    
    section Segurança
    Teste Token Inválido     :done, invalid, after update, 1d
    Teste Credenciais Inválidas :done, creds, after invalid, 1d
    Teste Email Duplicado    :done, duplicate, after creds, 1d
    Teste Validação Dados    :done, validation, after duplicate, 1d
```

### 🏆 Resultados por Categoria

```mermaid
pie title Distribuição de Status dos Testes
    "✅ Sucesso (83.3%)" : 10
    "⚠️ Problemas (16.7%)" : 2
    "❌ Falhas (0%)" : 0
```

---

## 🔧 Troubleshooting

### ❌ Problemas Comuns

#### 1. **Erro 500 - Erro Interno do Servidor**
```
Sintoma: Status 500 em todos os endpoints
Causa: Banco de dados não criado
Solução: Execute `dotnet ef database update`
```

#### 2. **Erro 404 - Swagger não encontrado**
```
Sintoma: Cannot GET /swagger
Causa: Projeto não está rodando
Solução: Execute `dotnet run --urls "http://localhost:5210"`
```

#### 3. **Erro 401 - Token Inválido**
```
Sintoma: 401 Unauthorized em endpoints protegidos
Causa: Token JWT inválido ou expirado
Solução: Faça login novamente para obter novo token
```

#### 4. **Erro 403 - Acesso Negado**
```
Sintoma: 403 Forbidden em /api/users
Causa: Usuário não tem role "Admin"
Solução: Use token de usuário administrador
```

### 🛠️ Comandos de Diagnóstico

```bash
# Verificar se o projeto compila
dotnet build

# Verificar migrações pendentes
dotnet ef migrations list

# Aplicar migrações
dotnet ef database update

# Verificar logs da aplicação
dotnet run --urls "http://localhost:5210" --verbosity detailed

# Testar conectividade
curl -I http://localhost:5210/swagger
```

### 📋 Checklist de Verificação

- [ ] ![✅](https://img.shields.io/badge/.NET-8.0%20instalado-green?style=flat-square) .NET 8.0 SDK instalado
- [ ] ![✅](https://img.shields.io/badge/SQL%20Server-LocalDB%20funcionando-green?style=flat-square) SQL Server LocalDB funcionando
- [ ] ![✅](https://img.shields.io/badge/Projeto-compilando-green?style=flat-square) Projeto compila sem erros
- [ ] ![✅](https://img.shields.io/badge/Banco-migrado-green?style=flat-square) Banco de dados migrado
- [ ] ![✅](https://img.shields.io/badge/Swagger-acessível-green?style=flat-square) Swagger UI acessível
- [ ] ![✅](https://img.shields.io/badge/Endpoints-funcionando-green?style=flat-square) Endpoints respondem corretamente

---

## 📞 Suporte

### 👥 Equipe de Desenvolvimento

| Nome | RM | GitHub | Responsabilidade |
|------|----|---------|------------------|
| ![Amanda](https://img.shields.io/badge/Amanda-Mesquita%20Cirino%20Da%20Silva-green?style=flat-square) | ![RM](https://img.shields.io/badge/RM-559177-blue?style=flat-square) | [![GitHub](https://img.shields.io/badge/GitHub-carmipa-black?style=flat-square&logo=github)](https://github.com/carmipa) | Backend & JWT |
| ![Journey](https://img.shields.io/badge/Journey-Tiago%20Lopes%20Ferreira-green?style=flat-square) | ![RM](https://img.shields.io/badge/RM-556071-blue?style=flat-square) | [![GitHub](https://img.shields.io/badge/GitHub-JouTiago-black?style=flat-square&logo=github)](https://github.com/JouTiago) | Testes & Documentação |
| ![Paulo](https://img.shields.io/badge/Paulo-André%20Carminati-green?style=flat-square) | ![RM](https://img.shields.io/badge/RM-557881-blue?style=flat-square) | [![GitHub](https://img.shields.io/badge/GitHub-mandyy14-black?style=flat-square&logo=github)](https://github.com/mandyy14) | Arquitetura & DevOps |

### 🔗 Links Úteis

- ![Repositório](https://img.shields.io/badge/Repositório-GitHub-black?style=flat-square&logo=github) [Repositório Principal](https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025)
- ![Projeto](https://img.shields.io/badge/Projeto-CP5%20JWT-black?style=flat-square&logo=github) [Projeto CP5](https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025/tree/main/cp-5-autenticacao-autorizacao-swt)
- ![Swagger](https://img.shields.io/badge/Swagger-UI-green?style=flat-square&logo=swagger) [Swagger UI](http://localhost:5210/swagger)

### 📧 Contato

![Email](https://img.shields.io/badge/Email-safescribe%40fiap.com.br-blue?style=flat-square&logo=gmail)

---

## 📄 Licença

![MIT License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

Este projeto está licenciado sob a Licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

---

<div align="center">

![SafeScribe](https://img.shields.io/badge/SafeScribe-CP5%20JWT%20API-blue?style=for-the-badge)

**Desenvolvido com ❤️ pela equipe SafeScribe - FIAP 2025**

![FIAP](https://img.shields.io/badge/FIAP-Advanced%20Business%20Development-purple?style=for-the-badge)

</div>
