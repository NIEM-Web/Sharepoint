using System;
using System.Collections.Generic;
using System.Text;

namespace Niem.MyNiem
{

    public abstract class Expression
    {
        private List<OrderByField> _orderByFields = null;
        private List<string> _groupByFields = null;

        internal Expression()
        {
            _orderByFields = new List<OrderByField>();
            _groupByFields = new List<string>();
        }

        public string GetCAMLQuery()
        {
            //perform rendering caml
            string camlWhere = GetCAMLInternal();
            //perform rendering orderby
            string camlOrderBy = GetOrderBy();
            //perform rendering groupby
            string camlGroupBy = GetGroupBy();

            return string.Format("<Query><Where>{0}</Where>{1}{2}</Query>", camlWhere, camlOrderBy, camlGroupBy);
        }

        public string GetCAML()
        {
            return GetCAMLInternal();
        }

        protected internal abstract string GetCAMLInternal();

        public Expression OrderBy(string fieldName, bool ascending)
        {
            _orderByFields.Add(new OrderByField(fieldName, ascending));

            return this;
        }

        public Expression GroupBy(string fieldName)
        {
            _groupByFields.Add(fieldName);

            return this;
        }

        private string GetOrderBy()
        {
            if (_orderByFields.Count == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append("<OrderBy>");
            foreach (OrderByField of in _orderByFields)
            {
                sb.Append(of.ToString());
            }
            sb.Append("</OrderBy>");

            return sb.ToString();
        }

        private string GetGroupBy()
        {
            if (_groupByFields.Count == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append("<GroupBy>");
            foreach (string gf in _groupByFields)
            {
                sb.AppendFormat("<FieldRef Name='{0}' />", gf);
            }
            sb.Append("</GroupBy>");

            return sb.ToString();
        }
        
        //
        public static Operator operator +(Expression exp1, Expression exp2)
        {
            return Operator.Or(exp1, exp2);
        }

        public static Operator operator *(Expression exp1, Expression exp2)
        {
            return Operator.And(exp1, exp2);
        }
    }
}
