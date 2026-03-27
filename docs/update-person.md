# Guia de Update de Pessoa

Este guia documenta como usar a acao de update da API de pessoas.

## Endpoint

- Metodo: `PATCH`
- Rota: `/persons/{id}`
- Content-Type: `application/json`

## Objetivo

Atualizar parcialmente uma pessoa ja existente.

## Regras de negocio e validacao

1. `id` deve apontar para uma pessoa existente.
2. Campos aceitos no body:
   - `name` (opcional, maximo 120 caracteres)
   - `email` (opcional, formato de email valido, maximo 180 caracteres)
3. Campos `null` nao sobrescrevem valor existente (update parcial).
4. Se `id` nao existir (ou for GUID invalido), o retorno atual e `404 Not Found`.
5. O campo `UpdatedAt` e atualizado em toda chamada de update bem-sucedida.

## Exemplos de requisicao

### Atualizar apenas nome

```bash
curl -X PATCH "http://localhost:3000/persons/0dff2ca3-7f62-4e58-8f95-ff2d13a0b1be" \
  -H "Content-Type: application/json" \
  -d "{\"name\":\"Ada King\"}"
```

### Atualizar apenas email

```bash
curl -X PATCH "http://localhost:3000/persons/0dff2ca3-7f62-4e58-8f95-ff2d13a0b1be" \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"ada.king@example.com\"}"
```

### Atualizar nome e email

```bash
curl -X PATCH "http://localhost:3000/persons/0dff2ca3-7f62-4e58-8f95-ff2d13a0b1be" \
  -H "Content-Type: application/json" \
  -d "{\"name\":\"Ada King\",\"email\":\"ada.king@example.com\"}"
```

## Exemplo de resposta de sucesso (200)

```json
{
  "id": "0dff2ca3-7f62-4e58-8f95-ff2d13a0b1be",
  "name": "Ada King",
  "email": "ada.king@example.com",
  "createdAt": "2026-03-27T18:34:01.000Z",
  "updatedAt": "2026-03-27T18:40:13.000Z"
}
```

## Exemplo de erro de validacao (400)

```json
{
  "errors": [
    {
      "propertyName": "Email",
      "errorMessage": "'Email' is not a valid email address."
    }
  ]
}
```

## Exemplo de pessoa nao encontrada (404)

```json
{
  "message": "Person with ID 0dff2ca3-7f62-4e58-8f95-ff2d13a0b1be not found"
}
```

## Checklist rapido para testar update

1. Crie uma pessoa (`POST /persons`).
2. Copie o `id` retornado.
3. Execute um `PATCH /persons/{id}` com `name` e/ou `email`.
4. Valide o retorno `200`.
5. Confirme o novo estado com `GET /persons/{id}`.

## Referencias no codigo

- Controller: `src/minsait-person-back-dotnet/Controllers/PersonsController.cs`
- Service: `src/minsait-person-back-dotnet/Services/PersonService.cs`
- DTO: `src/minsait-person-back-dotnet/DTOs/UpdatePersonDto.cs`
- Validator: `src/minsait-person-back-dotnet/DTOs/UpdatePersonDtoValidator.cs`
- Filtro de validacao: `src/minsait-person-back-dotnet/Filters/FluentValidationActionFilter.cs`
