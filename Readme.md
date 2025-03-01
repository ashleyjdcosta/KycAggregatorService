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
    git clone [repository_url]
    ```
2.  **Build the Project:**
    ```bash
    dotnet build
    ```
3.  **Run the Application:**
    ```bash
    dotnet run
    ```


## Future Enhancements

* Implement Redis caching for distributed environments.
* Add cache expiration and invalidation mechanisms.
* Add more robust logging.
* Implement more detailed unit tests.