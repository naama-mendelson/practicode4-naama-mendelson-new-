# Stage 1: Build stage - Using .NET 8.0 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# מעתיקים את ה-csproj לתיקיית העבודה של Docker
COPY TodoApi.csproj .
RUN dotnet restore
# מעתיקים את כל שאר הקבצים (Controllers, Models, Program.cs, וכו')
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Final stage (Runtime) - Using .NET 8.0 Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# Define the entry point for the application
ENTRYPOINT ["dotnet", "TodoApi.dll"]