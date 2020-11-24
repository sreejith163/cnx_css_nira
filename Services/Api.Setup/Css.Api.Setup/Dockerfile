#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Api.SetupMenu/Css.Api.SetupMenu/Css.Api.SetupMenu.csproj", "Services/Api.SetupMenu/Css.Api.SetupMenu/"]
COPY ["Services/Api.SetupMenu/Css.Api.SetupMenu.Models/Css.Api.SetupMenu.Models.csproj", "Services/Api.SetupMenu/Css.Api.SetupMenu.Models/"]
COPY ["Services/API.Core/Css.Api.Core.Models/Css.Api.Core.Models.csproj", "Services/API.Core/Css.Api.Core.Models/"]
COPY ["Services/API.Core/Css.Api.Core.ExceptionHandling/Css.Api.Core.ExceptionHandling.csproj", "Services/API.Core/Css.Api.Core.ExceptionHandling/"]
COPY ["Services/Api.SetupMenu/Css.Api.SetupMenu.Business/Css.Api.SetupMenu.Business.csproj", "Services/Api.SetupMenu/Css.Api.SetupMenu.Business/"]
COPY ["Services/Api.SetupMenu/Css.Api.SetupMenu.Repository/Css.Api.SetupMenu.Repository.csproj", "Services/Api.SetupMenu/Css.Api.SetupMenu.Repository/"]
COPY ["Services/API.Core/Css.Api.Core.Utilities/Css.Api.Core.Utilities.csproj", "Services/API.Core/Css.Api.Core.Utilities/"]
COPY ["Services/API.Core/Css.Api.Core.DataAccess/Css.Api.Core.DataAccess.csproj", "Services/API.Core/Css.Api.Core.DataAccess/"]
RUN dotnet restore "Services/Api.SetupMenu/Css.Api.SetupMenu/Css.Api.SetupMenu.csproj"
COPY . .
WORKDIR "/src/Services/Api.SetupMenu/Css.Api.SetupMenu"
RUN dotnet build "Css.Api.SetupMenu.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Css.Api.SetupMenu.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Css.Api.SetupMenu.dll"]
