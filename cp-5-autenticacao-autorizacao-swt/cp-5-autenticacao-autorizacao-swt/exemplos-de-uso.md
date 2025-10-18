# 🚀 Exemplos Práticos de Uso - SafeScribe API

![Status](https://img.shields.io/badge/Status-Exemplos%20Práticos-blue?style=for-the-badge)
![API](https://img.shields.io/badge/API-JWT%20Authentication-green?style=for-the-badge)
![Swagger](https://img.shields.io/badge/Swagger-UI%20Ready-orange?style=for-the-badge)

## 📋 Índice de Exemplos

- [🔐 Fluxo Completo de Autenticação](#-fluxo-completo-de-autenticação)
- [📱 Exemplos com cURL](#-exemplos-com-curl)
- [🌐 Exemplos com JavaScript/Fetch](#-exemplos-com-javascriptfetch)
- [🐍 Exemplos com Python](#-exemplos-com-python)
- [📊 Exemplos com Postman](#-exemplos-com-postman)
- [🔄 Automação de Testes](#-automação-de-testes)

---

## 🔐 Fluxo Completo de Autenticação

### Diagrama do Fluxo Completo

```mermaid
sequenceDiagram
    participant U as Usuário
    participant F as Frontend
    participant API as SafeScribe API
    participant DB as Database
    
    Note over U,DB: 🚀 Fluxo Completo de Autenticação
    
    U->>F: 1. Acessar aplicação
    F->>API: 2. POST /api/auth/register
    Note right of F: {nome, email, senha}
    API->>DB: 3. Criar usuário
    DB-->>API: 4. Usuário criado
    API-->>F: 5. Token JWT + Refresh Token
    F->>F: 6. Armazenar tokens
    
    U->>F: 7. Fazer login
    F->>API: 8. POST /api/auth/login
    Note right of F: {email, senha}
    API->>DB: 9. Validar credenciais
    DB-->>API: 10. Usuário válido
    API-->>F: 11. Novos tokens
    F->>F: 12. Atualizar tokens
    
    U->>F: 13. Acessar recurso protegido
    F->>API: 14. GET /api/users/profile
    Note right of F: Authorization: Bearer token
    API->>API: 15. Validar JWT
    API->>DB: 16. Buscar dados
    DB-->>API: 17. Dados do usuário
    API-->>F: 18. Perfil do usuário
    F-->>U: 19. Exibir dados
    
    U->>F: 20. Fazer logout
    F->>API: 21. POST /api/auth/logout
    API->>DB: 22. Invalidar refresh token
    API-->>F: 23. Logout confirmado
    F->>F: 24. Limpar tokens
```

---

## 📱 Exemplos com cURL

### 1. 🆕 Registro de Usuário

```bash
# Registro de novo usuário
curl -X POST "http://localhost:5210/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva",
    "email": "joao@exemplo.com",
    "senha": "MinhaSenh@123",
    "confirmarSenha": "MinhaSenh@123"
  }'
```

**Resposta Esperada:**
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

### 2. 🔑 Login

```bash
# Login com credenciais
curl -X POST "http://localhost:5210/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "joao@exemplo.com",
    "senha": "MinhaSenh@123"
  }'
```

### 3. 🔒 Validar Token

```bash
# Validar token JWT
curl -X GET "http://localhost:5210/api/auth/validate" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### 4. 👤 Buscar Perfil

```bash
# Buscar perfil do usuário autenticado
curl -X GET "http://localhost:5210/api/users/profile" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### 5. 🔄 Refresh Token

```bash
# Renovar token usando refresh token
curl -X POST "http://localhost:5210/api/auth/refresh-token" \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "vgR6Hi51LZP7RB+THVOxPkOTeQq6VyeyuFv/ODPqvSw="
  }'
```

### 6. 🚪 Logout

```bash
# Fazer logout
curl -X POST "http://localhost:5210/api/auth/logout" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

## 🌐 Exemplos com JavaScript/Fetch

### 1. 🔐 Classe de Autenticação

```javascript
class SafeScribeAuth {
  constructor(baseUrl = 'http://localhost:5210') {
    this.baseUrl = baseUrl;
    this.token = localStorage.getItem('jwt_token');
    this.refreshToken = localStorage.getItem('refresh_token');
  }

  // Registro de usuário
  async register(userData) {
    try {
      const response = await fetch(`${this.baseUrl}/api/auth/register`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(userData)
      });

      if (response.ok) {
        const data = await response.json();
        this.saveTokens(data.token, data.refreshToken);
        return data;
      } else {
        throw new Error(`Erro no registro: ${response.status}`);
      }
    } catch (error) {
      console.error('Erro no registro:', error);
      throw error;
    }
  }

  // Login
  async login(email, senha) {
    try {
      const response = await fetch(`${this.baseUrl}/api/auth/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, senha })
      });

      if (response.ok) {
        const data = await response.json();
        this.saveTokens(data.token, data.refreshToken);
        return data;
      } else {
        throw new Error(`Erro no login: ${response.status}`);
      }
    } catch (error) {
      console.error('Erro no login:', error);
      throw error;
    }
  }

  // Buscar perfil
  async getProfile() {
    try {
      const response = await fetch(`${this.baseUrl}/api/users/profile`, {
        headers: {
          'Authorization': `Bearer ${this.token}`
        }
      });

      if (response.ok) {
        return await response.json();
      } else if (response.status === 401) {
        // Token expirado, tentar refresh
        await this.refreshToken();
        return await this.getProfile();
      } else {
        throw new Error(`Erro ao buscar perfil: ${response.status}`);
      }
    } catch (error) {
      console.error('Erro ao buscar perfil:', error);
      throw error;
    }
  }

  // Refresh token
  async refreshToken() {
    try {
      const response = await fetch(`${this.baseUrl}/api/auth/refresh-token`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ refreshToken: this.refreshToken })
      });

      if (response.ok) {
        const data = await response.json();
        this.saveTokens(data.token, data.refreshToken);
        return data;
      } else {
        // Refresh token inválido, fazer logout
        this.logout();
        throw new Error('Sessão expirada');
      }
    } catch (error) {
      console.error('Erro ao renovar token:', error);
      throw error;
    }
  }

  // Logout
  async logout() {
    try {
      if (this.token) {
        await fetch(`${this.baseUrl}/api/auth/logout`, {
          method: 'POST',
          headers: {
            'Authorization': `Bearer ${this.token}`
          }
        });
      }
    } catch (error) {
      console.error('Erro no logout:', error);
    } finally {
      this.clearTokens();
    }
  }

  // Salvar tokens
  saveTokens(token, refreshToken) {
    this.token = token;
    this.refreshToken = refreshToken;
    localStorage.setItem('jwt_token', token);
    localStorage.setItem('refresh_token', refreshToken);
  }

  // Limpar tokens
  clearTokens() {
    this.token = null;
    this.refreshToken = null;
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('refresh_token');
  }
}
```

### 2. 🎯 Exemplo de Uso

```javascript
// Inicializar cliente
const auth = new SafeScribeAuth();

// Exemplo de registro
async function registrarUsuario() {
  try {
    const resultado = await auth.register({
      nome: "Maria Santos",
      email: "maria@exemplo.com",
      senha: "MinhaSenh@123",
      confirmarSenha: "MinhaSenh@123"
    });
    
    console.log('Usuário registrado:', resultado.user);
    console.log('Token JWT:', resultado.token);
  } catch (error) {
    console.error('Erro no registro:', error);
  }
}

// Exemplo de login
async function fazerLogin() {
  try {
    const resultado = await auth.login("maria@exemplo.com", "MinhaSenh@123");
    console.log('Login realizado:', resultado.user);
  } catch (error) {
    console.error('Erro no login:', error);
  }
}

// Exemplo de buscar perfil
async function buscarPerfil() {
  try {
    const perfil = await auth.getProfile();
    console.log('Perfil do usuário:', perfil);
  } catch (error) {
    console.error('Erro ao buscar perfil:', error);
  }
}
```

---

## 🐍 Exemplos com Python

### 1. 📦 Instalação de Dependências

```bash
pip install requests python-dotenv
```

### 2. 🔐 Cliente Python

```python
import requests
import json
from typing import Optional, Dict, Any

class SafeScribeClient:
    def __init__(self, base_url: str = "http://localhost:5210"):
        self.base_url = base_url
        self.token: Optional[str] = None
        self.refresh_token: Optional[str] = None
        self.session = requests.Session()
    
    def register(self, nome: str, email: str, senha: str, confirmar_senha: str) -> Dict[str, Any]:
        """Registrar novo usuário"""
        url = f"{self.base_url}/api/auth/register"
        data = {
            "nome": nome,
            "email": email,
            "senha": senha,
            "confirmarSenha": confirmar_senha
        }
        
        response = self.session.post(url, json=data)
        response.raise_for_status()
        
        result = response.json()
        self.token = result["token"]
        self.refresh_token = result["refreshToken"]
        
        return result
    
    def login(self, email: str, senha: str) -> Dict[str, Any]:
        """Fazer login"""
        url = f"{self.base_url}/api/auth/login"
        data = {"email": email, "senha": senha}
        
        response = self.session.post(url, json=data)
        response.raise_for_status()
        
        result = response.json()
        self.token = result["token"]
        self.refresh_token = result["refreshToken"]
        
        return result
    
    def get_profile(self) -> Dict[str, Any]:
        """Buscar perfil do usuário"""
        if not self.token:
            raise ValueError("Usuário não autenticado")
        
        url = f"{self.base_url}/api/users/profile"
        headers = {"Authorization": f"Bearer {self.token}"}
        
        response = self.session.get(url, headers=headers)
        response.raise_for_status()
        
        return response.json()
    
    def refresh_token(self) -> Dict[str, Any]:
        """Renovar token"""
        if not self.refresh_token:
            raise ValueError("Refresh token não disponível")
        
        url = f"{self.base_url}/api/auth/refresh-token"
        data = {"refreshToken": self.refresh_token}
        
        response = self.session.post(url, json=data)
        response.raise_for_status()
        
        result = response.json()
        self.token = result["token"]
        self.refresh_token = result["refreshToken"]
        
        return result
    
    def logout(self) -> bool:
        """Fazer logout"""
        if not self.token:
            return True
        
        url = f"{self.base_url}/api/auth/logout"
        headers = {"Authorization": f"Bearer {self.token}"}
        
        try:
            response = self.session.post(url, headers=headers)
            response.raise_for_status()
            return True
        except requests.RequestException:
            return False
        finally:
            self.token = None
            self.refresh_token = None
```

### 3. 🎯 Exemplo de Uso Python

```python
def exemplo_completo():
    client = SafeScribeClient()
    
    try:
        # 1. Registrar usuário
        print("🆕 Registrando usuário...")
        resultado_registro = client.register(
            nome="Pedro Oliveira",
            email="pedro@exemplo.com",
            senha="MinhaSenh@123",
            confirmar_senha="MinhaSenh@123"
        )
        print(f"✅ Usuário registrado: {resultado_registro['user']['nome']}")
        
        # 2. Buscar perfil
        print("👤 Buscando perfil...")
        perfil = client.get_profile()
        print(f"✅ Perfil encontrado: {perfil}")
        
        # 3. Fazer logout
        print("🚪 Fazendo logout...")
        client.logout()
        print("✅ Logout realizado com sucesso")
        
    except requests.RequestException as e:
        print(f"❌ Erro na requisição: {e}")
    except Exception as e:
        print(f"❌ Erro: {e}")

# Executar exemplo
if __name__ == "__main__":
    exemplo_completo()
```

---

## 📊 Exemplos com Postman

### 1. 🗂️ Estrutura da Collection

```json
{
  "info": {
    "name": "SafeScribe API - CP5 JWT",
    "description": "Collection para testes da API SafeScribe",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "variable": [
    {
      "key": "baseUrl",
      "value": "http://localhost:5210",
      "type": "string"
    },
    {
      "key": "jwtToken",
      "value": "",
      "type": "string"
    },
    {
      "key": "refreshToken",
      "value": "",
      "type": "string"
    }
  ]
}
```

### 2. 🔐 Requests de Autenticação

#### POST Register
```json
{
  "name": "Register User",
  "request": {
    "method": "POST",
    "header": [
      {
        "key": "Content-Type",
        "value": "application/json"
      }
    ],
    "body": {
      "mode": "raw",
      "raw": "{\n  \"nome\": \"{{nome}}\",\n  \"email\": \"{{email}}\",\n  \"senha\": \"{{senha}}\",\n  \"confirmarSenha\": \"{{senha}}\"\n}"
    },
    "url": {
      "raw": "{{baseUrl}}/api/auth/register",
      "host": ["{{baseUrl}}"],
      "path": ["api", "auth", "register"]
    }
  },
  "event": [
    {
      "listen": "test",
      "script": {
        "exec": [
          "if (pm.response.code === 201) {",
          "    const response = pm.response.json();",
          "    pm.collectionVariables.set('jwtToken', response.token);",
          "    pm.collectionVariables.set('refreshToken', response.refreshToken);",
          "    console.log('Tokens salvos automaticamente');",
          "}"
        ]
      }
    }
  ]
}
```

#### POST Login
```json
{
  "name": "Login User",
  "request": {
    "method": "POST",
    "header": [
      {
        "key": "Content-Type",
        "value": "application/json"
      }
    ],
    "body": {
      "mode": "raw",
      "raw": "{\n  \"email\": \"{{email}}\",\n  \"senha\": \"{{senha}}\"\n}"
    },
    "url": {
      "raw": "{{baseUrl}}/api/auth/login",
      "host": ["{{baseUrl}}"],
      "path": ["api", "auth", "login"]
    }
  }
}
```

#### GET Profile
```json
{
  "name": "Get User Profile",
  "request": {
    "method": "GET",
    "header": [
      {
        "key": "Authorization",
        "value": "Bearer {{jwtToken}}"
      }
    ],
    "url": {
      "raw": "{{baseUrl}}/api/users/profile",
      "host": ["{{baseUrl}}"],
      "path": ["api", "users", "profile"]
    }
  }
}
```

---

## 🔄 Automação de Testes

### 1. 🧪 Script de Teste Automatizado

```javascript
// Teste automatizado com JavaScript
const testSuite = {
  async runAllTests() {
    const results = {
      passed: 0,
      failed: 0,
      tests: []
    };

    console.log('🚀 Iniciando testes automatizados...\n');

    // Teste 1: Registro
    await this.testRegister(results);
    
    // Teste 2: Login
    await this.testLogin(results);
    
    // Teste 3: Validação de token
    await this.testValidateToken(results);
    
    // Teste 4: Perfil do usuário
    await this.testUserProfile(results);
    
    // Teste 5: Logout
    await this.testLogout(results);

    this.printResults(results);
  },

  async testRegister(results) {
    try {
      const response = await fetch('http://localhost:5210/api/auth/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          nome: 'Test User',
          email: 'test@exemplo.com',
          senha: 'TestSenh@123',
          confirmarSenha: 'TestSenh@123'
        })
      });

      if (response.ok) {
        results.passed++;
        results.tests.push({ name: 'Registro', status: '✅ PASS' });
      } else {
        results.failed++;
        results.tests.push({ name: 'Registro', status: '❌ FAIL', error: response.status });
      }
    } catch (error) {
      results.failed++;
      results.tests.push({ name: 'Registro', status: '❌ ERROR', error: error.message });
    }
  },

  async testLogin(results) {
    try {
      const response = await fetch('http://localhost:5210/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          email: 'test@exemplo.com',
          senha: 'TestSenh@123'
        })
      });

      if (response.ok) {
        const data = await response.json();
        this.token = data.token;
        results.passed++;
        results.tests.push({ name: 'Login', status: '✅ PASS' });
      } else {
        results.failed++;
        results.tests.push({ name: 'Login', status: '❌ FAIL', error: response.status });
      }
    } catch (error) {
      results.failed++;
      results.tests.push({ name: 'Login', status: '❌ ERROR', error: error.message });
    }
  },

  async testValidateToken(results) {
    if (!this.token) {
      results.failed++;
      results.tests.push({ name: 'Validação Token', status: '❌ SKIP', error: 'Token não disponível' });
      return;
    }

    try {
      const response = await fetch('http://localhost:5210/api/auth/validate', {
        headers: { 'Authorization': `Bearer ${this.token}` }
      });

      if (response.ok) {
        results.passed++;
        results.tests.push({ name: 'Validação Token', status: '✅ PASS' });
      } else {
        results.failed++;
        results.tests.push({ name: 'Validação Token', status: '❌ FAIL', error: response.status });
      }
    } catch (error) {
      results.failed++;
      results.tests.push({ name: 'Validação Token', status: '❌ ERROR', error: error.message });
    }
  },

  async testUserProfile(results) {
    if (!this.token) {
      results.failed++;
      results.tests.push({ name: 'Perfil Usuário', status: '❌ SKIP', error: 'Token não disponível' });
      return;
    }

    try {
      const response = await fetch('http://localhost:5210/api/users/profile', {
        headers: { 'Authorization': `Bearer ${this.token}` }
      });

      if (response.ok) {
        results.passed++;
        results.tests.push({ name: 'Perfil Usuário', status: '✅ PASS' });
      } else {
        results.failed++;
        results.tests.push({ name: 'Perfil Usuário', status: '❌ FAIL', error: response.status });
      }
    } catch (error) {
      results.failed++;
      results.tests.push({ name: 'Perfil Usuário', status: '❌ ERROR', error: error.message });
    }
  },

  async testLogout(results) {
    if (!this.token) {
      results.failed++;
      results.tests.push({ name: 'Logout', status: '❌ SKIP', error: 'Token não disponível' });
      return;
    }

    try {
      const response = await fetch('http://localhost:5210/api/auth/logout', {
        method: 'POST',
        headers: { 'Authorization': `Bearer ${this.token}` }
      });

      if (response.ok) {
        results.passed++;
        results.tests.push({ name: 'Logout', status: '✅ PASS' });
      } else {
        results.failed++;
        results.tests.push({ name: 'Logout', status: '❌ FAIL', error: response.status });
      }
    } catch (error) {
      results.failed++;
      results.tests.push({ name: 'Logout', status: '❌ ERROR', error: error.message });
    }
  },

  printResults(results) {
    console.log('\n📊 Resultados dos Testes:');
    console.log('========================');
    
    results.tests.forEach(test => {
      console.log(`${test.status} ${test.name}`);
      if (test.error) {
        console.log(`   Erro: ${test.error}`);
      }
    });

    console.log('\n📈 Resumo:');
    console.log(`✅ Sucessos: ${results.passed}`);
    console.log(`❌ Falhas: ${results.failed}`);
    console.log(`📊 Taxa de Sucesso: ${((results.passed / (results.passed + results.failed)) * 100).toFixed(1)}%`);
  }
};

// Executar testes
testSuite.runAllTests();
```

### 2. 🐍 Teste Automatizado Python

```python
import requests
import json
import time
from datetime import datetime

class APITester:
    def __init__(self, base_url="http://localhost:5210"):
        self.base_url = base_url
        self.token = None
        self.results = {
            "passed": 0,
            "failed": 0,
            "tests": []
        }
    
    def log_test(self, name, status, error=None):
        self.results["tests"].append({
            "name": name,
            "status": status,
            "error": error,
            "timestamp": datetime.now().isoformat()
        })
        
        if status == "✅ PASS":
            self.results["passed"] += 1
        else:
            self.results["failed"] += 1
        
        print(f"{status} {name}")
        if error:
            print(f"   Erro: {error}")
    
    def test_register(self):
        """Teste de registro de usuário"""
        try:
            url = f"{self.base_url}/api/auth/register"
            data = {
                "nome": "Test User Python",
                "email": "test_python@exemplo.com",
                "senha": "TestSenh@123",
                "confirmarSenha": "TestSenh@123"
            }
            
            response = requests.post(url, json=data)
            if response.status_code == 201:
                self.log_test("Registro", "✅ PASS")
                return True
            else:
                self.log_test("Registro", "❌ FAIL", f"Status: {response.status_code}")
                return False
        except Exception as e:
            self.log_test("Registro", "❌ ERROR", str(e))
            return False
    
    def test_login(self):
        """Teste de login"""
        try:
            url = f"{self.base_url}/api/auth/login"
            data = {
                "email": "test_python@exemplo.com",
                "senha": "TestSenh@123"
            }
            
            response = requests.post(url, json=data)
            if response.status_code == 200:
                result = response.json()
                self.token = result["token"]
                self.log_test("Login", "✅ PASS")
                return True
            else:
                self.log_test("Login", "❌ FAIL", f"Status: {response.status_code}")
                return False
        except Exception as e:
            self.log_test("Login", "❌ ERROR", str(e))
            return False
    
    def test_validate_token(self):
        """Teste de validação de token"""
        if not self.token:
            self.log_test("Validação Token", "❌ SKIP", "Token não disponível")
            return False
        
        try:
            url = f"{self.base_url}/api/auth/validate"
            headers = {"Authorization": f"Bearer {self.token}"}
            
            response = requests.get(url, headers=headers)
            if response.status_code == 200:
                self.log_test("Validação Token", "✅ PASS")
                return True
            else:
                self.log_test("Validação Token", "❌ FAIL", f"Status: {response.status_code}")
                return False
        except Exception as e:
            self.log_test("Validação Token", "❌ ERROR", str(e))
            return False
    
    def test_user_profile(self):
        """Teste de perfil do usuário"""
        if not self.token:
            self.log_test("Perfil Usuário", "❌ SKIP", "Token não disponível")
            return False
        
        try:
            url = f"{self.base_url}/api/users/profile"
            headers = {"Authorization": f"Bearer {self.token}"}
            
            response = requests.get(url, headers=headers)
            if response.status_code == 200:
                self.log_test("Perfil Usuário", "✅ PASS")
                return True
            else:
                self.log_test("Perfil Usuário", "❌ FAIL", f"Status: {response.status_code}")
                return False
        except Exception as e:
            self.log_test("Perfil Usuário", "❌ ERROR", str(e))
            return False
    
    def test_logout(self):
        """Teste de logout"""
        if not self.token:
            self.log_test("Logout", "❌ SKIP", "Token não disponível")
            return False
        
        try:
            url = f"{self.base_url}/api/auth/logout"
            headers = {"Authorization": f"Bearer {self.token}"}
            
            response = requests.post(url, headers=headers)
            if response.status_code == 200:
                self.log_test("Logout", "✅ PASS")
                return True
            else:
                self.log_test("Logout", "❌ FAIL", f"Status: {response.status_code}")
                return False
        except Exception as e:
            self.log_test("Logout", "❌ ERROR", str(e))
            return False
    
    def run_all_tests(self):
        """Executar todos os testes"""
        print("🚀 Iniciando testes automatizados...\n")
        
        # Executar testes em sequência
        self.test_register()
        time.sleep(1)  # Pequena pausa entre testes
        
        self.test_login()
        time.sleep(1)
        
        self.test_validate_token()
        time.sleep(1)
        
        self.test_user_profile()
        time.sleep(1)
        
        self.test_logout()
        
        # Imprimir resultados
        self.print_results()
    
    def print_results(self):
        """Imprimir resultados dos testes"""
        print("\n📊 Resultados dos Testes:")
        print("=========================")
        
        total = self.results["passed"] + self.results["failed"]
        success_rate = (self.results["passed"] / total * 100) if total > 0 else 0
        
        print(f"\n📈 Resumo:")
        print(f"✅ Sucessos: {self.results['passed']}")
        print(f"❌ Falhas: {self.results['failed']}")
        print(f"📊 Taxa de Sucesso: {success_rate:.1f}%")
        
        # Salvar resultados em arquivo
        with open("test_results.json", "w", encoding="utf-8") as f:
            json.dump(self.results, f, indent=2, ensure_ascii=False)
        
        print(f"\n💾 Resultados salvos em: test_results.json")

# Executar testes
if __name__ == "__main__":
    tester = APITester()
    tester.run_all_tests()
```

---

## 📋 Checklist de Exemplos

### ✅ Exemplos Implementados

- [x] ![cURL](https://img.shields.io/badge/cURL-Exemplos%20Completos-green?style=flat-square) Exemplos com cURL
- [x] ![JavaScript](https://img.shields.io/badge/JavaScript-Fetch%20API-blue?style=flat-square) Exemplos com JavaScript/Fetch
- [x] ![Python](https://img.shields.io/badge/Python-Requests%20Library-yellow?style=flat-square) Exemplos com Python
- [x] ![Postman](https://img.shields.io/badge/Postman-Collection-orange?style=flat-square) Exemplos com Postman
- [x] ![Automação](https://img.shields.io/badge/Automação-Testes%20Automáticos-purple?style=flat-square) Automação de Testes

### 🎯 Próximos Passos

1. **Teste os exemplos** no seu ambiente local
2. **Customize os dados** conforme necessário
3. **Integre com sua aplicação** frontend
4. **Implemente tratamento de erros** adequado
5. **Adicione logs** para monitoramento

---

<div align="center">

![Exemplos](https://img.shields.io/badge/Exemplos-CP5%20JWT%20API-blue?style=for-the-badge)

**Exemplos práticos para SafeScribe API - CP5 JWT**

![Status](https://img.shields.io/badge/Status-Ready%20to%20Use-green?style=for-the-badge)

</div>
