FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SproutSignal.Web/SproutSignal.Web.csproj SproutSignal.Web/
RUN dotnet restore SproutSignal.Web/SproutSignal.Web.csproj

COPY . .
RUN dotnet publish SproutSignal.Web/SproutSignal.Web.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

CMD ["sh", "-c", "dotnet SproutSignal.Web.dll --urls http://0.0.0.0:${PORT:-8080}"]
