using POS.Dal;
using POS.Dal.Enums;

namespace POS.BizRunner.Interfaces
{
    public interface ISessionBiz
    {
        RecordSession Create(WorkingTime workingTime, int userId);

        RecordSession GetById(int sessionId);

        RecordSession GetInprogress(int userId);

        void Complete(RecordSession session);

        void Stop(RecordSession session);
    }
}