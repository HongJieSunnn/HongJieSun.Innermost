#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Innermost.MusicHub/Innermost.MusicHub.API/Innermost.MusicHub.API.csproj", "Services/Innermost.MusicHub/Innermost.MusicHub.API/"]
COPY ["BuildingBlocks/CommonService/CommonService/CommonService.csproj", "BuildingBlocks/CommonService/CommonService/"]
COPY ["BuildingBlocks/Extensions/MongoDBExtensions/MongoDBExtensions.csproj", "BuildingBlocks/Extensions/MongoDBExtensions/"]
COPY ["BuildingBlocks/EventBus/EventBusCommon/EventBusCommon.csproj", "BuildingBlocks/EventBus/EventBusCommon/"]
COPY ["../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext/Innermost.MongoDBContext.csproj", "../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext/"]
COPY ["BuildingBlocks/Extensions/TypeExtensions/TypeExtensions.csproj", "BuildingBlocks/Extensions/TypeExtensions/"]
COPY ["BuildingBlocks/EventBus/IntegrationEventServiceMongoDB/IntegrationEventServiceMongoDB.csproj", "BuildingBlocks/EventBus/IntegrationEventServiceMongoDB/"]
COPY ["BuildingBlocks/Extensions/ILoggerExtensions/ILoggerExtensions.csproj", "BuildingBlocks/Extensions/ILoggerExtensions/"]
COPY ["../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection/Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection.csproj", "../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection/"]
COPY ["Services/Innermost.MusicHub/Innermost.MusicHub.Infrastructure/Innermost.MusicHub.Infrastructure.csproj", "Services/Innermost.MusicHub/Innermost.MusicHub.Infrastructure/"]
COPY ["Services/Innermost.MusicHub/Innermost.MusicHub.Domain/Innermost.MusicHub.Domain.csproj", "Services/Innermost.MusicHub/Innermost.MusicHub.Domain/"]
COPY ["BuildingBlocks/SeedWork/DomainSeedWork/DomainSeedWork.csproj", "BuildingBlocks/SeedWork/DomainSeedWork/"]
COPY ["../../CSharpSource/TagS/TagS.Microservices.Client/TagS.Microservices.Client.csproj", "../../CSharpSource/TagS/TagS.Microservices.Client/"]
RUN dotnet restore "Services/Innermost.MusicHub/Innermost.MusicHub.API/Innermost.MusicHub.API.csproj"
COPY . .
WORKDIR "/src/Services/Innermost.MusicHub/Innermost.MusicHub.API"
RUN dotnet build "Innermost.MusicHub.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innermost.MusicHub.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innermost.MusicHub.API.dll"]
