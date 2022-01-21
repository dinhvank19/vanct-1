using System.Collections.Generic;
using System.Net.Mail;
using POS.Shared.Properties;

namespace POS.Shared.Email
{
    public class Mailer
    {
        #region Properties

        private readonly string _displayName;
        private readonly SmtpAccess _sender;
        private readonly string _smtpUsername;

        #endregion

        #region Constructors

        public Mailer(string smtpHost, string stmpUsername, string smtpPassword, string displayName, string emailFrom, bool ssl)
        {
            _smtpUsername = stmpUsername;
            _displayName = displayName;
            _sender = new SmtpAccess(smtpHost, emailFrom, _smtpUsername, smtpPassword, ssl);
        }

        public Mailer()
        {
            _smtpUsername = Settings.Default.SmtpUsername;
            _displayName = Settings.Default.DisplayName;
            string smtpHost = Settings.Default.SmtpServer;
            string emailFrom = Settings.Default.SmtpEmailFrom;
            string smtpPassword = Settings.Default.SmtpPassword;
            bool ssl = Settings.Default.RequireSsl;
            _sender = new SmtpAccess(smtpHost, emailFrom, _smtpUsername, smtpPassword, ssl);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sends the specified receive email address.
        /// </summary>
        /// <param name="receiveEmailAddress">The receive email address.</param>
        /// <param name="body">The body.</param>
        /// <param name="subject">The subject.</param>
        public void Send(string receiveEmailAddress, string body, string subject)
        {
            //create email message
            var message = new MailMessage {From = new MailAddress(_smtpUsername, _displayName)};
            message.To.Add(receiveEmailAddress);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            _sender.Send(message);
        }

        /// <summary>
        /// Sends the specified receive email address.
        /// </summary>
        /// <param name="receiveEmailAddress">The receive email address.</param>
        /// <param name="body">The body.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="replyTo">The reply automatic.</param>
        public void Send(string receiveEmailAddress, string body, string subject, string replyTo)
        {
            //create email message
            var message = new MailMessage { From = new MailAddress(_smtpUsername, _displayName) };
            message.To.Add(receiveEmailAddress);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(replyTo);
            _sender.Send(message);
        }

        /// <summary>
        ///     Sends the specified receive email address.
        /// </summary>
        /// <param name="receiveEmailAddresses">The receive email address.</param>
        /// <param name="body">The body.</param>
        /// <param name="subject">The subject.</param>
        public void Send(IList<string> receiveEmailAddresses, string body, string subject)
        {
            //create email message
            var message = new MailMessage {From = new MailAddress(_smtpUsername, _displayName)};
            foreach (string emailTo in receiveEmailAddresses) message.To.Add(emailTo);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            _sender.Send(message);
        }

        /// <summary>
        /// Sends the specified receive email addresses.
        /// </summary>
        /// <param name="receiveEmailAddresses">The receive email addresses.</param>
        /// <param name="body">The body.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="replyTo">The reply automatic.</param>
        public void Send(IList<string> receiveEmailAddresses, string body, string subject, string replyTo)
        {
            //create email message
            var message = new MailMessage { From = new MailAddress(_smtpUsername, _displayName) };
            foreach (string emailTo in receiveEmailAddresses) message.To.Add(emailTo);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(replyTo);
            _sender.Send(message);
        }

        #endregion
    }
}