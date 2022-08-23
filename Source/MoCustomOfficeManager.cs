using MoMercenaryAssociation.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MoMercenaryAssociation
{
    class MoCustomOfficeManager : MoManagerBase<MoCustomOfficeManager, MoCustomOffice>
    {
        public Dictionary<string ,MoCustomOffice> Offices { get => Managements; }
        public MoCustomOfficeManager()
        {
        }

        //public void AddNewOffice(MoCustomOffice Office)
        //{
        //    Offices.Add(Office.MenuId, Office);
        //}

        public void ReadMMAConfigXml(MoCustomOffice NewOffice, XmlNode OfficeNode, string FilePath)
        {
            if (OfficeNode.Name != "Office")
                return;
            XmlNode MercenariesNode = OfficeNode.SelectSingleNode("Characters");
            if(MercenariesNode!=null)
            {
                MoLogs.Get().Log("Start Reading Characters of Office " + NewOffice.MenuId);
                foreach (XmlNode MercenaryNode in MercenariesNode.ChildNodes)
                {
                    MoMercenary NewMercenary;
                    { 
                        XmlAttribute attribute = MercenaryNode.Attributes["Id"];
                        if (attribute == null)
                        {
                            MoExceptionManager.Get().RecordException(new MoExceptionBase(""));
                        }
                        string Id = attribute.Value;
                        try
                        {
                            NewMercenary = new MoMercenary(Id);
                        }
                        catch (MoNonexistentStringidException MNSE)
                        {
                            continue;
                        } 
                    }
                    foreach(XmlNode MethodList in MercenaryNode.ChildNodes)
                    {
                        //读取招募条件，虽然条件在写的时候是分组的，但最终存储是以条存的
                        try
                        {
                            RecruitType recruitType = (RecruitType) Enum.Parse(typeof(RecruitType), MethodList.Attributes["RecruitType"].Value);
                            foreach(XmlNode RecruitNode in MethodList.ChildNodes)
                            {
                                int Amount;
                                try
                                {
                                    Amount = int.Parse(RecruitNode.Attributes["Amount"].Value);
                                }
                                catch(Exception e)
                                {
                                    continue;
                                }
                                string OptionText = "";
                                Dictionary<RecruitCostType, float> ListOfCost = new Dictionary<RecruitCostType, float>();
                                foreach (XmlAttribute attribute in RecruitNode.Attributes)
                                {
                                    try
                                    {
                                        switch (attribute.Name)
                                        {
                                            case "Gold": ListOfCost.Add(RecruitCostType.Gold, float.Parse(attribute.Value)); break;
                                            case "Prosperity": ListOfCost.Add(RecruitCostType.Prosperity, float.Parse(attribute.Value)); break;
                                            case "Hearth": ListOfCost.Add(RecruitCostType.Hearth, float.Parse(attribute.Value)); break;
                                            case "Militia": ListOfCost.Add(RecruitCostType.Militia, float.Parse(attribute.Value)); break;
                                            case "Renown": ListOfCost.Add(RecruitCostType.Renown, float.Parse(attribute.Value)); break;
                                            case "Text": OptionText = attribute.Value;break;
                                            default:break;
                                        }
                                    }
                                    catch(Exception ex)
                                    {
                                        MoLogs.Get().DebugAndLog(FilePath);
                                        MoExceptionManager.Get().RecordException(ex);
                                        continue;
                                    }
                                }
                                RecruitInfo newInfo = new RecruitInfo() {recruitType= recruitType, Amount = Amount, ListOfCost = ListOfCost, OptionText= OptionText };
                                NewMercenary.RecruitInfos.Add(newInfo);

                            }
                        }
                        catch(NullReferenceException NRE)
                        {
                            MoLogs.Get().DebugAndLog(FilePath);
                            MoExceptionManager.Get().RecordException(NRE);
                            continue;
                        }
                        catch(Exception ex)
                        {
                            MoExceptionManager.Get().RecordException(ex);
                            continue;
                        }
                    }
                    MoLogs.Get().Log("Add Mercenary " + NewMercenary.StringId + " in Office" + NewOffice.MenuId);
                    NewOffice.AddMercenary(NewMercenary);
                }
            }
            //MoLogs.Get().Log("Record Office " + NewOffice.MenuId + " in CustomOfficeManager");
            //AddNewOffice(NewOffice);
        }
    }
}
