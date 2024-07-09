# Using the .NET SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy solution and project files and restore as distinct layers
COPY *.sln .
COPY Messages/Messages.csproj ./Messages/
COPY Publisher/Publisher.csproj ./Publisher/
RUN dotnet restore Publisher/Publisher.csproj

# Copy everything else and build
COPY . .
WORKDIR /source/Publisher
RUN dotnet publish Publisher.csproj -c Release -o /app

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Publisher.dll"]
