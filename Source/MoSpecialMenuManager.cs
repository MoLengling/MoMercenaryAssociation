using System;
using System.Collections.Generic;
using System.Text;

namespace MoMercenaryAssociation
{
    class MoSpecialMenuManager : MoManagerBase<MoSpecialMenuManager, MoSpecialMenu>
    {
        public Dictionary<string, MoSpecialMenu> SpecialMenus => Managements;
        public MoSpecialMenuManager()
        {
            
        }

        public void AddSpecialMenu(MoSpecialMenu menu)
        {
            AddManagement(menu.StringId, menu);
        }

        public void StartCreateMenu()
        {
            MoLogs.Get().Log("SpecialMenu", "Start Create Special Menu");
            foreach(KeyValuePair<string,MoSpecialMenu> menu in SpecialMenus)
            {
                try
                {
                    MoLogs.Get().Log("SpecialMenu", "create Spcial Menu" + menu.Value.RealMenu.MenuId);
                    menu.Value.CreateSpecialMenu();
                }
                catch(Exception e)
                {
                    MoExceptionManager.Get().RecordException(e);
                }
            }
        }
    }
}
