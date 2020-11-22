using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace DSE
{
    public sealed class HttpDiagnosticSourceListener :
        IObserver<DiagnosticListener>,
        IObserver<KeyValuePair<string, object>>
    {
        public void OnNext(DiagnosticListener listener)
        {
            if (listener.Name is "HttpHandlerDiagnosticListener") listener.Subscribe(this);
        }

        public void OnNext(KeyValuePair<string, object> item)
            => Console.Write($"Event: {item.Key} \n{JsonSerializer.Serialize(item.Value)}");

        public void OnCompleted()
        {
            // ...
        }

        public void OnError(Exception error)
        {
            // ...
        }
    }
}