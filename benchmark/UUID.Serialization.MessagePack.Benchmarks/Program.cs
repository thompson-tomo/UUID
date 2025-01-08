using BenchmarkDotNet.Running;

namespace UUIDSerializationMessagePackBenchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<UUIDFormatterBenchmarks>();
        }
    }
}