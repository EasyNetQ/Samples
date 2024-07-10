FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.sln .
COPY Messages/Messages.csproj ./Messages/
COPY Publisher/Publisher.csproj ./Publisher/
RUN dotnet restore Publisher/Publisher.csproj

COPY . .
WORKDIR /source/Publisher
RUN dotnet publish Publisher.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Publisher.dll"]
