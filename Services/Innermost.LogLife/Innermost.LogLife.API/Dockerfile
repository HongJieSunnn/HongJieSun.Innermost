#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Innermost.LogLife/Innermost.LogLife.API/Innermost.LogLife.API.csproj", "Services/Innermost.LogLife/Innermost.LogLife.API/"]
COPY ["Services/Innermost.LogLife/Innermost.LogLife.Infrastructure/Innermost.LogLife.Infrastructure.csproj", "Services/Innermost.LogLife/Innermost.LogLife.Infrastructure/"]
COPY ["BuildingBlocks/SeedWork/DomainSeedWork/DomainSeedWork.csproj", "BuildingBlocks/SeedWork/DomainSeedWork/"]
COPY ["../../CSharpSource/Innermost.IdempotentCommand/Innermost.IdempotentCommand/Innermost.IdempotentCommand.csproj", "../../CSharpSource/Innermost.IdempotentCommand/Innermost.IdempotentCommand/"]
COPY ["BuildingBlocks/Extensions/MediatRExtensions/MediatRExtensions.csproj", "BuildingBlocks/Extensions/MediatRExtensions/"]
COPY ["Services/Innermost.LogLife/Innermost.LogLife.Domain/Innermost.LogLife.Domain.csproj", "Services/Innermost.LogLife/Innermost.LogLife.Domain/"]
COPY ["../../CSharpSource/TagS/TagS.Microservices.Client/TagS.Microservices.Client.csproj", "../../CSharpSource/TagS/TagS.Microservices.Client/"]
COPY ["BuildingBlocks/EventBus/EventBusCommon/EventBusCommon.csproj", "BuildingBlocks/EventBus/EventBusCommon/"]
COPY ["Services/Innermost.TagServer/Innermost.TagReferrers/Innermost.TagReferrers.csproj", "Services/Innermost.TagServer/Innermost.TagReferrers/"]
COPY ["BuildingBlocks/CommonService/CommonService/CommonService.csproj", "BuildingBlocks/CommonService/CommonService/"]
COPY ["BuildingBlocks/Extensions/MongoDBExtensions/MongoDBExtensions.csproj", "BuildingBlocks/Extensions/MongoDBExtensions/"]
COPY ["../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext/Innermost.MongoDBContext.csproj", "../../CSharpSource/Innermost.MongoDBContext/Innermost.MongoDBContext/"]
COPY ["BuildingBlocks/Extensions/TypeExtensions/TypeExtensions.csproj", "BuildingBlocks/Extensions/TypeExtensions/"]
COPY ["../../CSharpSource/Innermost.IdempotentCommand/Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection/Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection.csproj", "../../CSharpSource/Innermost.IdempotentCommand/Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection/"]
COPY ["BuildingBlocks/WebHost/WebHost.Extensions/WebHost.CustomExtensions.csproj", "BuildingBlocks/WebHost/WebHost.Extensions/"]
COPY ["BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj", "BuildingBlocks/EventBus/EventBusServiceBus/"]
COPY ["BuildingBlocks/EventBus/IntegrationEventRecord/IntegrationEventRecord.csproj", "BuildingBlocks/EventBus/IntegrationEventRecord/"]
RUN dotnet restore "Services/Innermost.LogLife/Innermost.LogLife.API/Innermost.LogLife.API.csproj"
COPY . .
WORKDIR "/src/Services/Innermost.LogLife/Innermost.LogLife.API"
RUN dotnet build "Innermost.LogLife.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innermost.LogLife.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innermost.LogLife.API.dll"]
