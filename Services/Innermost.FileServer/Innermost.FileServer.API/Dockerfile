#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Innermost.FileServer/Innermost.FileServer.API/Innermost.FileServer.API.csproj", "Services/Innermost.FileServer/Innermost.FileServer.API/"]
RUN dotnet restore "Services/Innermost.FileServer/Innermost.FileServer.API/Innermost.FileServer.API.csproj"
COPY . .
WORKDIR "/src/Services/Innermost.FileServer/Innermost.FileServer.API"
RUN dotnet build "Innermost.FileServer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innermost.FileServer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innermost.FileServer.API.dll"]