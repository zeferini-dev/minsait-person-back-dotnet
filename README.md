# Zeferini Person API - .NET Core

API RESTful para gerenciamento de pessoas usando Event Sourcing, implementada em .NET Core 10.

## Tecnologias

- .NET Core 10
- ASP.NET Core Web API
- Swagger/OpenAPI
- PostgreSQL (Event Store)
- Docker

## Estrutura do Projeto

```
Minsait-Person-api-dotnet/
├── Controllers/
│   └── PersonsController.cs    # Endpoints REST
├── DTOs/
│   ├── CreatePersonDto.cs      # DTO para criação
│   └── UpdatePersonDto.cs      # DTO para atualização
├── Models/
│   ├── Event.cs                # Entidade de evento
│   └── Person.cs               # Entidade de pessoa
├── Services/
│   ├── EventsService.cs        # Serviço de Event Sourcing
│   └── PersonService.cs        # Serviço de negócio
├── Program.cs                  # Configuração da aplicação
├── Dockerfile                  # Containerização
└── Minsait-Person-api-dotnet.csproj
```

## Endpoints

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | /persons | Criar pessoa |
| GET | /persons | Listar todas as pessoas |
| GET | /persons/{id} | Buscar pessoa por ID |
| PATCH | /persons/{id} | Atualizar pessoa |
| DELETE | /persons/{id} | Remover pessoa |

## Variáveis de Ambiente

| Variável | Descrição | Padrão |
|----------|-----------|--------|
| PORT | Porta da aplicação | 3000 |
| EVENTS_DATABASE_URL | URL do banco de eventos | postgresql://events:events123@localhost:5433/eventstore |

## Executar Localmente

```bash
# Restaurar dependências
dotnet restore

# Executar em desenvolvimento
dotnet run

# Ou com hot reload
dotnet watch run
```

## Docker

```bash
# Build da imagem
docker build -t Minsait-Person-api-dotnet .

# Executar container
docker run -p 3000:3000 \
  -e EVENTS_DATABASE_URL=postgresql://events:events123@host.docker.internal:5433/eventstore \
  Minsait-Person-api-dotnet
```

## Swagger

Acesse a documentação da API em: http://localhost:3000

## Event Sourcing

Esta API utiliza Event Sourcing para persistência. Todos os comandos (create, update, delete) geram eventos que são armazenados no PostgreSQL. O estado atual é reconstruído a partir dos eventos.

### Tipos de Eventos

- `PersonCreated` - Quando uma pessoa é criada
- `PersonUpdated` - Quando uma pessoa é atualizada
- `PersonDeleted` - Quando uma pessoa é removida
