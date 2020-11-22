using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DSE;

// =====================================================================================================================

using var subscription = DiagnosticListener.AllListeners.Subscribe(new HttpDiagnosticSourceListener());
var t = Task.Run(async () => await new HttpClient().GetAsync("https://example.com"));
var c = -1;
while (true)
{
    Console.WriteLine($"{++c}");
    if (t.Status is TaskStatus.Faulted) Console.WriteLine($"{t.Exception?.Message}");

    Thread.Sleep(TimeSpan.FromSeconds(1));
}