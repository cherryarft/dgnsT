using System;
using System.Diagnostics.Tracing;
using System.Text;

namespace ESE
{
    internal sealed class GcEventListener : EventListener
    {
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            if (eventSource.Name == "Microsoft-Windows-DotNETRuntime")
                EnableEvents(eventSource, EventLevel.Informational, (EventKeywords) 0x1);
        }

        protected override void OnEventWritten(EventWrittenEventArgs args)
        {
            if (args.EventName is not "GCStart_V2") return;
            if (args.Payload is null) return;
            if (args.PayloadNames is null) return;

            var sb = new StringBuilder();

            for (var i = 0; i < args.Payload.Count; ++i)
            {
                var value = args.Payload[i];

                if (value is null) continue;

                sb.Append(args.PayloadNames[i] switch
                {
                    "Depth" => $"Gen# {value}",
                    "Reason" => Convert.ToUInt32(value) switch
                    {
                        0x0 => "\tSmall object heap allocation.",
                        0x4 => "\tLarge object heap allocation.",

                        _ => string.Empty
                    },

                    _ => string.Empty
                });
            }

            Console.WriteLine(sb.ToString());
        }
    }
}