# Kyc Aggregation Service

This service aggregates Know Your Customer (KYC) data from external APIs and provides a unified endpoint for retrieving this information.

## Features

* **Efficient Error Handling:** Robust error handling to gracefully manage API failures, invalid requests, and other exceptions.
* **Persistent Caching:** Implements persistent caching using `FileCache` to reduce redundant API calls and improve performance. Can be configured to use Redis for distributed caching.
* **Accurate JSON Parameter Matching:** Utilizes JSON annotations in the model classes to ensure precise matching of JSON parameters during deserialization.
* **Modular Design:** Organized into modular components (models, services, controllers) for improved maintainability and scalability.

## Architecture

The service follows a modular architecture:

* **Controllers:** Handle incoming HTTP requests and orchestrate the data retrieval and aggregation process.
* **Services:** Implement reusable business logic, such as the `FileCache` service for persistent caching.
* **Models:** Define the data structures used by the service, ensuring accurate JSON serialization and deserialization.

## Caching

The service currently uses `FileCache` for persistent caching. This approach stores cached data in files on the file system.

**Potential Enhancement:**

* The service can be easily configured to use Redis for distributed caching, which would be beneficial for distributed environments.

## Error Handling

The service implements comprehensive error handling, including:

* Handling specific HTTP status codes (400 Bad Request, 404 Not Found, 500 Internal Server Error).
* Logging errors for debugging and monitoring.
* Returning informative error messages to the client.

## JSON Parameter Matching

JSON annotations (`JsonPropertyName`) are used in the model classes to ensure accurate matching of JSON parameters, addressing potential case-sensitivity issues.

## Getting Started

1.  **Clone the Repository:**
    ```bash
    git clone https://github.com/ashleyjdcosta/KycAggregatorService.git
    ```
2.  **Build the Project:**
    ```bash
    dotnet build
    ```
3.  **Run the Application:**
    ```bash
    dotnet run
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

## Future Enhancements

* Implement Redis caching for distributed environments.
* Add cache expiration and invalidation mechanisms.
* Add more robust logging.
* Implement more detailed unit tests.