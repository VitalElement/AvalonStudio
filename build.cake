/////////////////////////////////////////////////////////////////////
// ADDINS
/////////////////////////////////////////////////////////////////////

#addin "Cake.FileHelpers"

//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////
#tool "nuget:?package=NuGet.CommandLine&version=4.3.0"

///////////////////////////////////////////////////////////////////////////////
// USINGS
///////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var platform = Argument("platform", "AnyCPU");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// CONFIGURATION
///////////////////////////////////////////////////////////////////////////////

var MainRepo = "VitalElement/AvalonStudio";
var MasterBranch = "master";
var ReleasePlatform = "Any CPU";
var ReleaseConfiguration = "Release";

///////////////////////////////////////////////////////////////////////////////
// PARAMETERS
///////////////////////////////////////////////////////////////////////////////

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

///////////////////////////////////////////////////////////////////////////////
// DIRECTORIES
///////////////////////////////////////////////////////////////////////////////

var msvcp140_x86 = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Redist\MSVC\14.14.26405\x86\Microsoft.VC141.CRT\msvcp140.dll";
var msvcp140_x64 = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Redist\MSVC\14.14.26405\x64\Microsoft.VC141.CRT\msvcp140.dll";
var vcruntime140_x86 = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Redist\MSVC\14.14.26405\x86\Microsoft.VC141.CRT\vcruntime140.dll";
var vcruntime140_x64 = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Redist\MSVC\14.14.26405\x64\Microsoft.VC141.CRT\vcruntime140.dll";
var editbin = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Tools\MSVC\14.14.26428\bin\HostX86\x86\editbin.exe";

var artifactsDir = (DirectoryPath)Directory("./artifacts");
var zipRootDir = artifactsDir.Combine("zip");
var nugetRoot = artifactsDir.Combine("nuget");

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
        Framework = XmlPeek(string.Format("{0}/{1}/{1}.csproj", netCoreAppsRoot, name), "//*[local-name()='TargetFrameworks']/text()").Split(';').First(),
        Runtimes = XmlPeek(string.Format("{0}/{1}/{1}.csproj", netCoreAppsRoot, name), "//*[local-name()='RuntimeIdentifiers']/text()").Split(';')
    }).ToList();

///////////////////////////////////////////////////////////////////////////////
// INFORMATION
///////////////////////////////////////////////////////////////////////////////

Information("Building version {0} of AvalonStudio ({1}, {2}) using version {3} of Cake.", 
    platform,
    configuration,
    target,
    typeof(ICakeContext).Assembly.GetName().Version.ToString());

if (isRunningOnAppVeyor)
{
    Information("Repository Name: " + BuildSystem.AppVeyor.Environment.Repository.Name);
    Information("Repository Branch: " + BuildSystem.AppVeyor.Environment.Repository.Branch);
}

Information("Target: " + target);
Information("Platform: " + platform);
Information("Configuration: " + configuration);
Information("IsLocalBuild: " + isLocalBuild);
Information("IsRunningOnUnix: " + isRunningOnUnix);
Information("IsRunningOnWindows: " + isRunningOnWindows);
Information("IsRunningOnAppVeyor: " + isRunningOnAppVeyor);
Information("IsPullRequest: " + isPullRequest);
Information("IsMainRepo: " + isMainRepo);
Information("IsMasterBranch: " + isMasterBranch);
Information("IsTagged: " + isTagged);
Information("IsReleasable: " + isReleasable);
Information("IsMyGetRelease: " + isMyGetRelease);
Information("IsNuGetRelease: " + isNuGetRelease);

var avalonBuildRIDs = new List<string>
{
    "win7-x64"
};

///////////////////////////////////////////////////////////////////////////////
// TASKS
/////////////////////////////////////////////////////////////////////////////// 

Task("Clean")
.Does(()=>{
    CleanDirectory(zipRootDir);
    CleanDirectory(nugetRoot);
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

        var settings = new DotNetCoreBuildSettings {
            Configuration = configuration,

            MSBuildSettings = new DotNetCoreMSBuildSettings {
                MaxCpuCount = 0
            }
        };

        DotNetCoreBuild(project.Path, settings);
    }
});

void RunCoreTest(string dir, bool net461Only)
{
    Information("Running tests from " + dir);
    DotNetCoreRestore(dir);
    var frameworks = new List<string>(){"net5.0"};
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
        RunCoreTest("./AvalonStudio/AvalonStudio.Controls.Standard.Tests", false);
    });



Task("Publish-NetCore")
    .Does(() =>
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

            /*if (IsRunningOnWindows() && (runtime == "win7-x86" || runtime == "win7-x64"))
            {
                Information("Patching executable subsystem for: {0}, runtime: {1}", project.Name, runtime);
                var targetExe = outputDir.CombineWithFilePath(project.Name + ".exe");
                var exitCodeWithArgument = StartProcess(editbin, new ProcessSettings { 
                    Arguments = "/subsystem:windows " + targetExe.FullPath
                });
                Information("The editbin command exit code: {0}", exitCodeWithArgument);
            }*/
        }
    }
});

Task("Copy-Redist-Files-NetCore")
    .IsDependentOn("Publish-NetCore")
    .WithCriteria(()=>((isMainRepo && isMasterBranch && isRunningOnAppVeyor  && !isPullRequest) || isLocalBuild))
    .Does(() =>
{
    foreach (var project in netCoreProjects)
    {
        foreach(var runtime in project.Runtimes)
        {
            var outputDir = zipRootDir.Combine(project.Name + "-" + runtime);
            /*if (IsRunningOnWindows() && runtime == "win7-x86")
            {
                Information("Copying redist files for: {0}, runtime: {1}", project.Name, runtime);
                CopyFileToDirectory(msvcp140_x86, outputDir);
                CopyFileToDirectory(vcruntime140_x86, outputDir);
            }
            if (IsRunningOnWindows() && runtime == "win7-x64")
            {
                Information("Copying redist files for: {0}, runtime: {1}", project.Name, runtime);
                CopyFileToDirectory(msvcp140_x64, outputDir);
                CopyFileToDirectory(vcruntime140_x64, outputDir);
            }*/
        }
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

            Zip(outputDir.FullPath, zipRootDir.CombineWithFilePath(project.Name + "-" + runtime + fileZipSuffix));

            if(DirectoryExists(outputDir))
            {
                DeleteDirectory(outputDir, new DeleteDirectorySettings {
                    Recursive = true,
                    Force = true
                });
            }
        }
    }    
});

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Run-Net-Core-Unit-Tests")
    .IsDependentOn("Build-NetCore")
    .IsDependentOn("Publish-NetCore")
    .IsDependentOn("Copy-Redist-Files-NetCore")
    .IsDependentOn("Zip-NetCore");

Task("OSX")
    .IsDependentOn("Run-Net-Core-Unit-Tests");

Task("Linux")
    .IsDependentOn("Run-Net-Core-Unit-Tests");

RunTarget(target);
