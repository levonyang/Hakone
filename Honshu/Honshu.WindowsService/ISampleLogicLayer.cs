using System;

namespace Honshu.WindowsService
{
    /// <summary>
    /// This provides interfaces to the SampleLogicLayer class.
    /// </summary>
    internal interface ISampleLogicLayer : IDisposable
    {
        /// <summary>
        /// Runs the business logic here.
        /// </summary>
        void Run();
    }
}