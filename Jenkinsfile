pipeline {
  agent any

  environment {
    DOTNET_ROOT = "/usr/share/dotnet"
    PATH = "${DOTNET_ROOT}:${PATH}"
    NODE_HOME = tool name: 'nodejs18', type: 'nodejs'
    PATH = "${NODE_HOME}/bin:${env.PATH}"
  }

  stages {
    stage('拉取代码') {
      steps {
        git 'https://gitee.com/ByteXiong/BearPlatform.git'
      }
    }

    stage('构建后端 (.NET 8)') {
      steps {
        dir('BearPlatform') {
          sh 'dotnet restore'
          sh 'dotnet publish -c Release -o /data/docker/bearplatform-api'
        }
      }
    }

    stage('构建前端 (Vue3)') {
      steps {
        dir('BearPlatform/BearPlatform.Admin') {
          sh 'pnpm install'
          sh 'pnpm run build'
          sh 'rm -rf /data/docker/bearplatform-admin/*'
          sh 'cp -r dist/* /data/docker/bearplatform-admin/'
        }
      }
    }
  }

  post {
    success {
      echo "✅ 构建成功"
    }
    failure {
      echo "❌ 构建失败"
    }
  }
}
