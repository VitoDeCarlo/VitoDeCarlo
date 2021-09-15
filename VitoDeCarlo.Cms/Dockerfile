FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["VitoDeCarlo.Cms/VitoDeCarlo.Cms.csproj", "VitoDeCarlo.Cms/"]
COPY ["VitoDeCarlo.Theme/VitoDeCarlo.Theme.csproj", "VitoDeCarlo.Theme/"]
RUN dotnet restore "VitoDeCarlo.Cms/VitoDeCarlo.Cms.csproj"
COPY . .
WORKDIR "/src/VitoDeCarlo.Cms"
RUN dotnet build "VitoDeCarlo.Cms.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VitoDeCarlo.Cms.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VitoDeCarlo.Cms.dll"]