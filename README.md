# Hotel Listing API
This repository contains the source code for an educational Web API built using .NET Core 7. The project covers REST principles, connecting to a database, using Swagger, and developing custom middleware to bring out the full feature set of a .NET API.

## Table of Contents

- [Technologies](#technologies)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Code Samples](#code-samples)
- [Documentation](#documentation)
- [Contributing](#contributing)
- [License](#license)

## Technologies

The project uses the following technologies:

- .NET Core 7
- REST principles
- Swagger
- Entity Framework Core
- SQL Server
- Serilog

## Getting Started

To get started with the project, follow these steps:

1. Clone the repository to your local machine.
2. Install the .NET Core 7 SDK and SQL Server on your machine.
3. Update the connection string in the `appsettings.json` file with your SQL Server connection string.
4. Open the solution file in Visual Studio or another IDE of your choice.
5. Restore the NuGet packages.
6. Set the startup project to `HotelListing.API`.
7. Run the project.

## Project Structure

The solution is composed of the following projects:

- `Core`: Contains the application logic and interfaces.
- `Data`: Contains the domain models and business rules.
- `API`: Contains the Web API controllers and middleware.

## Code Samples

The project contains code samples for several areas of .NET Core development, including:

- REST principles
- Swagger
- Entity Framework Core
- SQL Server
- Custom middleware

Each code sample is well-documented, and the code is designed to be easy to read and follow.

## Documentation

The repository contains documentation for each of the code samples, including:

- An overview of the sample and its intended use case
- Installation and configuration instructions
- Code walkthroughs and explanations
- Best practices and tips for working with the sample

The documentation is designed to be accessible to developers of all skill levels, from beginners to advanced users.

## Contributing

Contributions to this repository are welcome. If you have a code sample or educational resource that you would like to contribute, please follow these steps:

1. Fork the repository.
2. Create a new branch for your changes.
3. Make your changes and commit them to your branch.
4. Submit a pull request.

Please ensure that your contributions adhere to the repository's code of conduct and that they are well-documented and follow best practices.

## License

This repository is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.

## Migrations

Add-Migration InitialCreate -Context HotelListingDbContext -OutputDir Data/Migrations
Remove-Migration -Context HotelListingDbContext
Update-Database -Context HotelListingDbContext
