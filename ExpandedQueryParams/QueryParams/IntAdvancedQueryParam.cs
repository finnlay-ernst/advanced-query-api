using ExpandedQueryParams.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;

namespace ExpandedQueryParams.QueryParams
{        
    public class IntAdvancedQueryParam : AdvancedQueryParam
    {
        public IntAdvancedQueryParam(string Name)
        {
            base.Name = Name;
        }

        public int? LT { get; set; }
        public int? GT { get; set; }
        public int? LTE { get; set; }
        public int? GTE { get; set; }
        public int? NE { get; set; }
        public int? EQ { get; set; }

        // Index to allow property assignment using string property name (similar to JavaScript)
        // https://stackoverflow.com/questions/10283206/setting-getting-the-class-properties-by-string-name
        public override object this[string propertyName]
        {
            set
            {                
                Type classType = GetType();
                PropertyInfo propertyInfo = classType.GetProperty(propertyName);
                
                if (propertyInfo == null)
                {
                    throw new ArgumentException($"Property \"{propertyName}\" not defined for type {classType}");
                }
                else 
                {                 
                    propertyInfo.SetValue(this, value, null);
                }
            }
        }

        public override bool Passes(object subject)
        {
            int castedSubject = (int)subject;
            return
                (LT == null || castedSubject < LT) &&
                (GT == null || castedSubject > GT) &&
                (LTE == null || castedSubject <= LTE) &&
                (GTE == null || castedSubject >= GTE) &&
                (NE == null || castedSubject != NE) &&
                (EQ == null || castedSubject == EQ);
        }
    }
}
