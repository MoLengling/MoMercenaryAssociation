using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
//using MoMercenaryAssociation.Old;

namespace MoMercenaryAssociation
{
    static class MoUpdateConfigTool
    {
        private static int index;
        private static string CustomMenuId = "MMA_AutoCustomMenu_";
        public static void DoUpdate(string path, XmlNode RootNode)
        {
            XmlDocument Document = new XmlDocument();

            //添加根节点
            XmlElement MenuElement = Document.CreateElement("Submenus");
            Document.AppendChild(MenuElement);

            foreach (XmlNode Menu in RootNode.ChildNodes)
            {
                //添加Office节点
                XmlElement Office = Document.CreateElement("Office");
                MenuElement.AppendChild(Office);


                XmlElement xe = (XmlElement)Menu;

                //写ID
                string officeId = xe.GetAttribute("Id");
                if (officeId == "")
                {
                    MoLogs.Get().Debug("MoOld", "It seems that there is an office without Id:" + path);
                    officeId = "MoOldMenu_" + index;
                    index++;
                }
                Office.SetAttribute("Id", officeId);

                //写Text
                string Text = xe.GetAttribute("Text");
                Office.SetAttribute("Text", Text);

                //写Overwrite
                string WriteMode = xe.GetAttribute("WriteMode").ToLower();
                if (WriteMode == "remove")
                    WriteMode = "Remove";
                else if (WriteMode == "ensure" || WriteMode == "")
                    WriteMode = "";
                else
                    WriteMode = "OverWrite";
                Office.SetAttribute("OverwirteMode", WriteMode);

                //Condition
                string SettlementId = xe.GetAttribute("SettlementId");
                string CultureId = xe.GetAttribute("CultureId");
                string MapFactionId = xe.GetAttribute("MapFactionId");
                string CustomCondition = "";
                if (SettlementId != "default" && SettlementId != "")
                {
                    CustomCondition += "Settlement == " + SettlementId;
                }
                else
                {
                    CustomCondition += "true";
                }
                if (CultureId != "default" && CultureId != "")//确认文化
                {
                    CustomCondition += " and Culture == " + CultureId;
                }
                else
                {
                    CustomCondition += " and true";
                }
                if (MapFactionId != "default" && MapFactionId != "")//确认地图势力
                {
                    CustomCondition += " and MapFaction == " + MapFactionId;
                }
                else
                {
                    CustomCondition += " and true";
                }


                //ParentMenu
                string OptionText = xe.GetAttribute("OptionText");
                XmlElement Parents = Document.CreateElement("Parentmenus");
                Office.AppendChild(Parents);

                string Town = xe.GetAttribute("InTown").ToLower();
                string Village = xe.GetAttribute("InVillage").ToLower();
                string Castle = xe.GetAttribute("InCastle").ToLower();

                bool InTown = false, InVillage = false, InCastle = false;
                //当全部判断条件全部为默认、未设置时，Type有效。
                if ((Town == "" || Town == "default") &&
                    (Village == "" || Village == "default") &&
                    (Castle == "" || Castle == "default"))
                {
                    switch (xe.GetAttribute("SettlementType").ToLower())
                    {
                        case "town": InTown = true; break;
                        case "village": InVillage = true; break;
                        case "castle": InCastle = true; break;
                        default:
                            InTown = true;
                            InVillage = true;
                            InCastle = true;
                            break;
                    }
                }
                else
                {
                    InTown = Town == "true";
                    InVillage = Village == "true";
                    InCastle = Castle == "true";
                }

                if (InTown)
                {
                    XmlElement Parent = Document.CreateElement("Parentmenu");
                    Parent.SetAttribute("Id", "MMA_Town");
                    Parent.SetAttribute("OptionText", OptionText);
                    Parent.SetAttribute("ReturnText", MoStrings.DefaultReturnOptionText);
                    Parent.SetAttribute("Return", "true");
                    Parent.SetAttribute("EnableConditiosn", CustomCondition);
                    Parents.AppendChild(Parent);
                }
                if (InVillage)
                {
                    XmlElement Parent = Document.CreateElement("Parentmenu");
                    Parent.SetAttribute("Id", "MMA_Village");
                    Parent.SetAttribute("OptionText", OptionText);
                    Parent.SetAttribute("ReturnText", MoStrings.DefaultReturnOptionText);
                    Parent.SetAttribute("Return", "true");
                    Parent.SetAttribute("EnableConditiosn", CustomCondition);
                    Parents.AppendChild(Parent);
                }
                if (InCastle)
                {
                    XmlElement Parent = Document.CreateElement("Parentmenu");
                    Parent.SetAttribute("Id", "MMA_Castle");
                    Parent.SetAttribute("OptionText", OptionText);
                    Parent.SetAttribute("ReturnText", MoStrings.DefaultReturnOptionText);
                    Parent.SetAttribute("Return", "true");
                    Parent.SetAttribute("EnableConditiosn", CustomCondition);
                    Parents.AppendChild(Parent);
                }

                
                //Mercenaries
                string officeType = xe.GetAttribute("OfficeType").ToLower();
                XmlElement Mercenaries = Document.CreateElement("Characters");
                Office.AppendChild(Mercenaries);

                foreach (XmlNode MercenaryNode in Menu.ChildNodes)
                {
                    XmlElement Mercenary = Document.CreateElement("Character");
                    Mercenaries.AppendChild(Mercenary);

                    //MercenaryId
                    XmlElement MercenaryNodeElement = (XmlElement)MercenaryNode;
                    string MercenaryId = MercenaryNodeElement.GetAttribute("Id");
                    Mercenary.SetAttribute("Id", MercenaryId);


                    int cost, min, max;
                    if (int.TryParse(MercenaryNodeElement.GetAttribute("Cost"), out cost) == false || cost <= 0)
                        cost = 1000;
                    if (int.TryParse(MercenaryNodeElement.GetAttribute("AmountMin"), out min) == false)
                    {
                        min = 1;
                    }
                    if (int.TryParse(MercenaryNodeElement.GetAttribute("AmountMax"), out max) == false)
                    {
                        max = 5;
                    }


                    bool isMercenary, isPrisoner, canCreateHero;
                    string isMercenaryStr = MercenaryNodeElement.GetAttribute("IsMercenary");
                    string isPrisonerStr = MercenaryNodeElement.GetAttribute("IsPrisoner");
                    canCreateHero = MercenaryNodeElement.GetAttribute("CanCreateHero").ToLower() == "true";

                    
                    if (officeType == "mercenary" && isMercenaryStr != "false")
                    {
                        isMercenary = true;
                    }
                    else
                    {
                        isMercenary = false;
                    }
                    if (officeType == "prisoner" && isPrisonerStr != "false")
                    {
                        isPrisoner = true;
                    }
                    else
                    {
                        isPrisoner = false;
                    }
                    if (MercenaryNodeElement.Name == "Mercenary") { isMercenary = true; }
                    if (MercenaryNodeElement.Name == "Prisoner") { isPrisoner = true; }
                    if (MercenaryNodeElement.Name == "Hero" || MercenaryNodeElement.Name == "Companion") { canCreateHero = true; }

                    if (isMercenary)
                    {
                        XmlElement MethodList = Document.CreateElement("MethodList");
                        MethodList.SetAttribute("RecruitType", "Soldier");
                        Mercenary.AppendChild(MethodList);

                        XmlElement MinRecruit = Document.CreateElement("Recruit");
                        MinRecruit.SetAttribute("Amount", min.ToString());
                        MinRecruit.SetAttribute("Gold", (cost * min).ToString());
                        MethodList.AppendChild(MinRecruit);

                        XmlElement MaxRecruit = Document.CreateElement("Recruit");
                        MaxRecruit.SetAttribute("Amount", max.ToString());
                        MaxRecruit.SetAttribute("Gold", (cost * max).ToString());
                        MethodList.AppendChild(MaxRecruit);
                    }

                    if (isPrisoner)
                    {
                        XmlElement MethodList = Document.CreateElement("MethodList");
                        MethodList.SetAttribute("RecruitType", "Prisoner");
                        Mercenary.AppendChild(MethodList);

                        XmlElement MinRecruit = Document.CreateElement("Recruit");
                        MinRecruit.SetAttribute("Amount", min.ToString());
                        MinRecruit.SetAttribute("Gold", (cost * min).ToString());
                        MethodList.AppendChild(MinRecruit);

                        XmlElement MaxRecruit = Document.CreateElement("Recruit");
                        MaxRecruit.SetAttribute("Amount", max.ToString());
                        MaxRecruit.SetAttribute("Gold", (cost * max).ToString());
                        MethodList.AppendChild(MaxRecruit);
                    }

                    if(canCreateHero)
                    {
                        XmlElement MethodList = Document.CreateElement("MethodList");
                        MethodList.SetAttribute("RecruitType", "Companion");
                        Mercenary.AppendChild(MethodList);

                        XmlElement MinRecruit = Document.CreateElement("Recruit");
                        MinRecruit.SetAttribute("Amount", "1");
                        MinRecruit.SetAttribute("Gold", (cost*10).ToString());
                        MethodList.AppendChild(MinRecruit);
                    }
                }

            }

            Document.Save(path);
        }
    }
}
