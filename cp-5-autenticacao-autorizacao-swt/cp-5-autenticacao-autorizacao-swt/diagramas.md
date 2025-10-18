# 📊 Diagramas de Fluxo - SafeScribe API

## 🔐 Fluxo de Autenticação JWT

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Frontend      │    │   Backend API   │    │   Database      │
│                 │    │                 │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │                       │                       │
    ┌────▼────┐              ┌───▼───┐              ┌────▼────┐
    │1. Login │              │Auth   │              │User     │
    │Request  │─────────────▶│Service│              │Table    │
    └─────────┘              └───┬───┘              └─────────┘
         │                       │                       │
         │                       │ 2. Validate           │
         │                       │    Credentials        │
         │                       │                       │
         │                       ▼                       │
         │              ┌─────────────────┐              │
         │              │3. Generate JWT  │              │
         │              │   + Refresh     │              │
         │              │   Token         │              │
         │              └─────────────────┘              │
         │                       │                       │
         │ 4. Return Tokens      │                       │
         │◄──────────────────────┤                       │
         │                       │                       │
    ┌────▼────┐              ┌───▼───┐              ┌────▼────┐
    │5. Store │              │Save   │              │Update   │
    │Tokens   │              │Refresh│─────────────▶│Refresh  │
    └─────────┘              │Token  │              │Token    │
                             └───────┘              └─────────┘
```

## 🔄 Fluxo de Refresh Token

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Frontend      │    │   Backend API   │    │   Database      │
│                 │    │                 │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │                       │                       │
    ┌────▼────┐              ┌───▼───┐              ┌────▼────┐
    │1. JWT   │              │Auth   │              │User     │
    │Expired  │              │Service│              │Table    │
    └─────────┘              └───┬───┘              └─────────┘
         │                       │                       │
         │ 2. Send Refresh       │                       │
         │    Token Request      │                       │
         │──────────────────────▶│                       │
         │                       │                       │
         │                       │ 3. Validate           │
         │                       │    Refresh Token      │
         │                       │                       │
         │                       ▼                       │
         │              ┌─────────────────┐              │
         │              │4. Generate New  │              │
         │              │   JWT + Refresh │              │
         │              │   Token         │              │
         │              └─────────────────┘              │
         │                       │                       │
         │ 5. Return New Tokens  │                       │
         │◄──────────────────────┤                       │
         │                       │                       │
         │                       │ 6. Update Refresh     │
         │                       │    Token in DB        │
         │                       │──────────────────────▶│
```

## 🛡️ Fluxo de Autorização

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Client        │    │   Middleware    │    │   Controller    │
│                 │    │                 │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │                       │                       │
    ┌────▼────┐              ┌───▼───┐              ┌────▼────┐
    │1. Send  │              │JWT    │              │Protected│
    │Request  │              │Auth   │              │Endpoint │
    │+ Token  │─────────────▶│MW     │              │         │
    └─────────┘              └───┬───┘              └─────────┘
         │                       │                       │
         │                       │ 2. Validate JWT       │
         │                       │                       │
         │                       ▼                       │
         │              ┌─────────────────┐              │
         │              │3. Extract Claims│              │
         │              │   (User ID,     │              │
         │              │    Role, etc.)  │              │
         │              └─────────────────┘              │
         │                       │                       │
         │                       │ 4. Check Authorization│
         │                       │                       │
         │                       ▼                       │
         │              ┌─────────────────┐              │
         │              │5. Allow/Deny    │              │
         │              │   Access        │              │
         │              └─────────────────┘              │
         │                       │                       │
         │                       │ 6. Forward to         │
         │                       │    Controller         │
         │                       │──────────────────────▶│
         │                       │                       │
         │                       │ 7. Process Request    │
         │                       │◄──────────────────────┤
         │                       │                       │
         │ 8. Return Response    │                       │
         │◄──────────────────────┼───────────────────────┤
