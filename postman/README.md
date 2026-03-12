# Postman - Zeferini Person API

## Arquivos

- **Minsait-Person-API.postman_collection.json** – Coleção com todas as requisições da API
- **Minsait-Person-API-Local.postman_environment.json** – Ambiente Local (opcional)

## Importar no Postman

1. Abra o Postman
2. Clique em **Import**
3. Arraste os arquivos ou selecione a pasta `postman`
4. Selecione o ambiente **Local** na barra superior (opcional)

## Variáveis

| Variável  | Padrão              | Descrição                                       |
|-----------|---------------------|-------------------------------------------------|
| `baseUrl` | http://localhost:3000 | URL base da API                               |
| `personId`| (vazio)             | ID da pessoa criada (preenchido após Create)    |

## Fluxo sugerido

1. **Create Person** – Cria uma pessoa e define `personId` automaticamente
2. **Get Person By Id** – Busca a pessoa criada
3. **Update Person** – Atualiza a pessoa
4. **Get All Persons** – Lista todas as pessoas
5. **Delete Person** – Remove a pessoa

## Pré-requisito

A API deve estar em execução:

```bash
dotnet run --project src/minsait-person-back-dotnet
```

Ou use a porta configurada em `PORT` / `appsettings.json`.
