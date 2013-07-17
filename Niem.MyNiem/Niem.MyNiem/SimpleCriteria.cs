using System;
using System.Collections.Generic;
using System.Text;

namespace Niem.MyNiem
{
    public class SimpleCriteria : Criteria
    {
        protected SimpleCriteria()
        {
        }

        internal SimpleCriteria(string fieldName, CriteriaType criteriaType)
            : base(fieldName, criteriaType)
        {
        }

        public override string Value
        {
            get
            {
                throw new NotSupportedException("SimpleCriteria does not have a Value element.");
            }
            set
            {
                throw new NotSupportedException("SimpleCriteria does not have a Value element.");
            }
        } 

        protected internal override string GetCAMLInternal()
        {
            string str = @"<{0}>
                               <FieldRef Name='{1}' {2}/>
                          </{0}>";

            return string.Format(str, GetCriteriaSymbol(), _fieldName, GetAttributes(_fieldRefAttributes));
        }

        public override Criteria AddValueAttribute(string name, string value)
        {
            throw new NotSupportedException("Simple criteria does not have Value element.");
        }

        public override Criteria AddFieldRefAttribute(string name, string value)
        {
            _fieldRefAttributes.Add(name, value);

            return this;
        }
    }
}
