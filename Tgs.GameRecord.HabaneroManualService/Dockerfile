#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameRecord.HabaneroManualService/Tgs.GameRecord.HabaneroManualService.csproj", "Tgs.GameRecord.HabaneroManualService/"]
COPY ["Tgs.GameRecord.HabaneroService.Core/Tgs.GameRecord.HabaneroService.Core.csproj", "Tgs.GameRecord.HabaneroService.Core/"]
RUN dotnet restore "Tgs.GameRecord.HabaneroManualService/Tgs.GameRecord.HabaneroManualService.csproj"
COPY . .
WORKDIR "/src/Tgs.GameRecord.HabaneroManualService"
RUN dotnet build "Tgs.GameRecord.HabaneroManualService.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration=Release
RUN dotnet publish "Tgs.GameRecord.HabaneroManualService.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameRecord.HabaneroManualService.dll"]