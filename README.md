# LetsTripTogether - Public API

## 📋 Sobre o Projeto

**LetsTripTogether Public API** é uma API pública desenvolvida para atuar como **BFF (Backend for Frontend)** e gateway entre clientes externos e a API interna do sistema. Ela fornece uma camada de abstração que simplifica a comunicação com a API interna, gerencia autenticação, processa notificações em tempo real e oferece uma interface unificada para aplicações frontend.

### Objetivo

O objetivo principal desta API é fornecer:
- **Gateway/Proxy**: Intermediar todas as requisições entre clientes e a API interna
- **BFF (Backend for Frontend)**: Otimizar e adaptar respostas para necessidades específicas do frontend
- **Notificações em Tempo Real**: Sistema de notificações via SignalR para eventos importantes
- **Autenticação Unificada**: Gerenciamento centralizado de tokens JWT e autenticação
- **CORS e Segurança**: Configuração de CORS e políticas de segurança para clientes externos
- **Processamento de Eventos**: Recebimento e processamento de eventos da API interna

## 🏗️ Arquitetura

Este projeto segue os princípios da **Clean Architecture** (Arquitetura Limpa) e **CQRS**, organizando o código em camadas bem definidas com responsabilidades claras:

```
┌─────────────────────────────────────────┐
│           WebApi (Presentation)         │  ← Controllers, SignalR Hubs, Middleware, Configuração HTTP
├─────────────────────────────────────────┤
│         Application (Use Cases)         │  ← Handlers, Validators, DTOs, Behaviours
├─────────────────────────────────────────┤
│            Domain (Core)                │  ← Value Objects, Eventos, Constantes de Segurança
├─────────────────────────────────────────┤
│        Infrastructure (External)        │  ← HTTP Clients, SignalR Services, Event Handlers, Health Checks
└─────────────────────────────────────────┘
```

### Camadas

#### 1. **Domain** (Camada de Domínio)
- **Responsabilidade**: Contém entidades de domínio e eventos relacionados a notificações
- **Contém**:
  - Value Objects (RealTimeNotification)
  - Eventos de domínio (NotificationEvents)
  - Constantes de segurança (Claims, TokenTypes)
- **Características**: Zero dependências externas, regras de negócio encapsuladas

#### 2. **Application** (Camada de Aplicação)
- **Responsabilidade**: Orquestra os casos de uso e coordena chamadas à API interna
- **Contém**:
  - Handlers (MediatR) para cada caso de uso
  - Validators (FluentValidation)
  - DTOs (Commands, Queries, Responses)
  - Behaviours (Validation, Exception Handling)
  - Interfaces de serviços (IInternalApiService, IRealTimeNotificationService)
  - Extensions para HttpContext e UserContext
- **Padrões**: CQRS (Command Query Responsibility Segregation) com MediatR

#### 3. **Infrastructure** (Camada de Infraestrutura)
- **Responsabilidade**: Implementa detalhes técnicos e integrações externas
- **Contém**:
  - HTTP Client Service (comunicação com API interna)
  - InternalApiService (proxy para todos os endpoints da API interna)
  - SignalR Services (RealTimeNotificationService)
  - Event Handlers (processamento de eventos da API interna)
  - Configurações (InternalApiSettings, JsonWebTokenSettings)
  - Health Checks (verificação de saúde da API interna)
- **Tecnologias**: HTTP Client, SignalR, JWT

#### 4. **WebApi** (Camada de Apresentação)
- **Responsabilidade**: Expõe a API REST, gerencia requisições HTTP e notificações em tempo real
- **Contém**:
  - Controllers (v1, Error, Health, Notification)
  - SignalR Hubs (NotificationHub)
  - Startup/Program configuration
  - Middleware pipeline
  - Swagger/OpenAPI
  - Health checks
  - CORS configuration
- **Características**: Versionamento de API, documentação automática, notificações em tempo real

## 🛠️ Tecnologias Utilizadas

### Backend
- **.NET 8.0** - Framework principal
- **C#** - Linguagem de programação
- **ASP.NET Core** - Framework web

