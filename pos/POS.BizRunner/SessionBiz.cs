using System;
using POS.BizRunner.Interfaces;
using POS.Dal;
using POS.Dal.Enums;

namespace POS.BizRunner
{
    public class SessionBiz : ISessionBiz
    {
        /// <summary>
        /// Creates the specified working time.
        /// </summary>
        /// <param name="workingTime">The working time.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public RecordSession Create(WorkingTime workingTime, int userId)
        {
            var session = GetInprogress(userId);
            if (session != null)
                return session;

            session = new RecordSession
            {
                UserId = userId,
                WorkingTime = workingTime.ToString(),
                CreatedDate = DateTime.Now,
                SessionStatus = SessionStatus.Inprogress.ToString()
            };

            session.Insert();
            return session;
        }

        public RecordSession GetById(int sessionId)
        {
            throw new System.NotImplementedException();
        }

        public RecordSession GetInprogress(int userId)
        {
            return RecordSession.GetInprogress(userId);
        }

        public void Complete(RecordSession session)
        {
            throw new System.NotImplementedException();
        }

        public void Stop(RecordSession session)
        {
            throw new System.NotImplementedException();
        }
    }
}