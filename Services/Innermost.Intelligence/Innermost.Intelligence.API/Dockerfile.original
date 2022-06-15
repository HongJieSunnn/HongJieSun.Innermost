#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Innermost.Intelligence/Innermost.Intelligence.API/Innermost.Intelligence.API.csproj", "Services/Innermost.Intelligence/Innermost.Intelligence.API/"]
RUN dotnet restore "Services/Innermost.Intelligence/Innermost.Intelligence.API/Innermost.Intelligence.API.csproj"
COPY . .
WORKDIR "/src/Services/Innermost.Intelligence/Innermost.Intelligence.API"
RUN dotnet build "Innermost.Intelligence.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innermost.Intelligence.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innermost.Intelligence.API.dll"]