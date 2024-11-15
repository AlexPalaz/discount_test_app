# DiscountApp

DiscountApp is an ASP.NET Core application that uses SignalR for real-time discount code management. This application allows users to generate and redeem discount codes through a SignalR-based user interface.

## Prerequisites

Before running the project, ensure you have installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or higher)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/) (should be auto-configured during the nuget packages restoring)
- [SQLite](https://www.sqlite.org/download.html) (should be auto-configured when you setup the database via EF)

## Setting Up the Local Database

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/DiscountApp.git
   cd DiscountApp
   ```
2. To run the project you have to:
   - Open the project with Visual Studio
   - Waiting for the Nuget Package Restore
   - Run the migration

   ```bash
   dotnet ef database update
   ```
   - Run the project

   Alternatively, you can go into the /DiscountApp folder and run the following commands:
   
    ```bash
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

## Project Structure

- **Contexts**: Contains the `DiscountContext` class, which configures the database context and tables.
- **Hubs**: Contains the `DiscountHub` class, which handles real-time SignalR connections for generating and using discount codes.
- **Migrations**: Contains database migration files for Entity Framework.
- **Models**: Defines the `DiscountCode` model used in the application.
- **Pages**: Contains Razor pages for the front-end of the application.
- **Services**: Contains the `DiscountService`, which provides the business logic for managing discount codes.

## Usage

- **Generate Codes**: Allows you to generate a specified number of discount codes with a length of 7 or 8 characters.
- **Use Code**: Allows you to redeem a discount code, marking it as used in the database.

## Notes

- The project uses SQLite for simplicity in local development. For production use, consider configuring another database provider.

   
