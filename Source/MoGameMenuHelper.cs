using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using static TaleWorlds.CampaignSystem.GameMenus.GameMenuOption;

namespace MoMercenaryAssociation
{
    public class MoGameMenuHelper:MoSingletonBase<MoGameMenuHelper>
    {
        public class MoGameMenuHelperParam:MoSingletonParam
        {
            internal CampaignGameStarter GameStarter;

            public MoGameMenuHelperParam(CampaignGameStarter gameStarter)
            {
                GameStarter = gameStarter;
            }
        }
        private int MenuIndex, OptionIndex;
        private readonly string MenuId;
        private readonly string OptionId;
        private CampaignGameStarter _GameStarter;
        public MoGameMenuHelper()
        {
            MenuIndex = 0;
            MenuId = "MMA_CustomMenu_";
            OptionIndex = 0;
            OptionId = "MMA_CustonOption_";
        }
        //public MoGameMenuHelper(CampaignGameStarter obj):this()
        //{
        //    _GameStarter = obj;
        //}

        public void Reset()
        {
            _GameStarter = null;
            MenuIndex = 0;
            OptionIndex = 0;
        }

        protected override void Init(MoSingletonParam Param)
        {
            MoGameMenuHelperParam MyParam = (MoGameMenuHelperParam)Param;
            if (MyParam != null)
            {
                _GameStarter = MyParam.GameStarter;
            }
        }
        public override bool IsReady()
        {
            return _GameStarter != null;
        }
        public string CreateGameMenu(string CustomMenuId, string GameMenuText, OnInitDelegate onInitDelegate = null)
        {
            _GameStarter.AddGameMenu(CustomMenuId, GameMenuText, onInitDelegate);
            MoLogs.Get().Log("Create Menu with Id:" + CustomMenuId);
            return CustomMenuId;
        }
        public string CreateGameMenu(string GameMenuText, OnInitDelegate onInitDelegate = null)
        {
            string GameMenuId = MenuId + MenuIndex++;            
            return CreateGameMenu(GameMenuId, GameMenuText, onInitDelegate);
        }
        public string CreateGameMenuOption(string GameMenuId, string GameMenuOptionText, GameMenuOption.OnConditionDelegate Conditions = null, GameMenuOption.OnConsequenceDelegate clickEvent = null, bool isLeave = false, int index = -1, bool isRepeatable = false)
        {
            string GameOptionId = OptionId + OptionIndex++;
            MoLogs.Get().Log("Create MenuOption with Id:"+ GameOptionId +"For menu with id:"+ GameMenuId);
            _GameStarter.AddGameMenuOption(GameMenuId, GameOptionId, GameMenuOptionText, Conditions, clickEvent, isLeave, index, isRepeatable);
            return GameOptionId;
        }

        public string CreateSubMenuOption(string FromGameMenuId, string ToGameMuneId, string GameMenuOptionText, bool isLeave = false, int index = -1, bool isRepeatable = false)
        {
            return CreateGameMenuOption(FromGameMenuId, GameMenuOptionText, delegate (MenuCallbackArgs args) { args.optionLeaveType = LeaveType.Submenu; return true; }, delegate { GameMenu.SwitchToMenu(ToGameMuneId); }, isLeave, index, isRepeatable);
        }
        public string CreateLeaveMenuOption(string FromGameMenuId, string ToGameMuneId, string GameMenuOptionText, bool isLeave = false, int index = -1, bool isRepeatable = false)
        {
            return CreateGameMenuOption(FromGameMenuId, GameMenuOptionText, delegate (MenuCallbackArgs args) { args.optionLeaveType = LeaveType.Leave; return true; }, delegate { GameMenu.SwitchToMenu(ToGameMuneId); }, isLeave, index, isRepeatable);
        }

        //MMA内部创建时执行检查的方法
        internal string CreateSubMenuOption_Check(string FromGameMenuId, string ToGameMuneId, string GameMenuOptionText, bool isLeave = false, int index = -1, bool isRepeatable = false)
        {
            if(FromGameMenuId == "IsRemoved" || ToGameMuneId == "IsRemoved")
            {
                return "IsRemoved";
            }
            return CreateGameMenuOption(FromGameMenuId, GameMenuOptionText, delegate (MenuCallbackArgs args) { args.optionLeaveType = LeaveType.Submenu; return true; }, delegate { GameMenu.SwitchToMenu(ToGameMuneId); }, isLeave, index, isRepeatable);
        }
    }
}
