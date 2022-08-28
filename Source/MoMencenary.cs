using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.CampaignSystem;
using MoMercenaryAssociation.Exceptions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Actions;
using System.Reflection;

namespace MoMercenaryAssociation
{
    //prosperity 繁荣度
    //readyMilitia 民兵
    //settlement village Hearth 怀疑是 户数
    class MoMercenary :MoDataBase
    {
        CharacterObject _Character;
        

        public List<RecruitInfo> RecruitInfos { get; private set; }
        public MoMercenary()
        {
            MoExceptionManager.Get().ThrowException(new MoEmptyTroopMethodException("There is something invokes the Method that has no Params, and should never be used in program of MoMencenary."));
        }

        public MoMercenary(string id):base(id)
        {
            RecruitInfos = new List<RecruitInfo>();
            if(!verifyCharacter())
            {
                MoExceptionManager.Get().ThrowException(new MoNonexistentStringidException("id:" + id + " was not found."));
            }
        }

        public bool verifyCharacter()
        {
            _Character = CharacterObject.Find(StringId);
            if (_Character == null)
                return false;
            return true;
        }

        public void CreateRecruitOption(string MenuId)
        {
            foreach(RecruitInfo info in RecruitInfos)
            {
                string OptionText;
                if (info.OptionText != "")
                {
                    OptionText = info.OptionText;
                }
                else
                {
                    OptionText = MoStrings.DefaultRecruitOptionText;
                }
                
                //Get {Amount} {TroopName} as your {RecruitType} cost {GoldValue} {Gold} {ProsperityValue} {Prosperity}
                 //{HearthValue} {Hearth} {MilitiaValue} {Militia} {RenownValue} {Renown}
                    string[][] OptionString = new string[][]{
                        new string[]{ "Amount", info.Amount.ToString()} ,
                        new string[]{ "TroopName",_Character.Name.ToString()} ,
                        new string[]{ "RecruitType", info.recruitType.ToDescription()} ,
                        new string[]{ "GoldValue", info.ListOfCost.ContainsKey(RecruitCostType.Gold) ? info.ListOfCost[RecruitCostType.Gold].ToString():""} ,
                        new string[]{ "Gold", info.ListOfCost.ContainsKey(RecruitCostType.Gold) ? MoStrings.Gold: "" } ,
                        new string[]{ "ProsperityValue", info.ListOfCost.ContainsKey(RecruitCostType.Prosperity) ? info.ListOfCost[RecruitCostType.Prosperity].ToString() : "" } ,
                        new string[]{ "Prosperity", info.ListOfCost.ContainsKey(RecruitCostType.Prosperity) ? MoStrings.Prosperity : "" } ,
                        new string[]{ "HearthValue", info.ListOfCost.ContainsKey(RecruitCostType.Hearth) ? info.ListOfCost[RecruitCostType.Hearth].ToString() : "" } ,
                        new string[]{ "Hearth", info.ListOfCost.ContainsKey(RecruitCostType.Hearth) ? MoStrings.Hearth : "" } ,
                        new string[]{ "MilitiaValue", info.ListOfCost.ContainsKey(RecruitCostType.Militia) ? info.ListOfCost[RecruitCostType.Militia].ToString() : "" } ,
                        new string[]{ "Militia", info.ListOfCost.ContainsKey(RecruitCostType.Militia) ? MoStrings.Militia : "" } ,
                        new string[]{ "RenownValue", info.ListOfCost.ContainsKey(RecruitCostType.Renown) ? info.ListOfCost[RecruitCostType.Renown].ToString() : "" } ,
                        new string[]{ "Renown", info.ListOfCost.ContainsKey(RecruitCostType.Renown) ? MoStrings.Renown : "" } 
                    };

                    OptionText = MoTexts.ToString(OptionText, OptionString);
                
                MoGameMenuHelper.Get().CreateGameMenuOption(MenuId, OptionText,
                    info.OnOptionInitDelegate
                    ,
                    delegate { info.OnOptionClickDelegate(this._Character); }
                );

            }
        }
    }


    struct RecruitInfo
    {
        public RecruitType recruitType;
        public int Amount;
        public Dictionary<RecruitCostType, float> ListOfCost;//整个List加起来是招募Amount数量的士兵的花费
        public string OptionText;

