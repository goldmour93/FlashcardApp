pipeline {
    agent any
    options {
        skipDefaultCheckout(true)
        disableConcurrentBuilds()
    }
    environment {
        IMAGE_REPO = 'goldmour/flashcardapp-api'
    }

    stages {
        stage('Checkout') {
            steps {
                echo 'Checking out FlashcardApp from GitHub'
                checkout scm
            }
        }

        stage('Restore') {
            steps {
                sh 'dotnet restore tests/FlashcardApp.Tests.csproj'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build tests/FlashcardApp.Tests.csproj --configuration Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                sh '''
                    mkdir -p test-results
                    dotnet test tests/FlashcardApp.Tests.csproj --configuration Release --no-build --logger "junit;LogFilePath=$WORKSPACE/test-results/results.xml"
                '''
            }
            post {
                always {
                    archiveArtifacts artifacts: 'test-results/results.xml', allowEmptyArchive: true
                    junit 'test-results/results.xml'
                }
            }
        }

        stage('Prepare CI Containers') {
            steps {
                script {
                    env.IMAGE_TAG = sh(script: 'git rev-parse --short=12 HEAD', returnStdout: true).trim()
                    env.COMPOSE_PROJECT_NAME = "flashcard-ci-${env.BUILD_NUMBER}"
                }
                sh '''
                    set +x
                    set -eu
                    umask 077

                    # Generate isolated, disposable CI database credentials.
                    PG_PASSWORD="$(cat /proc/sys/kernel/random/uuid | tr -d '-')"
                    cat > .env <<EOF
POSTGRES_DB=flashcardapp_ci
POSTGRES_USER=flashcard_ci
POSTGRES_PASSWORD=$PG_PASSWORD
DB_CONNECTION_STRING=Host=postgres;Port=5432;Database=flashcardapp_ci;Username=flashcard_ci;Password=$PG_PASSWORD
IMAGE_TAG=$IMAGE_TAG
EOF
                    docker compose -f docker-compose.yml -f docker-compose.ci.yml config --quiet
                '''
            }
        }

        stage('Docker Build') {
            steps {
                sh 'docker compose -f docker-compose.yml -f docker-compose.ci.yml build api'
            }
        }

        stage('Run Containers') {
            steps {
                sh '''
                    docker compose -f docker-compose.yml -f docker-compose.ci.yml up -d --no-build postgres api
                    docker compose -f docker-compose.yml -f docker-compose.ci.yml ps -a
                '''
            }
        }

        stage('Container Testing') {
            steps {
                echo 'Checking PostgreSQL with an actual SQL query'
                sh '''
                    set -eu
                    docker compose -f docker-compose.yml -f docker-compose.ci.yml exec -T postgres sh -c 'PGPASSWORD="$POSTGRES_PASSWORD" psql -U "$POSTGRES_USER" -d "$POSTGRES_DB" -tAc "SELECT 1"' | grep -qx 1
                '''
                echo 'Checking the running API from inside the CI Docker network'
                sh '''
                    set -eu
                    docker run --rm --network "${COMPOSE_PROJECT_NAME}_default" curlimages/curl:8.16.0 --fail --silent --show-error --retry 20 --retry-delay 3 --retry-connrefused http://api:8080/ | grep -F "Flashcard API is running"
                '''
                echo 'Checking that the API can connect to PostgreSQL'
                sh '''
                    docker run --rm --network "${COMPOSE_PROJECT_NAME}_default" curlimages/curl:8.16.0 --fail --silent --show-error --retry 20 --retry-delay 3 --retry-connrefused http://api:8080/health/ready
                '''
            }
        }

        stage('Push to Docker Hub') {
            when {
                expression { env.GIT_BRANCH == 'origin/main' || env.BRANCH_NAME == 'main' }
            }
            steps {
                withCredentials([
                    usernamePassword(
                        credentialsId: 'dockerhub-flashcard',
                        usernameVariable: 'DH_USER',
                        passwordVariable: 'DH_TOKEN'
                    )
                ]) {
                    sh '''
                        set +x
                        set -eu
                        DOCKER_CONFIG="$(mktemp -d)"
                        export DOCKER_CONFIG
                        trap 'docker logout >/dev/null 2>&1 || true; rm -rf "$DOCKER_CONFIG"' EXIT

                        printf '%s' "$DH_TOKEN" | docker login --username "$DH_USER" --password-stdin
                        docker push "$IMAGE_REPO:$IMAGE_TAG"
                    '''
                }
            }
        }
    }

    post {
        always {
            sh '''
                set +e
                if [ -f .env ]; then
                    docker compose -f docker-compose.yml -f docker-compose.ci.yml down --remove-orphans --volumes || true
                    rm -f .env
                fi
            '''
        }
        success {
            echo 'FlashcardApp CI, container checks and eligible image publishing completed successfully.'
        }
        failure {
            echo 'Pipeline failed. Inspect the failing stage in Console Output.'
        }
    }
}
