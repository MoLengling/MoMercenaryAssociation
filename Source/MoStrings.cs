using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using TaleWorlds.Localization;

namespace MoMercenaryAssociation
{
    public static class MoStrings
    {
        public static string OnModuleLoaded ="OnModuleLoaded";
        public static string ToTownOptionText = "ToTownOptionText";
        public static string LeaveTownOptionText = "LeaveTownOptionText";
        public static string TownText = "TownText";
        public static string ReturnToTownHallText = "ReturnToTownHallText";
        public static string ToCastleOptionText = "ToCastleOptionText";
        public static string LeaveCastleOptionText = "LeaveCastleOptionText";
        public static string CastleText = "CastleText";
        public static string ReturnToCastleHallText = "ReturnToCastleHallText";
        public static string ToVillageOptionText = "ToVillageOptionText";
        public static string LeaveVillageOptionText = "LeaveVillageOptionText";
        public static string VillageText = "VillageText";
        public static string ReturnToVillageHallText = "ReturnToVillageHallText";
        public static string ReturnToSettlementText = "ReturnToSettlementText";
        public static string UnableToCoverTheCost_Gold = "UnableToCoverTheCost_Gold";
        public static string UnableToCoverTheCost_Prosperity = "UnableToCoverTheCost_Prosperity";
        public static string UnableToCoverTheCost_Hearth = "UnableToCoverTheCost_Hearth";
        public static string UnableToCoverTheCost_Militia = "UnableToCoverTheCost_Militia";
        public static string UnableToCoverTheCost_Renown = "UnableToCoverTheCost_Renown";
        public static string UnusableCondition_Gold = "UnusableCondition_Gold";
        public static string UnusableCondition_Prosperity = "UnusableCondition_Prosperity";
        public static string UnusableCondition_Hearth = "UnusableCondition_Hearth";
        public static string UnusableCondition_Militia = "UnusableCondition_Militia";
        public static string UnusableCondition_Renown = "UnusableCondition_Renown";
        public static string DefaultRecruitOptionText = "DefaultRecruitOptionText";
        public static string DefaultReturnOptionText = "DefaultReturnOptionText";

        public static string Soldier = "Soldier";
        public static string Prisoner = "Prisoner";
        public static string Companion = "Companion";
        public static string Family = "Family";
        public static string Translator="Translator";

        public static string Gold = "Gold";
        public static string Prosperity = "Prosperity";
        public static string Hearth = "Hearth";
        public static string Militia = "Militia";
        public static string Renown = "Renown";

        static List<string> MMAStrings;
        //string 

        public static void StartInit()
        {
            MMAStrings = new List<string>();
            MMAStrings.Add(OnModuleLoaded);
            MMAStrings.Add(ToTownOptionText);
            MMAStrings.Add(LeaveTownOptionText);
            MMAStrings.Add(TownText);
            MMAStrings.Add(ReturnToTownHallText);
            MMAStrings.Add(ToCastleOptionText);
            MMAStrings.Add(LeaveCastleOptionText);
            MMAStrings.Add(CastleText);
            MMAStrings.Add(ReturnToCastleHallText);
            MMAStrings.Add(ToVillageOptionText);
            MMAStrings.Add(LeaveVillageOptionText);
            MMAStrings.Add(VillageText);
            MMAStrings.Add(ReturnToVillageHallText);
            MMAStrings.Add(ReturnToSettlementText);
            MMAStrings.Add(UnableToCoverTheCost_Gold);
            MMAStrings.Add(UnableToCoverTheCost_Prosperity);
            MMAStrings.Add(UnableToCoverTheCost_Hearth);
            MMAStrings.Add(UnableToCoverTheCost_Militia);
            MMAStrings.Add(UnableToCoverTheCost_Renown);
            MMAStrings.Add(UnusableCondition_Gold);
            MMAStrings.Add(UnusableCondition_Prosperity);
            MMAStrings.Add(UnusableCondition_Hearth);
            MMAStrings.Add(UnusableCondition_Militia);
            MMAStrings.Add(UnusableCondition_Renown);
            MMAStrings.Add(DefaultRecruitOptionText);
            MMAStrings.Add(Soldier);
            MMAStrings.Add(Prisoner);
            MMAStrings.Add(Companion);
            MMAStrings.Add(Family);
            MMAStrings.Add(Translator);
            MMAStrings.Add(Gold);
            MMAStrings.Add(Prosperity);
            MMAStrings.Add(Hearth);
            MMAStrings.Add(Militia);
            MMAStrings.Add(Renown);
            MMAStrings.Add(DefaultReturnOptionText); 
            MoLogs.Get().Log("MoString Start Read String from xml:" + MoSettings.MMAStringsPath);
            MoXmlReader.Get(new XmlReaderParam(MoSettings.MMAStringsPath, "MMAStrings", (XmlReaderParam.AfterCreateReader)ReadStrings));
            MoXmlReader.Get().StartReadXml();
        }

