using BenchmarkDotNet.Running;

namespace UUIDSerializationDapperBenchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<UUIDHandlerBenchmarks>();
        }
    }
}