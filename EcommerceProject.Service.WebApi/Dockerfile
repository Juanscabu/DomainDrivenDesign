FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
EXPOSE 80
WORKDIR /src 
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS http://*:44319
COPY . .
WORKDIR /src/EcommerceProject.Service.WebApi
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /src/EcommerceProject.Service.WebApi/out ./
ENTRYPOINT ["dotnet", "EcommerceProject.Service.WebApi.dll"]