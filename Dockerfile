FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["MyPantry.Api/MyPantry.Api.csproj", "MyPantry.Api/"]
COPY ["MyPantry.Core/MyPantry.Core.csproj", "MyPantry.Core/"]
COPY ["MyPantry.Infrastructure/MyPantry.Infrastructure.csproj", "MyPantry.Infrastructure/"]
RUN dotnet restore "MyPantry.Api/MyPantry.Api.csproj"

COPY . .
WORKDIR "/src/MyPantry.Api"
RUN dotnet publish "MyPantry.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyPantry.Api.dll"]
