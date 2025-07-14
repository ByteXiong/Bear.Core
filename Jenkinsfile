pipeline {
    agent any

    environment {
        API_IMAGE = "bearplatform-api:latest"
        ADMIN_IMAGE = "bearplatform-admin:latest"
        DEPLOY_DIR = "/data/docker"
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Detect Changes') {
            steps {
                script {
                    // 检查最近5次提交变更文件
                    def changeFiles = sh(script: 'git diff --name-only HEAD HEAD~5', returnStdout: true).trim()
                    echo "Changed files:\n${changeFiles}"

                    def changedList = changeFiles.split('\n')

                    env.BACKEND_CHANGED = changedList.any { it.startsWith('BearPlatform/') }.toString()
                    env.FRONTEND_CHANGED = changedList.any { it.startsWith('BearPlatform.Admin/') }.toString()

                    echo "Backend changed: ${env.BACKEND_CHANGED}"
                    echo "Frontend changed: ${env.FRONTEND_CHANGED}"
                }
            }
        }

        stage('Build') {
            parallel {
                stage('Build Backend') {
                    when {
                        expression { env.BACKEND_CHANGED == 'true' }
                    }
                    steps {
                        dir('BearPlatform') {
                            sh '''
                            docker build -t ${API_IMAGE} .
                            '''
                        }
                    }
                }

                stage('Build Frontend') {
                    when {
                        expression { env.FRONTEND_CHANGED == 'true' }
                    }
                    steps {
                        dir('BearPlatform.Admin') {
                            sh '''
                            docker build -t ${ADMIN_IMAGE} .
                            '''
                        }
                    }
                }
            }
        }

        stage('Deploy') {
            steps {
                dir("${DEPLOY_DIR}") {
                    sh '''
                    docker-compose pull || true
                    docker-compose up -d --build
                    '''
                }
            }
        }
    }

    post {
        success {
            echo "Build and deploy succeeded."
        }
        failure {
            echo "Build or deploy failed."
        }
    }
}
