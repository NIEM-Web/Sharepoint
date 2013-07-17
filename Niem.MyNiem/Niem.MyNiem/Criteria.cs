using System;
using System.Collections.Generic;
using System.Text;

namespace Niem.MyNiem
{

    public abstract class Criteria : Expression
    {
        protected string _fieldName;
        protected CriteriaType _criteriaType;
        protected Dictionary<string, string> _fieldRefAttributes;

        protected Criteria()
        {
            _fieldRefAttributes = new Dictionary<string, string>();
        }

        protected internal Criteria(string fieldName, CriteriaType criteriaType) : this()
        {
            _fieldName = fieldName;
            _criteriaType = criteriaType;
        }

        public string FieldName
        {
            get { return _fieldName; }
        }

        public CriteriaType CriteriaType
        {
            get { return _criteriaType; }
        }

        public abstract string Value
        {
            get;
            set;
        }

        public abstract Criteria AddFieldRefAttribute(string name, string value);
        public abstract Criteria AddValueAttribute(string name, string value);

        protected string GetAttributes(Dictionary<string, string> attributes)
        {
            if (attributes.Count == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> item in attributes)
                sb.AppendFormat("{0}='{1}' ", item.Key, item.Value);

            return sb.ToString();
        }

        protected string GetCriteriaSymbol()
        {
            switch (_criteriaType)
            {
                case CriteriaType.Eq:
                    return "Eq";
                case CriteriaType.Neq:
                    return "Neq";
                case CriteriaType.Gt:
                    return "Gt";
                case CriteriaType.Geq:
                    return "Geq";
                case CriteriaType.Lt:
                    return "Lt";
                case CriteriaType.Leq:
                    return "Leq";
                case CriteriaType.IsNull:
                    return "IsNull";
                case CriteriaType.IsNotNull:
                    return "IsNotNull";
                case CriteriaType.BeginsWith:
                    return "BeginsWith";
                case CriteriaType.Contains:
                    return "Contains";
                case CriteriaType.DateRangesOverlap:
                    return "DateRangesOverlap";
                default:
                    throw new Exception("Unhandled CriteriaType: " + _criteriaType.ToString());
            }
        }

        //static builders
        public static Criteria Eq(string fieldName, string fieldType, string value)
        {
            return new ConditionCriteria(fieldName, fieldType, value, CriteriaType.Eq);
        }

        public static Criteria Neq(string fieldName, string fieldType, string value)
        {
            return new ConditionCriteria(fieldName, fieldType, value, CriteriaType.Neq);
        }

        public static Criteria Gt(string fieldName, string fieldType, string value)
        {
            return new ConditionCriteria(fieldName, fieldType, value, CriteriaType.Gt);
        }

        public static Criteria Geq(string fieldName, string fieldType, string value)
        {
            return new ConditionCriteria(fieldName, fieldType, value, CriteriaType.Geq);
        }

        public static Criteria Lt(string fieldName, string fieldType, string value)
        {
            return new ConditionCriteria(fieldName, fieldType, value, CriteriaType.Lt);
        }

        public static Criteria Leq(string fieldName, string fieldType, string value)
        {
            return new ConditionCriteria(fieldName, fieldType, value, CriteriaType.Leq);
        }

        public static Criteria IsNull(string fieldName)
        {
            return new SimpleCriteria(fieldName, CriteriaType.IsNull);
        }

        public static Criteria IsNotNull(string fieldName)
        {
            return new SimpleCriteria(fieldName, CriteriaType.IsNotNull);
        }

        public static Criteria BeginsWith(string fieldName, string fieldType, string value)
        {
            return new ConditionCriteria(fieldName, fieldType, value, CriteriaType.BeginsWith);
        }

        public static Criteria Contains(string fieldName, string fieldType, string value)
        {
            return new ConditionCriteria(fieldName, fieldType, value, CriteriaType.Contains);
        }

        public static Criteria DateRangesOverlap(string fieldName, string fieldType, string value)
        {
            return new ConditionCriteria(fieldName, fieldType, value, CriteriaType.DateRangesOverlap);
        }
        

        //criteria operators
        public static Criteria operator !(Criteria exp)
        {
            if (exp is ConditionCriteria)
            {
                ConditionCriteria cexp = (ConditionCriteria)exp;

                if (exp._criteriaType == CriteriaType.Eq)
                    return new ConditionCriteria(cexp.FieldName, cexp.FieldType, cexp.Value, CriteriaType.Neq);
                else if (exp._criteriaType == CriteriaType.Neq)
                    return new ConditionCriteria(cexp.FieldName, cexp.FieldType, cexp.Value, CriteriaType.Eq);
            }
            else if (exp is SimpleCriteria)
            {
                SimpleCriteria cexp = (SimpleCriteria)exp;

                if (exp._criteriaType == CriteriaType.IsNull)
                    return new SimpleCriteria(cexp.FieldName, CriteriaType.IsNotNull);
                else if (exp._criteriaType == CriteriaType.IsNotNull)
                    return new SimpleCriteria(cexp.FieldName, CriteriaType.IsNull);
            }

            throw new Exception("Only Eq, Neq, IsNull, IsNotNull criterias can be negated.");
        }
    }
       
}
