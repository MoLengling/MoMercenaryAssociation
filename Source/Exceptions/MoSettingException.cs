using System;
using System.Collections.Generic;
using System.Text;

namespace MoMercenaryAssociation.Exceptions
{
    class MoSettingException : MoExceptionBase
    {
        public MoSettingException(string exceptionMessage) : base(exceptionMessage)
        {
        }
    }

    class MoSettingParamException : MoSettingException
    {
        public MoSettingParamException(string exceptionMessage) : base(exceptionMessage)
        {
        }
    }
}
