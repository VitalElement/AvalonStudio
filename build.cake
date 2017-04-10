
var target = Argument("target", "Default");
var platform = Argument("platform", "AnyCPU");
var configuration = Argument("configuration", "Release");

var version = "dev-0.20";

var editbin = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Tools\MSVC\14.10.25017\bin\HostX86\x86\editbin.exe";

var artifactsDir = (DirectoryPath)Directory("./artifacts");
var zipRootDir = artifactsDir.Combine("zip");

var fileZipSuffix = ".zip";

var buildDirs = GetDirectories("./AvalonStudio/AvalonStudio/**/bin/**") + 
    GetDirectories("./AvalonStudio/AvalonStudio/**/obj/**") + 
    GetDirectories("./AvalonStudio/AvalonStudioBuild/**/bin/**") + 
    GetDirectories("./AvalonStudio/AvalonStudioBuild/**/obj/**") +
    GetDirectories("./artifacts/**/zip/**");

var netCoreAppsRoot= "./AvalonStudio";
var netCoreApps = new string[] { "AvalonStudio", "AvalonStudioBuild" };
var netCoreProjects = netCoreApps.Select(name => 
    new {
        Path = string.Format("{0}/{1}", netCoreAppsRoot, name),
        Name = name,
        Framework = XmlPeek(string.Format("{0}/{1}/{1}.csproj", netCoreAppsRoot, name), "//*[local-name()='TargetFramework']/text()"),
        Runtimes = XmlPeek(string.Format("{0}/{1}/{1}.csproj", netCoreAppsRoot, name), "//*[local-name()='RuntimeIdentifiers']/text()").Split(';')
    }).ToList();

if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
{
    if (BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag && !string.IsNullOrWhiteSpace(BuildSystem.AppVeyor.Environment.Repository.Tag.Name))
        version = BuildSystem.AppVeyor.Environment.Repository.Tag.Name;
    else
        version += "-build" + EnvironmentVariable("APPVEYOR_BUILD_NUMBER");
}

var MainRepo = "VitalElement/AvalonStudio";
var MasterBranch = "master";
var ReleasePlatform = "Any CPU";
var ReleaseConfiguration = "Release";

var isPlatformAnyCPU = StringComparer.OrdinalIgnoreCase.Equals(platform, "Any CPU");
var isPlatformX86 = StringComparer.OrdinalIgnoreCase.Equals(platform, "x86");
var isPlatformX64 = StringComparer.OrdinalIgnoreCase.Equals(platform, "x64");
var isLocalBuild = BuildSystem.IsLocalBuild;
var isRunningOnUnix = IsRunningOnUnix();
var isRunningOnWindows = IsRunningOnWindows();
var isRunningOnAppVeyor = BuildSystem.AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = BuildSystem.AppVeyor.Environment.PullRequest.IsPullRequest;
var isMainRepo = StringComparer.OrdinalIgnoreCase.Equals(MainRepo, BuildSystem.AppVeyor.Environment.Repository.Name);
var isMasterBranch = StringComparer.OrdinalIgnoreCase.Equals(MasterBranch, BuildSystem.AppVeyor.Environment.Repository.Branch);
var isTagged = BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag 
               && !string.IsNullOrWhiteSpace(BuildSystem.AppVeyor.Environment.Repository.Tag.Name);
var isReleasable = StringComparer.OrdinalIgnoreCase.Equals(ReleasePlatform, platform) 
                   && StringComparer.OrdinalIgnoreCase.Equals(ReleaseConfiguration, configuration);
var isMyGetRelease = !isTagged && isReleasable;
var isNuGetRelease = isTagged && isReleasable;

Task("Clean")
.Does(()=>{
    CleanDirectories(buildDirs);
});

Task("Restore-NetCore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    foreach (var project in netCoreProjects)
    {
        DotNetCoreRestore(project.Path);
    }
});

Task("Build-NetCore")
    .IsDependentOn("Restore-NetCore")
    .Does(() =>
{
    foreach (var project in netCoreProjects)
    {
        Information("Building: {0}", project.Name);
        DotNetCoreBuild(project.Path, new DotNetCoreBuildSettings {
            Configuration = configuration
        });
    }
});

void RunCoreTest(string dir, bool net461Only)
{
    Information("Running tests from " + dir);
    DotNetCoreRestore(dir);
    var frameworks = new List<string>(){"netcoreapp2.0"};
    foreach(var fw in frameworks)
    {
        if(fw != "net461" && net461Only)
            continue;
        Information("Running for " + fw);
        DotNetCoreTest(System.IO.Path.Combine(dir, System.IO.Path.GetFileName(dir)+".csproj"),
            new DotNetCoreTestSettings{Framework = fw});
    }
}


Task("Run-Net-Core-Unit-Tests")
    .IsDependentOn("Clean")
    .Does(() => {
        RunCoreTest("./AvalonStudio/AvalonStudio.Extensibility.Tests", false);
    });



Task("Publish-NetCore")
    .IsDependentOn("Restore-NetCore")
    .Does(() =>
{
    if(isMainRepo && isMasterBranch)
    {
        foreach (var project in netCoreProjects)
        {
            foreach(var runtime in project.Runtimes)
            {
                var outputDir = zipRootDir.Combine(project.Name + "-" + runtime);

                Information("Publishing: {0}, runtime: {1}", project.Name, runtime);
                DotNetCorePublish(project.Path, new DotNetCorePublishSettings {
                    Framework = project.Framework,
                    Configuration = configuration,
                    Runtime = runtime,
                    OutputDirectory = outputDir.FullPath
                });

                if (IsRunningOnWindows() && (runtime == "win7-x86" || runtime == "win7-x64"))
                {
                    Information("Patching executable subsystem for: {0}, runtime: {1}", project.Name, runtime);
                    var targetExe = outputDir.CombineWithFilePath(project.Name + ".exe");
                    var exitCodeWithArgument = StartProcess(editbin, new ProcessSettings { 
                        Arguments = "/subsystem:windows " + targetExe.FullPath
                    });
                    Information("The editbin command exit code: {0}", exitCodeWithArgument);
                }
            }
        }
    }
    else
    {
        Information("Skipping Publish because build is not on MasterBranch");
    }
});

Task("Zip-NetCore")
    .IsDependentOn("Publish-NetCore")
    .Does(() =>
{
    foreach (var project in netCoreProjects)
    {
        foreach(var runtime in project.Runtimes)
        {
            var outputDir = zipRootDir.Combine(project.Name + "-" + runtime);

            Zip(outputDir.FullPath, zipRootDir.CombineWithFilePath(project.Name + "-" + runtime + fileZipSuffix), 
                GetFiles(outputDir.FullPath + "/*.*"));
        }
    }
});

Task("Default")
    .IsDependentOn("Restore-NetCore")
    .IsDependentOn("Build-NetCore")
    .IsDependentOn("Run-Net-Core-Unit-Tests")
    .IsDependentOn("Publish-NetCore")
    .IsDependentOn("Zip-NetCore");

RunTarget(target);
