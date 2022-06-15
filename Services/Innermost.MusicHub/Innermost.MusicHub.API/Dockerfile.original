#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Innermost.MusicHub/Innermost.MusicHub.API/Innermost.MusicHub.API.csproj", "Services/Innermost.MusicHub/Innermost.MusicHub.API/"]
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