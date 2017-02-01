using System;
using System.Collections.Generic;

namespace ParkInspect.Model.Factory
{
    public static class FilterFactory
    {
        private const char Seperator = '.';
        public static Boolean Like(this object target, Dictionary<string, object> filters)
        {
            Boolean isLike = true;
            foreach (var propertyName in filters.Keys)
            {
                var properties = propertyName.Split(Seperator);
                var obj = target;

                for(int i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];

                    //Should not happen, unless wrong key is given
                    if (obj.GetType().GetProperty(property) == null)
                        continue;

                    if (obj.GetType().GetProperty(property).GetGetMethod() != null && 
                        propertyName.Contains(Seperator.ToString()) && 
                        i != properties.Length - 1)
                    {
                        obj = obj.GetType().GetProperty(property).GetValue(obj);
                    }
                    else
                    {
                        var realValue = obj.GetType().GetProperty(property).GetValue(obj);
                        var value = GetValue(realValue).ToLower();
                        var filterValue = GetValue(filters[propertyName]).ToLower();

                        if (!value.Contains(filterValue))
                        {
                            isLike = false;
                            break;
                        }
                    }
                }
            }
            return isLike;
        }

        private static string GetValue(object obj)
        {

            if (obj is DateTime)
                return ((DateTime)obj).ToString("MM/dd/yyyy H:mm:ss tt").Replace('-', '/');

            return Convert.ToString(obj);

        }

    }
}
