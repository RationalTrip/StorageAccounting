# StorageAccounting


## Technologies


.Net 6, EntityFramework Core 7, MS SQL Server


## Overview


Simple API, for managing available space in Rooms for renting.
We have 3 entities:


1. RentingRoom - saves information about room, their name and available space (area)


2. Equipment - saves information about equipment, that can be stored at room, contain name, size (required area for 1 equipment)


3. RentingContract - saves information about which equipment, count of the equipment and in which room it stores


## Endpoints


- `/api/Equipment` - endpoints for Equipment
    - `get /` - get all equipment, with optional query parameters `start`, `size`
    - `get /count` - get all equipment count
    - `get /{id:int}` - get Equipment by id
    - `post /` - create Equipment
    - `get /{id:int}/contracts` - get all contracts for current equipment, with optional query parameters `start`, `size`
    - `get /{id:int}/contracts/count` - get count of all contracts for current equipment




- `/api/RentingContract` - endpoints for RentingContract
    - `get /` - get all contracts, with optional query parameters `start`, `size`
    - `get /count` - get all contracts count
    - `post /` - create contract
    - `get /{id:int}` - get contract by id
    - `delete /{id:int} `- delete contract by id




- `/api/StorageRoom` - endpoints for StorageRoom
    - `get /` - get all rooms, with optional query parameters `start`, `size`
    - `get /count` - get all rooms count
    - `post /` - create room
    - `get /{id:int}` - get room by id
    - `get /{id:int}/contracts` - get all contracts stored in current room, with optional query parameters `start`, `size`
    - `get /{id:int}/contracts/count` - get count of all contracts stored in current room
    - `get /{id:int}/area` - get information about occupied area


# Execution


For running application in Development mode, you should:


1. in file `./src/StorageAccounting.WebAPI/appsettings.Development.json` specify Sql Server connection string with the key `ConnectionStrings:StorageAccountingSqlServer`
2. open project manager console, select project `./src/StorageAccounting.Database` and apply migration by command
```bash
$update-database
```


(Optional.) You can change the api key, in file `./src/StorageAccounting.WebAPI/appsettings.Development.json` with the key `ApiKey`


# Project Structure


- StorageAccounting.Domain - Entities, common classes and exceptions
- StorageAccounting.Application - Models, Dtos, Mapping, Service and Repository Interfaces
- StorageAccounting.Database - Context, Migrations, Repository implementation
- StorageAccounting.Infrastructure - Services implementation
- StorageAccounting.WebAPI - Controllers, Authentication Handler, Composition root, Entry point