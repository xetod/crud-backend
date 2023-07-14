# CRUDIO!

CRUDIO is an application developed using .NET Core 6 and react. It provides a simple and efficient way to perform basic CRUD operations on a database.

## Features

- Strived to write clean code by adhering to coding standards and principles.
- Prioritized maintainability and testability when designing and developing the codebase.
- Implemented Domain-Driven Design (DDD) principles, incorporating design patterns such as factory, null object, and specification.
- Employed built-in validation mechanisms to guarantee data integrity.
- Utilized a layered architecture for improved modularity and scalability.
- Conducted thorough unit and integration tests for comprehensive code coverage.

## Clone repositories
The code for the back end and front end are stored in separate repositories, which can be cloned by following these steps:

- Back-end:
    ```sh
    git clone https://github.com/xetod/crud-backend.git
    ```
- Front-end:
    ```sh
    git clone https://github.com/xetod/crud-frontend.git
    ```

## Run the application
To make it easier to run the application, I've pushed the images to Docker Hub. With docker-compose, accessing the backend and frontend containers is a breeze. Just go to the folder where you cloned the backend code and run this command:

```sh
docker-compose up
```

To access the application, just navigate to the following address:
```sh
http://localhost:7070
```
If you want to view the endpoints, simply visit the following address:
```sh
http://localhost:6060/swagger/index.html
```
##Dependencies
The backend of CRUDIO relies on the following dependencies:

- .NET Core 6
- Entity Framework Core
- ASP.NET Web API
- XUnit
- Automapper

The frontend of CRUDIO may have its own specific dependencies specified in the package.json file located in the frontend repository. To install these dependencies, navigate to the frontend directory and run the command "npm install".
