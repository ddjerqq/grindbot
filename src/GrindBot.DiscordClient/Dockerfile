FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GrindBot.DiscordClient/GrindBot.DiscordClient.csproj", "GrindBot.DiscordClient/"]
RUN dotnet restore "GrindBot.DiscordClient/GrindBot.DiscordClient.csproj"
COPY . .
WORKDIR "/src/GrindBot.DiscordClient"
RUN dotnet build "./GrindBot.DiscordClient.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GrindBot.DiscordClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrindBot.DiscordClient.dll"]
