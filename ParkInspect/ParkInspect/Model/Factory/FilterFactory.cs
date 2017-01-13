using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkInspect.Model.Factory
{
    public static class FilterFactory
    {
        public static Boolean Like(this object target, Dictionary<string, object> filters)
        {
            Boolean isLike = true;
            foreach (var propertyName in filters.Keys)
            {

                var properties = propertyName.Split('.');
                var obj = target;

                foreach (var property in properties)
                {
                    //Should not happen, unless wrong key is given
                    if (obj.GetType().GetProperty(property) == null)
                        continue;


                    if (obj.GetType().GetProperty(property).GetGetMethod() != null &&
                        obj.GetType().GetProperty(property).GetGetMethod().IsVirtual)
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
                return ((DateTime)obj).ToString("dd/MM/yyyy H:mm:ss tt").Replace('-', '/');

            return Convert.ToString(obj);

        }

    }
}
