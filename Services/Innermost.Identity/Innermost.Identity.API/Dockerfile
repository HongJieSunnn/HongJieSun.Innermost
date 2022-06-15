#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Innermost.Identity/Innermost.Identity.API/Innermost.Identity.API.csproj", "Services/Innermost.Identity/Innermost.Identity.API/"]
COPY ["BuildingBlocks/CommonService/CommonService/CommonService.csproj", "BuildingBlocks/CommonService/CommonService/"]
COPY ["BuildingBlocks/Extensions/MongoDBExtensions/MongoDBExtensions.csproj", "BuildingBlocks/Extensions/MongoDBExtensions/"]
COPY ["BuildingBlocks/EventBus/EventBusCommon/EventBusCommon.csproj", "BuildingBlocks/EventBus/EventBusCommon/"]
COPY ["../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext/Innermost.MongoDBContext.csproj", "../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext/"]
COPY ["BuildingBlocks/Extensions/TypeExtensions/TypeExtensions.csproj", "BuildingBlocks/Extensions/TypeExtensions/"]
COPY ["BuildingBlocks/WebHost/WebHost.Extensions/WebHost.CustomExtensions.csproj", "BuildingBlocks/WebHost/WebHost.Extensions/"]
COPY ["BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj", "BuildingBlocks/EventBus/EventBusServiceBus/"]
COPY ["BuildingBlocks/EventBus/IntegrationEventServiceSQL/IntegrationEventServiceSQL.csproj", "BuildingBlocks/EventBus/IntegrationEventServiceSQL/"]
COPY ["BuildingBlocks/Extensions/ILoggerExtensions/ILoggerExtensions.csproj", "BuildingBlocks/Extensions/ILoggerExtensions/"]

RUN dotnet restore "Services/Innermost.Identity/Innermost.Identity.API/Innermost.Identity.API.csproj"
COPY . .
WORKDIR "/src/Services/Innermost.Identity/Innermost.Identity.API"
RUN dotnet build "Innermost.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innermost.Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innermost.Identity.API.dll"]
