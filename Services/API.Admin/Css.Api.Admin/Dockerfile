#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/API.Admin/Css.Api.Admin/Css.Api.Admin.csproj", "Services/API.Admin/Css.Api.Admin/"]
COPY ["Services/API.Admin/Css.Api.Admin.Business/Css.Api.Admin.Business.csproj", "Services/API.Admin/Css.Api.Admin.Business/"]
COPY ["Services/API.Core/Css.Api.Core.Models/Css.Api.Core.Models.csproj", "Services/API.Core/Css.Api.Core.Models/"]
COPY ["Services/API.Admin/Css.Api.Admin.Repository/Css.Api.Admin.Repository.csproj", "Services/API.Admin/Css.Api.Admin.Repository/"]
COPY ["Services/API.Core/Css.Api.Core.Utilities/Css.Api.Core.Utilities.csproj", "Services/API.Core/Css.Api.Core.Utilities/"]
COPY ["Services/API.Core/Css.Api.Core.ExceptionHandling/Css.Api.Core.ExceptionHandling.csproj", "Services/API.Core/Css.Api.Core.ExceptionHandling/"]
COPY ["Services/API.Admin/Css.Api.Admin.Models/Css.Api.Admin.Models.csproj", "Services/API.Admin/Css.Api.Admin.Models/"]
COPY ["Services/API.Core/Css.Api.Core.DataAccess/Css.Api.Core.DataAccess.csproj", "Services/API.Core/Css.Api.Core.DataAccess/"]
COPY ["Services/API.Admin/Css.Api.Admin.EventHandlers/Css.Api.Admin.EventHandlers.csproj", "Services/API.Admin/Css.Api.Admin.EventHandlers/"]
COPY ["Services/API.Core/Css.Api.Core.EventBus/Css.Api.Core.EventBus.csproj", "Services/API.Core/Css.Api.Core.EventBus/"]
#COPY ["Services/API.Admin/Css.Api.Admin/App_Data/SSL/css.api.admin.pfx", "/app/publish/"]
#COPY ["Services/API.Admin/Css.Api.Admin/App_Data/SSL/css.api.admin.crt", "/usr/local/share/ca-certificates/"]
RUN dotnet restore "Services/API.Admin/Css.Api.Admin/Css.Api.Admin.csproj"
COPY . .
WORKDIR "/src/Services/API.Admin/Css.Api.Admin"
RUN dotnet build "Css.Api.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Css.Api.Admin.csproj" -c Release -o /app/publish

#RUN apt-get update && apt-get install -y curl && apt-get install -y ca-certificates
#RUN update-ca-certificates --fresh

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Css.Api.Admin.dll"]