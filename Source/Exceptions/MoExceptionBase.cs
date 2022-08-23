using System;
using System.Collections.Generic;
using System.Text;

namespace MoMercenaryAssociation.Exceptions
{
    class MoExceptionBase:Exception
    {
        private string ExceptionMessage;

        public MoExceptionBase(string exceptionMessage)
        {
            ExceptionMessage = exceptionMessage;
        }

        public override string ToString()
        {
            return ExceptionMessage + base.ToString();
        }
    }

    class MoEmptyTroopMethodException : MoExceptionBase
    {
        public MoEmptyTroopMethodException(string exceptionMessage) : base(exceptionMessage)
        {
        }
    }

    class MoNonexistentStringidException : MoExceptionBase
    {
        public MoNonexistentStringidException(string exceptionMessage) : base(exceptionMessage)
        {
        }
    }

    class MoSyntaxAnalysisException : MoExceptionBase
    {
        public MoSyntaxAnalysisException(string exceptionMessage) : base(exceptionMessage)
        {
        }
    }
}
