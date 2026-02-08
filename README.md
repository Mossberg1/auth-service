# Authentication Service

This is a small authentication microservice that uses JWT tokens. 

## Requirements
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) 
- PostgreSQL database
- Docker (optional)

## Setup

Edit `Auth/appsettings.json` with your connection string and the correct settings for your JWT tokens.
```json
"ConnectionStrings": {
    "Default": "DefaultConnectionString"
},
"JwtSettings": {
    "SecretKey": "SecretKey",   
    "Issuer": "Issuer",
    "Audience": "Audience"
}
```
After this you have to apply the migrations to the database. This assumes that you already have a
postgres database runnnig. If you dont have one, there is one you could run inside `docker-compose.yaml`. 
```bash
dotnet ef database update --project ./Auth/Auth/Auth.csproj
```

## Build docker image
```bash
cd Auth
docker build -t counter-image -f Auth/Dockerfile .
```

## Compile
```bash
dotnet build ./Auth/Auth/Auth.csproj
```

## Release build
```bash
dotnet publish Auth/Auth/Auth.csproj -c Release -o ./publish
```

## Run locally
```bash
dotnet run ./Auth/Auth/Auth.csproj
```

## License
Open source (MIT)