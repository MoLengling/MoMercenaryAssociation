using System;
using System.Collections.Generic;
using System.Text;

namespace MoMercenaryAssociation
{
    public class MoManagerBase<ManagerName,ManagementName> : MoSingletonBase<ManagerName>
        where ManagerName: MoManagerBase<ManagerName,ManagementName>,new()
        where ManagementName:MoDataBase,new()
    {       
        protected Dictionary<string, ManagementName> Managements;
        public MoManagerBase():base() 
        {
            Managements = new Dictionary<string, ManagementName>();
        }

        virtual public bool TryFindManagement(string id,out ManagementName Management)
        {
            if (Managements.ContainsKey(id))
            {
                Management = Managements[id];
                return true;
            }
            Management = default(ManagementName);
            return false;
        }

        virtual public ManagementName[] GetManagements()
        {
            ManagementName[] OutManagements;

                OutManagements = new ManagementName[Managements.Count];
                int i = 0;
                foreach (ManagementName management in Managements.Values)
                {
                    OutManagements[i] = management;
                    i++;
                }

            return OutManagements;
        }

        //添加受管理对象，添加后对象列表中存在该对象返回true
        virtual public bool AddManagement(string id, ManagementName management)
        {
            if(Managements.ContainsKey(id))
            {
                if(!Managements[id].Equals(management))
                {
                    Managements[id] = management;
                }
            }
            else
            {
                Managements.Add(id, management);
            }
            return true;
        }

        virtual public bool AddManagement(ManagementName management)
        {
            return AddManagement(management.StringId, management);
        }

        virtual public void RemoveManagement(string id)
        {
            Managements.Remove(id);
        }
    }
}
