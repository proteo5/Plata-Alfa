using System;
using System.Collections.Generic;
using System.Text;

namespace PlataAlfa.DB.MongoDB
{
    public static class DynamicHelper
    {
        public static bool IsSet(dynamic obj, string field)
        {
            var d = obj as IDictionary<string, object>;

            return d.ContainsKey(field);
        }
    }

}
