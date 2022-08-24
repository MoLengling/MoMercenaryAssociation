using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MoMercenaryAssociation
{
    public class MoReflection
    {
        public static bool SetPropertyValue<T>(string PropertyName,T Value,object obj)
        {
            try
            {
                Type Ts = obj.GetType();
                Ts.GetProperty(PropertyName).SetValue(obj, (object)Value, null);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool SetFieldValue(string FieldName,object Value,object obj)
        {
            try
            {
                Type Ts = obj.GetType();
                Ts.GetField(FieldName,BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public).SetValue(obj, Value);
                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}
