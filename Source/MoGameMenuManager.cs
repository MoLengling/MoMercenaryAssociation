using MoMercenaryAssociation.Exceptions;
using System.Collections.Generic;
using System.Xml;

namespace MoMercenaryAssociation
{
    class MoGameMenuManager : MoManagerBase<MoGameMenuManager, MoGameMenu>
    {
        public Dictionary<string,MoGameMenu> GameMenus { get => Managements; }
        public MoGameMenu[] GetGameMenus() => base.GetManagements();
        public MoGameMenuManager():base()
        {
            InitMMAHallMenu();
        }
        public bool AddNewGameMenu(MoGameMenu NewMenu)
        {
            return AddManagement(NewMenu);
        }
        public void InitMMAHallMenu()
        {
            //这些菜单在管理器构造时已经被加载
            AddNewGameMenu(new MoGameMenu("MMA_Town").SetMenuText(MoStrings.TownText));
            AddNewGameMenu(new MoGameMenu("MMA_Castle").SetMenuText(MoStrings.CastleText));
            AddNewGameMenu(new MoGameMenu("MMA_Village").SetMenuText(MoStrings.VillageText));
        }
        public void InitCustomGameMenus(List<string> CustomMMAConfigPath)
        {
            //遍历每个文件，读取其中的菜单信息。
            foreach (string configPath in CustomMMAConfigPath)
            {
                MoLogs.Get().Log("Start Reading" + configPath);
                MoXmlReader.Get(new XmlReaderParam(configPath, "Submenus", delegate(XmlNode RootNode,bool Sueeccd)
                {
                    if(Sueeccd)
                    {
                        MoLogs.Get().Log("Succeed");
                        ReadMMAConfigXml(RootNode, configPath);
                    }
                    else
                    {

                        MoLogs.Get().Log("Failed");
                        MoXmlReader.Get(new XmlReaderParam(configPath, "Menus", delegate (XmlNode OldRootNode, bool IsSueeccd)
                        {
                            if (IsSueeccd)
                            {
                                MoLogs.Get().Log("MoChangeTool", "Find old Config File,now we will change it.");
                                MoUpdateConfigTool.DoUpdate(configPath, OldRootNode);
                                MoLogs.Get().Log("Start Reading Again" + configPath);
                                MoXmlReader.Get(new XmlReaderParam(configPath, "Submenus", delegate (XmlNode NewRootNode, bool AreSueeccd)
                                {
                                    if (AreSueeccd)
                                    {
                                        MoLogs.Get().Log("Succeed");
                                        ReadMMAConfigXml(NewRootNode, configPath);
                                    }
                                }));
                                MoXmlReader.Get().StartReadXml();
                            }
                        }));
                        MoXmlReader.Get().StartReadXml();
                    }
                }
                ));
                MoXmlReader.Get().StartReadXml();
            }
        }

        public void ReadMMAConfigXml(XmlNode rootNode, string FilePath)
        {
            MoGameMenuManagerHelper.StartReadDate(rootNode, FilePath);
        }

        public void StartCreateMenu()
        {
            MoSpecialMenuManager.Get().StartCreateMenu();
            string Temp = GameMenus["MMA_Town"].CreateSubmenusRecursively("town",MoStrings.LeaveTownOptionText);
            MoGameMenuHelper.Get().CreateSubMenuOption("town", Temp, MoStrings.ToTownOptionText, false, 5, false);
            Temp = GameMenus["MMA_Castle"].CreateSubmenusRecursively("castle", MoStrings.LeaveCastleOptionText);
            MoGameMenuHelper.Get().CreateSubMenuOption("castle",Temp, MoStrings.ToCastleOptionText, false, 5, false);
            Temp = GameMenus["MMA_Village"].CreateSubmenusRecursively("village", MoStrings.LeaveVillageOptionText);
            MoGameMenuHelper.Get().CreateSubMenuOption("village", Temp, MoStrings.ToVillageOptionText, false, 5, false);
        }
        public bool CreateGameMenu(string TargetMenuId, string FatherMenuId, string ReturnOptionText, out string CreateMenuId)
        {
            if(GameMenus.ContainsKey(TargetMenuId))
            {
                CreateMenuId = GameMenus[TargetMenuId].CreateSubmenusRecursively(FatherMenuId, ReturnOptionText);
                if (CreateMenuId == "IsRemoved")
                    return false;
                return true;
            }
            CreateMenuId = "";
            return false;
        }
        public bool CreateGameMenu(string TargetMenuId,string FatherMenuId, out string CreateMenuId)
        {
            if (GameMenus.ContainsKey(TargetMenuId))
            {
                CreateMenuId = GameMenus[TargetMenuId].CreateSubmenusRecursively(FatherMenuId);
                if (CreateMenuId == "IsRemoved")
                    return false;
                return true;
            }
            CreateMenuId = "";
            return false;
        }
    }

