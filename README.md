# PexitaMVC - Bill Splitting App

PexitaMVC is a web application built with ASP.NET Core MVC that simplifies the process of splitting bills among friends or groups. Unlike traditional bill-splitting apps that divide costs equally, PexitaMVC allows users to specify how much each person has to pay, accommodating situations where contributions are unequal. 

This project is designed as a learning platform for T-SQL, Clean Architecture, SOLID principles, testing and other essential skills.


## Features
- **Flexible Bill Splitting**: Specify individual payments instead of splitting the bill equally.
- **Authentication & Authorization**: Built with Microsoft Identity for secure user management.
- **Clean Architecture**: Separation of concerns using Core, Infrastructure, and Application layers.
- **Database Integration**: MS SQL Server with Docker deployment.
- **Unit Testing**: Comprehensive testing using xUnit.



## Tech Stack
- **Framework**: ASP.NET Core MVC
- **Database**: MS SQL Server (deployed via Docker)
- **Authentication**: ASP.NET Core Identity
- **Language**: C#, T-SQL
- **Testing**: xUnit
- **Other Tools**: Docker for database containerization

## Prerequisites
- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- SQL Server Management Studio (optional, for database inspection)

## Architecture Overview
PexitaMVC follows Clean Architecture principles:

- **Core**: Contains domain entities and business logic.
- **Application**: Manages application-level rules and interfaces.
- **Infrastructure**: Handles database interactions and external dependencies.
- **Presentation**: The MVC layer with Controllers, Views, and Models.

## License
**This project is licensed under the GPLv3 License. See [LICENSE](LICENSE.txt) for details.**
