using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace AvalonStudio.Languages.CSharp
{
    using System;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        public static Lazy<Type> LazyGetType(this Lazy<Assembly> lazyAssembly, string typeName)
        {
            if (lazyAssembly == null)
            {
                throw new ArgumentNullException(nameof(lazyAssembly));
            }

            return new Lazy<Type>(() =>
            {
                var type = lazyAssembly.Value.GetType(typeName);

                if (type == null)
                {
                    throw new InvalidOperationException($"Could not get type '{typeName}'");
                }

                return type;
            });
        }

        public static Lazy<MethodInfo> LazyGetMethod(this Lazy<Type> lazyType, string methodName)
        {
            if (lazyType == null)
            {
                throw new ArgumentNullException(nameof(lazyType));
            }

            return new Lazy<MethodInfo>(() =>
            {
                var type = lazyType.Value;
                var methodInfo = type.GetMethod(methodName);

                if (methodInfo == null)
                {
                    throw new InvalidOperationException($"Could not get method '{methodName}' on type '{type.FullName}'");
                }

                return methodInfo;
            });
        }

        public static Lazy<MethodInfo> LazyGetMethod(this Lazy<Type> lazyType, string methodName, BindingFlags bindingFlags)
        {
            if (lazyType == null)
            {
                throw new ArgumentNullException(nameof(lazyType));
            }

            return new Lazy<MethodInfo>(() =>
            {
                var type = lazyType.Value;
                var methodInfo = type.GetMethod(methodName, bindingFlags);

                if (methodInfo == null)
                {
                    throw new InvalidOperationException($"Could not get method '{methodName}' on type '{type.FullName}'");
                }

                return methodInfo;
            });
        }

        public static MethodInfo GetMethod(this Lazy<Type> lazyType, string methodName)
        {
            if (lazyType == null)
            {
                throw new ArgumentNullException(nameof(lazyType));
            }

            var type = lazyType.Value;
            var methodInfo = type.GetMethod(methodName);

            if (methodInfo == null)
            {
                throw new InvalidOperationException($"Could not get method '{methodName}' on type '{type.FullName}'");
            }

            return methodInfo;
        }

        public static MethodInfo GetMethod(this Lazy<Type> lazyType, string methodName, BindingFlags bindingFlags)
        {
            if (lazyType == null)
            {
                throw new ArgumentNullException(nameof(lazyType));
            }

            var type = lazyType.Value;
            var methodInfo = type.GetMethod(methodName, bindingFlags);

            if (methodInfo == null)
            {
                throw new InvalidOperationException($"Could not get method '{methodName}' on type '{type.FullName}'");
            }

            return methodInfo;
        }

        public static object CreateInstance(this Lazy<Type> lazyType, params object[] args)
        {
            if (lazyType == null)
            {
                throw new ArgumentNullException(nameof(lazyType));
            }

            return Activator.CreateInstance(lazyType.Value, args);
        }

        public static T Invoke<T>(this MethodInfo methodInfo, object obj, object[] args)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            return (T)methodInfo.Invoke(obj, args);
        }

        public static T Invoke<T>(this Lazy<MethodInfo> lazyMethodInfo, object obj, object[] args)
        {
            if (lazyMethodInfo == null)
            {
                throw new ArgumentNullException(nameof(lazyMethodInfo));
            }

            return (T)lazyMethodInfo.Value.Invoke(obj, args);
        }

        public static T InvokeStatic<T>(this MethodInfo methodInfo, object[] args)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            return (T)methodInfo.Invoke(null, args);
        }

        public static T InvokeStatic<T>(this Lazy<MethodInfo> lazyMethodInfo, object[] args)
        {
            if (lazyMethodInfo == null)
            {
                throw new ArgumentNullException(nameof(lazyMethodInfo));
            }

            return lazyMethodInfo.Value.InvokeStatic<T>(args);
        }

        public static object InvokeStatic(this MethodInfo methodInfo, object[] args)
        {
            return methodInfo.Invoke(null, args);
        }

        public static object InvokeStatic(this Lazy<MethodInfo> lazyMethodInfo, object[] args)
        {
            return lazyMethodInfo.InvokeStatic(args);
        }

        public static Lazy<FieldInfo> LazyGetField(this Lazy<Type> lazyType, string fieldName, BindingFlags bindingFlags)
        {
            if (lazyType == null)
            {
                throw new ArgumentNullException(nameof(lazyType));
            }

            return new Lazy<FieldInfo>(() =>
            {
                var type = lazyType.Value;
                var field = type.GetField(fieldName, bindingFlags);

                if (field == null)
                {
                    throw new InvalidOperationException($"Could not get method '{fieldName}' on type '{type.FullName}'");
                }

                return field;
            });
        }

        public static T GetValue<T>(this Lazy<FieldInfo> lazyFieldInfo, object o)
        {
            return (T)lazyFieldInfo.Value.GetValue(o);
        }
    }

    public interface IAssemblyLoader
    {
        Assembly Load(AssemblyName name);

        IReadOnlyList<Assembly> LoadAllFrom(string folderPath);

        Assembly LoadFrom(string assemblyPath);
    }

    public static class IAssemblyLoaderExtensions
    {
        public static Lazy<Assembly> LazyLoad(this IAssemblyLoader loader, string assemblyName)
        {
            return new Lazy<Assembly>(() => loader.Load(assemblyName));
        }

        public static Assembly Load(this IAssemblyLoader loader, string name)
        {
            var assemblyName = name;
            if (name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                assemblyName = name.Substring(0, name.Length - 4);
            }

            return loader.Load(new AssemblyName(assemblyName));
        }

        public static IEnumerable<Assembly> Load(this IAssemblyLoader loader, params string[] assemblyNames)
        {
            foreach (var name in assemblyNames)
            {
                yield return Load(loader, name);
            }
        }
    }


    internal class AssemblyLoader : IAssemblyLoader
    {

        public AssemblyLoader()
        {

        }

        public Assembly Load(AssemblyName name)
        {
            Assembly result;
            try
            {
                result = Assembly.Load(name);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public IReadOnlyList<Assembly> LoadAllFrom(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath)) return Array.Empty<Assembly>();

            try
            {
                var assemblies = new List<Assembly>();

                foreach (var filePath in Directory.EnumerateFiles(folderPath, "*.dll"))
                {
                    var assembly = LoadFrom(filePath);
                    if (assembly != null)
                    {
                        assemblies.Add(assembly);
                    }
                }

                return assemblies;
            }
            catch
            {
                return Array.Empty<Assembly>();
            }
        }

        public Assembly LoadFrom(string assemblyPath)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath)) return null;

            Assembly assembly = null;

            try
            {
                assembly = Assembly.LoadFrom(assemblyPath);
            }
            catch
            {
            }

            return assembly;
        }
    }

    internal static class Configuration
    {
        public const string RoslynVersion = "2.4.0.0";
        public const string RoslynPublicKeyToken = "31bf3856ad364e35";

        public readonly static string RoslynFeatures = GetRoslynAssemblyFullName("Microsoft.CodeAnalysis.Features");
        public readonly static string RoslynCSharpFeatures = GetRoslynAssemblyFullName("Microsoft.CodeAnalysis.CSharp.Features");
        public readonly static string RoslynWorkspaces = GetRoslynAssemblyFullName("Microsoft.CodeAnalysis.Workspaces");

        private static string GetRoslynAssemblyFullName(string name)
        {
            return $"{name}, Version={RoslynVersion}, Culture=neutral, PublicKeyToken={RoslynPublicKeyToken}";
        }
    }


    public class MetadataHelper
    {
        private readonly IAssemblyLoader _loader;
        private readonly Lazy<Assembly> _featureAssembly;
        private readonly Lazy<Assembly> _csharpFeatureAssembly;
        private readonly Lazy<Assembly> _workspaceAssembly;
        private readonly Lazy<Type> _csharpMetadataAsSourceService;
        private readonly Lazy<Type> _symbolKey;
        private readonly Lazy<Type> _metadataAsSourceHelper;
        private readonly Lazy<MethodInfo> _getLocationInGeneratedSourceAsync;
        private Dictionary<string, Document> _metadataDocumentCache = new Dictionary<string, Document>();

        private const string CSharpMetadataAsSourceService = "Microsoft.CodeAnalysis.CSharp.MetadataAsSource.CSharpMetadataAsSourceService";
        private const string SymbolKey = "Microsoft.CodeAnalysis.SymbolKey";
        private const string MetadataAsSourceHelpers = "Microsoft.CodeAnalysis.MetadataAsSource.MetadataAsSourceHelpers";
        private const string GetLocationInGeneratedSourceAsync = "GetLocationInGeneratedSourceAsync";
        private const string AddSourceToAsync = "AddSourceToAsync";
        private const string Create = "Create";
        private const string MetadataKey = "$Metadata$";

        public MetadataHelper(IAssemblyLoader loader)
        {
            _loader = loader;
            _featureAssembly = _loader.LazyLoad(Configuration.RoslynFeatures);
            _csharpFeatureAssembly = _loader.LazyLoad(Configuration.RoslynCSharpFeatures);
            _workspaceAssembly = _loader.LazyLoad(Configuration.RoslynWorkspaces);

            _csharpMetadataAsSourceService = _csharpFeatureAssembly.LazyGetType(CSharpMetadataAsSourceService);
            _symbolKey = _workspaceAssembly.LazyGetType(SymbolKey);
            _metadataAsSourceHelper = _featureAssembly.LazyGetType(MetadataAsSourceHelpers);

            _getLocationInGeneratedSourceAsync = _metadataAsSourceHelper.LazyGetMethod(GetLocationInGeneratedSourceAsync);
        }

        public Document FindDocumentInMetadataCache(string fileName)
        {
            if (_metadataDocumentCache.TryGetValue(fileName, out var metadataDocument))
            {
                return metadataDocument;
            }

            return null;
        }

        public string GetSymbolName(ISymbol symbol)
        {
            var topLevelSymbol = symbol.GetTopLevelContainingNamedType();
            return GetTypeDisplayString(topLevelSymbol);
        }

        public async Task<(Document metadataDocument, string documentPath)> GetAndAddDocumentFromMetadata(Project project, ISymbol symbol, CancellationToken cancellationToken = new CancellationToken())
        {
            var fileName = GetFilePathForSymbol(project, symbol);

            Project metadataProject;

            // since submission projects cannot have new documents added to it
            // we will use a separate project to hold metadata documents
            if (project.IsSubmission)
            {
                metadataProject = project.Solution.Projects.FirstOrDefault(x => x.Name == MetadataKey);
                if (metadataProject == null)
                {
                    metadataProject = project.Solution.AddProject(MetadataKey, $"{MetadataKey}.dll", LanguageNames.CSharp)
                        .WithCompilationOptions(project.CompilationOptions)
                        .WithMetadataReferences(project.MetadataReferences);
                }
            }
            else
            {
                // for regular projects we will use current project to store metadata
                metadataProject = project;
            }

            if (!_metadataDocumentCache.TryGetValue(fileName, out var metadataDocument))
            {
                var topLevelSymbol = symbol.GetTopLevelContainingNamedType();

                var temporaryDocument = metadataProject.AddDocument(fileName, string.Empty);
                var service = _csharpMetadataAsSourceService.CreateInstance(temporaryDocument.Project.LanguageServices);
                var method = _csharpMetadataAsSourceService.GetMethod(AddSourceToAsync);

                var documentTask = method.Invoke<Task<Document>>(service, new object[] { temporaryDocument, topLevelSymbol, default(CancellationToken) });
                metadataDocument = await documentTask;

                _metadataDocumentCache[fileName] = metadataDocument;
            }

            return (metadataDocument, fileName);
        }

        public async Task<Location> GetSymbolLocationFromMetadata(ISymbol symbol, Document metadataDocument, CancellationToken cancellationToken = new CancellationToken())
        {
            var symbolKeyCreateMethod = _symbolKey.GetMethod(Create, BindingFlags.Static | BindingFlags.Public);
            var symboldId = symbolKeyCreateMethod.InvokeStatic(new object[] { symbol, cancellationToken });

            return await _getLocationInGeneratedSourceAsync.InvokeStatic<Task<Location>>(new object[] { symboldId, metadataDocument, cancellationToken });
        }

        private static string GetTypeDisplayString(INamedTypeSymbol symbol)
        {
            if (symbol.SpecialType != SpecialType.None)
            {
                var specialType = symbol.SpecialType;
                var name = Enum.GetName(typeof(SpecialType), symbol.SpecialType).Replace("_", ".");
                return name;
            }

            if (symbol.IsGenericType)
            {
                symbol = symbol.ConstructUnboundGenericType();
            }

            if (symbol.IsUnboundGenericType)
            {
                // TODO: Is this the best to get the fully metadata name?
                var parts = symbol.ToDisplayParts();
                var filteredParts = parts.Where(x => x.Kind != SymbolDisplayPartKind.Punctuation).ToArray();
                var typeName = new StringBuilder();
                foreach (var part in filteredParts.Take(filteredParts.Length - 1))
                {
                    typeName.Append(part.Symbol.Name);
                    typeName.Append(".");
                }
                typeName.Append(symbol.MetadataName);

                return typeName.ToString();
            }

            return symbol.ToDisplayString();
        }

        private static string GetFilePathForSymbol(Project project, ISymbol symbol)
        {
            var topLevelSymbol = symbol.GetTopLevelContainingNamedType();
            return $"$metadata$/Project/{Folderize(project.Name)}/Assembly/{Folderize(topLevelSymbol.ContainingAssembly.Name)}/Symbol/{Folderize(GetTypeDisplayString(topLevelSymbol))}.cs".Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }

        private static string Folderize(string path) => string.Join("/", path.Split('.'));
    }
}