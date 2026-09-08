# Formula AirLine Message Queue Project

This project demonstrates a simple .NET message-driven booking flow using an ASP.NET Core API and RabbitMQ.

The system accepts flight booking requests through an API endpoint, stores the booking in memory, and pushes the message to a RabbitMQ queue. A separate worker process listens to the queue and simulates processing the booking request.

## Project Overview

### Components

- `FormulaAirLine.API`
  - ASP.NET Core Web API
  - Exposes booking endpoints
  - Publishes booking messages to RabbitMQ

- `FormulaAirLine.TicketProcessing`
  - Console worker app
  - Subscribes to the RabbitMQ queue
  - Reads and processes incoming booking messages

## Tech Stack

- .NET 10
- ASP.NET Core
- RabbitMQ.Client
- Swagger / OpenAPI

## Prerequisites

Before running the project, make sure you have:

- .NET 10 SDK installed
- RabbitMQ running locally on `localhost`
- Default RabbitMQ credentials:
  - username: `guest`
  - password: `guest`

If RabbitMQ is not installed locally, you can run it with Docker:

```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

Then open the RabbitMQ management UI at:

- http://localhost:15672

## Solution Structure

```text
MessageQueue/
├── FormulaAirLine.API/
│   ├── Controller/
│   ├── Models/
│   ├── Service/
│   ├── Program.cs
│   └── FormulaAirLine.API.csproj
├── FormulaAirLine.TicketProcessing/
│   ├── Program.cs
│   └── FormulaAirLine.TicketProcessing.csproj
├── Directory.Build.props
├── Directory.Packages.props
├── FormulaAireLine.slnx
└── README.md
```

## Run the API

From the project root:

```bash
dotnet run --project FormulaAirLine.API
```

The API will start and Swagger UI will be available at:

- http://localhost:5000/
- https://localhost:5001/

Depending on your configured launch settings, Swagger is served at the root path because `c.RoutePrefix = string.Empty` is set in the API configuration.

## API Endpoint

### Create Booking

```http
POST /api/Booking
Content-Type: application/json
```

Example request body:

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "passengerName": "John Doe",
  "passportNumber": "P1234567",
  "passengerEmail": "john.doe@example.com",
  "passengerPhone": "+123456789",
  "from": "LHR",
  "to": "JFK"
}
```

The API adds the booking to an in-memory list and publishes the same object to the RabbitMQ queue named `booking-queue`.

## Run the Ticket Processor

Open a second terminal and run:

```bash
dotnet run --project FormulaAirLine.TicketProcessing
```

This worker will connect to RabbitMQ, declare the queue, and consume messages from `booking-queue`.

## Message Flow

```text
Client -> API -> RabbitMQ Queue -> Ticket Processing Worker
```

1. A client sends a booking request to the API.
2. The API validates and accepts the request.
3. The booking is stored in memory.
4. The message is serialized and published to RabbitMQ.
5. The ticket processing app reads the queue and logs the message.

## Notes

- The current booking storage is in-memory and is not persistent.
- The queue is declared as `booking-queue` with default RabbitMQ settings.
- This project is intended as a simple demonstration of asynchronous message-based processing.

## Useful Commands

```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Run API
dotnet run --project FormulaAirLine.API

# Run worker
dotnet run --project FormulaAirLine.TicketProcessing
```

## License

This project is provided as a sample/demo application for learning and experimentation.
