using System;
using System.Collections.Generic;
using System.Text;

namespace Niem.MyNiem
{
    public class ConditionCriteria : SimpleCriteria
    {
        protected string _fieldType;
        protected string _value;
        protected Dictionary<string, string> _valueAttributes;

        private ConditionCriteria()
        {
        }

        internal ConditionCriteria(string fieldName, string fieldType, string value, CriteriaType criteriaType)
            : base(fieldName, criteriaType)
        {
            _fieldType = fieldType;
            _value = value;
            _valueAttributes = new Dictionary<string, string>();
        }

        public string FieldType
        {
            get { return _fieldType; }
        }

        public override string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        protected internal override string GetCAMLInternal()
        {
            string str = @"<{0}>
                               <FieldRef Name='{1}' {4}/>
                               <Value Type='{2}' {5}>{3}</Value>
                          </{0}>";

            return string.Format(str, GetCriteriaSymbol(), _fieldName, _fieldType, _value, GetAttributes(_fieldRefAttributes), GetAttributes(_valueAttributes));
        }

        public override Criteria AddValueAttribute(string name, string value)
        {
            _valueAttributes.Add(name, value);

            return this;
        }
    }
}
