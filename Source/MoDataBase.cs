using System;
using System.Collections.Generic;
using System.Text;

namespace MoMercenaryAssociation
{
    public class MoDataBase
    {
        protected string stringId;
        public string StringId { get => stringId; private set => stringId = value; }
        public MoDataBase()
        {

        }

        public MoDataBase(string stringId)
        {
            this.stringId = stringId;
        }
    }
}
