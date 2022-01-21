namespace Hulk.Shared.Log
{
    public class LoggingFactory
    {
        private static ILogger _logger;

        private static readonly object Lock = new object();

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <returns>ILogger instance</returns>
        public static ILogger GetLogger()
        {
            lock (Lock)
            {
                return _logger ?? (_logger = new Log4NetAdapter());
            }
        }
    }
}