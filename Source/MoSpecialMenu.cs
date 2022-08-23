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
        public void AddParents(string parent, SubmenuOption optioninfo) => Parents.Add(parent, optioninfo);

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
        }

        public void CreateSpecialMenu()
        {
            foreach(KeyValuePair<string,SubmenuOption> parent in Parents)
            {
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
