#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/1.Services/Microservices.GeneratingChaos.Service.FastApi/Microservices.GeneratingChaos.Services.FastApi.csproj", "src/1.Services/Microservices.GeneratingChaos.Service.FastApi/"]
RUN dotnet restore "src/1.Services/Microservices.GeneratingChaos.Service.FastApi/Microservices.GeneratingChaos.Services.FastApi.csproj"
COPY . .
WORKDIR "/src/src/1.Services/Microservices.GeneratingChaos.Service.FastApi"
RUN dotnet build "Microservices.GeneratingChaos.Services.FastApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservices.GeneratingChaos.Services.FastApi.csproj" -c Release -o /app/publish

FROM base AS final
EXPOSE 80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.GeneratingChaos.Services.FastApi.dll"]