    internal static class MoGameMenuManagerHelper
    {
        public static void StartReadDate(XmlNode rootNode, string FilePath)
        {
            foreach (XmlNode Child in rootNode.ChildNodes)
            {
                MoGameMenu NewMenu;
                if (LoadGameMenuBaseData(Child, FilePath, out NewMenu))
                {

                    //将这些菜单添加到菜单管理器中去
                    MoGameMenuManager.Get().AddNewGameMenu(NewMenu);
                    XmlNode Parents = Child.SelectSingleNode("Parentmenus");
                    if (Parents != null)
                    {
                        LoadParentMenus(NewMenu, Parents);
                    }

                    XmlNode SubMenus = Child.SelectSingleNode("Submenus");
                    if (SubMenus != null)
                    {
                        LoadSubMenus(NewMenu, SubMenus);
                    }

                }
            }
        }
        private static void LoadParentMenus(MoGameMenu NewMenu, XmlNode ParentNodes)
        {
            MoLogs.Get().Log("GameMenu Id= " + NewMenu.MenuId + " defined its parent menu, Reading parent menu start");
            foreach (XmlNode ParentMenu in ParentNodes.ChildNodes)
            {
                string To, Re, CanRe, Id, EnableConditions;
                if (ReadOptionData(ParentMenu, out To, out Re, out CanRe, out Id, out EnableConditions))
                {
                    if (MoGameMenuManager.Get().Contains(Id))
                    {
                        if (CanRe.Trim().ToLower() == "false")
                            MoGameMenuManager.Get()[Id].AddSubMenu(NewMenu.MenuId, EnableConditions, To);
                        else
                            MoGameMenuManager.Get()[Id].AddSubMenu(NewMenu.MenuId, EnableConditions, To, Re);
                    }
                    else
                    {
                        MoLogs.Get().Log("MoGameMenu", "There is a Special ParentId:" + Id + " MoGamemenuMangager Can not find it.");
                        MoLogs.Get().Log("MoGameMenu", "Maybe it is a exist gamenenu but create by other mods, or is a officaial Menu.");
                        MoLogs.Get().Log("MoGameMenu", "MMA will try to add current menu to its defined parentmenu.");
                        MoSpecialMenu SpecialMenu;
                        if (!MoSpecialMenuManager.Get().TryFindManagement(NewMenu.MenuId, out SpecialMenu))
                        {
                            SpecialMenu = new MoSpecialMenu(NewMenu);
                            MoSpecialMenuManager.Get().AddSpecialMenu(SpecialMenu);
                        }

                        if (SpecialMenu.RealMenu != NewMenu)
                        {
                            MoExceptionManager.Get().RecordException(new MoExceptionBase("SpecialMenu.RealMenu != NewMenu!"));
                            continue;
                        }
                        if (CanRe.Trim().ToLower() == "false")
                            SpecialMenu.AddParentMenu(NewMenu.MenuId, EnableConditions, To);
                        else
                            SpecialMenu.AddParentMenu(NewMenu.MenuId, EnableConditions, To, Re);
                    }
                    MoLogs.Get().Log("Parentmenu: " + Id);
                }
            }
        }
        private static void LoadSubMenus(MoGameMenu NewMenu, XmlNode SubMenus)
        {
            MoLogs.Get().Log("GameMenu Id= " + NewMenu.MenuId + " has Submenu, Reading Submenu start");
            //如果有子菜单，则读取它的子菜单（的id）
            //不检查子菜单是否存在
            foreach (XmlNode SubMenu in SubMenus.ChildNodes)
            {
                string To , Re , CanRe , Id , EnableConditions ;
                if (ReadOptionData(SubMenu, out To, out Re, out CanRe, out Id, out EnableConditions))
                {
                    if (CanRe.Trim().ToLower() == "false")
                        MoGameMenuManager.Get()[NewMenu.MenuId].AddSubMenu(Id, EnableConditions, To);
                    else
                        MoGameMenuManager.Get()[NewMenu.MenuId].AddSubMenu(Id, EnableConditions, To, Re);
                    MoLogs.Get().Log("Submenu: " + Id);
                }
            }
        }

