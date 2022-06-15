#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Innermost.TagServer/Innermost.TagServer.API/Innermost.TagServer.API.csproj", "Services/Innermost.TagServer/Innermost.TagServer.API/"]
COPY ["Services/Innermost.TagServer/Innermost.TagReferrers/Innermost.TagReferrers.csproj", "Services/Innermost.TagServer/Innermost.TagReferrers/"]
COPY ["../../CSharpSource/TagS/TagS.Microservices.Server.Microsoft.DependencyInjection/TagS.Microservices.Server.Microsoft.DependencyInjection.csproj", "../../CSharpSource/TagS/TagS.Microservices.Server.Microsoft.DependencyInjection/"]
COPY ["../../CSharpSource/TagS/TagS.Microservices.Server/TagS.Microservices.Server.csproj", "../../CSharpSource/TagS/TagS.Microservices.Server/"]
COPY ["../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext/Innermost.MongoDBContext.csproj", "../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext/"]
COPY ["BuildingBlocks/CommonService/CommonService/CommonService.csproj", "BuildingBlocks/CommonService/CommonService/"]
COPY ["BuildingBlocks/Extensions/MongoDBExtensions/MongoDBExtensions.csproj", "BuildingBlocks/Extensions/MongoDBExtensions/"]
COPY ["BuildingBlocks/EventBus/EventBusCommon/EventBusCommon.csproj", "BuildingBlocks/EventBus/EventBusCommon/"]
COPY ["BuildingBlocks/Extensions/TypeExtensions/TypeExtensions.csproj", "BuildingBlocks/Extensions/TypeExtensions/"]
COPY ["../../CSharpSource/TagS/TagS.Microservices.Client/TagS.Microservices.Client.csproj", "../../CSharpSource/TagS/TagS.Microservices.Client/"]
COPY ["BuildingBlocks/SeedWork/DomainSeedWork/DomainSeedWork.csproj", "BuildingBlocks/SeedWork/DomainSeedWork/"]
COPY ["../../CSharpSource/Innermost.IdempotentCommand/Innermost.IdempotentCommand/Innermost.IdempotentCommand.csproj", "../../CSharpSource/Innermost.IdempotentCommand/Innermost.IdempotentCommand/"]
COPY ["../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection/Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection.csproj", "../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection/"]
COPY ["BuildingBlocks/EventBus/IntegrationEventServiceMongoDB/IntegrationEventServiceMongoDB.csproj", "BuildingBlocks/EventBus/IntegrationEventServiceMongoDB/"]
COPY ["BuildingBlocks/Extensions/ILoggerExtensions/ILoggerExtensions.csproj", "BuildingBlocks/Extensions/ILoggerExtensions/"]
COPY ["../../CSharpSource/TagS/TagS.Microservices.Server.Microsoft.AspNetCore.Http/TagS.Microservices.Server.Microsoft.AspNetCore.Http.csproj", "../../CSharpSource/TagS/TagS.Microservices.Server.Microsoft.AspNetCore.Http/"]
COPY ["../../CSharpSource/Innermost.IdempotentCommand/Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection/Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection.csproj", "../../CSharpSource/Innermost.IdempotentCommand/Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection/"]
COPY ["BuildingBlocks/Extensions/IServiceCollectionExtensions/IServiceCollectionExtensions.csproj", "BuildingBlocks/Extensions/IServiceCollectionExtensions/"]
COPY ["BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj", "BuildingBlocks/EventBus/EventBusServiceBus/"]
RUN dotnet restore "Services/Innermost.TagServer/Innermost.TagServer.API/Innermost.TagServer.API.csproj"
COPY . .
WORKDIR "/src/Services/Innermost.TagServer/Innermost.TagServer.API"
RUN dotnet build "Innermost.TagServer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innermost.TagServer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innermost.TagServer.API.dll"]
