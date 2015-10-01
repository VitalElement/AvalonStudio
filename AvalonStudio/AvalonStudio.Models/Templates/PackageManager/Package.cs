namespace AvalonStudio.Models.PackageManager
{
    using AvalonStudio.Models.Tools.Compiler;
    using AvalonStudio.Models.Tools.Debuggers;
    using LibGit2Sharp;
    using LibGit2Sharp.Handlers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    [XmlInclude(typeof(ToolChainPackage))]
    [XmlInclude(typeof(ClangToolChainPackage))]
    [XmlInclude(typeof(DebugAdaptorPackage))]
    [XmlInclude(typeof(GccToolChainPackage))]
    [XmlInclude(typeof(BitThunderToolChainPackage))]
    [XmlInclude(typeof(MinGWToolChainPackage))]
    [XmlInclude(typeof(OpenOCDDebugAdaptorPackage))]
    [XmlInclude(typeof(JLinkDebugAdaptorPackage))]
    [XmlInclude(typeof(PIC32ToolChainPackage))]
    public class Package
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string RepoUri { get; set; }

        public static void GeneratePackage(Package package)
        {
            package.SerializeToXml("c:\\VEStudio\\Package.xml");
        }

        [XmlIgnore]
        protected virtual string LocalFolder
        {
            get
            {
                return Path.Combine(Path.Combine(AvalonStudioService.RepoBaseFolder, this.Name.Replace(" ", string.Empty)));
            }
        }

        public virtual bool Install()
        {
            return false;
        }

        public void Uninstall()
        {
            if (IsInstalled)
            {                
                //Directory.Delete(LocalFolder, true);
            }
        }

        public bool IsInstalled
        {
            get
            {
                bool result = false;

                if (LibGit2Sharp.Repository.IsValid(Path.Combine(LocalFolder, ".git")))
                {
                    result = true;
                }

                return result;
            }
        }


        public async Task<Package> DownloadPackage(TransferProgressHandler progressHandler)
        {
            await Task.Factory.StartNew(() =>
           {
               if (!IsInstalled)
               {
                   string result = LibGit2Sharp.Repository.Clone(this.RepoUri, LocalFolder, new LibGit2Sharp.CloneOptions() { Checkout = true, OnTransferProgress = progressHandler });
               }
               else
               {
                   var repo = new LibGit2Sharp.Repository(Path.Combine(LocalFolder, ".git"));

                   var result = repo.Network.Pull(new LibGit2Sharp.Signature("VEStudio", "catalog@vestudio", new DateTimeOffset(DateTime.Now)), new LibGit2Sharp.PullOptions() { FetchOptions = new FetchOptions() { OnTransferProgress = progressHandler } });
               }
           });

            if (File.Exists(Path.Combine(LocalFolder, AvalonStudioService.PackageIndexFileName)))
            {
                return Package.Load(Path.Combine(LocalFolder, AvalonStudioService.PackageIndexFileName));
            }
            else
            {
                return null;
            }
        }

        public void SerializeToXml(string location)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Package));

                var textWriter = new StreamWriter(location);

                serializer.Serialize(textWriter, this);

                textWriter.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static Package Load(string location)
        {
            Package result = null;

            try
            {
                var serializer = new XmlSerializer(typeof(Package));

                var textReader = new StreamReader(location);

                result = (Package)serializer.Deserialize(textReader);

                textReader.Close();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }
    }   

    public abstract class ToolChainPackage : Package
    {
        public abstract Type ToolChainType { get; }

        public List<string> IncludePaths { get; set; }
        public string PathVariable { get; set; }
        public string WorkingDirectory { get; set; }              
    }

    public abstract class DebugAdaptorPackage : Package
    {

    }

    public class JLinkDebugAdaptorPackage : DebugAdaptorPackage
    {
        public static Package GeneratePackage()
        {
            var package = new JLinkDebugAdaptorPackage()
            {
                Name = "JLink Debug Adaptor",
                Version = "4.9.6",
                Description = "Installs the JLink Debug interface to allow debugging of any project.",
                RepoUri = "https://github.com/danwalmsley/VEJlinkAdaptor.git",
                Executable = "JLinkGDBServerCL.exe"
            };


            package.SerializeToXml("c:\\VEStudio\\package.xml");

            return package;
        }

        public override bool Install()
        {
            bool result = false;

            var debugAdaptor = new JLinkDebugAdaptor();
            debugAdaptor.Location = Path.Combine(LocalFolder, "JLinkGDBServerCL.exe");
            var currentInstallation = AvalonStudioSettings.This.InstalledDebugAdaptors.FirstOrDefault((dba) => dba.GetType() == typeof(JLinkDebugAdaptor));

            if (currentInstallation != null)
            {
                var index = AvalonStudioSettings.This.InstalledDebugAdaptors.IndexOf(currentInstallation);
                AvalonStudioSettings.This.InstalledDebugAdaptors[index] = debugAdaptor;
            }
            else
            {
                AvalonStudioSettings.This.InstalledDebugAdaptors.Add(debugAdaptor);
            }

            AvalonStudioSettings.This.Save();

            result = true;

            return result;
        }

        public string Executable { get; set; }
    }

    public class OpenOCDDebugAdaptorPackage : DebugAdaptorPackage
    {
        public static Package GeneratePackage()
        {
            var package = new OpenOCDDebugAdaptorPackage()
            {
                Name = "OpenOCD Debug Adaptor",
                Version = "0.8.0",
                Description = "Installs the OpenOCD Debug interface to allow debugging of any project.",
                RepoUri = "https://github.com/danwalmsley/VEOpenOCDDebugger.git",
                Executable = "bin\\openocd-0.8.0"
            };


            package.SerializeToXml("c:\\VEStudio\\package.xml");

            return package;
        }

        public string Executable { get; set; }

        public override bool Install()
        {
            bool result = false;

            var debugAdaptor = new OpenOCDDebugAdaptor();
            debugAdaptor.Location = Path.Combine(LocalFolder, "bin\\openocd.exe");
            var currentOpenOCDInstallation = AvalonStudioSettings.This.InstalledDebugAdaptors.FirstOrDefault((dba) => dba.GetType() == typeof(OpenOCDDebugAdaptor));

            if (currentOpenOCDInstallation != null)
            {
                var index = AvalonStudioSettings.This.InstalledDebugAdaptors.IndexOf(currentOpenOCDInstallation);
                AvalonStudioSettings.This.InstalledDebugAdaptors[index] = debugAdaptor;
            }
            else
            {
                AvalonStudioSettings.This.InstalledDebugAdaptors.Add(debugAdaptor);
            }

            AvalonStudioSettings.This.Save();

            result = true;

            return result;
        }
    }

    public class ClangToolChainPackage : ToolChainPackage
    {
        public override Type ToolChainType
        {
            get
            {
                return typeof(ClangToolChain);
            }
        }

        public static Package GenerateClangPackage()
        {
            var package = new ClangToolChainPackage()
            {
                Name = "Clang ToolChain",
                Version = "3.5.0.0",
                Description = "Clang Tool chain for VEStudio",
                RepoUri = "https://github.com/danwalmsley/VEClangToolChain.git",
                IncludePaths = new List<string>()
                {
                    "lib\\clang\\3.5.0\\include",
                },
                WorkingDirectory = "bin"
            };

            //GeneratePackage (package);

            return package;
        }

        public override bool Install()
        {
            bool result = false;

            var toolChain = AvalonStudioSettings.This.ToolchainSettings.FirstOrDefault((tcs) => tcs.ToolChainRealType == typeof(ClangToolChain));

            if (toolChain == null)
            {
                toolChain = new ToolChainSettings(typeof(ClangToolChain));
                AvalonStudioSettings.This.ToolchainSettings.Add(toolChain);
                AvalonStudioSettings.This.Save();
            }


            toolChain.ToolChainLocation = this.LocalFolder;
            toolChain.IncludePaths = this.IncludePaths;
            result = true;

            AvalonStudioSettings.This.Save();

            return result;
        }
    }

    public class GccToolChainPackage : ToolChainPackage
    {
        public override Type ToolChainType
        {
            get
            {
                return typeof(GCCToolChain);
            }
        }

        public static Package GenerateGCCPackage()
        {
            var package = new GccToolChainPackage()
            {
                Name = "GCC ToolChain",
                Version = "4.8.0.0",
                Description = "GCC Toolchain for ARM Embedded Processors.",
                RepoUri = "https://github.com/danwalmsley/VEClangToolChain.git",
                IncludePaths = new List<string>()
                {
                    "arm-none-eabi\\4.8.4\\include",
                },
                WorkingDirectory = "bin"
            };

            //GeneratePackage (package);
            return package;
        }

        public override bool Install()
        {
            bool result = false;

            var toolChain = AvalonStudioSettings.This.ToolchainSettings.FirstOrDefault((tcs) => tcs.ToolChainRealType == typeof(GCCToolChain));

            if (toolChain == null)
            {
                toolChain = new ToolChainSettings(typeof(GCCToolChain));
                AvalonStudioSettings.This.ToolchainSettings.Add(toolChain);
                AvalonStudioSettings.This.Save();
            }

            toolChain.ToolChainLocation = this.LocalFolder;
            toolChain.IncludePaths = this.IncludePaths;
            result = true;

            AvalonStudioSettings.This.Save();

            return result;
        }
    }

    public class PIC32ToolChainPackage : ToolChainPackage
    {
        public override Type ToolChainType
        {
            get
            {
                return typeof(PicXC32ToolChain);
            }
        }

        public static Package GenerateGCCPackage()
        {
            var package = new PIC32ToolChainPackage()
            {
                Name = "PIC32ToolChain",
                Version = "4.9.1.0",
                Description = "GCC Toolchain for PIC32 Embedded Processors.",
                RepoUri = "https://github.com/danwalmsley/VEGCCPic32Toolchain.git",
                IncludePaths = new List<string>()
                {
                    "mipsel-mcp-elf\\include",
                },
                WorkingDirectory = "bin"
            };

            try
            {
                GeneratePackage(package);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return package;
        }

        public override bool Install()
        {
            bool result = false;

            var toolChain = AvalonStudioSettings.This.ToolchainSettings.FirstOrDefault((tcs) => tcs.ToolChainRealType == typeof(PicXC32ToolChain));

            if (toolChain == null)
            {
                toolChain = new ToolChainSettings(typeof(PicXC32ToolChain));
                AvalonStudioSettings.This.ToolchainSettings.Add(toolChain);
                AvalonStudioSettings.This.Save();
            }

            toolChain.ToolChainLocation = this.LocalFolder;
            toolChain.IncludePaths = this.IncludePaths;
            result = true;

            AvalonStudioSettings.This.Save();

            return result;
        }
    }

    public class BitThunderToolChainPackage : ToolChainPackage
    {
        public override Type ToolChainType
        {
            get
            {
                return typeof(BitThunderToolChain);
            }
        }

        public static Package GeneratePackage()
        {
            var package = new BitThunderToolChainPackage()
            {
                Name = "BT Toolchain",
                Version = "0.6.2.0",
                Description = "BitThunder Toolchain based on GCC 4.9.0 and Make for Win32",
                RepoUri = "https://github.com/danwalmsley/VEBitThunderToolChain.git",
                IncludePaths = new List<string>()
                {
                    "0.6.1\\lib\\gcc\\arm-eabi-bt\\4.9.0\\include",
                },
                WorkingDirectory = "0.6.1\\bin"
            };

            //GeneratePackage (package);

            return package;
        }

        public override bool Install()
        {
            bool result = false;

            var toolChain = AvalonStudioSettings.This.ToolchainSettings.FirstOrDefault((tcs) => tcs.ToolChainRealType == typeof(BitThunderToolChain));

            if (toolChain == null)
            {
                toolChain = new ToolChainSettings(typeof(BitThunderToolChain));
                AvalonStudioSettings.This.ToolchainSettings.Add(toolChain);
                AvalonStudioSettings.This.Save();
            }

            toolChain.ToolChainLocation = this.LocalFolder;
            toolChain.IncludePaths = this.IncludePaths;
            result = true;

            AvalonStudioSettings.This.Save();

            return result;
        }
    }

    public class MinGWToolChainPackage : ToolChainPackage
    {
        public override Type ToolChainType
        {
            get
            {
                return typeof(MinGWToolChain);
            }
        }

        public static Package GeneratePackage()
        {
            var package = new MinGWToolChainPackage()
            {
                Name = "mingw32 Toolchain",
                Version = "1.0.0",
                Description = "mingw32 Toolchain based on GCC 4.9.0 and Make for Win32",
                RepoUri = "https://github.com/danwalmsley/VEMingW32Toolchain.git",
                IncludePaths = new List<string>()
                {
                    "0.6.1\\lib\\gcc\\arm-eabi-bt\\4.9.0\\include",
                },
                WorkingDirectory = "0.6.1\\bin"
            };

            GeneratePackage(package);

            return package;
        }

        public override bool Install()
        {
            bool result = false;

            var toolChain = AvalonStudioSettings.This.ToolchainSettings.FirstOrDefault((tcs) => tcs.ToolChainRealType == typeof(MinGWToolChain));

            if (toolChain == null)
            {
                toolChain = new ToolChainSettings(typeof(MinGWToolChain));
                AvalonStudioSettings.This.ToolchainSettings.Add(toolChain);
                AvalonStudioSettings.This.Save();
            }

            toolChain.ToolChainLocation = this.LocalFolder;
            toolChain.IncludePaths = this.IncludePaths;
            result = true;

            // Install debugger.
            if (result)
            {
                var debugAdaptor = new LocalDebugAdaptor();
                var currentInstallation = AvalonStudioSettings.This.InstalledDebugAdaptors.FirstOrDefault((dba) => dba.GetType() == typeof(LocalDebugAdaptor));

                if (currentInstallation != null)
                {
                    var index = AvalonStudioSettings.This.InstalledDebugAdaptors.IndexOf(currentInstallation);
                    AvalonStudioSettings.This.InstalledDebugAdaptors[index] = debugAdaptor;
                }
                else
                {
                    AvalonStudioSettings.This.InstalledDebugAdaptors.Add(debugAdaptor);
                }

                AvalonStudioSettings.This.Save();
            }

            AvalonStudioSettings.This.Save();

            return result;
        }
    }
}
