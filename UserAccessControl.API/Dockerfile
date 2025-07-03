FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UserAccessControl.API/UserAccessControl.API.csproj", "UserAccessControl.API/"]
COPY ["UserAccessControl.Application/UserAccessControl.Application.csproj", "UserAccessControl.Application/"]
COPY ["UserAccessControl.Core/UserAccessControl.Core.csproj", "UserAccessControl.Core/"]
RUN dotnet restore "UserAccessControl.API/UserAccessControl.API.csproj"
COPY . .
WORKDIR "/src/UserAccessControl.API"
RUN dotnet build "UserAccessControl.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UserAccessControl.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserAccessControl.API.dll"]
