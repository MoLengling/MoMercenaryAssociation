using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MoMercenaryAssociation
{

    class MoExceptionManager:MoSingletonBase<MoExceptionManager>
    {

        public MoExceptionManager():base()
        {
            
        }
        //直接抛出异常也应该记录
        public void ThrowException(Exception exception)
        {
            RecordException(exception,false);
            throw exception;
        }
        
        public void RecordException(Exception exception, bool printDebugMessage = true)
        {
            if (printDebugMessage)
            {
                //一切读写操作都应该得到管理，输出Log均经过MoLogs
                MoLogs.Get().DebugAndLog("[Warning]Exception",exception.ToString());
                return;
            }
            else
            {
                MoLogs.Get().Log("[Warning]Exception", exception.ToString());
            }
        }
    }
}
