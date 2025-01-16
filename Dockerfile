# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AstroArchitecture.Api/AstroArchitecture.Api.csproj", "src/AstroArchitecture.Api/"]
COPY ["src/AstroArchitecture.Handlers/AstroArchitecture.Handlers.csproj", "src/AstroArchitecture.Handlers/"]
COPY ["src/AstroArchitecture.Infrastructure/AstroArchitecture.Infrastructure.csproj", "src/AstroArchitecture.Infrastructure/"]
COPY ["src/AstroArchitecture.Core/AstroArchitecture.Core.csproj", "src/AstroArchitecture.Core/"]
COPY ["src/AstroArchitecture.Domain/AstroArchitecture.Domain.csproj", "src/AstroArchitecture.Domain/"]
RUN dotnet restore "./src/AstroArchitecture.Api/AstroArchitecture.Api.csproj"
COPY . .
WORKDIR "/src/src/AstroArchitecture.Api"
RUN dotnet build "./AstroArchitecture.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AstroArchitecture.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AstroArchitecture.Api.dll"]mcr.microsoft.com/dotnet/aspnet:8.0mcr.microsoft.com/dotnet/sdk:8.0
