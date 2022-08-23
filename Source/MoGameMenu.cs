using MoMercenaryAssociation.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Settlements;

namespace MoMercenaryAssociation
{
    class MoGameMenu : MoDataBase
    {
        //Dictionary<MoGameMenu, SubmenuOption> SubMenus;
        Dictionary<string, SubmenuOption> SubMenus;
        //List<string> SubMenuIds;
        protected string MenuText;
        public bool isRemoved { get; internal set; }

        public string MenuId { get => StringId; }
        public MoGameMenu() { }
        public MoGameMenu(string menuid) : base(menuid)
        {
            SubMenus = new Dictionary<string, SubmenuOption>();
        }
        public MoGameMenu(string menuid, string menuText) : this(menuid)
        {
            MenuText = menuText;
        }
        public void AddSubMenu(string submuneId,string conditions, string ToSubmenuText, string LeaveSubMenuText)
        {
            SubmenuOption optionText = new SubmenuOption(true, ToSubmenuText, LeaveSubMenuText, conditions);
            SubMenus.Add(submuneId, optionText);
        }
        public void AddSubMenu(string submuneId, string conditions, string ToSubmenuText)
        {
            SubmenuOption optionText = new SubmenuOption(false, ToSubmenuText, "", conditions);
            SubMenus.Add(submuneId, optionText);
        }
        public MoGameMenu SetMenuText(string Text)
        {
            MenuText = Text;
            return this;
        }
        virtual public string CreateSubmenusRecursively(string parentId)
        {
            //先创建自己
            string CreateMenuId = MoGameMenuHelper.Get().CreateGameMenu(MenuText);
            MoLogs.Get().Log("Create GameMenu: " + MenuId + " With Runtime Id:" + CreateMenuId +" Succeed");
            //循环递归每一个子菜单
            foreach (KeyValuePair<string, SubmenuOption> subMenu in SubMenus)
            {
                MoLogs.Get().Log("Create SubMenu: " + subMenu.Key + " Start");
                if (subMenu.Value.LeaveToThis)
                {
                    //创建有返回此菜单选项的子菜单
                    if (MoGameMenuManager.Get().CreateGameMenu(subMenu.Key, CreateMenuId, subMenu.Value.LeaveSubmenu,out string SubMenuRuntimeId))
                    {
                        //创建通往子菜单的按钮
                        MoGameMenuHelper.Get().CreateGameMenuOption(CreateMenuId, subMenu.Value.ToSubmenu,
                            delegate (MenuCallbackArgs args)
                            {
                                args.IsEnabled = subMenu.Value.Enable.IsEnable(Settlement.CurrentSettlement);
                                return true;
                            }
                        ,
                            delegate (MenuCallbackArgs args)
                            {
                                GameMenu.SwitchToMenu(SubMenuRuntimeId);
                            }
                         );
                    }
                    else //if (!MoCustomOfficeManager.Get().CreateOfficeMenu(subMenu.Key, CreateMenuId, subMenu.Value.LeaveSubmenu))
                    {
                        if (SubMenuRuntimeId == "IsRemoved")
                            continue;
                        MoExceptionManager.Get().ThrowException(new MoExceptionBase(""));
                    }
                }
                else
                {
                    //创建无返回此菜单选项的子菜单
                    if (MoGameMenuManager.Get().CreateGameMenu(subMenu.Key, CreateMenuId,out string SubMenuRuntimeId))
                    {
                        //创建通往子菜单的按钮
                        MoGameMenuHelper.Get().CreateGameMenuOption(MenuId, subMenu.Value.ToSubmenu,
                            delegate (MenuCallbackArgs args)
                            {
                                args.IsEnabled = subMenu.Value.Enable.IsEnable(Settlement.CurrentSettlement);
                                return true;
                            }
                        ,
                            delegate (MenuCallbackArgs args)
                            {
                                GameMenu.SwitchToMenu(SubMenuRuntimeId);
                            }
                         );
                    }
                    else //if (!MoCustomOfficeManager.Get().CreateOfficeMenu(subMenu.Key, CreateMenuId))
                    {
                        if (SubMenuRuntimeId == "IsRemoved")
                            continue;
                        MoExceptionManager.Get().ThrowException(new MoExceptionBase(""));
                    }
                }
            }

            return CreateMenuId;
        }
        virtual public string CreateSubmenusRecursively(string parentId, string ReturnParentOptionText)
        {
            if (isRemoved)
                //如果被标记删除则不创建
                return "IsRemoved";
            //先创建自己
            string CreateMenuId = MoGameMenuHelper.Get().CreateGameMenu(MenuText);
            MoLogs.Get().Log("Create GameMenu: " + MenuId + " With Runtime Id:" + CreateMenuId + " Succeed");
            //循环递归每一个子菜单
            foreach (KeyValuePair<string, SubmenuOption> subMenu in SubMenus)
            {
                MoLogs.Get().Log("Create SubMenu: " + subMenu.Key + " Start");
                if (subMenu.Value.LeaveToThis)
                {
                    //创建有返回此菜单选项的子菜单
                    if (MoGameMenuManager.Get().CreateGameMenu(subMenu.Key, CreateMenuId, subMenu.Value.LeaveSubmenu, out string SubMenuRuntimeId))
                    {
                        //创建通往子菜单的按钮
                        MoGameMenuHelper.Get().CreateGameMenuOption(CreateMenuId, subMenu.Value.ToSubmenu,
                            delegate (MenuCallbackArgs args)
                            {
                                args.IsEnabled = subMenu.Value.Enable.IsEnable(Settlement.CurrentSettlement);
                                return true;
                            }
                        ,
                            delegate (MenuCallbackArgs args)
                            {
                                GameMenu.SwitchToMenu(SubMenuRuntimeId);
                            }
                         );
                    }
                    else //if (!MoCustomOfficeManager.Get().CreateOfficeMenu(subMenu.Key, CreateMenuId, subMenu.Value.LeaveSubmenu))
                    {
                        if (SubMenuRuntimeId == "IsRemoved")
                            continue;
                        MoExceptionManager.Get().ThrowException(new MoExceptionBase(""));
                    }
                }
                else
                {
                    //创建无返回此菜单选项的子菜单
                    if (MoGameMenuManager.Get().CreateGameMenu(subMenu.Key, CreateMenuId, out string SubMenuRuntimeId))
                    {
                        //创建通往子菜单的按钮
                        MoGameMenuHelper.Get().CreateGameMenuOption(MenuId, subMenu.Value.ToSubmenu,
                            delegate (MenuCallbackArgs args)
                            {
                                args.IsEnabled = subMenu.Value.Enable.IsEnable(Settlement.CurrentSettlement);
                                return true;
                            }
                        ,
                            delegate (MenuCallbackArgs args)
                            {
                                GameMenu.SwitchToMenu(SubMenuRuntimeId);
                            }
                         );
                    }
                    else //if (!MoCustomOfficeManager.Get().CreateOfficeMenu(subMenu.Key, CreateMenuId))
                    {
                        if (SubMenuRuntimeId == "IsRemoved")
                            continue;
                        MoExceptionManager.Get().ThrowException(new MoExceptionBase(""));
                    }
                }
            }
            if (ReturnParentOptionText == "")
                ReturnParentOptionText = MoStrings.DefaultReturnOptionText;
            //创建通往父菜单的选项
            MoGameMenuHelper.Get().CreateLeaveMenuOption(CreateMenuId, parentId, ReturnParentOptionText);

            return CreateMenuId;
        }
    }
    internal struct SubmenuOption
    {
        public readonly bool LeaveToThis;
        public readonly string ToSubmenu;
        public readonly string LeaveSubmenu;
        public MoCustomOption Enable;

        public SubmenuOption(bool leaveToThis, string toSubmenu, string leaveSubmenu, string EnableCondition)
        {
            LeaveToThis = leaveToThis;
            ToSubmenu = toSubmenu;
            LeaveSubmenu = leaveSubmenu;
            Enable = new MoCustomOption(EnableCondition);
        }
    }

}
