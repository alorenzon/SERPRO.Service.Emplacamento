#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SERPRO.Service.Emplacamento/SERPRO.Service.Emplacamento.csproj", "SERPRO.Service.Emplacamento/"]
RUN dotnet restore "SERPRO.Service.Emplacamento/SERPRO.Service.Emplacamento.csproj"
COPY . .
WORKDIR "/src/SERPRO.Service.Emplacamento"
RUN dotnet build "SERPRO.Service.Emplacamento.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SERPRO.Service.Emplacamento.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SERPRO.Service.Emplacamento.dll"]