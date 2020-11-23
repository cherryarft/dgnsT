using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;

// =====================================================================================================================

switch (args[0])
{
    case "ps":
        PrintProcessStatus();
        break;
    case "printEvent":
        PrintRuntimeGcEvents(int.Parse(args[1]));
        break;
}

static void PrintRuntimeGcEvents(int processId)
{
    var providers = new List<EventPipeProvider>
    {
        new EventPipeProvider(
            "Microsoft-Windows-DotNETRuntime",
            EventLevel.Informational,
            (long) ClrTraceEventParser.Keywords.GC
        )
    };

    var client = new DiagnosticsClient(processId);
    using var session = client.StartEventPipeSession(providers, false);
    var source = new EventPipeEventSource(session.EventStream);

    source.Clr.All += (TraceEvent obj) => { Console.WriteLine(obj.EventName); };

    try
    {
        source.Process();
    }
    catch (Exception e)
    {
        Console.WriteLine("Error encountered while processing events");
        Console.WriteLine(e.ToString());
    }
}

static void PrintProcessStatus()
{
    DiagnosticsClient
        .GetPublishedProcesses()
        .Select(Process.GetProcessById).Where(_ => _ != null)
        .ToList().ForEach(_ => Console.WriteLine($"{_.Id} {_.ProcessName}"));
}