        private static bool ReadOptionData(XmlNode MenuNode, out string To, out string Re, out string CanRe, out string Id, out string EnableConditions)
        {
            To = "";
            Re = "";
            CanRe = "";
            Id = "";
            EnableConditions = "";

            foreach (XmlAttribute xmlAttribute in MenuNode.Attributes)
            {
                switch (xmlAttribute.Name)
                {
                    case "OptionText":
                        To = xmlAttribute.Value; break;
                    case "ReturnText":
                        Re = xmlAttribute.Value; break;
                    case "Return":
                        CanRe = xmlAttribute.Value; break;
                    case "Id":
                        Id = xmlAttribute.Value; break;
                    case "EnableConditiosn":
                        EnableConditions = xmlAttribute.Value; break;
                }
            }
            if (Id == "")
            {
                MoExceptionManager.Get().RecordException(new MoExceptionBase("Can not find SubmenuId, skip over it!"));
                return false;
            }
            return true;
        }

        private static bool LoadGameMenuBaseData(XmlNode Child, string FilePath, out MoGameMenu NewMenu)
        {
            //读取属性值
            string MenuId = ""/*, InTown = "", InCastle = "", InVillage = ""*/, Text = ""/*, OptionText = ""*/, OverwirteMode = "";
            foreach (XmlAttribute xmlAttribute in Child.Attributes)
            {
                switch (xmlAttribute.Name)
                {
                    case "Id":
                        MenuId = xmlAttribute.Value;
                        MoLogs.Get().Log("MoGameMenu", "Reading Gamemune,id: " + MenuId);
                        break;
                    case "OverwirteMode":
                        OverwirteMode = xmlAttribute.Value;
                        MoLogs.Get().Log("MoGameMenu", "Reading Gamemune,OverwirteMode: " + OverwirteMode);
                        break;
                    case "Text":
                        Text = xmlAttribute.Value;
                        MoLogs.Get().Log("MoGameMenu", "Reading Gamemune,Text: " + Text);
                        break;
                }

            }
            if (MenuId == "" || MenuId == null)
            {
                //如果没有菜单ID，记录异常，不加载此菜单
                MoExceptionManager.Get().RecordException(new MoExceptionBase("GameMenu without Menuid"));
                NewMenu = null;
                return false;
            }
            if (MoGameMenuManager.Get().Contains(MenuId))
            {
                MoLogs.Get().Log("MoGameMenu", "Exist GameMenu,Id: " + MenuId);
                if (OverwirteMode.ToLower() == "remove")
                {
                    //使用remove标记
                    MoGameMenuManager.Get()[MenuId].isRemoved = true;
                    NewMenu = null;
                    return false;

                }
                else if (OverwirteMode.ToLower() == "overwrite")
                {
                    MoGameMenuManager.Get().GameMenus.Remove(MenuId);
                }
                else
                {
                    MoExceptionManager.Get().RecordException(new MoExceptionBase("Same id:" + MenuId + ", No OverWriteMode!"));
                    MoLogs.Get().Log("MoGameMenu", "Perform avoidance actions, This is likely to cause problems in runtime");
                    int i = 0;
                    string tempId = MenuId;
                    while (MoGameMenuManager.Get().Contains(tempId))
                    {
                        tempId = MenuId;
                        tempId += "_Avoid" + i;
                    }
                    MenuId = tempId;
                    MoLogs.Get().Log("MoGameMenu", "avoidance actions succeed, new GameMenuId:" + MenuId);
                }
            }

            if (Child.Name == "Submenu" || Child.Name == "Menu")
            {
                MoLogs.Get().Log("MoGameMenu", "GameMenu Id=" + MenuId + " Text:" + Text);
                NewMenu = new MoGameMenu(MenuId, Text);
            }
            else if (Child.Name == "Office")
            {
                MoLogs.Get().Log("MoGameMenu", "CuntomOffice Id=" + MenuId + " Text:" + Text);
                MoCustomOffice NewOffice = new MoCustomOffice(MenuId, Text);
                MoCustomOfficeManager.Get().ReadMMAConfigXml(NewOffice, Child, FilePath);
                NewMenu = NewOffice;
            }
            else
            {
                MoExceptionManager.Get().RecordException(new MoExceptionBase("Check if the Submenu Node in "+ FilePath +" Id: " + MenuId) );
                NewMenu = null;
                return false;
            }

            return true;
        }
    }
}
