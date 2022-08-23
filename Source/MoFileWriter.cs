using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MoMercenaryAssociation
{
    public class MoFileWriterParam:MoSingletonParam
    {
        public string FilePath;
        public bool Append;

        public MoFileWriterParam(string filePath,bool append = false)
        {
            FilePath = filePath;
            Append = append;
        }
    }
    public class MoFileWriter:MoSingletonBase<MoFileWriter>
    {
        private MoFileWriterParam Param;
        private StreamWriter Writer;
        protected override void Init(MoSingletonParam Param)
        {
            this.Param = (MoFileWriterParam)Param;
            if(this.Param != null)
            {
                if(MoFileSystemManager.Get().CreatePath(this.Param.FilePath))
                    Writer = new StreamWriter(this.Param.FilePath, this.Param.Append);
            }
        }

        public static MoFileWriter Get(string filePath,bool append)
        {
            if (SingletionObject != null)
            {
                if(!(SingletionObject.Param.FilePath.Equals(filePath) && SingletionObject.Param.Append == append && SingletionObject.Writer != null))
                {
                    if(SingletionObject.Writer != null)
                    {
                        SingletionObject.Writer.Close();
                    }
                    SingletionObject.Init(new MoFileWriterParam(filePath, append));
                }
                return Get();
            }
            return Get(new MoFileWriterParam(filePath,append));
        }

        public void Close()
        {
            Writer.Close();
        }

        public void WriteLine(string text)
        {
            if (Writer == null)
                MoLogs.Get().Debug("Warning: Writer is null");
            else
            {
                Writer.WriteLine(text);
                Writer.Flush();
            }
        }

        public void Write(string text)
        {
            if (Writer == null)
                MoLogs.Get().Debug("Warning: Writer is null");
            else
            { 
                Writer.Write(text);
                Writer.Flush();
            }

        }
    }
}