### Arquitetura e Padrões
- **MediatR** - Implementação do padrão Mediator para CQRS
- **FluentValidation** - Validação de dados
- **AutoMapper** - Mapeamento de objetos

### Comunicação
- **HTTP Client** - Comunicação com a API interna
- **SignalR** - Notificações em tempo real via WebSocket
- **JWT (JSON Web Tokens)** - Autenticação stateless

### Autenticação e Segurança
- **JWT (JSON Web Tokens)** - Autenticação stateless
- **Microsoft.AspNetCore.Authentication.JwtBearer** - Middleware de autenticação JWT
- **CORS** - Cross-Origin Resource Sharing para clientes externos

### Documentação e Testes
- **Swashbuckle.AspNetCore** - Swagger/OpenAPI
- **NUnit** - Framework de testes
- **Moq** - Mocking para testes unitários
- **FluentAssertions** - Assertions expressivas em testes

### DevOps
- **Docker** - Containerização

## 🎯 Conceitos Principais

### Backend for Frontend (BFF)

A API pública atua como um **BFF**, oferecendo:
- **Abstração de Complexidade**: Oculta detalhes de implementação da API interna
- **Otimização de Respostas**: Adapta e otimiza dados para necessidades do frontend
- **Agregação de Dados**: Combina dados de múltiplas fontes quando necessário
- **Gerenciamento de Estado**: Centraliza lógica de autenticação e sessão

### CQRS (Command Query Responsibility Segregation)

O projeto utiliza **MediatR** para separar comandos (mudanças de estado) de queries (consultas):

- **Commands**: Operações que modificam estado (CreateGroup, VoteAtDestination, etc.)
- **Queries**: Operações de leitura (GetGroupById, GetAllGroups, etc.)

Cada caso de uso possui:
- `Handler`: Lógica de processamento e chamada à API interna
- `Validator`: Validação de entrada (FluentValidation)
- `Command/Query`: DTO de entrada
- `Response`: DTO de saída

### Notificações em Tempo Real

Sistema de notificações via **SignalR**:
- **NotificationHub**: Hub SignalR que gerencia conexões de clientes
- **RealTimeNotificationService**: Serviço que envia notificações para usuários específicos
- **Event Handlers**: Processam eventos recebidos da API interna e geram notificações
- **Grupos por Usuário**: Cada usuário é adicionado a um grupo (`user_{userId}`) para receber notificações personalizadas

### Proxy Pattern

A API pública atua como proxy para a API interna:
- **InternalApiService**: Serviço que encapsula todas as chamadas HTTP à API interna
- **HttpClientService**: Cliente HTTP configurado com base address da API interna
- **Transparência**: Mantém a mesma interface da API interna, mas com camada adicional de processamento

### Clean Architecture

- **Independência de Frameworks**: O domínio não depende de nenhum framework
- **Testabilidade**: Cada camada pode ser testada independentemente
- **Inversão de Dependências**: Interfaces no domínio e application, implementações na infraestrutura

### Padrões Implementados

1. **Proxy Pattern**: Abstração de acesso à API interna
2. **Mediator Pattern**: Desacoplamento via MediatR
3. **Strategy Pattern**: Diferentes estratégias de validação e comportamento
4. **Observer Pattern**: Sistema de eventos e notificações

## 🚀 Como Executar

### Pré-requisitos

- **.NET SDK 8.0** ou superior
- **API Interna em execução** (letstriptogether-internal-api)
  - A API interna deve estar rodando e acessível
  - Por padrão, espera-se em `http://localhost:5088/api/`

### Configuração Inicial

1. **Clone o repositório**
```bash
git clone <repository-url>
cd letstriptogether-public-api
```

2. **Configurar variáveis de ambiente**

Crie ou edite `src/WebApi/appsettings.Development.json` com as configurações necessárias:
- **InternalApiSettings**: BaseAddress da API interna
- **JsonWebTokenSettings**: Issuer e SecretKey (deve corresponder à API interna)
- **CorsSettings**: AllowedOrigins para clientes frontend
- **Swagger**: Habilitar/desabilitar documentação

