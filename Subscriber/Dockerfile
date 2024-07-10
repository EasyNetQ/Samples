FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.sln .
COPY Messages/Messages.csproj ./Messages/
COPY Subscriber/Subscriber.csproj ./Subscriber/
RUN dotnet restore Subscriber/Subscriber.csproj

COPY . .
WORKDIR /source/Subscriber
RUN dotnet publish Subscriber.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Subscriber.dll"]
