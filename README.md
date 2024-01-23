# Hotel Booking API Using C# and ASP.NET Core
This is a simple hotel booking API using C# and ASP.NET Core. It is a RESTful API that allows you to create, read, update and delete hotel bookings.

### Prerequisites

1. Install .NET SDK (8.0.101) and Runtime (8.0.1)

    ```sh
    sudo pacman -S dotnet-sdk
    sudo pacman -S dotnet-runtime
    dotnet tool install --global dotnet-ef
    ```
    
## Getting Started
To get a local copy up and running follow these simple steps.

1. Clone the repository
   ```sh
   git clone https://github.com/PublisherName/HotelBooking_API.git
    ```

2. cd into the project directory
   ```sh
   cd HotelBookingAPI
   ```

3. Update the appsettings.json file with your database connection string. Dummy connection string is provided inside the appsettings.json.back file.
   ```sh
    {
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultConnection": "server=<mysql_server>;database=<data_basename>;user=<username>;password=<password>"
    },
    "SecretKey": "<secret_key>"
    }
   ```

4. Install NuGet packages
   ```sh
   dotnet restore
   ```

5. Build the project
   ```sh
   dotnet build
   ```

6. Run the project
   ```sh
    dotnet run
    ```

7. Open the project in your browser
    ```sh
    https://localhost:5293/swagger/index.html
    ```

## Migration
1. Add migration
    ```sh
    dotnet ef migrations add InitialCreate
    ```
2. Update database
    ```sh
    dotnet ef database update
    ```