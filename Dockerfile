FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["LaborExchangeApi.csproj", "."]
RUN dotnet restore "./LaborExchangeApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "LaborExchangeApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LaborExchangeApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80

ENTRYPOINT [ "dotnet", "LaborExchangeApi.dll" ]
