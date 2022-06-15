#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Innermost.Push/Innermost.Push.API/Innermost.Push.API.csproj", "Services/Innermost.Push/Innermost.Push.API/"]
COPY ["BuildingBlocks/EventBus/EventBusCommon/EventBusCommon.csproj", "BuildingBlocks/EventBus/EventBusCommon/"]
COPY ["BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj", "BuildingBlocks/EventBus/EventBusServiceBus/"]
RUN dotnet restore "Services/Innermost.Push/Innermost.Push.API/Innermost.Push.API.csproj"
COPY . .
WORKDIR "/src/Services/Innermost.Push/Innermost.Push.API"
RUN dotnet build "Innermost.Push.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innermost.Push.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innermost.Push.API.dll"]
