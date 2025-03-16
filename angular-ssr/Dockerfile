# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
#RUN apt-get update; \
    #apt-get install -y ca-certificates curl gnupg; \
    #curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key \
    #| gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg; \
    #NODE_MAJOR=20; \
    #echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_$NODE_MAJOR.x nodistro main" \
    #> /etc/apt/sources.list.d/nodesource.list; \
    #apt-get update; \
    #apt-get install -y nodejs

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
RUN apt-get update; \
    apt-get install -y ca-certificates curl gnupg; \
    curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key \
    | gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg; \
    NODE_MAJOR=20; \
    echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_$NODE_MAJOR.x nodistro main" \
    > /etc/apt/sources.list.d/nodesource.list; \
    apt-get update; \
    apt-get install -y nodejs

WORKDIR /src
COPY "angular-ssr.csproj" . 
RUN dotnet restore "./angular-ssr.csproj"
COPY . .
RUN dotnet build "./angular-ssr.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./angular-ssr.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
USER $APP_UID
WORKDIR /app
COPY --from=publish /app/publish .
ENV PATH="/usr/bin:$PATH"
ENTRYPOINT ["dotnet", "angular-ssr.dll"]