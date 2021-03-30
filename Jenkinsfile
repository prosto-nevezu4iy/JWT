pipeline {
	agent any

	stages {

		stage ('Clean workspace') {
		  steps {
			cleanWs()
		  }
		}

		stage ('Git Checkout') {
		  steps {
			  git branch: 'main', credentialsId: 'jenkins-user-for-github-repository', url: 'https://github.com/prosto-nevezu4iy/JWT.git'
			}
		}

stage('Restore packages') {
  steps {
    bat "dotnet restore ${workspace}\\JWT.sln"
  }
}

stage('Clean') {
  steps {
    bat "msbuild.exe ${workspace}\\JWT.sln" /nologo /nr:false /p:platform=\"x64\" /p:configuration=\"release\" /t:clean"
  }
}
	}
}