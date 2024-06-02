#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Webshop.Order.Api/Webshop.Order.Api.csproj", "Webshop.Order.Api/"]
COPY ["Webshop.Order.Application/Webshop.Order.Application.csproj", "Webshop.Order.Application/"]
COPY ["Webshop.Application/Webshop.Application.csproj", "Webshop.Application/"]
COPY ["Webshop.Domain/Webshop.Domain.csproj", "Webshop.Domain/"]
COPY ["Webshop.Order.Domain/Webshop.Order.Domain.csproj", "Webshop.Order.Domain/"]
COPY ["Webshop.Order.Persistence/Webshop.Order.Persistence.csproj", "Webshop.Order.Persistence/"]
COPY ["Webshop.Data.Persistence/Webshop.Data.Persistence.csproj", "Webshop.Data.Persistence/"]
RUN dotnet restore "Webshop.Order.Api/Webshop.Order.Api.csproj"
COPY . .
WORKDIR "/src/Webshop.Order.Api"
RUN dotnet build "Webshop.Order.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Webshop.Order.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webshop.Order.Api.dll"]