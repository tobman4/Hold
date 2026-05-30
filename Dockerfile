# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY src/Hold.sln ./
COPY src/Hold.API/Hold.API.csproj ./Hold.API/

# Restore dependencies
RUN dotnet restore Hold.API/Hold.API.csproj

# Copy the rest of the source code
COPY src/Hold.API/ ./Hold.API/

# Build and publish
RUN dotnet publish Hold.API/Hold.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Standard ASP.NET Core ports
EXPOSE 8080
EXPOSE 8081

# Copy the published output from the build stage
COPY --from=build /app/publish .

# The SQLite database (hold.db) will be created in /app by default.
# Consider mounting a volume to /app/hold.db for persistence.

ENTRYPOINT ["dotnet", "Hold.API.dll"]
