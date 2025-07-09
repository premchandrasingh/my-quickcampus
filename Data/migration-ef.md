
## Install the EF Core tools globally if not already installed:
   ```bash
   dotnet tool install --global dotnet-ef
   ```

## Check EF Core tools version:

   ```bash
   dotnet ef --version
   ```


## Update EF Core tools globally (if installed already):
   
   ```bash
   dotnet tool update --global dotnet-ef
   ```


## Add the necessary EF Core packages to your project:

`Microsoft.EntityFrameworkCore.Design` is required to add migrations and update the database.

   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.Sqlite
   dotnet add package Microsoft.EntityFrameworkCore.Design
   ```

## Add new migration:

   ```bash
   dotnet ef migrations add Initial -c SqliteDbContext
   ```

## Apply the migration to the database:
   
   ```bash
   dotnet ef database update
   ```
## Remove migration:   
   ```bash
   dotnet ef migrations remove
   ```


Doc- https://learn.microsoft.com/en-us/ef/core/cli/dotnet