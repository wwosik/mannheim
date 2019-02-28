using System;

namespace Mannheim.XUnit
{
    public class TestOutputLoggerProviderScope : IDisposable
    {
        private readonly object state;

        public TestOutputLoggerProviderScope(object state)
        {
			this.state = state;
        }

        public void Dispose()
        {
        }
    }
}