```

## 🏗️ Arquitetura de Camadas

```
┌─────────────────────────────────────────────────────────────────┐
│                    🌐 PRESENTATION LAYER                        │
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐             │
│  │AuthController│  │UsersController│  │Middleware   │             │
│  │             │  │              │  │             │             │
│  │• Login      │  │• Get Profile │  │• JWT Auth   │             │
│  │• Register   │  │• Update User │  │• Exception  │             │
│  │• Validate   │  │• List Users  │  │• Logging    │             │
│  │• Refresh    │  │• Delete User │  │• Validation │             │
│  │• Logout     │  │              │  │             │             │
│  └─────────────┘  └─────────────┘  └─────────────┘             │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                    🏢 APPLICATION LAYER                         │
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐             │
│  │AuthService  │  │UserService  │  │Validators   │             │
│  │             │  │             │  │             │             │
│  │• LoginAsync │  │• GetByIdAsync│  │• LoginRequest│            │
│  │• RegisterAsync│ │• GetAllAsync │  │• RegisterRequest│        │
│  │• ValidateToken│ │• UpdateAsync │  │• UserRequest│            │
│  │• RefreshToken│ │• DeleteAsync │  │             │             │
│  │• LogoutAsync │ │• ToggleBlock │  │             │             │
│  └─────────────┘  └─────────────┘  └─────────────┘             │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                      🎯 DOMAIN LAYER                           │
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐             │
│  │User Entity  │  │Enums        │  │Interfaces   │             │
│  │             │  │             │  │             │             │
│  │• Id         │  │• UserRole   │  │• IAuthService│            │
│  │• Nome       │  │• UserStatus │  │• IUserService│            │
│  │• Email      │  │             │  │• IUserRepo  │             │
│  │• SenhaHash  │  │             │  │             │             │
│  │• Role       │  │             │  │             │             │
│  │• Status     │  │             │  │             │             │
│  │• Timestamps │  │             │  │             │             │
│  └─────────────┘  └─────────────┘  └─────────────┘             │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                   🗄️ INFRASTRUCTURE LAYER                      │
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐             │
│  │UserRepository│  │DbContext    │  │Database     │             │
│  │             │  │             │  │             │             │
│  │• GetById    │  │• Users DbSet│  │• SQL Server │             │
│  │• GetByEmail │  │• Configs    │  │• LocalDB    │             │
│  │• GetAll     │  │• Migrations │  │• Tables     │             │
│  │• Add        │  │• Relations  │  │• Indexes    │             │
│  │• Update     │  │             │  │• Constraints│             │
│  │• Delete     │  │             │  │             │             │
│  └─────────────┘  └─────────────┘  └─────────────┘             │
└─────────────────────────────────────────────────────────────────┘
```

## 📊 Fluxo de Dados - Request/Response

```
Client Request
     │
     ▼
┌─────────────┐
│HTTP Request │
│• Method     │
│• URL        │
│• Headers    │
│• Body       │
└─────────────┘
     │
     ▼
┌─────────────┐
│Middleware   │
│• Auth       │
│• Exception  │
│• Logging    │
└─────────────┘
     │
     ▼
┌─────────────┐
│Controller   │
│• Route      │
│• Validation │
│• Business   │
└─────────────┘
     │
     ▼
┌─────────────┐
│Service      │
│• Logic      │
│• Rules      │
│• Processing │
└─────────────┘
     │
     ▼
┌─────────────┐
│Repository   │
│• Data Access│
│• Queries    │
│• Persistence│
└─────────────┘
     │
     ▼
┌─────────────┐
│Database     │
│• SQL Server │
│• Tables     │
│• Data       │
└─────────────┘
     │
     ▼
┌─────────────┐
│Response     │
│• Status Code│
│• Headers    │
│• Body       │
└─────────────┘
     │
     ▼
