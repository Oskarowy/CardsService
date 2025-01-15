# Bank Cards Handling Microservice

A microservice built with .NET 8 Minimal API to determine the allowed actions for a user's bank card based on its type and status.

## Features

- Exposes a single API endpoint to retrieve allowed actions for a given user card.
- Implements `Policy Pattern` for flexible and maintainable business logic.
- Supports future expansion for new card types, statuses, or criteria.
- Fully covered with unit and integration tests using xUnit and Moq.
- Designed with TDD and SOLID principles.
- Using SwaggerUI to explore API endpoints.

## API Endpoint

**POST** `/api/allowed-actions`

### Request Body
```json
{
  "userId": "UserId",
  "cardNumber": "CardNumber"
}
```

### Response
```json
{
  "allowedActions": ["Action1", "Action2", "Action3"]
}
```

### Error Responses
- `400 Bad Request`: Missing or invalid `userId` or `cardNumber`.
- `404 Not Found`: Card details not found for the provided user and card.

## Technologies Used

- **.NET 8 Minimal API**
- **C#**
- **xUnit** for unit and integration testing
- **Moq** for mocking dependencies

## Project Structure

- `Services` - Core business logic.
- `Policies` - Action policies implementing `IActionPolicy` interface.
- `Tests` - Unit and integration tests.

## Running the Project

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd cards-microservice
   ```

2. **Build and run the service**:
   ```bash
   dotnet build
   dotnet run
   ```

3. **Run tests**:
   ```bash
   dotnet test
   ```

## Future Enhancements

- Add new criteria such as `ClientType` with values like `Corporate`,`Retail`,`VIP`, etc.
- Add Docker support
- Implement authentication and authorization.
- Integrate with a real external card service - there is currently a Mock with simulated delay.
