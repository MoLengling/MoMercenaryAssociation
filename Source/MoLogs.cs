using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.Library;

namespace MoMercenaryAssociation
{
    public class MoLogs:MoSingletonBase<MoLogs>
    {
        //一个专用的Writer
        private MoFileWriter LogWriter;

        public MoLogs():base()
        {
            LogWriter = MoFileWriter.GetNotSingletion(new MoFileWriterParam(MoSettings.MMALogFilePath, false));
            Log("MMA Log Module Start");
        }
        public void Log(string Message)
        {
            this.Log("LogTemp",Message);
        }
        public void Log(string Module, string Message)
        {
            LogWriter.WriteLine(Module + "::" + Message);
        }
        public void Debug(string Message)
        {
            Debug("DebugTemp",Message);
        }
        public void Debug(string Module, string Message)
        {
            InformationManager.DisplayMessage(new InformationMessage(Module + "::" + Message));
        }

        public void DebugAndLog(string Message)
        {
            Log(Message);
            Debug(Message);
        }
        public void DebugAndLog(string Module,string Message)
        {
            Log(Module, Message);
            Debug(Module, Message);
        }
    }
}
