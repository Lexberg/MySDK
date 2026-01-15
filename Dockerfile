# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["src/MySDK/MySDK.csproj", "src/MySDK/"]
RUN dotnet restore "src/MySDK/MySDK.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/MySDK"
RUN dotnet build "MySDK.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "MySDK.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MySDK.dll"]
