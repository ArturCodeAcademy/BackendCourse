# Lecture 13 - DTOs, Validation and API Design

## Why an API must not return entities directly

An entity describes internal data and persistence. An API response is a public contract. DTOs deliberately choose which fields a client can send and receive.

ExpenseResponse excludes UserId. The route identifies the user scope. CreateExpenseRequest contains only fields that a client may create.

## Validation layers

1. Presentation validates the HTTP request shape: required values, lengths, ranges.
2. Application validates business rules that must hold for every caller.
3. Infrastructure/database retains constraints as the final integrity boundary.

A client can bypass a browser form, so server-side validation is mandatory.

## API decisions

- use nouns in routes: /api/users/{userId}/expenses
- use HTTP verbs rather than route names such as /createExpense
- return 201 with a Location for successful creation
- return 404 for a missing resource
- return consistent validation errors for bad input

## Guided checks

1. POST a missing name and read the validation-problem JSON.
2. Confirm that a response does not expose UserId.
3. Add an UpdateExpenseRequest and decide whether PUT or PATCH fits.
4. Document one endpoint: method, route, request, response, status codes.
