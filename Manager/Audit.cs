using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class Audit : IDisposable
    {
        private static EventLog customLog = null;
        const string SourceName = "Manager.Audit";
        const string LogName = "MySecTest";

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }

        public static void GenerationCertificationSuccess(string username)
        {
            if (customLog != null)
            {
                string GenerationCertificationSuccess =
                    AuditEvents.GenerationCertificationSuccess;
                string message = String.Format(GenerationCertificationSuccess,
                    username);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.GenerationCertificationSuccess));
            }
        }

        public static void RevocationCertSuccess(string username)
        {
            if (customLog != null)
            {
                string RevocationCertSuccess =
                    AuditEvents.RevocationCertSuccess;
                string message = String.Format(RevocationCertSuccess,
                    username);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.RevocationCertSuccess));
            }
        }
        public static void ConnectionSuccess(string username1, string username2)
        {
            if (customLog != null)
            {
                string ConnectionSuccess =
                    AuditEvents.ConnectionSuccess;
                string message = String.Format(ConnectionSuccess,
                    username1, username2);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ConnectionSuccess));
            }
        }
        public static void ConnectionFailed()
        {
            if (customLog != null)
            {
                string ConnectionFailed =
                    AuditEvents.ConnectionFailed;
                string message = String.Format(ConnectionFailed);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ConnectionFailed));
            }
        }

        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
