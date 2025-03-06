pipeline {
    agent any

    environment {
        DOCKER_USER = credentials('DOCKER_USER')
        DOCKER_PASSWORD = credentials('DOCKER_PASSWORD')
        SSH_DEPLOY_IP = credentials('SSH_DEPLOY_IP')
        SSH_DEPLOY_USER = credentials('SSH_DEPLOY_USER')
        SSH_DEPLOY_PASSWORD = credentials('SSH_DEPLOY_PASSWORD')
        PORT = credentials('PORT')
    }

    stages {
        stage('Docker Login') {
            steps {
                script {
                    sh 'echo "$DOCKER_PASSWORD" | docker login -u "$DOCKER_USER" --password-stdin'
                }
            }
        }

        stage('Build & Push Frontend Docker Image') {
            steps {
                script {
                    sh '''
                    export DOCKER_REGISTRY=$DOCKER_USER
                    cd demo-frontend
                    docker-compose build
                    docker-compose push
                    '''
                }
            }
        }

        stage('Upload Frontend Docker Compose File') {
            steps {
                archiveArtifacts artifacts: 'demo-frontend/docker-compose.yml', fingerprint: true
            }
        }

        stage('Build & Push Backend Docker Image') {
            steps {
                script {
                    sh '''
                    export DOCKER_REGISTRY=$DOCKER_USER
                    cd demo-backend/Demo
                    docker-compose -f docker-compose.yml -f docker-compose.override.yml build
                    docker-compose -f docker-compose.yml -f docker-compose.override.yml push
                    '''
                }
            }
        }

        stage('Upload Backend Docker Compose Files') {
            steps {
                archiveArtifacts artifacts: 'demo-backend/Demo/docker-compose.yml, demo-backend/Demo/docker-compose.override.yml', fingerprint: true
            }
        }

        stage('Deploy Frontend') {
            steps {
                script {
                    sh '''
                    mkdir -p ~/.ssh
                    sshpass -p "$SSH_DEPLOY_PASSWORD" ssh -o StrictHostKeyChecking=no $SSH_DEPLOY_USER@$SSH_DEPLOY_IP -p $PORT "mkdir -p app/demo-frontend"
                    sshpass -p "$SSH_DEPLOY_PASSWORD" scp -P $PORT demo-frontend/docker-compose.yml $SSH_DEPLOY_USER@$SSH_DEPLOY_IP:app/demo-frontend/
                    sshpass -p "$SSH_DEPLOY_PASSWORD" ssh -o StrictHostKeyChecking=no $SSH_DEPLOY_USER@$SSH_DEPLOY_IP -p $PORT "
                        cd app/demo-frontend
                        sed -i '/build:\\|context:\\|dockerfile:/d' docker-compose.yml
                        docker-compose down
                        docker-compose pull
                        docker-compose up -d --build
                    "
                    '''
                }
            }
        }

        stage('Deploy Backend') {
            steps {
                script {
                    sh '''
                    mkdir -p ~/.ssh
                    sshpass -p "$SSH_DEPLOY_PASSWORD" ssh -o StrictHostKeyChecking=no $SSH_DEPLOY_USER@$SSH_DEPLOY_IP -p $PORT "mkdir -p app/demo-backend/Demo"
                    sshpass -p "$SSH_DEPLOY_PASSWORD" scp -P $PORT demo-backend/Demo/docker-compose.yml demo-backend/Demo/docker-compose.override.yml $SSH_DEPLOY_USER@$SSH_DEPLOY_IP:app/demo-backend/Demo/
                    sshpass -p "$SSH_DEPLOY_PASSWORD" ssh -o StrictHostKeyChecking=no $SSH_DEPLOY_USER@$SSH_DEPLOY_IP -p $PORT "
                        cd app/demo-backend/Demo
                        sed -i '/build:\\|context:\\|dockerfile:/d' docker-compose.yml
                        docker-compose down
                        docker-compose pull
                        docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --build
                    "
                    '''
                }
            }
        }
    }
}
