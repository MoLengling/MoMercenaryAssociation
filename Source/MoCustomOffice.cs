using MoMercenaryAssociation.Exceptions;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Settlements;

namespace MoMercenaryAssociation
{
    class MoCustomOffice:MoGameMenu
    {
        public string OfficeId { get => MenuId; }
        public string OfficeText { get => MenuText; set => MenuText = value; }
        protected List<MoMercenary> Mercenaries;

        public MoCustomOffice() { }
        public MoCustomOffice(string menuid, string menuText) : base(menuid, menuText)
        {
            Mercenaries = new List<MoMercenary>();
        }
        public MoCustomOffice(string officeId, List<MoMercenary> mercenaries,string menuText):base(officeId,menuText)
        {
            Mercenaries = mercenaries;
        }

        public override string CreateSubmenusRecursively(string parentId, string OptionText)
        {
            string CreateMenuId = base.CreateSubmenusRecursively(parentId, OptionText);
            CreateRecruitOptions(CreateMenuId);

            return CreateMenuId;
        }
        public override string CreateSubmenusRecursively(string parentId)
        {
            string CreateMenuId = base.CreateSubmenusRecursively(parentId);
            CreateRecruitOptions(CreateMenuId);

            //创建Office回到定居点的直通车
            MoGameMenuHelper.Get().CreateGameMenuOption(CreateMenuId, MoStrings.ReturnToSettlementText, null,
                delegate
                {
                    if (Settlement.CurrentSettlement != null)
                        if (Settlement.CurrentSettlement.IsTown)
                            GameMenu.SwitchToMenu("town");
                        else if (Settlement.CurrentSettlement.IsCastle)
                            GameMenu.SwitchToMenu("castle");
                        else if (Settlement.CurrentSettlement.IsVillage)
                            GameMenu.SwitchToMenu("village");
                }
                );

            return CreateMenuId;
        }

        public MoMercenary[] GetMercenaries()
        {
            return Mercenaries.ToArray();
        }
        public void AddMercenary(MoMercenary mercenary)
        {
            Mercenaries.Add(mercenary);
        }
        public void CreateRecruitOptions(string GameMenuId)
        {
            foreach(MoMercenary mercenary in Mercenaries)
            {
                mercenary.CreateRecruitOption(GameMenuId);
            }
        }

    }

    class MoCustomOption
    {
        List<string> JudgeConditions;

        public MoCustomOption(string judgeConditions)
        {
            JudgeConditions = MoCustomOptionsTools.CreateJudgeConditions(judgeConditions);
        }

