using System;
using System.Collections.Generic;
using System.Text;

namespace MoMercenaryAssociation
{
    //如果一个单例注册时需要参数，应该继承这个类，并重写Init方法。
    public class MoSingletonParam
    {

    }
    public class MoSingletonBase<ClassName> where ClassName:MoSingletonBase<ClassName>,new()
    {
        protected static ClassName SingletionObject;
        private static object Lock = new object();
        protected MoSingletonBase() { }
        virtual protected void Init(MoSingletonParam Param)
        {

        }
        public static ClassName Get(MoSingletonParam Param)
        {
            if(SingletionObject == null || !SingletionObject.IsReady())
            {
                lock(Lock)
                {
                    SingletionObject = new ClassName();
                    SingletionObject.Init(Param);
                }
            }
            return SingletionObject;
        }
        public static ClassName Get()
        {
            if (SingletionObject == null)
            {
                lock (Lock)
                {
                    SingletionObject = new ClassName();
                }
            }
            return SingletionObject;
        }
        //检查单例是否被合法初始化
        public virtual bool IsReady()
        {
            return true;
        }
        //获取一个非单例对象，这不会覆盖之前的单例。
        public static ClassName GetNotSingletion()
        {
            return new ClassName();
        }
        //获取一个非单例对象并注册
        public static ClassName GetNotSingletion(MoSingletonParam Param)
        {
            ClassName NewObject = new ClassName();
            NewObject.Init(Param);
            return NewObject;
        }

        public static bool IsSingletionVaild()
        {
            return SingletionObject != null;
        }
    }
}