Exemplo de configuração:
```json
{
  "InternalApiSettings": {
    "BaseAddress": "http://localhost:5088/api/"
  },
  "JsonWebTokenSettings": {
    "Issuer": "http://localhost:5088",
    "SecretKey": "qF6k9J8sL1vR2wE3tY4uP5oQ6rS7tU8v"
  },
  "CorsSettings": {
    "AllowedOrigins": [
      "http://localhost:3000"
    ]
  },
  "Swagger": {
    "Enabled": true
  }
}
```

3. **Executar a aplicação**
```bash
cd src/WebApi
dotnet run
```

A API estará disponível em:
- **HTTP**: `http://localhost:5089`
- **HTTPS**: `https://localhost:7070` (se configurado)
- **Swagger**: `http://localhost:5089/swagger`
- **Health Check**: `http://localhost:5089/health`
- **SignalR Hub**: `http://localhost:5089/hubs/notifications`

## 📝 Comandos Úteis

### Execução

**Para executar a aplicação:**
```bash
dotnet run --project src/WebApi/WebApi.csproj
```

**Para executar em modo de desenvolvimento:**
```bash
dotnet watch run --project src/WebApi/WebApi.csproj
```

### Testes

**Para rodar todos os testes:**
```bash
dotnet test tests/Application.UnitTests/Application.UnitTests.csproj tests/Domain.UnitTests/Domain.UnitTests.csproj tests/Infrastructure.UnitTests/Infrastructure.UnitTests.csproj tests/WebApi.UnitTests/WebApi.UnitTests.csproj --verbosity normal
```

**Para rodar testes de um projeto específico:**
```bash
# Testes de Application
dotnet test tests/Application.UnitTests/Application.UnitTests.csproj --verbosity normal

# Testes de Domain
dotnet test tests/Domain.UnitTests/Domain.UnitTests.csproj --verbosity normal

# Testes de Infrastructure
dotnet test tests/Infrastructure.UnitTests/Infrastructure.UnitTests.csproj --verbosity normal

# Testes de WebApi
dotnet test tests/WebApi.UnitTests/WebApi.UnitTests.csproj --verbosity normal
```

## 🔐 Segurança

### Autenticação e Autorização

- **JWT Tokens**: Autenticação stateless com access e refresh tokens
- **Token Validation**: Validação de tokens emitidos pela API interna
- **CORS**: Configuração de origens permitidas para requisições cross-origin
- **Authorization Policies**: Políticas de autorização baseadas em claims

### Validação

- **FluentValidation**: Validação robusta em todas as camadas
- **Input Validation**: Validação de entrada nos handlers antes de chamar a API interna
- **Error Handling**: Tratamento centralizado de erros via ErrorController

## 📊 Funcionalidades Principais

### Autenticação
- Registro de usuário com confirmação por email
- Login com JWT
- Refresh token
- Reset de senha
- Alteração de senha (requer senha atual)
- Logout

### Gestão de Usuários
- Consultar informações do usuário atual
- Atualizar informações do usuário atual
- Alterar senha do usuário atual
- Definir preferências de viagem
- Excluir conta
- Anonimizar dados pessoais

### Gestão de Grupos
- Criar grupos de viagem
- Consultar grupos
- Adicionar/remover membros
- Gerenciar preferências do grupo
- Definir data esperada da viagem
- Consultar membros do grupo
- Consultar destinos não votados

### Sistema de Votação
- Votar em destinos (aprovar/rejeitar)
- Atualizar votos
- Consultar votos de membros
- Consultar destinos não votados

### Matching
- Consulta de matches do grupo
- Remover matches do grupo
- Notificações em tempo real quando um match é criado

### Convites
- Criar convites para grupos
- Aceitar/recusar convites
- Cancelar convites ativos
- Consultar convites
- Consultar detalhes de convite por token (informações do grupo e criador)

### Destinos
- Consultar destinos disponíveis
- Consultar detalhes de destinos

### Notificações em Tempo Real
- Conexão via SignalR para receber notificações em tempo real
- Notificações automáticas quando:
  - Um match é criado no grupo
- Processamento de eventos recebidos da API interna
- Grupos por usuário para notificações personalizadas

### Health Checks
- Verificação de saúde da API interna
- Endpoint de health check disponível em `/health`

