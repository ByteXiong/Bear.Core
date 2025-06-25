# 使用官方 .NET 运行时镜像作为基础镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8002

# 使用官方 .NET SDK 镜像作为构建镜像
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Bear.Core.Api/Bear.Core.Api.csproj", "Bear.Core.Api/"]
COPY ["Bear.Core.Business/Bear.Core.Business.csproj", "Bear.Core.Business/"]
COPY ["Bear.Core.Common/Bear.Core.Common.csproj", "Bear.Core.Common/"]
COPY ["Bear.Core.Core/Bear.Core.Core.csproj", "Bear.Core.Core/"]
COPY ["Bear.Core.Entity/Bear.Core.Entity.csproj", "Bear.Core.Entity/"]
COPY ["Bear.Core.EventBus/Bear.Core.EventBus.csproj", "Bear.Core.EventBus/"]
COPY ["Bear.Core.IBusiness/Bear.Core.IBusiness.csproj", "Bear.Core.IBusiness/"]
COPY ["Bear.Core.Infrastructure/.Bear.Core.Infrastructure.csproj", "Bear.Core.Infrastructure/"]
COPY ["Bear.Core.Repository/Bear.Core.Repository.csproj", "Bear.Core.Repository/"]
COPY ["Bear.Core.Models/Bear.Core.Models.csproj", "Bear.Core.Models/"]
COPY ["Bear.Core.TaskService/Bear.Core.TaskService.csproj", "Bear.Core.TaskService/"]
COPY ["Bear.Core.ViewModel/Bear.Core.ViewModel.csproj", "Bear.Core.ViewModel/"]
RUN dotnet restore "Bear.Core.Api/Bear.Core.Api.csproj"

COPY . .
WORKDIR "/src/Bear.Core.Api"
RUN dotnet build "Bear.Core.Api.csproj" -c Release -o /app/build

# 发布应用
FROM build AS publish
RUN dotnet publish "Bear.Core.Api.csproj" -c Release -o /app/publish

# 运行阶段
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bear.Core.Api.dll"]
