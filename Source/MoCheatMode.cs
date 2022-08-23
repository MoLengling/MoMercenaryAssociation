using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;

namespace MoMercenaryAssociation
{
    //class MoCheatMode
    //{
    //    public static int Gold
    //    {
    //        get
    //        {
    //            if (MoSettings.Get().CheatMode)
    //                return 10000000;
    //            else return Hero.MainHero.Gold;
    //        }
    //        set
    //        {
    //            if (!MoSettings.Get().CheatMode)
    //                Hero.MainHero.Gold = value;
    //        }
    //    }
    //    public static void ChangeHeroGold(int changeAmount)
    //    {
    //        Gold = (changeAmount <= int.MaxValue - Gold) ? (Gold + changeAmount) : int.MaxValue;
    //    }
    //    public static float Prosperity
    //    {
    //        get
    //        {
    //            if (MoSettings.Get().CheatMode)
    //                return 100000;
    //            if (Settlement.CurrentSettlement == null)
    //                return -1000;
    //            return Settlement.CurrentSettlement.Prosperity;
    //        }
    //        set
    //        {
    //            if (!MoSettings.Get().CheatMode && Settlement.CurrentSettlement != null)
    //                Settlement.CurrentSettlement.Prosperity = value;
    //        }
    //    }
    //    public static void ChangeSettlementProsperity(int changeAmount)
    //    {
    //        Prosperity = (changeAmount <= float.MaxValue - Prosperity) ? (Prosperity + changeAmount) : float.MaxValue;
    //    }
    //    public static float Renown
    //    {
    //        get
    //        {
    //            if (MoSettings.Get().CheatMode)
    //                return 10000;
    //            return Clan.PlayerClan.Renown;
    //        }
    //        set
    //        {
    //            if (!MoSettings.Get().CheatMode)
    //                Clan.PlayerClan.Renown = value;
    //        }
    //    }
    //    public static void ChangeClanRenown(int changeAmount)
    //    {
    //        Renown = (Renown <= float.MaxValue - Prosperity) ? (Renown + changeAmount) : float.MaxValue;
    //    }
    //    public static float Hearth
    //    {
    //        get
    //        {
    //            if (MoSettings.Get().CheatMode)
    //                return 10000;
    //            return Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.Village != null ? Settlement.CurrentSettlement.Village.Hearth : -1000;
    //        }
    //        set
    //        {
    //            if (!MoSettings.Get().CheatMode && Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.Village != null)
    //                Clan.PlayerClan.Renown = value;
    //        }
    //    }
    //    public static void ChangeVillageHearth(int changeAmount)
    //    {
    //        Hearth = (Hearth <= float.MaxValue - Prosperity) ? (Hearth + changeAmount) : float.MaxValue;
    //    }
    //    public static float Militia
    //    {
    //        get
    //        {
    //            if (MoSettings.Get().CheatMode)
    //                return 1000;
    //            return Settlement.CurrentSettlement!=null ? Settlement.CurrentSettlement.Militia : -1000;
    //        }
    //        set
    //        {
    //            if (!MoSettings.Get().CheatMode && Settlement.CurrentSettlement != null)
    //                Clan.PlayerClan.Renown = value;
    //        }
    //    }
    //    public static void ChangeSettlementMilitia(int changeAmount)
    //    {
    //        Militia = (Militia <= float.MaxValue - Prosperity) ? (Militia + changeAmount) : float.MaxValue;
    //    }
    //}
}
