# デバッグ コンテナーをカスタマイズする方法と、Visual Studio がこの Dockerfile を使用してより高速なデバッグのためにイメージをビルドする方法については、https://aka.ms/customizecontainer をご覧ください。

# このステージは、VS から高速モードで実行するときに使用されます (デバッグ構成の既定値)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# このステージは、サービス プロジェクトのビルドに使用されます
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProdBase.Domain/ProdBase.Domain.csproj", "ProdBase.Domain/"]
COPY ["ProdBase.Application/ProdBase.Application.csproj", "ProdBase.Application/"]
COPY ["ProdBase.Infrastructure/ProdBase.Infrastructure.csproj", "ProdBase.Infrastructure/"]
COPY ["ProdBase.Web/ProdBase.Web.csproj", "ProdBase.Web/"]
RUN dotnet restore "./ProdBase.Web/ProdBase.Web.csproj"
COPY . .
WORKDIR "/src/ProdBase.Web"
RUN dotnet build "./ProdBase.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# このステージは、最終ステージにコピーするサービス プロジェクトを公開するために使用されます
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProdBase.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# このステージは、運用環境または VS から通常モードで実行している場合に使用されます (デバッグ構成を使用しない場合の既定)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProdBase.Web.dll"]
