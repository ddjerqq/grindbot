FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY ["src/GrindBot.Domain/GrindBot.Domain.csproj", "src/GrindBot.Domain/"]
COPY ["src/GrindBot.Application/GrindBot.Application.csproj", "src/GrindBot.Application/"]
COPY ["src/GrindBot.Infrastructure/GrindBot.Infrastructure.csproj", "src/GrindBot.Infrastructure/"]
COPY ["src/GrindBot.DiscordClient/GrindBot.DiscordClient.csproj", "src/GrindBot.DiscordClient/"]

RUN dotnet restore "src/GrindBot.Domain/GrindBot.Domain.csproj"
RUN dotnet restore "src/GrindBot.Application/GrindBot.Application.csproj"
RUN dotnet restore "src/GrindBot.Infrastructure/GrindBot.Infrastructure.csproj"
RUN dotnet restore "src/GrindBot.DiscordClient/GrindBot.DiscordClient.csproj"

COPY . .

WORKDIR /app

RUN dotnet build -c Release --no-restore "src/GrindBot.Domain/GrindBot.Domain.csproj"
RUN dotnet build -c Release --no-restore "src/GrindBot.Application/GrindBot.Application.csproj"
RUN dotnet build -c Release --no-restore "src/GrindBot.Infrastructure/GrindBot.Infrastructure.csproj"
RUN dotnet build -c Release --no-restore "src/GrindBot.DiscordClient/GrindBot.DiscordClient.csproj"

FROM build AS publish
WORKDIR /app
RUN dotnet publish -c Release -o /app/publish --no-restore --no-build "src/GrindBot.DiscordClient/GrindBot.DiscordClient.csproj"

FROM base AS final
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "GrindBot.DiscordClient.dll"]
