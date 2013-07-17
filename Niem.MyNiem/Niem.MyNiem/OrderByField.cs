using System;
using System.Collections.Generic;
using System.Text;

namespace Niem.MyNiem
{
    internal class OrderByField
    {
        private string _fieldName;
        private bool _ascending;

        public OrderByField(string fieldName, bool ascending)
        {
            _fieldName = fieldName;
            _ascending = ascending;
        }

        public override string ToString()
        {
            if (_ascending)
                return string.Format("<FieldRef Name='{0}' />", _fieldName);
            else
                return string.Format("<FieldRef Name='{0}' Ascending='False' />", _fieldName);
        }
    }
}