Client Response
```

## 🔐 Estados de Autenticação

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Não           │    │   Autenticado   │    │   Autorizado    │
│   Autenticado   │    │                 │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │                       │                       │
    ┌────▼────┐              ┌───▼───┐              ┌────▼────┐
    │• Login  │              │• JWT  │              │• Role   │
    │• Register│             │  Valid│              │  Check  │
    │• Public │              │• Claims│             │• Permissions│
    │  Endpoints│            │  Set  │              │• Resource│
    └─────────┘              └───┬───┘              │  Access │
         │                       │                  └─────────┘
         │ 1. Login Success      │                       │
         │──────────────────────▶│                       │
         │                       │                       │
         │                       │ 2. Role Check         │
         │                       │──────────────────────▶│
         │                       │                       │
         │                       │ 3. Access Granted     │
         │                       │◄──────────────────────┤
         │                       │                       │
         │ 4. Full Access        │                       │
         │◄──────────────────────┼───────────────────────┤
```

## 📈 Métricas e Monitoramento

```
┌─────────────────────────────────────────────────────────────────┐
│                    📊 MONITORING DASHBOARD                     │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  🔐 Authentication Metrics                                     │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐              │
│  │Login Success│ │Token Valid  │ │Refresh Token│              │
│  │    95%      │ │    98%      │ │    92%      │              │
│  └─────────────┘ └─────────────┘ └─────────────┘              │
│                                                                 │
│  🛡️ Security Metrics                                           │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐              │
│  │Failed Logins│ │Invalid Tokens│ │Access Denied│              │
│  │    12/hour  │ │    3/hour   │ │    5/hour   │              │
│  └─────────────┘ └─────────────┘ └─────────────┘              │
│                                                                 │
│  🚀 Performance Metrics                                        │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐              │
│  │Response Time│ │Throughput   │ │Error Rate   │              │
│  │   45ms      │ │  1200/min   │ │   0.5%      │              │
│  └─────────────┘ └─────────────┘ └─────────────┘              │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

## 🔄 Ciclo de Vida do Token JWT

```
Token Creation
     │
     ▼
┌─────────────┐
│Login/Register│
│• Validate    │
│• Generate    │
│• Sign        │
└─────────────┘
     │
     ▼
┌─────────────┐
│Token Active │
│• Valid      │
│• Usable     │
│• Claims OK  │
└─────────────┘
     │
     ▼
┌─────────────┐
│Token Near   │
│Expiration   │
│• Refresh    │
│• Extend     │
└─────────────┘
     │
     ▼
┌─────────────┐
│Token Expired│
│• Invalid    │
│• Refresh    │
│• New Token  │
└─────────────┘
     │
     ▼
┌─────────────┐
│Token Revoked│
│• Logout     │
│• Security   │
│• Invalidate │
└─────────────┘
```

## 🎯 Casos de Uso Principais

```
┌─────────────────────────────────────────────────────────────────┐
│                    🎯 USE CASES DIAGRAM                        │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  👤 User Management                                             │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │ 1. Register User → 2. Login → 3. Access Resources      │    │
│  │ 4. Update Profile → 5. Logout                           │    │
│  └─────────────────────────────────────────────────────────┘    │
│                                                                 │
│  🔐 Authentication Flow                                         │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │ 1. Request Token → 2. Validate → 3. Authorize          │    │
│  │ 4. Access Granted → 5. Refresh Token                   │    │
│  └─────────────────────────────────────────────────────────┘    │
│                                                                 │
│  🛡️ Security Scenarios                                          │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │ 1. Invalid Credentials → 2. Deny Access                │    │
│  │ 3. Token Expired → 4. Refresh → 5. Continue            │    │
│  │ 6. Unauthorized Access → 7. Log Security Event         │    │
│  └─────────────────────────────────────────────────────────┘    │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