        public bool OnOptionInitDelegate(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Recruit;
            if (MoSettings.Get().CheatMode)
            { 
                return true; 
            }
            bool enable = true;
            foreach(KeyValuePair<RecruitCostType, float> Cost in ListOfCost)
            {
                string message;
                if (!CostUsable(Cost.Key, Settlement.CurrentSettlement, out message))
                {
                    enable = false;
                    args.Text = new TextObject(message);
                    break;
                }
                if (!CostEnough(Cost.Key, Cost.Value, Settlement.CurrentSettlement, out message))
                {
                    enable = false;
                    args.Tooltip = new TextObject(message);
                    break;
                }
            }
            args.IsEnabled = enable;
            return true;
        }
        public void OnOptionClickDelegate(CharacterObject character)
        {
            if (!MoSettings.Get().CheatMode)
            {
                foreach (KeyValuePair<RecruitCostType, float> Cost in ListOfCost)
                {
                    string message;
                    if (!CostUsable(Cost.Key, Settlement.CurrentSettlement, out message) ||
                        !CostEnough(Cost.Key, Cost.Value, Settlement.CurrentSettlement, out message))
                    {
                        //TextObject debugMessage = new TextObject(MoStrings.UnableToCoverTheCost);
                        //debugMessage.SetTextVariable("CostType", nameof(Cost.Key));
                        //MoLogs.Get().Debug(debugMessage.ToString());
                        return;
                    }
                    switch (Cost.Key)
                    {
                        case RecruitCostType.Gold: Hero.MainHero.Gold -= (int)Cost.Value; break;
                        case RecruitCostType.Prosperity: Settlement.CurrentSettlement.Prosperity -= Cost.Value; break;
                        case RecruitCostType.Hearth: Settlement.CurrentSettlement.Village.Hearth -= Cost.Value; break;
                        case RecruitCostType.Militia: Settlement.CurrentSettlement.Militia -= Cost.Value; break;
                        case RecruitCostType.Renown: Clan.PlayerClan.Renown -= Cost.Value; break;
                    }
                }
            }
            switch (recruitType)
            {
                case RecruitType.Soldier:
                    MobileParty.MainParty.AddElementToMemberRoster(character, Amount, false); break;
                case RecruitType.Prisoner:
                    MobileParty.MainParty.AddPrisoner(character, Amount); break;
                case RecruitType.Companion:
                    {
                        for (int i = 0; i < Amount; i++)
                        {
                            Type CharacterType = typeof(Hero);
                            Hero newHero = HeroCreator.CreateSpecialHero(character);
                            CharacterType.GetProperty("Occupation"/*, BindingFlags.NonPublic | BindingFlags.Instance*/).SetValue(newHero, Occupation.Wanderer);
                            
                            AddCompanionAction.Apply(Clan.PlayerClan, newHero);
                            AddHeroToPartyAction.Apply(newHero, MobileParty.MainParty);
                        }
                        break;
                    }
                case RecruitType.Family:
                    {
                        for (int i = 0; i < Amount; i++)
                        {
                            Type CharacterType = typeof(Hero);
                            Hero newHero = HeroCreator.CreateSpecialHero(character,Settlement.CurrentSettlement,Clan.PlayerClan);
                            CharacterType.GetProperty("Occupation").SetValue(newHero, Occupation.Lord);
                            AddHeroToPartyAction.Apply(newHero, MobileParty.MainParty);
                        }
                        break;
                    }
            }
           
        }

        //是否允许使用这些条件进行招募
        private static bool CostUsable(RecruitCostType costtype,Settlement settlement,out string Messgae)
        {
            if (MoSettings.Get().CheatMode)
            {
                Messgae = "";
                return true;
            }
            switch (costtype)
            {
                case RecruitCostType.Gold:
                    Messgae = "";
                    return true;
                case RecruitCostType.Prosperity:
                    Messgae = settlement.OwnerClan == Clan.PlayerClan ||
                            settlement.MapFaction.Leader.Clan == Clan.PlayerClan ? "" : MoStrings.UnusableCondition_Prosperity;
                    break;
                case RecruitCostType.Hearth:
                    Messgae = settlement.IsVillage && (settlement.OwnerClan == Clan.PlayerClan || settlement.MapFaction.Leader.Clan == Clan.PlayerClan)
                        ? "" : MoStrings.UnusableCondition_Hearth;
                    break;
                case RecruitCostType.Militia:
                   Messgae = settlement.Militia > MoSettings.Get().AIMinMilitiaNumber || settlement.OwnerClan == Clan.PlayerClan || settlement.MapFaction.Leader.Clan == Clan.PlayerClan
                        ? "" : MoStrings.UnusableCondition_Militia;
                    break;
                case RecruitCostType.Renown:
                    Messgae = "";
                    break;
                default:
                    Messgae = "SSSSS";
                    break;
            }

            if (Messgae != "")
                return false;
            return true;
        }

        //能否负担其
        private static bool CostEnough(RecruitCostType costtype,float value, Settlement settlement,out string Messgae)
        {
            if (MoSettings.Get().CheatMode)
            {
                Messgae = "";
                return true; 
            }
            switch (costtype)
            {
                case RecruitCostType.Gold:
                    Messgae = Hero.MainHero.Gold > value ? "" : MoStrings.UnableToCoverTheCost_Gold; break;
                case RecruitCostType.Prosperity:
                    Messgae = settlement.Prosperity > value ? "" : MoStrings.UnableToCoverTheCost_Gold; break;
                case RecruitCostType.Hearth:
                    Messgae = settlement.Village.Hearth > value ? "" : MoStrings.UnableToCoverTheCost_Gold; break;
                case RecruitCostType.Militia:
                    Messgae = settlement.Militia - value >= MoSettings.Get().AIMinMilitiaNumber ? "" : MoStrings.UnableToCoverTheCost_Gold; break;
                case RecruitCostType.Renown:
                    Messgae = Clan.PlayerClan.Renown > value ? "" : MoStrings.UnableToCoverTheCost_Gold; break;
                default:
                    Messgae = "SSSSS";
                    break;
            }

            if (Messgae == "")
                return true;
            return false;
        }
    }
    public enum RecruitType
    {
        Soldier,
        Prisoner,
        Companion,
        Family
    }
    public enum RecruitCostType
    {
        Gold,
        Prosperity,
        Hearth,
        Militia,
        Renown
    }

    public static class RecruitTypeEnumExtension
    {
        public static string ToDescription(this RecruitType recruitType)
        {
            switch(recruitType)
            {
                case RecruitType.Soldier:
                    return MoStrings.Soldier;
                case RecruitType.Prisoner:
                    return MoStrings.Prisoner;
                case RecruitType.Companion:
                    return MoStrings.Companion;
                case RecruitType.Family:
                    return MoStrings.Family;
                default: return "";
            }
        }
    }
}
