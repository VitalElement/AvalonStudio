namespace AvalonStudio
{
    using System;
    using System.IO;
    using CommandLine;
    using System.Linq;
    using AvalonStudio.Projects;
    using AvalonStudio.Projects.CPlusPlus;
    using AvalonStudio.Toolchains;
    using AvalonStudio.Toolchains.STM32;
    using AvalonStudio.Projects.Standard;
    using AvalonStudio.Toolchains.Llilum;
    using Extensibility;
    using Toolchains.Standard;
    using Utils;
    using Platform;
    using Repositories;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using TestFrameworks;
    class Program
    {
        const string version = "1.0.0.16";
        const string releaseName = "Gravity";

        const string baseDir = @"c:\development\vebuild\test";

        static ProgramConsole console = new ProgramConsole();

        static Solution LoadSolution(ProjectOption options)
        {
            var currentDir = Directory.GetCurrentDirectory();

            var solutionFile = Path.Combine(currentDir, options.Solution);

            if (File.Exists(solutionFile))
            {
                return Solution.Load(solutionFile);
            }

            throw new Exception("Solution file: " + options.Solution + "could not be found.");
        }

        static IProject FindProject(Solution solution, string project)
        {
            try
            {
                var result = solution.FindProject(project);

                if (result != null)
                {
                    (result as CPlusPlusProject).ResolveReferences();
                }

                return result;
            }
            catch (Exception e)
            {
                console.WriteLine(e.Message);
                return null;
            }
        }


        static int RunInstallPackage(PackageOptions options)
        {
            console.Write("Downloading catalogs...");

            var availablePackages = new List<PackageReference>();

            foreach (var packageSource in PackageSources.Instance.Sources)
            {
                Repository repo = null;

                repo = packageSource.DownloadCatalog();
                console.WriteLine("Done");

                console.WriteLine("Enumerating Packages...");

                if (repo != null)
                {
                    foreach (var packageReference in repo.Packages)
                    {
                        availablePackages.Add(packageReference);
                        console.WriteLine(packageReference.Name);
                    }
                }
            }

            var package = availablePackages.FirstOrDefault(p => p.Name == options.Package);

            if (package != null)
            {
                var task = package.DownloadInfoAsync();
                task.Wait();

                var repo = task.Result;

                var dlTask = repo.Synchronize(options.Tag, console);
                dlTask.Wait();

                return 1;

            }
            else
            {
                console.WriteLine("Unable to find package " + options.Package);
                return -1;
            }
        }

        static int RunTest(TestOptions options)
        {
            int result = 1;
            var solution = LoadSolution(options);

            var tests = new List<Test>();

            foreach (var project in solution.Projects)
            {
                if(project.TestFramework != null)
                {
                    project.ToolChain.Build(console, project).Wait();                    

                    var awaiter = project.TestFramework.EnumerateTestsAsync(project);
                    awaiter.Wait();

                    foreach(var test in awaiter.Result)
                    {
                        tests.Add(test);
                    }
                }
                
            }

            foreach (var test in tests)
            {
                test.Run();

                console.WriteLine(string.Format("Test: {0} Pass: {1}", test.Name, test.Pass));

                if(!test.Pass)
                {
                    result = 0;
                    break;
                }
            }

            return result;
        }

        static int RunBuild(BuildOptions options)
        {
            int result = 1;
            var solution = LoadSolution(options);
            var project = FindProject(solution, options.Project) as CPlusPlusProject;

            if (project != null)
            {
                var stopWatch = new System.Diagnostics.Stopwatch();
                stopWatch.Start();

                if (project.ToolChain is StandardToolChain)
                {
                    (project.ToolChain as StandardToolChain).Jobs = options.Jobs;
                }

                var awaiter = project.ToolChain.Build(console, project);
                awaiter.Wait();

                stopWatch.Stop();
                console.WriteLine(stopWatch.Elapsed.ToString());

                result = awaiter.Result ? 1 : 2;
            }
            else
            {
                console.WriteLine("Nothing to build.");
            }

            return result;
        }

        static int RunClean(CleanOptions options)
        {
            var solution = LoadSolution(options);

            var console = new ProgramConsole();

            var project = FindProject(solution, options.Project);

            if (project != null)
            {
                project.ToolChain.Clean(console, project).Wait();
            }
            else
            {
                console.WriteLine("Nothing to clean.");
            }

            return 1;
        }

        static string NormalizePath(string path)
        {
            if (path != null)
            {
                return Path.GetFullPath(new Uri(path).LocalPath)
                           .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
            else
            {
                return null;
            }
        }

        static int RunRemove(RemoveOptions options)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), options.File);

            if (File.Exists(file))
            {
                var solution = LoadSolution(options);
                var project = FindProject(solution, options.Project);

                if (project != null)
                {
                    // todo normalize paths.
                    var currentFile = project.Items.OfType<ISourceFile>().Where((s) => s.File.Normalize() == options.File.Normalize()).FirstOrDefault();

                    if (currentFile != null)
                    {
                        project.Items.RemoveAt(project.Items.IndexOf(currentFile));
                        project.Save();

                        Console.WriteLine("File removed.");

                        return 1;
                    }
                    else
                    {
                        Console.WriteLine("File not found in project.");
                        return -1;
                    }

                }
                else
                {
                    Console.WriteLine("Project not found.");
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("File not found.");
                return -1;
            }
        }

        static int RunAdd(AddOptions options)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), options.File);

            if (File.Exists(file))
            {
                var solution = LoadSolution(options);
                var project = FindProject(solution, options.Project) as CPlusPlusProject;

                if (project != null)
                {
                    var sourceFile = SourceFile.FromPath(project, project, options.File);
                    project.Items.Add(sourceFile);
                    project.SourceFiles.InsertSorted(sourceFile);
                    project.Save();
                    Console.WriteLine("File added.");
                    return 1;
                }
                else
                {
                    Console.WriteLine("Project not found.");
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("File not found.");
                return -1;
            }
        }

        static int RunAddReference(AddReferenceOptions options)
        {
            var solution = LoadSolution(options);
            var project = FindProject(solution, options.Project) as CPlusPlusProject;

            if (project != null)
            {
                var currentReference = project.References.Where((r) => r.Name == options.Name).FirstOrDefault();

                if (currentReference != null)
                {
                    project.UnloadedReferences[project.References.IndexOf(currentReference)] = new Reference { Name = options.Name, GitUrl = options.GitUrl, Revision = options.Revision };
                    Console.WriteLine("Reference successfully updated.");
                }
                else
                {
                    bool add = true;

                    if (string.IsNullOrEmpty(options.GitUrl))
                    {
                        var reference = FindProject(solution, options.Name);

                        if (reference == null)
                        {
                            add = false;
                        }
                    }

                    if (add)
                    {
                        project.UnloadedReferences.Add(new Reference { Name = options.Name, GitUrl = options.GitUrl, Revision = options.Revision });
                        Console.WriteLine("Reference added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Local reference does not exist, try creating the project first.");
                    }
                }

                project.Save();
            }

            return 1;
        }

        static int RunCreate(CreateOptions options)
        {
            string projectPath = string.Empty;

            if (string.IsNullOrEmpty(options.Project))
            {
                projectPath = Directory.GetCurrentDirectory();
                options.Project = Path.GetFileNameWithoutExtension(projectPath);
            }
            else
            {
                projectPath = Path.Combine(Directory.GetCurrentDirectory(), options.Project);
            }

            if (!Directory.Exists(projectPath))
            {
                Directory.CreateDirectory(projectPath);
            }

            throw new NotImplementedException();
            var project = CPlusPlusProject.Create(null, projectPath, options.Project);

            if (project != null)
            {
                Console.WriteLine("Project created successfully.");
                return 1;
            }
            else
            {
                Console.WriteLine("Unable to create project. May already exist.");
                return -1;
            }
        }

        static UInt32 PackValues(UInt16 a, UInt16 b)
        {
            return (UInt32)(a << 16 | b);
        }

        static void UnpackValues(UInt32 input, out UInt16 a, out UInt16 b)
        {
            a = (UInt16)(input >> 16);
            b = (UInt16)(input & 0x00FF);
        }

        static int Main(string[] args)
        {
            Platform.Platform.Initialise();

            PackageSources.InitialisePackageSources();

            var container = CompositionRoot.CreateContainer();

            Workspace.Instance = container.GetExportedValue<Workspace>();

            var packed = PackValues(3, 4);

            UInt16 a = 0;
            UInt16 b = 0;

            UnpackValues(packed, out a, out b);

            packed = PackValues(a, b);


            Console.WriteLine(string.Format("Avalon Build - {0} - {1}  - {2}", releaseName, version, Platform.Platform.PlatformIdentifier.ToString()));

            var result = Parser.Default.ParseArguments<AddOptions, RemoveOptions, AddReferenceOptions, BuildOptions, CleanOptions, CreateOptions, PackageOptions, TestOptions>(args).MapResult(
              (BuildOptions opts) => RunBuild(opts),
                (AddOptions opts) => RunAdd(opts),
                (AddReferenceOptions opts) => RunAddReference(opts),
                (PackageOptions opts) => RunInstallPackage(opts),
              (CleanOptions opts) => RunClean(opts),
              (CreateOptions opts) => RunCreate(opts),
              (RemoveOptions opts) => RunRemove(opts),
              (TestOptions opts) => RunTest(opts),
              errs => 1);

            return result - 1;
        }

        //static void GenerateTestProjects()
        //{
        //    if (!Directory.Exists(baseDir))
        //    {
        //        Directory.CreateDirectory(baseDir);
        //    }

        //    var project = new CPlusPlusProject();

        //    project.Name = "ArmSystem";

        //    project.Type = ProjectType.StaticLibrary;

        //    project.PublicIncludes.Add("./");

        //    project.Items.Add(new SourceFile { File = "allocator.c" });
        //    project.Items.Add(new SourceFile { File = "startup.c" });
        //    project.Items.Add(new SourceFile { File = "syscalls.c" });
        //    project.Items.Add(new SourceFile { File = "CPPSupport.cpp" });

        //    var projectDir = Path.Combine(baseDir, project.Name);

        //    if (!Directory.Exists(projectDir))
        //    {
        //        Directory.CreateDirectory(projectDir);
        //    }

        //    var projectFile = Path.Combine(projectDir, string.Format("{0}.{1}", project.Name, Solution.projectExtension));
        //    project.Serialize(projectFile);

        //    project = new CPlusPlusProject();

        //    project.Name = "STM32F4Cube";

        //    project.Type = ProjectType.StaticLibrary;

        //    project.PublicIncludes.Add("../STM32DiscoveryBootloader");
        //    project.PublicIncludes.Add("../STM32HalPlatform/USB/CustomHID");
        //    project.PublicIncludes.Add("./Drivers/STM32F4xx_HAL_Driver/Inc");
        //    project.PublicIncludes.Add("Middlewares/ST/STM32_USB_Device_Library/Core/Inc");
        //    project.PublicIncludes.Add("Drivers/CMSIS/Device/ST/STM32F4xx/Include");
        //    project.PublicIncludes.Add("Drivers/CMSIS/Include");

        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_adc.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_adc_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_can.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_cec.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_cortex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_crc.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_cryp.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_cryp_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_dac.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_dac_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_dcmi.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_dcmi_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_dma.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_dma_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_dma2d.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_eth.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_flash.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_flash_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_flash_ramfunc.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_fmpi2c.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_fmpi2c_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_gpio.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_hash.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_hash_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_hcd.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_i2c.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_i2c_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_i2s.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_i2s_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_irda.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_iwdg.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_ltdc.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_msp_template.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_nand.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_nor.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_pccard.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_pcd.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_pcd_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_pwr.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_pwr_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_qspi.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_rcc.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_rcc_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_rng.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_rtc.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_rtc_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_sai.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_sai_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_sd.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_sdram.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_smartcard.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_spdifrx.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_spi.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_sram.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_tim.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_tim_ex.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_uart.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_usart.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_hal_wwdg.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_ll_fmc.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_ll_fsmc.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_ll_sdmmc.c" });
        //    project.Items.Add(new SourceFile { File = "./Drivers/STM32F4xx_HAL_Driver/Src/stm32f4xx_ll_usb.c" });
        //    project.Items.Add(new SourceFile { File = "Middlewares/ST/STM32_USB_Device_Library/Core/Src/usbd_conf_template.c" });
        //    project.Items.Add(new SourceFile { File = "Middlewares/ST/STM32_USB_Device_Library/Core/Src/usbd_core.c" });
        //    project.Items.Add(new SourceFile { File = "Middlewares/ST/STM32_USB_Device_Library/Core/Src/usbd_ctlreq.c" });
        //    project.Items.Add(new SourceFile { File = "Middlewares/ST/STM32_USB_Device_Library/Core/Src/usbd_ioreq.c" });
        //    project.Items.Add(new SourceFile { File = "Drivers/CMSIS/Device/ST/STM32F4xx/Source/Templates/system_stm32f4xx.c" });


        //    projectDir = Path.Combine(baseDir, project.Name);

        //    if (!Directory.Exists(projectDir))
        //    {
        //        Directory.CreateDirectory(projectDir);
        //    }

        //    projectFile = Path.Combine(projectDir, string.Format("{0}.{1}", project.Name, Solution.projectExtension));
        //    project.Serialize(projectFile);

        //    project = new CPlusPlusProject();

        //    project.Name = "IntegratedDebugProtocol";

        //    project.Type = ProjectType.StaticLibrary;

        //    project.PublicIncludes.Add("./");

        //    project.Items.Add(new SourceFile { File = "IDP.cpp" });
        //    project.Items.Add(new SourceFile { File = "IDPPacket.cpp" });

        //    project.UnloadedReferences.Add(new Reference { Name = "Utils" });

        //    projectDir = Path.Combine(baseDir, project.Name);

        //    if (!Directory.Exists(projectDir))
        //    {
        //        Directory.CreateDirectory(projectDir);
        //    }

        //    projectFile = Path.Combine(projectDir, string.Format("{0}.{1}", project.Name, Solution.projectExtension));
        //    project.Serialize(projectFile);

        //    project = new CPlusPlusProject();

        //    project.Name = "CommonHal";

        //    project.Type = ProjectType.StaticLibrary;

        //    project.PublicIncludes.Add("./");

        //    project.UnloadedReferences.Add(new Reference { Name = "Utils" });

        //    project.Items.Add(new SourceFile { File = "II2C.cpp" });
        //    project.Items.Add(new SourceFile { File = "Interrupt.cpp" });
        //    project.Items.Add(new SourceFile { File = "IPort.cpp" });
        //    project.Items.Add(new SourceFile { File = "ISpi.cpp" });
        //    project.Items.Add(new SourceFile { File = "IUsbHidDevice.cpp" });
        //    project.SourceFiles.Add(new SourceFile { File = "SerialPort.cpp" });

        //    projectDir = Path.Combine(baseDir, project.Name);

        //    if (!Directory.Exists(projectDir))
        //    {
        //        Directory.CreateDirectory(projectDir);
        //    }

        //    projectFile = Path.Combine(projectDir, string.Format("{0}.{1}", project.Name, Solution.projectExtension));
        //    project.Serialize(projectFile);

        //    project = new CPlusPlusProject();

        //    project.Name = "STM32HalPlatform";

        //    project.Type = ProjectType.StaticLibrary;

        //    project.PublicIncludes.Add("./");
        //    project.PublicIncludes.Add("./USB");

        //    project.Items.Add(new SourceFile { File = "SignalGeneration/STM32FrequencyChannel.cpp" });
        //    project.Items.Add(new SourceFile { File = "SignalGeneration/STM32PwmChannel.cpp" });
        //    project.Items.Add(new SourceFile { File = "USB/CustomHID/usb_device.c" });
        //    project.Items.Add(new SourceFile { File = "USB/CustomHID/usbd_conf.c" });
        //    project.Items.Add(new SourceFile { File = "USB/CustomHID/usbd_customhid.c" });
        //    project.Items.Add(new SourceFile { File = "USB/CustomHID/usbd_desc.c" });
        //    project.Items.Add(new SourceFile { File = "USB/STM32UsbHidDevice.cpp" });
        //    project.Items.Add(new SourceFile { File = "STM32Adc.cpp" });
        //    project.Items.Add(new SourceFile { File = "STM32BootloaderService.cpp" });
        //    project.Items.Add(new SourceFile { File = "STM32InputCaptureChannel.cpp" });
        //    project.Items.Add(new SourceFile { File = "STM32QuadratureEncoder.cpp" });
        //    project.Items.Add(new SourceFile { File = "STM32Timer.cpp" });

        //    project.UnloadedReferences.Add(new Reference { Name = "CommonHal" });
        //    project.UnloadedReferences.Add(new Reference { Name = "STM32F4Cube" });
        //    project.UnloadedReferences.Add(new Reference { Name = "Utils" });

        //    projectDir = Path.Combine(baseDir, project.Name);

        //    if (!Directory.Exists(projectDir))
        //    {
        //        Directory.CreateDirectory(projectDir);
        //    }

        //    projectFile = Path.Combine(projectDir, string.Format("{0}.{1}", project.Name, Solution.projectExtension));
        //    project.Serialize(projectFile);

        //    project = new CPlusPlusProject();

        //    project.Name = "Utils";

        //    project.Type = ProjectType.StaticLibrary;

        //    project.PublicIncludes.Add("./");

        //    project.Items.Add(new SourceFile { File = "CRC.cpp" });
        //    project.Items.Add(new SourceFile { File = "Event.cpp" });
        //    project.Items.Add(new SourceFile { File = "PidController.cpp" });
        //    project.Items.Add(new SourceFile { File = "StraightLineFormula.cpp" });

        //    projectDir = Path.Combine(baseDir, project.Name);

        //    if (!Directory.Exists(projectDir))
        //    {
        //        Directory.CreateDirectory(projectDir);
        //    }

        //    projectFile = Path.Combine(projectDir, string.Format("{0}.{1}", project.Name, Solution.projectExtension));
        //    project.Serialize(projectFile);

        //    project = new CPlusPlusProject();

        //    project.Name = "GxInstrumentationHidDevice";

        //    project.Type = ProjectType.StaticLibrary;

        //    project.PublicIncludes.Add("./");

        //    project.Items.Add(new SourceFile { File = "GxInstrumentationHidDevice.cpp" });

        //    project.UnloadedReferences.Add(new Reference { Name = "CommonHal" });
        //    project.UnloadedReferences.Add(new Reference { Name = "IntegratedDebugProtocol" });
        //    project.UnloadedReferences.Add(new Reference { Name = "STM32F4Cube" });

        //    projectDir = Path.Combine(baseDir, project.Name);

        //    if (!Directory.Exists(projectDir))
        //    {
        //        Directory.CreateDirectory(projectDir);
        //    }

        //    projectFile = Path.Combine(projectDir, string.Format("{0}.{1}", project.Name, Solution.projectExtension));
        //    project.Serialize(projectFile);

        //    project = new CPlusPlusProject();

        //    project.Name = "Dispatcher";

        //    project.Type = ProjectType.StaticLibrary;

        //    project.PublicIncludes.Add("./");

        //    project.Items.Add(new SourceFile { File = "Dispatcher.cpp" });

        //    project.UnloadedReferences.Add(new Reference { Name = "Utils" });

        //    projectDir = Path.Combine(baseDir, project.Name);

        //    if (!Directory.Exists(projectDir))
        //    {
        //        Directory.CreateDirectory(projectDir);
        //    }

        //    projectFile = Path.Combine(projectDir, string.Format("{0}.{1}", project.Name, Solution.projectExtension));
        //    project.Serialize(projectFile);

        //    project = new CPlusPlusProject();

        //    project.Name = "GxBootloader";

        //    project.Type = ProjectType.StaticLibrary;

        //    project.PublicIncludes.Add("./");

        //    project.Items.Add(new SourceFile { File = "GxBootloader.cpp" });
        //    project.Items.Add(new SourceFile { File = "GxBootloaderHidDevice.cpp" });

        //    project.UnloadedReferences.Add(new Reference { Name = "IntegratedDebugProtocol" });
        //    project.UnloadedReferences.Add(new Reference { Name = "Utils" });
        //    project.UnloadedReferences.Add(new Reference { Name = "GxInstrumentationHidDevice" });
        //    project.UnloadedReferences.Add(new Reference { Name = "Dispatcher" });

        //    projectDir = Path.Combine(baseDir, project.Name);

        //    if (!Directory.Exists(projectDir))
        //    {
        //        Directory.CreateDirectory(projectDir);
        //    }

        //    projectFile = Path.Combine(projectDir, string.Format("{0}.{1}", project.Name, Solution.projectExtension));
        //    project.Serialize(projectFile);

        //    project = new CPlusPlusProject();

        //    project.Name = "STM32DiscoveryBootloader";

        //    project.Type = ProjectType.Executable;

        //    project.Includes.Add("./");

        //    project.UnloadedReferences.Add(new Reference { Name = "ArmSystem", GitUrl = "http://gxgroup.duia.eu/gx/ArmSystem.git", Revision = "HEAD" });
        //    project.UnloadedReferences.Add(new Reference { Name = "CommonHal" });
        //    project.UnloadedReferences.Add(new Reference { Name = "GxBootloader" });
        //    project.UnloadedReferences.Add(new Reference { Name = "STM32F4Cube" });
        //    project.UnloadedReferences.Add(new Reference { Name = "STM32HalPlatform" });

        //    project.Items.Add(new SourceFile { File = "startup_stm32f40xx.c" });
        //    project.Items.Add(new SourceFile { File = "main.cpp" });
        //    project.Items.Add(new SourceFile { File = "Startup.cpp" });
        //    project.Items.Add(new SourceFile { File = "DiscoveryBoard.cpp" });

        //    project.ToolChainArguments.Add("-mcpu=cortex-m4");
        //    project.ToolChainArguments.Add("-mthumb");
        //    project.ToolChainArguments.Add("-mfpu=fpv4-sp-d16");
        //    project.ToolChainArguments.Add("-mfloat-abi=hard");

        //    project.ToolChainArguments.Add("-fno-exceptions");
        //    project.ToolChainArguments.Add("-O3");
        //    project.ToolChainArguments.Add("-Os");

        //    project.CompilerArguments.Add("-ffunction-sections");
        //    project.CompilerArguments.Add("-fdata-sections");
        //    project.CompilerArguments.Add("-Wno-unknown-pragmas");

        //    project.CppCompilerArguments.Add("-fno-rtti");


        //    project.BuiltinLibraries.Add("m");
        //    project.BuiltinLibraries.Add("c_nano");
        //    project.BuiltinLibraries.Add("supc++_nano");
        //    project.BuiltinLibraries.Add("stdc++_nano");

        //    project.Defines.Add("__FPU_USED");
        //    project.Defines.Add("STM32F407xx");

        //    project.LinkerScript = "link.ld";

        //    project.BuildDirectory = "build";

        //    projectDir = Path.Combine(baseDir, project.Name);

        //    if (!Directory.Exists(projectDir))
        //    {
        //        Directory.CreateDirectory(projectDir);
        //    }

        //    projectFile = Path.Combine(projectDir, string.Format("{0}.{1}", project.Name, Solution.projectExtension));
        //    project.Serialize(projectFile);
        //}
    }
}
