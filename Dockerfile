#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Book.API/Book.API.csproj", "Book.API/"]
COPY ["Book.IService/Book.IService.csproj", "Book.IService/"]
COPY ["Book.Repository/Book.Repository.csproj", "Book.Repository/"]
COPY ["Book.Extensions/Book.Extensions.csproj", "Book.Extensions/"]
COPY ["Book.Models/Book.Models.csproj", "Book.Models/"]
COPY ["Book.IRepository/Book.IRepository.csproj", "Book.IRepository/"]
COPY ["Book.Tasks/Book.Tasks.csproj", "Book.Tasks/"]
COPY ["Book.Service/Book.Service.csproj", "Book.Service/"]
RUN dotnet restore "Book.API/Book.API.csproj"
COPY . .
WORKDIR "/src/Book.API"
RUN dotnet build "Book.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Book.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Book.API.dll"]