        private static void ReadStrings(XmlNode RootNode, bool Succeed)
        {
            try
            {
                MoLogs.Get().Log("MoString Init");
                OnModuleLoaded = RootNode.SelectSingleNode(OnModuleLoaded).InnerText;
                ToTownOptionText = RootNode.SelectSingleNode(ToTownOptionText).InnerText;
                LeaveTownOptionText = RootNode.SelectSingleNode(LeaveTownOptionText).InnerText;
                TownText = RootNode.SelectSingleNode(TownText).InnerText;
                ToCastleOptionText = RootNode.SelectSingleNode(ToCastleOptionText).InnerText;
                LeaveCastleOptionText = RootNode.SelectSingleNode(LeaveCastleOptionText).InnerText;
                CastleText = RootNode.SelectSingleNode(CastleText).InnerText;
                ToVillageOptionText = RootNode.SelectSingleNode(ToVillageOptionText).InnerText;
                LeaveVillageOptionText = RootNode.SelectSingleNode(LeaveVillageOptionText).InnerText;
                VillageText = RootNode.SelectSingleNode(VillageText).InnerText;
                ReturnToTownHallText = RootNode.SelectSingleNode(ReturnToTownHallText).InnerText;
                ReturnToCastleHallText = RootNode.SelectSingleNode(ReturnToCastleHallText).InnerText;
                ReturnToVillageHallText = RootNode.SelectSingleNode(ReturnToVillageHallText).InnerText;
                ReturnToSettlementText = RootNode.SelectSingleNode(ReturnToSettlementText).InnerText;
                UnableToCoverTheCost_Gold = RootNode.SelectSingleNode(UnableToCoverTheCost_Gold).InnerText;
                UnableToCoverTheCost_Prosperity = RootNode.SelectSingleNode(UnableToCoverTheCost_Prosperity).InnerText;
                UnableToCoverTheCost_Hearth = RootNode.SelectSingleNode(UnableToCoverTheCost_Hearth).InnerText;
                UnableToCoverTheCost_Militia = RootNode.SelectSingleNode(UnableToCoverTheCost_Militia).InnerText;
                UnableToCoverTheCost_Renown = RootNode.SelectSingleNode(UnableToCoverTheCost_Renown).InnerText;
                UnusableCondition_Gold = RootNode.SelectSingleNode(UnusableCondition_Gold).InnerText;
                UnusableCondition_Prosperity = RootNode.SelectSingleNode(UnusableCondition_Prosperity).InnerText;
                UnusableCondition_Hearth = RootNode.SelectSingleNode(UnusableCondition_Hearth).InnerText;
                UnusableCondition_Militia = RootNode.SelectSingleNode(UnusableCondition_Militia).InnerText;
                UnusableCondition_Renown = RootNode.SelectSingleNode(UnusableCondition_Renown).InnerText;
                Translator = RootNode.SelectSingleNode(Translator).InnerText;
                Soldier = RootNode.SelectSingleNode(Soldier).InnerText;
                Prisoner = RootNode.SelectSingleNode(Prisoner).InnerText;
                Companion = RootNode.SelectSingleNode(Companion).InnerText;
                Family = RootNode.SelectSingleNode(Family).InnerText;
                Gold = RootNode.SelectSingleNode(Gold).InnerText;
                Prosperity = RootNode.SelectSingleNode(Prosperity).InnerText;
                Hearth = RootNode.SelectSingleNode(Hearth).InnerText;
                Militia = RootNode.SelectSingleNode(Militia).InnerText;
                Renown = RootNode.SelectSingleNode(Renown).InnerText;
                DefaultRecruitOptionText = RootNode.SelectSingleNode(DefaultRecruitOptionText).InnerText;
                DefaultReturnOptionText = RootNode.SelectSingleNode(DefaultReturnOptionText).InnerText;
            }
            catch (Exception)
            {
                MoLogs.Get().Log("MoString", "Check if the strings is valid:");
                foreach (string s in MMAStrings)
                {
                    MoLogs.Get().Log("MoString", s);
                }
            }
        }
    }

    public static class MoTexts
    {
        // {=AAAA}testtesttesttest{On}
        public static string ToString(string SourceString,string InsertLocation,string InsertString)
        {
            //{=MoStrings1}Mo Mercenary Association has been loaded successfully.\n Author:{Author},\n Translator:{Translator} \n Have a good time!
            //拿到未替换的字符串。
            TextObject TargetText = new TextObject(SourceString);
            //拿到要替换进去的字符串。
            TextObject InsertText = new TextObject(InsertString);
            //把要替换的字符串当前语言的串拿到
            InsertString = InsertText.ToString();
            //把要替换的串设置进去。
            TargetText.SetTextVariable(InsertLocation, InsertString);
            //返回替换后的目标串
            return TargetText.ToString();
        }
        public static string ToString(string SourceString,string[][] InsertStrings)
        {
            TextObject TargetText = new TextObject(SourceString);
            foreach(string[] insert in InsertStrings)
            {
                TextObject InsertText = new TextObject(insert[1]);
                string InsertString = InsertText.ToString();
                TargetText.SetTextVariable(insert[0], InsertString);
            }
            return TargetText.ToString();
        }
    }
}
