#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/API.Job/Css.Api.Job/Css.Api.Job.csproj", "Services/API.Job/Css.Api.Job/"]
COPY ["Services/API.Job/Css.Api.Job.Business/Css.Api.Job.Business.csproj", "Services/API.Job/Css.Api.Job.Business/"]
COPY ["Services/API.Job/Css.Api.Job.Models/Css.Api.Job.Models.csproj", "Services/API.Job/Css.Api.Job.Models/"]
COPY ["Services/API.Core/Css.Api.Core.ExceptionHandling/Css.Api.Core.ExceptionHandling.csproj", "Services/API.Core/Css.Api.Core.ExceptionHandling/"]
COPY ["Services/API.Core/Css.Api.Core.Models/Css.Api.Core.Models.csproj", "Services/API.Core/Css.Api.Core.Models/"]
COPY ["Services/API.Core/Css.Api.Core.Utilities/Css.Api.Core.Utilities.csproj", "Services/API.Core/Css.Api.Core.Utilities/"]
RUN dotnet restore "Services/API.Job/Css.Api.Job/Css.Api.Job.csproj"
COPY . .
WORKDIR "/src/Services/API.Job/Css.Api.Job"
RUN dotnet build "Css.Api.Job.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Css.Api.Job.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Css.Api.Job.dll"]