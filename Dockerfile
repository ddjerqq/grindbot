ARG USER_ID=1000
ARG GROUP_ID=1000
ARG USERNAME="appuser"

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
ARG USER_ID
ARG GROUP_ID
ARG USERNAME

RUN groupadd -g ${GROUP_ID} ${USERNAME} && \
    useradd -u ${USER_ID} -g ${GROUP_ID} -m -s /bin/bash ${USERNAME} && \
    chown -R ${USERNAME}:${USERNAME} /home/${USERNAME}

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY ["src/GrindBot.Domain/GrindBot.Domain.csproj", "src/GrindBot.Domain/"]
COPY ["src/GrindBot.Application/GrindBot.Application.csproj", "src/GrindBot.Application/"]
COPY ["src/GrindBot.Infrastructure/GrindBot.Infrastructure.csproj", "src/GrindBot.Infrastructure/"]
COPY ["src/GrindBot.DiscordClient/GrindBot.DiscordClient.csproj", "src/GrindBot.DiscordClient"]

RUN dotnet restore "src/GrindBot.Domain/"
RUN dotnet restore "src/GrindBot.Application/"
RUN dotnet restore "src/GrindBot.Infrastructure/"
RUN dotnet restore "src/GrindBot.DiscordClient"

COPY . .

WORKDIR /app/src

RUN dotnet build -c Release --no-restore "src/GrindBot.Domain/"
RUN dotnet build -c Release --no-restore "src/GrindBot.Application/"
RUN dotnet build -c Release --no-restore "src/GrindBot.Infrastructure/"
RUN dotnet build -c Release --no-restore "src/GrindBot.DiscordClient"

FROM build AS publish
WORKDIR /app/src/
RUN dotnet publish -c Release -o /app/publish --no-restore --no-build "GrindBot.DiscordClient/GrindBot.DiscordClient.csproj"

FROM base AS final
USER ${USERNAME}
WORKDIR /home/${USERNAME}

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "GrindBot.DiscordClient.dll"]
