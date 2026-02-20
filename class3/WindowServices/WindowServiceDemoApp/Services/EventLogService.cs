using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowServiceDemoApp.Services
{
    // in .net framework services, writing to the event logs was done directly via 
    // System.Diagnostics.Eventlog.WriteEntry() method.
    // .net 8 we keep the same underlying API but wrap it behind an interface so it 
    // can be mocked in test and inject via DI in the worker class.
    public interface IEventLogService
    {
        void WriteInformation(string message);
        void WriteWarning(string message);
        void WriteError(string message);
    }
    public sealed class EventLogService : IEventLogService, IDisposable
    {
        private readonly EventLog _eventLog;

        public EventLogService()
        {
            const string source = "WindowServiceDemoApp";
            const string logName = "Application";

            if(!EventLog.SourceExists(source))
            {
                try
                {
                    EventLog.CreateEventSource(source, logName);
                }
                catch (System.Security.SecurityException)
                {
                    // Creating event sources requires admin, safe to ignore in dev
                }
            }
            _eventLog = new EventLog(logName) { Source = source };
        }

        public void Dispose() => _eventLog.Dispose();

        public void WriteError(string message)
          =>  _eventLog.WriteEntry(message, EventLogEntryType.Error);
        

        public void WriteInformation(string message)
         =>   _eventLog.WriteEntry(message, EventLogEntryType.Information);
        

        public void WriteWarning(string message)
         =>  _eventLog.WriteEntry(message, EventLogEntryType.Warning);
        
    }
}
