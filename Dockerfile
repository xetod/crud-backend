FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 6060

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY source/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

WORKDIR /src
RUN dotnet restore "/src/Crud.Api/Crud.Api.csproj"
COPY . .
WORKDIR "/src/source/Crud.Api"
RUN dotnet build "Crud.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Crud.Api.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .       
ENTRYPOINT ["dotnet", "Crud.Api.dll"]
