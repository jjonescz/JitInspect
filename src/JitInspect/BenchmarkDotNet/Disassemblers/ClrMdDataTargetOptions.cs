using System.Diagnostics;
using Microsoft.Diagnostics.Runtime;

namespace BenchmarkDotNet.Disassemblers;

internal static class ClrMdDataTargetOptions
{
    const string MicrosoftSymbolServerPath = "http://msdl.microsoft.com/download/symbols";

    internal static DataTarget AttachToProcess(int processId)
    {
        using var currentProcess = Process.GetCurrentProcess();
        return processId == currentProcess.Id
            ? DataTarget.CreateSnapshotAndAttach(processId, Create())
            : DataTarget.AttachToProcess(processId, false, Create());
    }

    internal static DataTargetOptions Create()
    {
        var options = new DataTargetOptions();

        // CLRMD 4's net10 asset exposes SymbolPaths as init-only while netstandard has a normal setter.
        typeof(DataTargetOptions)
            .GetProperty(nameof(DataTargetOptions.SymbolPaths))?
            .SetValue(options, new[] { MicrosoftSymbolServerPath });

        return options;
    }
}