FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BankApi/BankApi.csproj", "BankApi/"]
RUN dotnet restore "BankApi/BankApi.csproj"
COPY . .
WORKDIR "/src/BankApi"
RUN dotnet build "BankApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankApi.dll"]