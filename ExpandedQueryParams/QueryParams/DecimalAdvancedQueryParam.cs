using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpandedQueryParams.QueryParams
{
    public class DecimalAdvancedQueryParam : AdvancedQueryParam
    {
        public DecimalAdvancedQueryParam(string Name)
        {
            base.Name = Name;
        }

        public decimal? LT { get; set; }
        public decimal? GT { get; set; }
        public decimal? LTE { get; set; }
        public decimal? GTE { get; set; }
        public decimal? NE { get; set; }
        public decimal? EQ { get; set; }

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
            decimal castedSubject = (decimal)subject;
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