## 🧪 Testes

O projeto possui cobertura de testes em todas as camadas:

- **Domain.UnitTests**: Testes de value objects e eventos
- **Application.UnitTests**: Testes de handlers, validators e comportamentos
- **Infrastructure.UnitTests**: Testes de serviços HTTP e event handlers
- **WebApi.UnitTests**: Testes de controllers e hubs SignalR

### Estrutura de Testes

Cada teste segue o padrão **AAA** (Arrange-Act-Assert):
- **Arrange**: Configuração do cenário
- **Act**: Execução da ação
- **Assert**: Verificação do resultado

### Tecnologias de Teste

- **NUnit** - Framework de testes
- **Moq** - Mocking para testes unitários
- **FluentAssertions** - Assertions expressivas em testes

## 📚 Documentação da API

A documentação interativa da API está disponível via **Swagger/OpenAPI** quando a aplicação está em execução:

- Acesse: `http://localhost:5089/swagger`
- A API está versionada (v1)
- Todos os endpoints estão documentados com exemplos
- Endpoints de sistema (Error, Health) também estão disponíveis

## 🔄 Fluxo de Dados

### Requisição HTTP Normal

1. **Request** → Controller recebe requisição HTTP
2. **Validation** → FluentValidation valida o input
3. **Handler** → MediatR despacha para o handler apropriado
4. **InternalApiService** → Handler chama o serviço que faz requisição HTTP à API interna
5. **HttpClientService** → Executa chamada HTTP para a API interna
6. **Response** → DTO de resposta é retornado ao cliente

### Notificações em Tempo Real

1. **Evento na API Interna** → API interna envia evento via webhook/HTTP POST
2. **NotificationController** → Recebe o evento e cria ProcessNotificationCommand
3. **Event Handler** → Handler processa o evento de acordo com o tipo
4. **RealTimeNotificationService** → Envia notificação via SignalR para o usuário
5. **NotificationHub** → Distribui notificação para clientes conectados no grupo do usuário
6. **Cliente Frontend** → Recebe notificação em tempo real via WebSocket

## 🔌 Integração com API Interna

A API pública se comunica com a API interna através de:

- **Base Address**: Configurado em `InternalApiSettings.BaseAddress`
- **HTTP Client**: Cliente HTTP configurado com base address
- **Autenticação**: Tokens JWT são repassados nas requisições à API interna
- **Health Check**: Verifica periodicamente a saúde da API interna

### Endpoints Proxyados

Todos os endpoints da API interna são proxyados através da API pública:
- `/api/v1/auth/*` - Autenticação
- `/api/v1/users/*` - Gestão de usuários
- `/api/v1/groups/*` - Gestão de grupos
- `/api/v1/destinations/*` - Consulta de destinos
- `/api/v1/invitations/*` - Gestão de convites
- `/api/health` - Health check

## 🌐 SignalR e Notificações

### Conexão ao Hub

Clientes podem se conectar ao hub de notificações:

```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5089/hubs/notifications", {
        accessTokenFactory: () => accessToken
    })
    .build();

connection.on("ReceiveNotification", (notification) => {
    console.log("Nova notificação:", notification);
});

await connection.start();
```

### Tipos de Notificações

- **match**: Notificação quando um match é criado no grupo
  - Título: "Novo match!"
  - Mensagem: Inclui nome do grupo e destino encontrado

### Estrutura de Notificação

```json
{
  "id": "guid",
  "type": "match",
  "title": "Novo match!",
  "message": "O grupo 'Nome do Grupo' encontrou um destino perfeito: Destino",
  "createdAt": "2024-01-01T00:00:00.000Z",
  "read": false
}
```

## 🤝 Contribuindo

Este é um projeto interno. Para contribuições:

1. Siga os padrões de código estabelecidos
2. Mantenha a cobertura de testes
3. Documente mudanças significativas
4. Siga os princípios de Clean Architecture e CQRS
5. Garanta que a API interna esteja acessível antes de testar

## 📄 Licença

Este projeto é de uso interno.

---

**Desenvolvido com ❤️ usando .NET 8 e Clean Architecture**
