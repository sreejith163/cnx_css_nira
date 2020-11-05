#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/API.Scheduling/Css.Api.Scheduling/Css.Api.Scheduling.csproj", "Services/API.Scheduling/Css.Api.Scheduling/"]
COPY ["Services/API.Scheduling/Css.Api.Scheduling.Models/Css.Api.Scheduling.Models.csproj", "Services/API.Scheduling/Css.Api.Scheduling.Models/"]
COPY ["Services/API.Core/Css.Api.Core.Models/Css.Api.Core.Models.csproj", "Services/API.Core/Css.Api.Core.Models/"]
COPY ["Services/API.Scheduling/Css.Api.Scheduling.Business/Css.Api.Scheduling.Business.csproj", "Services/API.Scheduling/Css.Api.Scheduling.Business/"]
COPY ["Services/API.Scheduling/Css.Api.Scheduling.Repository/Css.Api.Scheduling.Repository.csproj", "Services/API.Scheduling/Css.Api.Scheduling.Repository/"]
COPY ["Services/API.Core/Css.Api.Core.Utilities/Css.Api.Core.Utilities.csproj", "Services/API.Core/Css.Api.Core.Utilities/"]
COPY ["Services/API.Core/Css.Api.Core.DataAccess/Css.Api.Core.DataAccess.csproj", "Services/API.Core/Css.Api.Core.DataAccess/"]
COPY ["Services/API.Core/Css.Api.Core.ExceptionHandling/Css.Api.Core.ExceptionHandling.csproj", "Services/API.Core/Css.Api.Core.ExceptionHandling/"]
RUN dotnet restore "Services/API.Scheduling/Css.Api.Scheduling/Css.Api.Scheduling.csproj"
COPY . .
WORKDIR "/src/Services/API.Scheduling/Css.Api.Scheduling"
RUN dotnet build "Css.Api.Scheduling.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Css.Api.Scheduling.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Css.Api.Scheduling.dll"]
