# KycAggregator Solution

## Overview

This solution is a KYC (Know Your Customer) Aggregator service built using .NET Core. It efficiently handles KYC operations and integrates various features such as error handling, caching, JSON annotations for model binding, and modular architecture. The project is designed to be scalable, maintainable, and adaptable for future enhancements like switching to Redis for caching.

## Features

- **Efficient Error Handling**: 
  We have implemented centralized and robust error handling mechanisms across the application to ensure smooth and safe execution of operations.

- **Persistent Caching**: 
  The solution currently uses `FileCache` to persist data and optimize performance by caching frequently used data. It also supports easy migration to **Redis** for distributed caching in future implementations.

- **JSON Annotations**: 
  Models are annotated with `JsonProperty` attributes to ensure accurate matching of JSON request/response parameters and improved deserialization.

- **Modular Architecture**:
  The solution follows a clean, modular design, organizing code into reusable components such as:
  - **Models**: Represents the data structures used across the system.
  - **Services**: Contains the business logic and integrates with external systems.
  - **Controllers**: Manages the API endpoints and handles HTTP requests.

## Project Structure

```bash
KycAggregator/
│
├── Controllers/      # Contains the API controllers
│   └── KycAggregatorController.cs
│
├── Models/           # Data models used in the solution
│   └── AggKycData.cs
│   └── ContactDetails.cs
│   └── KYCForm.cs
│   └── PersonalDetails.cs
│
├── Services/         # Cache implementation (FileCache)
│   └── FilCache.cs
│
├── Program.cs        # Entry point of the application and dependency registration
├── appsettings.json  # Application settings and configuration
└── README.md         # Project documentation
```

## Unit Tests for HTTP Requests

We have implemented two unit tests to validate the behavior of the `GetCustomerPersonalDetailsResponse` method, which makes an HTTP request to an external API.

1. **Successful API Call:**
   - Test: `GetCustomerPersonalDetailsResponse_ShouldReturnSuccess_WhenApiCallIsSuccessful`
   - Description: Simulates a successful API response (HTTP 200) and verifies that the method returns a successful status code (`IsSuccessStatusCode`).

2. **Failed API Call:**
   - Test: `GetCustomerPersonalDetailsResponse_ShouldReturnFailure_WhenApiCallFails`
   - Description: Simulates a failed API response (HTTP 400) and verifies that the method returns a failure status code (`IsSuccessStatusCode` is `false`).

By using mocked HTTP responses, these tests isolate the controller logic from the actual API, making them reliable and independent of external factors.

## Future scope

Redis Integration: Replace FileCache with Redis for better scalability and distributed caching.
Authentication & Authorization: Add OAuth2/JWT-based authentication to secure the API endpoints.
Advanced Logging: Integrate a logging framework (e.g., Serilog) for better log management.
Unit Testing: We can probably add comprehensive unit tests using xUnit or NUnit.
Third-Party Integration: Expand KYC validation with additional third-party services.
Rate Limiting: Implement rate limiting to protect the API from abuse.