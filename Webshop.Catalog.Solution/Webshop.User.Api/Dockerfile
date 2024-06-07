#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Webshop.User.Api/Webshop.User.Api.csproj", "Webshop.User.Api/"]
COPY ["Webshop.User.Application/Webshop.User.Application.csproj", "Webshop.User.Application/"]
COPY ["Webshop.Application/Webshop.Application.csproj", "Webshop.Application/"]
COPY ["Webshop.Domain/Webshop.Domain.csproj", "Webshop.Domain/"]
COPY ["Webshop.User.Persistence/Webshop.User.Persistence.csproj", "Webshop.User.Persistence/"]
COPY ["Webshop.Data.Persistence/Webshop.Data.Persistence.csproj", "Webshop.Data.Persistence/"]
RUN dotnet restore "Webshop.User.Api/Webshop.User.Api.csproj"
COPY . .
WORKDIR "/src/Webshop.User.Api"
RUN dotnet build "Webshop.User.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Webshop.User.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webshop.User.Api.dll"]