#load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./Source",
                            title: "Cake.SendGrid",
                            repositoryOwner: "cake-contrib",
                            repositoryName: "Cake.SendGrid",
                            shouldRunDotNetCorePack: true,
                            shouldRunDupFinder: true,
                            shouldRunInspectCode: false,
                            appVeyorAccountName: "cakecontrib");

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

Build.RunDotNetCore();
