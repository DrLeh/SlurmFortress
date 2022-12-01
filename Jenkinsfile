import org.jenkinsci.plugins.workflow.cps.CpsThread
import org.jenkinsci.plugins.workflow.actions.LabelAction
import com.cwctravel.hudson.plugins.extended_choice_parameter.ExtendedChoiceParameterDefinition

def buildNumber = env.BUILD_NUMBER
def octopusVersion = "1.0.${buildNumber}"
def octopusServer = 'http://asi-deploy-02/'
def octopusProject = ''
def appName = 'SlurmFortress.Web.Api'
def jiraProject = 'ENCORE';
def basePath = "src\\${appName}\\bin\\Release\\net6.0"
def artifactsPath = "${basePath}\\octo"
def publishPath = "${basePath}\\publish"

def nuget = "nuget.exe"
def nugetServer = "\\\\asinetwork.local\\servers\\nuget\\packages"
def branchIsMaster  = env.BRANCH_NAME == 'master';

def skipXray = false; 
if (params.SKIPXRAY != null) {
    skipXray = params.SKIPXRAY
}

def recordTests = branchIsMaster && !skipXray;

def cronSchedule = '0 16 * * 1-5'

def projectProperties = [
    [$class: 'BuildDiscarderProperty', strategy: [$class: 'LogRotator', numToKeepStr: '10']],
    parameters([
        booleanParam(defaultValue: false, description: 'Skip Xray?', name: 'SKIPXRAY')
    ]),
];
if (env.BRANCH_NAME == 'master'){ 
    projectProperties.push(
        pipelineTriggers(
            [
                parameterizedCron(cronSchedule)
            ]
        )
    )
}
properties(projectProperties)

echo "buildNumber: ${buildNumber}"
echo "octopusVersion: ${octopusVersion}"
echo "octopusServer: ${octopusServer}"
echo "appName: ${appName}"
echo "basePath: ${basePath}"
echo "artifactsPath: ${artifactsPath}"
echo "PATH is $env.PATH"

node {
    stage('checkout') {
        checkout scm
    }

    stage('build') {
        bat 'dotnet build SlurmFortress.sln -c Release'
    }

    stage('test') {
        //xunitlogger will pull in the assembly name here
        bat "dotnet test ${app.slnName} ${filter} -r reports -l:xunit;LogFileName={assembly}.TestResults.xml"

        if (recordTests) {
            step([$class: 'XrayImportBuilder', endpointName: '/xunit', importFilePath: 'reports/*.xml', importToSameExecution: 'true', projectKey: jiraProject, serverInstance: '0ec7c279-cb92-4a2e-93ac-9de8ec6d5eb9'])
        }
    }
    
    stage('package') {
        bat "dotnet publish src\\${appName} -c Release --output=${publishPath}"
        
        archiveArtifacts artifacts: '**/*.nupkg', onlyIfSuccessful: true

        if (branchIsMaster) {
            bat("${nuget} push **/*.nupkg -source ${nugetServer}")
        }
    }

    stage('deploy') {
        if(octopusProject != ''){
            if (env.BRANCH_NAME == 'master')
            { 
                withCredentials([string(credentialsId: 'octopus-api-key', variable: 'octopusApiKey')]) {

                    //half the time this takes forever because artifacts sometimes are created 
                    echo "octo pack starting"
                    bat "dotnet octo pack --id=${appName} --version=${octopusVersion} --basePath=\"${publishPath}\" --outFolder=\"${artifactsPath}\""
            
                    echo "octo push starting"
                    bat "dotnet octo push --package=${artifactsPath}\\${appName}.${octopusVersion}.nupkg --server=${octopusServer} --apiKey ${octopusApiKey}"
            
                    bat "dotnet octo create-release --server ${octopusServer} --apiKey ${octopusApiKey} --project \"${octopusProject}\" --packageversion \"${octopusVersion}\" --version \"${octopusVersion}\""
                    bat "octo deploy-release --server ${octopusServer} --apiKey ${octopusApiKey} --project \"${octopusProject}\" --releaseNumber \"${octopusVersion}\" --deployto DEV-ESPFamily"
                    bat "octo deploy-release --server ${octopusServer} --apiKey ${octopusApiKey} --project \"${octopusProject}\" --releaseNumber \"${octopusVersion}\" --deployto UAT-ESPFamily"
                }
            }
        }
    }
    
    stage('Cleanup') {
        step([$class: 'WsCleanup', notFailBuild: true])
    }
}
