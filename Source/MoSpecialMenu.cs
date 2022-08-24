using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Settlements;

namespace MoMercenaryAssociation
{
    class MoSpecialMenu : MoDataBase
    {
        protected Dictionary<string,SubmenuOption> Parents;
        public MoGameMenu RealMenu { get; private set; }
        public void AddParent(string parent, SubmenuOption optioninfo) => Parents.Add(parent, optioninfo);

        public string[] GetParents()
        {
            string[] parents = new string[Parents.Count];
            Parents.Keys.CopyTo(parents, 0);
            return parents;
        }
        public MoSpecialMenu()
        {

        }
        public MoSpecialMenu(MoGameMenu menu)
        {
            RealMenu = menu;
            StringId = menu.MenuId;
        }
        public void AddParentMenu(string submuneId, string conditions, string ToSubmenuText, string LeaveSubMenuText)
        {
            if (Parents.ContainsKey(submuneId))
                return;
            SubmenuOption optionText = new SubmenuOption(true, ToSubmenuText, LeaveSubMenuText, conditions);
            AddParent(submuneId, optionText);
        }
        public void AddParentMenu(string submuneId, string conditions, string ToSubmenuText)
        {
            if (Parents.ContainsKey(submuneId))
                return;
            SubmenuOption optionText = new SubmenuOption(false, ToSubmenuText, "", conditions);
            AddParent(submuneId, optionText);
        }
        public void CreateSpecialMenu()
        {
            
            foreach(KeyValuePair<string,SubmenuOption> parent in Parents)
            {
                if(MoGameMenuManager.Get().Contains(parent.Key))
                {
                    //再观察一次父Menu是不是后加载的，目前列表有没有父菜单。如果有则通过GameMenuManager创建。
                    MoGameMenuManager.Get()[parent.Key].AddSubMenu(StringId, parent.Value);
                    continue;
                }
                if (parent.Value.LeaveToThis)
                {
                    string RuntimeId = RealMenu.CreateSubmenusRecursively(parent.Key,parent.Value.LeaveSubmenu);
                    MoGameMenuHelper.Get().CreateGameMenuOption(parent.Key, parent.Value.ToSubmenu,
                            delegate (MenuCallbackArgs args)
                            {
                                args.IsEnabled = parent.Value.Enable.IsEnable(Settlement.CurrentSettlement);
                                return true;
                            }
                        ,
                            delegate (MenuCallbackArgs args)
                            {
                                GameMenu.SwitchToMenu(RuntimeId);
                            }
                         );
                }
                else
                {
                    string RuntimeId = RealMenu.CreateSubmenusRecursively(parent.Key);
                    MoGameMenuHelper.Get().CreateGameMenuOption(parent.Key, parent.Value.ToSubmenu,
                            delegate (MenuCallbackArgs args)
                            {
                                args.IsEnabled = parent.Value.Enable.IsEnable(Settlement.CurrentSettlement);
                                return true;
                            }
                        ,
                            delegate (MenuCallbackArgs args)
                            {
                                GameMenu.SwitchToMenu(RuntimeId);
                            }
                         );
                }
            }
        }
    }
}
