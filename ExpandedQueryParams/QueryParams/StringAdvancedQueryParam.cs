using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpandedQueryParams.QueryParams
{
    public class StringAdvancedQueryParam : AdvancedQueryParam
    {
        public StringAdvancedQueryParam(string Name)
        {
            base.Name = Name;
        }

        public string? LIKE { get; set; }
        public string? NOT { get; set; }
        public string? IS { get; set; }
        

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
            string castedSubject = (string)subject;
            return
                (LIKE == null || castedSubject.Contains(LIKE)) &&
                (NOT == null || !castedSubject.Equals(NOT)) &&
                (IS == null || castedSubject.Equals(IS));
        }
    }
}
