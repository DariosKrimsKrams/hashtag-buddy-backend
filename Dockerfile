#https://docs.docker.com/engine/examples/dotnetcore/

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy everything else and build
COPY . .
RUN dotnet publish Instaq.API.Extern -c Release -o Instaq.API.Extern/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/Instaq.API.Extern/out .
ENTRYPOINT ["dotnet", "Instaq.API.Extern.dll"]