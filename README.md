# Prize lottery Web API and Test project
The solution contains two projects:
- [Web API](https://github.com/RabaGhast/prize-lottery/tree/main/src/Web-API): The Web API itself.
- [Test project](https://github.com/RabaGhast/prize-lottery/tree/main/src/WEP-API.Tests): Project containing tests for each endpoint.


## Web API
This project is an ASP.NET Core 6 Web API. It uses SQLite for storing the data.

The API has built-in Swagger documentation that can be accessed from `https://localhost:44354/swagger`. This provides an easy way to test the API endpoints and understand their functionality.

### Running the API
You can run the API either by using the .NET runtime, which will use IIS by default.

1. Clone the repository to your local machine.
2. Open the project in Visual Studio.
3. Build the project.
4. Run the project.

## Endpoints
The project contains two main entity types, tickets and prizes.

### Tickets
Full CRUD implementation for managing tickets, in addition to:
- `/reserve`: Reserve a specific ticket for a user.
- `/pay`: Pay all tickets for a specific user.
- `/draw`: Draw a random payed ticket.
- `/initialize`: Remove all existing tickets and generate new blank ones. The number of tickets can be specified in the query.

### Prizes
Full CRUD implementation for managing prizes, in addition to:
- `/draw`: First draw a winning ticket, then call this endpoint with the winning ticket number. API will pick the cheapest wine.
- `/initialize`: Remove all existing prizes. Use it together with the `Tickets/initialize` to start a new lottery.
