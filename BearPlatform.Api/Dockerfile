# 使用官方 .NET 运行时镜像作为基础镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8002

# 使用官方 .NET SDK 镜像作为构建镜像
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BearPlatform.Api/BearPlatform.Api.csproj", "BearPlatform.Api/"]
COPY ["BearPlatform.Business/BearPlatform.Business.csproj", "BearPlatform.Business/"]
COPY ["BearPlatform.Common/BearPlatform.Common.csproj", "BearPlatform.Common/"]
COPY ["BearPlatform.Core/BearPlatform.Core.csproj", "BearPlatform.Core/"]
COPY ["BearPlatform.Entity/BearPlatform.Entity.csproj", "BearPlatform.Entity/"]
COPY ["BearPlatform.EventBus/BearPlatform.EventBus.csproj", "BearPlatform.EventBus/"]
COPY ["BearPlatform.IBusiness/BearPlatform.IBusiness.csproj", "BearPlatform.IBusiness/"]
COPY ["BearPlatform.Infrastructure/.BearPlatform.Infrastructure.csproj", "BearPlatform.Infrastructure/"]
COPY ["BearPlatform.Repository/BearPlatform.Repository.csproj", "BearPlatform.Repository/"]
COPY ["BearPlatform.Models/BearPlatform.Models.csproj", "BearPlatform.Models/"]
COPY ["BearPlatform.TaskService/BearPlatform.TaskService.csproj", "BearPlatform.TaskService/"]
COPY ["BearPlatform.ViewModel/BearPlatform.ViewModel.csproj", "BearPlatform.ViewModel/"]
RUN dotnet restore "BearPlatform.Api/BearPlatform.Api.csproj"

COPY . .
WORKDIR "/src/BearPlatform.Api"
RUN dotnet build "BearPlatform.Api.csproj" -c Release -o /app/build

# 发布应用
FROM build AS publish
RUN dotnet publish "BearPlatform.Api.csproj" -c Release -o /app/publish

# 运行阶段
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BearPlatform.Api.dll"]
