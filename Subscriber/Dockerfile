# Using the .NET SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy solution and project files and restore as distinct layers
COPY *.sln .
COPY Messages/Messages.csproj ./Messages/
COPY Subscriber/Subscriber.csproj ./Subscriber/
RUN dotnet restore Subscriber/Subscriber.csproj

# Copy everything else and build
COPY . .
WORKDIR /source/Subscriber
RUN dotnet publish Subscriber.csproj -c Release -o /app

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Subscriber.dll"]