        public bool IsEnable(Settlement settlement)
        {
            if (MoSettings.Get().CheatMode)
                return true;

            Stack<string> AnalysisStack = new Stack<string>();

            foreach (string JudgeCondition in JudgeConditions)
            {
                switch (JudgeCondition)
                {
                    case "not":
                    case "!":
                        AnalysisStack.Push(AnalysisStack.Pop() == "true" ? "false" : "true");
                        break;
                    case "or":
                    case "||":
                        AnalysisStack.Push(AnalysisStack.Pop() == "true" || AnalysisStack.Pop() == "true" ? "true" : "false");
                        break;
                    case "and":
                    case "&&":
                        AnalysisStack.Push(AnalysisStack.Pop() == "true" && AnalysisStack.Pop() == "true" ? "true" : "false");
                        break;
                    case "==":
                    case "=":
                        {
                            string stringid = AnalysisStack.Pop();

                            switch (AnalysisStack.Pop())
                            {
                                case "settlement":
                                    AnalysisStack.Push(stringid.Equals(settlement.StringId).ToString().ToLower());
                                    break;
                                case "culture":
                                    AnalysisStack.Push((stringid.Equals(settlement.Culture.StringId)
                                        || stringid.Equals("player") && settlement.Culture == Hero.MainHero.Culture
                                        || stringid.Equals("playerclan") && settlement.Culture == Clan.PlayerClan.Culture
                                        ).ToString().ToLower());
                                    break;
                                case "clan":
                                    AnalysisStack.Push((stringid.Equals(settlement.OwnerClan.StringId)
                                        || (stringid.Equals("player") || stringid.Equals("playerclan")) && settlement.OwnerClan == Clan.PlayerClan
                                        ).ToString().ToLower());
                                    break;
                                case "mapfaction":
                                    AnalysisStack.Push((stringid.Equals(settlement.StringId)
                                        || (stringid.Equals("player") || stringid.Equals("playerclan") && settlement.MapFaction == Clan.PlayerClan.MapFaction
                                        || stringid.Equals("playerfaction") && settlement.MapFaction.Leader == Hero.MainHero)
                                        ).ToString().ToLower());
                                    break;
                                case "intown":
                                    AnalysisStack.Push(settlement.IsTown.ToString().ToLower().Equals(stringid).ToString().ToLower());
                                    break;
                                case "incastle":
                                    AnalysisStack.Push(settlement.IsCastle.ToString().ToLower().Equals(stringid).ToString().ToLower());
                                    break;
                                case "invillage":
                                    AnalysisStack.Push(settlement.IsVillage.ToString().ToLower().Equals(stringid).ToString().ToLower());
                                    break;
                            }

                            break;
                        }
                    case "!=":
                        {
                            string stringid = AnalysisStack.Pop();
                            switch (AnalysisStack.Pop())
                            {
                                case "settlement":
                                    AnalysisStack.Push((!stringid.Equals(settlement.StringId)).ToString().ToLower());
                                    break;
                                case "culture":
                                    AnalysisStack.Push((!stringid.Equals(settlement.Culture.StringId)
                                        || stringid.Equals("player") && settlement.Culture != Hero.MainHero.Culture
                                        || stringid.Equals("playerclan") && settlement.Culture != Clan.PlayerClan.Culture
                                        ).ToString().ToLower());
                                    break;
                                case "clan":
                                    AnalysisStack.Push((!stringid.Equals(settlement.OwnerClan.StringId)
                                        || (stringid.Equals("player") || stringid.Equals("playerclan")) && settlement.OwnerClan != Clan.PlayerClan
                                        ).ToString().ToLower());
                                    break;
                                case "mapfaction":
                                    AnalysisStack.Push((!stringid.Equals(settlement.StringId)
                                        || (stringid.Equals("player") || stringid.Equals("playerclan") && settlement.MapFaction != Clan.PlayerClan.MapFaction
                                        || stringid.Equals("playerfaction") && settlement.MapFaction.Leader == Hero.MainHero)
                                        ).ToString().ToLower());
                                    break;
                                case "intown":
                                    AnalysisStack.Push((!settlement.IsTown.ToString().ToLower().Equals(stringid)).ToString().ToLower());
                                    break;
                                case "incastle":
                                    AnalysisStack.Push((!settlement.IsCastle.ToString().ToLower().Equals(stringid)).ToString().ToLower());
                                    break;
                                case "invillage":
                                    AnalysisStack.Push((!settlement.IsVillage.ToString().ToLower().Equals(stringid)).ToString().ToLower());
                                    break;
                            }
                            break;
                        }
                    default:
                        AnalysisStack.Push(JudgeCondition);
                        break;
                }

            }
            if (AnalysisStack.Count != 1)
                MoExceptionManager.Get().ThrowException(new Exception());
            return AnalysisStack.Peek() == "true";
        }
    }
    internal static class MoCustomOptionsTools
    {
        public static string StringFormat(string JudgementMessage)
        {
            int i = 0;
            JudgementMessage = JudgementMessage.Trim();
            string FormatString = "";
            while (i < JudgementMessage.Length)
            {
                switch (JudgementMessage[i])
                {
                    case '(':
                    case ')':
                        if (JudgementMessage[i - 1] != ' ')
                            FormatString += ' ';
                        FormatString += JudgementMessage[i];
                        if (JudgementMessage[i + 1] != ' ')
                            FormatString += ' ';
                        break;
                    case '!':
                        if (JudgementMessage[i - 1] != ' ')
                            FormatString += ' ';
                        FormatString += JudgementMessage[i];
                        if (JudgementMessage[i + 1] != ' ' && JudgementMessage[i + 1] != '=')
                            FormatString += ' ';
                        break;
                    case '=':
                        if (JudgementMessage[i - 1] != ' ' && JudgementMessage[i - 1] != '=' && JudgementMessage[i - 1] != '!')
                            FormatString += ' ';
                        FormatString += JudgementMessage[i];
                        if (JudgementMessage[i + 1] != ' ' && JudgementMessage[i + 1] != '=')
                            FormatString += ' ';
                        break;
                    case '|':
                        if (JudgementMessage[i - 1] != ' ' && JudgementMessage[i - 1] != '|')
                            FormatString += ' ';
                        FormatString += JudgementMessage[i];
                        if (JudgementMessage[i + 1] != ' ' && JudgementMessage[i + 1] != '|')
                            FormatString += ' ';
                        break;
                    case ' ':
                        if (JudgementMessage[i - 1] == ' ')
                            break;
                        else
                            FormatString += JudgementMessage[i];
                        break;
                    default:
                        FormatString += JudgementMessage[i];
                        break;
                }
                i++;
            }
            return FormatString;
        }
        public static List<string> CreateJudgeConditions(/*Settlement settlement,*/string JudgementMessage)
        {
            List<string> ResultList = new List<string>();
            if(JudgementMessage.Trim().Length<=0)
            {
                ResultList.Add("true");
                return ResultList;
            }
            string[] Conditions = StringFormat(JudgementMessage.ToLower()).Split(' ');
            Stack<string> AnalysisStack = new Stack<string>();
            for (int i = 0; i < Conditions.Length; i++)
            {
                switch (Conditions[i])
                {
                    case "&&":
                    case "and":
                    case "||":
                    case "or":
                    case "!":
                    case "not":
                    case "==":
                    case "=":
                    case "!=":
                        while (AnalysisStack.Count > 0 && HigherPriorityThan(AnalysisStack.Peek(), Conditions[i]))
                        {
                            ResultList.Add(AnalysisStack.Pop());
                        }
                        AnalysisStack.Push(Conditions[i]);
                        break;
                    case "(":
                        AnalysisStack.Push(Conditions[i]);
                        break;
                    case ")":
                        while (AnalysisStack.Peek() != "(")
                        {
                            ResultList.Add(AnalysisStack.Pop());
                        }
                        AnalysisStack.Pop();
                        break;
                    default:
                        ResultList.Add(Conditions[i]);
                        break;
                }
            }
            while (AnalysisStack.Count > 0)
            {
                ResultList.Add(AnalysisStack.Pop());
            }

            return ResultList;
        }
        public static bool HigherPriorityThan(string a, string b)
        {
            int levela = 0, levelb = 0;
            switch (a)
            {
                case "==":
                case "=":
                case "!=": levela = 4; break;
                case "!":
                case "not": levela = 3; break;
                case "and":
                case "&&": levela = 2; break;
                case "||":
                case "or": levela = 1; break;
                default:
                    MoExceptionManager.Get().ThrowException(new MoExceptionBase(""));
                    break;
            }
            switch (b)
            {
                case "==":
                case "=":
                case "!=": levelb = 4; break;
                case "!":
                case "not": levelb = 3; break;
                case "and":
                case "&&": levelb = 2; break;
                case "||":
                case "or": levelb = 1; break;
                default:
                    MoExceptionManager.Get().ThrowException(new MoExceptionBase(""));
                    break;
            }

            return levela >= levelb;
        }
    }
}
