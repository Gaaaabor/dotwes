#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DungeonOfTheWickedEventSourcing.Common/DungeonOfTheWickedEventSourcing.Common.csproj", "DungeonOfTheWickedEventSourcing.Common/"]
COPY ["DungeonOfTheWickedEventSourcing.Api/DungeonOfTheWickedEventSourcing.Api.csproj", "DungeonOfTheWickedEventSourcing.Api/"]
RUN dotnet restore "DungeonOfTheWickedEventSourcing.Api/DungeonOfTheWickedEventSourcing.Api.csproj"
COPY . .
WORKDIR "/src/DungeonOfTheWickedEventSourcing.Api"
RUN dotnet build "DungeonOfTheWickedEventSourcing.Api.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "DungeonOfTheWickedEventSourcing.Api.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DungeonOfTheWickedEventSourcing.Api.dll"]