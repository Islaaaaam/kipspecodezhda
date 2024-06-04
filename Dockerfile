FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SleekClothing.csproj", "./"]
RUN dotnet restore "./SleekClothing.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SleekClothing.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SleekClothing.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SleekClothing.dll"]
