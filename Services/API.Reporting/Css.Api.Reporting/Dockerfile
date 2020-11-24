#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/API.Reporting/Css.Api.Reporting/Css.Api.Reporting.csproj", "Services/API.Reporting/Css.Api.Reporting/"]
COPY ["Services/API.Core/Css.Api.Core.ExceptionHandling/Css.Api.Core.ExceptionHandling.csproj", "Services/API.Core/Css.Api.Core.ExceptionHandling/"]
COPY ["Services/API.Reporting/Css.Api.Reporting.Models/Css.Api.Reporting.Models.csproj", "Services/API.Reporting/Css.Api.Reporting.Models/"]
COPY ["Services/API.Core/Css.Api.Core.Models/Css.Api.Core.Models.csproj", "Services/API.Core/Css.Api.Core.Models/"]
COPY ["Services/API.Reporting/Css.Api.Reporting.Business/Css.Api.Reporting.Business.csproj", "Services/API.Reporting/Css.Api.Reporting.Business/"]
COPY ["Services/API.Reporting/Css.Api.Reporting.Repository/Css.Api.Reporting.Repository.csproj", "Services/API.Reporting/Css.Api.Reporting.Repository/"]
COPY ["Services/API.Core/Css.Api.Core.Utilities/Css.Api.Core.Utilities.csproj", "Services/API.Core/Css.Api.Core.Utilities/"]
COPY ["Services/API.Core/Css.Api.Core.DataAccess/Css.Api.Core.DataAccess.csproj", "Services/API.Core/Css.Api.Core.DataAccess/"]
RUN dotnet restore "Services/API.Reporting/Css.Api.Reporting/Css.Api.Reporting.csproj"
COPY . .
WORKDIR "/src/Services/API.Reporting/Css.Api.Reporting"
RUN dotnet build "Css.Api.Reporting.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Css.Api.Reporting.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Css.Api.Reporting.dll"]
