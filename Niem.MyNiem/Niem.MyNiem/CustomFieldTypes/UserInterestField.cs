using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Niem.MyNiem.FieldValidationRules;
using System.Windows.Controls;
using System.Globalization;

namespace Niem.MyNiem.CustomFieldTypes
{
    public class UserInterestField : SPFieldNumber
    {
        #region Constructors
        public UserInterestField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
            InitializeProperties();
        }

        public UserInterestField(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
            InitializeProperties();
        }

        private void InitializeProperties()
        {
            this.MinimumValue = 0;
            this.DefaultValue = "0";
            this.ShowInNewForm = false;
        }
        
        #endregion

        #region Overriden Members
        public override string DefaultValue
        {
            get
            {
                return base.DefaultValue;
            }
            set
            {
                base.DefaultValue = value;
            }
        }

        public override object DefaultValueTyped
        {
            get
            {
                return base.DefaultValueTyped;
            }
        }

        public override Microsoft.SharePoint.WebControls.BaseFieldControl FieldRenderingControl
        {
            get
            {
                BaseFieldControl fieldControl = new UserInterestFieldControl();
                fieldControl.FieldName = this.InternalName;
                return fieldControl;
            }
        }

        public override Type FieldValueType
        {
            get
            {
                return base.FieldValueType;
            }
        }

        public override bool Filterable
        {
            get
            {
                return base.Filterable;
            }
        }

        public override bool FilterableNoRecurrence
        {
            get
            {
                return base.FilterableNoRecurrence;
            }
        }

        public override object GetFieldValue(string value)
        {
            return base.GetFieldValue(value);
        }

        public override string GetFieldValueAsHtml(object value)
        {
            return base.GetFieldValueAsHtml(value);
        }

        public override string GetFieldValueAsText(object value)
        {
            return base.GetFieldValueAsText(value);
        }

        public override string GetFieldValueForEdit(object value)
        {
            return base.GetFieldValueForEdit(value);
        }

        public override string GetProperty(string propertyName)
        {
            return base.GetProperty(propertyName);
        }

        public override string GetValidatedString(object value)
        {
            if ((this.Required == true) && (value == null))
            {
                throw new SPFieldValidationException(this.Title + " must have a value.");
            }
            else
            {
                UserInterestFieldValidationRule rule
                   = new UserInterestFieldValidationRule();
                ValidationResult result
                   = rule.Validate(value, CultureInfo.InvariantCulture);
                if (!result.IsValid)
                {
                    throw new
                          SPFieldValidationException((String)result.ErrorContent);
                }
                else
                {
                    return base.GetValidatedString(value);
                }
            }
        }

        protected override bool HasValue(object value)
        {
            return base.HasValue(value);
        }

        public override void ParseAndSetValue(SPListItem item, string value)
        {
            base.ParseAndSetValue(item, value);
        }

        public override bool Sortable
        {
            get
            {
                return base.Sortable;
            }
        }

        public override string TypeDisplayName
        {
            get
            {
                return base.TypeDisplayName;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void Update()
        {
            base.Update();
        }

        #endregion
    }

}
