using log4net;
using log4net.Config;
using POS.Shared.Properties;

namespace POS.Shared.Logging
{
    public class Log4NetAdapter : ILogger
    {
        private readonly ILog _log;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Log4NetAdapter" /> class.
        /// </summary>
        public Log4NetAdapter()
        {
            XmlConfigurator.Configure();
            _log = LogManager.GetLogger(Settings.Default.LogProviderName);
        }

        #region ILogger Members

        /// <summary>
        ///     Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            _log.Info(message);
        }

        public void Debug(string message)
        {
            _log.Debug(message);
        }

        public void Warn(string message)
        {
            _log.Warn(message);
        }

        public void Error(string message)
        {
            _log.Error(message);
        }

        #endregion
    }
}