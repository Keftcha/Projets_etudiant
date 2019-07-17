using System;
using NUnitLite;
using NUnit.Common;

namespace inProjects.TomlHelpers.Tests
{
    class Program
    {
        static int Main(string[] args)
        {
            return new AutoRun(typeof(Program).Assembly).Execute(args, new ExtendedTextWrapper(Console.Out), Console.In);
        }
    }
}