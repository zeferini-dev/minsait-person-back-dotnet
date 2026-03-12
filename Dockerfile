# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the entire project
COPY . .

# Restore packages from the csproj location
WORKDIR /src/src/minsait-person-back-dotnet
RUN dotnet restore "minsait-person-back-dotnet.csproj"

# Build the application
RUN dotnet build "minsait-person-back-dotnet.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
WORKDIR /src/src/minsait-person-back-dotnet
RUN dotnet publish "minsait-person-back-dotnet.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "minsait-person-back-dotnet.dll"]
