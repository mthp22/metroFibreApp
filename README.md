# metroFibreAssessment
## Technology Stack
- .NET Core 9.0  
- MongoDB  
- Angular  

## Port Configuration
### Backend
The backend service runs on port `5098` by default.

### Frontend
The frontend application runs on port `4200` by default.

### Database
MongoDB runs on port `27017` by default.

## Docker Setup
### Automated Install
To run the application using Docker, execute the following commands:
```sh
docker-compose up -d 
This will build and start the services.
```

### Running Locally

**Backend**
Ensure you have .NET Core 9.0 installed.
Run the application using:

```sh
cd backend
dotnet run

```
**Frontend**
```sh
cd frontend 
npm install
ng serve

```
### Configuration Files
1. appsettings.Development.json: Development-specific settings for the backend.

1. appsettings.json: General settings for both environments.

### Database Initialization
The database initialization script is located in db/init/01-init-food-db.js.
This script will be executed when starting the Docker containers.