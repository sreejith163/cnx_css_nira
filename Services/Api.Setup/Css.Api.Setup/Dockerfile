#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Api.Setup/Css.Api.Setup/Css.Api.Setup.csproj", "Services/Api.Setup/Css.Api.Setup/"]
COPY ["Services/Api.Setup/Css.Api.Setup.Models/Css.Api.Setup.Models.csproj", "Services/Api.Setup/Css.Api.Setup.Models/"]
COPY ["Services/API.Core/Css.Api.Core.Models/Css.Api.Core.Models.csproj", "Services/API.Core/Css.Api.Core.Models/"]
COPY ["Services/API.Core/Css.Api.Core.ExceptionHandling/Css.Api.Core.ExceptionHandling.csproj", "Services/API.Core/Css.Api.Core.ExceptionHandling/"]
COPY ["Services/Api.Setup/Css.Api.Setup.Business/Css.Api.Setup.Business.csproj", "Services/Api.Setup/Css.Api.Setup.Business/"]
COPY ["Services/Api.Setup/Css.Api.Setup.Repository/Css.Api.Setup.Repository.csproj", "Services/Api.Setup/Css.Api.Setup.Repository/"]
COPY ["Services/API.Core/Css.Api.Core.Utilities/Css.Api.Core.Utilities.csproj", "Services/API.Core/Css.Api.Core.Utilities/"]
COPY ["Services/API.Core/Css.Api.Core.DataAccess/Css.Api.Core.DataAccess.csproj", "Services/API.Core/Css.Api.Core.DataAccess/"]
COPY ["Services/Api.Setup/Css.Api.Setup.EventHandlers/Css.Api.Setup.EventHandlers.csproj", "Services/Api.Setup/Css.Api.Setup.EventHandlers/"]
COPY ["Services/API.Core/Css.Api.Core.EventBus/Css.Api.Core.EventBus.csproj", "Services/API.Core/Css.Api.Core.EventBus/"]
RUN dotnet restore "Services/Api.Setup/Css.Api.Setup/Css.Api.Setup.csproj"
COPY . .
WORKDIR "/src/Services/Api.Setup/Css.Api.Setup"
RUN dotnet build "Css.Api.Setup.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Css.Api.Setup.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Css.Api.Setup.dll"]
