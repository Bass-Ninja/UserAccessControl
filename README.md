# User Access Control API

## Overview

This project is a backend REST API for managing users, resources, and access grants with different access levels (Read, Write, Admin).  
It is built with:

- .NET 8 (C#)
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL 15 (via Docker or local installation)
- xUnit for unit tests
- Clean Architecture principles

---

## Features

- Add, fetch, list, and delete users
- Add, fetch, list, and delete resources
- Assign access grants to users for specific resources with access levels
- Fetch all resources a user has access to
- API documentation via Swagger/OpenAPI
- Optional JWT-based authentication simulation (admin user)

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker & Docker Compose](https://www.docker.com/get-started) (if you want to run PostgreSQL in Docker)
- PostgreSQL 15 (if running locally instead of Docker)
- IDE or code editor (Visual Studio, VS Code, JetBrains Rider, etc.)

---

## Setup Instructions

### 1. Clone the repository

```bash
git clone https://github.com/Bass-Ninja/UserAccessControl.git
cd UserAccessControl
```

### 2. Configure PostgreSQL

#### Option A: Run PostgreSQL in Docker (recommended)
Make sure Docker is running. Then, from the root folder of the project (where the docker-compose.yml file is located), run:

```bash
docker-compose up -d
```
This will start a PostgreSQL container with default credentials configured in docker-compose.yml.

---
#### Option B: Use a local PostgreSQL installation
Make sure PostgreSQL is installed and running locally.

Create a database named UAC_DB.

For development purposes, appsettings.json is included in the repository. Changing the connection string is necessary if credentials do not match the default ones.

### 3. Start the application with Docker Compose

The provided `docker-compose.yml` starts both PostgreSQL and the API application together.

Simply run:

```bash
docker-compose up -d
```
The API container is configured to automatically apply EF Core migrations on startup, so the database schema will be created/updated automatically.

### 4. Verify the API is running
   Once the containers are up, navigate to:


```bash
https://localhost:5000/swagger/index.html
```
to view the Swagger UI and test the API endpoints.

---

### Authentication and Authorization

1. Obtain a JWT token by calling:

```bash
https://localhost:5001/api/auth/login
```
![img.png](img.png)

This returns a token for the fixed user `admin@ninja.si` with Admin role.

2. In Swagger UI, click **Authorize**, paste the token, and confirm.

![img_1.png](img_1.png)

3. Now you can call secured endpoints like:

![img_2.png](img_2.png)

Note: Only User endpoints cannot be called anonymously.


### Running Tests
To run the unit tests, navigate to the test project directory and run:

```bash
dotnet test
```
This will execute all xUnit tests and report the results.

### Additional Notes
If you want to run the API without Docker, make sure to have PostgreSQL running locally, update the connection string in appsettings.json accordingly, then run the API with:

```bash
dotnet run --project UserAccessControl.API
```
When running locally, migrations are applied automatically on API startup as well.

To stop Docker containers:

```bash
docker-compose down
```

---
### Troubleshooting
If you get errors about ports already in use, check if other instances of PostgreSQL or the API are running and stop them.

Ensure Docker Desktop is running properly before starting containers.

For any database-related errors, check the connection string and credentials in appsettings.json.
