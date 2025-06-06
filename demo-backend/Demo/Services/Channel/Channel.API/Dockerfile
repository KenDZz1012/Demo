# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Channel/Channel.API/Channel.API.csproj", "Services/Channel/Channel.API/"]
COPY ["Library/Service.Lib/Service.Lib.csproj", "Library/Service.Lib/"]
COPY ["Services/Channel/Channel.Infrastructure/Channel.Infrastructure.csproj", "Services/Channel/Channel.Infrastructure/"]
COPY ["Services/Channel/Channel.Application/Channel.Application.csproj", "Services/Channel/Channel.Application/"]
COPY ["Services/Channel/Channel.Domain/Channel.Domain.csproj", "Services/Channel/Channel.Domain/"]
COPY ["Services/Channel/Channel.Shared/Channel.Shared.csproj", "Services/Channel/Channel.Shared/"]
RUN dotnet restore "./Services/Channel/Channel.API/Channel.API.csproj"
COPY . .
WORKDIR "/src/Services/Channel/Channel.API"
RUN dotnet build "./Channel.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Channel.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Channel.API.dll"]