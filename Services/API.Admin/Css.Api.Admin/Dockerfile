#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/API.AdminOps/Css.Api.AdminOps/Css.Api.AdminOps.csproj", "Services/API.AdminOps/Css.Api.AdminOps/"]
COPY ["Services/API.AdminOps/Css.Api.AdminOps.Business/Css.Api.AdminOps.Business.csproj", "Services/API.AdminOps/Css.Api.AdminOps.Business/"]
COPY ["Services/API.Core/Css.Api.Core.Models/Css.Api.Core.Models.csproj", "Services/API.Core/Css.Api.Core.Models/"]
COPY ["Services/API.AdminOps/Css.Api.AdminOps.Models/Css.Api.AdminOps.Models.csproj", "Services/API.AdminOps/Css.Api.AdminOps.Models/"]
COPY ["Services/API.AdminOps/Css.Api.AdminOps.Repository/Css.Api.AdminOps.Repository.csproj", "Services/API.AdminOps/Css.Api.AdminOps.Repository/"]
COPY ["Services/API.Core/Css.Api.Core.Utilities/Css.Api.Core.Utilities.csproj", "Services/API.Core/Css.Api.Core.Utilities/"]
COPY ["Services/API.Core/Css.Api.Core.DataAccess/Css.Api.Core.DataAccess.csproj", "Services/API.Core/Css.Api.Core.DataAccess/"]
COPY ["Services/API.Core/Css.Api.Core.ExceptionHandling/Css.Api.Core.ExceptionHandling.csproj", "Services/API.Core/Css.Api.Core.ExceptionHandling/"]
RUN dotnet restore "Services/API.AdminOps/Css.Api.AdminOps/Css.Api.AdminOps.csproj"
COPY . .
WORKDIR "/src/Services/API.AdminOps/Css.Api.AdminOps"
RUN dotnet build "Css.Api.AdminOps.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Css.Api.AdminOps.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Css.Api.AdminOps.dll"]
