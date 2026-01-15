# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/MySDK/MySDK.csproj", "src/MySDK/"]
COPY ["src/MySDK.Api/MySDK.Api.csproj", "src/MySDK.Api/"]
RUN dotnet restore "src/MySDK.Api/MySDK.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/MySDK.Api"
RUN dotnet build "MySDK.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "MySDK.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MySDK.Api.dll"]
