﻿namespace POS.Shared.Logging
{
    public class LoggingFactory
    {
        private static ILogger _logger;
        private static readonly object Lock = new object();

        /// <summary>
        ///     Gets the logger.
        /// </summary>
        /// <returns>ILogger instance</returns>
        public static ILogger GetLogger()
        {
            lock (Lock)
            {
                if (_logger == null)
                {
                    _logger = new Log4NetAdapter();
                }
            }

            return _logger;
        }
    }
}