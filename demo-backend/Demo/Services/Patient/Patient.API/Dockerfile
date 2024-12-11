#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Patient/Patient.API/Patient.API.csproj", "Services/Patient/Patient.API/"]
RUN dotnet restore "Services/Patient/Patient.API/Patient.API.csproj"
COPY . .
WORKDIR "/src/Services/Patient/Patient.API"
RUN dotnet build "Patient.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Patient.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Patient.API.dll"]