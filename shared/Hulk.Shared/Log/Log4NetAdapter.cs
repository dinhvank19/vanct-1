using log4net;
using log4net.Config;

namespace Hulk.Shared.Log
{
    public class Log4NetAdapter : ILogger
    {
        private const string Log4Net = "Log4Net";
        private readonly ILog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetAdapter"/> class.
        /// </summary>
        public Log4NetAdapter()
        {
            XmlConfigurator.Configure();
            _log = LogManager.GetLogger(Log4Net);
        }

        #region ILogger Members

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Log(string message)
        {
            _log.Info(message);
        }

        #endregion
    }
}