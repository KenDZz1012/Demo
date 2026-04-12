FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Guild/Guild.API/Guild.API.csproj", "Services/Guild/Guild.API/"]
COPY ["Services/Guild/Guild.Infrastructure/Guild.Infrastructure.csproj", "Services/Guild/Guild.Infrastructure/"]
COPY ["Services/Guild/Guild.Domain/Guild.Domain.csproj", "Services/Guild/Guild.Domain/"]
COPY ["Services/Guild/Guild.Application/Guild.Application.csproj", "Services/Guild/Guild.Application/"]
COPY ["Library/Service.Lib/Service.Lib.csproj", "Library/Service.Lib/"]
RUN dotnet restore "Services/Guild/Guild.API/Guild.API.csproj"
COPY . .
WORKDIR "/src/Services/Guild/Guild.API"
RUN dotnet build "./Guild.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Guild.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Guild.API.dll"]
