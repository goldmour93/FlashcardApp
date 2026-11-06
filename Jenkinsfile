pipeline {
    agent any
    options {
        skipDefaultCheckout(true)
    }

    stages {
        stage('Checkout') {
            steps {
                echo 'Checking out the project from GitHub'
                checkout scm
            }
        }

        stage('Restore') {
            steps {
                echo 'Restoring test project dependencies'
                sh 'dotnet restore tests/FlashcardApp.Tests.csproj'
            }
        }

        stage('Build') {
            steps {
                echo 'Building the test project and dependencies'
                sh 'dotnet build tests/FlashcardApp.Tests.csproj --configuration Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                echo 'Running .NET unit tests'
                sh '''
                    mkdir -p test-results
                    dotnet test tests/FlashcardApp.Tests.csproj \
                        --configuration Release \
                        --no-build \
                        --logger "junit;LogFilePath=/var/jenkins_home/workspace/FlashcardApp-Ci/test-results/results.xml"
                '''
            }
            post {
                always {
                    archiveArtifacts artifacts: 'test-results/results.xml', allowEmptyArchive: true
                    junit 'test-results/results.xml'
                }
            }
        }
    }

    post {
        success {
            echo 'Pipeline completed successfully.'
        }
        failure {
            echo 'Pipeline failed.'
        }
    }
    
}
