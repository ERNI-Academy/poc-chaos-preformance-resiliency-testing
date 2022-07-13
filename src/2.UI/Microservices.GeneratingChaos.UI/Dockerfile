FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/2.UI/Microservices.GeneratingChaos.UI/Microservices.GeneratingChaos.UI.csproj", "src/2.UI/Microservices.GeneratingChaos.UI/"]
RUN dotnet restore "src/2.UI/Microservices.GeneratingChaos.UI/Microservices.GeneratingChaos.UI.csproj"
COPY . .
WORKDIR "/src/src/2.UI/Microservices.GeneratingChaos.UI"
RUN dotnet build "Microservices.GeneratingChaos.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservices.GeneratingChaos.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.GeneratingChaos.UI.dll"]