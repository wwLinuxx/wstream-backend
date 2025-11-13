FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 4444
EXPOSE 4445

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/UzTube.API/UzTube.API.csproj", "src/UzTube.API/"]
COPY ["src/UzTube.Shared/UzTube.Shared.csproj", "src/UzTube.Shared/"]
COPY ["src/UzTube.Core/UzTube.Core.csproj", "src/UzTube.Core/"]
COPY ["src/UzTube.DataAccess/UzTube.DataAccess.csproj", "src/UzTube.DataAccess/"]
COPY ["src/UzTube.Application/UzTube.Application.csproj", "src/UzTube.Application/"]
RUN dotnet restore "src/UzTube.API/UzTube.API.csproj"
COPY . .
WORKDIR "/src/src/UzTube.API"
RUN dotnet build "./UzTube.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UzTube.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UzTube.API.dll"]
