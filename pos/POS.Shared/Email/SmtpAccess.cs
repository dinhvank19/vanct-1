using System.Net;
using System.Net.Mail;

namespace POS.Shared.Email
{
    public class SmtpAccess
    {
        #region Properties

        private readonly string _emailFrom;
        private readonly string _smtpHost;
        private readonly string _smtpPassword;
        private readonly string _smtpUsername;
        private readonly bool _ssl;

        #endregion

        #region Constructors

        public SmtpAccess(string smtpHost, string emailFrom, string smtpUsername, string smtpPassword, bool ssl)
        {
            _smtpHost = smtpHost;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
            _ssl = ssl;
            _emailFrom = emailFrom;
        }

        #endregion

        /// <summary>
        ///     Checks the connection to SmtpServer
        /// </summary>
        /// <returns></returns>
        public void CheckConnection()
        {
            //create smtp object
            using (var smtp = new SmtpClient(_smtpHost))
            {
                smtp.EnableSsl = _ssl;
                smtp.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                smtp.Send(new MailMessage(_emailFrom, _smtpUsername, "MailSender test smtp client", "MailSender test smtp client"));
            }
        }

        /// <summary>
        ///     Send the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public void Send(MailMessage message)
        {
            using (var smtp = new SmtpClient(_smtpHost))
            {
                smtp.EnableSsl = _ssl;
                smtp.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                smtp.Send(message);
            }
        }

        /// <summary>
        ///     Sends the specified body.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="to">To.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="emailSubject">The email subject.</param>
        public void Send(string body, string to, string displayName, string emailSubject)
        {
            using (var smtp = new SmtpClient(_smtpHost))
            {
                smtp.EnableSsl = _ssl;
                smtp.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                var message = new MailMessage {From = new MailAddress(_smtpUsername, displayName)};
                message.To.Add(to);
                message.Subject = emailSubject;
                message.Body = body;
                message.IsBodyHtml = true;
                smtp.Send(message);
            }
        }

        /// <summary>
        ///     Valids the parameter. Check smtpUsername
        /// </summary>
        /// <returns></returns>
        public bool ValidParameter()
        {
            if (string.IsNullOrEmpty(_smtpHost)
                || string.IsNullOrEmpty(_smtpUsername)
                || string.IsNullOrEmpty(_smtpPassword))
                return false;

            return true;
        }
    }
}