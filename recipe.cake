#load nuget:?package=Cake.Recipe&version=2.2.0

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./Source",
                            title: "Cake.SendGrid",
                            masterBranchName: "main",
                            repositoryOwner: "cake-contrib",
                            repositoryName: "Cake.SendGrid",
                            shouldRunDotNetCorePack: true,
                            shouldRunDupFinder: false,
                            shouldRunInspectCode: false,
                            shouldRunCodecov: false,
                            appVeyorAccountName: "cakecontrib",
                            shouldCalculateVersion: true);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

Build.RunDotNetCore();
