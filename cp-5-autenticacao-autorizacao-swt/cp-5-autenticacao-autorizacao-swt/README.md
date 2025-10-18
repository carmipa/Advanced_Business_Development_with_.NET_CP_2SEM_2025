# 🔐 SafeScribe API - CP5 JWT Authentication

![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow?style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-8.0-purple?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-blue?style=for-the-badge&logo=aspnet)
![JWT](https://img.shields.io/badge/JWT-JSON%20Web%20Tokens-orange?style=for-the-badge&logo=jsonwebtokens)
![Swagger](https://img.shields.io/badge/Swagger-UI%20Ready-green?style=for-the-badge&logo=swagger)

## 📋 Sobre o Projeto

A startup **SafeScribe** está desenvolvendo uma plataforma inovadora para gestão de notas e documentos sensíveis voltada para equipes corporativas. A segurança e o controle de acesso são os pilares do produto. Este projeto implementa o núcleo da API RESTful da SafeScribe, com sistema de autenticação e autorização seguro utilizando JSON Web Tokens (JWT).

## 🎯 Missão

Construir o núcleo da API RESTful da SafeScribe, implementando um sistema de autenticação e autorização seguro utilizando JSON Web Tokens (JWT).

## 🛠️ Tecnologias Utilizadas

![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=c-sharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=flat-square&logo=aspnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-512BD4?style=flat-square&logo=entity-framework&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat-square&logo=microsoft-sql-server&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=flat-square&logo=jsonwebtokens&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=flat-square&logo=swagger&logoColor=black)
![Serilog](https://img.shields.io/badge/Serilog-000000?style=flat-square&logo=serilog&logoColor=white)
![FluentValidation](https://img.shields.io/badge/FluentValidation-000000?style=flat-square)
![AutoMapper](https://img.shields.io/badge/AutoMapper-000000?style=flat-square)

## 🏗️ Arquitetura

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
    
    style A fill:#85EA2D
    style E fill:#512BD4,color:#fff
    style H fill:#512BD4,color:#fff
    style K fill:#CC2927,color:#fff
    style M fill:#CC2927,color:#fff
```

## 🚀 Como Executar

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

## 🔗 Acesso à API

| Serviço | URL | Status |
|---------|-----|--------|
| ![Swagger UI](https://img.shields.io/badge/Swagger-UI-green?style=flat-square) | [http://localhost:5210/swagger](http://localhost:5210/swagger) | ![Status](https://img.shields.io/badge/Status-Online-green?style=flat-square) |
| ![API JSON](https://img.shields.io/badge/API-JSON%20Schema-blue?style=flat-square) | [http://localhost:5210/swagger/v1/swagger.json](http://localhost:5210/swagger/v1/swagger.json) | ![Status](https://img.shields.io/badge/Status-Online-green?style=flat-square) |

## 📚 Documentação

- [📖 Manual de Testes](manual_de-testes.md) - Guia completo de testes da API com Swagger UI
- [📊 Diagramas](diagramas.md) - Diagramas de arquitetura e fluxos em Mermaid
- [🚀 Exemplos de Uso](exemplos-de-uso.md) - Exemplos práticos com cURL, JavaScript, Python e Postman
- [🔗 Swagger UI](http://localhost:5210/swagger) - Documentação interativa da API

## 🔐 Funcionalidades Implementadas

### ✅ Autenticação
- ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) **Registro de Usuários** - Criação de novas contas
- ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) **Login** - Autenticação com email e senha
- ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) **Validação de Token** - Verificação de tokens JWT
- ![⚠️](https://img.shields.io/badge/⚠️-Problemático-yellow?style=flat-square) **Refresh Token** - Renovação de tokens (em desenvolvimento)
- ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) **Logout** - Invalidação de tokens

### 👤 Gerenciamento de Usuários
- ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) **Perfil do Usuário** - Visualização de dados pessoais
- ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) **Busca por ID** - Consulta de usuário específico
- ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) **Lista de Usuários** - Listagem (Admin only)
- ![⚠️](https://img.shields.io/badge/⚠️-Erro%20500-yellow?style=flat-square) **Atualização** - Edição de dados (em correção)

### 🛡️ Segurança
- ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) **JWT Authentication** - Tokens seguros
- ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) **Autorização por Roles** - Controle de acesso
- ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) **Validação de Dados** - FluentValidation
- ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) **Hash de Senhas** - BCrypt
- ![✅](https://img.shields.io/badge/✅-Funcionando-green?style=flat-square) **Tratamento de Erros** - Middleware global

## 📊 Status dos Endpoints

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

## 🧪 Como Testar

### 1. 🌐 Swagger UI (Recomendado)
1. Acesse [http://localhost:5210/swagger](http://localhost:5210/swagger)
2. Clique em "Try it out" em qualquer endpoint
3. Preencha os dados necessários
4. Clique em "Execute"

### 2. 📱 cURL
```bash
# Registro
curl -X POST "http://localhost:5210/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{"nome": "João Silva", "email": "joao@exemplo.com", "senha": "MinhaSenh@123", "confirmarSenha": "MinhaSenh@123"}'

# Login
curl -X POST "http://localhost:5210/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email": "joao@exemplo.com", "senha": "MinhaSenh@123"}'
```

### 3. 🐍 Python
```python
import requests

# Login
response = requests.post("http://localhost:5210/api/auth/login", json={
    "email": "joao@exemplo.com",
    "senha": "MinhaSenh@123"
})

token = response.json()["token"]

# Usar token
headers = {"Authorization": f"Bearer {token}"}
profile = requests.get("http://localhost:5210/api/users/profile", headers=headers)
print(profile.json())
```

## 📈 Métricas de Qualidade

![Taxa de Sucesso](https://img.shields.io/badge/Taxa%20de%20Sucesso-83.3%25-green?style=for-the-badge)

| Categoria | Total | ✅ Sucesso | ❌ Falha | ⚠️ Problemas |
|-----------|-------|------------|----------|--------------|
| ![Autenticação](https://img.shields.io/badge/Autenticação-5-blue?style=flat-square) | 5 | 4 | 0 | 1 |
| ![Usuários](https://img.shields.io/badge/Usuários-3-green?style=flat-square) | 3 | 2 | 0 | 1 |
| ![Segurança](https://img.shields.io/badge/Segurança-4-red?style=flat-square) | 4 | 4 | 0 | 0 |
| **Total** | **12** | **10** | **0** | **2** |

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

### 🛠️ Comandos de Diagnóstico

```bash
# Verificar se o projeto compila
dotnet build

# Verificar migrações pendentes
dotnet ef migrations list

# Aplicar migrações
dotnet ef database update

# Verificar conectividade
curl -I http://localhost:5210/swagger
```

## 👥 Equipe de Desenvolvimento

| Nome | RM | GitHub | Responsabilidade |
|------|----|---------|------------------|
| ![Amanda](https://img.shields.io/badge/Amanda-Mesquita%20Cirino%20Da%20Silva-green?style=flat-square) | ![RM](https://img.shields.io/badge/RM-559177-blue?style=flat-square) | [![GitHub](https://img.shields.io/badge/GitHub-carmipa-black?style=flat-square&logo=github)](https://github.com/carmipa) | Backend & JWT |
| ![Journey](https://img.shields.io/badge/Journey-Tiago%20Lopes%20Ferreira-green?style=flat-square) | ![RM](https://img.shields.io/badge/RM-556071-blue?style=flat-square) | [![GitHub](https://img.shields.io/badge/GitHub-JouTiago-black?style=flat-square&logo=github)](https://github.com/JouTiago) | Testes & Documentação |
| ![Paulo](https://img.shields.io/badge/Paulo-André%20Carminati-green?style=flat-square) | ![RM](https://img.shields.io/badge/RM-557881-blue?style=flat-square) | [![GitHub](https://img.shields.io/badge/GitHub-mandyy14-black?style=flat-square&logo=github)](https://github.com/mandyy14) | Arquitetura & DevOps |

## 🔗 Links Úteis

- ![Repositório](https://img.shields.io/badge/Repositório-GitHub-black?style=flat-square&logo=github) [Repositório Principal](https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025)
- ![Projeto](https://img.shields.io/badge/Projeto-CP5%20JWT-black?style=flat-square&logo=github) [Projeto CP5](https://github.com/carmipa/Advanced_Business_Development_with_.NET_CP_2SEM_2025/tree/main/cp-5-autenticacao-autorizacao-swt)
- ![Swagger](https://img.shields.io/badge/Swagger-UI-green?style=flat-square&logo=swagger) [Swagger UI](http://localhost:5210/swagger)

## 📅 Informações do Projeto

- **Data de Entrega**: 20/10/2025
- **Grupo**: Até 3 pessoas
- **Status**: ![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow?style=flat-square)

## 📄 Licença

![MIT License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

Este projeto está licenciado sob a Licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

---

<div align="center">

![SafeScribe](https://img.shields.io/badge/SafeScribe-CP5%20JWT%20API-blue?style=for-the-badge)

**Desenvolvido com ❤️ pela equipe SafeScribe - FIAP 2025**

![FIAP](https://img.shields.io/badge/FIAP-Advanced%20Business%20Development-purple?style=for-the-badge)

</div>
