using Jint;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace IridiumIon.NTypeScript
{
    /// <summary>
    /// Provides a compiler service. To reduce overhead of loading the compiler, use one instance throughout your program.
    /// </summary>
    public class TypeScriptCompiler
    {
        public Engine JSEngine { get; set; }

        /// <summary>
        /// Creates a new TypeScript compiler. Be sure to call InitializeCompiler() or InitializeCompilerAsync() before using the compiler.
        /// </summary>
        public TypeScriptCompiler()
        {
            JSEngine = new Engine();
        }

        /// <summary>
        /// Compiles the TypeScript compiler
        /// </summary>
        public void InitializeCompiler()
        {
            //Load the compiler
            var compilerSourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AvalonStudio.Languages.TypeScript.TSCompiler.Resources.typescriptServices.js");
            var compilerSource = ReadStream(compilerSourceStream);
            //Compile the compiler
            JSEngine.Execute(compilerSource);

            //Load bootstrap
            var bootstrapSourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AvalonStudio.Languages.TypeScript.TSCompiler.Resources.nTypescriptBootstrap.js");
            var bootstrapSource = ReadStream(bootstrapSourceStream);
            JSEngine.Execute(bootstrapSource);
        }

        public string ReadStream(Stream stream)
        {
            var value = string.Empty;
            using (stream)
            {
                using (var sourceReader = new StreamReader(stream))
                {
                    value = sourceReader.ReadToEnd();
                }
            }
            return value;
        }

        /// <summary>
        /// Compiles the TypeScript compiler asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeCompilerAsync()
        {
            await Task.Run(() => InitializeCompiler());
        }

        public string Compile(string source)
        {
            var transpileFunc = JSEngine.GetValue("tsTranspile"); //defined in bootstrap
            var transpileResult = transpileFunc.Invoke(source);
            var getTranspileResultCodeFunc = JSEngine.GetValue("getTranspileResultCode");
            var outputCode = getTranspileResultCodeFunc.Invoke(transpileResult);
            //var outputDiagnostics = JSEngine.CallGlobalFunction<string>("getTranspileResultDiagnostics", transpileResult);
            return outputCode.AsString();
        }

        public async Task<string> CompileAsync(string source)
        {
            return await Task.Run(() => Compile(source));
        }
    }
}