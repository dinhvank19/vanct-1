using System;
using System.Globalization;
using System.IO;
using System.Threading;
using DataSender.BO;
using Hulk.Shared;
using Hulk.Shared.Log;

namespace DataSender
{
    public class Program
    {
        private const string WorkingPath = "_working.json";
        private const string WorksPath = "_works.json";
        private const string BlockingPath = "_blocked.txt";
        private const string FilePassword = "26331";
        private const string FileName = "datashare.mdb";

        private static void Main()
        {
            try
            {
                // start process
                if (File.Exists(BlockingPath)) return;
                BlockingPath.WriteFile(DateTime.Now.ToString(CultureInfo.InvariantCulture));

                // initial importor object
                var importor = new ImportDataBiz(WorkingPath, WorksPath, FileName, FilePassword);

                // check finished work
                if (importor.CheckFinishedWorking())
                {
                    WorkingPath.DeleteFile();
                    var t = new Thread(importor.SyncWorks);
                    t.Start();
                }

                // check if report-user still offline
                if (!importor.CheckOnline())
                {
                    WorkingPath.DeleteFile();
                    //delete file
                    BlockingPath.DeleteFile();
                    return;
                }

                // send current data to server
                importor.SyncWorking();

                // end process
                BlockingPath.DeleteFile();
            }
            catch (Exception exception)
            {
                LoggingFactory.GetLogger().Log("[Main]" + exception);
                WorkingPath.DeleteFile();
                // end process
                BlockingPath.DeleteFile();
            }
        }
    }
}