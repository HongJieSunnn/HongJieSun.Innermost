#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Innermost.Search/Innermost.Search.API/Innermost.Search.API.csproj", "Services/Innermost.Search/Innermost.Search.API/"]
RUN dotnet restore "Services/Innermost.Search/Innermost.Search.API/Innermost.Search.API.csproj"
COPY . .
WORKDIR "/src/Services/Innermost.Search/Innermost.Search.API"
RUN dotnet build "Innermost.Search.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innermost.Search.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innermost.Search.API.dll"]