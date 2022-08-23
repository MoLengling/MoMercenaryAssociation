using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using TaleWorlds.Library;
using MoMercenaryAssociation.Exceptions;
using TaleWorlds.Core;

namespace MoMercenaryAssociation
{
	internal class MoSettingsParam:MoSingletonParam
    {
		public XmlNode Root;

        public MoSettingsParam(XmlNode root):base()
        {
            Root = root;
        }
    }

    class MoSettings : MoSingletonBase<MoSettings>
    {
		public static readonly string Name = "MoLengLing's Mercenary Association Reborn";
		public static readonly string MMAConfigFileName = "*MMAConfig.xml";
		public static readonly string ModuleFolderPath = BasePath.Name + "Modules/";
		public static readonly string MMAFolderPath = BasePath.Name + "Modules/MoMercenaryAssociation/";
		public static readonly string LocalDataPath = BasePath.Name + "Modules/MoMercenaryAssociation/ModuleData/";
		public static readonly string MMASettingPath = BasePath.Name + "Modules/MoMercenaryAssociation/ModuleData/MMASetting.xml";
		public static readonly string MMASubModulePath = BasePath.Name + "Modules/MoMercenaryAssociation/SubModule.xml";
		public static readonly string MMAOutPutPath = BasePath.Name + "Modules/MoMercenaryAssociation/ModuleData/Output/";
		public static readonly string MMALogFilePath = BasePath.Name + "Modules/MoMercenaryAssociation/ModuleData/Output/MMA.log";
		public static readonly string MMALogFilePathWithoutExtension = BasePath.Name + "Modules/MoMercenaryAssociation/ModuleData/Output/MMA";
		public static readonly string MMAStringsPath = BasePath.Name + "Modules/MoMercenaryAssociation/ModuleData/MMAString.xml";

		public static readonly string Author = "MoLengling";
		public static readonly string MMAURL = "https://www.nexusmods.com/mountandblade2bannerlord/mods/3310";

		private bool MoCheatMode;
		public string Version { get; private set; }
        public bool CheatMode {
			get
			{
				return Game.Current != null ? Game.Current.CheatMode || MoCheatMode : false;
			}
		}
		public float AIMinMilitiaNumber{ get; internal set; }

        override protected void Init(MoSingletonParam Param)
        {
			MoSettingsParam MyParam = (MoSettingsParam)Param;
			if(MyParam != null)
            {
				ReadSettings(MyParam.Root);
            }
			else
            {
				MoExceptionManager.Get().ThrowException(new
					MoSettingParamException("When MMA initialized the MoSettings module, an incorrect parameter was passed in.\n Expected parameter type: MoSettingsParam;\n Actual parameter type:" + Param.GetType()
					));
            }

        }

		private void ReadSettings(XmlNode RootNode)
		{
			foreach(XmlNode SettingNode in RootNode)
            {
                switch (SettingNode.Name)
                {
					case "CHEAT_MODE": bool.TryParse(SettingNode.InnerText,out MoCheatMode);break;
				}
            }
		}
	}
}
