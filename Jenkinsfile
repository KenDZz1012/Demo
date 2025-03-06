pipeline {
    agent any

    environment {
        // Sử dụng credentials binding với cú pháp an toàn hơn
        DOCKER_CREDS = credentials('docker-credentials') // ID: DOCKER_USER, Secret: DOCKER_PASSWORD
        SSH_CREDS = credentials('ssh-credentials')      // ID: SSH_DEPLOY_USER, Secret: SSH_DEPLOY_PASSWORD
        SSH_DEPLOY_IP = credentials('SSH_DEPLOY_IP')
        PORT = credentials('PORT')
        DOCKER_REGISTRY = "${env.DOCKER_CREDS_USR}"     // Sử dụng username từ credentials
    }

    stages {
        stage('Docker Login') {
            steps {
                script {
                    sh 'echo "${DOCKER_CREDS_PSW}" | docker login -u "${DOCKER_CREDS_USR}" --password-stdin'
                }
            }
        }

        stage('Build & Push Frontend') {
            steps {
                dir('demo-frontend') {
                    sh '''
                        docker-compose build
                        docker-compose push
                    '''
                }
            }
        }

        stage('Build & Push Backend') {
            steps {
                dir('demo-backend/Demo') {
                    sh '''
                        docker-compose -f docker-compose.yml -f docker-compose.override.yml build
                        docker-compose -f docker-compose.yml -f docker-compose.override.yml push
                    '''
                }
            }
        }

        stage('Archive Artifacts') {
            steps {
                archiveArtifacts artifacts: 'demo-frontend/docker-compose.yml,demo-backend/Demo/docker-compose.yml,demo-backend/Demo/docker-compose.override.yml', 
                               fingerprint: true
            }
        }

        stage('Deploy Frontend') {
            steps {
                script {
                    def sshCmd = """
                        mkdir -p app/demo-frontend && 
                        cd app/demo-frontend && 
                        sed -i '/build:\\|context:\\|dockerfile:/d' docker-compose.yml && 
                        docker-compose down && 
                        docker-compose pull && 
                        docker-compose up -d
                    """
                    sh """
                        sshpass -p '${SSH_CREDS_PSW}' scp -P ${PORT} demo-frontend/docker-compose.yml ${SSH_CREDS_USR}@${SSH_DEPLOY_IP}:app/demo-frontend/
                        sshpass -p '${SSH_CREDS_PSW}' ssh -o StrictHostKeyChecking=no ${SSH_CREDS_USR}@${SSH_DEPLOY_IP} -p ${PORT} '${sshCmd}'
                    """
                }
            }
        }

        stage('Deploy Backend') {
            steps {
                script {
                    def sshCmd = """
                        mkdir -p app/demo-backend/Demo && 
                        cd app/demo-backend/Demo && 
                        sed -i '/build:\\|context:\\|dockerfile:/d' docker-compose.yml && 
                        docker-compose down && 
                        docker-compose pull && 
                        docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
                    """
                    sh """
                        sshpass -p '${SSH_CREDS_PSW}' scp -P ${PORT} demo-backend/Demo/docker-compose.yml demo-backend/Demo/docker-compose.override.yml ${SSH_CREDS_USR}@${SSH_DEPLOY_IP}:app/demo-backend/Demo/
                        sshpass -p '${SSH_CREDS_PSW}' ssh -o StrictHostKeyChecking=no ${SSH_CREDS_USR}@${SSH_DEPLOY_IP} -p ${PORT} '${sshCmd}'
                    """
                }
            }
        }
    }

    post {
        always {
            cleanWs()
        }
    }
}
