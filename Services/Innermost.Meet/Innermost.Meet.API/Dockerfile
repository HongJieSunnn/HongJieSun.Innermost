#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Innermost.Meet/Innermost.Meet.API/Innermost.Meet.API.csproj", "Services/Innermost.Meet/Innermost.Meet.API/"]
COPY ["BuildingBlocks/CommonService/CommonService/CommonService.csproj", "BuildingBlocks/CommonService/CommonService/"]
COPY ["BuildingBlocks/Extensions/MongoDBExtensions/MongoDBExtensions.csproj", "BuildingBlocks/Extensions/MongoDBExtensions/"]
COPY ["BuildingBlocks/EventBus/EventBusCommon/EventBusCommon.csproj", "BuildingBlocks/EventBus/EventBusCommon/"]
COPY ["../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext/Innermost.MongoDBContext.csproj", "../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext/"]
COPY ["BuildingBlocks/Extensions/TypeExtensions/TypeExtensions.csproj", "BuildingBlocks/Extensions/TypeExtensions/"]
COPY ["Services/Innermost.Meet/Innermost.Meet.Domain/Innermost.Meet.Domain.csproj", "Services/Innermost.Meet/Innermost.Meet.Domain/"]
COPY ["BuildingBlocks/SeedWork/DomainSeedWork/DomainSeedWork.csproj", "BuildingBlocks/SeedWork/DomainSeedWork/"]
COPY ["../../CSharpSource/TagS/TagS.Microservices.Client/TagS.Microservices.Client.csproj", "../../CSharpSource/TagS/TagS.Microservices.Client/"]
COPY ["BuildingBlocks/EventBus/IntegrationEventServiceMongoDB/IntegrationEventServiceMongoDB.csproj", "BuildingBlocks/EventBus/IntegrationEventServiceMongoDB/"]
COPY ["BuildingBlocks/Extensions/ILoggerExtensions/ILoggerExtensions.csproj", "BuildingBlocks/Extensions/ILoggerExtensions/"]
COPY ["BuildingBlocks/CommonService/CommonIdentityService/CommonIdentityService.csproj", "BuildingBlocks/CommonService/CommonIdentityService/"]
COPY ["../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection/Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection.csproj", "../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection/"]
COPY ["../../CSharpSource/Innermost.IdempotentCommand/Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection/Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection.csproj", "../../CSharpSource/Innermost.IdempotentCommand/Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection/"]
COPY ["../../CSharpSource/Innermost.IdempotentCommand/Innermost.IdempotentCommand/Innermost.IdempotentCommand.csproj", "../../CSharpSource/Innermost.IdempotentCommand/Innermost.IdempotentCommand/"]
COPY ["BuildingBlocks/Extensions/IServiceCollectionExtensions/IServiceCollectionExtensions.csproj", "BuildingBlocks/Extensions/IServiceCollectionExtensions/"]
COPY ["Services/Innermost.Meet/Innermost.Meet.Infrastructure/Innermost.Meet.Infrastructure.csproj", "Services/Innermost.Meet/Innermost.Meet.Infrastructure/"]
COPY ["BuildingBlocks/Extensions/IEnumerableExtensions/IEnumerableExtensions.csproj", "BuildingBlocks/Extensions/IEnumerableExtensions/"]
COPY ["BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj", "BuildingBlocks/EventBus/EventBusServiceBus/"]
RUN dotnet restore "Services/Innermost.Meet/Innermost.Meet.API/Innermost.Meet.API.csproj"
COPY . .
WORKDIR "/src/Services/Innermost.Meet/Innermost.Meet.API"
RUN dotnet build "Innermost.Meet.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innermost.Meet.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innermost.Meet.API.dll"]
