using BenchmarkDotNet.Running;

namespace UUIDSerializationEntityBenchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<UUIDConverterBenchmarks>();
        }
    }
}