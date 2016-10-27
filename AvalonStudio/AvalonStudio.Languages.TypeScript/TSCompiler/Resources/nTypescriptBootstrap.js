function tsTranspile(source) {
    var compilerOptions = {};
    var transpileResult = ts.transpileModule(source, { compilerOptions: compilerOptions, reportDiagnostics: true });
    return transpileResult;
}

function getTranspileResultCode(transpileResult) {
    return transpileResult.outputText;
}

function getTranspileResultDiagnostics(transpileResult) {
    return transpileResult.diagnostics;
}