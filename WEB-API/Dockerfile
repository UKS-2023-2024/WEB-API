FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WEB-API/WEB-API.csproj", "WEB-API/"]
RUN dotnet restore "WEB-API/WEB-API.csproj"
COPY . .
WORKDIR "/src/WEB-API"
RUN dotnet build "WEB-API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WEB-API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WEB-API.dll"]
