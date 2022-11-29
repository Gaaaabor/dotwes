#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DungeonOfTheWickedEventSourcing.Web/DungeonOfTheWickedEventSourcing.Web.csproj", "DungeonOfTheWickedEventSourcing.Web/"]
RUN dotnet restore "DungeonOfTheWickedEventSourcing.Web/DungeonOfTheWickedEventSourcing.Web.csproj"
COPY . .
WORKDIR "/src/DungeonOfTheWickedEventSourcing.Web"
RUN dotnet build "DungeonOfTheWickedEventSourcing.Web.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "DungeonOfTheWickedEventSourcing.Web.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DungeonOfTheWickedEventSourcing.Web.dll"]