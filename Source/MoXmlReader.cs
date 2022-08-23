using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using MoMercenaryAssociation.Exceptions;

namespace MoMercenaryAssociation
{
    public class XmlReaderParam : MoSingletonParam
    {
        public string XmlPath;
        public string RootNodeName;
        public delegate void AfterCreateReader(XmlNode RootNode,bool Sueeccd);
        public AfterCreateReader Callback;
        public XmlReaderParam(string xmlPath, string rootNodeName, AfterCreateReader callback = null)
        {
            XmlPath = xmlPath;
            RootNodeName = rootNodeName;
            Callback = callback;
        }
    }
    public class MoXmlReader : MoSingletonBase<MoXmlReader>
    {
        private XmlReaderParam Param;
        private XmlDocument Document;

        public override bool IsReady()
        {
            return false;
        }
        override protected void Init(MoSingletonParam Param)
        {
            XmlReaderParam MyParam = (XmlReaderParam)Param;
            if (MyParam != null)
            {
                this.Param = MyParam;
                //StartReadXml();
            }
        }

        public void StartReadXml()
        {
            if(Param.XmlPath!="" && File.Exists(Param.XmlPath))
            {
                try
                {
                    XmlReaderSettings readerSettings = new XmlReaderSettings();
                    readerSettings.IgnoreComments = true;
                    XmlReader tempReader = XmlReader.Create(Param.XmlPath, readerSettings);
                    Document = new XmlDocument();
                    Document.Load(tempReader);
                    tempReader.Close();
                    if (Param.RootNodeName != "")
                    {
                        XmlNode root = Document.SelectSingleNode(Param.RootNodeName);
                        if (root != null)
                        {
                            if(Param.Callback!= null)
                                Param.Callback(root, true);

                        }
                        else
                        {
                            MoExceptionManager.Get().RecordException(new MoExceptionBase("the Root Node Name is Wrong"));
                            if (Param.Callback != null)
                                Param.Callback(root, false);
                        }
                    }
                }
                catch(XmlException xmlException)
                {
                    if (Param.Callback != null)
                        Param.Callback(default(XmlNode), false);
                    else
                        MoExceptionManager.Get().RecordException(xmlException);
                }
            }
            else
            {
                if (Param.Callback != null)
                    Param.Callback(default(XmlNode), false);
                MoLogs.Get().Log(Param.XmlPath + " File is not exist!!");
            }
        }
    }
}
