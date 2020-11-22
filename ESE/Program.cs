using System;
using System.Threading;
using System.Threading.Tasks;
using ESE;

const int iterations = 1_000_000;
const int forSmall = 8;
const int forLarge = 85_000;

using (new GcEventListener())
{
    var t_factory = Task.Run(() =>
    {
        while (true)
        {
            Task.Run(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                var trash = new object[iterations];
                for (var i = 0; i < iterations; ++i)
                    trash[i] = i % 1000 == 0
                        ? new byte[forLarge]
                        : new byte[forSmall];
            }).GetAwaiter().GetResult();
        }
    });

    var c = -1;
    while (true)
    {
        Thread.Sleep(TimeSpan.FromSeconds(1));

        Console.WriteLine(++c);

        if (t_factory.Status is TaskStatus.Faulted) Console.WriteLine(t_factory.Exception?.Message);
    }
}