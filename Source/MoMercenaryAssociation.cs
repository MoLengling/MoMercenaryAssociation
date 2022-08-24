using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace MoMercenaryAssociation
{
    public partial class MoMercenaryAssociation : MBSubModuleBase
    {
        private MoGameMenuHelper moGameMenuHelper;
        //这个mod的程序集中不应该储存任何可以被翻译的文本，全部通过读取文件的方式进行加载。

        //当战役开始后，
        public override void OnCampaignStart(Game game, object starterObject)
        {
            base.OnCampaignStart(game, starterObject);
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            return;
            //重设menuhelper的gamestarter
#pragma warning disable CS0162 // 检测到无法访问的代码
            MoGameMenuHelper.Get().Reset();

            Campaign val = game.GameType as Campaign;
            if (val != null)
            {
                CampaignGameStarter GameStarter = (CampaignGameStarter)gameStarterObject;
                MoGameMenuHelper.Get(new MoGameMenuHelper.MoGameMenuHelperParam(GameStarter));
            }
#pragma warning restore CS0162 // 检测到无法访问的代码
        }

        //protected override void OnSubModuleLoad()
        //{
        //    base.OnSubModuleLoad();

        //}

        private void AfterReadSettings(XmlNode RootNode, bool Succeed)
        {
            if (Succeed)
            {
                //读取setting，通知给mosetting完成初始化读取
                MoSettings.Get(new MoSettingsParam(RootNode));
                MoStrings.StartInit();
                TextObject TranslatorName = new TextObject(MoStrings.Translator);
                TextObject debugtext = new TextObject(MoStrings.OnModuleLoaded);
                debugtext.SetTextVariable("Author", MoSettings.Author);
                debugtext.SetTextVariable("Translator", TranslatorName);
                MoLogs.Get().DebugAndLog(debugtext.ToString());
            }
            else
            {
                MoLogs.Get().DebugAndLog("MMA Load Failed, Check if the file" + MoSettings.MMASettingPath + "is Exists");
            }
        }


        public override void OnInitialState()
        {
            base.OnInitialState();
            if (MoXmlReader.IsSingletionVaild())
                return;
            //读取设置文件，获取全部的字符串
            MoXmlReader.Get(new XmlReaderParam(MoSettings.MMASettingPath, "MMASettings",(XmlReaderParam.AfterCreateReader) AfterReadSettings));
            MoXmlReader.Get().StartReadXml();
        }
        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);
            //通过官方API拿到全部的Mod，并在其中寻找MMAConfig文件
            if (MoCustomOfficeManager.IsSingletionVaild())
                return;
            string[] ModuleNames = Campaign.Current.SandBoxManager.ModuleManager.ModuleNames;
            List<string> CustomMMAConfigPath = new List<string>();
            MoLogs.Get().DebugAndLog("MMA Start Reading ConfigFile");
            foreach (string moduleName in ModuleNames)
            {
                string Path = MoSettings.ModuleFolderPath + moduleName+ '/';
                if (MoFileSystemManager.Get().Exists(Path))
                {//获取所有已经启用的mod下的MMAconfig
                    string[] ConfigPath = Directory.GetFiles(Path, MoSettings.MMAConfigFileName, SearchOption.AllDirectories);
                    //交给
                    CustomMMAConfigPath.AddRange(ConfigPath); 
                }
            } 
            //游戏菜单管理器开始加载自定义菜单
            MoGameMenuManager.Get().InitCustomGameMenus(CustomMMAConfigPath);
        }
        public override void OnAfterGameInitializationFinished(Game game, object starterObject)
        {
            //游戏初始化完成后，开始创建菜单
            base.OnAfterGameInitializationFinished(game, starterObject);
            MoGameMenuHelper.Get().Reset();
            CampaignGameStarter GameStarter = (CampaignGameStarter)starterObject;
            if (GameStarter != null)
            {
                MoLogs.Get().DebugAndLog("MMA GameMenuHelper Start");
                moGameMenuHelper = MoGameMenuHelper.Get(new MoGameMenuHelper.MoGameMenuHelperParam(GameStarter));
            }
            MoLogs.Get().DebugAndLog("MMA Start CreateGameMenu");
            MoGameMenuManager.Get().StartCreateMenu();
        }
    }
}
