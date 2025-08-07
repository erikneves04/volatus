FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY *.sln .
COPY Volatus.Api/*.csproj ./Volatus.Api/
COPY Volatus.Domain/*.csproj ./Volatus.Domain/
COPY Volatus.Data/*.csproj ./Volatus.Data/
COPY Volatus.Tests/*.csproj ./Volatus.Tests/

RUN dotnet restore
COPY . .

WORKDIR /app/Volatus.Api
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Volatus.Api.dll"]