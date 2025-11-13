FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY LetsTripTogether.PublicApi.sln ./
COPY global.json ./

COPY src/ ./src/

WORKDIR /src/src/WebApi
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5089

ENV ASPNETCORE_URLS=http://+:5089
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "WebApi.dll"]
