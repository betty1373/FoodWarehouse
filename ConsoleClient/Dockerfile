#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsoleClient/ConsoleClient.csproj", "ConsoleClient/"]
RUN dotnet restore "ConsoleClient/ConsoleClient.csproj"
COPY . .
WORKDIR "/src/ConsoleClient"
RUN dotnet build "ConsoleClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleClient.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleClient.dll"]