## 📝 Fluxo de Gestão de Notas

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Frontend      │    │   Backend API   │    │   Database      │
│                 │    │                 │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │                       │                       │
    ┌────▼────┐              ┌───▼───┐              ┌────▼────┐
    │1. Create│              │Notes  │              │Notes    │
    │Note     │─────────────▶│Service│              │Table    │
    │Request  │              └───┬───┘              └─────────┘
    └─────────┘                  │                       │
         │                       │ 2. Validate           │
         │                       │    User Role          │
         │                       │    (Editor/Admin)     │
         │                       │                       │
         │                       ▼                       │
         │              ┌─────────────────┐              │
         │              │3. Create Note   │              │
         │              │   Entity        │              │
         │              └─────────────────┘              │
         │                       │                       │
         │                       │ 4. Save to DB         │
         │                       │─────────────────────▶│
         │                       │                       │
         │ 5. Return Note        │                       │
         │◄──────────────────────┤                       │
         │                       │                       │
```

## 🚪 Fluxo de Logout com Blacklist

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Frontend      │    │   Backend API   │    │   Blacklist     │
│                 │    │                 │    │   Service       │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │                       │                       │
    ┌────▼────┐              ┌───▼───┐              ┌────▼────┐
    │1. Logout│              │Auth   │              │Token    │
    │Request  │─────────────▶│Service│              │Blacklist│
    │(JWT)    │              └───┬───┘              └─────────┘
    └─────────┘                  │                       │
         │                       │ 2. Extract JTI        │
         │                       │    from JWT           │
         │                       │                       │
         │                       ▼                       │
         │              ┌─────────────────┐              │
         │              │3. Add to        │              │
         │              │   Blacklist     │─────────────▶│
         │              └─────────────────┘              │
         │                       │                       │
         │ 4. Success Response   │                       │
         │◄──────────────────────┤                       │
         │                       │                       │
```

## 🛡️ Fluxo de Tratamento de Exceções

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Controller    │    │   Middleware    │    │   Client        │
│                 │    │                 │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │                       │                       │
    ┌────▼────┐              ┌───▼───┐              ┌────▼────┐
    │1. Business│             │Global │              │HTTP     │
    │Exception │─────────────▶│Exception│            │Response │
    │Thrown    │              │Handler │             │(JSON)   │
    └─────────┘              └───┬───┘              └─────────┘
         │                       │                       │
         │                       │ 2. Classify           │
         │                       │    Exception          │
         │                       │                       │
         │                       ▼                       │
         │              ┌─────────────────┐              │
         │              │3. Generate      │              │
         │              │   Error Response│              │
         │              └─────────────────┘              │
         │                       │                       │
         │                       │ 4. Log Error          │
         │                       │    Details            │
         │                       │                       │
         │ 5. Return Error       │                       │
         │◄──────────────────────┤                       │
         │                       │                       │
```

---

## 📝 Como Usar Estes Diagramas

### 🖼️ Renderização no GitHub

1. **Mermaid Diagrams**: Use a sintaxe Mermaid em blocos de código:
   ```mermaid
   graph TB
       A[Start] --> B[Process]
       B --> C[End]
   ```

2. **ASCII Diagrams**: Use blocos de código com `text`:
   ```text
   ┌─────────────┐
   │    Box      │
   └─────────────┘
   ```

3. **Shields.io Badges**: Use URLs diretas:
   ```markdown
   ![Status](https://img.shields.io/badge/Status-Online-green)
   ```

### 🛠️ Ferramentas Recomendadas

- ![Mermaid](https://img.shields.io/badge/Mermaid-Diagrams-blue?style=flat-square&logo=mermaid) [Mermaid Live Editor](https://mermaid.live/)
- ![Draw.io](https://img.shields.io/badge/Draw.io-Diagrams-orange?style=flat-square&logo=diagrams.net) [Draw.io](https://app.diagrams.net/)
- ![Shields.io](https://img.shields.io/badge/Shields.io-Badges-green?style=flat-square) [Shields.io](https://shields.io/)

---

<div align="center">

![Diagramas](https://img.shields.io/badge/Diagramas-CP5%20JWT%20API-blue?style=for-the-badge)

**Diagramas criados para SafeScribe API - CP5 JWT**

</div>
