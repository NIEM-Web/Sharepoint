using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Niem.MyNiem
{
    public class CamlQueryElements
    {
        public string LogicalJoin { get; set; } // like <Or>, <And>
        public string ComparisonOperators { get; set; } // like <Eq>, <Contains>
        public string FieldName { get; set; } // Like Title
        public string FieldType { get; set; } // Like Text
        public string FieldValue { get; set; } // some value
    }

    public static class CamlQuery
    {
        // This function loop List of camlqueryelments which has our filter criteria
        // Then generate query in required format.
        // At end it return string which holds caml query.
        public static string GenerateQuery(IList<CamlQueryElements> queryElements)
        {
            StringBuilder queryJoin = new StringBuilder();
            string query = @"<{0}><FieldRef Name='{1}' /><Value {2} Type='{3}'>{4}</Value></{5}>";
            if (queryElements.Count > 0)
            {
                int itemCount = 0;
                foreach (CamlQueryElements element in queryElements)
                {
                    itemCount++;
                    string date = string.Empty;
                    // Display only Date
                    if (String.Compare(element.FieldType, "DateTime", true) == 0)
                        date = "IncludeTimeValue='false'";
                    queryJoin.AppendFormat
                   (string.Format(query, element.ComparisonOperators, element.FieldName,
                       date, element.FieldType, element.FieldValue, element.ComparisonOperators));

                    if (itemCount >= 2)
                    {
                        queryJoin.Insert(0, string.Format("<{0}>", element.LogicalJoin));
                        queryJoin.Append(string.Format("</{0}>", element.LogicalJoin));
                    }
                }
                queryJoin.Insert(0, "<Where>");
                queryJoin.Append("</Where>");
            }
            return queryJoin.ToString();
        }
    }
}
