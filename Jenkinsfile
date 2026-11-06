pipeline {
    agent any

    environment {
        REPO_URL = 'https://github.com/goldmour93/FlashcardApp.git'
        BRANCH_NAME = 'main'
    }

    stages {
        stage('Checkout') {
            steps {
                echo "Cloning the latest ${BRANCH_NAME} branch"
                git branch: BRANCH_NAME, url: REPO_URL
            }
        }

        stage('Restore') {
            steps {
                echo 'Restoring NuGet packages'
                bat 'dotnet restore FlashcardApp.sln'
            }
        }

        stage('Build') {
            steps {
                echo 'Building the solution'
                bat 'dotnet build FlashcardApp.sln --configuration Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                echo 'Running unit tests'
                bat 'dotnet test tests\\FlashcardApp.Tests.csproj --configuration Release --no-build --logger "console;verbosity=minimal"'
            }
        }

        stage('Finish') {
            steps {
                echo 'Pipeline completed successfully.'
            }
        }
    }

    post {
        always {
            echo 'Pipeline complete.'
        }
        success {
            echo 'Build and tests passed.'
        }
        failure {
            echo 'Build or tests failed.'
        }
    }
}
