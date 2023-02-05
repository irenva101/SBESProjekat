using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public enum AuditEventTypes
    {
        GenerationCertificationSuccess=0,
            RevocationCertSuccess=1,
            ConnectionSuccess=2,
            ConnectionFailed=3
    }

    public class AuditEvents
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceMgr
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceManager == null)
                    {
                        resourceManager = new ResourceManager(typeof(AuditEventFile).ToString(), Assembly.GetExecutingAssembly());
                    }
                    return resourceManager;
                }
            }
        }
        public static string GenerationCertificationSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.GenerationCertificationSuccess.ToString());
            }
        }
        public static string RevocationCertSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.RevocationCertSuccess.ToString());
            }
        }
        public static string ConnectionSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ConnectionSuccess.ToString());
            }
        }
        public static string ConnectionFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ConnectionFailed.ToString());
            }
        }

    }
}
