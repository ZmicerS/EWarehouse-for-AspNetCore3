# EWarehouse-for-AspNetCore3
This application is an entry-level skeleton for organizing the work of an e-warehouse of books. 
This is a Single Page Application.

Framework Angular version 8 is used, to implement the client.
Used by Lazy loading modules.
Several HttpInterceptors are used.
ErrorHandler is used.
Implemented several unit tests for Angular.

The server side is implemented by ASP.NET Core v3.0.
The server side uses a multi-layer architecture.
MSSql(localDb) is used as the database.
To work with the database, the Entity Framework Core Code First is used.
For CRUD operations UnitOfWork pattern is implemented.
Use the Fluent API to create tables.
For authorization, a JWT token is used.
Used by AutoMapper, Logger.
There are several unit tests with xUnit.
Publish
dotnet publish -r win-x64 -c Release.
Run Kestrel Web Server.
dotnet EWarehouse.Web.dll --server.urls "http://localhost:5101;http://*:5102"
