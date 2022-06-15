#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Innermost.LogLife/Innermost.LogLife.API/Innermost.LogLife.API.csproj", "Services/Innermost.LogLife/Innermost.LogLife.API/"]
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