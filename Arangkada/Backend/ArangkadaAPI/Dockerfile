FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS="http://+:80"

ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

COPY ["ArangkadaAPI.csproj", "./"]

RUN dotnet restore "./ArangkadaAPI.csproj"

COPY . .

WORKDIR "/src/."

RUN dotnet build "ArangkadaAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ArangkadaAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT [ "dotnet", "ArangkadaAPI.dll" ]