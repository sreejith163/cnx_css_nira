#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 443
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/API.Gateway/Css.Api.Gateway/Css.Api.Gateway.csproj", "Services/API.Gateway/Css.Api.Gateway/"]
COPY ["Services/API.Core/Css.Api.Core.ExceptionHandling/Css.Api.Core.ExceptionHandling.csproj", "Services/API.Core/Css.Api.Core.ExceptionHandling/"]
COPY ["Services/API.Core/Css.Api.Core.Models/Css.Api.Core.Models.csproj", "Services/API.Core/Css.Api.Core.Models/"]
COPY ["Services/API.Core/Css.Api.Core.Utilities/Css.Api.Core.Utilities.csproj", "Services/API.Core/Css.Api.Core.Utilities/"]
#COPY ["Services/API.Gateway/Css.Api.Gateway/App_Data/SSL/css.api.gateway.pfx", "/app/publish/"]
#COPY ["Services/API.Gateway/Css.Api.Gateway/App_Data/SSL/css.api.gateway.crt", "/usr/local/share/ca-certificates/"]
RUN dotnet restore "Services/API.Gateway/Css.Api.Gateway/Css.Api.Gateway.csproj"
COPY . .
WORKDIR "/src/Services/API.Gateway/Css.Api.Gateway"
RUN dotnet build "Css.Api.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Css.Api.Gateway.csproj" -c Release -o /app/publish

#RUN apt-get update && apt-get install -y curl && apt-get install -y ca-certificates
#RUN update-ca-certificates --fresh

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Css.Api.Gateway.dll"]